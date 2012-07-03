using System.Collections.Generic;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class IpAddress : DomainObjectWithTypedId<string>
    {
        public IpAddress()
        {
            DnsNames = new List<DnsName>();
        }

        public virtual string Host { get; set; }
        public virtual IList<DnsName> DnsNames { get; set; }
    }

    public class IpAddressMap : ClassMap<IpAddress>
    {
        public IpAddressMap()
        {
            Table("IpAddresses");
            Id(x => x.Id);

            Map(x => x.Host);
            HasMany(x => x.DnsNames).Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
