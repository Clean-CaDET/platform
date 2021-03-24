using Microsoft.EntityFrameworkCore;

namespace SmartTutor.ContentModel.LectureModel.Repository
{
    public class LectureContext : DbContext
    {
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<KnowledgeNode> KnowledgeNodes { get; set; }
        public DbSet<LearningObjectSummary> LearningObjectSummaries { get; set; }

        public LectureContext(DbContextOptions<LectureContext> options) : base(options)
        {
        }
    }
}