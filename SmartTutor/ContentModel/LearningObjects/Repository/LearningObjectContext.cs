using Microsoft.EntityFrameworkCore;
using SmartTutor.ContentModel.LectureModel.Repository;

namespace SmartTutor.ContentModel.LearningObjects.Repository
{
    public class LearningObjectContext : DbContext
    {
        public DbSet<LearningObject> LearningObjects { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Video> Videos { get; set; }

        public LearningObjectContext(DbContextOptions<LearningObjectContext> options) : base(options)
        {
        }
    }
}