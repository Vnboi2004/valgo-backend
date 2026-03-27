using Microsoft.Extensions.DependencyInjection;
using VAlgo.BuildingBlocks.Sandbox.Abstractions;
using VAlgo.BuildingBlocks.Sandbox.Implementations;

namespace VAlgo.BuildingBlocks.Sandbox
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSandbox(this IServiceCollection services)
        {
            services.AddScoped<ISandboxRunner, DockerSandboxRunner>();

            return services;
        }
    }
}