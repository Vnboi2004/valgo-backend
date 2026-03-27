using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VAlgo.BuildingBlocks.Sandbox.Abstractions;
using VAlgo.BuildingBlocks.Sandbox.Implementations;
using VAlgo.JudgeWorker.Clients;
using VAlgo.JudgeWorker.Services;
using VAlgo.JudgeWorker.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<JudgeApiKeyHandler>();

builder.Services.AddHttpClient<VAlgoApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["VAlgoApi:BaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddHttpMessageHandler<JudgeApiKeyHandler>();

builder.Services.AddScoped<ISandboxRunner, DockerSandboxRunner>();

builder.Services.AddHostedService<JudgeConsumer>();

var host = builder.Build();

await host.RunAsync();