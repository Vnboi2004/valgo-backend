using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VAlgo.JudgeWorker.Clients;
using VAlgo.JudgeWorker.Common;
using VAlgo.JudgeWorker.Execution.Comparators;
using VAlgo.JudgeWorker.Execution.Docker;
using VAlgo.JudgeWorker.Infrastructure.Docker;
using VAlgo.JudgeWorker.Messaging;
using VAlgo.JudgeWorker.Orchestration;
using VAlgo.JudgeWorker.Workers;

var builder = Host.CreateApplicationBuilder(args);

// Configuration
builder.Services.Configure<JudgeWorkerOptions>(builder.Configuration.GetSection("JudgeWorker"));

// Core identity
builder.Services.AddSingleton<WorkerIdentity>();

// Execution
builder.Services.AddSingleton<DockerExecutor>();

builder.Services.AddSingleton<IOutputComparator, ExactComparator>();
builder.Services.AddSingleton<IOutputComparator, IgnoreWhitespaceComparator>();
builder.Services.AddSingleton<IOutputComparator, FloatingPointComparator>();

// Orchestration
builder.Services.AddSingleton<JudgeOrchestrator>();
builder.Services.AddSingleton<JudgeResultMapper>();

// Infrastructure
builder.Services.AddSingleton<DockerAvailabilityChecker>();

// Messinging
builder.Services.AddSingleton<IJobQueue, InMemoryJobQueue>();

// HTTP Clients
builder.Services.AddHttpClient<SubmissionsClient>((sp, http) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    http.BaseAddress = new Uri(config["Clients:Submissions:BaseUrl"]!);
});

builder.Services.AddHttpClient<ProblemsClient>((sp, http) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    http.BaseAddress = new Uri(config["Clients:Problems:BaseUrl"]!);
});

// Worker
builder.Services.AddHostedService<JudgeWorkerService>();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Build & Run
var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var checker = scope.ServiceProvider.GetRequiredService<DockerAvailabilityChecker>();
    await checker.EnsureAvailableAsync();
}

await host.RunAsync();