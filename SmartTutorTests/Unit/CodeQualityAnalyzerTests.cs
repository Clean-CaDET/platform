using Moq;
using Shouldly;
using SmartTutor.InstructorModel.Instructors;
using SmartTutor.QualityAnalysis;
using SmartTutor.QualityAnalysis.Repository;
using Xunit;

namespace SmartTutor.Tests.Unit
{
    public class CodeQualityAnalyzerTests
    {
        private readonly ICodeQualityAnalyzer _analyzer;

        public CodeQualityAnalyzerTests()
        {
            var mockInstructor = new Mock<IInstructor>();
            var mockAdviceRepo = new Mock<IAdviceRepository>();

            _analyzer = new CaDETQualityAnalyzer(mockAdviceRepo.Object, mockInstructor.Object);
        }

        [Fact]
        public void Evaluates_empty_code_submission()
        {
            var result = _analyzer.EvaluateCode(new CodeSubmission(new []{""}, 1));
            result.LearningObjects.ShouldBeEmpty();
            result.GetIssueAdvice().ShouldBeEmpty();
        }
    }
}
