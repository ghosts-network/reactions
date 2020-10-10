using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GhostNetwork.Reactions.Domain;
using GhostNetwork.Reactions.MSsql;


namespace GhostNetwork.Reactions.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Reactions", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);
            });

            var connection = @"Server=DESKTOP-FQ83PKI;Database=Reactions;Trusted_Connection=True;";

            services.AddDbContext<MSsqlContext>(options =>
              options.UseSqlServer(
              connection, b => b.MigrationsAssembly("GhostNetwork.Reactions.MSsql")));

            services.AddScoped<IReactionStorage, MSsqlReactionStorage>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger()
                   .UseSwaggerUI(config =>
                   {
                       config.SwaggerEndpoint("/swagger/v1/swagger.json", "Relations.API V1");
                   });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}