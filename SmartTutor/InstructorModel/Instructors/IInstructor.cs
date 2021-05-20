using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.Lectures;
using System.Collections.Generic;

namespace SmartTutor.InstructorModel.Instructors
{
    public interface IInstructor
    {
        List<LearningObject> GatherLearningObjectsForLearner(int learnerId, List<LearningObjectSummary> learningObjectSummaries);
        List<LearningObject> GatherDefaultLearningObjects(List<LearningObjectSummary> learningObjectSummaries);
    }
}