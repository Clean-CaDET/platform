using Microsoft.EntityFrameworkCore;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.ProgressModel;

namespace SmartTutor.Database
{
    public class SmartTutorContext : DbContext
    {
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<KnowledgeNode> KnowledgeNodes { get; set; }
        public DbSet<LearningObjectSummary> LearningObjectSummaries { get; set; }

        #region Learning Objects
        public DbSet<LearningObject> LearningObjects { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<ChallengeFulfillmentStrategy> ChallengeFulfillmentStrategies { get; set; }
        public DbSet<BasicMetricsChecker> MetricsCheckers { get; set; }
        public DbSet<MetricRangeRule> MetricRangeRules { get; set; }
        #endregion

        public DbSet<NodeProgress> NodeProgresses { get; set; }
        public DbSet<Trainee> Trainees { get; set; }

        public SmartTutorContext(DbContextOptions<SmartTutorContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBasicMetricChecker(modelBuilder);
        }

        private static void ConfigureBasicMetricChecker(ModelBuilder modelBuilder)
        {
            // Add the shadow property to the model
            modelBuilder.Entity<MetricRangeRule>()
                .Property<int>("ClassMetricCheckerForeignKey");
            modelBuilder.Entity<MetricRangeRule>()
                .Property<int>("MethodMetricCheckerForeignKey");

            // Use the shadow property as a foreign key
            modelBuilder.Entity<BasicMetricsChecker>()
                .HasMany(b => b.ClassMetricRules)
                .WithOne()
                .HasForeignKey("ClassMetricCheckerForeignKey");
            modelBuilder.Entity<BasicMetricsChecker>()
                .HasMany(b => b.MethodMetricRules)
                .WithOne()
                .HasForeignKey("MethodMetricCheckerForeignKey");
        }
    }
}