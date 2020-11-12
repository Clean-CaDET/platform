
using SmellDetector.Controllers;
using SmellDetector.SmellModel;

namespace SmellDetectorTests.DataFactory
{
    public class CaDETClassDTOFactory
    {
        public int ExpectedIssues { get; set; }
        public string TestIdentifier { get; set; }
        public CaDETClassDTO CaDETClassDTO { get; set; }

        public void CreateIssuesLongMethodAndLongParameterList()
        {
            CaDETClassDTO = new CaDETClassDTO();
            TestIdentifier = "public void testMethod(int paramOne, int paramTwo, int another_paramTwo, int paramtwo, int paramTwos, int paramTwoas);";
            MetricsDTO metricsForIdentifier = new MetricsDTO();
            metricsForIdentifier.NOP = 6;
            CaDETClassDTO.CodeItemMetrics[TestIdentifier] = metricsForIdentifier;
            ExpectedIssues = 2; // one for long method(nop > 4) & one for long parameter list (nop > 5)

        }
    
        public void CreateIssueLongMethod()
        {
            CaDETClassDTO = new CaDETClassDTO();
            TestIdentifier = "public void testMethod(int paramOne, int paramTwo, int another_paramTwo, int paramtwo, int moreparam);";
            MetricsDTO metricsForIdentifier = new MetricsDTO();
            metricsForIdentifier.NOP = 5;
            CaDETClassDTO.CodeItemMetrics[TestIdentifier] = metricsForIdentifier;
            ExpectedIssues = 1; // one for long method(nop > 4) 
        }
    
        public void CreateAnotherIssueLongMethod()
        {
            CaDETClassDTO = new CaDETClassDTO();
            TestIdentifier = "public void testMethod();";
            MetricsDTO metricsForIdentifier = new MetricsDTO();
            metricsForIdentifier.NOP = 0;
            metricsForIdentifier.LOC = 100;
            CaDETClassDTO.CodeItemMetrics[TestIdentifier] = metricsForIdentifier;
            ExpectedIssues = 1; // one for long method(loc > 50) 
        }

        public void CreateEmptyIssue()
        {
            CaDETClassDTO = new CaDETClassDTO();
            TestIdentifier = "public void testMethod();";
            MetricsDTO metricsForIdentifier = new MetricsDTO();
            metricsForIdentifier.NOP = 0;
            CaDETClassDTO.CodeItemMetrics[TestIdentifier] = metricsForIdentifier;
            ExpectedIssues = 0; // there is no need for issues
        }
    }
}
