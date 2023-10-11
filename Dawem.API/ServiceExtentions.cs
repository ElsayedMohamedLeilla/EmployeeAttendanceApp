using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Generic;
using Dawem.Translations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Dawem.API
{
    public static class ServiceExtentions
    {

        /*public static void dewe()
        {

            IConfiguration configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile(DawemKeys.AppsettingsFile)
             .Build();

            string? connectionString = configuration.GetConnectionString("local");
            _ = new DbContextOptionsBuilder<ApplicationDBContext>()
                             .UseSqlServer(new SqlConnection(connectionString))
                             .Options;
        }*/
        public static void ConfigureSQLContext(this IServiceCollection services, IConfiguration config)
        {
            _ = services.AddDbContext<ApplicationDBContext>(opts =>
            {
                _ = opts.UseSqlServer(config[DawemKeys.ConnectionStringsDawemConnection],
                opts => opts.CommandTimeout(60));
                _ = opts.EnableSensitiveDataLogging(true);


            });
            _ = services.AddDbContext<ApplicationDBContext>(options =>
            {
                _ = options.UseSqlServer(
                config.GetConnectionString(DawemKeys.ConnectionStrings));
                _ = options.EnableSensitiveDataLogging(true);

            }
           );


        }

        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            IConfigurationSection appSettingsSection = config.GetSection(DawemKeys.Jwt);
            _ = services.Configure<Jwt>(appSettingsSection);
            Jwt? appSettings = appSettingsSection.Get<Jwt>();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Key);
            _ = services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
        public static void ConfigureRepositoryContainer(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDBContext>();
            services.AddScoped<IUnitOfWork<ApplicationDBContext>, UnitOfWork<ApplicationDBContext>>();
            services.AddScoped<ISmartUserRepository, SmartUserRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IPackageScreenRepository, PackageScreenRepository>();
            services.AddScoped<ITranslationRepository, TranslationRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IBranchCurrencyRepository, BranchCurrencyRepository>();
            services.AddScoped<IUserBranchRepository, UserBranchRepository>();
            services.AddScoped<ISmartUserTokenRepository, SmartUserTokenRepository>();
            services.AddScoped<ISmartUserRoleRepository, SmartUserRoleRepository>();
            services.AddScoped<IPaymentMethodBranchRepository, PaymentMethodBranchRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerBanksRepository, CustomerBankRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IPaymentTermRepository, PaymentTermRepository>();
            services.AddScoped<ISalesPersonRepository, SalesPersonRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IScreenRepository, ScreenRepository>();
            services.AddScoped<IAccountInfoRepository, AccountInfoRepository>();
            services.AddScoped<IFinincialYearRepository, FinincialYearRepository>();
            services.AddScoped<ISettingMigrationAccountRepository, SettingMigrationAccountRepository>();

            services.AddScoped<IActionLogRepository, ActionLogRepository>();
            services.AddScoped<IUserScreenActionPermissionRepository, UserScreenActionPermissionRepository>();


            services.AddScoped<IStoreRepository, StoreRepository>();

            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();

            _ = services.AddScoped<IUnitRepository, UnitRepository>();
            _ = services.AddScoped<IUnitBranchRepository, UnitBranchRepository>();

            _ = services.AddScoped<IProductUnitRepository, ProductUnitRepository>();
        }
        public static void ConfigureBLContainer(this IServiceCollection services)
        {
            _ = services.AddScoped<ILookupsBL, LookupsBL>();
            _ = services.AddScoped<ISmartUserBL, SmartUserBL>();
            _ = services.AddScoped<ITranslationBL, TranslationBL>();
            _ = services.AddScoped<IAccountBL, AccountBL>();
            _ = services.AddScoped<IScreenBL, ScreenBL>();
            _ = services.AddScoped<IBranchBL, BranchBL>();
            _ = services.AddScoped<ICompanyBL, CompanyBL>();
            _ = services.AddScoped<IPackageBL, PackageBL>();
            _ = services.AddScoped<IMailBL, MailBL>();
            _ = services.AddScoped<IActionLogBL, ActionLogBL>();
            _ = services.AddScoped<IUserScreenActionPermissionBL, UserScreenActionPermissionBL>();

            _ = services.AddScoped<IStoreBL, StoreBL>();
            _ = services.AddScoped<IGroupBL, GroupBL>();

            _ = services.AddScoped<IPaymentMethodBL, PaymentMethodBL>();
            _ = services.AddScoped<IPaymentMethodBranchBL, PaymentMethodBranchBL>();
            _ = services.AddScoped<IPackageScreenBL, PackageScreenBL>();
            _ = services.AddScoped<IBranchValidatorBL, BranchValidatorBL>();
            _ = services.AddScoped<IRegisterationValidatorBL, RegisterationValidatorBL>();
            services.AddScoped<IAccountInfoBL, AccountInfoBL>();
            _ = services.AddScoped<IUserBranchBL, UserBranchBL>();
            _ = services.AddScoped<IUnitBL, UnitBL>();
            _ = services.AddScoped<IUnitBranchBL, UnitBranchBL>();
            services.AddScoped<ISettingMigrationAccountBL, SettingMigrationAccountBL>();
            _ = services.AddScoped<IProductUnitBL, ProductUnitBL>();
            services.AddScoped<IFinincialYearBL, FinincialYearBL>();
            _ = services.AddScoped<IUserBranchBL, UserBranchBL>();
            _ = services.AddScoped<IUserBranchBL, UserBranchBL>();
            services.AddScoped<IContactBL, ContactBL>();
            services.AddScoped<ISalesPersonBL, SalesPersonBL>();

            services.AddScoped<ICustomerBL, CustomerBL>();
            services.AddScoped<IPaymentTermBL, PaymentTermBL>();
            services.AddScoped<IBankAccountBL, BankAccountBL>();

            _ = services.AddScoped<GeneralSetting>();
            _ = services.AddScoped<RequestHeaderContext>();
        }
        public static void AddUserConfiguration(this IServiceCollection services)
        {

            IdentityBuilder myUserBuilder = services.AddIdentity<User, Role>(options =>
            {
                //  options.SignIn.RequireConfirmedAccount = true;

                options.User.AllowedUserNameCharacters =
                DawemKeys.AllowedUserNameCharacters;
            });
            myUserBuilder = new IdentityBuilder(myUserBuilder.UserType, typeof(UserRole), myUserBuilder.Services);
            _ = myUserBuilder.AddRoles<Role>()
           .AddEntityFrameworkStores<ApplicationDBContext>()
           .AddUserManager<SmartUserManagerRepository>()
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

    }
}
