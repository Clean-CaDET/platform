using System.Collections.Generic;
using Moq;
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
        private readonly Mock<IRecommender> _recommender = new Mock<IRecommender>();

        public ContentServiceTests()
        {
            _contentService = new ContentService(_recommender.Object, CreateMockLectureRepository(), null,
                CreateMockTraineeRepository());
        }

        private static ITraineeRepository CreateMockTraineeRepository()
        {
            Mock<ITraineeRepository> traineeRepo = new Mock<ITraineeRepository>();
            traineeRepo.Setup(repo => repo.GetNodeProgressForTrainee(1, 1)).Returns(new NodeProgress());
            traineeRepo.Setup(repo => repo.GetTraineeById(1)).Returns(Trainee);
            return traineeRepo.Object;
        }

        private static ILectureRepository CreateMockLectureRepository()
        {
            Mock<ILectureRepository> lectureRepo = new Mock<ILectureRepository>();
            lectureRepo.Setup(repo => repo.GetKnowledgeNodeWithSummaries(2)).Returns(KnowledgeNode);
            return lectureRepo.Object;
        }

        [Theory]
        [MemberData(nameof(TraineeTestData))]
        public void Creates_node_content(int nodeId, int traineeId, int numOfInvocations)
        {
            _contentService.GetNodeContent(nodeId, traineeId);
            _recommender.Verify(recommender => recommender.BuildNodeProgressForTrainee(Trainee, KnowledgeNode),
                Times.Exactly(numOfInvocations));
        }

        public static IEnumerable<object[]> TraineeTestData =>
            new List<object[]>
            {
                new object[]
                {
                    1,
                    1,
                    0
                },
                new object[]
                {
                    2,
                    1,
                    1
                }
            };

        private static readonly Trainee Trainee = new Trainee {Id = 1};

        private static readonly KnowledgeNode KnowledgeNode = new KnowledgeNode {Id = 1};
    }
}