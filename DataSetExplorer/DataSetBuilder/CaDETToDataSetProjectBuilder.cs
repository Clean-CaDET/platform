using CodeModel;
using CodeModel.CaDETModel;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSetBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder
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
            return cadetClasses.Select(c => new Instance(
                c.FullName, GetCodeUrl(c.FullName), _projectAndCommitUrl, SnippetType.Class, _cadetProject.GetMetricsForCodeSnippet(c.FullName)
            )).ToList();
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
                m.Signature(), GetCodeUrl(m.Signature()), _projectAndCommitUrl, SnippetType.Function, _cadetProject.GetMetricsForCodeSnippet(m.Signature())
            )).ToList();
        }
    }
}
