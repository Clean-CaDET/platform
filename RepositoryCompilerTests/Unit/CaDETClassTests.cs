using System.Linq;
using RepositoryCompiler.CodeModel.CaDETModel;
using Shouldly;
using Xunit;

namespace RepositoryCompilerTests.Unit
{
    public class CaDETClassTests
    {
        [Fact]
        public void Compiles_CSharp_class_with_appropriate_fields_and_methods()
        {
            string classText = GetSimpleClassText();
            
            CaDETDocument document = new CaDETDocument("path", classText, LanguageEnum.CSharp);

            document.Classes.ShouldHaveSingleItem();
            var doctorClass = document.Classes.First();
            doctorClass.Fields.ShouldBeEmpty();
            doctorClass.Methods.ShouldContain(method => method.IsAccessor && method.Name.Equals("Email"));
            doctorClass.Methods.ShouldContain(method => method.IsConstructor);
            doctorClass.Methods.ShouldContain(method => !method.IsConstructor && !method.IsAccessor && method.Name.Equals("IsAvailable"));
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
