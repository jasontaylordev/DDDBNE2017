using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;
using System.Reflection;

using Northwind.Persistence;
using Northwind.Application.Customers.Queries.GetCustomerDetail;
using Northwind.Application.Customers.Queries.GetCustomerList;
using Northwind.Application.Customers.Commands.CreateCustomer;
using Northwind.Application.Customers.Commands.UpdateCustomer;
using Northwind.Application.Customers.Commands.DeleteCustomer;

namespace Northwind.Presentation
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
            services.AddDbContext<NorthwindContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("Northwind")));

            services.AddMvc();

            services.AddScoped<IGetCustomerDetailQuery, GetCustomerDetailQuery>();
            services.AddScoped<IGetCustomerListQuery, GetCustomerListQuery>();
            services.AddScoped<ICreateCustomerCommand, CreateCustomerCommand>();
            services.AddScoped<IUpdateCustomerCommand, UpdateCustomerCommand>();
            services.AddScoped<IDeleteCustomerCommand, DeleteCustomerCommand>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, NorthwindContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, new SwaggerUiSettings
            {
                DefaultUrlTemplate = "{controller}/{action}/{id?}"
            });

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // routes.MapSpaFallbackRoute(
                //     name: "spa-fallback",
                //     defaults: new { controller = "Home", action = "Index" });
            });

            app.MapWhen(a => !a.Request.Path.Value.StartsWith("/swagger"), builder =>
                builder.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Home", action = "Index" });
                })
            );

            NorthwindInitializer.Initialize(context);
        }
    }
}
