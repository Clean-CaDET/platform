using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SmartTutor.Controllers.Learners;
using SmartTutor.Controllers.Learners.DTOs;
using SmartTutor.LearnerModel;
using SmartTutor.Security.IAM;
using Xunit;

namespace SmartTutor.Tests.Integration
{
    public class LearnerTests : IClassFixture<TutorApplicationTestFactory<Startup>>
    {
        private readonly TutorApplicationTestFactory<Startup> _factory;

        public LearnerTests(TutorApplicationTestFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void Successfully_logins()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new LearnerController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<ILearnerService>(),
                scope.ServiceProvider.GetRequiredService<IAuthProvider>());
            var loginSubmission = new LoginDTO {StudentIndex = "SU-1-2021"};

            var learner = ((OkObjectResult) controller.Login(loginSubmission).Result).Value as LearnerDTO;

            learner.Id.ShouldBe(1);
        }

        [Fact]
        public void Nonexisting_user_login()
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new LearnerController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<ILearnerService>(),
                scope.ServiceProvider.GetRequiredService<IAuthProvider>());
            var loginSubmission = new LoginDTO {StudentIndex = "SA-1-2021"};

            var code = ((NotFoundObjectResult) controller.Login(loginSubmission).Result).StatusCode;

            code.ShouldBe(404);
        }
    }
}