using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.ProgressModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.InstructionalModel
{
    public class KnowledgeBasedRecommender : IInstructor
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly ILearningObjectRepository _learningObjectRepository;

        public KnowledgeBasedRecommender(ILectureRepository lectureRepository,
            ILearningObjectRepository learningObjectRepository)
        {
            _lectureRepository = lectureRepository;
            _learningObjectRepository = learningObjectRepository;
        }

        public NodeProgress BuildNodeProgressForTrainee(Learner learner, KnowledgeNode knowledgeNode)
        {
            var learningObjects = new List<LearningObject>();
            var sortedPreferences = learner.LearningPreferenceScore().OrderByDescending(key => key.Value).ToList();

            foreach (var summary in knowledgeNode.LearningObjectSummaries)
            {
                var learningObject = sortedPreferences
                    .Select(preference => GetLearningObjectForPreference(preference.Key, summary.Id))
                    .FirstOrDefault(learningObj => learningObj != null) ?? GetDefaultLearningObject(summary.Id);
                learningObjects.Add(learningObject);
            }

            return new NodeProgress
            {
                Learner = learner,
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