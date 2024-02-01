﻿using System.Collections.Generic;

namespace DataSetExplorer.UI.Controllers.Dataset.DTOs
{
    public class ProjectBuildSettingsDTO
    {
        public int NumOfInstances { get; set; }
        public string NumOfInstancesType { get; set; }
        public bool RandomizeClassSelection { get; set; }
        public bool RandomizeMemberSelection { get; set; }
        public List<string> IgnoredFolders { get; set; }
    }
}
