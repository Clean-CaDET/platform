using System.Collections.Generic;
using Moq;
using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.LectureModel.Repository;
using SmartTutor.ContentModel.ProgressModel;
using SmartTutor.ContentModel.ProgressModel.Repository;
using SmartTutor.Recommenders;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class ContentServiceTests
    {
        private readonly IContentService _contentService;

        public ContentServiceTests()
        {
            _contentService = new ContentService(CreateMockRecommender(), CreateMockLectureRepository(), null,
                CreateMockTraineeRepository());
        }

        private static ITraineeRepository CreateMockTraineeRepository()
        {
            Mock<ITraineeRepository> traineeRepo = new Mock<ITraineeRepository>();
            traineeRepo.Setup(repo => repo.GetNodeProgressForTrainee(1, 1)).Returns(new NodeProgress {Id = 1});
            traineeRepo.Setup(repo => repo.GetTraineeById(1)).Returns(Trainee);
            return traineeRepo.Object;
        }

        private static ILectureRepository CreateMockLectureRepository()
        {
            Mock<ILectureRepository> lectureRepo = new Mock<ILectureRepository>();
            lectureRepo.Setup(repo => repo.GetKnowledgeNodeWithSummaries(2)).Returns(KnowledgeNode);
            return lectureRepo.Object;
        }

        private static IRecommender CreateMockRecommender()
        {
            Mock<IRecommender> recommender = new Mock<IRecommender>();
            recommender.Setup(r => r.BuildNodeProgressForTrainee(Trainee, KnowledgeNode))
                .Returns(new NodeProgress {Id = 10});
            return recommender.Object;
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

        private static readonly Trainee Trainee = new Trainee {Id = 1};
        private static readonly KnowledgeNode KnowledgeNode = new KnowledgeNode {Id = 2};
    }
}