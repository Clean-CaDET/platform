using System.Collections.Generic;
using DataSetExplorer.Core.Annotations.Model;

namespace DataSetExplorer.Core.DataSets.Model
{
    public class SmellCandidateInstances
    {
        public int Id { get; private set; }
        public CodeSmell CodeSmell { get; private set; }
        public List<Instance> Instances { get; private set; }

        internal SmellCandidateInstances(CodeSmell codeSmell, List<Instance> instances)
        {
            CodeSmell = codeSmell;
            Instances = instances;
        }

        private SmellCandidateInstances() { }

        public bool HasInstanceWithCodeSnippetId(string codeSnippetId)
        {
            return Instances.Exists(i => i.CodeSnippetId.Equals(codeSnippetId));
        }

        public Instance GetInstanceWithCodeSnippetId(string codeSnippetId)
        {
            return Instances.Find(i => i.CodeSnippetId.Equals(codeSnippetId));
        }

        public override int GetHashCode()
        {
            if (CodeSmell != null) return CodeSmell.GetHashCode();
            return base.GetHashCode();
        }

        internal void AddInstances(SmellCandidateInstances newCandidate)
        {
            foreach (var instance in newCandidate.Instances)
            {
                if (HasInstanceWithCodeSnippetId(instance.CodeSnippetId))
                {
                    GetInstanceWithCodeSnippetId(instance.CodeSnippetId).AddAnnotations(instance);
                }
                else
                {
                    Instances.Add(instance);
                }
            }
        }

        public override bool Equals(object other)
        {
            return other is SmellCandidateInstances candidateInstances && CodeSmell.Equals(candidateInstances.CodeSmell);
        }
    }
}
