using CAM.Core.BaseClasses;
using FluentNHibernate.Mapping;

namespace CAM.Core.Domain
{
    public class OrganizationalUnit : ObjectBase
    {
        public virtual string Name { get; set; }
        public virtual string Path { get; set; }
    }

    public class OrganizationalUnitMap : ClassMap<OrganizationalUnit>
    {
        public OrganizationalUnitMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Path);
            References(x => x.Site);
        }
    }
}
