using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class Software : DomainObject
    {
        public virtual string Name { get; set; }
        public virtual string IsActive { get; set; }
        public virtual Site Site { get; set; }
    }

    public class SoftwareMap : ClassMap<Software>
    {
        public SoftwareMap()
        {
            Table("Software");

            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.IsActive);
            References(x => x.Site);
        }
    }
}
