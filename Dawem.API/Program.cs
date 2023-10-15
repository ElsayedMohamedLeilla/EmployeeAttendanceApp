using Dawem.API;
using Dawem.API.MiddleWares;
using Dawem.Data;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.AutoMapper;
using Dawem.Models.Generic;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Glamatek.API.MiddleWares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString(DawemKeys.DawemConnectionString) ??
    throw new InvalidOperationException(DawemKeys.ConnectionStringNotFound);
string AllowSpecificOrigins = DawemKeys.AllowSpecificOrigins;

builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddControllers().AddJsonOptions(options =>
//                options.JsonSerializerOptions.Converters.Add(new IntToStringConverter()));
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


builder.Services.AddIdentityCore<User>(options =>
{

    options.SignIn.RequireConfirmedAccount = true;
    options.Tokens.ChangePhoneNumberTokenProvider = DawemKeys.FourigitPhone;
    options.User.AllowedUserNameCharacters = DawemKeys.AllowedUserNameCharacters;
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
    // Default SignIn settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

IConfigurationSection appSettingsSection = builder.Configuration.GetSection(DawemKeys.AppSettings);

builder.Services.AddUserConfiguration();
builder.Services.Configure<Jwt>(appSettingsSection);
builder.Services.Configure<IdentityOptions>(opt => { opt.SignIn.RequireConfirmedEmail = true; opt.User.RequireUniqueEmail = true; });

builder.Services.AddTransient<UserManagerRepository>();
builder.Services.ConfigureSQLContext(builder.Configuration);
builder.Services.ConfigureRepositoryContainer();
builder.Services.ConfigureBLContainer();

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureJwtAuthentication(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.UseCamelCasing(true);
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
    options.SerializerSettings.Converters.Add(new Dawem.API.DateTimeConverter());
});

builder.Services.AddAutoMapper((serviceProvider, config) =>
{
    config.AddProfile<AutoMapperConfig>();
}, typeof(Program));

WebApplication app = builder.Build();
IServiceScope serviceScope = app.Services.GetService<IServiceScopeFactory>()
    .CreateScope();

ApplicationDBContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDBContext>();


SeedDB.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);


app.UseMiddleware<RequestHeaderContextMiddleWare>();














// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (/*app.Environment.IsDevelopment() || app.Environment.IsProduction()*/true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
