namespace SmartTutor.Tests.TestData
{
    internal class ChallengeTestData
    {
        public static string[] GetTwoPassingClasses()
        {
            return new[] {
@"using System;
namespace Methods.Small
{
   class Payment
   {
       public int Cost { get; set; }
       public bool IsExtra { get; set; }
   }
}",
@"using System;
namespace Methods.Small
{
    class PaymentService{
	    /// <summary>
        /// 1) Extract createPayment method.
        /// </summary>
    	private void CreatePayment(int price, int compensation) {
		    Payment payment = new Payment();
		    payment.Cost = price + compensation;
            payment.IsExtra = payment.Cost > 50000 ? true : false;
      		PrintPaymentDetails(payment.Cost, compensation);
    	}
	    private void PrintPaymentDetails(int cost, int compensation) {
      		Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + cost);
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
namespace Methods.Small
{
   class PaymentClass
   {
       public int Cost { get; set; }
       public bool IsExtra { get; set; }
   }
}",
@"using System;
namespace Methods.Small
{
    class PaymentService{
	    /// <summary>
        /// 1) Extract createPayment method.
        /// </summary>
    	private void CreatePayment(int pricee, int cOOmpensation) {
		    PaymentClass payment = new PaymentClass();
		    payment.Cost = pricee + cOOmpensation;
            payment.IsExtra = payment.Cost > 50000 ? true : false;

      		Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + payment.Cost);
      		PrintPaymentDetails(pricee);

            Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + payment.Cost);
      		PrintPaymentDetails(pricee);

            Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + payment.Cost);
      		PrintPaymentDetails(pricee);
            Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + payment.Cost);
      		PrintPaymentDetails(pricee);
    	}
	    private void PrintPaymentDetails(int price) {
      		Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + price);
        }
    }
}"
            };
        }
    }
}
