using Microsoft.EntityFrameworkCore;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.ProgressModel;

namespace SmartTutor.ContentModel.LectureModel.Repository
{
    public class SmartTutorContext : DbContext
    {
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<KnowledgeNode> KnowledgeNodes { get; set; }
        public DbSet<LearningObjectSummary> LearningObjectSummaries { get; set; }
        public DbSet<LearningObject> LearningObjects { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<NodeProgress> NodeProgresses { get; set; }
        public DbSet<Trainee> Trainees { get; set; }

        public SmartTutorContext(DbContextOptions<SmartTutorContext> options) : base(options)
        {
        }
    }
}