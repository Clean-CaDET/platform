using RepositoryCompiler.Controllers;
using RepositoryCompilerTests.Unit;
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
            CodeModelTestDataFactory factory = new CodeModelTestDataFactory();

            CaDETClassDTO dto = ctrl.GetBasicClassMetrics(factory.GetDoctorClassText().First());

            dto.ShouldNotBeNull();
            dto.FullName.ShouldBe("DoctorApp.Model.Data.Doctor");
        }
    }
}
