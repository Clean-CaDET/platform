using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTutor.ActiveEducationModel
{
    public class Player
    {
        //public Person Person { get; set; }
        public PlayerTitle Title { get; set; }
        public double Score { get; set; }
        public int Rank { get; set; }
        public List<EducationActivity> EducationActivities { get; set; }
    }
}
