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
        public DbSet<Annotator> Annotators { get; set; }
        public DbSet<CodeSmell> CodeSmells { get; set; }
        public DbSet<SmellHeuristic> SmellHeuristics { get; set; }
        public DbSet<DataSetAnnotation> DataSetAnnotations { get; set; }
        public DbSet<DataSetInstance> DataSetInstances { get; set; }
        public DbSet<DataSet> DataSets { get; set; }
        public DataSetExplorerContext(DbContextOptions<DataSetExplorerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSetInstance>().Ignore(i => i.MetricFeatures);
        }
    }
}
