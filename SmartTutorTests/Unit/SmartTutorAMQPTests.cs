using SmartTutor.Communucation;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class SmartTutorAMQPTests
    {
        [Fact]
        public void Consume_Issue_Report_Message()
        {
            SmartTutorMessageConsumer consumer = new SmartTutorMessageConsumer();
        }
    }
}
