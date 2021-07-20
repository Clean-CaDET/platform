using DataSetExplorer.DataSetBuilder.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Database
{
    public class DataSetExplorerContext : DbContext
    {
        public DataSetExplorerContext(DbContextOptions<DataSetExplorerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Annotator>().ToTable("Annotators");
            modelBuilder.Entity<CodeSmell>().ToTable("CodeSmells");
            modelBuilder.Entity<SmellHeuristic>().ToTable("SmellHeuristics");
            
            ConfigureDataSetAnnotations(modelBuilder);
            ConfigureDataSet(modelBuilder);
        }

        private static void ConfigureDataSetAnnotations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSetAnnotation>().ToTable("DataSetAnnotations");
            
            modelBuilder.Entity<DataSetAnnotation>()
                .HasOne(a => a.Annotator);
            modelBuilder.Entity<DataSetAnnotation>()
                .HasOne(a => a.InstanceSmell);
            modelBuilder.Entity<DataSetAnnotation>()
                .HasMany(a => a.ApplicableHeuristics);
        }

        private static void ConfigureDataSet(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSetInstance>().ToTable("DataSetInstances");

            modelBuilder.Entity<DataSetInstance>()
                .HasMany(i => i.Annotations);

            modelBuilder.Entity<DataSet>().ToTable("DataSets");

            modelBuilder.Entity<DataSet>()
                .HasMany(s => s._instances);
        }
    }
}
