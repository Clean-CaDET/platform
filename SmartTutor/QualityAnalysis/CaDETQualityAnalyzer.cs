using SmellDetector;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.InstructorModel.Instructors;

namespace SmartTutor.QualityAnalysis
{
    public class CaDETQualityAnalyzer: ICodeQualityAnalyzer
    {
        private readonly SmellDetectorService _smellDetectorService;
        private readonly IAdviceRepository _adviceRepository;
        private readonly IInstructor _instructor;

        public CaDETQualityAnalyzer(IAdviceRepository adviceRepository, IInstructor instructor)
        {
            _adviceRepository = adviceRepository;
            _instructor = instructor;
            _smellDetectorService = new SmellDetectorService();
        }
        public CodeQualityEvaluation EvaluateCode(CodeSubmission submission)
        {
            var issueReport = _smellDetectorService.AnalyzeCodeQuality(submission.SourceCode);
            if(issueReport.IssuesForCodeSnippet.Count == 0) return new CodeQualityEvaluation();

            var qualityEvaluation = GatherLOSummaries(issueReport);
            var instructorLOs =
                _instructor.BuildNodeForLearner(submission.LearnerId, qualityEvaluation.GetLearningObjectSummaries());
            qualityEvaluation.AddLearningObjects(instructorLOs);

            return qualityEvaluation;
        }

        private CodeQualityEvaluation GatherLOSummaries(SmellDetectionReport report)
        {
            var qualityEvaluation = new CodeQualityEvaluation();
            foreach (var codeSnippetId in report.IssuesForCodeSnippet.Keys)
            {
                qualityEvaluation.Put(codeSnippetId, GetSummaries(report.IssuesForCodeSnippet[codeSnippetId]));
            }

            return qualityEvaluation;
        }

        private List<IssueAdvice> GetSummaries(List<Issue> issues)
        {
            var issueTypes = issues.Select(i => i.IssueType.ToString()).ToList();
            return _adviceRepository.GetAdvice(issueTypes);
        }
    }
}
