using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Submissions;
using System.Collections.Generic;

namespace SmartTutor.ContentModel
{
    public interface IContentService
    {
        List<Lecture> GetLectures();
        List<NodeProgress> GetKnowledgeNodes(int lectureId, int? traineeId);
        NodeProgress GetNodeContent(int nodeId, int? traineeId);
        List<AnswerEvaluation> EvaluateAnswers(QuestionSubmission submission);
        List<ArrangeTaskContainerEvaluation> EvaluateArrangeTask(ArrangeTaskSubmission submission);
    }
}