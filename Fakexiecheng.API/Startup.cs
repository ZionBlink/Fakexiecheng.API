using Fakexiecheng.API.Database;
using Fakexiecheng.API.Models;
using Fakexiecheng.API.Moldes;
using Fakexiecheng.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using SimplePatch;
using System;
using System.Linq;
using System.Text;

namespace Fakexiecheng.API
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {

            DeltaConfig.Init(options =>
            {
                options.AddEntity<Form_TD>();



            });


            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                 .AddEntityFrameworkStores<AppDbContext>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretByte = Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Authentication:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = Configuration["Authentication:Audience"],

                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                });


            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                //setupAction.OutputFormatters.Add(
                //    new XmlDataContractSerializerOutputFormatter()    
                //);
            }).AddNewtonsoftJson(setupAction => { setupAction.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; })
            .AddNewtonsoftJson(setupAction =>
            {
                setupAction.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters()
            //数据验证（没理解仔细看代码）
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "无所谓",
                        Title = "数据验证失败",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "请看详细说明",
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetail)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });
            services.AddTransient<ITouristRouteRepository, TouristRouteRepository>();

            services.AddDbContext<AppDbContext>(option =>
            {
                /*  option.UseSqlServer("server=localhost; Database=FakeXiechengDb; User Id=sa;Password=Pass@w0rd;") ;*/
                //本地自带数据库
                option.UseSqlServer(Configuration["DbContext:ConnectionString"]);
                //docker数据库
                // option.UseSqlServer(Configuration["AppContext:ConnectionString"]);
            });

            //扫描profile文件，添加dto的映射，对将要传输至前端的数据塑形
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpClient();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.Configure<MvcOptions>(config =>
            {
                var outputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                if (outputFormatter != null)
                {
                    outputFormatter.SupportedMediaTypes.Add("application/vnd.blink.hateoas+json");

                }

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //你在哪里？
            app.UseRouting();

            //你是谁
            app.UseAuthentication();
            //你的权限是
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                /*endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapGet("/test", async context =>
                {
                    throw new Exception("test");
                    await context.Response.WriteAsync("Hello World form test!");
                });*/
                endpoints.MapControllers();


            });

        }
    }
}
