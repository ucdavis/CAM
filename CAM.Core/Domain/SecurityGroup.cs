using CAM.Core.BaseClasses;
using FluentNHibernate.Mapping;

namespace CAM.Core.Domain
{
    public class SecurityGroup : GroupBase
    {
    }

    public class SecurityGroupMap : ClassMap<SecurityGroup>
    {
        public SecurityGroupMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.IsActive);
            References(x => x.Site);
        }
    }
}
