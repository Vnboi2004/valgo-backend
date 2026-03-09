using Microsoft.Extensions.Options;
using VAlgo.API.Hubs;
using VAlgo.API.Realtime;
using VAlgo.Modules.Contests;
using VAlgo.Modules.Contests.Application.Realtime;
using VAlgo.Modules.Discussions;
using VAlgo.Modules.Identity;
using VAlgo.Modules.ProblemClassification;
using VAlgo.Modules.ProblemManagement;
using VAlgo.Modules.Submissions;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Infrastructure.Redis;
using VAlgo.SharedKernel.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Core framework
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen();

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


builder.Services.AddOpenApi();

var app = builder.Build();


// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<ContestLeaderboardHub>("/hubs/contest-leaderboard");

app.Run();
