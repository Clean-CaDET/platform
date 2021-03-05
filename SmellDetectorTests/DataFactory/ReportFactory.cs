using System.Collections.Generic;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetectorTests.DataFactory
{
    public class ReportFactory
    {
        public SmellDetectionReport CreateMockupReportMessage()
        {
            SmellDetectionReport reportMessage = new SmellDetectionReport();
            reportMessage.Report = new Dictionary<string, List<Issue>>();

            string exampleClassId = "public class Doctor";

            Issue detectedIssue = new Issue();
            detectedIssue.IssueType = SmellType.GOD_CLASS;
            detectedIssue.CodeItemId = exampleClassId;

            List<Issue> detectedIssues = new List<Issue>();
            detectedIssues.Add(detectedIssue);

            reportMessage.Report.Add(exampleClassId, detectedIssues);
            return reportMessage;
        }
    }
}