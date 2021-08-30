using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.InstructorModel.PrerequisiteSelectionStrategies;

namespace SmartTutor.InstructorModel.Instructors
{
    public class MakeItStickInstructor : IInstructor
    {
        private readonly IInstructor _instructor;
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly ILectureRepository _lectureRepository;
        private readonly IPrerequisiteSelectionStrategy _prerequisiteSelectionStrategy;

        public MakeItStickInstructor(IInstructor instructor,
            ILearningObjectRepository learningObjectRepository, ILectureRepository lectureRepository,
            IPrerequisiteSelectionStrategy prerequisiteSelectionStrategy)
        {
            _instructor = instructor;
            _learningObjectRepository = learningObjectRepository;
            _lectureRepository = lectureRepository;
            _prerequisiteSelectionStrategy = prerequisiteSelectionStrategy;
        }

        public List<LearningObject> GatherLearningObjectsForLearner(int learnerId,
            List<LearningObjectSummary> learningObjectSummaries)
        {
            var knowledgeNode = _lectureRepository.GetKnowledgeNode(learningObjectSummaries[0].KnowledgeNode.Id);
            var result = _instructor.GatherLearningObjectsForLearner(learnerId, learningObjectSummaries);
            AddPrerequisiteRecapLOs(knowledgeNode, result);
            AddKnowledgeNodeRecapLOs(knowledgeNode, result);
            return result.Distinct().ToList();
        }

        public List<LearningObject> GatherDefaultLearningObjects(List<LearningObjectSummary> learningObjectSummaries)
        {
            return _instructor.GatherDefaultLearningObjects(learningObjectSummaries);
        }

        private void AddPrerequisiteRecapLOs(KnowledgeNode knowledgeNode, List<LearningObject> result)
        {
            var prerequisites = _prerequisiteSelectionStrategy.GetPrerequisites(knowledgeNode);
            foreach (var prerequisiteNode in prerequisites)
            {
                var summaryId = prerequisiteNode.RecapLearningObjectSummary.Id;
                var recapLO = _learningObjectRepository.GetLearningObjectForSummary(summaryId);
                if (recapLO != null) result.Insert(0, recapLO);
            }
        }

        private void AddKnowledgeNodeRecapLOs(KnowledgeNode knowledgeNode, List<LearningObject> result)
        {
            var recapSummaryId = knowledgeNode.RecapLearningObjectSummary.Id;
            var recapLO = _learningObjectRepository.GetLearningObjectForSummary(recapSummaryId);
            if (recapLO != null) result.Add(recapLO);
        }
    }
}