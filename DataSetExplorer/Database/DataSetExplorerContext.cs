using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSets.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using DataSetExplorer.Annotations.Model;

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
        public DbSet<CodeSmellDefinition> CodeSmellDefinitions { get; set; }
        public DbSet<Heuristic> Heuristics { get; set; }
        public DbSet<CodeSmellHeuristic> CodeSmellHeuristics { get; set; }
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
            
            modelBuilder.Entity<CodeSmellHeuristic>().HasKey(ch => new { ch.CodeSmellDefinitionId, ch.HeuristicId });

            modelBuilder.Entity<CodeSmell>().HasOne<DataSet>().WithMany(d => d.SupportedCodeSmells)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CodeSmell>().HasMany<SmellCandidateInstances>().WithOne(c => c.CodeSmell)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CodeSmell>().HasMany<Annotation>().WithOne(a => a.InstanceSmell)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DataSetProject>().HasOne<DataSet>().WithMany(d => d.Projects)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SmellCandidateInstances>().HasOne<DataSetProject>().WithMany(p => p.CandidateInstances)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Instance>().HasOne<SmellCandidateInstances>().WithMany(c => c.Instances)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Annotation>().HasOne<Instance>().WithMany(i => i.Annotations)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SmellHeuristic>().HasOne<Annotation>().WithMany(a => a.ApplicableHeuristics)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<CodeSmellDefinition>()
                .Property(c => c.SnippetType)
                .HasConversion<string>();
        }
    }
}
