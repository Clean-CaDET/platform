using AutoMapper;
using DataSetExplorer.Controllers.Annotation.DTOs;
using DataSetExplorer.DataSetBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Controllers.Annotation.Mappers
{
    public class AnnotationProfile : Profile
    {
        public AnnotationProfile()
        {
            CreateMap<DataSetAnnotationDTO, DataSetAnnotation>()
                .ConstructUsing(src => new DataSetAnnotation(src.CodeSmell, src.Severity, new Annotator(src.AnnotatorId), CreateHeuristics(src.ApplicableHeuristics)));
        }

        private List<SmellHeuristic> CreateHeuristics(List<SmellHeuristicDTO> heuristics)
        {
            List<SmellHeuristic> ret = new List<SmellHeuristic>();
            foreach (var heuristicDTO in heuristics) ret.Add(new SmellHeuristic(heuristicDTO.Description, heuristicDTO.IsApplicable, heuristicDTO.ReasonForApplicability));
            return ret;
        }
    }
}
