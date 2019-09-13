using System;
using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Configuration;
using Nop.Web.Framework.Infrastructure.Extensions;

namespace Nop.Web
{
    /// <summary>
    /// Represents startup class of application
    /// </summary>
    public class Startup
    {
        #region Fields

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private NopConfig _nopConfig;

        #endregion

        #region Ctor

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        #endregion

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var s = services.ConfigureApplicationServices(_configuration, _hostingEnvironment);
            _nopConfig = s.GetService<NopConfig>();
            if (_nopConfig.UseResponseCompression)
            {
                //https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-2.2
                services.AddResponseCompression();
                services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = (CompressionLevel)_nopConfig.ResponseCompressionMode;
                });
            }


            return s;
        }

        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {           
            if (_nopConfig.UseResponseCompression)
            {
                //https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-2.2
                application.UseResponseCompression();
            }
            application.ConfigureRequestPipeline();
        }
    }
}