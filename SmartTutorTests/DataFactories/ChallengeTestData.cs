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
      		Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + Cost);
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
		    payment.Cost = price + cOOmpensation;
            payment.IsExtra = payment.Cost > 50000 ? true : false;

      		Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + payment.Cost);
      		PrintPaymentDetails();

            Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + payment.Cost);
      		PrintPaymentDetails();

            Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + payment.Cost);
      		PrintPaymentDetails();
            Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + payment.Cost);
      		PrintPaymentDetails();
    	}
	    private void PrintPaymentDetails() {
      		Console.WriteLine(""Hello."");
            Console.WriteLine(""Your payment is created."");
            Console.WriteLine(""Cost is: "" + payment.Cost);
        }
    }
}"
            };
        }
    }
}
