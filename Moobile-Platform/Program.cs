using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Moobile_Platform.CampaignManagement.Application.Internal.CommandServices;
using Moobile_Platform.CampaignManagement.Application.Internal.QueryServices;
using Moobile_Platform.CampaignManagement.Domain.Repositories;
using Moobile_Platform.CampaignManagement.Domain.Services;
using Moobile_Platform.CampaignManagement.Infrastructure.Repositories;
using Moobile_Platform.IAM.Application.CommandServices;
using Moobile_Platform.IAM.Application.OutBoundServices;
using Moobile_Platform.IAM.Application.QueryServices;
using Moobile_Platform.IAM.Domain.Repositories;
using Moobile_Platform.IAM.Domain.Services;
using Moobile_Platform.IAM.Infrastructure.Hashing.BCrypt.Services;
using Moobile_Platform.IAM.Infrastructure.Repositories;
using Moobile_Platform.IAM.Infrastructure.Tokens.JWT.Configuration;
using Moobile_Platform.IAM.Infrastructure.Tokens.JWT.Services;
using Moobile_Platform.RanchManagement.Application.Internal.CommandServices;
using Moobile_Platform.RanchManagement.Application.Internal.QueryServices;
using Moobile_Platform.RanchManagement.Domain.Repositories;
using Moobile_Platform.RanchManagement.Domain.Services;
using Moobile_Platform.RanchManagement.Infrastructure.Persistence.EFC.Repositories;
using Moobile_Platform.Shared.Application.OutboundServices;
using Moobile_Platform.Shared.Domain.Repositories;
using Moobile_Platform.Shared.Infrastructure.Interfaces.ASAP.Configuration;
using Moobile_Platform.Shared.Infrastructure.Media.Cloudinary;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using Moobile_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Moobile_Platform.StaffAdministration.Application.Internal.CommandServices;
using Moobile_Platform.StaffAdministration.Application.Internal.QueryServices;
using Moobile_Platform.StaffAdministration.Domain.Repositories;
using Moobile_Platform.StaffAdministration.Domain.Services;
using Moobile_Platform.StaffAdministration.Infrastructure.Persistence.EFC.Repositories;
using Moobile_Platform.VoiceCommand.Application.CommandServices;
using Moobile_Platform.VoiceCommand.Application.QueryServices;
using Moobile_Platform.VoiceCommand.Domain.Repositories;
using Moobile_Platform.VoiceCommand.Domain.Services;
using Moobile_Platform.VoiceCommand.Infrastructure.Parser;
using Moobile_Platform.VoiceCommand.Infrastructure.Persistence.EFC.Repositories;
using Moobile_Platform.VoiceCommand.Infrastructure.Speech;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//configure Lower Case URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configure Kebab Case Route Naming Convention
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Mapper for switching between admin and user resources of the ranch, campaign and staff management
builder.Services.AddHttpContextAccessor();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    
    // API Information
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Moobile Platform API",
        Version = "v1",
        Description = "Comprehensive API for ranch management, vaccination campaigns, livestock tracking, and staff administration",
        Contact = new OpenApiContact
        {
            Name = "Moobile Support Team",
            Email = "support@moobile.com",
            Url = new Uri("https://moobile.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // JWT Bearer Authentication Definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Global Security Requirement
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

/////////////////////////Begin Database Configuration/////////////////////////
// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Verify Database Connection string
if (connectionString is null)
    throw new Exception("Database connection string is not set");

// Configure Database Context and Logging Levels
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(
        options =>
        {
            options.UseMySQL(connectionString)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
else if (builder.Environment.IsProduction())
    builder.Services.AddDbContext<AppDbContext>(
        options =>
        {
            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Error)
                .EnableDetailedErrors();
        });

// Configure Swagger to show SQL queries in Development
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure Dependency Injection

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMediaStorageService, CloudinaryService>();

// Bounded Context Injection Configuration for Business

//IAM BC
builder.Services.AddScoped<IUserRepostory, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminCommandService, AdminCommandService>();
builder.Services.AddScoped<IAdminQueryService, AdminQueryService>();

//Ranch Management BC
builder.Services.AddScoped<IBovineRepository, BovineRepository>();
builder.Services.AddScoped<IBovineQueryService, BovineQueryService>();
builder.Services.AddScoped<IBovineCommandService, BovineCommandService>();
builder.Services.AddScoped<IVaccineRepository, VaccineRepository>();
builder.Services.AddScoped<IVaccineQueryService, VaccineQueryService>();
builder.Services.AddScoped<IVaccineCommandService, VaccineCommandService>();
builder.Services.AddScoped<IStableRepository, StableRepository>();
builder.Services.AddScoped<IStableQueryService, StableQueryService>();
builder.Services.AddScoped<IStableCommandService, StableCommandService>();

//Staff Administration BC
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffQueryService, StaffQueryService>();
builder.Services.AddScoped<IStaffCommandService, StaffCommandService>();

//Campaign Management BC
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<ICampaignCommandService, CampaignCommandService>();
builder.Services.AddScoped<ICampaignQueryService, CampaignQueryService>();

// Voice Command BC
builder.Services.AddScoped<IVoiceSpeechService, AzureSpeechService>();
builder.Services.AddScoped<IVoiceParserService, VoiceParserService>();
builder.Services.AddScoped<IVoiceCommandService, VoiceCommandService>();
builder.Services.AddScoped<IVoiceQueryService, VoiceQueryService>();
builder.Services.AddScoped<IVoiceTextToSpeechService, AzureTextToSpeechService>();
builder.Services.AddScoped<IVoiceRepository, VoiceRepository>();


/////////////////////////End Database Configuration/////////////////////////

// Configure Authentication & Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSettings:Secret"] ?? string.Empty)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();