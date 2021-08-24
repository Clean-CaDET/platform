using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;

namespace SmartTutor.ContentModel
{
    public class ContentService : IContentService
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly ILearningObjectRepository _learningObjectRepository;

        public ContentService(ILectureRepository lectureRepository, ILearningObjectRepository learningObjectRepository)
        {
            _lectureRepository = lectureRepository;
            _learningObjectRepository = learningObjectRepository;
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
            learningObjectSummary = _learningObjectRepository.SaveOrUpdateLearningObjectSummary(learningObjectSummary);
            foreach (var learningObject in learningObjectSummary.LearningObjects)
            {
                _learningObjectRepository.SaveOrUpdateLearningObject(learningObject);
            }
        }

        public List<LearningObject> GetLearningObjectsByLearningObjectSummary(int losId)
        {
            return _learningObjectRepository.GetLearningObjectsForSummary(losId);
        }

        public List<KnowledgeNode> GetKnowledgeNodesByLecture(int lectureId)
        {
            return _lectureRepository.GetKnowledgeNodesByLecture(lectureId);
        }

        public List<LearningObjectSummary> GetLearningObjectSummariesByNode(int nodeId)
        {
            return _learningObjectRepository.GetLearningObjectSummariesByNode(nodeId);
        }
    }
}