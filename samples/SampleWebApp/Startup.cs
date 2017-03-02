using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleWebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build()
                .ReplacePlaceholders();
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ServiceOptions>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IOptions<ServiceOptions> serviceOptions)
        {
            var options = serviceOptions.Value;
            Debug.Assert(options.FooService.Endpoint == options.BarService.Endpoint);

            app.Run(async context =>
            {
                await context.Response.WriteAsync("FooService Endpoint: " + options.FooService.Endpoint + Environment.NewLine);
                await context.Response.WriteAsync("FooService Resource: " + options.FooService.Resource + Environment.NewLine);
                await context.Response.WriteAsync("BarService Endpoint: " + options.BarService.Endpoint + Environment.NewLine);
                await context.Response.WriteAsync("BazService Endpoint: " + options.BazService.Endpoint + Environment.NewLine);
            });
        }
    }

    public class ServiceOptions
    {
        public ServiceEndpoint FooService { get; set; }

        public ServiceEndpoint BarService { get; set; }

        public ServiceEndpoint BazService { get; set; }
    }

    public class ServiceEndpoint
    {
        public string Endpoint { get; set; }

        public string Resource { get; set; }
    }
}
