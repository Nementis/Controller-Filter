using System;

namespace ControllerFilter.Attributes {

	[AttributeUsage( AttributeTargets.Class )]
	public class FilterControllerAttribute : Attribute {

		#region Public Properties

		public string HeaderPrefix { get; set; }

		#endregion

	}

}