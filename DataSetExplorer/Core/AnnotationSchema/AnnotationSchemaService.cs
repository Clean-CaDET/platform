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
        
        public Result<IEnumerable<HeuristicDefinition>> AddHeuristicsToCodeSmell(int id, IEnumerable<HeuristicDefinition> heuristics)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");
            foreach (var h in heuristics)
            {
                var heuristic = _annotationSchemaRepository.GetHeuristic(h.Id);
                if (heuristic == default) return Result.Fail($"Heuristic with id: {id} does not exist.");
            }
            
            foreach (var h in heuristics)
            {
                var heuristic = _annotationSchemaRepository.GetHeuristic(h.Id);
                var codeSmellHeuristic = new CodeSmellHeuristic(codeSmellDefinition, heuristic);
                _annotationSchemaRepository.SaveCodeSmellHeuristic(codeSmellHeuristic);
            }

            return Result.Ok(heuristics);
        }
        
        public Result<IEnumerable<HeuristicDefinition>> GetHeuristicsForCodeSmell(int id)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");

            IEnumerable<HeuristicDefinition> heuristics = _annotationSchemaRepository.GetHeuristicsForCodeSmell(id);
            return Result.Ok(heuristics);
        }

        public Result<CodeSmellHeuristic> DeleteHeuristicFromCodeSmell(int smellId, int heuristicId)
        {
            var codeSmellHeuristic = _annotationSchemaRepository.DeleteHeuristicFromCodeSmell(smellId, heuristicId);
            return Result.Ok(codeSmellHeuristic);
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

        public Result<IEnumerable<HeuristicDefinition>> GetAllHeuristics()
        {
            return Result.Ok(_annotationSchemaRepository.GetAllHeuristics());
        }

        public Result<HeuristicDefinition> UpdateHeuristic(int id, HeuristicDefinition heuristic)
        {
            var existingHeuristic = _annotationSchemaRepository.GetHeuristic(id);
            if (existingHeuristic == default) return Result.Fail<HeuristicDefinition>($"Heuristic with id: {id} does not exist.");
            existingHeuristic.Update(heuristic);
            _annotationSchemaRepository.SaveHeuristic(existingHeuristic);
            return Result.Ok(heuristic);
        }

        public Result<HeuristicDefinition> DeleteHeuristic(int id)
        {
            var heuristic = _annotationSchemaRepository.DeleteHeuristic(id);
            return Result.Ok(heuristic);
        }

        public Result<HeuristicDefinition> CreateHeuristic(HeuristicDefinition heuristic)
        {
            _annotationSchemaRepository.SaveHeuristic(heuristic);
            return Result.Ok(heuristic);
        }
    }
}
