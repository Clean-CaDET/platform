using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using System.Collections.Generic;
using SmartTutor.ContentModel.Exceptions;
using SmartTutor.Controllers.Content.DTOs;

namespace SmartTutor.ContentModel
{
    public class ContentService : IContentService
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly ISubscriptionService _subscriptionService;

        public ContentService(ILectureRepository lectureRepository, ISubscriptionService subscriptionService)
        {
            _lectureRepository = lectureRepository;
            _subscriptionService = subscriptionService;
        }

        public List<Lecture> GetLectures()
        {
            return _lectureRepository.GetLectures();
        }

        public void CreateCourse(Course course, int teacherId)
        {
            if (!_subscriptionService.CanAddCourse(teacherId)) throw new NotEnoughResourcesException();
            course = _lectureRepository.SaveOrUpdateCourse(course);
            _subscriptionService.AddCourseToTeacher(teacherId, course);
        }

        public void CreateLecture(Lecture lecture, int teacherId)
        {
            if (!_subscriptionService.CanAddLecture(teacherId)) throw new NotEnoughResourcesException();
            var course = _lectureRepository.GetCourse(lecture.CourseId);
            lecture = _lectureRepository.SaveOrUpdateLecture(lecture);
            course.AddLecture(lecture);
            _lectureRepository.SaveOrUpdateCourse(course);
            _subscriptionService.IncrementNumberOfLectures(teacherId);
        }

        public List<LectureDTO> GetLecturesByTeachersId(int id)
        {
            List<Lecture> lectures = _lectureRepository.GetLecturesOwnedByTeacher(id);
            var dtos = new List<LectureDTO>();
            foreach (var lecture in lectures)
            {
                dtos.Add(new LectureDTO()
                {
                    CourseId = lecture.CourseId, Id = lecture.Id, Description = lecture.Description, Name = lecture.Name
                });
            }

            return dtos;
        }

        public List<CourseDto> GetCoursesByTeachersId(int id)
        {
            List<Course> courses = _lectureRepository.GetCoursesOwnedByTeacher(id);
            var dtos = new List<CourseDto>();
            foreach (var course in courses)
            {
                dtos.Add(new CourseDto() {Name = course.Name, Id = course.Id});
            }

            return dtos;
        }
    }
}