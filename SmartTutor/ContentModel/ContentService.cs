using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.InstructorModel;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.ProgressModel.Content;
using SmartTutor.ProgressModel.Content.Repository;
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
        private readonly IProgressRepository _progressRepository;
        private readonly ILearnerRepository _learnerRepository;

        public ContentService(IInstructor instructor, ILectureRepository lectureRepository,
            ILearningObjectRepository learningObjectRepository, IProgressRepository progressRepository, ILearnerRepository learnerRepository)
        {
            _instructor = instructor;
            _lectureRepository = lectureRepository;
            _learningObjectRepository = learningObjectRepository;
            _progressRepository = progressRepository;
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
                var nodeProgress = _progressRepository.GetNodeProgressForLearner((int) traineeId, knowledgeNodeId);
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
            _progressRepository.SaveNodeProgress(nodeProgress);

            //TODO: Create learning session

            return nodeProgress;
        }
    }
}