using System;
using System.Collections.Generic;
using SmartTutor.ContentModel.Lectures;

namespace SmartTutor.InstructorModel.PrerequisiteSelectionStrategies
{
    public class RandomizedPrerequisiteSelectionStrategy : IPrerequisiteSelectionStrategy
    {
        private readonly Random _random = new();

        public List<KnowledgeNode> GetPrerequisites(KnowledgeNode knowledgeNode)
        {
            var result = new List<KnowledgeNode>();
            if (knowledgeNode.Prerequisites.Count <= 0) return result;
            var index = _random.Next(0, knowledgeNode.Prerequisites.Count);
            result.Add(knowledgeNode.Prerequisites[index]);
            return result;
        }
    }
}