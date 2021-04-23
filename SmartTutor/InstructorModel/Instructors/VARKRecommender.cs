using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.ProgressModel.Progress;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.InstructorModel.Instructors
{
    public class VARKRecommender : IInstructor
    {
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly ILearnerRepository _learnerRepository;

        public VARKRecommender(ILearningObjectRepository learningObjectRepository, ILearnerRepository learnerRepository)
        {
            _learningObjectRepository = learningObjectRepository;
            _learnerRepository = learnerRepository;
        }

        public NodeProgress BuildNodeForLearner(int learnerId, KnowledgeNode knowledgeNode)
        {
            var learner = _learnerRepository.GetById(learnerId);
            var sortedPreferences = learner.LearningPreferenceScore().OrderByDescending(key => key.Value).ToList();

            var learningObjects = new List<LearningObject>();
            foreach (var summary in knowledgeNode.LearningObjectSummaries)
            {
                var learningObject = sortedPreferences
                    .Select(preference => GetLearningObjectForPreference(preference.Key, summary.Id))
                    .FirstOrDefault(learningObj => learningObj != null) ?? GetDefaultLearningObject(summary.Id);
                learningObjects.Add(learningObject);
            }

            return new NodeProgress
            {
                LearnerId = learnerId,
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

        public NodeProgress BuildSimpleNode(KnowledgeNode knowledgeNode)
        {
            var learningObjects = _learningObjectRepository.GetFirstLearningObjectsForSummaries(
                knowledgeNode.LearningObjectSummaries.Select(s => s.Id).ToList());
            return new NodeProgress
            {
                LearningObjects = learningObjects,
                Node = knowledgeNode
            };
        }
    }
}