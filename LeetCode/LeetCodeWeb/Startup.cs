using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LeetCodeWeb
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        //此方法由运行时调用。使用此方法可将服务添加到容器中。
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //
        /// <summary>
        /// 服务配置
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c=> {
                c.SwaggerDoc("v1",new OpenApiInfo { 
                    Version="v1", //版本
                    Title="ApiDoc", //标题
                    Description="swagger API文档",
                    Contact = new OpenApiContact { Name = "kizuna_ai", Email = "949210784@qq.com", Url = new Uri("http://i3yuan.cnblogs.com") },
                    License = new OpenApiLicense { Name = "kizuna_ai", Url = new Uri("http://i3yuan.cnblogs.com") }
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 此方法由运行时调用,使用此方法配置HTTP请求管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(i=> {
                i.SwaggerEndpoint($"/swagger/v1/swagger.json", $"kizuna_ai");
                i.RoutePrefix = string.Empty;
                //i.RoutePrefix = "swagger";
            });
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
