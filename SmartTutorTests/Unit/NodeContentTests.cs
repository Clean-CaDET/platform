using Moq;
using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class NodeContentTests
    {
        private readonly ContentService _service;
        public NodeContentTests()
        {
            Mock<ILearningObjectRepository> learningObjectRepo = new Mock<ILearningObjectRepository>();
            learningObjectRepo.Setup(repo => repo.GetQuestionAnswers(19))
                .Returns(new List<QuestionAnswer>
                {
                    new QuestionAnswer
                    {
                        Id = 10,
                        Text = "First statement.",
                        IsCorrect = false,
                        Feedback = "This statement is false.",
                        QuestionId = 19
                    },
                    new QuestionAnswer
                    {
                        Id = 11,
                        Text = "Second statement.",
                        IsCorrect = true,
                        Feedback = "This statement is true.",
                        QuestionId = 19
                    },
                    new QuestionAnswer
                    {
                        Id = 12,
                        Text = "Third statement.",
                        IsCorrect = true,
                        Feedback = "This statement is true.",
                        QuestionId = 19
                    },
                    new QuestionAnswer
                    {
                        Id = 13,
                        Text = "Fourth statement.",
                        IsCorrect = false,
                        Feedback = "This statement is false.",
                        QuestionId = 19
                    }
                });

            _service = new ContentService(null, null, learningObjectRepo.Object);
        }
        
        [Theory]
        [MemberData(nameof(Data))]
        public void Evaluates_answer_submission(List<int> submittedAnswers, List<bool> expectedCorrectness)
        {
            var results = _service.EvaluateAnswers(19, submittedAnswers);

            var correctness = results.Select(a => a.SubmissionWasCorrect);
            correctness.ShouldBe(expectedCorrectness);
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[]
                {
                    new List<int> {10, 11, 12, 13},
                    new List<bool> {false, true, true, false}
                },
                new object[]
                {
                    new List<int> {10, 13},
                    new List<bool> {false, false, false, false}
                },
                new object[]
                {
                    new List<int> {11, 12},
                    new List<bool> {true, true, true, true}
                }
            };
    }
}
