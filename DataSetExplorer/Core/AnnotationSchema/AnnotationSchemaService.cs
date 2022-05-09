using System.Collections.Generic;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Core.AnnotationSchema.Repository;
using FluentResults;

namespace DataSetExplorer.Core.AnnotationSchema
{
    public class AnnotationSchemaService : IAnnotationSchemaService
    {
        private readonly IAnnotationSchemaRepository _annotationSchemaRepository;

        public AnnotationSchemaService(IAnnotationSchemaRepository annotationSchemaRepository)
        {
            _annotationSchemaRepository = annotationSchemaRepository;
        }

        public Result<CodeSmellDefinition> GetCodeSmellDefinition(int id)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");
            return Result.Ok(codeSmellDefinition);
        }

        public Result<CodeSmellDefinition> CreateCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition)
        {
            _annotationSchemaRepository.SaveCodeSmellDefinition(codeSmellDefinition);
            return Result.Ok(codeSmellDefinition);
        }
        
        public Result<HeuristicDefinition> AddHeuristicToCodeSmell(int id, HeuristicDefinition heuristic)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");

            codeSmellDefinition.Heuristics.Add(heuristic);
            _annotationSchemaRepository.SaveCodeSmellDefinition(codeSmellDefinition);
            return Result.Ok(heuristic);
        }
        
        public Result<IEnumerable<HeuristicDefinition>> GetHeuristicsForCodeSmell(int id)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");
            return Result.Ok((IEnumerable<HeuristicDefinition>)codeSmellDefinition.Heuristics);
        }

        public Result<CodeSmellDefinition> DeleteHeuristicFromCodeSmell(int smellId, int heuristicId)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(smellId);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {smellId} does not exist.");

            codeSmellDefinition.Heuristics.RemoveAt(codeSmellDefinition.Heuristics.FindIndex(h => h.Id == heuristicId));
            _annotationSchemaRepository.SaveCodeSmellDefinition(codeSmellDefinition);
            return Result.Ok(codeSmellDefinition);
        }

        public Result<IEnumerable<CodeSmellDefinition>> GetAllCodeSmellDefinitions()
        {
            return Result.Ok(_annotationSchemaRepository.GetAllCodeSmellDefinitions());
        }

        public Result<CodeSmellDefinition> UpdateCodeSmellDefinition(int id, CodeSmellDefinition codeSmellDefinition)
        {
            var existingCodeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (existingCodeSmellDefinition == default) return Result.Fail<CodeSmellDefinition>($"CodeSmellDefinition with id: {id} does not exist.");
            existingCodeSmellDefinition.Update(codeSmellDefinition);
            _annotationSchemaRepository.SaveCodeSmellDefinition(existingCodeSmellDefinition);
            return Result.Ok(codeSmellDefinition);
        }

        public Result<CodeSmellDefinition> DeleteCodeSmellDefinition(int id)
        {
            var codeSmellDefinition = _annotationSchemaRepository.DeleteCodeSmellDefinition(id);
            return Result.Ok(codeSmellDefinition);
        }

        public Result<HeuristicDefinition> UpdateHeuristicInCodeSmell(int id, HeuristicDefinition heuristic)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            codeSmellDefinition.Heuristics.RemoveAt(codeSmellDefinition.Heuristics.FindIndex(h => h.Id == heuristic.Id));
            return AddHeuristicToCodeSmell(id, heuristic);
        }
    }
}
