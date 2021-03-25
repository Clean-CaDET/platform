using System;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.LectureModel.Repository;
using SmartTutor.Recommenders;
using System.Collections.Generic;
using System.Linq;
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
            var sampleNodes = nodes.Where(n => n.HasNoPrerequisites());
            return sampleNodes.Select(n => new NodeProgress { Node = n, Status = NodeStatus.Unlocked }).ToList();
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
    }
}