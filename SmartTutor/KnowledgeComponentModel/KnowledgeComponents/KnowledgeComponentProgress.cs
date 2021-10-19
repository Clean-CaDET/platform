using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.KnowledgeComponentModel.KnowledgeComponents
{
    public class KnowledgeComponentProgress
    {
        public int Id { get; private set; }
        
        public double Progress { get; private set; }
        
        public int KnowledgeComponentId { get; private set; }
        
        public int LearnerId { get; private set; }
        
        public void UpdateProgress() {}
    }
}