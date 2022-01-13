using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel;
using CodeModel.CaDETModel;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.Core.DataSets
{
    internal class CaDETToDataSetProjectBuilder
    {
        private readonly string _projectAndCommitUrl;
        private readonly string _projectName;

        private readonly CaDETProject _cadetProject;
        private int _percentileOfProjectCovered = 100;
        private int _numberOfProjectInstancesCovered;

        private readonly bool _includeClasses;
        private bool _randomizeClassList;
        
        private readonly bool _includeMembers;
        private bool _randomizeMemberList;
        private CaDETMemberType[] _acceptedMemberTypes = {CaDETMemberType.Constructor, CaDETMemberType.Method};
        private readonly List<CodeSmell> _codeSmells;
        private readonly InstanceFilter _instanceFilter;
        private Dictionary<CaDETClass, List<CoupledClassStrength>> _classCouplings = new Dictionary<CaDETClass, List<CoupledClassStrength>>();

        internal CaDETToDataSetProjectBuilder(InstanceFilter instanceFilter, string projectAndCommitUrl, string projectName, string projectPath, LanguageEnum language, bool includeClasses, bool includeMembers, List<CodeSmell> codeSmells)
        {
            _instanceFilter = instanceFilter;
            _projectAndCommitUrl = projectAndCommitUrl;
            _projectName = projectName;
            _cadetProject = new CodeModelFactory(language).CreateProjectWithCodeFileLinks(projectPath);
            _includeClasses = includeClasses;
            _includeMembers = includeMembers;
            _codeSmells = codeSmells;
        }

        internal CaDETToDataSetProjectBuilder(InstanceFilter instanceFilter, string projectAndCommitUrl, string projectName, string projectPath, List<CodeSmell> codeSmells): this(instanceFilter, projectAndCommitUrl, projectName, projectPath, LanguageEnum.CSharp, true, true, codeSmells) { }

        internal CaDETToDataSetProjectBuilder SetProjectExtractionPercentile(int percentile)
        {
            _percentileOfProjectCovered = percentile;
            return this;
        }

        internal CaDETToDataSetProjectBuilder SetProjectInstancesExtractionNumber(int number)
        {
            _numberOfProjectInstancesCovered = number;
            return this;
        }

        internal CaDETToDataSetProjectBuilder RandomizeClassSelection()
        {
            ValidateClassesIncluded();
            _randomizeClassList = true;
            return this;
        }

        private void ValidateClassesIncluded()
        {
            if (!_includeClasses) throw new InvalidOperationException("Classes are not included.");
        }

        internal CaDETToDataSetProjectBuilder IncludeMemberTypes(CaDETMemberType[] acceptedTypes)
        {
            ValidateMembersIncluded();
            _acceptedMemberTypes = acceptedTypes;
            return this;
        }

        private void ValidateMembersIncluded()
        {
            if (!_includeMembers) throw new InvalidOperationException("Members are not included.");
        }

        internal CaDETToDataSetProjectBuilder RandomizeMemberSelection()
        {
            ValidateMembersIncluded();
            _randomizeMemberList = true;
            return this;
        }

        internal DataSetProject Build()
        {
            var builtDataSetProject = new DataSetProject(_projectName, _projectAndCommitUrl);

            foreach (var smell in _codeSmells)
            {
                if (_includeClasses) builtDataSetProject.AddCandidateInstance(new SmellCandidateInstances(smell, _instanceFilter.FilterInstances(smell, BuildClasses(), _numberOfProjectInstancesCovered)));
                if (_includeMembers) builtDataSetProject.AddCandidateInstance(new SmellCandidateInstances(smell, _instanceFilter.FilterInstances(smell, BuildMembers(), _numberOfProjectInstancesCovered)));
            }
            return builtDataSetProject;
        }

        private List<Instance> BuildClasses()
        {
            var cadetClasses = _cadetProject.Classes;
            if(_randomizeClassList) ShuffleList(cadetClasses);
            if(_percentileOfProjectCovered < 100) cadetClasses = cadetClasses.Take(DetermineNumberOfInstances(_cadetProject.Classes.Count)).ToList();
            return CaDETToDataSetProjectClasses(cadetClasses);
        }

        private int DetermineNumberOfInstances(int totalNumber)
        {
            return totalNumber * _percentileOfProjectCovered / 100;
        }

        private List<Instance> CaDETToDataSetProjectClasses(List<CaDETClass> cadetClasses)
        {
            CreateCouplingMap(cadetClasses);
            return cadetClasses.Select(c => new Instance(
                    c.FullName, GetCodeUrl(c.FullName), _projectAndCommitUrl, SnippetType.Class,
                    _cadetProject.GetMetricsForCodeSnippet(c.FullName), FindClassRelatedInstances(c)
                )).ToList();
        }

        private void CreateCouplingMap(List<CaDETClass> cadetClasses)
        {
            foreach (var instance in cadetClasses)
            {   
                foreach (var coupledClass in GetReferencedInstances(instance))
                {
                    AddCoupledClassToMap(coupledClass, instance);
                }
            }
        }

        private static List<CoupledClassStrength> GetReferencedInstances(CaDETClass referencingClass)
        {
            var referencedInstances = new List<CaDETClass>();
            referencedInstances.AddRange(referencingClass.GetFieldLinkedTypes());
            referencedInstances.AddRange(referencingClass.GetMethodInvocationsTypes());
            referencedInstances.AddRange(referencingClass.GetMethodLinkedParameterTypes());
            referencedInstances.AddRange(referencingClass.GetMethodLinkedReturnTypes());
            referencedInstances.AddRange(referencingClass.GetMethodLinkedVariableTypes());

            return CountCouplingStrength(referencedInstances);
        }

        private static List<CoupledClassStrength> CountCouplingStrength(List<CaDETClass> referencedInstances)
        {
            var coupledClasses = new Dictionary<CaDETClass, int>();
            foreach (var instance in referencedInstances)
            {
                if (!coupledClasses.ContainsKey(instance))
                    coupledClasses.Add(instance, 1);
                else coupledClasses[instance]++;
            }
            
            return coupledClasses.Select(coupledClass => new CoupledClassStrength(coupledClass.Key, coupledClass.Value)).ToList();
        }

        private void AddCoupledClassToMap(CoupledClassStrength coupledClassStrength, CaDETClass instance)
        {
            if (_classCouplings.ContainsKey(coupledClassStrength.CoupledClass))
            {
                _classCouplings[coupledClassStrength.CoupledClass].Add(new CoupledClassStrength(instance, coupledClassStrength.CouplingStrength));
                return;
            }

            var references = new List<CoupledClassStrength>();
            references.Add(new CoupledClassStrength(instance, coupledClassStrength.CouplingStrength));
            _classCouplings.Add(coupledClassStrength.CoupledClass, references);
        }

        private List<RelatedInstance> FindClassRelatedInstances(CaDETClass c)
        {
            var relatedInstances = new List<RelatedInstance>();
            if (c.Parent != null) relatedInstances.Add(new RelatedInstance(c.Parent.FullName, GetCodeUrl(c.Parent.FullName), RelationType.Parent, 1));
            relatedInstances.AddRange(FindReferencedInstances(c));
            relatedInstances.AddRange(FindInstancesThatReference(c));
            return relatedInstances;
        }

        private List<RelatedInstance> FindReferencedInstances(CaDETClass referencingClass)
        {
            return GetReferencedInstances(referencingClass).Select(cc => new RelatedInstance(cc.CoupledClass.FullName,
                GetCodeUrl(cc.CoupledClass.FullName), RelationType.Referenced, cc.CouplingStrength)).ToList();
        }

        private List<RelatedInstance> FindInstancesThatReference(CaDETClass referencedClass)
        {
            var instancesThatReference = new List<CoupledClassStrength>();
            if (_classCouplings.TryGetValue(referencedClass, out instancesThatReference))
            {
                return instancesThatReference.Select(cc => new RelatedInstance(cc.CoupledClass.FullName,
                    GetCodeUrl(cc.CoupledClass.FullName), RelationType.References, cc.CouplingStrength)).ToList();
            }
            return new List<RelatedInstance>();
        }

        private string GetCodeUrl(string snippetId)
        {
            _cadetProject.CodeLinks.TryGetValue(snippetId, out var codeLink);
            return _projectAndCommitUrl + codeLink.FileLocation + "#L" + codeLink.StartLoC + "-L" + codeLink.EndLoC;
        }

        private static void ShuffleList<T>(IList<T> list)
        {
            var rnd = new Random();
            for (var i = 0; i < list.Count - 1; i++)
            {
                int randomSwapLocation = rnd.Next(i, list.Count);
                var temp = list[i];
                list[i] = list[randomSwapLocation];
                list[randomSwapLocation] = temp;
            }
        }

        private List<Instance> BuildMembers()
        {
            var allMembers = _cadetProject.Classes.SelectMany(c => c.Members).ToList();
            var cadetMembers = allMembers.Where(m => _acceptedMemberTypes.Contains(m.Type)).ToList();
            if (_randomizeMemberList) ShuffleList(cadetMembers);
            if (_percentileOfProjectCovered < 100) cadetMembers = cadetMembers.Take(DetermineNumberOfInstances(cadetMembers.Count)).ToList();
            return CaDETToDataSetFunction(cadetMembers);
        }

        private List<Instance> CaDETToDataSetFunction(List<CaDETMember> cadetMembers)
        {
            return cadetMembers.Select(m => new Instance(
                m.Signature(), GetCodeUrl(m.Signature()), _projectAndCommitUrl, SnippetType.Function, _cadetProject.GetMetricsForCodeSnippet(m.Signature()), FindMethodRelatedInstances(m)
            )).ToList();
        }

        private List<RelatedInstance> FindMethodRelatedInstances(CaDETMember m)
        {
            var relatedInstances = new List<RelatedInstance>();
            relatedInstances.AddRange(FindReferencedInstances(m));
            return relatedInstances;
        }

        private IEnumerable<RelatedInstance> FindReferencedInstances(CaDETMember referencingMember)
        {
            return GetReferencedInstances(referencingMember).Select(cc => new RelatedInstance(cc.CoupledClass.FullName,
                GetCodeUrl(cc.CoupledClass.FullName), RelationType.Referenced, cc.CouplingStrength)).ToList();
        }

        private IEnumerable<CoupledClassStrength> GetReferencedInstances(CaDETMember referencingMember)
        {
            var referencedInstances = new List<CaDETClass>();
            referencedInstances.AddRange(referencingMember.GetLinkedReturnTypes());
            referencedInstances.AddRange(referencingMember.InvokedMethods.Select(m => m.Parent));
            referencedInstances.AddRange(referencingMember.AccessedAccessors.Select(a => a.Parent));
            referencedInstances.AddRange(referencingMember.AccessedFields.Select(f => f.Parent));

            return CountCouplingStrength(referencedInstances);
        }
    }
}
