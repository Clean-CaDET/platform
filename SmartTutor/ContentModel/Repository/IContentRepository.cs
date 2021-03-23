using System.Collections.Generic;
using SmartTutor.ContentModel.LectureModel;

namespace SmartTutor.ContentModel.Repository
{
    public interface IContentRepository
    {
        List<Lecture> GetLectures();
        Lecture GetLecture(int id);
    }
}


