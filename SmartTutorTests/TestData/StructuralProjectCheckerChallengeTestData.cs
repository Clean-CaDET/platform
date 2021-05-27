namespace SmartTutor.Tests.DataFactories
{
    internal class StructuralProjectCheckerChallengeTestData
    {
        public static string[] GetFourPassingClasses()
        {
            return new[]
           {
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
class PharmacyService
    {
        internal bool IsWorking(Pharmacist pharmacist, Stocktake stocktake)
        {
            //Check if pharmacist is on vacation.
            if (pharmacist.VacationSlots != null)
            {
                foreach (VacationSlot vacation in pharmacist.VacationSlots)
                {
                    DateTime vacationStart = vacation.StartTime;
                    DateTime vacationEnd = vacation.EndTime;

                    if (stocktake.StartTime > stocktake.EndTime) throw new InvalidOperationException(""Invalid stocktake time frame."");
                    //---s1---| vacationStart |---e1---s2---e2---s3---| vacationEnd |---e3---
                    if (stocktake.StartTime <= vacationEnd && stocktake.EndTime >= vacationStart)
            {
                return false;
            }
        }
    }

            return true;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
class VacationSlot
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public VacationSlot(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
class Stocktake
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public Stocktake(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        internal bool IsDoingStocktakes(Pharmacist pharmacist, Stocktake stocktake)
        {
        
            //Check if pharmacist is doing stocktakes at the time.
            if (pharmacist.Stocktakes != null)
            {
                foreach (Stocktake st in pharmacist.Operations)
                {
                    DateTime stStart = st.StartTime;
    DateTime stEnd = st.EndTime;

                    if (stocktake.StartTime > stocktake.EndTime) throw new InvalidOperationException(""Invalid stocktake time frame."");
                    //---s1---| oldStStart |---e1---s2---e2---s3---| oldStEnd |---e3---
                    if (stocktake.StartTime <= stEnd && stocktake.EndTime >= stStart)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
class Weekend
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public Weekend(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

         internal bool IsWeekend(Pharmacist pharmacist, Weekend weekend)
        {
            //Check if it is weekend.
            if (pharmacist.Weekends != null)
            {
                foreach (Weekend week in pharmacist.Weekends)
                {
                    DateTime weekendStart = week.StartTime;
                    DateTime weekendEnd = week.EndTime;

                    if (weekend.StartTime > weekend.EndTime) throw new InvalidOperationException(""Invalid weekend time frame."");
                    //---s1---| weekendStart |---e1---s2---e2---s3---| weekendEnd |---e3---
                    if (weekend.StartTime <= weekendEnd && weekend.EndTime >= weekendStart)
            {
                return false;
            }
        }
    }

            return true;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
class Pharmacist
    {
        public List<Stocktake> Stocktakes { get; }
        public List<VacationSlot> VacationSlots { get; }

        public Pharmacist()
        {
            Stocktake = new List<Stocktake>
            {
                new Stocktake(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2)),
            };
            VacationSlots = new List<VacationSlot>
            {
                new VacationSlot(DateTime.Now.AddDays(3), DateTime.Now.AddDays(4)),
                new VacationSlot(DateTime.Now.AddDays(5), DateTime.Now.AddDays(9))
            };
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

        public bool IsWorking(DateTime begin, DateTime end)
        {
            return _service.IsWorking(new Pharmacist(), new Stocktake(begin, end));
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
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
class PharmacyService
    {
        internal bool IsWorking(Pharmacist pharmacist, Stocktake stocktake)
        {
            //Check if pharmacist is on vacation.
            if (pharmacist.VacationSlots != null)
            {
                foreach (VacationSlot vacation in pharmacist.VacationSlots)
                {
                    DateTime vacationStart = vacation.StartTime;
                    DateTime vacationEnd = vacation.EndTime;

                    if (stocktake.StartTime > stocktake.EndTime) throw new InvalidOperationException(""Invalid stocktake time frame."");
                    //---s1---| vacationStart |---e1---s2---e2---s3---| vacationEnd |---e3---
                    if (stocktake.StartTime <= vacationEnd && stocktake.EndTime >= vacationStart)
                    {
                        return false;
                    }
                }
            }
        }

        internal bool IsDoingStocktakes(Pharmacist pharmacist, Stocktake stocktake)
        {
        
            //Check if pharmacist is doing stocktakes at the time.
            if (pharmacist.Stocktakes != null)
            {
                foreach (Stocktake st in pharmacist.Operations)
                {
                    DateTime stStart = st.StartTime;
    DateTime stEnd = st.EndTime;

                    if (stocktake.StartTime > stocktake.EndTime) throw new InvalidOperationException(""Invalid stocktake time frame."");
                    //---s1---| oldStStart |---e1---s2---e2---s3---| oldStEnd |---e3---
                    if (stocktake.StartTime <= stEnd && stocktake.EndTime >= stStart)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        internal bool IsWeekend(Pharmacist pharmacist, Weekend weekend)
        {
            //Check if it is weekend.
            if (pharmacist.Weekends != null)
            {
                foreach (Weekend week in pharmacist.Weekends)
                {
                    DateTime weekendStart = week.StartTime;
                    DateTime weekendEnd = week.EndTime;

                    if (weekend.StartTime > weekend.EndTime) throw new InvalidOperationException(""Invalid weekend time frame."");
                    //---s1---| weekendStart |---e1---s2---e2---s3---| weekendEnd |---e3---
                    if (weekend.StartTime <= weekendEnd && weekend.EndTime >= weekendStart)
            {
                return false;
            }
        }
    }

            return true;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
class VacationSlot
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public VacationSlot(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
class Stocktake
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public Stocktake(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
class Weekend
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public Weekend(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}",
@"using System;
using System.Collections.Generic;

namespace Classes.Structural
{
class Pharmacist
    {
        public List<Stocktake> Stocktakes { get; }
        public List<VacationSlot> VacationSlots { get; }

        public Pharmacist()
        {
            Stocktake = new List<Stocktake>
            {
                new Stocktake(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2)),
            };
            VacationSlots = new List<VacationSlot>
            {
                new VacationSlot(DateTime.Now.AddDays(3), DateTime.Now.AddDays(4)),
                new VacationSlot(DateTime.Now.AddDays(5), DateTime.Now.AddDays(9))
            };
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

        public bool IsWorking(DateTime begin, DateTime end)
        {
            return _service.IsWorking(new Pharmacist(), new Stocktake(begin, end));
        }
    }
    #endregion
}"
};
        }
    }
}
