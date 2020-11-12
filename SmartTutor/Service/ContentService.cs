
using SmartTutor.ContentModel;
using SmartTutor.Recommenders;
using System.Collections.Generic;

namespace SmartTutor.Service
{
    // TODO: Integrate here: Trainee repository & Recommender system
    public class ContentService
    {
        private IRecommender recommender;
        public ContentService(IRecommender recommender)
        {
            this.recommender = recommender;
        }

        internal Dictionary<string, List<EducationalContent>> FindContentForIssue(Dictionary<string, List<SmellType>> issues)
        {
            Dictionary<string, List<EducationalContent>> content = new Dictionary<string, List<EducationalContent>>();
            foreach (string entityId in issues.Keys)
            {
                content.Add(entityId, recommender.FindEducationalContent(issues[entityId]));
            }
            return content;
        }
        
    }
}