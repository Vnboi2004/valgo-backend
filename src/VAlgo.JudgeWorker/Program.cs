using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VAlgo.JudgeWorker.Clients;
using VAlgo.JudgeWorker.Sandbox;
using VAlgo.JudgeWorker.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<VAlgoApiClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5197");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddSingleton<DockerSandboxRunner>();

builder.Services.AddHostedService<JudgeConsumer>();

var host = builder.Build();

await host.RunAsync();