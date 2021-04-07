using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.NameChecker;
using System.Collections.Generic;

namespace SmartTutorTests.Unit
{
    public class BasicNameCheckerTests
    {
        private readonly BasicNameChecker _basicNameChecker;

        public BasicNameCheckerTests()
        {
            _basicNameChecker = new BasicNameChecker
            {
                NamingRules = new List<NamingRule>
                {
                    new NamingRule
                    {
                        Id = 3370001,
                        BannedWords = new List<string> { "Class", "List", "Method" },
                        RequiredWords = new List<string> { "Payment", "Service", "PaymentService", "compensation" },
                        Hint = new ChallengeHint
                        {
                            Id = 337003,
                            Content = "Cohesion",
                            LearningObjectSummaryId = 336
                        }
                    },
                    new NamingRule
                    {
                        Id = 3370002,
                        BannedWords = new List<string> (),
                        RequiredWords = new List<string> { "Create", "Payment", "price" }
                    }
                }
            };
        }
    }
}
