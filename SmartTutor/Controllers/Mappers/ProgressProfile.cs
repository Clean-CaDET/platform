using AutoMapper;
using SmartTutor.ContentModel.ProgressModel;
using SmartTutor.Controllers.DTOs.Trainee;

namespace SmartTutor.Controllers.Mappers
{
    public class ProgressProfile : Profile
    {
        public ProgressProfile()
        {
            CreateMap<TraineeDTO, Trainee>();
            CreateMap<Trainee, TraineeDTO>();
        }
    }
}