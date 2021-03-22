using Microsoft.EntityFrameworkCore;

namespace SmartTutor.ContentModel
{
    public class SmartTutorContext : DbContext
    {
        public DbSet<EducationalSnippet> EducationalSnippets { get; set; }
        public DbSet<EducationalContent> EducationalContents { get; set; }

        public SmartTutorContext(DbContextOptions<SmartTutorContext> options) : base(options)
        {
        }
    }
}