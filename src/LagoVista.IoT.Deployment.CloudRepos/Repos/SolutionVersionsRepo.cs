﻿using LagoVista.IoT.Deployment.Admin.Repos;
using System;
using System.Collections.Generic;
using LagoVista.IoT.Deployment.Admin.Models;
using System.Threading.Tasks;
using LagoVista.CloudStorage.Storage;
using LagoVista.IoT.Logging.Loggers;
using Microsoft.WindowsAzure.Storage.Blob;
using LagoVista.Core.Interfaces;
using Newtonsoft.Json;
using LagoVista.Core;
using System.Linq;
using System.IO;
using LagoVista.Core.Validation;

namespace LagoVista.IoT.Deployment.CloudRepos.Repos
{
    public class SolutionVersionsRepo : TableStorageBase<SolutionVersion>, ISolutionVersionRepo
    {
        IConnectionSettings _repoSettings;
        IAdminLogger _adminLogger;

        public SolutionVersionsRepo(IDeploymentRepoSettings repoSettings, IAdminLogger adminLogger) :
            base(repoSettings.DeploymentAdminTableStorage.AccountId, repoSettings.DeploymentAdminTableStorage.AccessKey, adminLogger)
        {
            _repoSettings = repoSettings.DeploymentAdminTableStorage;
            _adminLogger = adminLogger;
        }

        private const string CONTAINER_ID = "solutions";

        private CloudBlobClient CreateBlobClient(string solutionId)
        {
            var baseuri = $"https://{_repoSettings.AccountId}.blob.core.windows.net";

            var uri = new Uri(baseuri);
            return new CloudBlobClient(uri, new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(_repoSettings.AccountId, _repoSettings.AccessKey));
        }

        public async Task<InvokeResult<Solution>> GetSolutionVersionAsync(string solutionId, string versionId)
        {
            try
            {
                var cloudClient = CreateBlobClient(solutionId);
                var primaryContainer = cloudClient.GetContainerReference(CONTAINER_ID);

                var blobName = GetBlobName(solutionId, versionId);
                var blob = primaryContainer.GetBlobReference(blobName);
                
                using (var ms = new MemoryStream())
                {
                    await blob.DownloadToStreamAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    using (var rdr = new StreamReader(ms))
                    using (var jsonTextReader = new JsonTextReader(rdr))
                    {
                        var serializer = JsonSerializer.Create(_jsonSettings);
                        var solution = serializer.Deserialize<Solution>(jsonTextReader);
                        if (solution == null)
                        {
                            return InvokeResult<Solution>.FromError("Could not deserialize solution.");
                        }
                        else
                        {
                            return InvokeResult<Solution>.Create(solution);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception deserializeing: " + ex.Message);
                return InvokeResult<Solution>.FromError(ex.Message);
            }
        }

        private string GetBlobName(string solutionId, string versionId)
        {
            return $"{solutionId.ToLower()}/{versionId.ToLower()}.solution";
        }

        public Task<IEnumerable<SolutionVersion>> GetSolutionVersionsAsync(string solutionId)
        {
            return GetByParitionIdAsync(solutionId);
        }

        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            // note: do not change this to auto or array or all - only works with TypeNameHandling.Objects
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Newtonsoft.Json.Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
        };

        public async Task<SolutionVersion> PublishSolutionVersionAsync(SolutionVersion solutionVersion, Solution solution)
        {
            solutionVersion.RowKey = Guid.NewGuid().ToId();
            solutionVersion.PartitionKey = solution.Id;
            solutionVersion.TimeStamp = DateTime.UtcNow.ToJSONString();
            solutionVersion.Uri = $"https://{_repoSettings.AccountId}.blob.core.windows.net/{CONTAINER_ID}/{GetBlobName(solutionVersion.SolutionId,solutionVersion.RowKey)}";

            if (string.IsNullOrEmpty(solutionVersion.Status))
            {
                solutionVersion.Status = "New";
            }
           
            if (solutionVersion.Version == 0)
            {
                var versions = await GetSolutionVersionsAsync(solution.Id);
                if (versions.Any())
                {
                    solutionVersion.Version = versions.Max(ver => ver.Version) + 1;
                }
                else
                {
                    solutionVersion.Version = 1.0;
                }
            }

            var cloudClient = CreateBlobClient(solution.Id);
            var primaryContainer = cloudClient.GetContainerReference(CONTAINER_ID);
            await primaryContainer.CreateIfNotExistsAsync();
            var blob = primaryContainer.GetBlockBlobReference(GetBlobName(solutionVersion.SolutionId, solutionVersion.RowKey));
            var json = JsonConvert.SerializeObject(solution, _jsonSettings);
            await blob.UploadTextAsync(json);
            await InsertAsync(solutionVersion);
            return solutionVersion;
        }

        public async Task UpdateSolutionVersionStatusAsync(string solutionId, string versionId, string newStatus)
        {
            var solutionVersion = await GetAsync(solutionId, versionId);
            solutionVersion.Status = newStatus;
            await UpdateAsync(solutionVersion);
        }
    }
}
