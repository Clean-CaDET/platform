using System.Collections.Generic;
using SmartTutor.ProgressModel.Progress;

namespace SmartTutor.LearnerModel.Learners
{
    public class Learner
    {
        public int Id { get; private set; }
        public string StudentIndex { get; private set; }

        // TODO: Entity framework cannot map dictionaries, requires refactoring
        public int VisualScore { get; private set; }
        public int AuralScore { get; private set; }
        public int ReadWriteScore { get; private set; }
        public int KinaestheticScore { get; private set; }
        public List<CourseEnrollment> CourseEnrollments { get; private set; }

        public Learner(int id, int visualScore, int auralScore, int readWriteScore, int kinaestheticScore,
            List<CourseEnrollment> courseEnrollments)
        {
            Id = id;
            VisualScore = visualScore;
            AuralScore = auralScore;
            ReadWriteScore = readWriteScore;
            KinaestheticScore = kinaestheticScore;
            CourseEnrollments = courseEnrollments;
        }

        private Learner()
        {
        }

        public Dictionary<LearningPreference, int> VARKScore()
        {
            return new Dictionary<LearningPreference, int>
            {
                {LearningPreference.Aural, AuralScore},
                {LearningPreference.Kinaesthetic, KinaestheticScore},
                {LearningPreference.Visual, VisualScore},
                {LearningPreference.ReadWrite, ReadWriteScore}
            };
        }

        public void UpdateVARK(Dictionary<LearningPreference, int> varkScore)
        {
            if (varkScore[LearningPreference.Aural] != 0) AuralScore = varkScore[LearningPreference.Aural];
            if (varkScore[LearningPreference.Visual] != 0) VisualScore = varkScore[LearningPreference.Visual];
            if (varkScore[LearningPreference.ReadWrite] != 0) ReadWriteScore = varkScore[LearningPreference.ReadWrite];
            if (varkScore[LearningPreference.Kinaesthetic] != 0)
                KinaestheticScore = varkScore[LearningPreference.Kinaesthetic];
        }
    }
}