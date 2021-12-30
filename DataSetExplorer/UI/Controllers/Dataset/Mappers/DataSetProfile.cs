using AutoMapper;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;

namespace DataSetExplorer.UI.Controllers.Dataset.Mappers
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
