using LibGit2Sharp;
using System.Collections.Generic;

namespace SmartTutor.ActiveEducationModel
{
    public class Player
    {
        public UsernamePasswordCredentials Credentials { get; set; }
        public PlayerTitle Title { get; set; }
        public double Score { get; set; }
        public int Rank { get; set; }
        public List<EducationActivity> EducationActivities { get; set; }
    }
}
