﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Psg.Api.Base;
using Psg.Api.Data;
using Psg.Api.Helpers;
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
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<ITypeHelperService, TypeHelperService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlServer(baglantiSatiri);
            });
            services.AddDbContext<PsgContext>(options =>
            {
                options.UseSqlServer(baglantiSatiri);
            });
            services.AddTransient<KullaniciRepository>();
            services.AddTransient<ISeederManager, SeederManager>();
            services.AddTransient<UserSeeder>();

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IKullaniciRepository, KullaniciRepository>();
            services.AddScoped<ICinsiyetRepository, CinsiyetRepository>();
            services.AddScoped<IArkadaslikRepository, ArkadaslikRepository>();
            services.AddScoped<IUykuTestRepository, UykuTestRepository>();
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false

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
            seederManager.SeedAll();
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials()); ;
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
