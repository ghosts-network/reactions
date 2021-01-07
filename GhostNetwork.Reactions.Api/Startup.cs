using GhostNetwork.Reactions.Api.Helpers.OpenApi;
using GhostNetwork.Reactions.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Reactions",
                    Version = "1.0.0"
                });

                options.OperationFilter<OperationIdFilter>();

                options.IncludeXmlComments(XmlPathProvider.XmlPath);
            });

            services.AddScoped(provider =>
            {
                var client = new MongoClient($"mongodb://{Configuration["MONGO_ADDRESS"]}/greactions");
                return new MongoDbContext(client.GetDatabase("greactions"));
            });

            services.AddScoped<IReactionStorage, MongoReactionStorage>();

            services.AddControllers();
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