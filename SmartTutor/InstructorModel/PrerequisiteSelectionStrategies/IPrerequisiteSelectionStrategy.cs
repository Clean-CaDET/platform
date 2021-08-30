using System.Collections.Generic;
using SmartTutor.ContentModel.Lectures;

namespace SmartTutor.InstructorModel.PrerequisiteSelectionStrategies
{
    public interface IPrerequisiteSelectionStrategy
    {
        List<KnowledgeNode> GetPrerequisites(KnowledgeNode knowledgeNode);
    }
}