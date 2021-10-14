using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSetBuilder.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;

namespace DataSetExplorer.Database
{
    public class DataSetExplorerContext : DbContext
    {
        public DbSet<Annotator> Annotators { get; set; }
        public DbSet<CodeSmell> CodeSmells { get; set; }
        public DbSet<SmellHeuristic> SmellHeuristics { get; set; }
        public DbSet<Annotation> DataSetAnnotations { get; set; }
        public DbSet<Instance> DataSetInstances { get; set; }
        public DbSet<DataSet> DataSets { get; set; }
        public DbSet<DataSetProject> DataSetProjects { get; set; }
        public DataSetExplorerContext(DbContextOptions<DataSetExplorerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instance>()
                .Property(i => i.MetricFeatures)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<Dictionary<CaDETMetric, double>>(m));
        }
    }
}
