using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTutor.ContentModel
{
    class ContentWebApiContext : DbContext
    {
        public ContentWebApiContext(DbContextOptions<ContentWebApiContext> options) : base(options) { }
        public DbSet<EducationSnippet> EducationSnippets { get; set; }
        public DbSet<EducationContent> EducationContents { get; set; }
    }
}
