using System;
using SmellDetector.Controllers;
using SmellDetector.Services;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Linq;
using Xunit;
using Shouldly;
using SmellDetector.Communication;
using SmellDetectorTests.DataFactory;
using System.Collections.Generic;

namespace SmellDetectorTests.Unit
{
    public class SmellDetectorServiceTest
    {
        [Fact]
        public void Generate_Smell_Detection_Report_For_LongMethod_And_Long_Parameter_List_Issues()
        {
            DetectionService detectionService = new DetectionService();
            CaDETClassDTOFactory classFactory = new CaDETClassDTOFactory();

            classFactory.CreateIssuesLongMethodAndLongParameterList();
            var report = detectionService.GenerateSmellDetectionReport(classFactory.CaDETClassDTO);
            report.Report[classFactory.TestIdentifier].Count().ShouldBe(classFactory.ExpectedIssues);
        }

        [Fact]
        public void Generate_Smell_Detection_Report_For_LongMethod_Issue()
        {
            DetectionService detectionService = new DetectionService();
            CaDETClassDTOFactory classFactory = new CaDETClassDTOFactory();

            classFactory.CreateIssueLongMethod();
            var report = detectionService.GenerateSmellDetectionReport(classFactory.CaDETClassDTO);
            report.Report[classFactory.TestIdentifier].Count().ShouldBe(classFactory.ExpectedIssues);
        }

        [Fact]
        public void Generate_Smell_Detection_Report_For_Another_LongMethod_Issue()
        {
            DetectionService detectionService = new DetectionService();
            CaDETClassDTOFactory classFactory = new CaDETClassDTOFactory();

            classFactory.CreateAnotherIssueLongMethod();
            var report = detectionService.GenerateSmellDetectionReport(classFactory.CaDETClassDTO);
            report.Report[classFactory.TestIdentifier].Count().ShouldBe(classFactory.ExpectedIssues);
        }

        [Fact]
        public void Generate_Smell_Detection_Report_Without_Issues()
        {
            DetectionService detectionService = new DetectionService();
            CaDETClassDTOFactory classFactory = new CaDETClassDTOFactory();

            classFactory.CreateEmptyIssue();
            var report = detectionService.GenerateSmellDetectionReport(classFactory.CaDETClassDTO);
            report.Report.Count().ShouldBe(classFactory.ExpectedIssues);
        }
    }
}
