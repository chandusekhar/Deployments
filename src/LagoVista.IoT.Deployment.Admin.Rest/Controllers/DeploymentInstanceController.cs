﻿using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Deployment.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Models;
using LagoVista.IoT.Web.Common.Attributes;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Deployment.Admin.Rest.Controllers
{

    /// <summary>
    /// Manage Deployment Instances 
    /// </summary>
    [ConfirmedUser]
    [OrgAdmin]
    [Authorize]
    public class DeploymentInstanceController : LagoVistaBaseController
    {
        IDeploymentInstanceManager _instanceManager;
        public DeploymentInstanceController(IDeploymentInstanceManager instanceManager, UserManager<AppUser> userManager, IAdminLogger logger) : base(userManager, logger)
        {
            _instanceManager = instanceManager;
        }

        /// <summary>
        /// Deployment Instance - Add
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [HttpPost("/api/deployment/instance")]
        public Task<InvokeResult> AddInstanceAsync([FromBody] DeploymentInstance instance)
        {
            return _instanceManager.AddInstanceAsync(instance, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Update
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [HttpPut("/api/deployment/instance")]
        public Task<InvokeResult> UpdateInstanceAsync([FromBody] DeploymentInstance instance)
        {
            SetUpdatedProperties(instance);
            return _instanceManager.UpdateInstanceAsync(instance, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Get for Org
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/deployment/instances")]
        public async Task<ListResponse<DeploymentInstanceSummary>> GetInstancesForOrgAsync()
        {
            var instanceSummaries = await _instanceManager.GetInstanceForOrgAsync(OrgEntityHeader.Id, UserEntityHeader);
            return ListResponse<DeploymentInstanceSummary>.Create(instanceSummaries);
        }

        /// <summary>
        /// Deployment Instance - Get for Org
        /// </summary>
        /// <param name="str" />
        /// <returns></returns>
        [HttpGet("/api/deployment/instances/{str}")]
        public async Task<ListResponse<DeploymentInstanceSummary>> GetInstancesForOrgAsync(string str)
        {
            if (Enum.TryParse<NuvIoTEditions>(str, out NuvIoTEditions edition))
            {
                var instanceSummaries = await _instanceManager.GetInstanceForOrgAsync(edition, OrgEntityHeader.Id, UserEntityHeader);
                return ListResponse<DeploymentInstanceSummary>.Create(instanceSummaries);
            }

            throw new InvalidOperationException($"{str} is not a valid edition");
        }

        /// <summary>
        /// Deployment Instance - Get Status History
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/statushistory")]
        public Task<ListResponse<DeploymentInstanceStatus>> GetDeploymentInstanceStatusHistoryAsync(string id)
        {
            return _instanceManager.GetDeploymentInstanceStatusHistoryAsync(id, OrgEntityHeader, UserEntityHeader, GetListRequestFromHeader());
        }


        /// <summary>
        /// Deployment Instance - Check in Use
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/inuse")]
        public Task<DependentObjectCheckResult> InUseCheck(String id)
        {
            return _instanceManager.CheckInUseAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}")]
        public async Task<DetailResponse<DeploymentInstance>> GetInstanceAsync(String id)
        {
            var deviceInstance = await _instanceManager.GetInstanceAsync(id, OrgEntityHeader, UserEntityHeader);

            var response = DetailResponse<DeploymentInstance>.Create(deviceInstance);

            return response;
        }

        /// <summary>
        /// Deployment Instance - Update Status
        /// </summary>
        /// <returns></returns>
        [HttpPut("/api/deployment/instance/status")]
        public Task<InvokeResult> UpdateInstanceStatus([FromBody] InstanceStatusUpdate statusUpdate)
        {
            return _instanceManager.UpdateInstanceStatusAsync(statusUpdate.Id, statusUpdate.NewStatus, statusUpdate.Deployed, statusUpdate.Version,
                OrgEntityHeader, UserEntityHeader, statusUpdate.Details);
        }

        /// <summary>
        /// Deployment Instance - Get Runtime Status
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/runtime")]
        public Task<InvokeResult<InstanceRuntimeDetails>> GetInstanceRunTimeAsync(String id)
        {
            return _instanceManager.GetInstanceDetailsAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Get connected devices that have been monitored.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/connected/monitored")]
        public Task<ListResponse<WatchdogConnectedDevice>> GetTimedoutDevicesAsync(string id)
        {
            return _instanceManager.GetWatchdogConnectedDevicesAsync(id, OrgEntityHeader, UserEntityHeader, GetListRequestFromHeader());
        }

        /// <summary>
        /// Get connected devices that have timed out.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/connected/timedout")]
        public Task<ListResponse<WatchdogConnectedDevice>> GetWatchdogConnectedDevicesAsync(string id)
        {
            return _instanceManager.GetTimedoutDevicesAsync(id, OrgEntityHeader, UserEntityHeader, GetListRequestFromHeader());
        }

        /// <summary>
        /// Device Message Config - Key In Use
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{key}/keyinuse")]
        public Task<bool> InstanceKeyInUse(String key)
        {
            return _instanceManager.QueryInstanceKeyInUseAsync(key, OrgEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Delete
        /// </summary>
        /// <returns></returns>
        [HttpDelete("/api/deployment/instance/{id}")]
        public Task<InvokeResult> DeleteInstanceAsync(string id)
        {
            return _instanceManager.DeleteInstanceAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        ///  Deploymnent Instance - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/factory")]
        public DetailResponse<DeploymentInstance> CreateInstance()
        {
            var response = DetailResponse<DeploymentInstance>.Create();
            response.Model.Id = Guid.NewGuid().ToId();
            response.Model.DeploymentConfiguration = EntityHeader<DeploymentConfigurations>.Create(DeploymentConfigurations.SingleInstance);
            response.Model.NuvIoTEdition = EntityHeader<NuvIoTEditions>.Create(NuvIoTEditions.Container);
            response.Model.LogStorage = EntityHeader<LogStorage>.Create(LogStorage.Cloud);
            response.Model.WorkingStorage = EntityHeader<WorkingStorage>.Create(WorkingStorage.Cloud);
            response.Model.DeploymentType = EntityHeader<DeploymentTypes>.Create(DeploymentTypes.Managed);
            response.Model.QueueType = EntityHeader<QueueTypes>.Create(QueueTypes.InMemory);
            response.Model.LogStorage = EntityHeader<LogStorage>.Create(LogStorage.Cloud);
            response.Model.PrimaryCacheType = EntityHeader<CacheTypes>.Create(CacheTypes.LocalInMemory);
            response.Model.SharedAccessKey1 = _instanceManager.GenerateAccessKey();
            response.Model.SharedAccessKey2 = _instanceManager.GenerateAccessKey();

            SetAuditProperties(response.Model);
            SetOwnedProperties(response.Model);

            return response;
        }

        /* Methods to manage the instance */

        /// <summary>
        /// Deployment Instance - Deploy
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/deployhost")]
        public Task<InvokeResult> DeployHostAsync(String id)
        {
            return _instanceManager.DeployHostAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Start
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/start")]
        public Task<InvokeResult> StartAsync(String id)
        {
            return _instanceManager.StartAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Reset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/reset")]
        public Task<InvokeResult> ResetAsync(String id)
        {            
            return _instanceManager.ResetAppAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Start
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/pause")]
        public Task<InvokeResult> PauseAsync(String id)
        {
            return _instanceManager.PauseAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Reload Solution
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/reloadsolution")]
        public Task<InvokeResult> RealodSolutionAsync(String id)
        {
            return _instanceManager.ReloadSolutionAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Update Runtime
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/updateruntime")]
        public Task<InvokeResult> UpdateRuntimeAsync(String id)
        {
            return _instanceManager.UpdateRuntimeAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Restart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/restarthost")]
        public Task<InvokeResult> RestartHostAsync(String id)
        {
            return _instanceManager.RestartHostAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Deployment Instance - Reset Container
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/resetartcontainer")]
        public Task<InvokeResult> ResetContainerAsync(String id)
        {
            return _instanceManager.RestartContainerAsync(id, OrgEntityHeader, UserEntityHeader);
        }


        /// <summary>
        /// Deployment Instance - Stop
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/stop")]
        public Task<InvokeResult> StopAsync(String id)
        {
            return _instanceManager.StopAsync(id, OrgEntityHeader, UserEntityHeader);
        }


        /// <summary>
        /// Deployment Instance - Remove
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/destroyhost")]
        public Task<InvokeResult> RemoveAsync(String id)
        {
            return _instanceManager.DestroyHostAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Web Socket URI - Get a URI to Receive Web Socket Notifcations
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="id"></param>
        /// <param name="verbosity"></param>
        /// <returns></returns>
        [HttpGet("/api/wsuri/{channel}/{id}/{verbosity}")]
        public Task<InvokeResult<string>> GetMonitorUriAsync(string channel, string id, string verbosity)
        {
            return _instanceManager.GetRemoteMonitoringURIAsync(channel, id, verbosity, OrgEntityHeader, UserEntityHeader);
        }

       
        /// <summary>
        /// Deployment Instance - Regenreate and update access key.
        /// </summary>
        /// <param name="instanceid"></param>
        /// <param name="key">Currently support either key1 or key2</param>
        /// <returns>Newly Generated Access Key</returns>
        [HttpGet("/api/deployment/instance/{instanceid}/generate/{key}")]
        public Task<InvokeResult<string>> GenrateAccessKey(String instanceid, string key)
        {
            return _instanceManager.RegenerateKeyAsync(instanceid, key, OrgEntityHeader, UserEntityHeader);
        }


        /// <summary>
        /// Get Deployment Settings
        /// </summary>
        /// <param name="instanceid"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{instanceid}/settings")]
        public Task<InvokeResult<DeploymentSettings>> GetDeploymentSettingsAsync(string instanceid)
        {
            return _instanceManager.GetDeploymentSettingsAsync(instanceid, OrgEntityHeader, UserEntityHeader);
        }
    }
}