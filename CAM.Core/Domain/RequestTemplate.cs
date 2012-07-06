using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;

namespace CAM.Core.Domain
{
    public class RequestTemplate : RequestBase
    {
        public RequestTemplate()
        {
            AvailableSoftware = new List<Software>();
        }

        [Display(Name="Template Name")]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<Software> AvailableSoftware { get; set; }
        public virtual IList<NetworkShare> AvailableNetworkShares { get; set; }
    }

    public class RequestTemplateMap : ClassMap<RequestTemplate>
    {
        public RequestTemplateMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Description);
            
            References(x => x.Site);
            References(x => x.Unit);
            
            Map(x => x.NeedsEmail);
            Map(x => x.AdditionalFolders);

            HasManyToMany(x => x.DistributionLists).Table("RequestTemplatesXDistributionLists").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("DistributionListId").Cascade.SaveUpdate();
            
            HasManyToMany(x => x.Software).Table("RequestTemplatesXSoftware").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("SoftwareId").Cascade.SaveUpdate();
            HasManyToMany(x => x.AvailableSoftware).Table("RequestTemplatesXAvailableSoftware").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("SoftwareId").Cascade.SaveUpdate();
            
            HasManyToMany(x => x.NetworkShares).Table("RequestTemplatesXNetworkShares").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("NetworkShareId").Cascade.SaveUpdate();
            HasManyToMany(x => x.AvailableNetworkShares).Table("RequestTemplatesXAvailableNetworkShares").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("NetworkShareId").Cascade.SaveUpdate();
            
            HasManyToMany(x => x.SecurityGroups).Table("RequestTemplatesXSecurityGroups").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("SecurityGroupId").Cascade.SaveUpdate();

            Map(x => x.HireType).CustomType<NHibernate.Type.EnumStringType<HireType>>();
            Map(x => x.HardwareType).CustomType<NHibernate.Type.EnumStringType<HardwareType>>();
            Map(x => x.EmployeeType).CustomType<NHibernate.Type.EnumStringType<EmployeeType>>();
        }
    }

}
