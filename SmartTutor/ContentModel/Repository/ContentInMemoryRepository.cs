using SmartTutor.ContentModel.LectureModel;
using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel.Repository
{
    public class ContentInMemoryRepository : IContentRepository
    {
        public List<Lecture> GetLectures()
        {
            return GetLectureList();
        }

        public Lecture GetLecture(int id)
        {
            return GetFullLecture(id);
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

        private Lecture GetFullLecture(int id)
        {
            var lecture = new Lecture { Id = 3, Name = "Class - Cohesion" };
            if (id != lecture.Id) return null;

            var learningObjects = new List<LearningObject>();
            learningObjects.Add(new Text { Id = 1, Content = "Cohesion determines the degree to which a part of a codebase forms a meaningful atomic module. The elements of a highly cohesive module work together towards a common, well-defined goal and have a clear (single) responsibility. This responsibility is defined by the module’s name and described by its interface that sets its inputs and outputs." });
            learningObjects.Add(new Image { Id = 2, Url = "https://miro.medium.com/max/2400/1*3jfye6OQFu_dROKb14BhaQ.png", Caption = "The left class is playing with a few responsibilities, more than its name suggests anyway…" });
            learningObjects.Add(new Text { Id = 3, Content = "Structural cohesion is a metric that is calculated based on the number of connections between a module’s elements." });
            learningObjects.Add(new Image { Id = 4, Url = "https://miro.medium.com/max/700/1*OF4xmCDnuV_VRDcqiLp46Q.png", Caption = "How does this formula hold for data transfer object classes? What about classes without fields?" });
            learningObjects.Add(new Text { Id = 5, Content = "Semantic cohesion determines the degree to which the elements of a module are semantically related." });
            learningObjects.Add(new Video { Id = 6, Url = "https://www.youtube.com/watch?v=qE-Gmu_YuQE" });

            var knowledgeNode = new List<KnowledgeNode>
            {
                new KnowledgeNode { LearningObjective = "Understand the basics of structural and semantic cohesion.", Id = 1, LearningObjects = learningObjects }
            };

            lecture.KnowledgeNodes = knowledgeNode;

            return lecture;
        }
    }
}