using AutoMapper;
using SmartTutor.Controllers.KnowledgeComponent.DTOs;

namespace SmartTutor.Controllers.KnowledgeComponent
{
    using KnowledgeComponentModel.KnowledgeComponents;

    public class KnowledgeComponentProfile : Profile
    {
        public KnowledgeComponentProfile()
        {
            CreateMap<KnowledgeComponent, KnowledgeComponentDTO>();
        }
    }
}