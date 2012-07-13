using System.DirectoryServices.AccountManagement;
using AutoMapper;
using CAM.Core.Domain;
using CAM.Services;

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
                .ForMember(dest => dest.HomeDrive, opt => opt.MapFrom(src => src.HomeDrive))
                .ForMember(dest => dest.HomeDirectory, opt => opt.MapFrom(src => src.HomeDirectory))
                ;

            CreateMap<UserPrincipal, AdUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SamAccountName))
                .ForMember(dest => dest.DistinguishedName, opt => opt.MapFrom(src => src.DistinguishedName))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.GivenName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))

                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))

                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.VoiceTelephoneNumber))
                
                .ForMember(dest => dest.HomeDrive, opt => opt.MapFrom(src => src.HomeDrive))
                .ForMember(dest => dest.HomeDirectory, opt => opt.MapFrom(src => src.HomeDirectory))

                .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.AccountExpirationDate))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
                ;

            CreateMap<AdUser, UserPrincipal>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => string.Format("{0}, {1}", src.LastName, src.FirstName)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.Format("{0}, {1}", src.LastName, src.FirstName)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => string.Format("{0} - {1}", src.Unit, src.PositionTitle)))
                .ForMember(dest => dest.HomeDrive, opt => opt.MapFrom(src => src.HomeDrive))
                .ForMember(dest => dest.HomeDirectory, opt => opt.MapFrom(src => src.HomeDirectory))
                .ForMember(dest => dest.AccountExpirationDate, opt => opt.MapFrom(src => src.Expiration))
                .ForMember(x => x.Enabled, x => x.Ignore())
                .ForMember(dest => dest.VoiceTelephoneNumber, opt => opt.MapFrom(src => src.Phone))
                ;

            CreateMap<Request, AdUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.HomeDirectory, opt => opt.MapFrom(src => src.HomeDirectory))
                .ForMember(dest => dest.HomeDrive, opt => opt.MapFrom(src => src.HomeDrive))
                .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.End))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.UnitName))
                .ForMember(dest => dest.PositionTitle, opt => opt.MapFrom(src => src.PositionTitle))
                .ForMember(dest => dest.NeedsEmail, opt => opt.MapFrom(src => src.NeedsEmail))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.OfficePhone))
                .ForMember(dest => dest.OfficeLocation, opt => opt.MapFrom(src => src.OfficeLocation))
                .ForMember(dest => dest.ManagerKerb, opt => opt.MapFrom(src => src.Manager))
                ;

        }
    }
}