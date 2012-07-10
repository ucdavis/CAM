using CAM.Core.BaseClasses;
using FluentNHibernate.Mapping;

namespace CAM.Core.Domain
{
    public class Unit : ObjectBase
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsActive { get; set; }
    }

    public class UnitMap : ClassMap<Unit>
    {
        public UnitMap()
        {
            Id(x => x.Id);

            References(x => x.Site);
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.IsActive);
        }
    }
}
