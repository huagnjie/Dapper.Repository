using DapperTest.JWT;
using DapperTest.JWT.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace DapperTest
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
            services.AddRazorPages();
            #region jwt����

            services.AddTransient<ITokenHelper, TokenHelper>();
            //��ȡ�����ļ����õ�jwt�������
            services.Configure<JWTConfig>(Configuration.GetSection("JWTConfig"));
            //����JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();
            services.AddScoped<TokenFilter>();

            #endregion

            #region Swagger
            services.AddControllers();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "DapperTest",
                    Description = "���Խӿ�"
                });
                config.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme()
                {
                    Description = "�����������������ͷ����Ҫ��ӵ�Jwt��ȨToken",
                    Name = "Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                //����һ��Scheme��ע�������IdҪ������AddSecurityDefinition�еĲ���nameһ��
                var scheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "JwtBearer" }
                };
                //ע��ȫ����֤�����еĽӿڶ�����ʹ����֤��
                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    [scheme] = new string[0]
                });

                // Ϊ Swagger ����xml�ĵ�ע��·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
                //c.DocInclusionPredicate((docName, description) => true);
                //��ӶԿ������ı�ǩ(����)
                //c.DocumentFilter<ApplyTagDescriptions>();//��ʾ����
                //c.CustomSchemaIds(type => type.FullName);// ���Խ����ͬ�����ᱨ�������
                //c.OperationFilter<AuthTokenHeaderParameter>();
            });

            #endregion

            services.AddCors(options =>
            {
                options.AddPolicy("any", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });
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
            app.UseCors("any");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DapperTest");
            });
            app.UseRouting();

            //������֤�м����д��UseAuthorizationǰ
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=swagger}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();
            });
        }
    }
}
