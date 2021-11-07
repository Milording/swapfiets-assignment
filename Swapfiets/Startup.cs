using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Swapfiets.Services;

namespace Swapfiets
{
    [ExcludeFromCodeCoverage(Justification = "Infrastructure")]
    public class Startup
    {
        private IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient("bikeindex",
                                   client =>
                                   {
                                       client.BaseAddress = new Uri("https://bikeindex.org/");
                                   })
                    .SetHandlerLifetime(TimeSpan.FromMinutes(1))
                    .AddPolicyHandler(GetRetryPolicy());
            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swapfiets", Version = "v1" });
                                   });
            services.AddAuthentication(
                        CertificateAuthenticationDefaults.AuthenticationScheme)
                    .AddCertificate(option => option.AllowedCertificateTypes = CertificateTypes.All)
                    .AddCertificateCache();

            // Services initialization
            services.AddScoped<IBikeService, BikeService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swapfiets v1"));
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapControllers();
                             });
        }


        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                   .HandleTransientHttpError()
                   .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}