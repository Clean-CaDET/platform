using SmartTutor.ContentModel.LectureModel;
using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.Repository;

namespace SmartTutor.Recommenders
{
    public class KnowledgeBasedRecommender : IRecommender
    {
        private readonly IContentRepository _contentRepository;

        public KnowledgeBasedRecommender(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public List<LearningObject> FindEducationalContent(List<SmellType> issues)
        {
            var result = new List<LearningObject>();
            //TODO: Contact ContentService 
            return result;
        }
    }
}