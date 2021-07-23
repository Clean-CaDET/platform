using System;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Lectures.Repository;
using System.Collections.Generic;
using SmartTutor.ContentModel.DTOs;
using SmartTutor.ContentModel.Exceptions;

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

        public void CreateCourse(CreateCourseDto dto)
        {
            if (!_subscriptionService.CanAddCourse(dto.TeacherId)) throw new NotEnoughResourcesException();
            var course = new Course(dto.CourseName);
            course = _lectureRepository.SaveOrUpdateCourse(course);
            _subscriptionService.AddCourseToTeacher(dto.TeacherId, course);
            _subscriptionService.IncrementNumberOfCourses(dto.TeacherId);
        }

        public void CreateLecture(CreateLectureDto dto)
        {
            if (!_subscriptionService.CanAddLecture(dto.TeacherId)) throw new NotEnoughResourcesException();
            var lecture = new Lecture(dto.CourseId, dto.LectureName, dto.LectureDescription);
            var course = _lectureRepository.GetCourse(dto.CourseId);
            lecture = _lectureRepository.SaveOrUpdateLecture(lecture);
            course.AddLecture(lecture);
            _lectureRepository.SaveOrUpdateCourse(course);
            _subscriptionService.IncrementNumberOfLectures(dto.TeacherId);
        }
    }
}