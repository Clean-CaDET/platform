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
        private readonly Dictionary<CaDETClass, List<CoupledClassStrength>> _classCouplings = new Dictionary<CaDETClass, List<CoupledClassStrength>>();

        internal CaDETToDataSetProjectBuilder(InstanceFilter instanceFilter, string projectAndCommitUrl, string projectName, string projectPath, List<string> ignoredFolders, LanguageEnum language, bool includeClasses, bool includeMembers, List<CodeSmell> codeSmells)
        {
            _instanceFilter = instanceFilter;
            _projectAndCommitUrl = projectAndCommitUrl;
            _projectName = projectName;
            _cadetProject = new CodeModelFactory(language).CreateProjectWithCodeFileLinks(projectPath, ignoredFolders);
            _includeClasses = includeClasses;
            _includeMembers = includeMembers;
            _codeSmells = codeSmells;
        }

        internal CaDETToDataSetProjectBuilder(InstanceFilter instanceFilter, string projectAndCommitUrl, string projectName, string projectPath, List<string> ignoredFolders, List<CodeSmell> codeSmells): this(instanceFilter, projectAndCommitUrl, projectName, projectPath, ignoredFolders, LanguageEnum.CSharp, true, true, codeSmells) { }

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
            builtDataSetProject.GraphInstances = CaDETToGraphClasses(_cadetProject.Classes);

            foreach (var smell in _codeSmells)
            {
                if (_includeClasses) builtDataSetProject.AddCandidateInstance(new SmellCandidateInstances(smell, _instanceFilter.FilterInstances(smell, BuildClasses(), _numberOfProjectInstancesCovered)));
                if (_includeMembers) builtDataSetProject.AddCandidateInstance(new SmellCandidateInstances(smell, _instanceFilter.FilterInstances(smell, BuildMembers(), _numberOfProjectInstancesCovered)));
            }
            return builtDataSetProject;
        }

        private List<GraphInstance> CaDETToGraphClasses(List<CaDETClass> cadetClasses)
        {
            return cadetClasses.Select(c => new GraphInstance(
                    c.FullName, GetCodeUrl(c.FullName), FindGraphClassRelatedInstances(c))).ToList();
        }

        private List<GraphRelatedInstance> FindGraphClassRelatedInstances(CaDETClass c)
        {
            var relatedInstances = new List<GraphRelatedInstance>();
            if (c.Parent != null)
            {
                var couplingTypeAndSt = new Dictionary<CouplingType, int>();
                couplingTypeAndSt.Add(CouplingType.Parent, 1);
                relatedInstances.Add(new GraphRelatedInstance(c.Parent.FullName, couplingTypeAndSt));
            }
            relatedInstances.AddRange(FindReferencedGraphInstances(c));
            relatedInstances.AddRange(FindGraphInstancesThatReference(c));
            return relatedInstances;
        }

        private List<GraphRelatedInstance> FindReferencedGraphInstances(CaDETClass referencingClass)
        {
            var relatedInstances = new List<GraphRelatedInstance>();
            GetReferencedInstances(referencingClass).ForEach(cc => CoupledClassToGraphRelatedInstance(relatedInstances, cc));
            return relatedInstances;
        }

        private void CoupledClassToGraphRelatedInstance(List<GraphRelatedInstance> relatedInstances, CoupledClassStrength cc)
        {
            var index = relatedInstances.FindIndex(i => i.CodeSnippetId.Equals(cc.CoupledClass.FullName));
            if (index != -1)
            {
                try
                {
                    relatedInstances[index].CouplingTypeAndStrength.Add(cc.CouplingType, cc.CouplingStrength);
                }
                catch (Exception e)
                {
                    relatedInstances[index].CouplingTypeAndStrength[cc.CouplingType] += cc.CouplingStrength;
                }
            }
            else
            {
                var couplingTypeAndSt = new Dictionary<CouplingType, int>();
                couplingTypeAndSt.Add(cc.CouplingType, cc.CouplingStrength);
                relatedInstances.Add(new GraphRelatedInstance(cc.CoupledClass.FullName, couplingTypeAndSt));
            }
        }

        private List<GraphRelatedInstance> FindGraphInstancesThatReference(CaDETClass referencedClass)
        {
            if (_classCouplings.TryGetValue(referencedClass, out var instancesThatReference))
            {
                var relatedInstances = new List<GraphRelatedInstance>();
                instancesThatReference.ForEach(cc =>
                    CoupledClassToGraphRelatedInstance(relatedInstances, cc, RelationType.References));
                return relatedInstances;
            }
            return new List<GraphRelatedInstance>();
        }

        private void CoupledClassToGraphRelatedInstance(List<GraphRelatedInstance> relatedInstances, CoupledClassStrength cc, RelationType relationType)
        {
            var index = relatedInstances.FindIndex(i => i.CodeSnippetId.Equals(cc.CoupledClass.FullName));
            if (index != -1)
            {
                try
                {
                    relatedInstances[index].CouplingTypeAndStrength.Add(cc.CouplingType, cc.CouplingStrength);
                }
                catch (Exception e)
                {
                    relatedInstances[index].CouplingTypeAndStrength[cc.CouplingType] += cc.CouplingStrength;
                }
            }
            else
            {
                var couplingTypeAndSt = new Dictionary<CouplingType, int>();
                couplingTypeAndSt.Add(cc.CouplingType, cc.CouplingStrength);
                relatedInstances.Add(new GraphRelatedInstance(cc.CoupledClass.FullName, couplingTypeAndSt));
            }
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
            var coupledClasses = new List<CoupledClassStrength>();
            coupledClasses.AddRange(CountCouplingStrength(referencingClass.GetFieldLinkedTypes(), CouplingType.Field));
            coupledClasses.AddRange(CountCouplingStrength(referencingClass.GetMethodInvocationsTypes(), CouplingType.MethodInvocation));
            coupledClasses.AddRange(CountCouplingStrength(referencingClass.GetMethodLinkedParameterTypes(), CouplingType.Parameter));
            coupledClasses.AddRange(CountCouplingStrength(referencingClass.GetMethodLinkedReturnTypes(), CouplingType.ReturnType));
            coupledClasses.AddRange(CountCouplingStrength(referencingClass.GetMethodLinkedVariableTypes(), CouplingType.Variable));
            coupledClasses.AddRange(CountCouplingStrength(referencingClass.GetAccessedAccessorsTypes(), CouplingType.AccessedAccessor));
            coupledClasses.AddRange(CountCouplingStrength(referencingClass.GetAccessedFieldsTypes(), CouplingType.AccessedField));
            return coupledClasses;
        }

        private static List<CoupledClassStrength> CountCouplingStrength(List<CaDETClass> referencedInstances, CouplingType couplingType)
        {
            var coupledClasses = new Dictionary<CaDETClass, int>();
            foreach (var instance in referencedInstances)
            {
                if (!coupledClasses.ContainsKey(instance))
                    coupledClasses.Add(instance, 1);
                else coupledClasses[instance]++;
            }
            
            return coupledClasses.Select(coupledClass => new CoupledClassStrength(coupledClass.Key, coupledClass.Value, couplingType)).ToList();
        }

        private void AddCoupledClassToMap(CoupledClassStrength coupledClassStrength, CaDETClass instance)
        {
            if (_classCouplings.ContainsKey(coupledClassStrength.CoupledClass))
            {
                _classCouplings[coupledClassStrength.CoupledClass].Add(new CoupledClassStrength(instance, coupledClassStrength.CouplingStrength, coupledClassStrength.CouplingType));
                return; 
            }

            var references = new List<CoupledClassStrength>();
            references.Add(new CoupledClassStrength(instance, coupledClassStrength.CouplingStrength, coupledClassStrength.CouplingType));
            _classCouplings.Add(coupledClassStrength.CoupledClass, references);
        }

        private List<RelatedInstance> FindClassRelatedInstances(CaDETClass c)
        {
            var relatedInstances = new List<RelatedInstance>();
            if (c.Parent != null)
            {
                var couplingTypeAndSt = new Dictionary<CouplingType, int>();
                couplingTypeAndSt.Add(CouplingType.Parent, 1);
                relatedInstances.Add(new RelatedInstance(c.Parent.FullName, GetCodeUrl(c.Parent.FullName), RelationType.Parent, couplingTypeAndSt));
            }
            relatedInstances.AddRange(FindReferencedInstances(c));
            relatedInstances.AddRange(FindInstancesThatReference(c));
            return relatedInstances;
        }

        private List<RelatedInstance> FindReferencedInstances(CaDETClass referencingClass)
        {
            var relatedInstances = new List<RelatedInstance>();
            GetReferencedInstances(referencingClass).ForEach(cc => CoupledClassToRelatedInstance(relatedInstances, cc, RelationType.Referenced));
            return relatedInstances;
        }

        private void CoupledClassToRelatedInstance(List<RelatedInstance> relatedInstances, CoupledClassStrength cc, RelationType relationType)
        {
            var index = relatedInstances.FindIndex(i => i.CodeSnippetId.Equals(cc.CoupledClass.FullName));
            if (index != -1)
            {
                try
                {
                    relatedInstances[index].CouplingTypeAndStrength.Add(cc.CouplingType, cc.CouplingStrength);
                } catch(Exception e)
                {
                    relatedInstances[index].CouplingTypeAndStrength[cc.CouplingType] += cc.CouplingStrength;
                }
            }
            else
            {
                var couplingTypeAndSt = new Dictionary<CouplingType, int>();
                couplingTypeAndSt.Add(cc.CouplingType, cc.CouplingStrength);
                relatedInstances.Add(new RelatedInstance(cc.CoupledClass.FullName,
                    GetCodeUrl(cc.CoupledClass.FullName), relationType, couplingTypeAndSt));
            }
        }

        private List<RelatedInstance> FindInstancesThatReference(CaDETClass referencedClass)
        {
            if (_classCouplings.TryGetValue(referencedClass, out var instancesThatReference))
            {
                var relatedInstances = new List<RelatedInstance>();
                instancesThatReference.ForEach(cc =>
                    CoupledClassToRelatedInstance(relatedInstances, cc, RelationType.References));
                return relatedInstances;
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
            var relatedInstances = new List<RelatedInstance>();
            var classParent = referencingMember.Parent.Parent;
            if (classParent != null)
            {
                var couplingTypeAndSt = new Dictionary<CouplingType, int>();
                couplingTypeAndSt.Add(CouplingType.Parent, 1);
                relatedInstances.Add(new RelatedInstance(classParent.FullName, GetCodeUrl(classParent.FullName), RelationType.Parent, couplingTypeAndSt));
            }
            GetReferencedInstances(referencingMember).ForEach(cc => CoupledClassToRelatedInstance(relatedInstances, cc, RelationType.Referenced));
            return relatedInstances;
        }

        private List<CoupledClassStrength> GetReferencedInstances(CaDETMember referencingMember)
        {
            var coupledClasses = new List<CoupledClassStrength>();
            coupledClasses.AddRange(CountCouplingStrength(referencingMember.GetLinkedParamTypes(), CouplingType.Parameter));
            coupledClasses.AddRange(CountCouplingStrength(referencingMember.GetLinkedReturnTypes(), CouplingType.ReturnType));
            coupledClasses.AddRange(CountCouplingStrength(referencingMember.GetLinkedVariableTypes(), CouplingType.Variable));
            coupledClasses.AddRange(CountCouplingStrength(referencingMember.GetLinkedMethodInvocationTypes(), CouplingType.MethodInvocation));
            coupledClasses.AddRange(CountCouplingStrength(referencingMember.GetLinkedAccessedAccessorTypes(), CouplingType.AccessedAccessor));
            coupledClasses.AddRange(CountCouplingStrength(referencingMember.GetLinkedAccessedFieldTypes(), CouplingType.AccessedField));
            return coupledClasses;
        }
    }
}
