using CAM.Core.BaseClasses;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class NetworkShare : ObjectBase
    {
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string GroupId { get; set; }
        public virtual bool ForceSelect { get; set; }
    }

    public class NetworkShareMap : ClassMap<NetworkShare>
    {
        public NetworkShareMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.IsActive);
            Map(x => x.GroupId);
            Map(x => x.ForceSelect);

            References(x => x.Site);
        }
    }
}
