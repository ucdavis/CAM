using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class DnsName : DomainObject
    {
        [StringLength(100)]
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual IpAddress IpAddress { get; set; }
    }

    public class DnsNameMap : ClassMap<DnsName>
    {
        public DnsNameMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            References(x => x.IpAddress);
        }
    }
}
