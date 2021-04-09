using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.LectureModel.Repository;
using SmartTutor.ContentModel.ProgressModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.Recommenders
{
    public class KnowledgeBasedRecommender : IRecommender
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly ILearningObjectRepository _learningObjectRepository;

        public KnowledgeBasedRecommender(ILectureRepository lectureRepository,
            ILearningObjectRepository learningObjectRepository)
        {
            _lectureRepository = lectureRepository;
            _learningObjectRepository = learningObjectRepository;
        }

        public List<LearningObject> FindEducationalContent(List<SmellType> issues)
        {
            var result = new List<LearningObject>();
            //TODO: Contact ContentService 
            return result;
        }

        public NodeProgress BuildNodeProgressForTrainee(Trainee trainee, KnowledgeNode knowledgeNode)
        {
            var learningObjects = new List<LearningObject>();
            var sortedPreferences = trainee.LearningPreferenceScore().OrderByDescending(key => key.Value).ToList();

            foreach (var summary in knowledgeNode.LearningObjectSummaries)
            {
                var learningObject = sortedPreferences
                    .Select(preference => GetLearningObjectForPreference(preference.Key, summary.Id))
                    .FirstOrDefault(learningObj => learningObj != null) ?? GetDefaultLearningObject(summary.Id);
                learningObjects.Add(learningObject);
            }

            return new NodeProgress
            {
                Trainee = trainee,
                Node = knowledgeNode,
                Status = NodeStatus.Started,
                LearningObjects = learningObjects
            };
        }

        private LearningObject GetLearningObjectForPreference(LearningPreference learningPreference, int summaryId)
        {
            return learningPreference switch
            {
                LearningPreference.Aural => _learningObjectRepository.GetVideoForSummary(summaryId),
                LearningPreference.Kinaesthetic => _learningObjectRepository.GetInteractiveLOForSummary(summaryId),
                LearningPreference.Visual => _learningObjectRepository.GetImageForSummary(summaryId),
                LearningPreference.ReadWrite => _learningObjectRepository.GetTextForSummary(summaryId),
                _ => throw new ArgumentOutOfRangeException(nameof(learningPreference), learningPreference, null)
            };
        }

        private LearningObject GetDefaultLearningObject(int summaryId)
        {
            return _learningObjectRepository.GetLearningObjectForSummary(summaryId);
        }
    }
}