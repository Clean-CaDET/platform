using Microsoft.EntityFrameworkCore;

namespace SmartTutor.ContentModel
{
    public class SmartTutorContext : DbContext
    {
        private const string CONNECTION_STRING = "User ID =postgres;Password=super;Server=localhost;Port=5432;Database=smart-tutor-db;Integrated Security=true;Pooling=true;";
        public DbSet<EducationalSnippet> EducationalSnippets { get; set; }
        public DbSet<EducationalContent> EducationalContents { get; set; }

        public SmartTutorContext() : base()
        {
        }

        public SmartTutorContext(DbContextOptions<SmartTutorContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(CONNECTION_STRING);
        }
    }
}