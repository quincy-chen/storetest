using System;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
//using ApiCore.Dal;
//using ApiCore.Dal.IDal;
//using ApiCore.DBContext;
using ApiCore.Helper;

namespace ApiCore
{
    public class Startup
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        ///     注入
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

                #region 跨域 配置

                services.AddCors(options =>
                    options.AddPolicy("AllowSameDomain",
                        builder =>
                            builder
                                // .WithOrigins("http://localhost:9090","http://localhost:9091")//允许部分.  可以读取配置文件中的 AllowedHosts
                                .AllowAnyOrigin() //允许所有
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials()
                    )
                );

                #endregion

                #region 数据库初始化

                //services.AddDbContext<MyDBInfoContext>(options =>
                //    options.UseSqlServer(Configuration.GetConnectionString("SQLServerConnection")));

                #endregion

                #region Swagger接口文档初始化

                //注册Swagger生成器，定义一个和多个Swagger 文档
                services.AddSwaggerGen(sw => { sw.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });

                #endregion

                #region 接口注入初始化

                //SqlPadDal接口注入
                //services.AddTransient<ISqlPadDal, SqlPadDal>();

                #endregion

            }
            catch (Exception e)
            {
                Log4Helper.Error(e.Message);
                throw e;
            }
        }


        /// <summary>
        ///     加载配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

                //加载 Cors 跨域
                app.UseCors("AllowSameDomain");

                app.UseMvc();

                #region Swagger

                //启用中间件服务生成Swagger作为JSON终结点
                app.UseSwagger();
                //启用中间件服务对swagger-ui，指定Swagger JSON终结点
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

                /*
                 //全局捕获异常
                  if (env.IsDevelopment())
                    app.UseDeveloperExceptionPage();
                else
                    app.UseExceptionHandler(builder => builder.Run(async context => await ErrorEvent(context)));


                  throw new Exception("123");
                 */

                #endregion

            }
            catch (Exception e)
            {
                Log4Helper.Error(e.Message);
                throw e;
            }
        }

        // /// <summary>
        // /// 全局捕获异常(暂时不可用)
        // /// </summary>
        // /// <param name="context"></param>
        // /// <returns></returns>
        // private Task ErrorEvent(HttpContext context)
        // {
        //     var feature = context.Features.Get<IExceptionHandlerFeature>();
        //     var error = feature?.Error;
        //     Log4Helper.Error("Global\\Error", error);
        //     return context.Response.WriteAsync(JsonConvert.SerializeObject("系统未知异常，请联系管理员"),
        //         Encoding.GetEncoding("GBK"));
        // }
    }
}