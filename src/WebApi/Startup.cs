using HotChocolate;
using HotChocolate.AspNetCore;
using Memoyed.Application;
using Memoyed.WebApi.GraphQL;
using Memoyed.WebApi.GraphQL.ReturnTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                dbOptions.UseNpgsql(Configuration.GetConnectionString("Default"))
                    .UseSnakeCaseNamingConvention());
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddCors();
            services.AddControllers();

            services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<CardsQuery>()
                .AddMutationType<CardsMutation>()
                .Create());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.UseWebSockets();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseHttpsRedirection();
            }
            //
            // app.UseCors(opt =>
            // {
            //     opt.AllowAnyOrigin()
            //         .AllowAnyHeader()
            //         .AllowAnyMethod();
            // });

            app.UseGraphQL();
            app.UsePlayground();

            // app.UseRouting();

            // app.UseAuthorization();

            // app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }
}