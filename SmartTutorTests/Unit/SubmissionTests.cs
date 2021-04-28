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
    public class SubmissionTests
    {
        private readonly ISubmissionService _service;
        public SubmissionTests()
        {
            _service = new SubmissionService(CreateMockRepository(), new Mock<ISubmissionRepository>().Object);
        }

        private static ILearningObjectRepository CreateMockRepository()
        {
            Mock<ILearningObjectRepository> learningObjectRepo = new Mock<ILearningObjectRepository>();
            learningObjectRepo.Setup(repo => repo.GetQuestion(19))
                .Returns(new Question(19, 0, "", new List<QuestionAnswer>
                {
                    new QuestionAnswer(10, 19, "", false, ""),
                    new QuestionAnswer(11, 19, "", true, ""),
                    new QuestionAnswer(12, 19, "", true, ""),
                    new QuestionAnswer(13, 19, "", false, "")
                }));
            learningObjectRepo.Setup(repo => repo.GetArrangeTask(32))
                .Returns(new ArrangeTask(1, 0, "", new List<ArrangeTaskContainer>
                {
                    new ArrangeTaskContainer(1, 1, "", new List<ArrangeTaskElement>
                    {
                        new ArrangeTaskElement(1, 1, "")
                    }),
                    new ArrangeTaskContainer(2, 1, "", new List<ArrangeTaskElement>
                    {
                        new ArrangeTaskElement(2, 2, "")
                    }),
                    new ArrangeTaskContainer(3, 1, "", new List<ArrangeTaskElement>
                    {
                        new ArrangeTaskElement(3, 3, "")
                    }),
                    new ArrangeTaskContainer(4, 1, "", new List<ArrangeTaskElement>
                    {
                        new ArrangeTaskElement(4, 4, "")
                    }),
                    new ArrangeTaskContainer(5, 1, "", new List<ArrangeTaskElement>
                    {
                        new ArrangeTaskElement(5, 5, "")
                    }),
                }));
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
                        new ArrangeTaskContainerSubmission(1, 1, new List<int> { 1, 5 }),
                        new ArrangeTaskContainerSubmission(2, 2, new List<int> { 2 }),
                        new ArrangeTaskContainerSubmission(3, 3, new List<int> { 3 }),
                        new ArrangeTaskContainerSubmission(4, 4, new List<int> { }),
                        new ArrangeTaskContainerSubmission(5, 5, new List<int> { 4 }),
                    }),
                    new List<bool> {false, true, true, false, false}
                },
                new object[]
                {
                    new ArrangeTaskSubmission(32, new List<ArrangeTaskContainerSubmission>
                    {
                        new ArrangeTaskContainerSubmission(1, 1, new List<int> { 1,2,3,4,5 }),
                        new ArrangeTaskContainerSubmission(2, 2, new List<int> {}),
                        new ArrangeTaskContainerSubmission(3, 3, new List<int> {}),
                        new ArrangeTaskContainerSubmission(4, 4, new List<int> {}),
                        new ArrangeTaskContainerSubmission(5, 5, new List<int> {}),
                    }),
                    new List<bool> {false, false, false, false, false}
                },
                new object[]
                {
                    new ArrangeTaskSubmission(32, new List<ArrangeTaskContainerSubmission>
                    {
                        new ArrangeTaskContainerSubmission (1, 1, new List<int> { 1 }),
                        new ArrangeTaskContainerSubmission(2, 2, new List<int> { 2 }),
                        new ArrangeTaskContainerSubmission(3, 3, new List<int> { 3 }),
                        new ArrangeTaskContainerSubmission(4, 4, new List<int> { 4 }),
                        new ArrangeTaskContainerSubmission(5, 5, new List<int> { 5 }),
                    }),
                    new List<bool> {true, true, true, true, true}
                }
            };
    }
}