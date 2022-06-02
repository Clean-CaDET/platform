using System.Collections.Generic;
using System.Linq;
using DataSetExplorer.Core.Annotations;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Core.AnnotationSchema.Repository;
using DataSetExplorer.Core.DataSets;
using FluentResults;

namespace DataSetExplorer.Core.AnnotationSchema
{
    public class AnnotationSchemaService : IAnnotationSchemaService
    {
        private readonly IAnnotationSchemaRepository _annotationSchemaRepository;
        private readonly IAnnotationService _annotationService;
        private readonly IInstanceService _instanceService;

        public AnnotationSchemaService(IAnnotationSchemaRepository annotationSchemaRepository,
            IAnnotationService annotationService, IInstanceService instanceService)
        {
            _annotationSchemaRepository = annotationSchemaRepository;
            _annotationService = annotationService;
            _instanceService = instanceService;
        }

        public Result<CodeSmellDefinition> GetCodeSmellDefinition(int id)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");
            return Result.Ok(codeSmellDefinition);
        }

        public Result<CodeSmellDefinition> GetCodeSmellDefinitionByName(string name)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinitionByName(name);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with name: {name} does not exist.");
            return Result.Ok(codeSmellDefinition);
        }

        public Result<IEnumerable<CodeSmellDefinition>> GetAllCodeSmellDefinitions()
        {
            return Result.Ok(_annotationSchemaRepository.GetAllCodeSmellDefinitions());
        }

        public Result<CodeSmellDefinition> CreateCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition)
        {
            _annotationSchemaRepository.SaveCodeSmellDefinition(codeSmellDefinition);
            return Result.Ok(codeSmellDefinition);
        }

        public Result<CodeSmellDefinition> UpdateCodeSmellDefinition(int id, CodeSmellDefinition codeSmellDefinition)
        {
            var existingCodeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (existingCodeSmellDefinition == default) return Result.Fail<CodeSmellDefinition>($"CodeSmellDefinition with id: {id} does not exist.");

            var codeSmells = _annotationService.GetCodeSmellsByDefinition(existingCodeSmellDefinition).Value;
            foreach (var codeSmell in codeSmells)
            {
                if (codeSmell.Name.Equals(existingCodeSmellDefinition.Name))
                {
                    codeSmell.Name = codeSmellDefinition.Name;
                    _annotationService.UpdateCodeSmell(codeSmell);
                }
            }

            existingCodeSmellDefinition.Update(codeSmellDefinition);
            _annotationSchemaRepository.SaveCodeSmellDefinition(existingCodeSmellDefinition);

            return Result.Ok(codeSmellDefinition);
        }

        public Result<CodeSmellDefinition> DeleteCodeSmellDefinition(int id)
        {
            var codeSmellDefinition = _annotationSchemaRepository.DeleteCodeSmellDefinition(id);
            _instanceService.DeleteCandidateInstancesForSmell(codeSmellDefinition);
            return Result.Ok(codeSmellDefinition);
        }

        public Result<IEnumerable<HeuristicDefinition>> GetHeuristicsForCodeSmell(int id)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");
            return Result.Ok((IEnumerable<HeuristicDefinition>)codeSmellDefinition.Heuristics);
        }

        public Result<IDictionary<string, HeuristicDefinition[]>> GetHeuristicsForEachCodeSmell()
        {
            IDictionary<string, HeuristicDefinition[]> result = new Dictionary<string, HeuristicDefinition[]>();
            var codeSmellDefinitions = _annotationSchemaRepository.GetAllCodeSmellDefinitions();
            foreach (var codeSmellDefinition in codeSmellDefinitions)
            {
                var heuristics = GetHeuristicsForCodeSmell(codeSmellDefinition.Id);
                result[codeSmellDefinition.Name] = heuristics.Value.ToArray();
            }
            return Result.Ok(result);
        }

        public Result<HeuristicDefinition> AddHeuristicToCodeSmell(int id, HeuristicDefinition heuristic)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");

            codeSmellDefinition.Heuristics.Add(heuristic);
            _annotationSchemaRepository.SaveCodeSmellDefinition(codeSmellDefinition);
            return Result.Ok(heuristic);
        }

        public Result<HeuristicDefinition> UpdateHeuristicInCodeSmell(int id, HeuristicDefinition heuristic)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            var oldHeuristic = codeSmellDefinition.Heuristics.Find(h => h.Id == heuristic.Id);
            codeSmellDefinition.Heuristics.Remove(oldHeuristic);
            codeSmellDefinition.Heuristics.Add(heuristic);
            _annotationSchemaRepository.SaveCodeSmellDefinition(codeSmellDefinition);
            _annotationService.UpdateAnnotationsAfterHeuristicUpdate(codeSmellDefinition, heuristic, oldHeuristic);
            return Result.Ok(heuristic);
        }

        public Result<CodeSmellDefinition> DeleteHeuristicFromCodeSmell(int smellId, int heuristicId)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(smellId);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {smellId} does not exist.");

            var heuristic = codeSmellDefinition.Heuristics.Find(h => h.Id == heuristicId);
            codeSmellDefinition.Heuristics.RemoveAt(codeSmellDefinition.Heuristics.FindIndex(h => h.Id == heuristicId));
            _annotationSchemaRepository.SaveCodeSmellDefinition(codeSmellDefinition);

            _annotationService.UpdateAnnotationsAfterHeuristicDeletion(codeSmellDefinition, heuristic);
           
            return Result.Ok(codeSmellDefinition);
        }

        public Result<IEnumerable<SeverityDefinition>> GetSeveritiesForCodeSmell(int id)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");
            return Result.Ok((IEnumerable<SeverityDefinition>)codeSmellDefinition.Severities);
        }

        public Result<IDictionary<string, SeverityDefinition[]>> GetSeveritiesForEachCodeSmell()
        {
            IDictionary<string, SeverityDefinition[]> result = new Dictionary<string, SeverityDefinition[]>();
            var codeSmellDefinitions = _annotationSchemaRepository.GetAllCodeSmellDefinitions();
            foreach (var codeSmellDefinition in codeSmellDefinitions)
            {
                var severities = GetSeveritiesForCodeSmell(codeSmellDefinition.Id);
                result[codeSmellDefinition.Name] = severities.Value.ToArray();
            }
            return Result.Ok(result);
        }

        public Result<SeverityDefinition> AddSeverityToCodeSmell(int id, SeverityDefinition severity)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {id} does not exist.");

            codeSmellDefinition.Severities.Add(severity);
            _annotationSchemaRepository.SaveCodeSmellDefinition(codeSmellDefinition);
            return Result.Ok(severity);
        }

        public Result<SeverityDefinition> UpdateSeverityInCodeSmell(int id, SeverityDefinition severity)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(id);
            var oldSeverity = codeSmellDefinition.Severities.Find(s => s.Id == severity.Id);
            codeSmellDefinition.Severities.RemoveAt(codeSmellDefinition.Severities.FindIndex(s => s.Id == severity.Id));
            codeSmellDefinition.Severities.Add(severity);
            _annotationSchemaRepository.SaveCodeSmellDefinition(codeSmellDefinition);

            _annotationService.UpdateAnnotationsAfterSeverityUpdate(codeSmellDefinition, severity, oldSeverity);
            return Result.Ok(severity);
        }

        public Result<CodeSmellDefinition> DeleteSeverityFromCodeSmell(int smellId, int severityId)
        {
            var codeSmellDefinition = _annotationSchemaRepository.GetCodeSmellDefinition(smellId);
            if (codeSmellDefinition == default) return Result.Fail($"Code smell definition with id: {smellId} does not exist.");

            var severity = codeSmellDefinition.Severities.Find(s => s.Id == severityId);
            codeSmellDefinition.Severities.RemoveAt(codeSmellDefinition.Severities.FindIndex(s => s.Id == severityId));
            _annotationSchemaRepository.SaveCodeSmellDefinition(codeSmellDefinition);

            _annotationService.UpdateAnnotationsAfterSeverityDeletion(codeSmellDefinition, severity);

            return Result.Ok(codeSmellDefinition);
        }
    }
}
