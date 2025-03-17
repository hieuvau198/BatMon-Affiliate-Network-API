using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

#region User Secret
builder.Configuration.AddUserSecrets<Program>();

#endregion

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

#region Add Sign-in UI for Swagger page
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Add JWT token support to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert Access Token with 'Bearer ' prefix, Ex: 'Bearer ABCXYZ'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

#endregion

#region Register DbContext

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // Ensures env vars override appsettings.json

builder.Services.AddDbContext<AffiliateDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Register UoW
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

#region Register Repository DI
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

#endregion

#region Register Service DI
builder.Services.AddScoped<IAdvertiserService, AdvertiserService>();
builder.Services.AddScoped<IAdvertiserBalanceService, AdvertiserBalanceService>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IPromoteService, PromoteService>();
builder.Services.AddScoped<ICampaignPolicyService, CampaignPolicyService>();
builder.Services.AddScoped<ITrafficSourceService, TrafficSourceService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IAdvertiserUrlService, AdvertiserUrlService >();

#endregion

#region Register Mapping DTOs
builder.Services.AddAutoMapper(typeof(AdvertiserMappingProfile),
                                typeof(CampaignPolicyMappingProfile),
                                typeof(TrafficSourceMappingProfile),
                                typeof(AdvertiserUrlMappingProfile),
                                typeof(CampaignMappingProfile));
#endregion

#region Add CORS service and allow all origins for simplicity
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Allow all origins
              .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
              .AllowAnyHeader(); // Allow any headers
    });
});
#endregion


var app = builder.Build();

app.UseCors("AllowAll");

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty; // Opens Swagger at the root URL
});


app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
