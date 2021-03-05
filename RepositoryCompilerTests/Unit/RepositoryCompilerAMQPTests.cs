using System;
using System.Collections.Generic;
using System.Text;
using RepositoryCompiler.Communication;
using Shouldly;
using Xunit;

namespace RepositoryCompilerTests.Unit
{
    public class RepositoryCompilerAMQPTests
    {
        [Fact]
        public void Produce_Metrics_Report_Message()
        {
            RepositoryCompilerMessageProducer  producer = new RepositoryCompilerMessageProducer();
            producer.CreateNewMetricsReport("string test");
        }
    }
}
