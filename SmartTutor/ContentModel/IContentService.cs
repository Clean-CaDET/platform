using SmartTutor.ContentModel.Lectures;
using System.Collections.Generic;
using SmartTutor.ContentModel.DTOs;

namespace SmartTutor.ContentModel
{
    public interface IContentService
    {
        List<Lecture> GetLectures();
        void CreateCourse(CreateCourseDto dto);
        void CreateLecture(CreateLectureDto dto);
    }
}