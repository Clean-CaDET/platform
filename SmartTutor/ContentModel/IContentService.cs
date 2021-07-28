using SmartTutor.ContentModel.Lectures;
using System.Collections.Generic;
using SmartTutor.Controllers.Content.DTOs;

namespace SmartTutor.ContentModel
{
    public interface IContentService
    {
        List<Lecture> GetLectures();
        void CreateCourse(Course course, int teacherId);
        void CreateLecture(Lecture lecture, int teacherId);
        List<LectureDTO> GetLecturesByTeachersId(int id);
        List<CourseDto> GetCoursesByTeachersId(int id);
    }
}