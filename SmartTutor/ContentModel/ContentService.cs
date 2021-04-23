using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using System.Collections.Generic;

namespace SmartTutor.ContentModel
{
    public class ContentService : IContentService
    {
        private readonly ILectureRepository _lectureRepository;

        public ContentService(ILectureRepository lectureRepository)
        {
            _lectureRepository = lectureRepository;
        }

        public List<Lecture> GetLectures()
        {
            return _lectureRepository.GetLectures();
        }
    }
}