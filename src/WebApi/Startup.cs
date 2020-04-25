using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Types;
using Memoyed.Application;
using Memoyed.WebApi.GraphQL;
using Memoyed.WebApi.GraphQL.InputTypes;
using Memoyed.WebApi.GraphQL.Types;
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
            services.AddCors();
            services.AddControllers();

            services.AddScoped<CardType>();
            services.AddScoped<CardBoxType>();
            services.AddScoped<CardBoxSetType>();
            services.AddScoped<SessionCardStatusType>();
            services.AddScoped<RevisionSessionStatusType>();
            services.AddScoped<RevisionSessionType>();
            services.AddScoped<SessionCardType>();
            services.AddScoped<SessionCardAnswerTypeType>();
            services.AddScoped<CardsQuery>();

            services.AddScoped<CardBoxSetInput>();
            services.AddScoped<CardBoxInput>();
            services.AddScoped<CardInput>();
            services.AddScoped<RemoveCardInput>();
            services.AddScoped<RenameCardBoxSetInput>();
            services.AddScoped<StartRevisionSessionInput>();
            services.AddScoped<SetCardAnswerInput>();
            services.AddScoped<CardsMutation>();

            services.AddScoped<ISchema, CardsSchema>();

            services.AddGraphQL(options =>
                {
                    options.ExposeExceptions = true;
                    options.EnableMetrics = true;
                })
                .AddSystemTextJson()
                .AddDataLoader();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

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