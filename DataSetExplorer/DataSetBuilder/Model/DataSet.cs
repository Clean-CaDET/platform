using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSet
    {
        public int Id { get; private set; }
        public string Url { get; private set; }
        public HashSet<DataSetInstance> Instances { get; private set; }
        public DataSetState State { get; set; }

        internal DataSet(string dataSetUrl)
        {
            Url = dataSetUrl;
            Instances = new HashSet<DataSetInstance>();
            State = DataSetState.Processing;
        }

        private DataSet()
        {
        }

        internal void AddInstances(List<DataSetInstance> instances)
        {
            foreach (var instance in instances)
            {
                if (Instances.TryGetValue(instance, out var existingInstance))
                {
                    existingInstance.AddAnnotations(instance);
                } else
                {
                    Instances.Add(instance);
                }
            }
        }

        public List<DataSetInstance> GetInstancesOfType(SnippetType type)
        {
            return Instances.Where(i => i.Type.Equals(type)).ToList();
        }

        public List<DataSetInstance> GetInsufficientlyAnnotatedInstances()
        {
            return Instances.Where(i => !i.IsSufficientlyAnnotated()).ToList();
        }

        public List<DataSetInstance> GetInstancesWithAllDisagreeingAnnotations()
        {
            return Instances.Where(i => i.HasNoAgreeingAnnotations()).ToList();
        }

        public List<DataSetInstance> GetAllInstances()
        {
            return Instances.ToList();
        }
    }
}
