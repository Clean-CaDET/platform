namespace SmartTutor.ContentModel
{
    using KnowledgeComponentModel.KnowledgeComponents;

    public abstract class AssessmentEvent
    {
        public KnowledgeComponent KnowledgeComponent { get; protected set; }
    }
}