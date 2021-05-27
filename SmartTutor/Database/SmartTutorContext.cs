﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.MetricChecker;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.NameChecker;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.ProjectChecker;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.ProgressModel.Feedback;
using SmartTutor.ProgressModel.Progress;
using SmartTutor.ProgressModel.Submissions;
using System;
using SmartTutor.QualityAnalysis;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SmartTutor.Database
{
    public class SmartTutorContext : DbContext
    {
        #region Courses
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<KnowledgeNode> KnowledgeNodes { get; set; }
        public DbSet<LearningObjectSummary> LearningObjectSummaries { get; set; }
        public DbSet<Course> Courses { get; set; }
        #endregion

        #region Learning Objects
        public DbSet<LearningObject> LearningObjects { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<ArrangeTask> ArrangeTasks { get; set; }
        public DbSet<ArrangeTaskContainer> ArrangeTaskContainers { get; set; }
        public DbSet<ArrangeTaskElement> ArrangeTaskElements { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<ChallengeFulfillmentStrategy> ChallengeFulfillmentStrategies { get; set; }
        public DbSet<ProjectChecker> ProjectCheckers { get; set; }
        public DbSet<BasicMetricChecker> BasicMetricCheckers { get; set; }
        public DbSet<BasicNameChecker> BasicNameCheckers { get; set; }
        public DbSet<MetricRangeRule> MetricRangeRules { get; set; }
        public DbSet<ChallengeHint> ChallengeHints { get; set; }
        #endregion

        #region Progress Model
        public DbSet<NodeProgress> NodeProgresses { get; set; }
        public DbSet<ArrangeTaskSubmission> ArrangeTaskSubmissions { get; set; }
        public DbSet<ArrangeTaskContainerSubmission> ArrangeTaskContainerSubmissions { get; set; }
        public DbSet<ChallengeSubmission> ChallengeSubmissions { get; set; }
        public DbSet<QuestionSubmission> QuestionSubmissions { get; set; }
        public DbSet<LearningObjectFeedback> LearningObjectFeedback { get; set; }
        #endregion
        public DbSet<Learner> Learners { get; set; }
        public DbSet<IssueAdvice> Advice { get; set; }

        public SmartTutorContext(DbContextOptions<SmartTutorContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBasicMetricChecker(modelBuilder);
            ConfigureProjectChecker(modelBuilder);

            modelBuilder.Entity<Text>().ToTable("Texts");
            modelBuilder.Entity<Image>().ToTable("Images");
            modelBuilder.Entity<Video>().ToTable("Videos");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<ArrangeTask>().ToTable("ArrangeTasks");
            
            ConfigureChallenge(modelBuilder);

            modelBuilder.Entity<Learner>()
                .OwnsOne(l => l.Workspace)
                .Property(w => w.Path).HasColumnName("WorkspacePath");

            modelBuilder.Entity<IssueAdvice>()
                .HasMany(a => a.Summaries)
                .WithMany("Advice");
        }

        private static void ConfigureChallenge(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Challenge>().ToTable("Challenges");

            modelBuilder.Entity<BasicNameChecker>().ToTable("BasicNameCheckers");
            modelBuilder.Entity<BasicMetricChecker>().ToTable("BasicMetricCheckers");
            modelBuilder.Entity<ProjectChecker>().ToTable("ProjectCheckers");

            modelBuilder.Entity<Challenge>()
                .Property<int>("SolutionIdForeignKey");
            modelBuilder.Entity<Challenge>()
                .HasOne(b => b.Solution)
                .WithMany()
                .HasForeignKey("SolutionIdForeignKey");

            modelBuilder.Entity<Learner>()
                .OwnsOne(l => l.Workspace)
                .Property(w => w.Path).HasColumnName("WorkspacePath");
        }

        private static void ConfigureBasicMetricChecker(ModelBuilder modelBuilder)
        {
            //TODO: Look for patterns for better DBContext code organization when using Fluent API extensively.

            // Add the shadow property to the model
            modelBuilder.Entity<MetricRangeRule>()
                .Property<int?>("ClassMetricCheckerForeignKey").IsRequired(false);
            modelBuilder.Entity<MetricRangeRule>()
                .Property<int?>("MethodMetricCheckerForeignKey").IsRequired(false);

            // Use the shadow property as a foreign key
            modelBuilder.Entity<BasicMetricChecker>()
                .HasMany(b => b.ClassMetricRules)
                .WithOne()
                .HasForeignKey("ClassMetricCheckerForeignKey");
            modelBuilder.Entity<BasicMetricChecker>()
                .HasMany(b => b.MethodMetricRules)
                .WithOne()
                .HasForeignKey("MethodMetricCheckerForeignKey");
        }

        private static void ConfigureProjectChecker(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChallengeFulfillmentStrategy>()
               .Property<string>("StrategiesApplicableToSnippetCheckerForeignKey").IsRequired(false);

            modelBuilder.Entity<ProjectChecker>()
                .Property(pc => pc.StrategiesApplicableToSnippet)
                .IsRequired()
                .HasConversion(
                    sats => ((IEnumerable)sats).Cast<object>().Select(x => x.ToString()).ToArray(),
                    sats => sats == null
                        ? new Dictionary<string, List<ChallengeFulfillmentStrategy>>()
                        : JsonSerializer.Deserialize<Dictionary<string, List<ChallengeFulfillmentStrategy>>>(sats.ToString(), null),
                        new ValueComparer<Dictionary<string, List<ChallengeFulfillmentStrategy>>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, sats) => HashCode.Combine(a, sats.GetHashCode())))
                );
        }
    }
}