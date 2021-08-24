using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.Database;

namespace SmartTutor.ContentModel.Lectures.Repository
{
    public class LectureDatabaseRepository : ILectureRepository
    {
        private readonly SmartTutorContext _dbContext;

        public LectureDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Course GetCourse(int id)
        {
            return _dbContext.Courses.FirstOrDefault(c => c.Id == id);
        }

        public int GetCourseIdByLOId(int learningObjectSummaryId)
        {
            var knowledgeNode = GetKnowledgeNodeBySummary(learningObjectSummaryId);

            var lecture = GetLecture(knowledgeNode.LectureId);

            return lecture.CourseId;
        }

        public Lecture GetLecture(int id)
        {
            return _dbContext.Lectures.FirstOrDefault(l => l.Id == id);
        }

        public List<Lecture> GetLectures()
        {
            return _dbContext.Lectures.Include(l => l.KnowledgeNodes).ToList();
        }

        public List<KnowledgeNode> GetKnowledgeNodes(int id)
        {
            var lecture = _dbContext.Lectures.Where(l => l.Id == id).Include(l => l.KnowledgeNodes).FirstOrDefault();
            return lecture?.KnowledgeNodes;
        }

        public KnowledgeNode GetKnowledgeNodeWithSummaries(int id)
        {
            return _dbContext.KnowledgeNodes.Where(n => n.Id == id).Include(n => n.LearningObjectSummaries)
                .FirstOrDefault();
        }

        public KnowledgeNode GetKnowledgeNodeBySummary(int summaryId)
        {
            var learningObjectSummary = _dbContext.LearningObjectSummaries.FirstOrDefault(los => los.Id == summaryId);
            return GetKnowledgeNode(learningObjectSummary.KnowledgeNodeId);
        }

        public List<KnowledgeNode> GetKnowledgeNodesByLecture(int lectureId)
        {
            return _dbContext.KnowledgeNodes.Where(n => n.LectureId == lectureId)
                .Include(node => node.LearningObjectSummaries).ToList();
        }

        public KnowledgeNode GetKnowledgeNode(int knowledgeNodeId)
        {
            return _dbContext.KnowledgeNodes.FirstOrDefault(node =>
                node.Id.Equals(knowledgeNodeId));
        }

        public Lecture SaveOrUpdateLecture(Lecture lecture)
        {
            var l = _dbContext.Lectures.Attach(lecture).Entity;
            _dbContext.SaveChanges();
            return l;
        }

        public KnowledgeNode SaveOrUpdateKnowledgeNode(KnowledgeNode node)
        {
            var savedKnowledgeNode = _dbContext.KnowledgeNodes.Attach(node).Entity;
            _dbContext.SaveChanges();
            return savedKnowledgeNode;
        }

        public LearningObjectSummary SaveOrUpdateLearningObjectSummary(LearningObjectSummary learningObjectSummary)
        {
            Console.WriteLine(learningObjectSummary.Description);
            Console.WriteLine(learningObjectSummary.KnowledgeNodeId);
            var los = _dbContext.LearningObjectSummaries.Attach(learningObjectSummary).Entity;
            _dbContext.SaveChanges();
            return los;
        }

        public LearningObjectSummary GetLearningObjectSummary(int learningObjectSummaryId)
        {
            return _dbContext.LearningObjectSummaries.FirstOrDefault(summary =>
                summary.Id.Equals(learningObjectSummaryId));
        }

        public List<LearningObject> GetLearningObjectsByLearningObjectSummary(int losId)
        {
            return _dbContext.LearningObjects.Where(lo => lo.LearningObjectSummaryId.Equals(losId)).ToList();
        }

        public List<LearningObjectSummary> GetLearningObjectSummariesByNode(int nodeId)
        {
            return _dbContext.LearningObjectSummaries.Where(summary => summary.KnowledgeNodeId.Equals(nodeId))
                .Include(summary => summary.LearningObjects).ToList();
        }

        public LearningObject SaveOrUpdateLearningObject(LearningObject learningObject)
        {
            if (learningObject.GetType() == typeof(Video))
            {
                learningObject = _dbContext.Videos.Attach((Video) learningObject).Entity;
            }
            else if (learningObject.GetType() == typeof(Image))
            {
                learningObject = _dbContext.Images.Attach((Image) learningObject).Entity;
            }
            else if (learningObject.GetType() == typeof(Text))
            {
                learningObject = _dbContext.Texts.Attach((Text) learningObject).Entity;
            }
            else if (learningObject.GetType() == typeof(Question))
            {
                learningObject = _dbContext.Questions.Attach((Question) learningObject).Entity;
            }
            else if (learningObject.GetType() == typeof(Challenge))
            {
                learningObject = _dbContext.Challenges.Attach((Challenge) learningObject).Entity;
            }

            _dbContext.SaveChanges();
            return learningObject;
        }

        public void AddKnowledgeNodeToLecture(KnowledgeNode node, Lecture lecture)
        {
            node.LectureId = lecture.Id;
            lecture.KnowledgeNodes.Add(node);
            SaveOrUpdateLecture(lecture);
            SaveOrUpdateKnowledgeNode(node);
        }

        public void AddLearningObjectSummaryToKnowledgeNode(LearningObjectSummary learningObjectSummary,
            KnowledgeNode knowledgeNode)
        {
            knowledgeNode.LearningObjectSummaries.Add(learningObjectSummary);
            learningObjectSummary.KnowledgeNodeId = knowledgeNode.Id;
            SaveOrUpdateKnowledgeNode(knowledgeNode);
            SaveOrUpdateLearningObjectSummary(learningObjectSummary);
        }
    }
}