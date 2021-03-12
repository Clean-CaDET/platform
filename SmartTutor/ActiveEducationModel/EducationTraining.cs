using SmartTutor.ContentModel;

namespace SmartTutor.ActiveEducationModel
{
    public class EducationTraining : EducationActivity
    {
        public double Points { get; set; }
        public EducationContent Content { get; set; }

        public EducationTraining() { }

        public EducationTraining(EducationActivity educationActivity) : base(educationActivity)
        {
            Points = 0;
            Content = new EducationContent();
        }
    }
}
