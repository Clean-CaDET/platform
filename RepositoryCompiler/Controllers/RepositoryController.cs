using Microsoft.AspNetCore.Mvc;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Communication;
using RepositoryCompiler.Controllers.DTOs;
using System;

namespace RepositoryCompiler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly CodeRepositoryService _repositoryService;

        public RepositoryController(CodeRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        [HttpPost("parse/class")]
        public ClassMetricsDTO GetBasicClassMetrics([FromBody] string classCode)
        {
            var retVal = new ClassMetricsDTO(_repositoryService.BuildClassModel(classCode));
            return retVal;
        }
        //TODO: Should be moved to the smart tutor module
        [HttpPost("education/class")]
        public ClassQualityAnalysisResponse GetClassMetricsWithCohesionFeedback([FromBody] string classCode)
        {

            Guid id = Guid.NewGuid();
            CaDETClass parsedClass = _repositoryService.BuildClassModel(classCode);
            var metrics = new ClassMetricsDTO(parsedClass);
            EducationalContentDTO content = DetermineSuitableContent(parsedClass);

            /*            MessageProducer producer = new MessageProducer();*/
            CaDETClassDTO reportMessage = CreateMockupMetricsReportMessage();
            ApplicationBuilderExtentions._producer.CreateNewMetricsReport(reportMessage);
            id = reportMessage.Id;


            return new ClassQualityAnalysisResponse
            {
                Id = id,
                Content = content,
                Metrics = metrics
            };
        }

        // TODO: Delete Mockup
        public CaDETClassDTO CreateMockupMetricsReportMessage()
        {
            Guid id = Guid.NewGuid();
            CaDETClassDTO report = new CaDETClassDTO(id);

            string exampleMethodId = "public void SavePerson(Person person);";

            MetricsDTO metrics = new MetricsDTO();
            metrics.LOC = 1000;
            metrics.NOLV = 100;
            metrics.NOP = 1;

            report.CodeItemMetrics[exampleMethodId] = metrics;

            return report;
        }

        // TODO: Add GUID for return type here !
        [HttpPost("process/class")]
        public void ActivateProcessOfClassParsing([FromBody] string classCode)
        {
            CaDETClass parsedClass = _repositoryService.BuildClassModel(classCode);
            CreateMetricsReport(parsedClass);
        }

        private void CreateMetricsReport(CaDETClass parsedClass)
        {
/*            MessageProducer producer = new MessageProducer();*/
            CaDETClassDTO reportMessage = new CaDETClassDTO(parsedClass);
            ApplicationBuilderExtentions._producer.CreateNewMetricsReport(reportMessage);
        }

        private EducationalContentDTO DetermineSuitableContent(CaDETClass parsedClass)
        {
            if (parsedClass.IsDataClass())
                return new EducationalContentDTO
                {
                    Title = "Detected a data class in " + parsedClass.FullName,
                    Image = "https://farm4.static.flickr.com/3095/3120129852_0b88bba034_o.jpg",
                    GeneralDescription = "Data classes are classes that primarily contain fields and accessors (getters and setters)."
                };
            if (parsedClass.Metrics.LCOM == null)
                return new EducationalContentDTO
                {
                    Title = "Class with no state in " + parsedClass.FullName,
                    Image = "https://4.bp.blogspot.com/-WRrvdhV0Zok/U4XhwF5nlBI/AAAAAAAAZvA/YNreQ0PYNjU/s1600/6.png",
                    GeneralDescription =
                        "Stateless classes do not contain any fields and are a collection of (often static) methods."
                };
            if (parsedClass.Metrics.LCOM > 0.5 && parsedClass.Metrics.NMD > 5 && parsedClass.Metrics.NAD > 4)
                return new EducationalContentDTO
                {
                    Title = "Low cohesion in class " + parsedClass.FullName,
                    Image = "https://thebojansblog.files.wordpress.com/2015/04/your-email-1.jpg",
                    GeneralDescription =
                        "Cohesion refers to the degree to which the elements inside a module belong together. For classes, it measures the strength of the relationship between the methods and data of a class - signaling how well the class represents a unified concept.",
                    DetectedIssue =
                        "Our analysis shows that this class has low cohesion, namely " + (1-parsedClass.Metrics.LCOM) + ". This signals that there might be two or more concepts represented by this class. Looking at the figure, we see how the class can be split into three classes, each containing a method and related field. Importantly, we presume the methods are not accessors (getters and setters).",
                    Recommendations =
                        "See if you can split the class by grouping the fields (or their subset) with methods that utilize them. If you can find suitable names for the two groups than you should perform the Extract Class refactoring. If one group is hard to define and sufficiently large, this might mean you have even more classes to extract."
                };
            return new EducationalContentDTO
            {
                Title = "Highly cohesive class " + parsedClass.FullName,
                Image = "https://thebojansblog.files.wordpress.com/2015/04/high_cohesion.jpg",
                GeneralDescription =
                    "Cohesion refers to the degree to which the elements inside a module belong together. For classes, it measures the strength of the relationship between the methods and data of a class - signaling how well the class represents a unified concept.",
                DetectedIssue =
                    "Our analysis shows that this class has high cohesion, namely " + (1 - parsedClass.Metrics.LCOM) + ". This signals that the class is well structured and represents a unified concept.",
                Recommendations =
                    "While cohesion can be a good indicator that a class has more than one responsibility, it is not in itself sufficient. See if this class has an appropriate name."
            };
        }
    }
}
