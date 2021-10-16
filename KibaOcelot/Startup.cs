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
     Nuget����Ocelot����װ����Ҫ���ʹ��Consul������Ocelot.Provider.Consul��Ocelot.Provider.Polly�������ռ�using Ocelot.Provider.Consul;using Ocelot.Provider.Polly;��
     Ȼ�����������ռ�using Ocelot.DependencyInjection;using Ocelot.Middleware;
     ʹ��oceloat.josn����

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
                 .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true).Build());//�в�����ocelot����Ҫ����Ocelot.Provider.Consul��Ocelot.Provider.Polly�ģ���Ϊ�в������Ǻ�Consul��ѯ�����ϵ�
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline. ����async�������ڲ�����ʹ��await
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
