#region License
// Copyright 2008-2009 Jeremy Skinner (http://www.jeremyskinner.co.uk)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://www.codeplex.com/FluentValidation
#endregion

namespace FluentValidation.Tests {
	using System.Linq;
	using NUnit.Framework;

	[TestFixture]
	public class AssemblyScannerTester {

		[Test]
		public void Finds_validators_for_types() {
			var scanner = new AssemblyScanner(new[] { typeof(Model1Validator), typeof(Model2Validator) });
			var results = scanner.ToList();

			results[0].ValidatorType.ShouldEqual(typeof(Model1Validator));
			results[0].InterfaceType.ShouldEqual(typeof(IValidator<Model1>));

			results[1].ValidatorType.ShouldEqual(typeof(Model2Validator));
			results[1].InterfaceType.ShouldEqual(typeof(IValidator<Model2>));
		}


		public class Model1 {
			
		}

		public class Model2 {
			
		}

		public class Model1Validator:AbstractValidator<Model1> {
			
		}

		public class Model2Validator:AbstractValidator<Model2> {

		}
	}

}