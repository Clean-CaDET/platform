using System.Collections.Generic;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using DataSetExplorer.Core.AnnotationSchema.Model;

namespace DataSetExplorer.Infrastructure.Database
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
        public DbSet<HeuristicDefinition> Heuristics { get; set; }
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

            modelBuilder.Entity<RelatedInstance>()
                .Property(i => i.CouplingTypeAndStrength)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<Dictionary<CouplingType, int>>(m));

            
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

            modelBuilder.Entity<RelatedInstance>().HasOne<Instance>().WithMany(i => i.RelatedInstances)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<RelatedInstance>()
                .Property(i => i.RelationType)
                .HasConversion(new EnumToStringConverter<RelationType>());

            modelBuilder
                .Entity<CodeSmellDefinition>()
                .Property(c => c.SnippetType)
                .HasConversion<string>();
        }
    }
}
