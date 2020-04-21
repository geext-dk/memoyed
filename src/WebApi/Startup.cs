using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Types;
using Memoyed.Application;
using Memoyed.WebApi.GraphQL;
using Memoyed.WebApi.GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Memoyed.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCardsApplicationServices(dbOptions =>
                dbOptions.UseSqlite(Configuration.GetConnectionString("Default")));
            services.AddCors();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Memoyed Api",
                    Version = "v1"
                });
            });
            
            services.AddScoped<CardType>();
            services.AddScoped<CardBoxType>();
            services.AddScoped<CardBoxSetType>();
            services.AddScoped<CardsQuery>();
            services.AddScoped<ISchema, CardsSchema>();

            services.AddGraphQL(options =>
            {
                options.ExposeExceptions = true;
                options.EnableMetrics = true;
            }).AddSystemTextJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Memoyed Api v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseCors(opt =>
            {
                opt.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            app.UseGraphQL<ISchema>();
            
            app.UseGraphiQLServer(new GraphiQLOptions
            {
                GraphQLEndPoint = "/graphql"
            });
        }
    }
}