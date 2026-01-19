using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SmartTutor.Controllers.Content.DTOs;
using SmartTutor.Controllers.Progress;
using SmartTutor.Controllers.Progress.DTOs.Progress;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Progress;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutor.Tests.Integration
{
    public class ProgressTests : IClassFixture<TutorApplicationTestFactory<Startup>>
    {
        private readonly TutorApplicationTestFactory<Startup> _factory;

        public ProgressTests(TutorApplicationTestFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void Finds_lecture_nodes()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new ProgressController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<IProgressService>());

            var node = ((OkObjectResult) controller.GetLectureNodes(1).Result).Value as List<KnowledgeNodeProgressDTO>;

            node.Count.ShouldBe(4);
        }

        [Fact]
        public void No_lecture_nodes()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new ProgressController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<IProgressService>());

            var code = ((NotFoundObjectResult)controller.GetLectureNodes(111).Result).StatusCode;

            code.ShouldBe(404);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Delivers_content(int nodeId, KnowledgeNodeProgressDTO expectedNode)
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new ProgressController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<IProgressService>());

            var actualNode = ((OkObjectResult)controller.GetNodeContent(nodeId).Result).Value as KnowledgeNodeProgressDTO;

            actualNode.Status.ShouldBe(expectedNode.Status);
            var actualLOIds = actualNode.LearningObjects.Select(lo => lo.Id);
            var expectedLOIds = expectedNode.LearningObjects.Select(lo => lo.Id);
            actualLOIds.Count().ShouldBe(expectedLOIds.Count());
            actualLOIds.All(expectedLOIds.Contains).ShouldBeTrue();
        }

        public static IEnumerable<object[]> TestData()
        {
            return new List<object[]>
            {
                new object[]
                {
                    1,
                    new KnowledgeNodeProgressDTO
                    {
                        Status = NodeStatus.Unlocked,
                        LearningObjects = new List<LearningObjectDTO>
                        {
                            new LearningObjectDTO {Id = 1},
                            new LearningObjectDTO {Id = 2},
                            new LearningObjectDTO {Id = 3},
                            new LearningObjectDTO {Id = 4}
                        }
                    },
                },
                new object[]
                {
                    2,
                    new KnowledgeNodeProgressDTO
                    {
                        Status = NodeStatus.Unlocked,
                        LearningObjects = new List<LearningObjectDTO>
                        {
                            new LearningObjectDTO {Id = 6},
                            new LearningObjectDTO {Id = 10},
                            new LearningObjectDTO {Id = 12}
                        }
                    },
                }
            };
        }
    }
}
