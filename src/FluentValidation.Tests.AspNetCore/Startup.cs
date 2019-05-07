namespace FluentValidation.Tests.AspNetCore {
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using FluentValidation.AspNetCore;
	using FluentValidation.Attributes;
	using Microsoft.AspNetCore.Mvc.ModelBinding;
	using System.Globalization;
	using Microsoft.AspNetCore.Localization;
#if !NETCOREAPP3_0
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#endif 

	public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setup => {
                
            })
#if NETCOREAPP3_0
			.AddNewtonsoftJson()
#endif   
			.AddFluentValidation(cfg => {
	            cfg.ValidatorFactoryType = typeof(AttributedValidatorFactory);
	            cfg.ImplicitlyValidateChildProperties = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            app.UseRequestLocalization(options => {
                options.DefaultRequestCulture = new RequestCulture(cultureInfo);
                options.SupportedCultures = new []{ cultureInfo };
                options.SupportedUICultures = new []{ cultureInfo };
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}