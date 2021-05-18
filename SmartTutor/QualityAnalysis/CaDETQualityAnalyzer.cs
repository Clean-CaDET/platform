using SmartTutor.InstructorModel.Instructors;
using SmartTutor.QualityAnalysis.Repository;
using SmellDetector;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;
using System.Linq;

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
        public CodeEvaluation EvaluateCode(CodeSubmission submission)
        {
            var issueReport = _smellDetectorService.AnalyzeCodeQuality(submission.SourceCode);
            if(issueReport.IssuesForCodeSnippet.Count == 0) return new CodeEvaluation();

            var qualityEvaluation = GatherLOSummaries(issueReport);
            var instructorLOs =
                _instructor.BuildNodeForLearner(submission.LearnerId, qualityEvaluation.GetLearningObjectSummaries());
            qualityEvaluation.AddLearningObjects(instructorLOs);

            return qualityEvaluation;
        }

        private CodeEvaluation GatherLOSummaries(SmellDetectionReport report)
        {
            var qualityEvaluation = new CodeEvaluation();
            foreach (var codeSnippetId in report.IssuesForCodeSnippet.Keys)
            {
                qualityEvaluation.Put(codeSnippetId, GetSummaries(report.IssuesForCodeSnippet[codeSnippetId]));
            }

            return qualityEvaluation;
        }

        private List<IssueAdvice> GetSummaries(IEnumerable<Issue> issues)
        {
            var issueTypes = issues.Select(i => i.IssueType.ToString()).ToList();
            return _adviceRepository.GetAdvice(issueTypes);
        }
    }
}
