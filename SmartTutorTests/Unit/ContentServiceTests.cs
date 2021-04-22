using Moq;
using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.InstructionalModel;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Repository;
using System.Collections.Generic;
using SmartTutor.LearnerModel.Learners;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class ContentServiceTests
    {
        private readonly IContentService _contentService;

        public ContentServiceTests()
        {
            _contentService = CreateService();
        }

        private static IContentService CreateService()
        {
            var learner = new Learner {Id = 1};
            var kn1 = new KnowledgeNode {Id = 1};
            var kn2 = new KnowledgeNode {Id = 2};
            var progress1 = new NodeProgress {Id = 1};
            var progress2 = new NodeProgress {Id = 10};

            Mock<ILearnerRepository> learnerRepo = new Mock<ILearnerRepository>();
            learnerRepo.Setup(repo => repo.GetNodeProgressForTrainee(1, 1)).Returns(progress1);
            learnerRepo.Setup(repo => repo.GetTraineeById(1)).Returns(learner);

            Mock<ILectureRepository> lectureRepo = new Mock<ILectureRepository>();
            lectureRepo.Setup(repo => repo.GetKnowledgeNodeWithSummaries(1)).Returns(kn1);
            lectureRepo.Setup(repo => repo.GetKnowledgeNodeWithSummaries(2)).Returns(kn2);
            
            Mock<IInstructor> instructor = new Mock<IInstructor>();
            instructor.Setup(r => r.BuildNodeProgressForTrainee(learner, kn2))
                .Returns(progress2);

            return new ContentService(instructor.Object, lectureRepo.Object, null, learnerRepo.Object);
        }

        [Theory]
        [MemberData(nameof(TraineeTestData))]
        public void Creates_node_content(int nodeId, int traineeId, int nodeProgressId)
        {
            var result = _contentService.GetNodeContent(nodeId, traineeId);
            result.Id.ShouldBe(nodeProgressId);
        }

        public static IEnumerable<object[]> TraineeTestData =>
            new List<object[]>
            {
                new object[]
                {
                    1,
                    1,
                    1
                },
                new object[]
                {
                    2,
                    1,
                    10
                }
            };
    }
}