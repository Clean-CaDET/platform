using System;
using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.ProgressModel;

namespace SmartTutor.Recommenders
{
    internal class HybridRecommender : IRecommender
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