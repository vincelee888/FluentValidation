﻿namespace FluentValidation.Validators {
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public interface IValidationWorker {
		/// <summary>
		/// Performs validation.
		/// </summary>
		/// <param name="context">Current validation context</param>
		void Validate(IValidationContext context);

		/// <summary>
		/// Performs validation asynchronously.
		/// </summary>
		/// <param name="context">Current validation context/</param>
		/// <param name="cancellationToken">Cancellation context</param>
		/// <returns></returns>
		Task ValidateAsync(IValidationContext context, CancellationToken cancellationToken);

		/// <summary>
		/// Determines whether validation should be run asynchronously.
		/// </summary>
		/// <param name="context">Current validation context</param>
		/// <returns>Bool indicating if validation should be run asynchronously.</returns>
		bool ShouldValidateAsync(IValidationContext context);
	}
	
	/// <summary>
	/// Base validator implementation 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ValidatorBase<T> {
		private readonly Action<T, IValidationContext> _action;
		private readonly Func<T, IValidationContext, CancellationToken, Task> _asyncAction;
		private readonly bool _isAsync;
		
		/// <summary>
		/// Creates a new instance of the CustomValidator
		/// </summary>
		/// <param name="action"></param>
		public ValidatorBase(Action<T, IValidationContext> action) {
			_isAsync = false;
			_action = action;
			_asyncAction = (x, ctx, cancel) => TaskHelpers.RunSynchronously(() =>_action(x, ctx), cancel);
		}

		/// <summary>
		/// Creates a new isntance of the CutomValidator.
		/// </summary>
		/// <param name="asyncAction"></param>
		public ValidatorBase(Func<T, IValidationContext, CancellationToken, Task> asyncAction) {
			_isAsync = true;
			_asyncAction = asyncAction;
			_action = (x, ctx) => Task.Run(() => _asyncAction(x, ctx, new CancellationToken())).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Performs validation.
		/// </summary>
		/// <param name="context">Current validation context</param>
		public virtual void Validate(IValidationContext context) {
			_action((T) context.Model, context);
		}

		/// <summary>
		/// Performs validation asynchronously.
		/// </summary>
		/// <param name="context">Current validation context/</param>
		/// <param name="cancellationToken">Cancellation context</param>
		/// <returns></returns>
		public virtual async Task ValidateAsync(IValidationContext context, CancellationToken cancellationToken) {
			await _asyncAction((T)context.Model, context, cancellationToken);
		}

		/// <summary>
		/// Determines whether validation should be run asynchronously.
		/// </summary>
		/// <param name="context">Current validation context</param>
		/// <returns>Bool indicating if validation should be run asynchronously.</returns>
		public bool ShouldValidateAsync(IValidationContext context) {
			return _isAsync && context.IsAsync;
		}
	}
}