using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace KibaOcelot
{
    /*
     Nuget搜索Ocelot，安装（如要配合使用Consul，下载Ocelot.Provider.Consul和Ocelot.Provider.Polly，命名空间using Ocelot.Provider.Consul;using Ocelot.Provider.Polly;）
     然后引用命名空间using Ocelot.DependencyInjection;using Ocelot.Middleware;
     使用oceloat.josn配置

     */
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        //{
        //    var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
        //    builder.SetBasePath(environment.ContentRootPath)
        //           .AddJsonFile("appsettings.json", false, reloadOnChange: true)
        //           .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
        //           .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
        //           .AddEnvironmentVariables();
        //    Configuration = builder.Build(); 
        //}   
       

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot(new ConfigurationBuilder()
                 .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true).Build());//有参数的ocelot是需要引用Ocelot.Provider.Consul和Ocelot.Provider.Polly的，因为有参数的是和Consul查询服务结合的
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 加上async，这样内部可以使用await
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            await app.UseOcelot();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
