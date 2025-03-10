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
                    Description="�ӿ�˵���ĵ�",
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

                #region   JWTȨ����֤
                //����С��
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //��header�����token,�Ҵ��ݵ���̨
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ�����ݽ�������ͷ�н��д��䣩ֱ�����¿�������Bearer {token}(ע������֮����һ���ո�)",
                    Name="Authorization",           //JWT��Ĭ�ϲ�������
                    In=ParameterLocation.Header,    //JWTĬ�ϴ��Authorization��Ϣ��λ�ã�����ָ��������ͷ�У�
                    Type=SecuritySchemeType.ApiKey

                }); 


                #endregion 
            });

            #endregion

            services.AddSingleton(new AppSettingsHelper(Configuration));
            ////NetCore�Դ�������ע�루������ʵ�����ٵ����������20����
            //services.AddScoped<IAdvertisementServices, AdvertisementServices>();
            //services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));

            //ͳһ��֤JWT��Token
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o=> 
            {
                /*Ĭ����ͨ��Http��Authorizationͷ����ȡJWT��Token����*/
                //��ȡ������Ϣ
                var audienceConfig = Configuration.GetSection("Audience");
                var symmetricKeyAsBase64 = audienceConfig["Secret"];
                var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
                var signingKey = new SymmetricSecurityKey(keyByteArray);

                ///*Ҳ����ʹ��Url������Token*/
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
                    IssuerSigningKey = signingKey,  //ǩ����Կ
                    ValidateIssuer = true,
                    ValidIssuer = audienceConfig["Issuer"], //������
                    ValidateAudience = true,
                    ValidAudience = audienceConfig["Audience"], //������
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,      //����ǻ������ʱ�䣬Ҳ����˵����ʹ���������˹���ʱ�䣬����ҲҪ���ǽ�ȥ������ʱ��+���壬Ĭ�Ϻ�����7���ӣ������ֱ������Ϊ0
                    RequireExpirationTime = true,
                };
            });

            // 1����Ȩ����������ϱߵ�����ͬ�����ô����ǲ�����controller�У�д��� roles ��
            // Ȼ����ôд [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//������ɫ
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//��Ĺ�ϵ
                options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//�ҵĹ�ϵ
            });

        }

        //��������
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
                c.RoutePrefix = "";//·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ��
            });

            #endregion 

            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            #region   2-��֤��Ȩ
            //����֤
            app.UseAuthentication();
            //����Ȩ�м��
            app.UseAuthorization();
            
            #endregion 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
