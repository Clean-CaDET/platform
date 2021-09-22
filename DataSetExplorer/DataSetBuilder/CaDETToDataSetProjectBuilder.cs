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

        private readonly bool _includeClasses;
        private bool _randomizeClassList;
        
        private readonly bool _includeMembers;
        private bool _randomizeMemberList;
        private CaDETMemberType[] _acceptedMemberTypes = {CaDETMemberType.Constructor, CaDETMemberType.Method};
        private int _minimumELOC = 10;
        private int _minimumNMD = 3;
        private int _minimumNAD = 5;
        private int _minimumAID = 4;
        private double _maximumWOC = 0.33;
        private int _maximumWMCNAMM = 24;
        private int _minimumNOPANOPP = 2;
        private double _maximumBUR = 0.33;
        private double _maximumBOvR = 0.33;
        private readonly List<CodeSmell> _codeSmells;

        internal CaDETToDataSetProjectBuilder(string projectAndCommitUrl, string projectName, string projectPath, LanguageEnum language, bool includeClasses, bool includeMembers, List<CodeSmell> codeSmells)
        {
            _projectAndCommitUrl = projectAndCommitUrl;
            _projectName = projectName;
            _cadetProject = new CodeModelFactory(language).CreateProjectWithCodeFileLinks(projectPath);
            _includeClasses = includeClasses;
            _includeMembers = includeMembers;
            _codeSmells = codeSmells;
        }

        internal CaDETToDataSetProjectBuilder(string projectAndCommitUrl, string projectName, string projectPath, List<CodeSmell> codeSmells): this(projectAndCommitUrl, projectName, projectPath, LanguageEnum.CSharp, true, true, codeSmells) { }

        internal CaDETToDataSetProjectBuilder SetProjectExtractionPercentile(int percentile)
        {
            _percentileOfProjectCovered = percentile;
            return this;
        }

        internal CaDETToDataSetProjectBuilder SetAutomatizationThresholds(Dictionary<string, int> thresholds)
        {
            if (thresholds.TryGetValue("minimumELOC", out var minimumELOC)) _minimumELOC = minimumELOC;
            if (thresholds.TryGetValue("minimumNMD", out var minimumNMD)) _minimumNMD = minimumNMD;
            if (thresholds.TryGetValue("minimumNAD", out var minimumNAD)) _minimumNAD = minimumNAD;
            if (thresholds.TryGetValue("minimumAID", out var minimumAID)) _minimumAID = minimumAID;
            if (thresholds.TryGetValue("maximumWOC", out var maximumWOC)) _maximumWOC = maximumWOC;
            if (thresholds.TryGetValue("maximumWMCNAMM", out var maximumWMCNAMM)) _maximumWMCNAMM = maximumWMCNAMM;
            if (thresholds.TryGetValue("minimumNOPANOPP", out var minimumNOPANOPP)) _minimumNOPANOPP = minimumNOPANOPP;
            if (thresholds.TryGetValue("maximumBUR", out var maximumBUR)) _maximumBUR = maximumBUR;
            if (thresholds.TryGetValue("maximumBOvR", out var maximumBOvR)) _maximumBOvR = maximumBOvR;
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
                if (_includeClasses) builtDataSetProject.AddCandidateInstance(new CandidateDataSetInstance(smell, FilterInstancesForSmell(smell, BuildClasses())));
                if (_includeMembers) builtDataSetProject.AddCandidateInstance(new CandidateDataSetInstance(smell, FilterInstancesForSmell(smell, BuildMembers())));
            }
            return builtDataSetProject;
        }

        private List<DataSetInstance> BuildClasses()
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

        private List<DataSetInstance> CaDETToDataSetProjectClasses(List<CaDETClass> cadetClasses)
        {
            return cadetClasses.Select(c => new DataSetInstance(
                c.FullName, GetCodeUrl(c.FullName), _projectAndCommitUrl, SnippetType.Class, _cadetProject.GetMetricsForCodeSnippet(c.FullName)
            )).ToList();
        }

        private List<DataSetInstance> FilterInstancesForSmell(CodeSmell codeSmell, List<DataSetInstance> instances)
        {
            var filteredInstances = new List<DataSetInstance>();
            switch (codeSmell.Name)
            {
                case "Large Class":
                    foreach (var i in instances)
                    {
                        var NAD = i.MetricFeatures.GetValueOrDefault(CaDETMetric.NAD);
                        var NMD = i.MetricFeatures.GetValueOrDefault(CaDETMetric.NMD);
                        if (i.Type.Equals(SnippetType.Class) && (NAD >= _minimumNAD || NMD >= _minimumNMD)) filteredInstances.Add(i);
                    }
                    return filteredInstances;
                case "Long Method":
                    foreach (var i in instances)
                    {
                        var MELOC = i.MetricFeatures.GetValueOrDefault(CaDETMetric.MELOC);
                        if (i.Type.Equals(SnippetType.Function) && MELOC >= _minimumELOC) filteredInstances.Add(i);
                    }
                    return filteredInstances;
                case "Feature Envy":
                    foreach (var i in instances)
                    {
                        var AID = i.MetricFeatures.GetValueOrDefault(CaDETMetric.AID);
                        if (i.Type.Equals(SnippetType.Function) && AID >= _minimumAID) filteredInstances.Add(i);
                    }
                    return filteredInstances;
                case "Data Class":
                    foreach (var i in instances)
                    {
                        var WOC = i.MetricFeatures.GetValueOrDefault(CaDETMetric.WOC);
                        var WMCNAMM = i.MetricFeatures.GetValueOrDefault(CaDETMetric.WMCNAMM);
                        var NOPA = i.MetricFeatures.GetValueOrDefault(CaDETMetric.NOPA);
                        var NOPP = i.MetricFeatures.GetValueOrDefault(CaDETMetric.NOPP);
                        if (i.Type.Equals(SnippetType.Class) && 
                            WOC < _maximumWOC && WMCNAMM <= _maximumWMCNAMM && NOPA+NOPP > _minimumNOPANOPP) filteredInstances.Add(i);
                    }
                    return filteredInstances;
                case "Refused Bequest":
                    foreach (var i in instances)
                    {
                        var BUR = i.MetricFeatures.GetValueOrDefault(CaDETMetric.BUR);
                        var BOvR = i.MetricFeatures.GetValueOrDefault(CaDETMetric.BOvR);
                        if (i.Type.Equals(SnippetType.Class) && BUR < _maximumBUR && BOvR < _maximumBOvR) filteredInstances.Add(i);
                    }
                    return filteredInstances;
                default:
                    return new List<DataSetInstance>();
            }
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

        private List<DataSetInstance> BuildMembers()
        {
            var allMembers = _cadetProject.Classes.SelectMany(c => c.Members).ToList();
            var cadetMembers = allMembers.Where(m => _acceptedMemberTypes.Contains(m.Type)).ToList();
            if (_randomizeMemberList) ShuffleList(cadetMembers);
            if (_percentileOfProjectCovered < 100) cadetMembers = cadetMembers.Take(DetermineNumberOfInstances(cadetMembers.Count)).ToList();
            return CaDETToDataSetFunction(cadetMembers);
        }

        private List<DataSetInstance> CaDETToDataSetFunction(List<CaDETMember> cadetMembers)
        {
            return cadetMembers.Select(m => new DataSetInstance(
                m.Signature(), GetCodeUrl(m.Signature()), _projectAndCommitUrl, SnippetType.Function, _cadetProject.GetMetricsForCodeSnippet(m.Signature())
            )).ToList();
        }
    }
}
