using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.Content;
using SmartTutor.Controllers.Content.DTOs;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.Lectures;
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
            actualLectureIds.Count().ShouldBe(expectedLectureIds.Count);
            actualLectureIds.All(expectedLectureIds.Contains).ShouldBeTrue();
        }

        [Fact]
        public void Creates_new_knowledge_node_successfully()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new ContentController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<IContentService>());
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartTutorContext>();
            int knowledgeNodesLenght = dbContext.KnowledgeNodes.Count();
            var dto = new KnowledgeNodeDto
            {
                LectureId = 1, LearningObjectSummaries = new List<LearningObjectSummaryDto>(), LearningObjective = "",
                Type = KnowledgeNodeType.Factual
            };
            controller.CreateKnowledgeNode(dto);
            dbContext.KnowledgeNodes.Count().ShouldBe(knowledgeNodesLenght + 1);
        }

        [Fact]
        public void Creates_new_learning_object_summary_and_its_learning_objects_successfully()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new ContentController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<IContentService>());
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartTutorContext>();

            int losLenght = dbContext.LearningObjectSummaries.Count();

            int imagesLenght = dbContext.Images.Count();
            int videosLenght = dbContext.Videos.Count();
            int textsLenght = dbContext.Texts.Count();
            int questionsLenght = dbContext.Questions.Count();

            var learningObjectDtos = new List<LearningObjectDTO>();

            learningObjectDtos.Add(new ImageDTO {Caption = "", Url = ""});
            learningObjectDtos.Add(new TextDTO {Content = ""});
            learningObjectDtos.Add(new QuestionDTO {Text = "", PossibleAnswers = new List<QuestionAnswerDTO>()});
            learningObjectDtos.Add(new VideoDTO {Url = ""});

            var dto = new LearningObjectSummaryDto()
                {Description = "", KnowledgeNodeId = 1, LearningObjects = learningObjectDtos};
            controller.CreateLearningObjectSummary(dto);

            dbContext.LearningObjectSummaries.Count().ShouldBe(losLenght + 1);

            dbContext.Images.Count().ShouldBe(imagesLenght + 1);
            dbContext.Videos.Count().ShouldBe(videosLenght + 1);
            dbContext.Texts.Count().ShouldBe(textsLenght + 1);
            dbContext.Questions.Count().ShouldBe(questionsLenght + 1);
        }
    }
}