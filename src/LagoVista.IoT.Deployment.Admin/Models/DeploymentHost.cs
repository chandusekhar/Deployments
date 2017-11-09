﻿using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Validation;
using LagoVista.Core;
using LagoVista.IoT.Deployment.Admin.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core.Models;
using Newtonsoft.Json;

namespace LagoVista.IoT.Deployment.Admin.Models
{

    public enum HostTypes
    {
        [EnumLabel(DeploymentHost.HostType_Free, DeploymentAdminResources.Names.Host_Type_Free, typeof(DeploymentAdminResources))]
        Free,
        [EnumLabel(DeploymentHost.HostType_Community, DeploymentAdminResources.Names.Host_Type_Community, typeof(DeploymentAdminResources))]
        Community,
        [EnumLabel(DeploymentHost.HostType_Shared, DeploymentAdminResources.Names.Host_Type_Shared, typeof(DeploymentAdminResources))]
        Shared,
        [EnumLabel(DeploymentHost.HostType_SharedHighPerformacne, DeploymentAdminResources.Names.Host_Type_SharedHighPerformance, typeof(DeploymentAdminResources))]
        SharedHighPerformance,
        [EnumLabel(DeploymentHost.HostType_Dedicated, DeploymentAdminResources.Names.Host_Type_Dedicated, typeof(DeploymentAdminResources))]
        Dedicated,
        [EnumLabel(DeploymentHost.HostType_Clustered, DeploymentAdminResources.Names.Host_Type_Clustered, typeof(DeploymentAdminResources))]
        Clustered,
        [EnumLabel(DeploymentHost.HostType_MCP, DeploymentAdminResources.Names.Host_Type_MCP, typeof(DeploymentAdminResources))]
        MCP,
        [EnumLabel(DeploymentHost.HostType_Notifications, DeploymentAdminResources.Names.Host_Type_Notifications, typeof(DeploymentAdminResources))]
        Notification,
        [EnumLabel(DeploymentHost.HostType_Development, DeploymentAdminResources.Names.HostType_Development, typeof(DeploymentAdminResources))]
        Development

    }

    public enum HostStatus
    {
        [EnumLabel(DeploymentHost.HostStatus_Offline, DeploymentAdminResources.Names.HostStatus_Offline, typeof(DeploymentAdminResources))]
        Offline,

        [EnumLabel(DeploymentHost.HostStatus_Deploying, DeploymentAdminResources.Names.HostStatus_Deploying, typeof(DeploymentAdminResources))]
        Deploying,

        [EnumLabel(DeploymentHost.HostStatus_PendingDNSConfiguration, DeploymentAdminResources.Names.HostStatus_PendingDNSConfiguration, typeof(DeploymentAdminResources))]
        PendingDNSConfiguration,

        [EnumLabel(DeploymentHost.HostStatus_DeployingContainer, DeploymentAdminResources.Names.HostStatus_DeployingContainer, typeof(DeploymentAdminResources))]
        DeployingContainer,

        [EnumLabel(DeploymentHost.HostStatus_FailedDeployment, DeploymentAdminResources.Names.HostStatus_FailedDeployment, typeof(DeploymentAdminResources))]
        FailedDeployment,
        [EnumLabel(DeploymentHost.HostStatus_HealthCheckFailed, DeploymentAdminResources.Names.HostStatus_HealthCheckFailed, typeof(DeploymentAdminResources))]
        HostHealthCheckFailed,        
        
        [EnumLabel(DeploymentHost.HostStatus_Running, DeploymentAdminResources.Names.HostStatus_Running, typeof(DeploymentAdminResources))]
        Running,

        [EnumLabel(DeploymentHost.HostStatus_Destorying, DeploymentAdminResources.Names.HostStatus_Destroying, typeof(DeploymentAdminResources))]
        Destroying,

        [EnumLabel(DeploymentHost.HostStatus_Restarting, DeploymentAdminResources.Names.HostStatus_Restarting, typeof(DeploymentAdminResources))]
        Restarting,

        /*        [EnumLabel(DeploymentHost.HostStatus_Stopped, DeploymentAdminResources.Names.HostStatus_Stopped, typeof(DeploymentAdminResources))]
                Stopped,
                [EnumLabel(DeploymentHost.HostStatus_Stopping, DeploymentAdminResources.Names.HostStatus_Stopping, typeof(DeploymentAdminResources))]
                Stopping, 
                [EnumLabel(DeploymentHost.HostStatus_Starting, DeploymentAdminResources.Names.HostStatus_Starting, typeof(DeploymentAdminResources))]
                Starting, */
    }

    public enum HostCapacityStatus
    {
        [EnumLabel(DeploymentHost.HostCapacity_underutilized, DeploymentAdminResources.Names.HostCapacity_Underutlized, typeof(DeploymentAdminResources))]
        UnderUtilized,
        [EnumLabel(DeploymentHost.HostCapacity_Ok, DeploymentAdminResources.Names.HostCapacity_OverCapacity, typeof(DeploymentAdminResources))]
        Ok,
        [EnumLabel(DeploymentHost.HostCapacity_75Percent, DeploymentAdminResources.Names.HostCapacity_75Percent, typeof(DeploymentAdminResources))]
        At75Percent,
        [EnumLabel(DeploymentHost.HostCapacity_90Percent, DeploymentAdminResources.Names.HostCapacity_90Percent, typeof(DeploymentAdminResources))]
        At90Percent,
        [EnumLabel(DeploymentHost.HostCapacity_atcapacity, DeploymentAdminResources.Names.HostCapacity_AtCapacity, typeof(DeploymentAdminResources))]
        AtCapacity,
        [EnumLabel(DeploymentHost.HostCapacity_overcapacity, DeploymentAdminResources.Names.HostCapacity_OverCapacity, typeof(DeploymentAdminResources))]
        OverCapacity,
        [EnumLabel(DeploymentHost.HostCapacity_failureimmintent, DeploymentAdminResources.Names.HostCapacity_FailureImminent, typeof(DeploymentAdminResources))]
        FailureImminent
    }

   
    [EntityDescription(DeploymentAdminDomain.DeploymentAdmin, DeploymentAdminResources.Names.Host_Title, Resources.DeploymentAdminResources.Names.Host_Help, Resources.DeploymentAdminResources.Names.Host_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(DeploymentAdminResources))]
    public class DeploymentHost : LagoVista.IoT.DeviceAdmin.Models.IoTModelBase, IOwnedEntity, IValidateable, IKeyedEntity, INoSQLEntity, IFormDescriptor
    {
        public const string HostSize_ExtraSmall = "extrasmall";
        public const string HostSize_Small = "small";
        public const string HostSize_Medium = "medium";
        public const string HostSize_Large = "large";
        public const string HostSize_ExtraLarge = "extralarge";

        public const string HostType_Free = "free";
        public const string HostType_Community = "community";
        public const string HostType_Shared = "shared";
        public const string HostType_SharedHighPerformacne = "shared_highperformance";
        public const string HostType_Dedicated = "dedicated";
        public const string HostType_Clustered = "clustered";
        public const string HostType_Notifications = "notifications";
        public const string HostType_MCP = "mcp";
        public const string HostType_Development = "development";


        public const string HostStatus_Offline = "offline";
        public const string HostStatus_Deploying = "deploying";

        public const string HostStatus_PendingDNSConfiguration = "pending-dns-Configuration";
        public const string HostStatus_DeployingContainer = "deployingcontainer";

        public const string HostStatus_Starting = "starting";
        public const string HostStatus_Running = "running";
        public const string HostStatus_Stopping = "stopping";
        public const string HostStatus_Restarting = "restarting";

        public const string HostStatus_Destorying = "destroying";

        public const string HostStatus_FailedDeployment = "faileddeployment";
        public const string HostStatus_HealthCheckFailed = "healthcheckfailed";
       
        

        public const string HostCapacity_underutilized = "underutilized";
        public const string HostCapacity_Ok = "ok";
        public const string HostCapacity_75Percent = "75percent";
        public const string HostCapacity_90Percent = "90percent";

        public const string HostCapacity_atcapacity = "atcapacity";
        public const string HostCapacity_overcapacity = "overcapacity";
        public const string HostCapacity_failureimmintent = "failureimminent";

        public DeploymentHost()
        {
            CapacityStatus = EntityHeader<HostCapacityStatus>.Create(HostCapacityStatus.UnderUtilized);
            Status = EntityHeader<HostStatus>.Create(HostStatus.Offline);

            CloudProvider = new EntityHeader() { Text = "Digital Ocean", Id = "378463ADF57B4C02B60FEF4DCB30F7E2" };
            GenerateAccessKeys();
        }
        
        public void GenerateAccessKeys()
        {
            HostAccessKey1 = Guid.NewGuid().ToId() + Guid.NewGuid().ToId();
            HostAccessKey2 = Guid.NewGuid().ToId() + Guid.NewGuid().ToId();
        }

        public string DatabaseName { get; set; }
        public string EntityType { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Common_Key, HelpResource: Resources.DeploymentAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: Resources.DeploymentAdminResources.Names.Common_Key_Validation, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public string Key { get; set; }


        [FormField(LabelResource: DeploymentAdminResources.Names.Host_Type, EnumType: (typeof(HostTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(DeploymentAdminResources), WaterMark: DeploymentAdminResources.Names.Host_Type_Select, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<HostTypes> HostType { get; set; }


        [FormField(LabelResource: DeploymentAdminResources.Names.Host_Size, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), WaterMark: DeploymentAdminResources.Names.Host_SelectSize, IsRequired: true)]
        public EntityHeader Size { get; set; }


        EntityHeader<HostStatus> _status;
        [FormField(LabelResource: DeploymentAdminResources.Names.Host_Status, EnumType: (typeof(HostStatus)), FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), WaterMark: DeploymentAdminResources.Names.Host_Type_Select, IsRequired: true, IsUserEditable: false)]
        public EntityHeader<HostStatus> Status
        {
            get { return _status; }
            set
            {
                _status = value;
                StatusTimeStamp = DateTime.UtcNow.ToJSONString();
            }
        }

        [FormField(LabelResource: DeploymentAdminResources.Names.Instance_StatusTimeStamp, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public string StatusTimeStamp { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Host_CapacityStatus, EnumType: (typeof(HostCapacityStatus)), FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), WaterMark: DeploymentAdminResources.Names.Host_Type_Select, IsRequired: false, IsUserEditable: false)]
        public EntityHeader<HostCapacityStatus> CapacityStatus { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_DedicatedInstance, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public EntityHeader DedicatedInstance { get; set; }

        public bool IsPublic { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_ContainerRepository, WaterMark: Resources.DeploymentAdminResources.Names.Host_ContainerRepository_Select, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public EntityHeader ContainerRepository { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_ContainerTag, WaterMark: Resources.DeploymentAdminResources.Names.Host_ContainerTag_Select, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public EntityHeader ContainerTag { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_DNSName, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public string DnsHostName { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_IPv4_Address, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public string Ipv4Address { get; set; }

        public string DNSEntryId { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_CloudProvider, HelpResource: Resources.DeploymentAdminResources.Names.Host_CloudProvider_Help, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false, IsRequired: true)]
        public EntityHeader CloudProvider { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_AdminAPIUri, HelpResource: Resources.DeploymentAdminResources.Names.Host_AdminAPIUri_Help, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false, IsRequired: false)]
        public string AdminAPIUri { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_MonitoringURI, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false, IsRequired: false)]
        public string MonitoringURI { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_MonitoringProvider, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false, IsRequired: false)]
        public EntityHeader MonitoringProvider { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_UpSince, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsRequired: false, IsUserEditable: false)]
        public string UpSince { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_ComputeResourceId, HelpResource: Resources.DeploymentAdminResources.Names.Host_ComputeResourceId_Help, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false, IsRequired: false)]
        public string ComputeResourceId { get; set; }


        

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_LastPing, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public string LastPing { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_AverageCPU_1_Minute, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public string AverageCPU { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_AverageMemory_1_Minute, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public string AverageMemory { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_Subscription, WaterMark: Resources.DeploymentAdminResources.Names.Host_SubscriptionSelect, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: true, IsRequired: true)]
        public EntityHeader Subscription { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_StatusDetails, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public string StatusDetails { get; set; }

        public String HostAccessKey1 { get; set; }

        public String HostAccessKey2 { get; set; }

        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_DebugMode, HelpResource: Resources.DeploymentAdminResources.Names.Host_DebugMode_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: true)]
        public bool DebugMode { get; set; }


        [FormField(LabelResource: Resources.DeploymentAdminResources.Names.Host_ShowSiteDetails, HelpResource:Resources.DeploymentAdminResources.Names.Host_ShowSiteDetails_Help, FieldType: FieldTypes.CheckBox, ResourceType: typeof(DeploymentAdminResources), IsUserEditable: false)]
        public bool ShowSolutionDetailsSite { get; set; }

        public DeploymentHostSummary CreateSummary()
        {
            return new DeploymentHostSummary()
            {
                Id = Id,
                Name = Name,
                Status = Status.Text,
                CapacityStatus = CapacityStatus.Text,
                HostType = HostType.Text,
                Key = Key,
                Description = Description,
                IsPublic = IsPublic
            };
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(DeploymentHost.Name),
                nameof(DeploymentHost.Key),
                nameof(DeploymentHost.Status),
                nameof(DeploymentHost.HostType),
                nameof(DeploymentHost.Size),
                nameof(DeploymentHost.DnsHostName),
                nameof(DeploymentHost.Ipv4Address),
                nameof(DeploymentHost.Subscription),
                nameof(DeploymentHost.CloudProvider),
                nameof(DeploymentHost.ContainerRepository),
                nameof(DeploymentHost.ContainerTag),
            };
        }
    }

    public class DeploymentHostSummary : SummaryData
    {
        [ListColumn(HeaderResource: DeploymentAdminResources.Names.Host_Type, ResourceType: typeof(DeploymentAdminResources))]
        public string HostType { get; set; }


        [ListColumn(HeaderResource: DeploymentAdminResources.Names.Host_Status, ResourceType: typeof(DeploymentAdminResources))]
        public string Status { get; set; }

        [ListColumn(HeaderResource: DeploymentAdminResources.Names.Host_CapacityStatus, ResourceType: typeof(DeploymentAdminResources))]
        public string CapacityStatus { get; set; }
    }

}
