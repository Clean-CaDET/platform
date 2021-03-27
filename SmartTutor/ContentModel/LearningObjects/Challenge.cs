using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using SmartTutor.ContentModel.LearningObjects.MetricRules;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects
{
    public class Challenge : LearningObject
    {
        public string Url { get; internal set; }
        public List<CaDETClass> ResolvedClasses { get; internal set; }
        public List<MetricRangeRule> ClassMetricRules { get; internal set; }
        public List<MetricRangeRule> MethodMetricRules { get; internal set; }
    }
}
