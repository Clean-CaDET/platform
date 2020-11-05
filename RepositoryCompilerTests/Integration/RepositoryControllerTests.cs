using RepositoryCompiler.Controllers;
using RepositoryCompiler.Controllers.DTOs;
using RepositoryCompilerTests.DataFactories;
using Shouldly;
using System.Linq;
using Xunit;

namespace RepositoryCompilerTests.Integration
{
    public class RepositoryControllerTests
    {
        [Fact]
        public void Gets_educational_content_for_class_cohesion()
        {
            RepositoryController ctrl = new RepositoryController(new CodeRepositoryService());
            CodeFactory factory = new CodeFactory();

            ClassMetricsDTO metricsDto = ctrl.GetBasicClassMetrics(factory.GetDoctorClassText().First());

            metricsDto.ShouldNotBeNull();
            metricsDto.FullName.ShouldBe("DoctorApp.Model.Data.Doctor");
        }
    }
}
