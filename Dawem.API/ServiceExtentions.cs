﻿using Dawem.BackgroundJobs;
using Dawem.BusinessLogic.Dawem.Lookups;
using Dawem.Contract.BusinessLogic.Dawem.Lookups;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Dawem.API
{
    public static class ServiceExtentions
    {
        public static void ConfigureSQLContext(this IServiceCollection services, IConfiguration config)
        {
            _ = services.AddDbContext<ApplicationDBContext>(opts =>
            {
                _ = opts.UseSqlServer(config[LeillaKeys.ConnectionStringsDawemConnection],
                opts => opts.CommandTimeout(60));
                _ = opts.EnableSensitiveDataLogging(true);


            });
            _ = services.AddDbContext<ApplicationDBContext>(options =>
            {
                _ = options.UseSqlServer(
                config.GetConnectionString(LeillaKeys.ConnectionStrings));
                _ = options.EnableSensitiveDataLogging(true);

            }
           );
        }
        public static void ConfigureGlobals(this IServiceCollection services, IConfiguration config)
        {
            IConfigurationSection appSettingsSection = config.GetSection(LeillaKeys.Globals);
            var globals = appSettingsSection.Get<GlobalVariables>();
            StaticGlobalVariables.IPInfoToken = globals.IPInfoToken;
        }
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            IConfigurationSection appSettingsSection = config.GetSection(LeillaKeys.Jwt);
            _ = services.Configure<Jwt>(appSettingsSection);
            Jwt? appSettings = appSettingsSection.Get<Jwt>();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Key);

            var audience = appSettings.Issuer;
            var issuer = appSettings.Issuer;

            _ = services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Query.TryGetValue("access_token", out StringValues token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var te = context.Exception;
                        return Task.CompletedTask;
                    }
                };

            }
            );
        }
        public static void ConfigureRepositoryContainerOld(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDBContext>();
            services.AddScoped<IUnitOfWork<ApplicationDBContext>, UnitOfWork<ApplicationDBContext>>();

        }
        public static void ConfigureBLContainer(this IServiceCollection services)
        {
            _ = services.AddScoped<ILookupsBL, LookupsBL>();

        }
        public static void AddUserConfiguration(this IServiceCollection services)
        {

            IdentityBuilder myUserBuilder = services.AddIdentity<MyUser, Role>(options =>
            {
                //  options.SignIn.RequireConfirmedAccount = true;

                options.User.AllowedUserNameCharacters =
                LeillaKeys.AllowedUserNameCharacters;
            });
            myUserBuilder = new IdentityBuilder(myUserBuilder.UserType, typeof(UserRole), myUserBuilder.Services);
            _ = myUserBuilder.AddRoles<Role>()
           .AddEntityFrameworkStores<ApplicationDBContext>()
           .AddUserManager<UserManagerRepository>()
           .AddDefaultTokenProviders();



            _ = services.Configure<GlameraUserIdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

        }
        public static void ConfigureBackGroundService(this IServiceCollection services)
        {
            services.AddHostedService<SchedulePlanHostedService>();
            services.AddHostedService<SummonsHostedService>();
            services.AddHostedService<SubscriptionHostedService>();
            services.AddHostedService<AlertsHostedService>();
            //services.AddSingleton<CancellationTokenSource>();
        }
    }
}
