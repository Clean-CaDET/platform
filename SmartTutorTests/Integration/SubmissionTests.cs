using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SmartTutor.Controllers.Content.DTOs;
using SmartTutor.Controllers.Progress;
using SmartTutor.Controllers.Progress.DTOs.SubmissionEvaluation;
using SmartTutor.Database;
using SmartTutor.ProgressModel;
using SmartTutor.Tests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutor.Tests.Integration
{
    public class SubmissionTests: IClassFixture<TutorApplicationTestFactory<Startup>>
    {
        private readonly TutorApplicationTestFactory<Startup> _factory;

        public SubmissionTests(TutorApplicationTestFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [MemberData(nameof(QuestionSubmissions))]
        public void Accepts_question_submission_and_produces_correct_evaluation(QuestionSubmissionDTO submission, bool expectedCorrectness)
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new SubmissionController(_factory.Services.GetRequiredService<IMapper>(), scope.ServiceProvider.GetRequiredService<ISubmissionService>());
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartTutorContext>();

            controller.SubmitQuestionAnswers(submission);

            var actualSubmission = dbContext.QuestionSubmissions.OrderBy(s => s.TimeStamp).Last(q => q.QuestionId == submission.QuestionId);
            actualSubmission.IsCorrect.ShouldBe(expectedCorrectness);
        }

        public static IEnumerable<object[]> QuestionSubmissions() => new List<object[]>
        {
            new object[]
            {
                new QuestionSubmissionDTO
                {
                    QuestionId = 17, Answers = new List<QuestionAnswerDTO>
                    {
                        new QuestionAnswerDTO {Id = 2},
                        new QuestionAnswerDTO {Id = 5}
                    },
                    LearnerId = 1
                },
                true
            },
            new object[]
            {
                new QuestionSubmissionDTO
                {
                    QuestionId = 17, Answers = new List<QuestionAnswerDTO>
                    {
                        new QuestionAnswerDTO {Id = 11},
                        new QuestionAnswerDTO {Id = 12},
                        new QuestionAnswerDTO {Id = 14},
                        new QuestionAnswerDTO {Id = 15}
                    },
                    LearnerId = 1
                },
                false
            },
            new object[]
            {
                new QuestionSubmissionDTO
                {
                    QuestionId = 17, Answers = new List<QuestionAnswerDTO>(),
                    LearnerId = 1
                },
                false
            }
        };
        
        [Fact]
        public void Question_and_learner_are_in_different_courses_returns_forbidden_status()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new SubmissionController(_factory.Services.GetRequiredService<IMapper>(), scope.ServiceProvider.GetRequiredService<ISubmissionService>());
            var submission = new QuestionSubmissionDTO
            {
                QuestionId = 17, Answers = new List<QuestionAnswerDTO>
                {
                    new QuestionAnswerDTO {Id = 2},
                    new QuestionAnswerDTO {Id = 5}
                },
                LearnerId = 4
            };
            
            controller.SubmitQuestionAnswers(submission).Result.ShouldBeOfType(typeof(ForbidResult));
        }

        [Theory]
        [MemberData(nameof(ArrangeTaskSubmissions))]
        public void Accepts_arrange_task_submission_and_produces_correct_evaluation(ArrangeTaskSubmissionDTO submission, bool expectedCorrectness)
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new SubmissionController(_factory.Services.GetRequiredService<IMapper>(), scope.ServiceProvider.GetRequiredService<ISubmissionService>());
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartTutorContext>();

            controller.SubmitArrangeTask(submission);

            var actualSubmission = dbContext.ArrangeTaskSubmissions.OrderBy(s => s.TimeStamp).Last(a => a.ArrangeTaskId == submission.ArrangeTaskId);
            actualSubmission.IsCorrect.ShouldBe(expectedCorrectness);
        }

        public static IEnumerable<object[]> ArrangeTaskSubmissions() => new List<object[]>
        {
            new object[]
            {
                new ArrangeTaskSubmissionDTO
                {
                    ArrangeTaskId = 32, Containers = new List<ArrangeTaskContainerDTO>
                    {
                        new ArrangeTaskContainerDTO {Id = 1, Elements = new List<ArrangeTaskElementDTO>()},
                        new ArrangeTaskContainerDTO {Id = 2, Elements = new List<ArrangeTaskElementDTO>()},
                        new ArrangeTaskContainerDTO {Id = 3, Elements = new List<ArrangeTaskElementDTO>()},
                        new ArrangeTaskContainerDTO {Id = 4, Elements = new List<ArrangeTaskElementDTO>()},
                        new ArrangeTaskContainerDTO {Id = 5, Elements = new List<ArrangeTaskElementDTO>
                        {
                            new ArrangeTaskElementDTO { Id = 1 },
                            new ArrangeTaskElementDTO { Id = 2 },
                            new ArrangeTaskElementDTO { Id = 3 },
                            new ArrangeTaskElementDTO { Id = 4 },
                            new ArrangeTaskElementDTO { Id = 5 }
                        }}
                    },
                    LearnerId = 1
                },
                false
            },
            new object[]
            {
                new ArrangeTaskSubmissionDTO
                {
                    ArrangeTaskId = 32, Containers = new List<ArrangeTaskContainerDTO>
                    {
                        new ArrangeTaskContainerDTO {Id = 1, Elements = new List<ArrangeTaskElementDTO> {new ArrangeTaskElementDTO { Id = 1 }}},
                        new ArrangeTaskContainerDTO {Id = 2, Elements = new List<ArrangeTaskElementDTO> {new ArrangeTaskElementDTO { Id = 2 }}},
                        new ArrangeTaskContainerDTO {Id = 3, Elements = new List<ArrangeTaskElementDTO> {new ArrangeTaskElementDTO { Id = 3 }}},
                        new ArrangeTaskContainerDTO {Id = 4, Elements = new List<ArrangeTaskElementDTO> {new ArrangeTaskElementDTO { Id = 4 }}},
                        new ArrangeTaskContainerDTO {Id = 5, Elements = new List<ArrangeTaskElementDTO> {new ArrangeTaskElementDTO { Id = 5 }}}
                    },
                    LearnerId = 1
                },
                true
            }
        };
        
        [Fact]
        public void Rejects_bad_arrange_task_submission()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new SubmissionController(_factory.Services.GetRequiredService<IMapper>(), scope.ServiceProvider.GetRequiredService<ISubmissionService>());
            var submission = new ArrangeTaskSubmissionDTO
            {
                ArrangeTaskId = 32, LearnerId = 1,
                Containers = new List<ArrangeTaskContainerDTO>
                {
                    new ArrangeTaskContainerDTO
                    {
                        Id = 1, Elements = new List<ArrangeTaskElementDTO>
                        {
                            new ArrangeTaskElementDTO {Id = 1}
                        }
                    }
                }
            };

            Should.Throw<InvalidOperationException>(() => controller.SubmitArrangeTask(submission));
        }

        [Theory]
        [MemberData(nameof(ChallengeSubmissions))]
        public void Accepts_challenge_submission_and_produces_correct_evaluation(ChallengeSubmissionDTO submission, ChallengeEvaluationDTO expectedEvaluation)
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new SubmissionController(_factory.Services.GetRequiredService<IMapper>(), scope.ServiceProvider.GetRequiredService<ISubmissionService>());
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartTutorContext>();

            var actualEvaluation = ((OkObjectResult)controller.SubmitChallenge(submission).Result).Value as ChallengeEvaluationDTO;
            
            actualEvaluation.SolutionLO.Id.ShouldBe(expectedEvaluation.SolutionLO.Id);
            actualEvaluation.ChallengeId.ShouldBe(expectedEvaluation.ChallengeId);
            actualEvaluation.ApplicableHints.Count.ShouldBe(expectedEvaluation.ApplicableHints.Count);
            actualEvaluation.ApplicableHints.Select(h => h.LearningObject.Id)
                .All(expectedEvaluation.ApplicableHints.Select(i => i.LearningObject.Id).Contains).ShouldBeTrue();

            var actualSubmission = dbContext.ChallengeSubmissions.OrderBy(s => s.TimeStamp).Last(c => c.ChallengeId == submission.ChallengeId);
            actualSubmission.IsCorrect.ShouldBe(expectedEvaluation.ChallengeCompleted);
        }

        public static IEnumerable<object[]> ChallengeSubmissions() => new List<object[]>
        {
            new object[]
            {
                new ChallengeSubmissionDTO { LearnerId = 1, ChallengeId = 41, SourceCode = ChallengeTestData.GetTwoViolatingClasses()},
                new ChallengeEvaluationDTO
                {
                    ChallengeCompleted = false, ChallengeId = 41, SolutionLO = new VideoDTO {Id = 42},
                    ApplicableHints = new List<ChallengeHintDTO> { new ChallengeHintDTO
                    {
                        Id = 1, LearningObject = new TextDTO {Id = 43},
                        ApplicableToCodeSnippets = new List<string> { "ExamplesApp.Method.PaymentService.CreatePayment(int, int)" }
                    } }
                }
            },
            new object[]
            {
                new ChallengeSubmissionDTO { LearnerId = 1, ChallengeId = 41, SourceCode = ChallengeTestData.GetTwoPassingClasses()},
                new ChallengeEvaluationDTO
                {
                    ChallengeCompleted = true, ChallengeId = 41, SolutionLO = new VideoDTO {Id = 42},
                    ApplicableHints = new List<ChallengeHintDTO> { new ChallengeHintDTO
                    {
                        Id = 1, LearningObject = new TextDTO {Id = 43},
                        ApplicableToCodeSnippets = new List<string> { "ExamplesApp.Method.PaymentService.CreatePayment(int, int)" }
                    } }
                }
            }
        };

        [Fact]
        public void Rejects_bad_challenge_submission()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new SubmissionController(_factory.Services.GetRequiredService<IMapper>(), scope.ServiceProvider.GetRequiredService<ISubmissionService>());
            var submission = new ChallengeSubmissionDTO
            {
                ChallengeId = 41,
                LearnerId = 1
            };

            controller.SubmitChallenge(submission).Result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Gets_syntax_error_hint()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new SubmissionController(_factory.Services.GetRequiredService<IMapper>(), scope.ServiceProvider.GetRequiredService<ISubmissionService>());
            var submission = new ChallengeSubmissionDTO
            {
                ChallengeId = 41,
                LearnerId = 1,
                SourceCode = new []
                {
                    @"public class Test
                    {
                        private string name;
                        public Test() { name = test }
                    }"
                }
            };

            var actualEvaluation = ((OkObjectResult)controller.SubmitChallenge(submission).Result).Value as ChallengeEvaluationDTO;

            actualEvaluation.ApplicableHints.Count.ShouldBe(1);
            var errors = actualEvaluation.ApplicableHints[0].Content;
            errors.Split("\n").Length.ShouldBe(1);
        }

    }
}
