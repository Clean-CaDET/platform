using SmartTutor.ContentModel.Lectures;
using SmartTutor.ProgressModel;
using System;
using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.InstructionalModel
{
    internal class HybridRecommender : IInstructor
    {
        public NodeProgress BuildNodeProgressForTrainee(Learner learner, KnowledgeNode knowledgeNode)
        {
            throw new NotImplementedException();
        }
    }
}