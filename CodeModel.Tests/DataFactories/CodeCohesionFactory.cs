using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCompilerTests.DataFactories
{
    class CodeCohesionFactory
    {
        public IEnumerable<string> GetCohesionClasses()
        {
            return new[]
            {
                @"
            using System.Collections.Generic;
            namespace DoctorApp.Model.Data.DateR
            {
                public class DateRange
                {
                    public DateTime From { get; set; }
                    public DateTime To { get; set; }

                    public DateRange(DateTime from, DateTime to)
                    {
                        From = from;
                        To = to;
                        if(To.Equals(From)) return;
                    }
                    public bool OverlapsWith(DateRange timeSpan)
                    {
                        return !(From > timeSpan.To || To < timeSpan.From);
                    }
                }
            }",
                @"
            using System.Collections.Generic;
            using DoctorApp.Model.Data.DateR;
            namespace DoctorApp.Model.Data
            {
                public class Doctor
                {
                    public string Test;
                    public string Name { get; set; }
                    public string Email { get; set; }
                    public List<DateRange> HolidayDates { get; set; }

                    public Doctor(string name, string email)
                    {
                        Name = name;
                        Email = email;
                        HolidayDates = new List<DateRange>();
                    }

                    public void ProcessTest()
                    {
                        Test = null;
                    }

                    public string GetTwoNames()
                    {
                        return Name + Name;
                    }

                    public string GetThreeEmails()
                    {
                        return Email + Email + Email;
                    }
                }
            }"
            };
        }

        public IEnumerable<string> GetTCCMultipleClassTexts()
        {
            return new[]
            {
                @"
            using System.Collections.Generic;
            namespace NDCApp.Model.Data
            {
                public class Class6
                {
                    private int a1;
                    private int a2;
                    private int a3;
                    private int a4;
                   
 
                    internal bool m1IsEqual()
                    {
                        if((a1 == a2)){
                             return true;
                        }
                        return false;
                    }
                    internal bool m2IsEqual()
                    {
                        if((a1 == a3)){
                             return true;
                        }
                        return false;
                    }
                    internal bool m3IsEqual()
                    {
                        if((a3 == a4)){
                             return true;
                        }
                        return false;
                    }
   
                    internal bool m4IsEqual()
                    {          
                        if((a2 == a4)){
                             return true;
                        }
                        return false;
                    }
                }
            }",
                @"
            using System.Collections.Generic;
            namespace NDCApp.Model.Data
            {
                public class Class7
                {
                    private int a1;
                    private int a2;
                    private int a3;
                    private int a4;
                   
 
                    internal bool m1IsEqual()
                    {
                        if((a1 == 5)){
                             return true;
                        }
                        return false;
                    }
 
                    internal bool m2IsEqualM1()
                    {
                        if((a1 == 6)){
                             return true;
                        }
                        return false;
                    }
 
                    internal bool m3IsEqualm3()
                    {
                        if((a1 == 7)){
                             return true;
                        }
                        return false;
                    }
   
                    internal bool IsEqualm4()
                    {          
                        return false;
                    }
                }
            }",
                 @"
            using System.Collections.Generic;
            namespace NDCApp.Model.Data
            {
                public class Class8
                {
                    private int a1;
                    private int a2;
                    private int a3;
                    private int a4;
                   
 
                    internal bool m1IsEqual()
                    {
                        if((a1 == a2 == a3)){
                             return true;
                        }
                        return false;
                    }
 
                    internal bool m2IsEqualM1()
                    {
                        if((a1 == a2 == a3)){
                             return true;
                        }
                        return false;
                    }
 
                    internal bool m3IsEqualm3()
                    {
                        if((a1 == a2 == a3)){
                             return true;
                        }
                        return false;
                    }
   
                    internal bool IsEqualm4()
                    {          
                        return false;
                    }
                }
            }"
            };
        }

        public IEnumerable<string> GetICBMCTestClasses()
        {
            return new[]
            {
            @"
            namespace TestApp
            {
                public class TestClass0
                {
                    private int a;
                    private int b;

                    public bool IsAZero()
                    {
                        int asdf = b;
                        return a == 0;
                    }
                    public bool APlusB()
                    {
                        double asdf = 324;
                        return a + b;
                    }
                }
            }",
            @"
            namespace TestApp
            {
                public class TestClass01
                {
                    private int a;
                    private int b;

                    public bool IsAZero()
                    {
                        int asdf = 21;
                        return a == 0;
                    }
                    public bool APlusB()
                    {
                        double asdf = 324;
                        return a + b;
                    }
                }
            }",
            @"
            namespace TestApp
            {
                public class TestClass02
                {
                    private int a;
                    private int b;

                    public bool IsAZero()
                    {
                        int x = 21;
                        return a == 0;
                    }
                    public int APlusNumPlusB()
                    {
                        int x = 324;
                        return a + x + b;
                    }
                    public int BPlusNumPlusA()
                    {
                        int x = 12;
                        return b + x + a;
                    }
                }
            }",
            @"
            namespace TestApp
            {
                public class TestClass1
                {
                    private int a;
                    private int b;
                    private int c;

                    public bool IsAZero()
                    {
                        int asdf =  a;
                        return a == 0;
                    }
                    public bool IsAPlusBEqualsC()
                    {
                        double asdf = 324;
                        return a + b == c;
                    }
                    public bool IsAEqualsBPlusC()
                    {
                        int w;
                        return a == b + c;
                    }
                    public bool IsCZero()
                    {
                        int asdf = c;
                        return c == 0;
                    }
                }
            }",
                @"
            namespace TestApp
            {
                public class TestClass2
                {
                    private int a;
                    private int b;
                    private int c;

                    public bool IsAC()
                    {
                        int x = 0;
                        return a == c;
                    }
                    public bool IsAPlusBEqualsC()
                    {
                        int x = 0;
                        return a + b == c;
                    }
                    public bool IsAEqualsBPlusC()
                    {
                        int x = 0;
                        return a == b + c;
                    }
                    public bool IsCZero()
                    {
                        int x = 0;
                        return c == 0;
                    }
                }
            }",

                @"
            namespace TestApp
            {
                public class TestClass3
                {
                    private int a;
                    private int b;
                    private int c;

                    public bool IsAC()
                    {
                        int x = 0;
                        return a == c;
                    }
                    public bool IsAPlusBEqualsC()
                    {
                        int x = 0;
                        return a + b == c;
                    }
                    public bool IsAEqualsBPlusC()
                    {
                        int x = 0;
                        return a == b + c;
                    }
                    public bool IsCZeroPlusA()
                    {
                        int x = 0;
                        return c == 0 + a;
                    }
                }
            }",

                @"
            namespace TestApp
            {
                public class TestClass4
                {
                    private int a;
                    private int b;
                    private int c;

                    public bool IsAZero()
                    {
                        int x = 0;
                        return a == 0;
                    }
                    public bool IsAEqualsToC()
                    {
                        int x = 0;
                        return a == c;
                    }
                    public bool IsBEqualToC()
                    {
                        int x = 0;
                        return b == c;
                    }
                    public bool IsCZero()
                    {
                        int x = 0;
                        return c == 0;
                    }
                }
            }",

                @"
            namespace TestApp
            {
                public class TestClass5
                {
                    private int a;
                    private int b;
                    private int c;

                    public bool IsAZero()
                    {
                        int x = 0;
                        return a == 0;
                    }
                    public bool IsAEqualsToCPlusB()
                    {
                        int x = 0;
                        return a == c + b;
                    }
                    public bool IsBEqualToC()
                    {
                        int x = 0;
                        return b == c;
                    }
                    public bool IsCZero()
                    {
                        int x = 0;
                        return c == 0;
                    }
                }
            }"
            };
        }
    }
}
