using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Controllers;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker;
using SmartTutor.ContentModel.LectureModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class BasicMetricsCheckerTests
    {
        private readonly BasicMetricsChecker _basicMetricsChecker;

        public BasicMetricsCheckerTests()
        {
            _basicMetricsChecker = new BasicMetricsChecker
            {
                ClassMetricRules = new List<MetricRangeRule>
                {
                    new MetricRangeRule
                    {
                        Id = 33701,
                        MetricName = "CLOC",
                        FromValue = 3,
                        ToValue = 30,
                        Hint = new ChallengeHint
                        {
                            Id = 337001,
                            Content = "Cohesion",
                            LearningObjectSummary = new LearningObjectSummary
                                {Id = 331, Description = "Cohesion definition"}
                        }
                    },
                    new MetricRangeRule {Id = 33702, MetricName = "NMD", FromValue = 0, ToValue = 2, Hint = new ChallengeHint {Id = 5}}
                },
                MethodMetricRules = new List<MetricRangeRule>
                {
                    new MetricRangeRule
                    {
                        Id = 33703,
                        MetricName = "MELOC",
                        FromValue = 2,
                        ToValue = 5,
                        Hint = new ChallengeHint
                        {
                            Id = 337002,
                            Content = "Cohesion",
                            LearningObjectSummary = new LearningObjectSummary
                                {Id = 336, Description = "Structural cohesion example"}
                        }
                    },
                    new MetricRangeRule {Id = 33704, MetricName = "NOP", FromValue = 1, ToValue = 4, Hint = new ChallengeHint {Id = 6}}
                }
            };
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

            HintDirectory challengeEvaluation = _basicMetricsChecker.EvaluateSubmission(new CodeRepositoryService().BuildClassesModel(sourceCode));

            
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

            HintDirectory challengeEvaluation = _basicMetricsChecker.EvaluateSubmission(new CodeRepositoryService().BuildClassesModel(sourceCode));

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
            HintDirectory challengeHints = _basicMetricsChecker.EvaluateSubmission(caDETClasses);

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
            HintDirectory challengeHints = _basicMetricsChecker.EvaluateSubmission(caDETClasses);

        }
    }
}
