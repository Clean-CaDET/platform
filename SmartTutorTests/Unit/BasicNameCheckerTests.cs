using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Controllers;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.NameChecker;
using SmartTutor.ContentModel.LectureModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class BasicNameCheckerTests
    {/*
        private readonly BasicNameChecker _basicNameChecker;

        public BasicNameCheckerTests()
        {
            _basicNameChecker = new BasicNameChecker(new List<NamingRule>
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
                        LearningObjectSummary = new LearningObjectSummary { Id = 336, Description = "Structural cohesion example" }
                    }
                },
                new NamingRule
                {
                    Id = 3370002,
                    BannedWords = new List<string> (),
                    RequiredWords = new List<string> { "Create", "Payment" }
                }
            });
        }

        [Fact]
        public void Checks_completed_challenge_fulfillment()
        {
            string[] sourceCode = new string[] {
                @"using System;
                namespace ExamplesApp.Method
                {
                   class Payment
                   {
    	               public int Cost { get; set; }
    	               public bool IsExtra { get; set; }
                   }
                }",
                @"using System;
                namespace ExamplesApp.Method
                {
                    class PaymentService{
	                    /// <summary>
                        /// 1) Extract createPayment method.
                        /// </summary>
    	                private void CreatePayment(int price, int compensation) {
		                    Payment payment = new Payment();
		                    payment.Cost = price + compensation;
                            payment.IsExtra = payment.Cost > 50000 ? true : false;
      		                PrintPaymentDetails(payment.Cost);
    	                }
	                    private void PrintPaymentDetails(int cost) {
      		                System.out.println(""Hello."");
                            System.out.println(""Your payment is created."");
                            System.out.println(""Cost is: "" + cost);
                        }
                    }
                }"
            };

            ChallengeEvaluation challengeEvaluation = _basicNameChecker.CheckChallengeFulfillment(new CodeRepositoryService().BuildClassesModel(sourceCode));

            challengeEvaluation.ChallengeCompleted.ShouldBeTrue();
            challengeEvaluation.ApplicableHints.Count().ShouldBe(1);
            challengeEvaluation.ApplicableHints[0].Id.ShouldBe(337003);
            challengeEvaluation.ApplicableHints[0].Content.ShouldBe("Cohesion");
            challengeEvaluation.ApplicableHints[0].LearningObjectSummary.Id.ShouldBe(336);
            challengeEvaluation.ApplicableHints[0].LearningObjectSummary.Description.ShouldBe("Structural cohesion example");
        }

        [Fact]
        public void Checks_incompleted_challenge_fulfillment()
        {
            string[] sourceCode = new string[] {
                @"using System;
                namespace ExamplesApp.Method
                {
                   class Payment
                   {
    	               public int Cost { get; set; }
    	               public bool IsExtra { get; set; }
                   }
                }",
                @"using System;
                namespace ExamplesApp.Method
                {
                    class PaymentaService{
	                    /// <summary>
                        /// 1) Extract createPayment method.
                        /// </summary>
    	                private void CreatePaymentMethod(int price, int compensation) {
		                    Payment payment = new Payment();
		                    payment.Cost = price + compensation;
                            payment.IsExtra = payment.Cost > 50000 ? true : false;
      		                PrintPaymentDetails(payment.Cost);
    	                }
	                    private void PrintPaymentDetails(int cost) {
      		                System.out.println(""Hello."");
                            System.out.println(""Your payment is created."");
                            System.out.println(""Cost is: "" + cost);
                        }
                    }
                }"
            };

            ChallengeEvaluation challengeEvaluation = _basicNameChecker.CheckChallengeFulfillment(new CodeRepositoryService().BuildClassesModel(sourceCode));

            challengeEvaluation.ChallengeCompleted.ShouldBeFalse();
            challengeEvaluation.ApplicableHints.Count().ShouldBe(1);
            challengeEvaluation.ApplicableHints[0].Id.ShouldBe(337003);
            challengeEvaluation.ApplicableHints[0].Content.ShouldBe("Cohesion");
            challengeEvaluation.ApplicableHints[0].LearningObjectSummary.Id.ShouldBe(336);
            challengeEvaluation.ApplicableHints[0].LearningObjectSummary.Description.ShouldBe("Structural cohesion example");
        }

        [Fact]
        public void Gets_challenge_hints_for_complete_solution_attempt()
        {
            string[] sourceCode = new string[] {
                @"using System;
                namespace ExamplesApp.Method
                {
                   class Payment
                   {
    	               public int Cost { get; set; }
    	               public bool IsExtra { get; set; }
                   }
                }",
                @"using System;
                namespace ExamplesApp.Method
                {
                    class PaymentService{
	                    /// <summary>
                        /// 1) Extract createPayment method.
                        /// </summary>
    	                private void CreatePayment(int price, int compensation) {
		                    Payment payment = new Payment();
		                    payment.Cost = price + compensation;
                            payment.IsExtra = payment.Cost > 50000 ? true : false;
      		               PrintPaymentDetails(payment.Cost);
    	                }
	                    private void PrintPaymentDetails(int cost) {
      		                System.out.println(""Hello."");
                            System.out.println(""Your payment is created."");
                            System.out.println(""Cost is: "" + cost);
                        }
                    }
                }"
            };
            List<CaDETClass> caDETClasses = new CodeRepositoryService().BuildClassesModel(sourceCode);
            List<ChallengeHint> challengeHints = _basicNameChecker.GetHintsForSolutionAttempt(caDETClasses);

            challengeHints.Count.ShouldBe(0);
        }

        [Fact]
        public void Gets_challenge_hints_for_incomplete_solution_attempt()
        {
            string[] sourceCode = new string[] {
                @"using System;
                namespace ExamplesApp.Method
                {
                   class Payment
                   {
    	               public int Cost { get; set; }
    	               public bool IsExtra { get; set; }
                   }
                }",
                @"using System;
                namespace ExamplesApp.Method
                {
                    class PaymentService{
	                    /// <summary>
                        /// 1) Extract createPayment method.
                        /// </summary>
    	                private void CreatePaymentMethod(int price, int compensation) {
		                    Payment pay = new Payment();
		                    pay.Cost = price + compensation;
                            pay.IsExtra = pay.Cost > 50000 ? true : false;
      		                PrintPaymentDetails(payment.Cost);
    	                }
	                    private void PrintPaymentDetails(int cost) {
      		                System.out.println(""Hello."");
                            System.out.println(""Your payment is created."");
                            System.out.println(""Cost is: "" + cost);
                        }
                    }
                }"
            };
            List<CaDETClass> caDETClasses = new CodeRepositoryService().BuildClassesModel(sourceCode);
            List<ChallengeHint> challengeHints = _basicNameChecker.GetHintsForSolutionAttempt(caDETClasses);

            challengeHints.Count.ShouldBe(1);
            challengeHints[0].Id.ShouldBe(337003);
            challengeHints[0].Content.ShouldBe("Cohesion");
            challengeHints[0].LearningObjectSummary.Id.ShouldBe(336);
            challengeHints[0].LearningObjectSummary.Description.ShouldBe("Structural cohesion example");
        }*/
    }
}
