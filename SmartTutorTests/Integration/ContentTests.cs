using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.Content;
using SmartTutor.Controllers.Content.DTOs;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutor.Tests.Integration
{
    public class ContentTests : IClassFixture<TutorApplicationTestFactory<Startup>>
    {
        private readonly TutorApplicationTestFactory<Startup> _factory;

        public ContentTests(TutorApplicationTestFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void Retrieves_lectures()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new ContentController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<IContentService>());

            var lectures = ((OkObjectResult) controller.GetLectures().Result).Value as List<LectureDTO>;

            var expectedLectureIds = new List<int> {1, 2, 3, 4};
            var actualLectureIds = lectures.Select(l => l.Id);
            actualLectureIds.Count().ShouldBe(expectedLectureIds.Count);
            actualLectureIds.All(expectedLectureIds.Contains).ShouldBeTrue();
        }
    }
}
