namespace SmartTutorTests.DataFactories
{
    internal class ChallengeTestData
    {
        public static string[] GetTwoPassingClasses()
        {
            return new[] {
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
	                    private void PrintPaymentDetails(int cost) {
      		                System.out.println(""Hello."");
                            System.out.println(""Your payment is created."");
                            System.out.println(""Cost is: "" + Cost);
                        }
                    }
                }"
            };
        }

        public static string[] GetTwoViolatingClasses()
        {
            return new[]
            {
                @"using System;
                namespace ExamplesApp.Method
                {
                   class PaymentClass
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
    	                private void CreatePayment(int pricee, int cOOmpensation) {
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
                            System.out.println(""Cost is: "" + cost);
                        }
                    }
                }"
            };
        }
    }
}
