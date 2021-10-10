using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.KnowledgeComponentModel
{
    public class KnowledgeComponentProgress
    {
        public int Id { get; private set; }
        
        public double Progress { get; private set; }
        
        public KnowledgeComponent KnowledgeComponent { get; private set; }
        
        public Learner Learner { get; private set; } 
        
        public void UpdateProgress() {}
    }
}