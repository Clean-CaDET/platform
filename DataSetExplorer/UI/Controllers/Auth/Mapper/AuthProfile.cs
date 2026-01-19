using AutoMapper;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.UI.Controllers.Auth.DTOs;

namespace DataSetExplorer.UI.Controllers.Auth.Mapper
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AnnotatorDTO, Annotator>();
        }
    }
}
