using AutoMapper;
using CAM.Core.Domain;

namespace CAM
{
    public class AutomapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ViewModelProfile>());  
        }
    }

    public class ViewModelProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Request, Request>()
                .ForMember( x=> x.Id, x=>x.Ignore())
                .ForMember(x => x.Unit, x=> x.Ignore())
                .ForMember(x => x.Pending, x => x.Ignore())
                .ForMember(x => x.Approved, x => x.Ignore())
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.CreatedDate, x => x.Ignore())
                //.ForMember(x => x.Software, x=>x.Ignore())
                //.ForMember(x => x.NetworkShares, x => x.Ignore())
                //.ForMember(x => x.SecurityGroups, x => x.Ignore())
                ;

            CreateMap<RequestTemplate, RequestTemplate>()
                .ForMember(x => x.Id, x => x.Ignore())
                ;
            
            CreateMap<RequestTemplate, Request>()
                .ForMember(dest => dest.Site, opt => opt.MapFrom(src => src.Site))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit))
                .ForMember(dest => dest.NeedsEmail, opt => opt.MapFrom(src => src.NeedsEmail))
                .ForMember(dest => dest.AdditionalFolders, opt => opt.MapFrom(src => src.AdditionalFolders))
                .ForMember(dest => dest.PositionTitle, opt => opt.MapFrom(src => src.PositionTitle))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.DepartmentName))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.UnitName))
                .ForMember(dest => dest.HireType, opt => opt.MapFrom(src => src.HireType))
                .ForMember(dest => dest.HardwareType, opt => opt.MapFrom(src => src.HardwareType))
                .ForMember(dest => dest.Software, opt => opt.MapFrom(src => src.Software))
                .ForMember(dest => dest.SecurityGroups, opt => opt.MapFrom(src => src.SecurityGroups))
                ;
        }
    }
}