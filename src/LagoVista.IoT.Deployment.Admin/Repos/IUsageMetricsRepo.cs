﻿using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Deployment.Admin.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Deployment.Admin.Repos
{
    public interface IUsageMetricsRepo
    {
        Task<ListResponse<UsageMetrics>> GetMetricsForHostAsync(string hostId, ListRequest request);
        Task<ListResponse<UsageMetrics>> GetMetricsForDependencyAsync(string dependencyId, ListRequest request);
        Task<ListResponse<UsageMetrics>> GetMetricsForInstanceAsync(string instanceId, ListRequest request);
        Task<ListResponse<UsageMetrics>> GetMetricsForPipelineModuleAsync(string pipelineModuleId, ListRequest request);
    }
}
