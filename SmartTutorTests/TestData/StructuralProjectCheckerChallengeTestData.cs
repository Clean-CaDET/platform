namespace SmartTutor.Tests.DataFactories
{
    internal class StructuralProjectCheckerChallengeTestData
    {
        public static string[] GetPassingClasses()
        {
            return new[]
           {
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
public class PharmacyService
    {
        public List<Purchase> Purchases { get; set; }
        public List<Pharmacist> Pharmacists { get; set; }
        
        private List<Pharmacist> GetPharmacyPharmacists(List<int> pharmacistIds) 
        {
            // Get only pharmacists from this pharmacy.
            List<Pharmacist> pharmacists = new List<Pharmacist>();
            foreach (var pharmacist in Pharmacists)
            {
                foreach (var pharmacistId in pharmacistIds)
                {
                    if (pharmacist.Id == pharmacistId)
                    {
                        pharmacists.Add(pharmacist);
                    }
                }
            }
            return pharmacists;
        }

        private List<Purchase> GetPharmacyPharmacistsPurchases(List<int> pharmacistIds) 
        {
           // Get all purchases from pharmacists.
            List<Purchase> purchases = new List<Purchase>();
            foreach (var purchase in Purchases)
            {
                foreach (var pharmacist in GetPharmacyPharmacists(pharmacistIds))
                {
                    if (pharmacist == purchase.Pharmacist)
                    {
                        purchases.Add(purchase);
                    }
                }
            }
            return purchases;
        }

        private Purchase GetMostExpensiveGranulePurchase(List<Purchase> granulePurchases) 
        {
            // Get most expensive purchase.
            double maxGranulePurchaseCost = 0;
            Purchase mostExpensiveGranulePurchase = null;
            foreach (var granulePurchase in granulePurchases)
            {
                if (granulePurchase.Cost > maxGranulePurchaseCost)
                {
                    maxGranulePurchaseCost = granulePurchase.Cost;
                    mostExpensiveGranulePurchase = granulePurchase;
                }
            }

            return mostExpensiveGranulePurchase;
        }

        internal Purchase GetMostExpensiveGranulePurchaseInPharmacyForPharmacists(List<int> pharmacistIds)
        {
            List<Purchase> pharmacyPharmacistsPurchases = GetPharmacyPharmacistsPurchases(pharmacistIds);
            List<Purchase> granulePurchases = pharmacyPharmacistsPurchases[0].GetGranulePurchases(pharmacyPharmacistsPurchases);
            return GetMostExpensiveGranulePurchase(granulePurchases);
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
public class Pharmacist
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }

        public Pharmacist(int id, string firstName, string lastName, string email, string phoneNumber)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
public class Pill
    {
        public int Id { get; }
        public string Name { get; }
        public PillForm Form { get; }
        public DateTime ExpiryDate { get; }
        public double MgMeasure { get; }
        public double Price { get; }

        public Pill(int id, string name, PillForm form, DateTime expiryDate, double mgMeasure, double price)
        {
            Id = id;
            Name = name;
            Form = form;
            ExpiryDate = expiryDate;
            MgMeasure = mgMeasure;
            Price = price;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
 public enum PillForm
    {
        Tablet,
        Capsule,
        Spansule,
        Softgel,
        Liquid,
        Granule,
        Powder
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
public class Purchase
    {
        public int Id { get; }
        public double Cost { get; }
        public List<Pill> PurchasedPills { get; }
        public DateTime PurchaseTime { get; }
        public Pharmacist Pharmacist { get; }

        public Purchase(int id, double cost, List<Pill> purchasedPills, DateTime purchaseTime, Pharmacist pharmacist)
        {
            Id = id;
            Cost = cost;
            PurchasedPills = purchasedPills;
            PurchaseTime = purchaseTime;
            Pharmacist = pharmacist;
        }

        public List<Purchase> GetGranulePurchases(List<Purchase> purchases) 
        {
            // Get only purchases where bought pill is in granule form.
            List<Purchase> granulePurchases = new List<Purchase>();
            foreach (var purchase in purchases)
            {
                foreach (var purchasedPill in purchase.PurchasedPills)
                {
                    if (purchasedPill.Form == PillForm.Granule)
                    {
                        granulePurchases.Add(purchase);
                    }
                }
            }
            return granulePurchases;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
 #region Run
    public class Run
    {
        private readonly PharmacyService _service = new PharmacyService();

        public Run()
        {
            Pharmacist pharmacistFirst = new Pharmacist(12787, ""Lazar"", ""Višnjić"", ""lazar55visnjic@gmail.com"", ""021/7745-720"");
            Pharmacist pharmacistSecond = new Pharmacist(24221, ""Katarina"", ""Miljković"", ""KataRinaMiljkoVic00@gmail.com"", ""021/4364-001"");
            _service.Pharmacists = new List<Pharmacist> { pharmacistFirst, pharmacistSecond };

            Pill pillFirst = new Pill(789, ""Brufen"", PillForm.Granule, DateTime.Now.AddMonths(32), 400, 101.99);
            Pill pillSecond = new Pill(799, ""Vitamin C"", PillForm.Granule, DateTime.Now.AddMonths(15), 550, 100.00);
            Pill pillThird = new Pill(748, ""Vitamin B"", PillForm.Capsule, DateTime.Now.AddYears(1), 300, 570.35);
            Pill pillFourth = new Pill(712, ""Aspirin"", PillForm.Granule, DateTime.Now.AddMonths(21), 150, 237.50);
            _service.Purchases = new List<Purchase>
            {
                new Purchase(5550121, 401.99, new List<Pill> { pillFirst, pillSecond, pillSecond, pillSecond, pillSecond}, DateTime.Now.AddMinutes(150), pharmacistFirst),
                new Purchase(5550122, 807.85, new List<Pill> { pillThird, pillFourth }, DateTime.Now.AddHours(27), pharmacistSecond),
                new Purchase(5455510, 670.35, new List<Pill> { pillSecond, pillThird }, DateTime.Now.AddDays(3), new Pharmacist(44541, ""Sanja"", ""Gojković"", ""123sanja123@gmail.com"", ""021/7522-616""))
            };
        }

        public Purchase GetMostExpensiveGranulePurchaseInPharmacyForPharmacists(List<int> pharmacistIds)
        {
            return _service.GetMostExpensiveGranulePurchaseInPharmacyForPharmacists(pharmacistIds);
        }
    }
#endregion
}
"
};
        }

        public static string[] GetViolatingClasses()
        {
            return new[]
           {
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
public class PharmacyService
    {
        public List<Purchase> Purchases { get; set; }
        public List<Pharmacist> Pharmacists { get; set; }

        internal Purchase GetMostExpensiveGranulePurchaseInPharmacyForPharmacists(List<int> pharmacistIds)
        {
            // Get only pharmacists from this pharmacy.
            List<Pharmacist> pharmacists = new List<Pharmacist>();
            foreach (var pharmacist in Pharmacists)
            {
                foreach (var pharmacistId in pharmacistIds)
                {
                    if (pharmacist.Id == pharmacistId)
                    {
                        pharmacists.Add(pharmacist);
                    }
                }
            }

            // Get all purchases from pharmacists.
            List<Purchase> purchases = new List<Purchase>();
            foreach (var purchase in Purchases)
            {
                foreach (var pharmacist in pharmacists)
                {
                    if (pharmacist == purchase.Pharmacist)
                    {
                        purchases.Add(purchase);
                    }
                }
            }

            // Get only purchases where bought pill is in granule form.
            List<Purchase> granulePurchases = new List<Purchase>();
            foreach (var purchase in purchases)
            {
                foreach (var purchasedPill in purchase.PurchasedPills)
                {
                    if (purchasedPill.Form == PillForm.Granule)
                    {
                        granulePurchases.Add(purchase);
                    }
                }
            }

            // Get most expensive purchase.
            double maxGranulePurchaseCost = 0;
            Purchase mostExpensiveGranulePurchase = null;
            foreach (var granulePurchase in granulePurchases)
            {
                if (granulePurchase.Cost > maxGranulePurchaseCost)
                {
                    maxGranulePurchaseCost = granulePurchase.Cost;
                    mostExpensiveGranulePurchase = granulePurchase;
                }
            }

            return mostExpensiveGranulePurchase;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
public class Pharmacist
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }

        public Pharmacist(int id, string firstName, string lastName, string email, string phoneNumber)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
public class Pill
    {
        public int Id { get; }
        public string Name { get; }
        public PillForm Form { get; }
        public DateTime ExpiryDate { get; }
        public double MgMeasure { get; }
        public double Price { get; }

        public Pill(int id, string name, PillForm form, DateTime expiryDate, double mgMeasure, double price)
        {
            Id = id;
            Name = name;
            Form = form;
            ExpiryDate = expiryDate;
            MgMeasure = mgMeasure;
            Price = price;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
 public enum PillForm
    {
        Tablet,
        Capsule,
        Spansule,
        Softgel,
        Liquid,
        Granule,
        Powder
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
public class Purchase
    {
        public int Id { get; }
        public double Cost { get; }
        public List<Pill> PurchasedPills { get; }
        public DateTime PurchaseTime { get; }
        public Pharmacist Pharmacist { get; }

        public Purchase(int id, double cost, List<Pill> purchasedPills, DateTime purchaseTime, Pharmacist pharmacist)
        {
            Id = id;
            Cost = cost;
            PurchasedPills = purchasedPills;
            PurchaseTime = purchaseTime;
            Pharmacist = pharmacist;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
 #region Run
    public class Run
    {
        private readonly PharmacyService _service = new PharmacyService();

        public Run()
        {
            Pharmacist pharmacistFirst = new Pharmacist(12787, ""Lazar"", ""Višnjić"", ""lazar55visnjic@gmail.com"", ""021/7745-720"");
            Pharmacist pharmacistSecond = new Pharmacist(24221, ""Katarina"", ""Miljković"", ""KataRinaMiljkoVic00@gmail.com"", ""021/4364-001"");
            _service.Pharmacists = new List<Pharmacist> { pharmacistFirst, pharmacistSecond };

            Pill pillFirst = new Pill(789, ""Brufen"", PillForm.Granule, DateTime.Now.AddMonths(32), 400, 101.99);
            Pill pillSecond = new Pill(799, ""Vitamin C"", PillForm.Granule, DateTime.Now.AddMonths(15), 550, 100.00);
            Pill pillThird = new Pill(748, ""Vitamin B"", PillForm.Capsule, DateTime.Now.AddYears(1), 300, 570.35);
            Pill pillFourth = new Pill(712, ""Aspirin"", PillForm.Granule, DateTime.Now.AddMonths(21), 150, 237.50);
            _service.Purchases = new List<Purchase>
            {
                new Purchase(5550121, 401.99, new List<Pill> { pillFirst, pillSecond, pillSecond, pillSecond, pillSecond}, DateTime.Now.AddMinutes(150), pharmacistFirst),
                new Purchase(5550122, 807.85, new List<Pill> { pillThird, pillFourth }, DateTime.Now.AddHours(27), pharmacistSecond),
                new Purchase(5455510, 670.35, new List<Pill> { pillSecond, pillThird }, DateTime.Now.AddDays(3), new Pharmacist(44541, ""Sanja"", ""Gojković"", ""123sanja123@gmail.com"", ""021/7522-616""))
            };
        }

        public Purchase GetMostExpensiveGranulePurchaseInPharmacyForPharmacists(List<int> pharmacistIds)
        {
            return _service.GetMostExpensiveGranulePurchaseInPharmacyForPharmacists(pharmacistIds);
        }
    }
#endregion
}
"
};
        }
    }
}
