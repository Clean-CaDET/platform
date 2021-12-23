using DataSetExplorer.Annotations.Model;
using DataSetExplorer.Annotations.Model.Repository;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.Annotations
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
        
        public Result<IEnumerable<Heuristic>> AddHeuristicsToCodeSmell(int id, IEnumerable<Heuristic> heuristics)
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
        
        public Result<IEnumerable<Heuristic>> GetHeuristicsForCodeSmell(int id)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");

            IEnumerable<Heuristic> heuristics = _annotationSchemaRepository.GetHeuristicsForCodeSmell(id);
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

        public Result<IEnumerable<Heuristic>> GetAllHeuristics()
        {
            return Result.Ok(_annotationSchemaRepository.GetAllHeuristics());
        }

        public Result<Heuristic> UpdateHeuristic(int id, Heuristic heuristic)
        {
            var existingHeuristic = _annotationSchemaRepository.GetHeuristic(id);
            if (existingHeuristic == default) return Result.Fail<Heuristic>($"Heuristic with id: {id} does not exist.");
            existingHeuristic.Update(heuristic);
            _annotationSchemaRepository.SaveHeuristic(existingHeuristic);
            return Result.Ok(heuristic);
        }

        public Result<Heuristic> DeleteHeuristic(int id)
        {
            var heuristic = _annotationSchemaRepository.DeleteHeuristic(id);
            return Result.Ok(heuristic);
        }

        public Result<Heuristic> CreateHeuristic(Heuristic heuristic)
        {
            _annotationSchemaRepository.SaveHeuristic(heuristic);
            return Result.Ok(heuristic);
        }
    }
}
