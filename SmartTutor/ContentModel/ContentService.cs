using System.Collections.Generic;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.Repository;
using SmartTutor.Recommenders;

namespace SmartTutor.ContentModel
{
    public class ContentService : IContentService
    {
        private readonly IRecommender _recommender;
        private readonly IContentRepository _contentRepository;

        public ContentService(IRecommender recommender, IContentRepository repository)
        {
            _recommender = recommender;
            _contentRepository = repository;
        }

        public List<Lecture> GetLectures()
        {
            return _contentRepository.GetLectures();
        }

        public Lecture GetFullLecture(int id)
        {
            return _contentRepository.GetLecture(id);
        }
    }
}