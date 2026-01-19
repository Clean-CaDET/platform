using System.Collections.Generic;
using DataSetExplorer.Core.AnnotationSchema.Model;

namespace DataSetExplorer.Core.AnnotationSchema.Repository
{
    public interface IAnnotationSchemaRepository
    {
        CodeSmellDefinition GetCodeSmellDefinition(int id);
        CodeSmellDefinition GetCodeSmellDefinitionByName(string name);
        IEnumerable<CodeSmellDefinition> GetAllCodeSmellDefinitions();
        void SaveCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition);
        CodeSmellDefinition DeleteCodeSmellDefinition(int id);
    }
}
