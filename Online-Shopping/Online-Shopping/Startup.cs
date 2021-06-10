using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Online_Shopping.Api.Client.DTOs;
using Online_Shopping.Api.Manage.DTOs;
using Online_Shopping.Data;
using Online_Shopping.Data.Entities;
using Online_Shopping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping
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
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            }).AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

            services.AddControllers().AddFluentValidation();

            services.AddScoped<IJwtService, JwtService>();

            #region IValidator
            services.AddTransient<IValidator<AdminLoginDto>, AdminLoginDtoValidator>();
            services.AddTransient<IValidator<AdminEditDto>, AdminEditDtoValidator>();
            services.AddTransient<IValidator<RegisterDto>, RegisterDtoValidator>();
            services.AddTransient<IValidator<MemberEditDto>, MemberEditDtoValidator>();
            services.AddTransient<IValidator<MemberLoginDto>, MemberLoginDtoValidator>();
            services.AddTransient<IValidator<CategoryCreateDto>, CategoryCreateDtoValidator>();
            services.AddTransient<IValidator<SubCategoryCreateDto>, SubCategoryCreateDtoValidator>();
            services.AddTransient<IValidator<TagCreateDto>, TagCreateValidatorDto>();
            services.AddTransient<IValidator<BrandCreateDto>, BrandCreateValidatorDto>();
            services.AddTransient<IValidator<SizeCreateDto>, SizeCreateValidatorDto>();
            services.AddTransient<IValidator<ColorCreateDto>, ColorCreateValidatorDto>();
            services.AddTransient<IValidator<ProductCreateDto>, ProductCreateValidatorDto>();
            services.AddTransient<IValidator<ProductPhotoDto>, ProductPhotoDtoValidator>();
            #endregion

            services.AddAutoMapper(typeof(Startup));

            #region AuthenticationJWTBearer
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration.GetSection("JWT:Issuer").Value,
                    ValidAudience = Configuration.GetSection("JWT:Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:Key").Value))
                };
            });
            #endregion



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
