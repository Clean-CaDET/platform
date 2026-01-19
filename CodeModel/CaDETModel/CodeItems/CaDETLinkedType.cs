using System.Collections.Generic;

namespace CodeModel.CaDETModel.CodeItems
{
    /// <summary>
    /// Class representing the type of field, property, method return value, etc.
    /// 
    /// Examples:
    ///
    /// Field: private Doctor doctor;
    /// FullType: "Doctor"
    /// LinkedTypes: [DoctorApp.Model.Data.Doctor]
    /// 
    /// Property: private Dictionary<int, Doctor> Doctors { get; set; }
    /// FullType: "Dictionary<int, Doctor>"
    /// LinkedTypes: [DoctorApp.Model.Data.Doctor]
    ///
    /// Method: private List<Dictionary<Doctor, DateRange>> GetDoctors() {}
    /// FullType: "List<Dictionary<Doctor, DateRange>>"
    /// LinkedTypes: [DoctorApp.Model.Data.Doctor, DoctorApp.Model.Data.DateR.DateRange]
    /// </summary>
    public class CaDETLinkedType
    {
        public string FullType { get; set; }
        public List<CaDETClass> LinkedTypes { get; set; }
    }
}