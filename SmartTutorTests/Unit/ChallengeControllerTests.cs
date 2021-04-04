using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Controllers;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.Controllers;
using System.Collections.Generic;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class ChallengeControllerTests
    {
        private readonly ChallengeController _challengeController;

        public ChallengeControllerTests()
        {
            _challengeController = new ChallengeController();
        }

        [Fact]
        public void Gets_all_challenge_hints()
        {
            List<ChallengeHint> challengeHints = _challengeController.GetAllHints(3371);

            challengeHints.Count.ShouldBe(2);
            challengeHints[0].Id.ShouldBe(337001);
            challengeHints[0].Content.ShouldBe("Cohesion");
            challengeHints[0].LearningObjectSummary.Id.ShouldBe(331);
            challengeHints[0].LearningObjectSummary.Description.ShouldBe("Cohesion definition");
            challengeHints[1].Id.ShouldBe(337002);
            challengeHints[1].Content.ShouldBe("Cohesion");
            challengeHints[1].LearningObjectSummary.Id.ShouldBe(336);
            challengeHints[1].LearningObjectSummary.Description.ShouldBe("Structural cohesion example");
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
      		                PrintPaymentDetails();
    	                }
	                    private void PrintPaymentDetails() {
      		                System.out.println(""Hello."");
                            System.out.println(""Your payment is created."");
                            System.out.println(""Cost is: "" + payment.Cost);
                        }
                    }
                }"
            };
            List<CaDETClass> caDETClasses = new CodeRepositoryService().BuildClassesModel(sourceCode);
            List<ChallengeHint> challengeHints = _challengeController.GetHintsForSolutionAttempt(3371, caDETClasses);

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
    	                private void CreatePayment(int price, int compensation) {
		                    Payment payment = new Payment();
		                    payment.Cost = price + compensation;
                            payment.IsExtra = payment.Cost > 50000 ? true : false;

      		                System.out.println(""Hello."");
                            System.out.println(""Your payment is created."");
                            System.out.println(""Cost is: "" + payment.Cost);
      		                PrintPaymentDetails();
    	                }
	                    private void PrintPaymentDetails() {
      		                System.out.println(""Hello."");
                            System.out.println(""Your payment is created."");
                            System.out.println(""Cost is: "" + payment.Cost);
                        }
                    }
                }"
            };
            List<CaDETClass> caDETClasses = new CodeRepositoryService().BuildClassesModel(sourceCode);
            List<ChallengeHint> challengeHints = _challengeController.GetHintsForSolutionAttempt(3371, caDETClasses);

            challengeHints.Count.ShouldBe(1);
            challengeHints[0].Id.ShouldBe(337002);
            challengeHints[0].Content.ShouldBe("Cohesion");
            challengeHints[0].LearningObjectSummary.Id.ShouldBe(336);
            challengeHints[0].LearningObjectSummary.Description.ShouldBe("Structural cohesion example");
        }
    }
}
