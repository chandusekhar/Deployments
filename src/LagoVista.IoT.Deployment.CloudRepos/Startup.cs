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
            services.AddTransient<IDeviceMessageDefinitionRepo, DeviceMessageDefinitionRepo>();
            services.AddTransient<IDeploymentInstanceRepo, DeploymentInstanceRepo>();
            services.AddTransient<IDeploymentHostRepo, DeploymentHostRepo>();
        }
    }
}