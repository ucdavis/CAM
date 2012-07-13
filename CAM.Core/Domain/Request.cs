using System;
using System.Collections.Generic;
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

        private void SetDefaults()
        {
            Start = DateTime.Now.AddDays(14);

            Pending = true;

            Software = new List<Software>();
            SecurityGroups = new List<SecurityGroup>();
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
        [Display(Name="Office Location")]
        public virtual string OfficeLocation { get; set; }
        [Required]
        [StringLength(20)]
        public virtual string Room { get; set; }
        [StringLength(50)]
        [Phone]
        [Display(Name="Contact Phone")]
        public virtual string ContactPhone { get; set; }
        [StringLength(50)]
        [Phone]
        [Display(Name="Office Phone")]
        public virtual string OfficePhone { get; set; }

        public virtual DateTime Start { get; set; }
        public virtual DateTime? End { get; set; }

        public virtual bool Pending { get; set; }
        public virtual bool? Approved { get; set; }
        /// <summary>
        /// kerb id of the user who is requesting it
        /// </summary>
        public virtual string CreatedBy { get; set; }
        /// <summary>
        /// Kerb id of the supervisor
        /// </summary>
        public virtual string Manager { get; set; }
        public virtual string CreatedDate { get; set; }

        public virtual string GetHomeDirectory()
        {
            return string.Format(HomeDirectory, LastName);
        }
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
            Map(x => x.OfficePhone);

            Map(x => x.Start);
            Map(x => x.End).Column("`End`");

            Map(x => x.Pending);
            Map(x => x.Approved);
            Map(x => x.CreatedBy);
            Map(x => x.CreatedDate);
            Map(x => x.Manager);

            References(x => x.Unit);
            References(x => x.Site);

            References(x => x.OrganizationalUnit);
            Map(x => x.HomeDirectory);
            Map(x => x.HomeDrive);

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