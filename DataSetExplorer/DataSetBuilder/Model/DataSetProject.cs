using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSetProject
    {
        internal readonly string _name;
        internal readonly string _url;
        internal readonly HashSet<DataSetInstance> _instances;

        internal DataSetProject(string name, string url)
        {
            _name = name;
            _url = url;
            _instances = new HashSet<DataSetInstance>();
        }

        public DataSetProject(string name) : this(name, null) { }

        internal void AddInstances(List<DataSetInstance> instances)
        {
            foreach (var instance in instances)
            {
                if (_instances.TryGetValue(instance, out var existingInstance))
                {
                    existingInstance.AddAnnotations(instance);
                }
                else
                {
                    _instances.Add(instance);
                }
            }
        }

        public List<DataSetInstance> GetInsufficientlyAnnotatedInstances(string projectName = null)
        {
            return _instances.Where(i => !i.IsSufficientlyAnnotated()).ToList();
        }

        public List<DataSetInstance> GetInstancesWithAllDisagreeingAnnotations(string projectName = null)
        {
            return _instances.Where(i => i.HasNoAgreeingAnnotations()).ToList();
        }
    }
}
