﻿namespace SmartTutor.ActiveEducationModel
{
    public class Challenge : EducationActivity
    {
        public ChallengeProject Project { get; set; }
        // public List<ChallengePrerequisite> Prerequisites { get; set; }
        // public List<BonusQuestion> BonusQuestions { get; set; }

        public Challenge() { }

        public Challenge(EducationActivity educationActivity) : base(educationActivity)
        {
            Project = new ChallengeProject();
        }
    }
}
