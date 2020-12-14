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
        private string _dataSetName;

        private readonly CaDETProject _cadetProject;
        private int _percentileOfProjectCovered = 100;

        private readonly bool _includeClasses;
        private bool _randomizeClassList;
        
        private readonly bool _includeMembers;
        private bool _randomizeMemberList;
        private CaDETMemberType[] _acceptedMemberTypes = {CaDETMemberType.Constructor, CaDETMemberType.Method};
        private int _minimumELOC;
        

        internal CaDETToDataSetBuilder(string dataSetName, string projectPath, LanguageEnum language, bool includeClasses, bool includeMembers)
        {
            _dataSetName = dataSetName;
            _cadetProject = new CodeModelFactory(language).CreateProject(projectPath);
            _includeClasses = includeClasses;
            _includeMembers = includeMembers;
        }

        internal CaDETToDataSetBuilder(string dataSetName, string projectPath): this(dataSetName, projectPath, LanguageEnum.CSharp, true, true) { }

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
            var builtDataSet = new DataSet(_dataSetName);
            if (_includeClasses) builtDataSet.AddInstances(BuildClasses());
            if (_includeMembers) builtDataSet.AddInstances(BuildMembers());
            return builtDataSet;
        }

        private List<DataSetInstance> BuildClasses()
        {
            var cadetClasses = _cadetProject.Classes;
            if(_randomizeClassList) ShuffleList(cadetClasses);
            if(_percentileOfProjectCovered < 100) cadetClasses = cadetClasses.Take(DetermineNumber(cadetClasses)).ToList();
            return CaDETToDataSetClasses(cadetClasses);
        }

        private int DetermineNumber<T>(List<T> list)
        {
            return list.Count * _percentileOfProjectCovered / 100;
        }

        private static List<DataSetInstance> CaDETToDataSetClasses(IEnumerable<CaDETClass> cadetClasses)
        {
            return cadetClasses.Select(c => new DataSetInstance(c.FullName, null, null, SnippetType.Class)).ToList();
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
            foreach (var c in _cadetProject.Classes)
            {
                cadetMembers.AddRange(c.Members.Where(
                    m => _acceptedMemberTypes.Contains(m.Type) && m.Metrics.ELOC > _minimumELOC));
            }
            if (_randomizeMemberList) ShuffleList(cadetMembers);
            if (_percentileOfProjectCovered < 100) cadetMembers = cadetMembers.Take(DetermineNumber(cadetMembers)).ToList();

            return CaDETToDataSetFunction(cadetMembers);
        }

        private static List<DataSetInstance> CaDETToDataSetFunction(IEnumerable<CaDETMember> cadetMembers)
        {
            return cadetMembers.Select(m => new DataSetInstance(m.GetSignature(), null, null, SnippetType.Function)).ToList();
        }
    }
}
