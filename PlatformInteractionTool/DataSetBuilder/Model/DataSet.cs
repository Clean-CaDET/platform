using System.Collections.Generic;
using System.Linq;

namespace PlatformInteractionTool.DataSetBuilder.Model
{
    internal class DataSet
    {
        private readonly HashSet<DataSetInstance> _instances;

        internal DataSet()
        {
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

        internal List<DataSetInstance> GetInstancesOfType(SnippetType type)
        {
            return _instances.Where(i => i.Type.Equals(type)).ToList();
        }
    }
}
