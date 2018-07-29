﻿using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Models.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.IoT.Deployment.Admin.Models
{
    [EntityDescription(DeploymentAdminDomain.DeploymentAdmin, DeploymentAdminResources.Names.ClientApp_Title, DeploymentAdminResources.Names.ClientApp_Help, DeploymentAdminResources.Names.ClientApp_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(DeploymentAdminResources))]
    public class ClientApp : LagoVista.IoT.DeviceAdmin.Models.IoTModelBase, IOwnedEntity, IValidateable, IKeyedEntity, INoSQLEntity, IFormDescriptor
    {
        public ClientApp()
        {
            DeviceTypes = new ObservableCollection<EntityHeader>();
            DeviceConfigurations = new ObservableCollection<EntityHeader>();
        }

        public string DatabaseName { get; set; }
        public string EntityType { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Common_Key, HelpResource: DeploymentAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: DeploymentAdminResources.Names.Common_Key_Validation, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public String Key { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.Common_IsPublic, FieldType: FieldTypes.Bool, ResourceType: typeof(DeploymentAdminResources))]
        public bool IsPublic { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }


        [FormField(LabelResource: DeploymentAdminResources.Names.ClientApp_AppAuthKey1, FieldType: FieldTypes.Text,ResourceType: typeof(DeploymentAdminResources), IsRequired: true, IsUserEditable:false)]
        public String AppAuthKeyPrimary { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.ClientApp_AppAuthKey2, FieldType: FieldTypes.Text, ResourceType: typeof(DeploymentAdminResources), IsRequired: true, IsUserEditable:false)]
        public String AppAuthKeySecondary { get; set; }

        public EntityHeader ToEntityHeader()
        {
            return new EntityHeader()
            {
                Id = Id,
                Text = Name,
            };
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(ClientApp.Name),
                nameof(ClientApp.Key),
                nameof(ClientApp.Description),
                nameof(ClientApp.DeploymentInstance),
                nameof(ClientApp.DeviceTypes),
                nameof(ClientApp.DeviceConfigurations),
            };
        }

        public ClientAppSummary CreateSummary()
        {
            return new ClientAppSummary()
            {
                Description = Description,
                Id = Id,
                IsPublic = IsPublic,
                Key = Key,
                Name = Name,
                InstanceId = DeploymentInstance.Id,
                InstanceName = DeploymentInstance.Text                
            };
        }

        [FormField(LabelResource: DeploymentAdminResources.Names.ClientApp_Instance, FieldType: FieldTypes.EntityHeaderPicker, WaterMark:DeploymentAdminResources.Names.ClientApp_SelectInstance, ResourceType: typeof(DeploymentAdminResources), IsRequired: true)]
        public EntityHeader DeploymentInstance { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.ClientApp_DeviceTypes, FieldType: FieldTypes.ChildList, ResourceType: typeof(DeploymentAdminResources))]
        public ObservableCollection<EntityHeader> DeviceTypes { get; set; }

        [FormField(LabelResource: DeploymentAdminResources.Names.ClientApp_DeviceConfigs, FieldType: FieldTypes.ChildList, ResourceType: typeof(DeploymentAdminResources))]
        public ObservableCollection<EntityHeader> DeviceConfigurations { get; set; }       
    }

    public class ClientAppSummary : SummaryData
    {
        public string InstanceId { get; set; }
        public string InstanceName { get; set; }
    }
}
