using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.InstructorModel;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Repository;
using SmartTutor.ProgressModel.Submissions;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel
{
    public class ContentService : IContentService
    {
        //TODO: Redesign the modules as this is quickly becoming a god class.
        //TODO: Establish test suite and refactor.
        private readonly IInstructor _instructor;
        private readonly ILectureRepository _lectureRepository;
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly ILearnerRepository _learnerRepository;

        public ContentService(IInstructor instructor, ILectureRepository lectureRepository,
            ILearningObjectRepository learningObjectRepository, ILearnerRepository learnerRepository)
        {
            _instructor = instructor;
            _lectureRepository = lectureRepository;
            _learningObjectRepository = learningObjectRepository;
            _learnerRepository = learnerRepository;
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
            var knowledgeNode = _lectureRepository.GetKnowledgeNodeWithSummaries(knowledgeNodeId);
            if (knowledgeNode == null) return null;

            if (traineeId != null)
            {
                var nodeProgress = _learnerRepository.GetNodeProgressForLearner((int) traineeId, knowledgeNodeId);
                return nodeProgress ?? CreateNodeForLearner(knowledgeNode, (int) traineeId);
            }

            var learningObjects = _learningObjectRepository.GetFirstLearningObjectsForSummaries(
                knowledgeNode.LearningObjectSummaries.Select(s => s.Id).ToList());
            return new NodeProgress
            {
                Id = 0,
                LearningObjects = learningObjects,
                Node = knowledgeNode
            };
        }

        private NodeProgress CreateNodeForLearner(KnowledgeNode node, int learnerId)
        {
            var learner = _learnerRepository.GetById(learnerId);
            var nodeProgress = _instructor.BuildNodeProgressForLearner(learner, node);
            _learnerRepository.SaveNodeProgress(nodeProgress);

            //TODO: Create learning session

            return nodeProgress;
        }

        public List<AnswerEvaluation> EvaluateAnswers(QuestionSubmission submission)
        {
            //TODO: Discuss what is a submission and what is an evaluation and ensure we are using the appropriate terminology for our UL.
            var answers = _learningObjectRepository.GetQuestionAnswers(submission.QuestionId);
            //TODO: Some of the logic below should be moved to the LearningSession/NodeProgress aggregate
            //TODO: Probably need a QuestionEvaluation here.
            var evaluations = new List<AnswerEvaluation>();
            foreach (var answer in answers)
            {
                var answerWasMarked = submission.SubmittedAnswerIds.Contains(answer.Id);
                evaluations.Add(new AnswerEvaluation
                {
                    FullAnswer = answer,
                    SubmissionWasCorrect = answer.IsCorrect ? answerWasMarked : !answerWasMarked
                });
            }

            submission.IsCorrect = evaluations.Select(a => a.SubmissionWasCorrect).All(c => c);
            _learnerRepository.SaveQuestionSubmission(submission);

            return evaluations;
        }

        public List<ArrangeTaskContainerEvaluation> EvaluateArrangeTask(ArrangeTaskSubmission submission)
        {
            var containers = _learningObjectRepository.GetArrangeTaskContainers(submission.ArrangeTaskId);
            //TODO: Some of the logic below should be moved to the LearningSession/NodeProgress aggregate
            var evaluations = new List<ArrangeTaskContainerEvaluation>();
            foreach (var container in containers)
            {
                var submittedContainer = submission.Containers.Find(c => c.ContainerId == container.Id);
                //TODO: If null throw exception since it is an invalid submission and see what the controller should return following best practices.
                if (submittedContainer == null) return null;

                evaluations.Add(new ArrangeTaskContainerEvaluation
                {
                    FullAnswer = container,
                    SubmissionWasCorrect = container.IsCorrectSubmission(submittedContainer.ElementIds)
                });
            }

            submission.IsCorrect = evaluations.Select(e => e.SubmissionWasCorrect).All(c => c);
            _learnerRepository.SaveArrangeTaskSubmission(submission);

            return evaluations;
        }


    }
}