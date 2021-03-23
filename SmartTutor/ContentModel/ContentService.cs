using SmartTutor.Recommenders;

namespace SmartTutor.ContentModel
{
    // TODO: Integrate here: Trainee repository & Recommender system
    public class ContentService
    {
        private IRecommender recommender;
        public ContentService(IRecommender recommender)
        {
            this.recommender = recommender;
        }
        
    }
}