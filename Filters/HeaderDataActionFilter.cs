using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ControllerFilter.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControllerFilter.Filters {

	public class HeaderDataActionFilter : IAsyncActionFilter {

		#region Private Members

		private const string AppHeaderPrefix = "CFX";

		#endregion

		#region Interface Implementations

		public async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next ) {
			FilterControllerAttribute[] controllerAttributes;

			try {
				controllerAttributes = context.Controller.GetType().GetCustomAttributes<FilterControllerAttribute>( true ).ToArray();
			}
			catch ( Exception ex ) {
				FormatErrorResult( context, "Controller Validation Error", "Error while retrieving controller metadata information", ex.Message );

				return;
			}

			//=== If the current controller is decorated with a Filter Controller attribute continue to read its metadata
			if ( controllerAttributes.Any() ) {
				var currentControllerAttribute = controllerAttributes.First();

				//=== header prefix, if present will be used later to build header name
				string headerPrefix = default;

				if ( !string.IsNullOrEmpty( currentControllerAttribute.HeaderPrefix ) ) {
					headerPrefix = currentControllerAttribute.HeaderPrefix;
				}

				PropertyInfo[] controllerPropertiesInfo;

				try {
					controllerPropertiesInfo = context.Controller.GetType().GetProperties();
				}
				catch ( Exception ex ) {
					FormatErrorResult( context, "Controller Properties Validation Error", "Error while retrieving controller properties metadata information", ex.Message );

					return;
				}

				//=== If the current controller has public properties enumerate them
				if ( controllerPropertiesInfo.Length > 0 ) {
					foreach ( var controllerPropertyInfo in controllerPropertiesInfo ) {
						FilterPropertyAttribute[] propertiesAttributes;

						try {
							propertiesAttributes = controllerPropertyInfo.GetCustomAttributes<FilterPropertyAttribute>( true ).ToArray();
						}
						catch ( Exception ex ) {
							FormatErrorResult( context, "Controller Properties Attributes Validation Error", "Error while retrieving controller properties attributes metadata information", ex.Message );

							return;
						}

						//=== If the current property is decorated with a Filter Property attribute continue to read its metadata
						if ( propertiesAttributes.Any() ) {
							var currentPropertyAttribute = propertiesAttributes.First();

							var headerName = currentPropertyAttribute.HeaderName;

							var completeHeaderName = $"{AppHeaderPrefix}{( string.IsNullOrEmpty( headerPrefix ) ? null : "-" + headerPrefix )}-{headerName}";
							var currentHeaders = context.HttpContext.Request.Headers[completeHeaderName];

							if ( currentHeaders.Count == 0 ) {
								if ( currentPropertyAttribute.Required ) {
									FormatErrorResult( context, "Request Headers Parsing Error", $"Required Header {completeHeaderName} not found in request headers" );
								}
							}
							else {
								try {
									var propertyTypeConverter = TypeDescriptor.GetConverter( controllerPropertyInfo.PropertyType );
									controllerPropertyInfo.SetValue( context.Controller, propertyTypeConverter.ConvertFromString( currentHeaders.First() ) );
								}
								catch ( Exception ex ) {
									FormatErrorResult( context, "Controller Properties Set Error", $"Error while setting {controllerPropertyInfo.Name} property value for controller {context.Controller.GetType().Name} from header {completeHeaderName}", ex.Message );
								}
							}
						}
					}
				}
			}

			if ( context.ModelState.IsValid) {
				await next();
			}
			else {
				context.Result = new BadRequestObjectResult(context.ModelState);
			}
		}

		#endregion

		private void FormatErrorResult( ActionExecutingContext context, string title, string message, string exceptionMessage = null ) {

			context.ModelState.AddModelError( title, $"{message}. {(exceptionMessage != null ? "Detailed exception message: " + exceptionMessage : "")}");
		}

	}

}