using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.DTOs;
using SmartTutor.Controllers.Content;
using SmartTutor.Database;
using Xunit;
using Xunit.Abstractions;

namespace SmartTutor.Tests.Integration
{
    public class SubscriptionTests: IClassFixture<TutorApplicationTestFactory<Startup>>
    {
        private readonly TutorApplicationTestFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public SubscriptionTests(TutorApplicationTestFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
        }
        
        [Theory]
        [MemberData(nameof(Subscriptions))]
        public void Subscribes_teacher_successfully_and_makes_valid_subscription(SubscriptionDto subscriptionDto, bool expectedCorrectness)
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new SubscriptionController(scope.ServiceProvider.GetRequiredService<ISubscriptionService>());
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartTutorContext>();
            controller.SubscribeTeacher(subscriptionDto);
            var isSubscriptionCreated = dbContext.Subscriptions.ToList().Any(subscription => subscription.TeacherId.Equals(subscriptionDto.TeacherId) && subscription.Id>=2 && subscription.IsValid());
            isSubscriptionCreated.ShouldBe(expectedCorrectness);
        }

        public static IEnumerable<object[]> Subscriptions() => new List<object[]>
        {
            new object[]
            {
                new SubscriptionDto
                {
                    TeacherId = 1, IndividualPlanId = 1, NumberOfDays = 30
                },
                false
            },
            new object[]
            {
                new SubscriptionDto
                {
                    TeacherId = 2, IndividualPlanId = 1, NumberOfDays = -30
                },
                false
            },
            new object[]
            {
                new SubscriptionDto
                {
                    TeacherId = 2, IndividualPlanId = 1, NumberOfDays = 30
                },
                true
            }
        };
    }
}