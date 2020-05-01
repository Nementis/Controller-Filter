using ControllerFilter.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ControllerFilter {

	public class Startup {

		#region Constructors

		public Startup( IConfiguration configuration ) {
			Configuration = configuration;
		}

		#endregion

		#region Public Properties

		public IConfiguration Configuration { get; }

		#endregion

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure( IApplicationBuilder app, IWebHostEnvironment env ) {

			if ( env.IsDevelopment() ) {
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints( endpoints => { endpoints.MapControllers(); } );
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices( IServiceCollection services ) {

			services.AddControllers( options => { options.Filters.Add<HeaderDataActionFilter>(); } );

		}

	}

}