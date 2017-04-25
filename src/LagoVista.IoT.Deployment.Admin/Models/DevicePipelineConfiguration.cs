﻿using LagoVista.Core.Models;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.DeviceAdmin.Models;
using System.Collections.Generic;

namespace LagoVista.IoT.Deployment.Admin.Models
{
    public class DevicePipelineModuleConfiguration<TPipelineModule> : EntityHeader<TPipelineModule> where TPipelineModule : IPipelineModuleConfiguration
    {
        public DevicePipelineModuleConfiguration()
        {
            SecondaryPipelineModuleConfigurations = new List<EntityHeader>();
        }

        public EntityHeader PrimaryOutputPipelineModuleConfiguration { get; set; }

        public List<EntityHeader> SecondaryPipelineModuleConfigurations { get; set; }


        public DiagramLocation DiagramLocation { get; set; }
    }
}
