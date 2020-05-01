using System;

namespace ControllerFilter.Attributes {

	[AttributeUsage( AttributeTargets.Property )]
	public class FilterPropertyAttribute : Attribute {

		#region Constructors

		public FilterPropertyAttribute( string headerName ) {
			HeaderName = headerName;
		}

		#endregion

		#region Public Properties

		public string HeaderName { get; }

		public bool Required { get; set; }

		#endregion

	}

}