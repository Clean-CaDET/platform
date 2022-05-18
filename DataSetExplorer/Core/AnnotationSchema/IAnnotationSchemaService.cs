﻿using System.Collections.Generic;
using DataSetExplorer.Core.AnnotationSchema.Model;
using FluentResults;

namespace DataSetExplorer.Core.AnnotationSchema
{
    public interface IAnnotationSchemaService
    {
        Result<CodeSmellDefinition> GetCodeSmellDefinition(int id);
        Result<IEnumerable<CodeSmellDefinition>> GetAllCodeSmellDefinitions();
        Result<CodeSmellDefinition> CreateCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition);
        Result<CodeSmellDefinition> UpdateCodeSmellDefinition(int id, CodeSmellDefinition codeSmellDefinition);
        Result<CodeSmellDefinition> DeleteCodeSmellDefinition(int id);
        Result<IEnumerable<HeuristicDefinition>> GetHeuristicsForCodeSmell(int id);
        Result<IDictionary<string, HeuristicDefinition[]>> GetHeuristicsForEachCodeSmell();
        Result<HeuristicDefinition> AddHeuristicToCodeSmell(int id, HeuristicDefinition heuristic);
        Result<HeuristicDefinition> UpdateHeuristicInCodeSmell(int id, HeuristicDefinition heuristic);
        Result<CodeSmellDefinition> DeleteHeuristicFromCodeSmell(int smellId, int heuristicId);
    }
}
