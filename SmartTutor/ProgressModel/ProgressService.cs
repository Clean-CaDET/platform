using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.InstructorModel.Instructors;
using SmartTutor.ProgressModel.Progress;
using SmartTutor.ProgressModel.Progress.Repository;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ProgressModel
{
    public class ProgressService : IProgressService
    {
        private readonly IInstructor _instructor;
        private readonly ILectureRepository _lectureRepository;
        private readonly IProgressRepository _progressRepository;

        public ProgressService(IInstructor instructor, ILectureRepository lectureRepository,
            IProgressRepository progressRepository)
        {
            _instructor = instructor;
            _progressRepository = progressRepository;
            _lectureRepository = lectureRepository;
        }

        public List<NodeProgress> GetKnowledgeNodes(int lectureId, int? learnerId)
        {
            var nodes = _lectureRepository.GetKnowledgeNodes(lectureId);
            if (nodes == null) return null;
            if (learnerId == null) return ShowSampleNodes(nodes);
            //TODO: Check if prerequisites fulfilled.
            return null;
        }

        private static List<NodeProgress> ShowSampleNodes(List<KnowledgeNode> nodes)
        {
            return nodes.Select(n => new NodeProgress(0, 0, n, NodeStatus.Unlocked, null)).ToList();
        }

        public NodeProgress GetNodeContent(int knowledgeNodeId, int? learnerId)
        {
            var knowledgeNode = _lectureRepository.GetKnowledgeNodeWithSummaries(knowledgeNodeId);
            if (knowledgeNode == null) return null;

            if (learnerId == null)
            {
                return new NodeProgress(
                    0, 0, knowledgeNode, NodeStatus.Unlocked, _instructor.GatherDefaultLearningObjects(knowledgeNode.LearningObjectSummaries));
            }

            return BuildNodeForLearner(knowledgeNode, (int) learnerId);
        }

        private NodeProgress BuildNodeForLearner(KnowledgeNode node, int learnerId)
        {
            var nodeProgress = _progressRepository.GetNodeProgressForLearner(learnerId, node.Id) ?? new NodeProgress(
                0, learnerId, node, NodeStatus.Unlocked, _instructor.GatherLearningObjectsForLearner(learnerId, node.LearningObjectSummaries)
            );

            //TODO: Create learning session and save.
            _progressRepository.SaveNodeProgress(nodeProgress);

            return nodeProgress;
        }
    }
}