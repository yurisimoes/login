using System;
using Api.Data;
using Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Quartz;

namespace Api
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
            
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
                options.UseSimpleTypeLoader();
                options.UseInMemoryStore();
            });
            
            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            services.ConfigureOpenIddict();
            
            services.Scan(x =>
                x.FromAssemblyOf<IService>()
                    .AddClasses(c => c.AssignableTo<IService>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.AddControllers();
            services.AddDbContext<LoginDbContext>(context =>
            {
                context.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
                    .UseSnakeCaseNamingConvention();
                context.UseOpenIddict<Guid>();
            });      
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });
            
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}