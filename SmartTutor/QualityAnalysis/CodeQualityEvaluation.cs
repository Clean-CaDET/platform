using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.QualityAnalysis
{
    public class CodeQualityEvaluation
    {
        public Dictionary<string, List<SmellResolutions>> learningObjects { get; set; }
    }
}