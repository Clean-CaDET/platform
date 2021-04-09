using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.ProgressModel;
using System;
using System.Collections.Generic;

namespace SmartTutor.Recommenders
{
    internal class CollaborativeFilteringRecommender : IRecommender
    {
        public List<LearningObject> FindEducationalContent(List<SmellType> issues)
        {
            throw new NotImplementedException();
        }

        public NodeProgress BuildNodeProgressForTrainee(Trainee trainee, KnowledgeNode knowledgeNode)
        {
            throw new NotImplementedException();
        }
    }
}