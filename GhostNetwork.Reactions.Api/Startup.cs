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

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Reactions", Version = "v1" });
            });

            services.AddDbContext<MssqlContext>(options => options
                .UseSqlServer(MssqlConnectionString(), b => b.MigrationsAssembly(typeof(MssqlContext).Assembly.FullName)));

            services.AddScoped<IReactionStorage, MssqlReactionStorage>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MssqlContext context)
        {
            context.Database.Migrate();

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