using System;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.LectureModel.Repository;
using SmartTutor.Recommenders;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.TraineeModel;

namespace SmartTutor.ContentModel
{
    public class ContentService : IContentService
    {
        private readonly IRecommender _recommender;
        private readonly ILectureRepository _lectureRepository;

        public ContentService(IRecommender recommender, ILectureRepository repository)
        {
            _recommender = recommender;
            _lectureRepository = repository;
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

        public NodeProgress GetNodeProgress(int traineeId, int knowledgeNodeId)
        {
            //TODO: Load KN
            //TODO: Load Trainee prefs
            //TODO: Get recommender to build NodeProgress with LOs for Trainee
            //TODO: Save started NodeProgress to repo
            //TODO: Return NodeProgress
            throw new NotImplementedException();
        }
    }
}