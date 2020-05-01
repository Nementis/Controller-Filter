using ControllerFilter.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace ControllerFilter.Controllers.Base {

	[FilterController( HeaderPrefix = "Base" )]
	public class BaseControllerFilterController : ControllerBase {

		#region Public Properties

		[FilterProperty( "IntegerProperty", Required = true )]
		public int IntegerProperty { get; set; }

		[FilterProperty( "StringProperty", Required = false )]
		public string StringProperty { get; set; }

		#endregion

	}

}