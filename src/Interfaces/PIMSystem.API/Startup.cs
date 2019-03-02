using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetroBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PIMSystem.Core.Data;
using PIMSystem.Core.Domain.Entities;
using PIMSystem.Core.Service.Data;
using PIMSystem.Core.Service.Event;
using PIMSystem.Data;
using PIMSystem.Service.Data;
using PIMSystem.Service.Event;
using Swashbuckle.AspNetCore.Swagger;

namespace PIMSystem.API
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
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithMethods("GET", "POST")
                       .AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "PIMSystem.API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
            });

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUploadService, UploadService>();
            services.AddTransient<IUploadItemService, UploadItemService>();
            services.AddTransient<IMqService, RabbitMqService>();
            services.AddTransient<IRepository<Category>, Repository<Category>>();
            services.AddTransient<IRepository<Product>, Repository<Product>>();
            services.AddTransient<IRepository<Upload>, Repository<Upload>>();
            services.AddTransient<IRepository<UploadItem>, Repository<UploadItem>>();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DbConnection"));
            });

            string rabbitMqUri = Configuration.GetValue<string>("rabbitMqUri");
            string rabbitMqUserName = Configuration.GetValue<string>("rabbitMqUserName");
            string rabbitMqPassword = Configuration.GetValue<string>("rabbitMqPassword");
            services.AddSingleton(MetroBusInitializer.Instance.UseRabbitMq(rabbitMqUri, rabbitMqUserName, rabbitMqPassword).Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PIMSystem.API");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = "PIMSystem - API";
                c.EnableFilter();
                c.DefaultModelsExpandDepth(-1); // Hide models in Swagger UI
                c.DisplayRequestDuration();
            });

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
