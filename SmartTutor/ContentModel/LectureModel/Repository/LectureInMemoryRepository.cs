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
            throw new System.NotImplementedException();
        }

        private List<Lecture> GetLectureList()
        {
            return new List<Lecture>
            {
                new Lecture {Id = 1, Name = "Good Naming"},
                new Lecture {Id = 2, Name = "Clean Methods"},
                new Lecture {Id = 3, Name = "Class - Cohesion"},
                new Lecture {Id = 4, Name = "Class - Coupling"},
                new Lecture {Id = 5, Name = "Single Responsibility Principle"}
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
                            Id = 1,
                            LearningObjective = "List and describe advantages of good naming strategies.",
                            Type = KnowledgeNodeType.Factual
                        },
                        new KnowledgeNode
                        {
                            Id = 2,
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
    }
}