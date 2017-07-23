﻿using LagoVista.IoT.Web.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using System;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Identity;
using LagoVista.IoT.Deployment.Admin.Managers;
using Microsoft.AspNetCore.Mvc;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.Core.Validation;
using System.Threading.Tasks;
using LagoVista.Core;
using LagoVista.Core.Models.UIMetaData;

namespace LagoVista.IoT.Deployment.Admin.Rest.Controllers
{
    [Authorize]
   
    public class ContainerRepositoryController : LagoVistaBaseController
    {
        IContainerRepositoryManager _containerManager;

        public ContainerRepositoryController(IContainerRepositoryManager containerManager, UserManager<AppUser> userManager, IAdminLogger logger) : base(userManager, logger)
        {
            _containerManager = containerManager;
        }

        /// <summary>
        /// Container Repo - Add New
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        [HttpPost("/api/container/repo")]
        public Task<InvokeResult> AddSolutionAsync([FromBody] ContainerRepository container)
        {
            return _containerManager.AddContainerRepoAsync(container, OrgEntityHeader, UserEntityHeader);
        }


        /// <summary>
        /// Container Repo - Get Container
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/container/repo/{id}")]
        public async Task<DetailResponse<ContainerRepository>> UpdateContainerAsync(string id)
        {
            var container = await _containerManager.GetContainerRepoAsync(id, OrgEntityHeader, UserEntityHeader);
            var detailResponse = DetailResponse<ContainerRepository>.Create(container);
            detailResponse.View["password"].IsRequired = false;
            return detailResponse;
        }

        /// <summary>
        /// Container Repo - Update Container
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        [HttpPut("/api/container/repo")]
        public Task<InvokeResult> UpdateContainerAsync([FromBody] ContainerRepository container)
        {
            SetUpdatedProperties(container);
            return _containerManager.UpdateContainerRepoAsync(container, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Container Repo - Get Repos for Org
        /// </summary>
        /// <param name="id">Organization Id</param>
        /// <returns></returns>        
        [HttpGet("/api/org/{id}/container/repos")]
        public async Task<ListResponse<ContainerRepositorySummary>> GetContainersForOrgAsync(String id)
        {
            var containerRepos = await _containerManager.GetContainerReposForOrgAsync(id, UserEntityHeader);
            return ListResponse<ContainerRepositorySummary>.Create(containerRepos);
        }

        /// <summary>
        /// Container Repo - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/container/repo/factory")]
        public DetailResponse<ContainerRepository> CreateContainerRepository()
        {
            var response = DetailResponse<ContainerRepository>.Create();
            response.Model.Id = Guid.NewGuid().ToId();
            response.View["password"].IsRequired = true;
            SetAuditProperties(response.Model);
            SetOwnedProperties(response.Model);

            return response;
        }

        /// <summary>
        /// Tagged Container  - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/container/tag/factory")]
        public DetailResponse<ContainerRepository> CreateTaggedContainer()
        {
            return DetailResponse<ContainerRepository>.Create();
        }
    }
}
