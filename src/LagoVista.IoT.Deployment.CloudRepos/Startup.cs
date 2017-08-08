﻿using LagoVista.IoT.Deployment.Admin.Repos;
using LagoVista.IoT.Deployment.CloudRepos.Repos;
using Microsoft.Extensions.DependencyInjection;

namespace LagoVista.IoT.Deployment.CloudRepos
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDeviceConfigurationRepo, DeviceConfigurationRepo>();
            services.AddTransient<ISolutionRepo, SolutionRepo>();
            services.AddTransient<IUsageMetricsRepo, UsageMetricsRepo>();
            services.AddTransient<IDeploymentActivityRepo, DeploymentActivityRepo>();
            services.AddTransient<IFailedDeploymentActivityRepo, FailedDeploymentActivityRepo>();
            services.AddTransient<ICompletedDeploymentActivityRepo, CompletedDeploymentActivityRepo>();
            services.AddTransient<IContainerRepositoryRepo, ContainerRepositoryRepo>();
            services.AddTransient<IDeploymentInstanceRepo, DeploymentInstanceRepo>();
            services.AddTransient<IDeploymentHostRepo, DeploymentHostRepo>();
        }
    }
}