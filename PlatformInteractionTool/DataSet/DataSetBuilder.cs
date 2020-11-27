using PlatformInteractionTool.DataSet.Model;
using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace PlatformInteractionTool.DataSet
{
    internal class DataSetBuilder
    {
        private readonly CaDETProject _cadetProject;
        private int _percentileOfProjectCovered = 100;

        private readonly bool _includeClasses;
        private bool _randomizeClassList = false;
        
        private readonly bool _includeMembers;
        private bool _randomizeMemberList = false;
        private CaDETMemberType[] _acceptedMemberTypes = {CaDETMemberType.Constructor, CaDETMemberType.Method};
        private int _minimumLOC = 0;

        internal DataSetBuilder(string projectPath, LanguageEnum language, bool includeClasses, bool includeMembers)
        {
            _cadetProject = new CodeModelFactory(language).ParseFiles(projectPath);
            _includeClasses = includeClasses;
            _includeMembers = includeMembers;
        }

        internal DataSetBuilder SetProjectExtractionPercentile(int percentile)
        {
            _percentileOfProjectCovered = percentile;
            return this;
        }
        
        internal DataSetBuilder RandomizeClassSelection()
        {
            ValidateClassesIncluded();
            _randomizeClassList = true;
            return this;
        }

        private void ValidateClassesIncluded()
        {
            if (!_includeClasses) throw new InvalidOperationException("Classes are not included.");
        }

        internal DataSetBuilder IncludeMemberTypes(CaDETMemberType[] acceptedTypes)
        {
            ValidateMembersIncluded();
            _acceptedMemberTypes = acceptedTypes;
            return this;
        }

        private void ValidateMembersIncluded()
        {
            if (!_includeMembers) throw new InvalidOperationException("Members are not included.");
        }

        internal DataSetBuilder RandomizeMemberSelection()
        {
            ValidateMembersIncluded();
            _randomizeMemberList = true;
            return this;
        }

        internal DataSetBuilder IncludeMembersWith(int minimumLOC)
        {
            ValidateMembersIncluded();
            _minimumLOC = minimumLOC;
            return this;
        }

        internal DataSetProject Build()
        {
            var builtProject = new DataSetProject();
            if (_includeClasses) builtProject.Classes = BuildClasses();
            if (_includeMembers) builtProject.Functions = BuildMembers();
            return builtProject;
        }

        private List<DataSetClass> BuildClasses()
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

        private static List<DataSetClass> CaDETToDataSetClasses(IEnumerable<CaDETClass> cadetClasses)
        {
            return cadetClasses.Select(c => new DataSetClass(c)).ToList();
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

        private List<DataSetFunction> BuildMembers()
        {
            var cadetMembers = new List<CaDETMember>();
            foreach (var c in _cadetProject.Classes)
            {
                cadetMembers.AddRange(c.Members.Where(
                    m => _acceptedMemberTypes.Contains(m.Type) && m.Metrics.LOC > _minimumLOC));
            }
            if (_randomizeMemberList) ShuffleList(cadetMembers);
            if (_percentileOfProjectCovered < 100) cadetMembers = cadetMembers.Take(DetermineNumber(cadetMembers)).ToList();

            return CaDETToDataSetMembers(cadetMembers);
        }

        private static List<DataSetFunction> CaDETToDataSetMembers(IEnumerable<CaDETMember> cadetMembers)
        {
            return cadetMembers.Select(m => new DataSetFunction(m)).ToList();
        }
    }
}
