using GhostNetwork.Reactions.Mssql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Reactions", Version = "v1" });
            });

            services.AddDbContext<MssqlContext>(options => options
                .UseSqlServer(Configuration["MSSQL_ADDRESS"], b => b.MigrationsAssembly("GhostNetwork.Reactions.Mssql")));

            services.AddScoped<IReactionStorage, MssqlReactionStorage>();

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