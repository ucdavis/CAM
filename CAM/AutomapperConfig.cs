using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                .ForMember(x => x.Software, x=>x.Ignore())
                .ForMember(x => x.NetworkShares, x => x.Ignore())
                .ForMember(x => x.SecurityGroups, x => x.Ignore())
                ;

        }
    }
}