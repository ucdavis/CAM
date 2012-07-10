using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CAM.Core.BaseClasses;
using FluentNHibernate.Mapping;

namespace CAM.Core.Domain
{
    public class RequestTemplate : RequestBase
    {
        public RequestTemplate()
        {
            AvailableSoftware = new List<Software>();
            AvailableSecurityGroups = new List<SecurityGroup>();
        }

        [Display(Name="Template Name")]
        [StringLength(50)]
        public virtual string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }

        public virtual IList<Software> AvailableSoftware { get; set; }
        public virtual IList<NetworkShare> AvailableNetworkShares { get; set; }
        public virtual IList<SecurityGroup> AvailableSecurityGroups { get; set; }
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

            HasManyToMany(x => x.Software).Table("RequestTemplatesXSoftware").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("SoftwareId").Cascade.SaveUpdate();
            HasManyToMany(x => x.AvailableSoftware).Table("RequestTemplatesXAvailableSoftware").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("SoftwareId").Cascade.SaveUpdate();
            
            HasManyToMany(x => x.NetworkShares).Table("RequestTemplatesXNetworkShares").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("NetworkShareId").Cascade.SaveUpdate();
            HasManyToMany(x => x.AvailableNetworkShares).Table("RequestTemplatesXAvailableNetworkShares").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("NetworkShareId").Cascade.SaveUpdate();
            
            HasManyToMany(x => x.SecurityGroups).Table("RequestTemplatesXSecurityGroups").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("SecurityGroupId").Cascade.SaveUpdate();
            HasManyToMany(x => x.AvailableSecurityGroups).Table("RequestTemplatesXAvailableSecurityGroups").ParentKeyColumn("RequestTemplateId").ChildKeyColumn("SecurityGroupId").Cascade.SaveUpdate();

            Map(x => x.HireType).CustomType<NHibernate.Type.EnumStringType<HireType>>();
            Map(x => x.HardwareType).CustomType<NHibernate.Type.EnumStringType<HardwareType>>();
            Map(x => x.EmployeeType).CustomType<NHibernate.Type.EnumStringType<EmployeeType>>();
        }
    }

}
