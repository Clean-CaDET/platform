using System;
using System.Collections.Generic;

namespace ExamplesApp.Method.Violation
{
    class DoctorService
    {
        /// <summary>
        /// 1) By looking at the comments, extract the appropriate methods.
        /// 2) Identify similar code in the new methods and reduce code duplication by extracting a shared method.
        /// </summary>
        /// <param name="doctor"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        bool IsAvailable(Doctor doctor, Operation operation)
        {
            //Check if doctor is on vacation.
            if (doctor.GetVacationSlots() != null)
            {
                foreach (VacationSlot vacation in doctor.GetVacationSlots())
                {
                    DateTime vacationStart = vacation.GetStartTime();
                    DateTime vacationEnd = vacation.GetEndTime();

                    if (operation.GetStartTime() > operation.GetEndTime()) throw new InvalidOperationException("Invalid operation time frame.");
                    //---s1---| vacationStart |---e1---s2---e2---s3---| vacationEnd |---e3---
                    if (operation.GetStartTime() <= vacationEnd && operation.GetEndTime() >= vacationStart)
                    {
                        return false;
                    }
                }
            }

            //Check if doctor has operations at the time.
            if (doctor.GetOperations() != null)
            {
                foreach (Operation op in doctor.GetOperations())
                {
                    DateTime opStart = op.GetStartTime();
                    DateTime opEnd = op.GetEndTime();

                    if (operation.GetStartTime() > operation.GetEndTime()) throw new InvalidOperationException("Invalid operation time frame.");
                    //---s1---| oldOpStart |---e1---s2---e2---s3---| oldOpEnd |---e3---
                    if (operation.GetStartTime() <= opEnd && operation.GetEndTime() >= opStart)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    internal class VacationSlot
    {
        public DateTime GetStartTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetEndTime()
        {
            throw new NotImplementedException();
        }
    }

    internal class Operation
    {
        public DateTime GetStartTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetEndTime()
        {
            throw new NotImplementedException();
        }
    }

    internal class Doctor
    {
        public List<Operation> GetOperations()
        {
            throw new System.NotImplementedException();
        }

        public List<VacationSlot> GetVacationSlots()
        {
            throw new NotImplementedException();
        }
    }
}
