using Moq;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.InstructorModel.Instructors;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.ProgressModel.Progress;
using System.Collections.Generic;
using Xunit;

namespace SmartTutor.Tests.Unit
{
    public class InstructorTests
    {
        private readonly IInstructor _instructor;

        public InstructorTests()
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
            learnerRepo.Setup(repo => repo.GetById(1))
                .Returns(new Learner(1, 1, 2, 4, 3, new List<CourseEnrollment>()));
            learnerRepo.Setup(repo => repo.GetById(2))
                .Returns(new Learner(2, 3, 4, 1, 2, new List<CourseEnrollment>()));
            learnerRepo.Setup(repo => repo.GetById(3))
                .Returns(new Learner(3, 2, 3, 1, 4, new List<CourseEnrollment>()));

            return new VARKRecommender(learningObjectRepo.Object, learnerRepo.Object);
        }

        [Theory]
        [MemberData(nameof(LearnerTestData))]
        public void Builds_personalized_node(int learnerId, KnowledgeNode node,
            NodeProgress expectedNodeProgress)
        {
            var result = _instructor.GatherLearningObjectsForLearner(learnerId, node.LearningObjectSummaries);
            result.ShouldBe(expectedNodeProgress.LearningObjects);
        }

        public static IEnumerable<object[]> LearnerTestData =>
            new List<object[]>
            {
                new object[]
                {
                    1,
                    KnowledgeNode,
                    new NodeProgress(0, 1, KnowledgeNode, NodeStatus.Started,
                        new List<LearningObject> {Text1, Text2, Text3})
                },
                new object[]
                {
                    2,
                    KnowledgeNode,
                    new NodeProgress(0, 2, KnowledgeNode, NodeStatus.Started,
                        new List<LearningObject> {Video1, Image2, Text3})
                },
                new object[]
                {
                    3,
                    KnowledgeNode,
                    new NodeProgress(0, 3, KnowledgeNode, NodeStatus.Started,
                        new List<LearningObject> {Question1, ArrangeTask2, Text3})
                }
            };
        //TODO: Rework.


        private static readonly KnowledgeNode KnowledgeNode = new KnowledgeNode(1, new List<LearningObjectSummary>
        {
            new LearningObjectSummary(1, ""), new LearningObjectSummary(2, ""), new LearningObjectSummary(3, "")
        }, 1);

        private static readonly Text Text1 = new Text(1, 1, "");
        private static readonly Text Text2 = new Text(2, 2, "");
        private static readonly Text Text3 = new Text(3, 3, "");
        private static readonly Video Video1 = new Video(4, 1, "");
        private static readonly Image Image1 = new Image(5, 1, "", "");
        private static readonly Image Image2 = new Image(6, 2, "", "");
        private static readonly Question Question1 = new Question(7, 1, "", null);
        private static readonly ArrangeTask ArrangeTask2 = new ArrangeTask(8, 3, "", null);
    }
}