using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSet
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public HashSet<DataSetProject> Projects { get; private set; }
        
        public DataSet(string name)
        {
            Name = name;
            Projects = new HashSet<DataSetProject>();
        }

        private DataSet()
        {
        }

        public void AddProject(DataSetProject project)
        {
            Projects.Add(project);
        }

        public List<DataSetInstance> GetInstancesOfType(SnippetType type, string projectName = null)
        {
            if (!projectName.Equals(null))
            {
                var project = Projects.First(p => p.Name == projectName);
                return project.Instances.Where(i => i.Type.Equals(type)).ToList();
            }
            return Projects.SelectMany(p => p.Instances.Where(i => i.Type.Equals(type))).ToList();
        }

        public List<DataSetInstance> GetInstancesWithAllDisagreeingAnnotations()
        {
            var instances = new List<DataSetInstance>();
            foreach (var project in Projects) instances.AddRange(project.GetInstancesWithAllDisagreeingAnnotations());
            return instances;
        }

        public List<DataSetInstance> GetInsufficientlyAnnotatedInstances()
        {
            var instances = new List<DataSetInstance>();
            foreach (var project in Projects) instances.AddRange(project.GetInsufficientlyAnnotatedInstances());
            return instances;
        }
    }
}
