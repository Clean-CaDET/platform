using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.ProjectChecker
{
    public class SnippetStrategies
    {
        public int Id { get; private set; }
        public string CodeSnippetId { get; private set; }
        public List<ChallengeFulfillmentStrategy> Strategies { get; private set; }
    }
}