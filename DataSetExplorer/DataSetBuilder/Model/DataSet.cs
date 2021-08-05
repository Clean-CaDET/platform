using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSet
    {
        private readonly string _name;
        internal readonly List<DataSetProject> _projects;

        public DataSet(string name)
        {
            _name = name;
            _projects = new List<DataSetProject>();
        }

        private DataSet()
        {
        }

        public void AddProject(DataSetProject project)
        {
            _projects.Add(project);
        }

        public List<DataSetInstance> GetInstancesOfType(SnippetType type, string projectName = null)
        {
            if (!projectName.Equals(null))
            {
                var project = GetProjectByName(projectName);
                return project._instances.Where(i => i.Type.Equals(type)).ToList();
            }
            return _projects.SelectMany(p => p._instances.Where(i => i.Type.Equals(type))).ToList();
        }

        private DataSetProject GetProjectByName(string name)
        {
            return _projects.First(p => p._name.Equals(name));
        }
    }
}
