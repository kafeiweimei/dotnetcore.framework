using Autofac;
using Common;
using Extensions.ServiceExtensions;
using IRepository.Base;
using IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DotNetCore.Framework.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private const string DefaultCorsPolicyName = "Default";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContent();

            #region   Swagger
            services.AddSwaggerGen(c=> 
            {
                c.SwaggerDoc("V1",new OpenApiInfo 
                {
                    Version="V0.0.1",
                    Title= "DotNetCore.Framework.Api",
                    Description="接口说明文档",
                    Contact=new OpenApiContact
                    {
                        Name="DotNetCore.FrameWork",
                        Email="XXX@163.com",
                        Url= new Uri("https://coffeemilk.blog.csdn.net/")
                    }
                    
                });

                var basePath = AppContext.BaseDirectory;
                var apiXmlPath = Path.Combine(basePath, "DotNetCore.Framework.Api.xml");
                c.IncludeXmlComments(apiXmlPath, true);

                var modelsXmlPath = Path.Combine(basePath, "Models.xml");
                c.IncludeXmlComments(modelsXmlPath, true);

                #region   JWT权限认证
                //开启小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //在header中添加token,且传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权（数据将在请求头中进行传输）直接在下框中输入Bearer {token}(注意两者之间有一个空格)",
                    Name="Authorization",           //JWT的默认参数名称
                    In=ParameterLocation.Header,    //JWT默认存放Authorization信息的位置（这里指定在请求头中）
                    Type=SecuritySchemeType.ApiKey

                }); 


                #endregion 
            });

            #endregion

            services.AddSingleton(new AppSettingsHelper(Configuration));
            ////NetCore自带的依赖注入（适用于实例较少的情况【低于20】）
            //services.AddScoped<IAdvertisementServices, AdvertisementServices>();
            //services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));

            //统一认证JWT的Token
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o=> 
            {
                /*默认是通过Http的Authorization头来获取JWT的Token内容*/
                //读取配置信息
                var audienceConfig = Configuration.GetSection("Audience");
                var symmetricKeyAsBase64 = audienceConfig["Secret"];
                var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
                var signingKey = new SymmetricSecurityKey(keyByteArray);

                ///*也可以使用Url来传递Token*/
                //o.Events = new JwtBearerEvents()
                //{
                //    OnMessageReceived=context=>
                //    {
                //        context.Token = context.Request.Query["access_Token"];
                //        return Task.CompletedTask;
                //    }
                //};

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,  //签署密钥
                    ValidateIssuer = true,
                    ValidIssuer = audienceConfig["Issuer"], //发行人
                    ValidateAudience = true,
                    ValidAudience = audienceConfig["Audience"], //订阅人
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,      //这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
                    RequireExpirationTime = true,
                };
            });

            // 1【授权】、这个和上边的异曲同工，好处就是不用在controller中，写多个 roles 。
            // 然后这么写 [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//单独角色
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//或的关系
                options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//且的关系
            });

        }

        //配置容器
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacModuleRegister>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region   1-Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c=> 
            {
                c.SwaggerEndpoint("/Swagger/V1/Swagger.json","ApiHelp V1");
                c.RoutePrefix = "";//路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉
            });

            #endregion 

            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            #region   2-认证授权
            //先认证
            app.UseAuthentication();
            //再授权中间件
            app.UseAuthorization();
            
            #endregion 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
