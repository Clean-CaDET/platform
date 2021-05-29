namespace SmartTutor.Tests.DataFactories
{
    internal class SemanticProjectCheckerChallengeTestData
    {
        public static string[] GetPassingClasses()
        {
            return new[]
           {
@"
using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Semantic
{
     public class Pharmacist
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<Stocktake> StocktakesDone { get; set; }

        public bool HasAllVitaminsForDay(DateTime day)
        {
            foreach (var stocktake in StocktakesDone)
            {
                if (stocktake.DayOfStocktake.Date.Equals(day.Date))
                {
                    foreach (var vitamin in stocktake.Vitamins)
                    {
                        if (vitamin.Value <= 0) return false;
                    }
                }
            }

            return true;
        }

        public List<int> GetAllNotProfitablePharmacistStocktakeMonthsForYear(Pharmacist pharmacist, int year)
        {
            List<int> allNotProfitableMonths = new List<int>();
            foreach (var stocktake in pharmacist.StocktakesDone)
            {
                DateTime timeOfStocktake = stocktake.DayOfStocktake;
                if (stocktake.Profit <= 0 && timeOfStocktake.Year == year && !allNotProfitableMonths.Contains(timeOfStocktake.Month))
                {
                    allNotProfitableMonths.Add(timeOfStocktake.Month);
                }
            }
            return allNotProfitableMonths;
        }
    }
}",
@"using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Semantic
{
public class Stocktake
    {
        public Dictionary<string, int> Medicines { get; }
        public Dictionary<string, int> Vitamins { get; }
        public double Profit { get; }
        public DateTime DayOfStocktake { get; }

        public Stocktake(Dictionary<string, int> medicines, Dictionary<string, int> vitamins, double profit, DateTime dayOfStocktake)
        {
            Medicines = medicines;
            Vitamins = vitamins;
            Profit = profit;
            DayOfStocktake = dayOfStocktake;
        }

        public bool IsProfitableStocktakeForDay(Stocktake stocktake, DateTime day)
        {
            bool isDayOfStocktake = stocktake.DayOfStocktake.Date.Equals(day.Date);
            bool isProfitable = stocktake.Profit > 0;
            return isDayOfStocktake && isProfitable;
        }

        public List<string> GetAllStocktakeResourcesNames(Stocktake stocktake)
        {
            List<string> allResources = new List<string>();
            foreach (var medicine in stocktake.Medicines)
            {
                allResources.Add(medicine.Key);
            }
            foreach (var vitamin in stocktake.Vitamins)
            {
                allResources.Add(vitamin.Key);
            }
            return allResources;
        }
    }
}",
@"using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Semantic
{
 #region Run
    public class Run
    {
        private readonly Pharmacist _pharmacist;

        public Run()
        {
            _pharmacist.Id = 135671;
            _pharmacist.FullName = ""Petar Milenković"";
            _pharmacist.StocktakesDone = new List<Stocktake> {
            new Stocktake(new Dictionary<string, int>
                {
                    { ""Brufen"", 15 },
                    { ""Aspirin"", 81 },
                    { ""Panadol"", 0 },
                    { ""Paracetamol"", 1 }
                },
                new Dictionary<string, int>
                {
                    { ""Vitamin C"", 3 },
                    { ""Vitamin B"", 24 }
                },
                -359,
                DateTime.Now
            ),
            new Stocktake(new Dictionary<string, int>
                {
                    { ""Brufen"", 78 },
                    { ""Aspirin"", 0 },
                    { ""Panadol"", 0 },
                    { ""Paracetamol"", 14 }
                }, new Dictionary<string, int>
                {
                    { ""Vitamin C"", 5 },
                    { ""Vitamin B"", 15 }
                },
                671,
                DateTime.Now.AddDays(31)
            ),
            new Stocktake(new Dictionary<string, int>
                {
                    { ""Brufen"", 0 },
                    { ""Aspirin"", 47 },
                    { ""Panadol"", 6 },
                    { ""Paracetamol"", 7 }
                },
                new Dictionary<string, int>
                {
                    { ""Vitamin C"", 0 },
                    { ""Vitamin B"", 21 }
                },
                783,
                DateTime.Now.AddDays(62)
            )};
        }

        public bool HasAllVitaminsForDay(DateTime day)
        {
            return _pharmacist.HasAllVitaminsForDay(day);
        }
    }
#endregion
}
" };
        }

        public static string[] GetViolatingClasses()
        {
            return new[]
          {
@"
using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Semantic
{
     public class Pharmacist
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<Stocktake> StocktakesDone { get; set; }

        public bool HasAllVitaminsForDay(DateTime day)
        {
            foreach (var stocktake in StocktakesDone)
            {
                if (stocktake.DayOfStocktake.Date.Equals(day.Date))
                {
                    foreach (var vitamin in stocktake.Vitamins)
                    {
                        if (vitamin.Value <= 0) return false;
                    }
                }
            }

            return true;
        }

        public bool IsProfitableStocktakeForDay(Stocktake stocktake, DateTime day)
        {
            bool isDayOfStocktake = stocktake.DayOfStocktake.Date.Equals(day.Date);
            bool isProfitable = stocktake.Profit > 0;
            return isDayOfStocktake && isProfitable;
        }

        public List<string> GetAllStocktakeResourcesNames(Stocktake stocktake)
        {
            List<string> allResources = new List<string>();
            foreach (var medicine in stocktake.Medicines)
            {
                allResources.Add(medicine.Key);
            }
            foreach (var vitamin in stocktake.Vitamins)
            {
                allResources.Add(vitamin.Key);
            }
            return allResources;
        }
    }
}",
@"using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Semantic
{
public class Stocktake
    {
        public Dictionary<string, int> Medicines { get; }
        public Dictionary<string, int> Vitamins { get; }
        public double Profit { get; }
        public DateTime DayOfStocktake { get; }

        public Stocktake(Dictionary<string, int> medicines, Dictionary<string, int> vitamins, double profit, DateTime dayOfStocktake)
        {
            Medicines = medicines;
            Vitamins = vitamins;
            Profit = profit;
            DayOfStocktake = dayOfStocktake;
        }

        public List<int> GetAllNotProfitablePharmacistStocktakeMonthsForYear(Pharmacist pharmacist, int year)
        {
            List<int> allNotProfitableMonths = new List<int>();
            foreach (var stocktake in pharmacist.StocktakesDone)
            {
                DateTime timeOfStocktake = stocktake.DayOfStocktake;
                if (stocktake.Profit <= 0 && timeOfStocktake.Year == year && !allNotProfitableMonths.Contains(timeOfStocktake.Month))
                {
                    allNotProfitableMonths.Add(timeOfStocktake.Month);
                }
            }
            return allNotProfitableMonths;
        }

    }
}",
@"using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Semantic
{
 #region Run
    public class Run
    {
        private readonly Pharmacist _pharmacist;

        public Run()
        {
            _pharmacist.Id = 135671;
            _pharmacist.FullName = ""Petar Milenković"";
            _pharmacist.StocktakesDone = new List<Stocktake> {
            new Stocktake(new Dictionary<string, int>
                {
                    { ""Brufen"", 15 },
                    { ""Aspirin"", 81 },
                    { ""Panadol"", 0 },
                    { ""Paracetamol"", 1 }
                },
                new Dictionary<string, int>
                {
                    { ""Vitamin C"", 3 },
                    { ""Vitamin B"", 24 }
                },
                -359,
                DateTime.Now
            ),
            new Stocktake(new Dictionary<string, int>
                {
                    { ""Brufen"", 78 },
                    { ""Aspirin"", 0 },
                    { ""Panadol"", 0 },
                    { ""Paracetamol"", 14 }
                }, new Dictionary<string, int>
                {
                    { ""Vitamin C"", 5 },
                    { ""Vitamin B"", 15 }
                },
                671,
                DateTime.Now.AddDays(31)
            ),
            new Stocktake(new Dictionary<string, int>
                {
                    { ""Brufen"", 0 },
                    { ""Aspirin"", 47 },
                    { ""Panadol"", 6 },
                    { ""Paracetamol"", 7 }
                },
                new Dictionary<string, int>
                {
                    { ""Vitamin C"", 0 },
                    { ""Vitamin B"", 21 }
                },
                783,
                DateTime.Now.AddDays(62)
            )};
        }

        public bool HasAllVitaminsForDay(DateTime day)
        {
            return _pharmacist.HasAllVitaminsForDay(day);
        }
    }
#endregion
}
" };
        }
    }
}
