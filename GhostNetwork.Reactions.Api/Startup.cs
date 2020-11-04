using System;
using GhostNetwork.Reactions.MongoDb;
using GhostNetwork.Reactions.Mssql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace GhostNetwork.Reactions.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Reactions", Version = "v1" });
            });

            if (Configuration.GetSection("MONGO_ADDRESS").Exists())
            {
                services.AddScoped(provider =>
                {
                    var client = new MongoClient($"mongodb://{Configuration["MONGO_ADDRESS"]}/greactions");
                    return new MongoDbContext(client.GetDatabase("greactions"));
                });

                services.AddScoped<IReactionStorage, MongoReactionStorage>();
            }
            else if (Configuration.GetSection("MSSQL_ADDRESS").Exists())
            {
                services.AddDbContext<MssqlContext>(options => options
                    .UseSqlServer(MssqlConnectionString(), b => b.MigrationsAssembly(typeof(MssqlContext).Assembly.FullName)));

                services.AddScoped<IReactionStorage, MssqlReactionStorage>();
            }
            else
            {
                throw new NullReferenceException("Database not configured");
            }

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (Configuration.GetSection("MSSQL_ADDRESS").Exists())
            {
                app.ApplicationServices.GetService<MssqlContext>();
            }

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

        private string MssqlConnectionString()
        {
            return $"Server={Configuration["MSSQL_ADDRESS"]},{Configuration["MSSQL_PORT"]};Database=Reaction;User={Configuration["MSSQL_USER"]};Password={Configuration["MSSQL_PASSWORD"]}";
        }
    }
}