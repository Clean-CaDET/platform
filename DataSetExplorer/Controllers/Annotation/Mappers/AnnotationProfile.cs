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
            CreateMap<SmellHeuristicDTO, SmellHeuristic>();
            CreateMap<DataSetAnnotationDTO, DataSetAnnotation>()
                .ForMember(dest => dest.InstanceSmell, opt => opt.MapFrom(src => new CodeSmell(src.CodeSmell)));
        }
    }
}
