using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.Content;
using SmartTutor.Controllers.Content.DTOs;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.Database;
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
            expectedLectureIds.All(actualLectureIds.Contains).ShouldBeTrue();
        }

        [Theory]
        [MemberData(nameof(Courses))]
        public void Validates_subscription_limitation_when_creating_course(CreateCourseDto createCourseDto,
            bool shouldCourseBeCreated)
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new ContentController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<IContentService>());
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartTutorContext>();
            controller.CreateCourse(createCourseDto);
            var isCourseCreated = dbContext.Courses.ToList()
                .Any(course => course.Name.Equals(createCourseDto.Course.Name));
            isCourseCreated.ShouldBe(shouldCourseBeCreated);
        }

        public static IEnumerable<object[]> Courses() => new List<object[]>
        {
            new object[]
            {
                new CreateCourseDto()
                {
                    TeacherId = 1, Course = new CourseDto() {Name = "Test_example_name_success"}
                },
                true
            },
            new object[]
            {
                new CreateCourseDto
                {
                    TeacherId = 3, Course = new CourseDto() {Name = "Test_example_name_teacher_not_subscribed"}
                },
                false
            }
        };

        [Theory]
        [MemberData(nameof(Lectures))]
        public void Validates_subscription_limitation_when_creating_lecture(CreateLectureDto createLectureDto,
            bool shouldLectureBeCreated)
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new ContentController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<IContentService>());
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartTutorContext>();
            controller.CreateLecture(createLectureDto);
            var isCourseCreated = dbContext.Lectures.ToList().Any(lecture =>
                lecture.CourseId.Equals(createLectureDto.Lecture.CourseId) &&
                lecture.Description.Equals(createLectureDto.Lecture.Description) &&
                lecture.Name.Equals(createLectureDto.Lecture.Name));
            isCourseCreated.ShouldBe(shouldLectureBeCreated);
        }

        public static IEnumerable<object[]> Lectures() => new List<object[]>
        {
            new object[]
            {
                new CreateLectureDto()
                {
                    TeacherId = 1,
                    Lecture = new LectureDTO() {CourseId = 1, Description = "Interesting", Name = "Basics"}
                },
                true
            },
            new object[]
            {
                new CreateLectureDto()
                {
                    TeacherId = 3,
                    Lecture = new LectureDTO() {CourseId = 1, Description = "Example", Name = "Example"}
                },
                false
            }
        };
    }
}