using System;
using System.ComponentModel.DataAnnotations;
using CAM.Core.BaseClasses;
using FluentNHibernate.Mapping;

namespace CAM.Core.Domain
{
    public class Request : RequestBase
    {
        public Request()
        {
            SetDefaults();
        }

        public Request(RequestTemplate template)
        {
            SetDefaults();

            Site = template.Site;
            Unit = template.Unit;

            NeedsEmail = template.NeedsEmail;
            AdditionalFolders = template.AdditionalFolders;

            HireType = template.HireType;
            HardwareType = template.HardwareType;

            foreach(var sf in template.Software) { Software.Add(sf); }
            foreach(var ns in template.NetworkShares) { NetworkShares.Add(ns);}
        }

        private void SetDefaults()
        {
            Start = DateTime.Now.AddDays(14);
        }

        [Required]
        [StringLength(50)]
        public virtual string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string Email { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name="Position Title")]
        public virtual string PositionTitle { get; set; }
        
        [StringLength(100)]
        public virtual string DepartmentName { get; set; }
        public virtual string UnitName { get; set; }
        
        [Required]
        [StringLength(100)]
        [Display(Name="Office Location")]
        public virtual string OfficeLocation { get; set; }
        [Required]
        [StringLength(20)]
        public virtual string Room { get; set; }
        [StringLength(50)]
        [Phone]
        [Display(Name="Contact Phone")]
        public virtual string ContactPhone { get; set; }

        public virtual DateTime Start { get; set; }
        public virtual DateTime? End { get; set; }
    }

    public class RequestMap : ClassMap<Request>
    {
        public RequestMap()
        {
            Id(x => x.Id);

            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Email);
            Map(x => x.PositionTitle);
            Map(x => x.DepartmentName);
            Map(x => x.UnitName);
            Map(x => x.OfficeLocation);
            Map(x => x.Room);
            Map(x => x.ContactPhone);

            Map(x => x.Start);
            Map(x => x.End).Column("`End`");

            References(x => x.Unit);
            References(x => x.Site);

            Map(x => x.HireType).CustomType<NHibernate.Type.EnumStringType<HireType>>();
            Map(x => x.HardwareType).CustomType<NHibernate.Type.EnumStringType<HardwareType>>();
            Map(x => x.EmployeeType).CustomType<NHibernate.Type.EnumStringType<EmployeeType>>();
            Map(x => x.NeedsEmail);
            Map(x => x.AdditionalFolders);

            HasManyToMany(x => x.Software).Table("RequestsXSoftware").ParentKeyColumn("RequestId").ChildKeyColumn("SoftwareId").Cascade.SaveUpdate();
            HasManyToMany(x => x.SecurityGroups).Table("RequestsXSecurityGroups").ParentKeyColumn("RequestId").ChildKeyColumn("SecurityGroupId").Cascade.SaveUpdate();
        }
    }
}