using System;

namespace SmartTutor.ActiveEducationModel
{
    public class EducationActivity
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ActivityStatus Status { get; set; }
        public Player Player { get; set; }
    }
}
