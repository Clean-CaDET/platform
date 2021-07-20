using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSet
    {
        public int Id { get; private set; }
        public string Url { get; }
        internal readonly HashSet<DataSetInstance> _instances;

        internal DataSet(string dataSetUrl)
        {
            Url = dataSetUrl;
            _instances = new HashSet<DataSetInstance>();
        }

        private DataSet()
        {
        }

        internal void AddInstances(List<DataSetInstance> instances)
        {
            foreach (var instance in instances)
            {
                if (_instances.TryGetValue(instance, out var existingInstance))
                {
                    existingInstance.AddAnnotations(instance);
                } else
                {
                    _instances.Add(instance);
                }
            }
        }

        public List<DataSetInstance> GetInstancesOfType(SnippetType type)
        {
            return _instances.Where(i => i.Type.Equals(type)).ToList();
        }

        public List<DataSetInstance> GetInsufficientlyAnnotatedInstances()
        {
            var instances = new List<DataSetInstance>();

            instances.AddRange(_instances.Where(i => !i.IsSufficientlyAnnotated()));
            
            return instances;
        }

        public List<DataSetInstance> GetInstancesWithAllDisagreeingAnnotations()
        {
            return _instances.Where(i => i.HasNoAgreeingAnnotations()).ToList();
        }

        public List<DataSetInstance> GetAllInstances()
        {
            return _instances.ToList();
        }
    }
}
