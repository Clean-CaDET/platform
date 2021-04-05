using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.LectureModel.Repository;
using SmartTutor.ContentModel.ProgressModel;
using SmartTutor.Recommenders;
using System;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.ProgressModel;
using SmartTutor.ContentModel.ProgressModel.Repository;

namespace SmartTutor.ContentModel
{
    public class ContentService : IContentService
    {
        private readonly IRecommender _recommender;
        private readonly ILectureRepository _lectureRepository;
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly ITraineeRepository _traineeRepository;

        public ContentService(IRecommender recommender, ILectureRepository lectureRepository,
            ILearningObjectRepository learningObjectRepository, ITraineeRepository traineeRepository)
        {
            _recommender = recommender;
            _lectureRepository = lectureRepository;
            _learningObjectRepository = learningObjectRepository;
            _traineeRepository = traineeRepository;
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
            return nodes.Select(n => new NodeProgress {Node = n, Status = NodeStatus.Unlocked}).ToList();
        }

        public NodeProgress GetNodeContent(int knowledgeNodeId, int? traineeId)
        {
            if (traineeId != null)
            {
                return CreateNodeForTrainee(knowledgeNodeId, (int) traineeId);
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

        private NodeProgress CreateNodeForTrainee(int knowledgeNodeId, int traineeId)
        {
            var trainee = _traineeRepository.GetTraineeById(traineeId);
            var knowledgeNode = _lectureRepository.GetKnowledgeNodeWithSummaries(knowledgeNodeId);

            var nodeProgress = _recommender.BuildNodeProgressForTrainee(trainee, knowledgeNode);
            _traineeRepository.SaveNodeProgress(nodeProgress);

            //TODO: Create learning session

            return nodeProgress;
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

        public List<ArrangeTaskContainerEvaluation> EvaluateArrangeTask(int arrangeTaskId,
            List<ArrangeTaskContainer> submittedAnswers)
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