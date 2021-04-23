using Moq;
using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.InstructorModel.Instructors;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.ProgressModel.Progress;
using SmartTutor.ProgressModel.Progress.Repository;
using System.Collections.Generic;
using SmartTutor.ProgressModel;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class ProgressServiceTests
    {
        private readonly IProgressService _progressService;

        public ProgressServiceTests()
        {
            _progressService = CreateService();
        }

        private static IProgressService CreateService()
        {
            var learner = new Learner {Id = 1};
            var kn1 = new KnowledgeNode {Id = 1};
            var kn2 = new KnowledgeNode {Id = 2};
            var progress1 = new NodeProgress {Id = 1};
            var progress2 = new NodeProgress {Id = 10};

            Mock<IProgressRepository> progressRepo = new Mock<IProgressRepository>();
            progressRepo.Setup(repo => repo.GetNodeProgressForLearner(1, 1)).Returns(progress1);

            Mock<ILectureRepository> lectureRepo = new Mock<ILectureRepository>();
            lectureRepo.Setup(repo => repo.GetKnowledgeNodeWithSummaries(1)).Returns(kn1);
            lectureRepo.Setup(repo => repo.GetKnowledgeNodeWithSummaries(2)).Returns(kn2);
            
            Mock<IInstructor> instructor = new Mock<IInstructor>();
            instructor.Setup(r => r.BuildNodeForLearner(1, kn2))
                .Returns(progress2);

            return new ProgressService(instructor.Object, lectureRepo.Object, progressRepo.Object);
        }

        [Theory]
        [MemberData(nameof(LearnerTestData))]
        public void Creates_node_content(int nodeId, int traineeId, int nodeProgressId)
        {
            var result = _progressService.GetNodeContent(nodeId, traineeId);
            result.Id.ShouldBe(nodeProgressId);
        }

        public static IEnumerable<object[]> LearnerTestData =>
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