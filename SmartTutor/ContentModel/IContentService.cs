using SmartTutor.ContentModel.LectureModel;
using System.Collections.Generic;

namespace SmartTutor.ContentModel
{
    public interface IContentService
    {
        List<Lecture> GetLectures();
        Lecture GetFullLecture(int id);
    }
}