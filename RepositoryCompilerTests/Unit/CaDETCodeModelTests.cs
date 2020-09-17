using System.Linq;
using RepositoryCompiler.CodeModel.CaDETModel;
using Shouldly;
using Xunit;

namespace RepositoryCompilerTests.Unit
{
    public class CaDETCodeModelTests
    {
        //This test is a safety net for the C# SyntaxParser and serves to check
        //the understanding of the API. It should probably be removed in the long run.
        [Fact]
        public void Compiles_CSharp_class_with_appropriate_fields_and_methods()
        {
            string classText = GetSimpleClassText();
            
            CaDETDocument document = new CaDETDocument("", classText, LanguageEnum.CSharp);

            document.Classes.ShouldHaveSingleItem();
            var doctorClass = document.Classes.First();
            doctorClass.GetMetricNAD().ShouldBe(0);
            doctorClass.GetMetricNMD().ShouldBe(5);
            doctorClass.Methods.ShouldContain(method => method.IsAccessor && method.Name.Equals("Email"));
            doctorClass.Methods.ShouldContain(method => method.IsConstructor);
            doctorClass.Methods.ShouldContain(method => !method.IsConstructor && !method.IsAccessor && method.Name.Equals("IsAvailable"));
            doctorClass.Methods.First().Parent.SourceCode.ShouldBe(doctorClass.SourceCode);
        }
        [Fact]
        public void Calculates_LOC_for_CSharp_class_elements()
        {
            var doc = new CaDETDocument("", GetSimpleClassText(), LanguageEnum.CSharp);
            var doctorClass = doc.Classes.First();

            doctorClass.GetMetricLOC().ShouldBe(22);
            doctorClass.Methods.Find(method => method.Name.Equals("Email")).GetMetricLOC().ShouldBe(1);
            doctorClass.Methods.Find(method => method.Name.Equals("IsAvailable")).GetMetricLOC().ShouldBe(8);
        }

        private string GetSimpleClassText()
        {
            return @"
            using System.Collections.Generic;
            namespace DoctorApp.Model.Data
            {
                public class Doctor
                {
                    public string Name { get; set; }
                    public string Email { get; set; }
                    public List<DateRange> HolidayDates { get; set; }

                    public Doctor(string name, string email)
                    {
                        Name = name;
                        Email = email;
                        HolidayDates = new List<DateRange>();
                    }

                    internal bool IsAvailable(DateRange timeSpan)
                    {
                        foreach (DateRange holiday in HolidayDates)
                        {
                            if (holiday.OverlapsWith(timeSpan)) return false;
                        }
                        return true;
                    }
                }
            }";
        }
    }
}
