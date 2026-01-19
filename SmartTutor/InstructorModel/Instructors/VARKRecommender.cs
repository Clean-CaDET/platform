using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.LearnerModel.Learners.Repository;
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

        public List<LearningObject> GatherLearningObjectsForLearner(int learnerId, List<LearningObjectSummary> loSummaries)
        {
            var learner = _learnerRepository.GetById(learnerId);
            var sortedPreferences = learner.VARKScore().OrderByDescending(key => key.Value).ToList();

            var learningObjects = new List<LearningObject>();
            foreach (var summary in loSummaries)
            {
                var learningObject = sortedPreferences
                    .Select(preference => GetLearningObjectForPreference(preference.Key, summary.Id))
                    .FirstOrDefault(learningObj => learningObj != null) ?? GetDefaultLearningObject(summary.Id);
                learningObjects.Add(learningObject);
            }

            return learningObjects;
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

        public List<LearningObject> GatherDefaultLearningObjects(List<LearningObjectSummary> loSummaries)
        {
            return _learningObjectRepository.GetFirstLearningObjectsForSummaries(
                loSummaries.Select(s => s.Id).ToList());
        }
    }
}