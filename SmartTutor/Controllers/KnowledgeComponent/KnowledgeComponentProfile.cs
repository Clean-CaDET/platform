using AutoMapper;
using SmartTutor.Controllers.KnowledgeComponent.DTOs;

namespace SmartTutor.Controllers.KnowledgeComponent
{
    public class KnowledgeComponentProfile : Profile
    {
        public KnowledgeComponentProfile()
        {
            CreateMap<KnowledgeComponentModel.KnowledgeComponents.KnowledgeComponent, KnowledgeComponentDTO>();
        }
    }
}