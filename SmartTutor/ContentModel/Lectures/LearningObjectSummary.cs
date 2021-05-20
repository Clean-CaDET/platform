using SmartTutor.QualityAnalysis;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.Lectures
{
    public class LearningObjectSummary
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public KnowledgeNode KnowledgeNode {get; private set; }

        public LearningObjectSummary(int id, string description)
        {
            Id = id;
            Description = description;
        }

        //TODO: Remove if EF6 supports unidirectional many-to-many
        private List<IssueAdvice> Advice { get; set; }
    }
}