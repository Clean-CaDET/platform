using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSetProject
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public List<CandidateDataSetInstance> CandidateInstances { get; private set; }
        public DataSetProjectState State { get; private set; }

        internal DataSetProject(string name, string url)
        {
            Name = name;
            Url = url;
            CandidateInstances = new List<CandidateDataSetInstance>();
            State = DataSetProjectState.Processing;
        }

        public DataSetProject(string name) : this(name, null) { }

        internal void AddCandidateInstance(CandidateDataSetInstance newCandidate)
        {
            var i = CandidateInstances.FindIndex(c => c.CodeSmell.Name.Equals(newCandidate.CodeSmell.Name));
            if (i != -1) AddInstances(i, newCandidate);
            else CandidateInstances.Add(newCandidate);
        }

        private void AddInstances(int i, CandidateDataSetInstance candidate)
        {
            foreach(var instance in candidate.Instances)
            {
                if (CandidateInstances[i].HasInstanceWithCodeSnippetId(instance.CodeSnippetId))
                {
                    CandidateInstances[i].GetInstanceWithCodeSnippetId(instance.CodeSnippetId).AddAnnotations(instance);
                } else
                {
                    CandidateInstances[i].Instances.Add(instance);
                }
            }
        }

        public List<CandidateDataSetInstance> GetInsufficientlyAnnotatedInstances()
        {
            var insufficientlyAnnotated = new List<CandidateDataSetInstance>();
            foreach (var candidate in CandidateInstances)
            {
                var instances = candidate.Instances.Where(i => !i.IsSufficientlyAnnotated()).ToList();
                insufficientlyAnnotated.Add(new CandidateDataSetInstance(candidate.CodeSmell, instances));
            }
            return insufficientlyAnnotated;
        }

        public List<CandidateDataSetInstance> GetInstancesWithAllDisagreeingAnnotations()
        {
            var noAgreement = new List<CandidateDataSetInstance>();
            foreach (var candidate in CandidateInstances)
            {
                var instances = candidate.Instances.Where(i => i.HasNoAgreeingAnnotations()).ToList();
                noAgreement.Add(new CandidateDataSetInstance(candidate.CodeSmell, instances));
            }
            return noAgreement;
        }

        public void Processed()
        {
            if (State == DataSetProjectState.Processing) State = DataSetProjectState.Built;
        }

        public void Failed()
        {
            if (State == DataSetProjectState.Processing) State = DataSetProjectState.Failed;
        }

        internal void SetCandidateInstances(List<CandidateDataSetInstance> candidateInstances)
        {
            CandidateInstances = candidateInstances;
        }
    }
}
