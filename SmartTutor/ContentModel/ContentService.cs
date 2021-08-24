using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel
{
    public class ContentService : IContentService
    {
        private readonly ILectureRepository _lectureRepository;

        public ContentService(ILectureRepository lectureRepository)
        {
            _lectureRepository = lectureRepository;
        }

        public List<Lecture> GetLectures()
        {
            return _lectureRepository.GetLectures();
        }

        public void CreateKnowledgeNode(KnowledgeNode node)
        {
            _lectureRepository.SaveOrUpdateKnowledgeNode(node);
        }

        public void CreateLearningObjectSummary(LearningObjectSummary learningObjectSummary)
        {
            learningObjectSummary = _lectureRepository.SaveOrUpdateLearningObjectSummary(learningObjectSummary);
            foreach (var learningObject in learningObjectSummary.LearningObjects)
            {
                _lectureRepository.SaveOrUpdateLearningObject(learningObject);
            }
        }

        public List<LearningObject> GetLearningObjectsByLearningObjectSummary(int losId)
        {
            return _lectureRepository.GetLearningObjectsByLearningObjectSummary(losId);
        }

        public List<KnowledgeNode> GetKnowledgeNodesByLecture(int lectureId)
        {
            return _lectureRepository.GetKnowledgeNodesByLecture(lectureId);
        }

        public List<LearningObjectSummary> GetLearningObjectSummariesByNode(int nodeId)
        {
            return _lectureRepository.GetLearningObjectSummariesByNode(nodeId);
        }
    }
}