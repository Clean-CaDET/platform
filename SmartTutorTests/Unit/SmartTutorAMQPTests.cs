using SmartTutor.Communication;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class SmartTutorAMQPTests
    {
        [Fact]
        public void Consume_Issue_Report_Message()
        {
            // TODO: Remove once we round up the PoC
            MessageConsumer consumer = new MessageConsumer();
        }
    }
}
