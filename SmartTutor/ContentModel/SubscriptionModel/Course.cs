using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartTutor.ContentModel.LectureModel;

namespace SmartTutor.ContentModel.SubscriptionModel
{
    public class Course
    {
        [Key] public int Id { get; set; }
        public List<Lecture> Lectures { get; set; }
        public List<Teacher> Teachers { get; set; }
        
    }
}