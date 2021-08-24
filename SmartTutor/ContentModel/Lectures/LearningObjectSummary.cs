using SmartTutor.QualityAnalysis;
using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel.Lectures
{
    public class LearningObjectSummary
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public int KnowledgeNodeId { get; set; }
        public List<LearningObject> LearningObjects { get; private set; }

        public LearningObjectSummary(int id, string description)
        {
            Id = id;
            Description = description;
        }

        //TODO: Remove if EF6 supports unidirectional many-to-many
        private List<IssueAdvice> Advice { get; set; }
    }
}