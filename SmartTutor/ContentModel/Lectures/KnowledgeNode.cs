﻿using System.Collections.Generic;

namespace SmartTutor.ContentModel.Lectures
{
    public class KnowledgeNode
    {
        public int Id { get; private set; }
        public string LearningObjective { get; private set; }
        public KnowledgeNodeType Type { get; private set; }
        public int LectureId { get; private set; }
        public List<LearningObjectSummary> LearningObjectSummaries { get; private set; }
        public List<KnowledgeNode> Prerequisites;
        //TODO: Remove if EF6 supports unidirectional many-to-many
        public List<KnowledgeNode> PrerequisiteOf { get; set; }

        private KnowledgeNode() {}
        public KnowledgeNode(int id, List<LearningObjectSummary> summaries, int lectureId) : this()
        {
            Id = id;
            LearningObjectSummaries = summaries;
            LectureId = lectureId;
        }
    }

    public enum KnowledgeNodeType
    {
        Factual,
        Procedural,
        Conceptual
    }
}