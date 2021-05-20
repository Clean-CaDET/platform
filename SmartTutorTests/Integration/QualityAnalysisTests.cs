using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SmartTutor.Controllers.Content.DTOs;
using SmartTutor.Controllers.QualityAnalysis;
using SmartTutor.Controllers.QualityAnalysis.DTOs;
using SmartTutor.QualityAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace SmartTutor.Tests.Integration
{
    public class QualityAnalysisTests : IClassFixture<TutorApplicationTestFactory<Startup>>
    {
        private readonly TutorApplicationTestFactory<Startup> _factory;

        public QualityAnalysisTests(TutorApplicationTestFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Evaluates_code_quality(CodeSubmissionDTO submission, CodeEvaluationDTO expectedEvaluation)
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new QualityAnalysisController(_factory.Services.GetRequiredService<IMapper>(), scope.ServiceProvider.GetRequiredService<ICodeQualityAnalyzer>());
            
            var evaluation = ((OkObjectResult)controller.Evaluate(submission).Result).Value as CodeEvaluationDTO;

            var actualLOs = evaluation.LearningObjects.Select(lo => lo.Id);
            var expectedLOs = expectedEvaluation.LearningObjects.Select(lo => lo.Id);
            actualLOs.Count().ShouldBe(expectedLOs.Count());
            actualLOs.All(expectedLOs.Contains).ShouldBeTrue();
            var actualAffectedSnippets = evaluation.CodeSnippetIssueAdvice.Keys.ToList();
            var expectedAffectedSnippets = expectedEvaluation.CodeSnippetIssueAdvice.Keys.ToList();
            actualAffectedSnippets.Count().ShouldBe(expectedAffectedSnippets.Count());
            actualAffectedSnippets.All(expectedAffectedSnippets.Contains).ShouldBeTrue();
        }

        public static IEnumerable<object[]> TestData() => new List<object[]>
        {
            new object[]
            {
                new CodeSubmissionDTO
                {
                    SourceCode = GetCode("SmellyProject"),
                    LearnerId = 1
                },
                new CodeEvaluationDTO
                {
                    LearningObjects = new HashSet<LearningObjectDTO>
                    {
                        new LearningObjectDTO { Id = 100 },
                        new LearningObjectDTO { Id = 102 }
                    },
                    CodeSnippetIssueAdvice = new Dictionary<string, List<IssueAdviceDTO>>
                    {
                        {"BurningKnight.assets.ImGuiHelper", new List<IssueAdviceDTO>()},
                        {"BurningKnight.level.Painter", new List<IssueAdviceDTO>()},
                    }
                }
            }
        };

        private static string[] GetCode(string projectPath)
        {
            var testDataFiles = Directory.GetFiles("../../../TestData/" + projectPath);
            return testDataFiles.Select(File.ReadAllText).ToArray();
        }
    }
}
