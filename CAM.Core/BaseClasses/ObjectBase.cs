using CAM.Core.Domain;
using UCDArch.Core.DomainModel;

namespace CAM.Core.BaseClasses
{
    public class ObjectBase : DomainObject
    {
        public virtual Site Site { get; set; }
    }

    public class ObjectBaseStringId : DomainObjectWithTypedId<string>
    {
        public virtual Site Site { get; set; }
    }
}
