using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Data;
using Elsa;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Activities;
using Elsa.Scripting.Liquid.Messages;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Handlers;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa
{
    public class Startup
    { 
         //test Push fgfgfg
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            var elsaSection = Configuration.GetSection("Elsa");

            // Elsa services.
           services
                .AddElsa(elsa => elsa
                    .UseEntityFrameworkPersistence(ef => ef.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))
                    .AddConsoleActivities()
                    .AddActivity<NotifictionInterval>()
                    .AddActivity<AddApproval>()     
                    .AddActivity<MainProjectPlanningUsers>()
                    .AddActivity<EngineeringOfPlanning>()
                    .AddActivity<SendRequestWorkflowUsers>()
                    .AddActivity<RejectToSectionsFlagsPlanning>()
                    .AddActivity<FyiForSectionsHeadPlanning>()
                    .AddActivity<updateSignalForManagerOfDevelopmentCoordinationPlanning>()
                    .AddActivity<FlagForCheckUpdateSignal>()
                    .AddHttpActivities(elsaSection.GetSection("Server").Bind)
                    .AddQuartzTemporalActivities()
                    .AddWorkflowsFrom<Startup>()
                   
                );

            // Elsa API endpoints.
            services.AddElsaApiEndpoints();
            services.Configure<ApiVersioningOptions>(options => options.UseApiBehavior = false);

            // Allow arbitrary client browser apps to access the API.
            // In a production environment, make sure to allow only origins you trust.
            services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .WithExposedHeaders("Content-Disposition"))
            );

            services.AddDbContext<PlanningDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<SsoDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddNotificationHandler<EvaluatingLiquidExpression, ConfigureLiquidEngine>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseHttpActivities();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
