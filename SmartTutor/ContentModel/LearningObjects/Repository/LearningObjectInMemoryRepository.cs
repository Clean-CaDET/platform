using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.Repository
{
    public class LearningObjectInMemoryRepository : ILearningObjectRepository
    {
        //Key is mapped to LearningObjectSummaryId.
        private readonly Dictionary<int, List<LearningObject>> _learningObjectCache;

        public LearningObjectInMemoryRepository()
        {
            //TODO: Will be updated from DB on App startup.
            _learningObjectCache = new Dictionary<int, List<LearningObject>>();
            _learningObjectCache.Add(1, new List<LearningObject>
            {
                new Text { Id = 1, LearningObjectSummaryId = 1, Content = "Cohesion determines the degree to which a part of a codebase forms a meaningful atomic module. The elements of a highly cohesive module work together towards a common, well-defined goal and have a clear (single) responsibility. This responsibility is defined by the module’s name and described by its interface that sets its inputs and outputs." }
            });

            _learningObjectCache.Add(2, new List<LearningObject>
            {
                new Image { Id = 2, LearningObjectSummaryId = 2, Url = "https://miro.medium.com/max/2400/1*3jfye6OQFu_dROKb14BhaQ.png", Caption = "The left class is playing with a few responsibilities, more than its name suggests anyway…" }
            });

            _learningObjectCache.Add(3, new List<LearningObject>
            {
                new Text { Id = 3, LearningObjectSummaryId = 3, Content = "Structural cohesion is a metric that is calculated based on the number of connections between a module’s elements." }
            });

            _learningObjectCache.Add(4, new List<LearningObject>
            {
                new Image { Id = 4, LearningObjectSummaryId = 4, Url = "https://miro.medium.com/max/700/1*OF4xmCDnuV_VRDcqiLp46Q.png", Caption = "How does this formula hold for data transfer object classes? What about classes without fields?" }
            });

            _learningObjectCache.Add(5, new List<LearningObject>
            {
                new Text { Id = 5, LearningObjectSummaryId = 5, Content = "Semantic cohesion determines the degree to which the elements of a module are semantically related." }
            });

            _learningObjectCache.Add(6, new List<LearningObject>
            {
                new Video { Id = 6, LearningObjectSummaryId = 6, Url = "https://www.youtube.com/watch?v=qE-Gmu_YuQE" }
            });

        }

        public List<LearningObject> GetLearningObjectsForSummary(int summaryId)
        {
            return _learningObjectCache[summaryId];
        }
    }
}
