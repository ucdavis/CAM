using CAM.Core.BaseClasses;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class DistributionList : GroupBase
    {

    }

    public class DistributionListMap : ClassMap<DistributionList>
    {
        public DistributionListMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.IsActive);
            References(x => x.Site);
            Map(x => x.SID);
            Map(x => x.NameLower);
        }
    }
}