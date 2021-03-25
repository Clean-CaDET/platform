using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects
{
    public class Challenge : LearningObject
    {
        public string Url { get; internal set; }
        public List<CaDETClass> ResolvedClasses { get; internal set; }
        public Dictionary<string, double> MetricsRange { get; internal set; }
    }
}
