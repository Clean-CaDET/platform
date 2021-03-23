using Microsoft.EntityFrameworkCore;
using SmartTutor.ContentModel.LectureModel;

namespace SmartTutor.ContentModel.Repository
{
    public class ContentModelContext : DbContext
    {
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<KnowledgeNode> KnowledgeNodes { get; set; }
        public DbSet<LearningObject> LearningObjects { get; set; }

        public ContentModelContext(DbContextOptions<ContentModelContext> options) : base(options)
        {
        }
    }
}