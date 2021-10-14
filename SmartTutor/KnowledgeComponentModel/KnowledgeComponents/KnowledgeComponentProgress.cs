using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.KnowledgeComponentModel.KnowledgeComponents
{
    public class KnowledgeComponentProgress
    {
        public int Id { get; private set; }
        
        public double Progress { get; private set; }
        
        public KnowledgeComponent KnowledgeComponent { get; private set; } //TODO: KC parent id
        
        public Learner Learner { get; private set; } //TODO: Learner id
        
        public void UpdateProgress() {}
    }
}