using System;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;

namespace SmartTutor.InstructorModel.Instructors
{
    public class MakeItStickInstructor : IInstructor
    {
        private readonly IInstructor _instructor;
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly ILectureRepository _lectureRepository;

        public MakeItStickInstructor(IInstructor instructor,
            ILearningObjectRepository learningObjectRepository, ILectureRepository lectureRepository)
        {
            _instructor = instructor;
            _learningObjectRepository = learningObjectRepository;
            _lectureRepository = lectureRepository;
        }

        public List<LearningObject> GatherLearningObjectsForLearner(int learnerId,
            List<LearningObjectSummary> learningObjectSummaries)
        {
            var knowledgeNode = learningObjectSummaries[0].KnowledgeNode;
            var result = _instructor.GatherLearningObjectsForLearner(learnerId, learningObjectSummaries);
            AddPrerequisiteRevisionQuestions(knowledgeNode.Id, result);
            AddKnowledgeNodeRevisionQuestions(knowledgeNode, result);
            return result.Distinct().ToList();
        }

        public List<LearningObject> GatherDefaultLearningObjects(List<LearningObjectSummary> learningObjectSummaries)
        {
            return _instructor.GatherDefaultLearningObjects(learningObjectSummaries);
        }

        private void AddPrerequisiteRevisionQuestions(int id, List<LearningObject> result)
        {
            Random random = new Random();
            KnowledgeNode knowledgeNode = _lectureRepository.GetKnowledgeNode(id);
            foreach (var prerequisiteNode in knowledgeNode.Prerequisites ?? new List<KnowledgeNode>())
            {
                int index = random.Next(0, prerequisiteNode.LearningObjectSummaries.Count);
                var summary = prerequisiteNode.LearningObjectSummaries[index];
                var question = _learningObjectRepository.GetQuestionForSummary(summary.Id);
                if (question != null) result.Insert(0, question);
            }
        }

        private void AddKnowledgeNodeRevisionQuestions(KnowledgeNode knowledgeNode, List<LearningObject> result)
        {
            foreach (var questions in knowledgeNode.LearningObjectSummaries.Select(summary =>
                _learningObjectRepository.GetQuestionsForSummary(summary.Id)))
            {
                result.AddRange(questions);
            }
        }
    }
}