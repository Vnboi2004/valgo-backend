using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VAlgo.API.Hubs;
using VAlgo.API.Realtime;
using VAlgo.API.Services;
using VAlgo.Modules.Contests;
using VAlgo.Modules.Contests.Application.Realtime;
using VAlgo.Modules.Discussions;
using VAlgo.Modules.Identity;
using VAlgo.Modules.Identity.Infrastructure.Security;
using VAlgo.Modules.ProblemClassification;
using VAlgo.Modules.ProblemManagement;
using VAlgo.Modules.Submissions;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Infrastructure.Redis;
using VAlgo.SharedKernel.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Core framework
builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtOptions = jwtSection.Get<JwtOptions>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.SecretKey)
        ),

            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Modules

// Submissions
builder.Services.AddSubmissionsModule(builder.Configuration);

// Problem Management
builder.Services.AddProblemManagementModule(builder.Configuration);

// Problem Classification
builder.Services.AddProblemClassificationModule(builder.Configuration);

// Identity
builder.Services.AddIdentityModule(builder.Configuration);

// Contests
builder.Services.AddContestsModule(builder.Configuration);

// Discussions
builder.Services.AddDiscussionsModule(builder.Configuration);

// SharedKernel
builder.Services.AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>();
builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();

// Register RedisConnectionFactory
builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
builder.Services.AddSingleton<RedisConnectionStringFactory>(sp =>
{
    var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
    return new RedisConnectionStringFactory(options.ConnectionString);
});

// Register RedisDatabaseProvider
builder.Services.AddSingleton<RedisDatabaseProvider>();

// Register Hubs
builder.Services.AddSignalR();

builder.Services.AddScoped<IContestLeaderboardNotifier, SignalRContestLeaderboardNotifier>();

// Register services
builder.Services.AddScoped<IUserReadService, UserReadService>();
builder.Services.AddScoped<IProblemReadService, ProblemReadService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();


// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ContestLeaderboardHub>("/hubs/contest-leaderboard");

app.Run();
