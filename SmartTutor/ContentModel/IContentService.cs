using SmartTutor.ContentModel.LectureModel;
using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.ProgressModel;

namespace SmartTutor.ContentModel
{
    public interface IContentService
    {
        List<Lecture> GetLectures();
        List<NodeProgress> GetKnowledgeNodes(int lectureId, int? traineeId);
        NodeProgress GetNodeContent(int nodeId, int? traineeId);
        List<AnswerEvaluation> EvaluateAnswers(int questionId, List<int> submittedAnswers);
        List<ArrangeTaskContainerEvaluation> EvaluateArrangeTask(int arrangeTaskId, List<ArrangeTaskContainer> submittedAnswers);
    }
}