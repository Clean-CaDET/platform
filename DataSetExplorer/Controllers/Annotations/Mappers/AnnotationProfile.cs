using AutoMapper;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.Controllers.Annotations.DTOs;
using System.Collections.Generic;

namespace DataSetExplorer.Controllers.Annotations.Mappers
{
    public class AnnotationProfile : Profile
    {
        public AnnotationProfile()
        {
            CreateMap<AnnotationDTO, Annotation>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ConstructUsing(src => new Annotation(src.CodeSmell, src.Severity, new Annotator(src.AnnotatorId), CreateHeuristics(src.ApplicableHeuristics)));
        }

        private List<SmellHeuristic> CreateHeuristics(List<SmellHeuristicDTO> heuristics)
        {
            List<SmellHeuristic> ret = new List<SmellHeuristic>();
            foreach (var heuristicDTO in heuristics) ret.Add(new SmellHeuristic(heuristicDTO.Description, heuristicDTO.IsApplicable, heuristicDTO.ReasonForApplicability));
            return ret;
        }
    }
}
