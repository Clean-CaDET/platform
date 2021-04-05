using System;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.LectureModel.Repository;
using SmartTutor.Recommenders;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.ProgressModel;

namespace SmartTutor.ContentModel
{
    public class ContentService : IContentService
    {
        private readonly IRecommender _recommender;
        private readonly ILectureRepository _lectureRepository;
        private readonly ILearningObjectRepository _learningObjectRepository;

        public ContentService(IRecommender recommender, ILectureRepository lectureRepository, ILearningObjectRepository learningObjectRepository)
        {
            _recommender = recommender;
            _lectureRepository = lectureRepository;
            _learningObjectRepository = learningObjectRepository;
        }

        public List<Lecture> GetLectures()
        {
            return _lectureRepository.GetLectures();
        }

        public List<NodeProgress> GetKnowledgeNodes(int lectureId, int? traineeId)
        {
            var nodes = _lectureRepository.GetKnowledgeNodes(lectureId);
            if (traineeId == null) return ShowSampleNodes(nodes);

            return null;
        }

        private static List<NodeProgress> ShowSampleNodes(List<KnowledgeNode> nodes)
        {
            return nodes.Select(n => new NodeProgress { Node = n, Status = NodeStatus.Unlocked }).ToList();
        }

        public NodeProgress GetNodeContent(int knowledgeNodeId, int? traineeId)
        {
            if (traineeId != null)
            {
                return CreateNodeForTrainee(knowledgeNodeId, traineeId);
            }
            
            var knowledgeNode = _lectureRepository.GetKnowledgeNodeWithSummaries(knowledgeNodeId);
            if (knowledgeNode == null) return null;

            var learningObjects = _learningObjectRepository.GetFirstLearningObjectsForSummaries(
                knowledgeNode.LearningObjectSummaries.Select(s => s.Id).ToList());
            return new NodeProgress
            {
                Id = 0,
                LearningObjects = learningObjects,
                Node = knowledgeNode
            };
        }

        private NodeProgress CreateNodeForTrainee(int knowledgeNodeId, int? traineeId)
        {
            //TODO: Load Trainee prefs
            //TODO: Get recommender to build NodeProgress with LOs for Trainee
            //TODO: Save started NodeProgress to repo
            //TODO: Create learning session
            //TODO: Return NodeProgress

            throw new NotImplementedException();
        }

        public List<AnswerEvaluation> EvaluateAnswers(int questionId, List<int> submittedAnswers)
        {
            var answers = _learningObjectRepository.GetQuestionAnswers(questionId);
            //TODO: Tie in with the ProgressModel once it is setup, as the [submission]/[evaluation of submission] are important events.
            //TODO: Some of the logic below should be moved to the LearningSession/NodeProgress aggregate
            var evaluations = new List<AnswerEvaluation>();
            foreach (var answer in answers)
            {
                var answerWasMarked = submittedAnswers.Contains(answer.Id);
                evaluations.Add(new AnswerEvaluation
                {
                    FullAnswer = answer,
                    SubmissionWasCorrect = answer.IsCorrect ? answerWasMarked : !answerWasMarked
                });
            }
            return evaluations;
        }

        public List<ArrangeTaskContainerEvaluation> EvaluateArrangeTask(int arrangeTaskId, List<ArrangeTaskContainer> submittedAnswers)
        {
            var containers = _learningObjectRepository.GetArrangeTaskContainers(arrangeTaskId);
            //TODO: Tie in with the ProgressModel once it is setup, as the [submission]/[evaluation of submission] are important events.
            //TODO: Some of the logic below should be moved to the LearningSession/NodeProgress aggregate
            var evaluations = new List<ArrangeTaskContainerEvaluation>();
            foreach (var container in containers)
            {
                var submittedContainer = submittedAnswers.Find(c => c.Id == container.Id);
                //TODO: If null throw exception since it is an invalid submission and see what the controller should return following best practices.
                if (submittedContainer == null) return null;

                evaluations.Add(new ArrangeTaskContainerEvaluation
                {
                    FullAnswer = container,
                    SubmissionWasCorrect = container.IsCorrectSubmission(submittedContainer)
                });
            }
            return evaluations;
        }
    }
}