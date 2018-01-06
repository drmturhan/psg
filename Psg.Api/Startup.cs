using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Psg.Api.Data;
using Psg.Api.Repos;
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
            services.AddCors(setup =>
            {
                setup.AddPolicy("psg", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(baglantiSatiri);
            });
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUykuTestRepository, UykuTestRepository>();
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer=false,
                        ValidateAudience=false

                    };

                });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("psg");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
