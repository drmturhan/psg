using AutoMapper;
using Core.Base;
using Core.EntityFramework;
using Core.Mail;
using Identity.DataAccess;
using Identity.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Psg.Api.Extensions;
using Psg.Api.Helpers;
using Psg.Api.Preferences;
using Psg.Api.Repos;
using Psg.Api.Seeds;
using System.Net;
using System.Text;

namespace Psg.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;
        public IConfigurationRoot Configuration { get; }
        public ApiTercihleri Tercihler { get; set; }
        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            _environment = env;
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            bool useSqLite = false;
            bool.TryParse(Configuration["Data:useSqLite"], out useSqLite);
            string baglantiSatiri = useSqLite ? Configuration["Data:SqlLiteConnectionString"] : Configuration["Data:SqlServerConnectionString"];
            services.AddAutoMapper();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.Configure<FotografAyarlari>(Configuration.GetSection("FotografAyarlari"));
            services.Configure<ApiTercihleri>(Configuration.GetSection("ApiTercihleri"));
            services.Configure<UygulamaAyarlari>(Configuration.GetSection("AppSettings"));
            services.Configure<EpostaHesapBilgileri>(Configuration.GetSection("SistemPostaHesapBilgileri"));
            services.Configure<SMSHesapBilgileri>(Configuration.GetSection("SistemSMSHesapBilgileri"));



            var sp = services.BuildServiceProvider();
            Tercihler = sp.GetService<IOptions<ApiTercihleri>>().Value;


            services.AddTransient<IEmailSender, PostaciKit>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<ITypeHelperService, TypeHelperService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddDbContexts(useSqLite, baglantiSatiri, Tercihler);

            if (Tercihler.AspNetCoreIdentityKullan)
            {

                services.AddMTIdentity(Configuration);

            }
            services.AddTransient<KullaniciRepository>();
            services.AddTransient<ISeederManager, SeederManager>();

            
            services.AddScoped<IKullaniciRepository, KullaniciRepository>();



            services.AddScoped<ICinsiyetRepository, CinsiyetRepository>();
            services.AddScoped<IArkadaslikRepository, ArkadaslikRepository>();
            services.AddScoped<IUykuTestRepository, UykuTestRepository>();
            var uygulamaAyarlari = sp.GetService<IOptions<UygulamaAyarlari>>().Value;

   
           services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(uygulamaAyarlari.JwtKey)),
                        ValidIssuer = "",
                        ValidateIssuer = false,
                        ValidAudience= "",
                        ValidateAudience=false

                    };

                });
            services.AddMvc().AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            services.AddScoped<KullaniciAktiviteleriniTakipEt>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ISeederManager seederManager)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                
            }
            else
            {

                
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var hata = context.Features.Get<IExceptionHandlerFeature>();
                        if (hata != null)
                        {
                            context.Response.UygulamaHatasiEkle();
                            await context.Response.WriteAsync(hata.Error.Message);
                        }
                    });
                });
            }


            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials()); ;
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
