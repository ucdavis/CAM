using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class DistributionList : DomainObject
    {
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Site Site { get; set; }
    }

    public class DistributionListMap : ClassMap<DistributionList>
    {
        public DistributionListMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.IsActive);
            References(x => x.Site);
        }
    }
}