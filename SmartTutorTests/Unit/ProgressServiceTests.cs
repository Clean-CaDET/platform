using Moq;
using Shouldly;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.InstructorModel.Instructors;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Progress;
using SmartTutor.ProgressModel.Progress.Repository;
using System.Collections.Generic;
using Xunit;

namespace SmartTutor.Tests.Unit
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
            var kn1 = new KnowledgeNode(1, null, 1);
            var kn2 = new KnowledgeNode(2, null, 1);
            var progress1 = new NodeProgress(1, 0, null, NodeStatus.Unlocked, null);

            Mock<IProgressRepository> progressRepo = new Mock<IProgressRepository>();
            progressRepo.Setup(repo => repo.GetNodeProgressForLearner(1, 1)).Returns(progress1);

            Mock<ILectureRepository> lectureRepo = new Mock<ILectureRepository>();
            lectureRepo.Setup(repo => repo.GetKnowledgeNodeWithSummaries(1)).Returns(kn1);
            lectureRepo.Setup(repo => repo.GetKnowledgeNodeWithSummaries(2)).Returns(kn2);
            
            return new ProgressService(new Mock<IInstructor>().Object, lectureRepo.Object, progressRepo.Object);
        }

        [Theory]
        [MemberData(nameof(LearnerTestData))]
        public void Creates_node_content(int nodeId, int learnerId, int nodeProgressId)
        {
            var result = _progressService.GetNodeContent(nodeId, learnerId);
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
                    0
                }
            };
    }
}