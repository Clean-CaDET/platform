using SmartTutor.ContentModel.LectureModel;
using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LectureModel.Repository;

namespace SmartTutor.Recommenders
{
    public class KnowledgeBasedRecommender : IRecommender
    {
        private readonly ILectureRepository _lectureRepository;

        public KnowledgeBasedRecommender(ILectureRepository lectureRepository)
        {
            _lectureRepository = lectureRepository;
        }

        public List<LearningObject> FindEducationalContent(List<SmellType> issues)
        {
            var result = new List<LearningObject>();
            //TODO: Contact ContentService 
            return result;
        }
    }
}