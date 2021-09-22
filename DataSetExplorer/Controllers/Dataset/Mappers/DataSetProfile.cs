using AutoMapper;
using DataSetExplorer.Controllers.Dataset.DTOs;
using DataSetExplorer.DataSetBuilder.Model;

namespace DataSetExplorer.Controllers.Dataset.Mappers
{
    public class DataSetProfile : Profile
    {
        public DataSetProfile()
        {
            CreateMap<ProjectDTO, DataSetProject>();
            CreateMap<CodeSmellDTO, CodeSmell>();
        }
    }
}
