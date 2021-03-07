using System;
using System.Collections.Generic;

namespace RepositoryCompilerTests.DataFactories.TestClasses
{
    public class DateRange
    {
        public int NumOfDays;
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public DateRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
            if (To.Equals(From)) return;
        }
        public bool OverlapsWith(DateRange timeSpan)
        {
            return !(From > timeSpan.To || To < timeSpan.From);
        }
    }
    public class Doctor
    {
        public DateRange TestDR;
        public string Test;
        public string Name { get; set; }
        public string Email { get; set; }
        public DateRange TestProp { get; set;}
        public List<DateRange> HolidayDates { get; set; }

        public Doctor(string name, string email)
        {
            Name = name;
            Email = email;
            HolidayDates = new List<DateRange>();
            TestDR = new DateRange(new DateTime(), new DateTime());
        }

        public DateRange TestFunction() {
            return TestProp;
        }

        public DateRange TestFieldFunction() {
            return TestDR;
        }
    }
    public class DoctorService
    {
        public Doctor TestDoc {get;set;}
        private List<Doctor> _doctors;
        public Doctor FindAvailableDoctor(DateRange timeSpan)
        {
            foreach (Doctor d in _doctors)
            {
                foreach(DateRange holiday in d.HolidayDates)
                {
                    d.Test = null;
                    if (!holiday.OverlapsWith(timeSpan)) return d;
                    LogChecked(33);
                }
            }
            return null;
        }
        private int LogChecked(int testData)
        {
            DateTime test1 = TestDoc.TestProp.From;
            DateTime test = _doctors[0].HolidayDates[0].From;
            var a = TestDoc.Name;
            var b = TestDoc.TestProp;
            var c = b.To;
            var temp1 = _doctors[0];
            temp1.Test = null;
            var temp2 = temp1.TestDR;
            int testNum = temp2.NumOfDays;
            var test2 = FindAvailableDoctor(temp2).TestFunction().OverlapsWith(temp2);
            return testData;
        }
    }
}
