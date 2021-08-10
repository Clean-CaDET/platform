using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSetProject
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public HashSet<DataSetInstance> Instances { get; private set; }
        public DataSetProjectState State { get; private set; }

        internal DataSetProject(string name, string url)
        {
            Name = name;
            Url = url;
            Instances = new HashSet<DataSetInstance>();
            State = DataSetProjectState.Processing;
        }

        public DataSetProject(string name) : this(name, null) { }

        internal void AddInstances(List<DataSetInstance> instances)
        {
            foreach (var instance in instances)
            {
                if (Instances.TryGetValue(instance, out var existingInstance))
                {
                    existingInstance.AddAnnotations(instance);
                }
                else
                {
                    Instances.Add(instance);
                }
            }
        }

        public List<DataSetInstance> GetInsufficientlyAnnotatedInstances(string projectName = null)
        {
            return Instances.Where(i => !i.IsSufficientlyAnnotated()).ToList();
        }

        public List<DataSetInstance> GetInstancesWithAllDisagreeingAnnotations(string projectName = null)
        {
            return Instances.Where(i => i.HasNoAgreeingAnnotations()).ToList();
        }

        public void Processed()
        {
            if (State == DataSetProjectState.Processing) State = DataSetProjectState.Built;
        }

        public void Failed()
        {
            if (State == DataSetProjectState.Processing) State = DataSetProjectState.Failed;
        }
    }
}
