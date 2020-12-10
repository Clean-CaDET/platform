using System.Collections.Generic;
using System.Linq;

namespace PlatformInteractionTool.DataSetBuilder.Model
{
    internal class DataSet
    {
        private readonly List<DataSetInstance> _instances;

        internal DataSet()
        {
            _instances = new List<DataSetInstance>();
        }

        internal void AddInstances(List<DataSetInstance> instances)
        {
            _instances.AddRange(instances);
        }

        internal List<DataSetInstance> GetInstancesOfType(SnippetType type)
        {
            return _instances.Where(i => i.Type.Equals(type)).ToList();
        }
    }
}
