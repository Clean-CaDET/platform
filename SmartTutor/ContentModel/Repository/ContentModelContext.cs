using Microsoft.EntityFrameworkCore;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LectureModel;

namespace SmartTutor.ContentModel.Repository
{
    public class ContentModelContext : DbContext
    {
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<KnowledgeNode> KnowledgeNodes { get; set; }
        public DbSet<LearningObjectSummary> LearningObjectSummaries { get; set; }
        public DbSet<LearningObject> LearningObjects { get; set; }

        public ContentModelContext(DbContextOptions<ContentModelContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lecture>()
                .Property(l => l.KnowledgeNodes)
                .HasColumnType("jsonb");
        }
    }
}