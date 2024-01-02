using Dawem.API;
using Dawem.API.MiddleWares;
using Dawem.BusinessLogic;
using Dawem.BusinessLogic.Localization;
using Dawem.BusinessLogic.SignalR;
using Dawem.BusinessLogicCore;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Helpers;
using Dawem.Models.AutoMapper;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Dawem.Repository;
using Dawem.Repository.Managers;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Dawem.Validation;
using Dawem.Validation.FluentValidation.Authentications;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString(LeillaKeys.DawemConnectionString) ??
    throw new InvalidOperationException(LeillaKeys.ConnectionStringNotFound);
string AllowSpecificOrigins = LeillaKeys.AllowSpecificOrigins;

builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Serilog.Core.Logger logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();


builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowSpecificOrigins,
        builder => builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
});


builder.Services.AddIdentityCore<MyUser>(options =>
{

    options.SignIn.RequireConfirmedAccount = true;
    options.Tokens.ChangePhoneNumberTokenProvider = LeillaKeys.FourigitPhone;
    options.User.AllowedUserNameCharacters = LeillaKeys.AllowedUserNameCharacters;
})
.AddRoles<Role>()
.AddEntityFrameworkStores<ApplicationDBContext>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;


});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

IConfigurationSection appSettingsSection = builder.Configuration.GetSection(LeillaKeys.AppSettings);

builder.Services.AddUserConfiguration();
builder.Services.Configure<Jwt>(appSettingsSection);
builder.Services.Configure<IdentityOptions>(opt => { opt.SignIn.RequireConfirmedEmail = true; opt.User.RequireUniqueEmail = true; });

builder.Services.AddTransient<UserManagerRepository>();
builder.Services.ConfigureSQLContext(builder.Configuration);
builder.Services.ConfigureRepositoryContainer();
builder.Services.ConfigureBLValidation();
builder.Services.ConfigureRepository();
builder.Services.ConfigureBusinessLogic();
builder.Services.ConfigureBusinessLogicCore();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureBackGroundService();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.UseCamelCasing(true);
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
    options.SerializerSettings.DateParseHandling = DateParseHandling.None;
    options.SerializerSettings.Converters.Add(new MultiFormatDateConverter());
});

builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddFluentValidationAutoValidation(cpnfig =>
{
    cpnfig.OverrideDefaultResultFactoryWith<FluentValidationResultFactory>();

});

builder.Services.AddAutoMapper((serviceProvider, config) =>
{
}, typeof(AutoMapperConfig));

builder.Services.AddSignalR();
WebApplication app = builder.Build();
IServiceScope serviceScope = app.Services.GetService<IServiceScopeFactory>()
    .CreateScope();

ApplicationDBContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
SeedDB.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);
app.UseMiddleware<RequestHeaderContextMiddleWare>();

if (!app.Environment.IsDevelopment())
{
    context.Database.Migrate();
}

IServiceScopeFactory serviceScopeFactory = serviceScope.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
IServiceProvider serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
IUnitOfWork<ApplicationDBContext> unitOfWork = serviceProvider.GetService<IUnitOfWork<ApplicationDBContext>>();
GeneralSetting generalSetting = serviceProvider.GetService<GeneralSetting>();
RepositoryManager repositoryManager = new(unitOfWork, generalSetting, new RequestInfo());
new TranslationBL(unitOfWork, repositoryManager).RefreshCachedTranslation();


//var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseStaticFiles();

List<CultureInfo> supportedCultures = new()
{
                    new CultureInfo(LeillaKeys.En),
                    new CultureInfo(LeillaKeys.Ar)
                };

app.UseCors(AllowSpecificOrigins);

RequestLocalizationOptions requestLocalizationOptions = new()
{
    DefaultRequestCulture = new RequestCulture(LeillaKeys.En),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
app.UseMiddleware<UnauthorizedMessageHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization(requestLocalizationOptions);

app.UseMiddleware<ExceptionHandlerMiddleware>();
//app.UseMiddleware<PermissionMiddleWare>();
app.UseMiddleware<PermissionLogMiddleWare>();

//app.UseMiddleware<UserScreenActionPermissionMiddleWare>();

app.MapHub<NotificationHub>("/notificationHub");
app.Run();

