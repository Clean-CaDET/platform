using System.Collections.Generic;
using AutoMapper;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.UI.Controllers.Annotations.DTOs;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.UI.Controllers.AnnotationSchema.DTOs;

namespace DataSetExplorer.UI.Controllers.Annotations.Mappers
{
    public class AnnotationProfile : Profile
    {
        public AnnotationProfile()
        {
            CreateMap<AnnotationDTO, Annotation>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(src => new Annotation(src.CodeSmell, src.Severity, new Annotator(src.AnnotatorId), CreateHeuristics(src.ApplicableHeuristics), src.Note));
            CreateMap<CodeSmellDefinitionDTO, CodeSmellDefinition>();
            CreateMap<HeuristicDefinitionDTO, HeuristicDefinition>();
            CreateMap<SeverityRangeDTO, SeverityRange>();
        }

        private List<SmellHeuristic> CreateHeuristics(List<SmellHeuristicDTO> heuristics)
        {
            List<SmellHeuristic> ret = new List<SmellHeuristic>();
            foreach (var heuristicDTO in heuristics) ret.Add(new SmellHeuristic(heuristicDTO.Description, heuristicDTO.IsApplicable, heuristicDTO.ReasonForApplicability));
            return ret;
        }
    }
}
