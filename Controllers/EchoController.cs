using ControllerFilter.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ControllerFilter.Controllers {

	[ApiController]
	[Route( "api/[controller]" )]
	public class EchoController : BaseControllerFilterController {

		#region Private Members

		private readonly ILogger<EchoController> _logger;

		#endregion

		#region Constructors

		public EchoController( ILogger<EchoController> logger ) {

			_logger = logger;
		}

		#endregion

		[HttpGet]
		public ActionResult Echo() {
			return Ok( $"Received Integer: {IntegerProperty}, Received String: {StringProperty ?? "null"}" );
		}

	}

}