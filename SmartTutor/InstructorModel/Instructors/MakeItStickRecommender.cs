using System;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures;

namespace SmartTutor.InstructorModel.Instructors
{
    public class MakeItStickRecommender : IInstructor
    {
        private readonly VARKRecommender _varkRecommender;
        private readonly ILearningObjectRepository _learningObjectRepository;

        public MakeItStickRecommender(VARKRecommender varkRecommender,
            ILearningObjectRepository learningObjectRepository)
        {
            _varkRecommender = varkRecommender;
            _learningObjectRepository = learningObjectRepository;
        }

        public List<LearningObject> GatherLearningObjectsForLearner(int learnerId,
            List<LearningObjectSummary> learningObjectSummaries)
        {
            var result = new List<LearningObject>();
            var knowledgeNode = learningObjectSummaries[0].KnowledgeNode;
            result.AddRange(GatherPrerequisiteLearningObjectSummaries(knowledgeNode));
            result.AddRange(_varkRecommender.GatherLearningObjectsForLearner(learnerId, learningObjectSummaries));
            result.AddRange(GatherKnowledgeNodeLearningObjectSummary(knowledgeNode));
            return result;
        }

        public List<LearningObject> GatherDefaultLearningObjects(List<LearningObjectSummary> learningObjectSummaries)
        {
            return _varkRecommender.GatherDefaultLearningObjects(learningObjectSummaries);
        }

        private List<LearningObject> GatherPrerequisiteLearningObjectSummaries(KnowledgeNode knowledgeNode)
        {
            Random random = new Random();
            var result = new List<LearningObject>();
            foreach (var prerequisiteNode in knowledgeNode.Prerequisites ?? new List<KnowledgeNode>())
            {
                int index = random.Next(0, prerequisiteNode.LearningObjectSummaries.Count);
                var summary = prerequisiteNode.LearningObjectSummaries[index];
                var question = _learningObjectRepository.GetQuestionForSummary(summary.Id);
                if (question != null) result.Add(question);
            }

            return result;
        }

        private List<LearningObject> GatherKnowledgeNodeLearningObjectSummary(KnowledgeNode knowledgeNode)
        {
            return knowledgeNode.LearningObjectSummaries
                .Select(summary => _learningObjectRepository.GetQuestionForSummary(summary.Id))
                .Where(question => question != null).Cast<LearningObject>().ToList();
        }
    }
}