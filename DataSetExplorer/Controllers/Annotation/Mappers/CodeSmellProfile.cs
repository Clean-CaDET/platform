using AutoMapper;
using DataSetExplorer.Controllers.Annotation.DTOs;
using DataSetExplorer.DataSetBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Controllers.Annotation.Mappers
{
    public class CodeSmellProfile : Profile
    {
        public CodeSmellProfile()
        {
            CreateMap<CodeSmell, CodeSmellDTO>()
                .ForMember(dest => dest.SnippetTypes, opt => opt.MapFrom(src => src.RelevantSnippetTypes()));
        }
    }
}
