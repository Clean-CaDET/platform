using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSet
    {
        public string Url { get; }
        private readonly HashSet<DataSetInstance> _instances;

        internal DataSet(string dataSetUrl)
        {
            Url = dataSetUrl;
            _instances = new HashSet<DataSetInstance>();
        }

        internal void AddInstances(List<DataSetInstance> instances)
        {
            foreach (var instance in instances)
            {
                if (_instances.TryGetValue(instance, out DataSetInstance existingInstance))
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

        public List<DataSetInstance> GetInstancesForCrossValidation()
        {
            var instances = new List<DataSetInstance>();

            instances.AddRange(_instances.Where(i => !i.IsSufficientlyAnnotated()));
            
            return instances;
        }
    }
}
