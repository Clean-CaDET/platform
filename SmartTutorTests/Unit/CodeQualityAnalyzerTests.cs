using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.InstructorModel.Instructors;
using SmartTutor.QualityAnalysis;
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
