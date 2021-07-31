using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.Content;
using SmartTutor.Controllers.Content.DTOs;
using SmartTutor.Database;
using Xunit;

namespace SmartTutor.Tests.Integration
{
    public class SubscriptionTests : IClassFixture<TutorApplicationTestFactory<Startup>>
    {
        private readonly TutorApplicationTestFactory<Startup> _factory;

        public SubscriptionTests(TutorApplicationTestFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [MemberData(nameof(Subscriptions))]
        public void Subscribes_teacher_successfully_and_makes_valid_subscription(
            CreateSubscriptionDto createSubscriptionDto, bool expectedCorrectness)
        {
            using var scope = _factory.Services.CreateScope();
            var controller = new SubscriptionController(_factory.Services.GetRequiredService<IMapper>(),
                scope.ServiceProvider.GetRequiredService<ISubscriptionService>());
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartTutorContext>();
            controller.SubscribeTeacher(createSubscriptionDto);
            var isSubscriptionCreated = dbContext.Subscriptions.ToList().Any(subscription =>
                subscription.TeacherId.Equals(createSubscriptionDto.TeacherId) && subscription.Id >= 2 &&
                subscription.IsValid());
            isSubscriptionCreated.ShouldBe(expectedCorrectness);
        }

        public static IEnumerable<object[]> Subscriptions() => new List<object[]>
        {
            new object[]
            {
                new CreateSubscriptionDto
                {
                    TeacherId = 1, IndividualPlanId = 1
                },
                false
            },
            new object[]
            {
                new CreateSubscriptionDto
                {
                    TeacherId = 2, IndividualPlanId = 1
                },
                true
            }
        };
    }
}