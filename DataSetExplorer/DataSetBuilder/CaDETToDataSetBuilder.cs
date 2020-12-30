using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using DataSetExplorer.DataSetBuilder.Model;

namespace DataSetExplorer.DataSetBuilder
{
    internal class CaDETToDataSetBuilder
    {
        private readonly string _projectUrl;

        private readonly CaDETProject _cadetProject;
        private int _percentileOfProjectCovered = 100;

        private readonly bool _includeClasses;
        private bool _randomizeClassList;
        
        private readonly bool _includeMembers;
        private bool _randomizeMemberList;
        private CaDETMemberType[] _acceptedMemberTypes = {CaDETMemberType.Constructor, CaDETMemberType.Method};
        private int _minimumELOC;
        private int _minimumNMD;
        private int _minimumNAD;


        internal CaDETToDataSetBuilder(string projectUrl, string projectPath, LanguageEnum language, bool includeClasses, bool includeMembers)
        {
            _projectUrl = projectUrl;
            _cadetProject = new CodeModelFactory(language).CreateProjectWithCodeFileLinks(projectPath);
            _includeClasses = includeClasses;
            _includeMembers = includeMembers;
        }

        internal CaDETToDataSetBuilder(string projectUrl, string projectPath): this(projectUrl, projectPath, LanguageEnum.CSharp, true, true) { }

        internal CaDETToDataSetBuilder SetProjectExtractionPercentile(int percentile)
        {
            _percentileOfProjectCovered = percentile;
            return this;
        }
        
        internal CaDETToDataSetBuilder RandomizeClassSelection()
        {
            ValidateClassesIncluded();
            _randomizeClassList = true;
            return this;
        }

        /// <summary>Final dataset will include classes that have minimumNMD or minimumNAD</summary>
        /// <param name="minimumNMD">Minimum number of methods.</param>
        /// <param name="minimumNAD">Minimum number of fields and properties with implicit fields.</param>
        internal CaDETToDataSetBuilder IncludeClassesWith(int minimumNMD, int minimumNAD)
        {
            ValidateClassesIncluded();
            _minimumNMD = minimumNMD;
            _minimumNAD = minimumNAD;
            return this;
        }

        private void ValidateClassesIncluded()
        {
            if (!_includeClasses) throw new InvalidOperationException("Classes are not included.");
        }

        internal CaDETToDataSetBuilder IncludeMemberTypes(CaDETMemberType[] acceptedTypes)
        {
            ValidateMembersIncluded();
            _acceptedMemberTypes = acceptedTypes;
            return this;
        }

        private void ValidateMembersIncluded()
        {
            if (!_includeMembers) throw new InvalidOperationException("Members are not included.");
        }

        internal CaDETToDataSetBuilder RandomizeMemberSelection()
        {
            ValidateMembersIncluded();
            _randomizeMemberList = true;
            return this;
        }

        internal CaDETToDataSetBuilder IncludeMembersWith(int minimumELOC)
        {
            ValidateMembersIncluded();
            _minimumELOC = minimumELOC;
            return this;
        }

        internal DataSet Build()
        {
            var builtDataSet = new DataSet(_projectUrl);
            if (_includeClasses) builtDataSet.AddInstances(BuildClasses());
            if (_includeMembers) builtDataSet.AddInstances(BuildMembers());
            return builtDataSet;
        }

        private List<DataSetInstance> BuildClasses()
        {
            var cadetClasses = _cadetProject.Classes.Where(
                c => c.Metrics.NAD >= _minimumNAD || c.Metrics.NMD >= _minimumNMD).ToList();
            if(_randomizeClassList) ShuffleList(cadetClasses);
            if(_percentileOfProjectCovered < 100) cadetClasses = cadetClasses.Take(DetermineNumberOfInstances(_cadetProject.Classes.Count)).ToList();
            return CaDETToDataSetClasses(cadetClasses);
        }

        private int DetermineNumberOfInstances(int totalNumber)
        {
            return totalNumber * _percentileOfProjectCovered / 100;
        }

        private List<DataSetInstance> CaDETToDataSetClasses(List<CaDETClass> cadetClasses)
        {
            return cadetClasses.Select(c => 
                new DataSetInstance(c.FullName, GetCodeUrl(c.FullName), _projectUrl, SnippetType.Class)).ToList();
        }

        private string GetCodeUrl(string snippetId)
        {
            _cadetProject.CodeLinks.TryGetValue(snippetId, out var codeLink);
            return _projectUrl + "\\" + codeLink.FileLocation + "#L" + codeLink.StartLoC + "-L" + codeLink.EndLoC;
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

        private List<DataSetInstance> BuildMembers()
        {
            var cadetMembers = new List<CaDETMember>();
            var allMembers= _cadetProject.Classes.SelectMany(c => c.Members).ToList();
            cadetMembers.AddRange(allMembers.Where(
                    m => _acceptedMemberTypes.Contains(m.Type) && m.Metrics.ELOC >= _minimumELOC));
            if (_randomizeMemberList) ShuffleList(cadetMembers);
            if (_percentileOfProjectCovered < 100) cadetMembers = cadetMembers.Take(DetermineNumberOfInstances(allMembers.Count)).ToList();

            return CaDETToDataSetFunction(cadetMembers);
        }

        private List<DataSetInstance> CaDETToDataSetFunction(List<CaDETMember> cadetMembers)
        {
            return cadetMembers.Select(m => 
                new DataSetInstance(m.Signature(), GetCodeUrl(m.Signature()), _projectUrl, SnippetType.Function)).ToList();
        }
    }
}
