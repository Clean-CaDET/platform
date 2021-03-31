using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LectureModel.Repository;
using SmartTutor.ContentModel.ProgressModel;
using SmartTutor.ContentModel.LectureModel;
using System.Collections.Generic;

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

        public NodeProgress BuildNodeProgressForTrainee(Trainee trainee, KnowledgeNode knowledgeNode)
        {
            throw new System.NotImplementedException();
        }
    }
}