using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.ProgressModel;
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