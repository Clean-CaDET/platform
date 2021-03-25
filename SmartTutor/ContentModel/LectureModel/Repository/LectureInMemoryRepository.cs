using System.Collections.Generic;

namespace SmartTutor.ContentModel.LectureModel.Repository
{
    public class LectureInMemoryRepository : ILectureRepository
    {
        public List<Lecture> GetLectures()
        {
            return GetLectureList();
        }

        public List<KnowledgeNode> GetKnowledgeNodes(int id)
        {
            return GetFullLecture(id);
        }

        public KnowledgeNode GetKnowledgeNodeWithSummaries(int id)
        {
            return GetNodeSummaries(id);
        }

        private List<Lecture> GetLectureList()
        {
            return new List<Lecture>
            {
                new Lecture {Id = 1, Name = "Good Naming", Description = "Learn about the importance of assigning meaningful names in your code."},
                new Lecture {Id = 2, Name = "Clean Methods", Description = "Examine strategies for writing better methods that ultimately end up short."},
                new Lecture {Id = 3, Name = "Class - Cohesion", Description = "See what it means for a code module to be cohesive, and the benefits this brings."},
                new Lecture {Id = 4, Name = "Class - Coupling", Description = "Understand the dangers of tight coupling and the types of design that minimize it."},
                new Lecture {Id = 5, Name = "Single Responsibility Principle", Description = "Explore how the leading software principle helps us develop high quality modules."}
            };
        }

        private List<KnowledgeNode> GetFullLecture(int id)
        {
            return id switch
            {
                1 => new List<KnowledgeNode>
                    {
                        new KnowledgeNode
                        {
                            Id = 11,
                            LearningObjective = "List and describe advantages of good naming strategies.",
                            Type = KnowledgeNodeType.Factual
                        },
                        new KnowledgeNode
                        {
                            Id = 12,
                            LearningObjective = "Apply good naming practices through code refactoring.",
                            Type = KnowledgeNodeType.Procedural
                        }
                    },
                3 => new List<KnowledgeNode>
                    {
                        new KnowledgeNode
                        {
                            Id = 33,
                            LearningObjective = "Understand the basics of structural and semantic cohesion.",
                            Type = KnowledgeNodeType.Factual
                        }
                    },
                _ => null
            };
        }
        private KnowledgeNode GetNodeSummaries(int id)
        {
            return id switch
            {
                11 => new KnowledgeNode
                {
                    Id = 1,
                    LearningObjective = "List and describe advantages of good naming strategies.",
                    Type = KnowledgeNodeType.Factual,
                    LearningObjectSummaries = new List<LearningObjectSummary>
                    {
                        new LearningObjectSummary {Id = 111, Description = "Naming definition"},
                        new LearningObjectSummary {Id = 112, Description = "Naming example"}
                    }
                },
                12 => new KnowledgeNode
                {
                    Id = 2,
                    LearningObjective = "Apply good naming practices through code refactoring.",
                    Type = KnowledgeNodeType.Procedural,
                    LearningObjectSummaries = new List<LearningObjectSummary>
                    {
                        new LearningObjectSummary {Id = 121, Description = "Naming algorithm"}
                    }
                },
                33 => new KnowledgeNode
                {
                    Id = 33,
                    LearningObjective = "Understand the basics of structural and semantic cohesion.",
                    Type = KnowledgeNodeType.Factual,
                    LearningObjectSummaries = new List<LearningObjectSummary>
                    {
                        new LearningObjectSummary {Id = 331, Description = "Cohesion definition"},
                        new LearningObjectSummary {Id = 332, Description = "Cohesion example"},
                        new LearningObjectSummary {Id = 333, Description = "Structural cohesion definition"},
                        new LearningObjectSummary {Id = 334, Description = "Structural cohesion formula"},
                        new LearningObjectSummary {Id = 335, Description = "Semantic cohesion definition"},
                        new LearningObjectSummary {Id = 336, Description = "Structural cohesion example"}
                    }
                },
                _ => null
            };
        }
    }
}