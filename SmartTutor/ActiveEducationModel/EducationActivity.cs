using System;

namespace SmartTutor.ActiveEducationModel
{
    public class EducationActivity
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ActivityStatus Status { get; set; }
        public Player Player { get; set; }

        public EducationActivity()
        {
        }

        public EducationActivity(DateTime start, DateTime end, ActivityStatus status, Player player)
        {
            Start = start;
            End = end;
            Status = status;
            Player = player;
        }

        public EducationActivity(EducationActivity educationActivity)
            : this(educationActivity.Start, educationActivity.End, educationActivity.Status, educationActivity.Player)
        {
        }
    }
}
