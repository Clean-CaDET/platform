using AutoMapper;
using DataSetExplorer.Controllers.Dataset.DTOs;
using DataSetExplorer.DataSetBuilder.Model;

namespace DataSetExplorer.Controllers.Dataset.Mappers
{
    public class DataSetProfile : Profile
    {
        public DataSetProfile()
        {
            CreateMap<DatasetDTO, DataSet>();
            CreateMap<ProjectUpdateDTO, DataSetProject>();
            CreateMap<CodeSmellDTO, CodeSmell>();
            CreateMap<ProjectDTO, DataSetProject>();
            CreateMap<SmellFilterDTO, SmellFilter>();
            CreateMap<MetricThresholdsDTO, MetricThresholds>();
        }
    }
}
