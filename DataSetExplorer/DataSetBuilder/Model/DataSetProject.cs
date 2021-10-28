using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSetProject
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public HashSet<SmellCandidateInstances> CandidateInstances { get; internal set; }
        public ProjectState State { get; private set; }

        internal DataSetProject(string name, string url)
        {
            Name = name;
            Url = url;
            CandidateInstances = new HashSet<SmellCandidateInstances>();
            State = ProjectState.Processing;
        }

        public DataSetProject(string name) : this(name, null) { }

        internal void AddCandidateInstance(SmellCandidateInstances newCandidate)
        {
            if (CandidateInstances.TryGetValue(newCandidate, out var existingCandidate))
            {
                existingCandidate.AddInstances(newCandidate);
            } else
            {
                CandidateInstances.Add(newCandidate);
            }
        }

        public List<SmellCandidateInstances> GetInsufficientlyAnnotatedInstances()
        {
            var insufficientlyAnnotated = new List<SmellCandidateInstances>();
            foreach (var candidate in CandidateInstances)
            {
                var instances = candidate.Instances.Where(i => !i.IsSufficientlyAnnotated()).ToList();
                insufficientlyAnnotated.Add(new SmellCandidateInstances(candidate.CodeSmell, instances));
            }
            return insufficientlyAnnotated;
        }

        public List<SmellCandidateInstances> GetInstancesWithAllDisagreeingAnnotations()
        {
            var noAgreement = new List<SmellCandidateInstances>();
            foreach (var candidate in CandidateInstances)
            {
                var instances = candidate.Instances.Where(i => i.HasNoAgreeingAnnotations()).ToList();
                noAgreement.Add(new SmellCandidateInstances(candidate.CodeSmell, instances));
            }
            return noAgreement;
        }

        public void Processed()
        {
            if (State == ProjectState.Processing) State = ProjectState.Built;
        }

        public void Failed()
        {
            if (State == ProjectState.Processing) State = ProjectState.Failed;
        }
    }
}
