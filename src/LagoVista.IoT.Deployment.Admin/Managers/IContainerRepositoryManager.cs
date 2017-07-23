﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;

namespace LagoVista.IoT.Deployment.Admin.Managers
{
    public interface IContainerRepositoryManager
    {
        Task<InvokeResult> AddContainerRepoAsync(ContainerRepository containerRepo, EntityHeader org, EntityHeader user);
        Task<ContainerRepository> GetContainerRepoAsync(string id, EntityHeader org, EntityHeader user);
        Task<IEnumerable<ContainerRepositorySummary>> GetContainerReposForOrgAsync(string orgId, EntityHeader user);
        Task<InvokeResult> UpdateContainerRepoAsync(ContainerRepository containerRepo, EntityHeader org, EntityHeader user);
    }
}