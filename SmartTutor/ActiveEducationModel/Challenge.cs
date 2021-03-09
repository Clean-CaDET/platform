using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTutor.ActiveEducationModel
{
    public class Challenge : EducationActivity
    {
        public ChallengeProject Project { get; set; }
       // public List<ChallengePrerequisite> Prerequisites { get; set; }
       // public List<BonusQuestion> BonusQuestions { get; set; }
    }
}
