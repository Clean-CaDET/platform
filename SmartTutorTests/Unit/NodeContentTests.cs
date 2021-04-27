using Moq;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Submissions;
using SmartTutor.ProgressModel.Submissions.Repository;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutor.Tests.Unit
{
    public class NodeContentTests
    {
        private readonly ISubmissionService _service;
        public NodeContentTests()
        {
            _service = new SubmissionService(CreateMockRepository(), new Mock<ISubmissionRepository>().Object);
        }

        private static ILearningObjectRepository CreateMockRepository()
        {
            Mock<ILearningObjectRepository> learningObjectRepo = new Mock<ILearningObjectRepository>();
            learningObjectRepo.Setup(repo => repo.GetQuestion(19))
                .Returns(new Question { Id=19, PossibleAnswers = new List<QuestionAnswer>
                {
                    new QuestionAnswer
                    {
                        Id = 10, Text = "First.", IsCorrect = false, Feedback = "This statement is false.", QuestionId = 19
                    },
                    new QuestionAnswer
                    {
                        Id = 11, Text = "Second.", IsCorrect = true, Feedback = "This statement is true.", QuestionId = 19
                    },
                    new QuestionAnswer
                    {
                        Id = 12, Text = "Third.", IsCorrect = true, Feedback = "This statement is true.", QuestionId = 19
                    },
                    new QuestionAnswer
                    {
                        Id = 13, Text = "Fourth.", IsCorrect = false, Feedback = "This statement is false.", QuestionId = 19
                    }
                }});
            learningObjectRepo.Setup(repo => repo.GetArrangeTask(32))
                .Returns(new ArrangeTask {Containers = new List<ArrangeTaskContainer>
                {
                    new ArrangeTaskContainer { Id = 1, Elements = new List<ArrangeTaskElement>
                    {
                        new ArrangeTaskElement {Id = 1}
                    }},
                    new ArrangeTaskContainer { Id = 2, Elements = new List<ArrangeTaskElement>
                    {
                        new ArrangeTaskElement {Id = 2}
                    }},
                    new ArrangeTaskContainer { Id = 3, Elements = new List<ArrangeTaskElement>
                    {
                        new ArrangeTaskElement {Id = 3}
                    }},
                    new ArrangeTaskContainer { Id = 4, Elements = new List<ArrangeTaskElement>
                    {
                        new ArrangeTaskElement {Id = 4}
                    }},
                    new ArrangeTaskContainer { Id = 5, Elements = new List<ArrangeTaskElement>
                    {
                        new ArrangeTaskElement {Id = 5}
                    }}
                }});
            return learningObjectRepo.Object;
        }

        [Theory]
        [MemberData(nameof(AnswersTestData))]
        public void Evaluates_answer_submission(QuestionSubmission submission, List<bool> expectedCorrectness)
        {
            var results = _service.EvaluateAnswers(submission);

            var correctness = results.Select(a => a.SubmissionWasCorrect);
            correctness.ShouldBe(expectedCorrectness);
        }

        public static IEnumerable<object[]> AnswersTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new QuestionSubmission(19, new List<int> {10, 11, 12, 13}),
                    new List<bool> {false, true, true, false}
                },
                new object[]
                {
                    new QuestionSubmission(19, new List<int> {10, 13}),
                    new List<bool> {false, false, false, false}
                },
                new object[]
                {
                    new QuestionSubmission(19, new List<int> {11, 12}),
                    new List<bool> {true, true, true, true}
                }
            };

        [Theory]
        [MemberData(nameof(ArrangeTasksTestData))]
        public void Evaluates_arrange_task_submission(ArrangeTaskSubmission submission, List<bool> expectedCorrectness)
        {
            var results = _service.EvaluateArrangeTask(submission);

            var correctness = results.Select(a => a.SubmissionWasCorrect);
            correctness.ShouldBe(expectedCorrectness);
        }

        public static IEnumerable<object[]> ArrangeTasksTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new ArrangeTaskSubmission(32, new List<ArrangeTaskContainerSubmission>
                    {
                        new ArrangeTaskContainerSubmission { Id = 1, ContainerId = 1, ElementIds = new List<int> { 1, 5 } },
                        new ArrangeTaskContainerSubmission { Id = 2, ContainerId = 2, ElementIds = new List<int> { 2 } },
                        new ArrangeTaskContainerSubmission { Id = 3, ContainerId = 3, ElementIds = new List<int> { 3 } },
                        new ArrangeTaskContainerSubmission { Id = 4, ContainerId = 4, ElementIds = new List<int> { } },
                        new ArrangeTaskContainerSubmission { Id = 5, ContainerId = 5, ElementIds = new List<int> { 4 } }
                    }),
                    new List<bool> {false, true, true, false, false}
                },
                new object[]
                {
                    new ArrangeTaskSubmission(32, new List<ArrangeTaskContainerSubmission>
                    {
                        new ArrangeTaskContainerSubmission { Id = 1, ContainerId = 1, ElementIds = new List<int> { 1, 2, 3, 4, 5} },
                        new ArrangeTaskContainerSubmission { Id = 2, ContainerId = 2, ElementIds = new List<int> {} },
                        new ArrangeTaskContainerSubmission { Id = 3, ContainerId = 3, ElementIds = new List<int> {} },
                        new ArrangeTaskContainerSubmission { Id = 4, ContainerId = 4, ElementIds = new List<int> {} },
                        new ArrangeTaskContainerSubmission { Id = 5, ContainerId = 5, ElementIds = new List<int> {} }
                    }),
                    new List<bool> {false, false, false, false, false}
                },
                new object[]
                {
                    new ArrangeTaskSubmission(32, new List<ArrangeTaskContainerSubmission>
                    {
                        new ArrangeTaskContainerSubmission { Id = 1, ContainerId = 1, ElementIds = new List<int> { 1 } },
                        new ArrangeTaskContainerSubmission { Id = 2, ContainerId = 2, ElementIds = new List<int> { 2 } },
                        new ArrangeTaskContainerSubmission { Id = 3, ContainerId = 3, ElementIds = new List<int> { 3 } },
                        new ArrangeTaskContainerSubmission { Id = 4, ContainerId = 4, ElementIds = new List<int> { 4 } },
                        new ArrangeTaskContainerSubmission { Id = 5, ContainerId = 5, ElementIds = new List<int> { 5 } }
                    }),
                    new List<bool> {true, true, true, true, true}
                }
            };
    }
}