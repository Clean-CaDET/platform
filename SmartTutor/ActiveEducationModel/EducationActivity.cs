using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTutor.ActiveEducationModel
{
    public class EducationActivity
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ActivityStatus Status { get; set; }
        public SmellType ActivityType { get; set; }
        public Player Player { get; set; }
    }
}
