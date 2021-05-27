namespace SmartTutor.Tests.DataFactories
{
    internal class SemanticProjectCheckerChallengeTestData
    {
        public static string[] GetFourPassingClasses()
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
        public int Id { get; }
        public ISet<Stocktake> Stocktakes { get; } = new HashSet<Stocktake>();

        public bool HasStocktakes(List<Stocktake> stocktakeSet)
        {
            foreach (var stocktake in stocktakeSet)
            {
                if (!Stocktakes.Contains(stocktake)) return false;
            }

            return true;
        }

        public void AssignStocktake(Stocktake stocktakeData)
        {
            if (Stocktakes.Contains(stocktakeData)) throw new ThePharmacistAlreadyHasStocktakeException(""Pharmacist: "" + Id.ToString() + ""Stock: "" + stocktakeData);
            Stocktakes.Add(stocktakeData);
        }

    }
}",
@"using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Semantic
{
public class ThePharmacistAlreadyHasStocktakeException : Exception
    {
        public ThePharmacistAlreadyHasStocktakeException(string id) : base(Id)
        {
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
        private readonly string _name;

        public Stocktake(string name)
        {
            _name = name;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Stocktake other)) return false;
            return other._name.Equals(_name);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
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
            _pharmacist = new Pharmacist();
            _pharmacist.AssignStocktake(new Stocktake(""Test 1""));
            _pharmacist.AssignStocktake(new Stocktake(""Test 2""));
            _pharmacist.AssignStocktake(new Stocktake(""Test 3""));
        }

        public void AddStock()
        {
            _pharmacist.AssignStocktake(new Stocktake(""Test 1""));
        }

        public List<Stocktake> GetStock()
        {
            return _pharmacist.Stocktakes.ToList();
        }

        public bool HasStock(List<Stocktake> all)
        {
            return _pharmacist.HasStocktake(all);
        }
    }
#endregion
}"
            };
        }

        public static string[] GetFourViolatingClasses()
        {
            return new[]
            {
@"
using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Semantic
{
    public class PharmacistInfo
    {
        public int PharmacistId { get; }
        public ISet<Stocktake> Stocktakes { get; } = new HashSet<Stocktake>();

        public bool HasStocktakes(List<Stocktake> stocktakeSet)
        {
            foreach (var stocktake in stocktakeSet)
            {
                if (!Stocktakes.Contains(stocktake)) return false;
            }

            return true;
        }

        public void AssignStocktake(Stocktake stocktakeData)
        {
            if (Stocktakes.Contains(stocktakeData)) throw new ThePharmacistAlreadyHasStocktakeException(""Pharmacist: "" + PharmacistId.ToString() + ""Stock: "" + stocktakeData);
            Stocktakes.Add(stocktakeData);
        }

    }
}",
@"using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Semantic
{
public class ThePharmacistAlreadyHasStocktakeException : Exception
    {
        public ThePharmacistAlreadyHasStocktakeException(string pharmacistId) : base(pharmacistId)
        {
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
        private readonly string _nameStr;

        public Stocktake(string name)
        {
            _nameStr = name;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Stocktake other)) return false;
            return other._nameStr.Equals(_nameStr);
        }

        public override int GetHashCode()
        {
            return _nameStr.GetHashCode();
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
        private readonly PharmacistInfo _pharmacist;

        public Run()
        {
            _pharmacist = new PharmacistInfo();
            _pharmacist.AssignStocktake(new Stocktake(""Test 1""));
            _pharmacist.AssignStocktake(new Stocktake(""Test 2""));
            _pharmacist.AssignStocktake(new Stocktake(""Test 3""));
        }

        public void AddStock()
        {
            _pharmacist.AssignStocktake(new Stocktake(""Test 1""));
        }

        public List<Stocktake> GetStock()
        {
            return _pharmacist.Stocktakes.ToList();
        }

        public bool HasStock(List<Stocktake> all)
        {
            return _pharmacist.HasStocktake(all);
        }
    }
#endregion
}"
            };
        }
    }
}
