using Moq;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.InstructorModel;
using SmartTutor.InstructorModel.Instructors;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.ProgressModel.Progress;
using System.Collections.Generic;
using SmartTutor.LearnerModel.Learners.Repository;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class RecommenderTests
    {
        private readonly IInstructor _instructor;

        public RecommenderTests()
        {
            _instructor = CreateInstructor();
        }

        private static IInstructor CreateInstructor()
        {
            Mock<ILearningObjectRepository> learningObjectRepo = new Mock<ILearningObjectRepository>();
            learningObjectRepo.Setup(repo => repo.GetVideoForSummary(1))
                .Returns(Video1);
            learningObjectRepo.Setup(repo => repo.GetInteractiveLOForSummary(1))
                .Returns(Question1);
            learningObjectRepo.Setup(repo => repo.GetImageForSummary(1))
                .Returns(Image1);
            learningObjectRepo.Setup(repo => repo.GetTextForSummary(1))
                .Returns(Text1);
            learningObjectRepo.Setup(repo => repo.GetInteractiveLOForSummary(2))
                .Returns(ArrangeTask2);
            learningObjectRepo.Setup(repo => repo.GetImageForSummary(2))
                .Returns(Image2);
            learningObjectRepo.Setup(repo => repo.GetTextForSummary(2))
                .Returns(Text2);
            learningObjectRepo.Setup(repo => repo.GetLearningObjectForSummary(3))
                .Returns(Text3);

            Mock<ILearnerRepository> learnerRepo = new Mock<ILearnerRepository>();
            learnerRepo.Setup(repo => repo.GetById(1)).Returns(new Learner
                {Id = 1, AuralScore = 1, KinaestheticScore = 2, VisualScore = 3, ReadWriteScore = 4});
            learnerRepo.Setup(repo => repo.GetById(2)).Returns(new Learner
                { Id = 2, AuralScore = 4, KinaestheticScore = 2, VisualScore = 3, ReadWriteScore = 1 });
            learnerRepo.Setup(repo => repo.GetById(3)).Returns(new Learner
                { Id = 3, AuralScore = 3, KinaestheticScore = 4, VisualScore = 2, ReadWriteScore = 1 });

            return new VARKRecommender(learningObjectRepo.Object, learnerRepo.Object);
        }

        [Theory]
        [MemberData(nameof(LearnerTestData))]
        public void Builds_node_progress(int learnerId, KnowledgeNode node,
            NodeProgress expectedNodeProgress)
        {
            var result = _instructor.BuildNodeForLearner(learnerId, node);
            result.LearningObjects.ShouldBe(expectedNodeProgress.LearningObjects);
        }

        public static IEnumerable<object[]> LearnerTestData =>
            new List<object[]>
            {
                new object[]
                {
                    1,
                    KnowledgeNode,
                    new NodeProgress
                    {
                        LearnerId = 1, Node = KnowledgeNode, Status = NodeStatus.Started,
                        LearningObjects = new List<LearningObject> {Text1, Text2, Text3}
                    }
                },
                new object[]
                {
                    2,
                    KnowledgeNode,
                    new NodeProgress
                    {
                        LearnerId = 2, Node = KnowledgeNode, Status = NodeStatus.Started,
                        LearningObjects = new List<LearningObject> {Video1, Image2, Text3}
                    }
                },
                new object[]
                {
                    3,
                    KnowledgeNode,
                    new NodeProgress
                    {
                        LearnerId = 3, Node = KnowledgeNode, Status = NodeStatus.Started,
                        LearningObjects = new List<LearningObject> {Question1, ArrangeTask2, Text3}
                    }
                }
            };
        //TODO: Rework.
        

        private static readonly KnowledgeNode KnowledgeNode = new KnowledgeNode
        {
            Id = 1,
            LearningObjectSummaries = new List<LearningObjectSummary>
            {
                new LearningObjectSummary {Id = 1}, new LearningObjectSummary {Id = 2},
                new LearningObjectSummary {Id = 3}
            }
        };

        private static readonly Text Text1 = new Text { Id = 1, LearningObjectSummaryId = 1 };
        private static readonly Text Text2 = new Text { Id = 2, LearningObjectSummaryId = 2 };
        private static readonly Text Text3 = new Text { Id = 3, LearningObjectSummaryId = 3 };
        private static readonly Video Video1 = new Video { Id = 4, LearningObjectSummaryId = 1 };
        private static readonly Image Image1 = new Image { Id = 5, LearningObjectSummaryId = 1 };
        private static readonly Image Image2 = new Image { Id = 6, LearningObjectSummaryId = 2 };
        private static readonly Question Question1 = new Question { Id = 7, LearningObjectSummaryId = 1 };
        private static readonly ArrangeTask ArrangeTask2 = new ArrangeTask { Id = 8, LearningObjectSummaryId = 1 };
    }
}