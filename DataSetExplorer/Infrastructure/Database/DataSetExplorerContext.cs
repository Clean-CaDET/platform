﻿using System.Collections.Generic;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Core.CleanCodeAnalysis.Model;

namespace DataSetExplorer.Infrastructure.Database
{
    public class DataSetExplorerContext : DbContext
    {
        public DbSet<Annotator> Annotators { get; set; }
        public DbSet<CodeSmell> CodeSmells { get; set; }
        public DbSet<SmellHeuristic> SmellHeuristics { get; set; }
        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<Instance> Instances { get; set; }
        public DbSet<DataSet> DataSets { get; set; }
        public DbSet<DataSetProject> DataSetProjects { get; set; }
        public DbSet<CodeSmellDefinition> CodeSmellDefinitions { get; set; }
        public DbSet<HeuristicDefinition> HeuristicDefinitions { get; set; }
        public DbSet<SeverityDefinition> SeverityDefinitions { get; set; }
        public DbSet<SmellCandidateInstances> SmellCandidateInstances { get; set; }
        public DbSet<GraphInstance> GraphInstances { get; set; }
        public DbSet<GraphRelatedInstance> GraphRelatedInstances { get; set; }
        public DbSet<Identifier> Identifiers { get; set; }
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

            modelBuilder.Entity<GraphRelatedInstance>()
                .Property(i => i.CouplingTypeAndStrength)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<Dictionary<CouplingType, int>>(m));

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

            modelBuilder.Entity<GraphInstance>().HasOne<DataSetProject>().WithMany(p => p.GraphInstances)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Instance>().HasOne<SmellCandidateInstances>().WithMany(c => c.Instances)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Annotation>().HasOne<Instance>().WithMany(i => i.Annotations)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SmellHeuristic>().HasOne<Annotation>().WithMany(a => a.ApplicableHeuristics)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RelatedInstance>().HasOne<Instance>().WithMany(i => i.RelatedInstances)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Identifier>().HasOne<Instance>().WithMany(i => i.Identifiers)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GraphRelatedInstance>().HasOne<GraphInstance>().WithMany(i => i.RelatedInstances)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<RelatedInstance>()
                .Property(i => i.RelationType)
                .HasConversion(new EnumToStringConverter<RelationType>());

            modelBuilder
                .Entity<GraphRelatedInstance>()
                .Property(i => i.RelationType)
                .HasConversion(new EnumToStringConverter<RelationType>());

            modelBuilder.Entity<HeuristicDefinition>().HasOne<CodeSmellDefinition>().WithMany(d => d.Heuristics)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeverityDefinition>().HasOne<CodeSmellDefinition>().WithMany(d => d.Severities)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<CodeSmellDefinition>()
                .Property(c => c.SnippetType)
                .HasConversion<string>();

            modelBuilder
                .Entity<Identifier>()
                .Property(i => i.Type)
                .HasConversion<string>();

            modelBuilder
                .Entity<CodeSmell>()
                .Property(c => c.SnippetType)
                .HasConversion<string>();

            modelBuilder.Entity<CodeSmellDefinition>(codeSmell => {
                codeSmell.HasIndex(c => c.Name).IsUnique();
            });
            modelBuilder.Entity<Annotator>(annotator => {
                annotator.HasIndex(a => a.Email).IsUnique();
            });
        }
    }
}
