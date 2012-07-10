using System.ComponentModel.DataAnnotations;
using CAM.Core.Domain;
using UCDArch.Core.DomainModel;

namespace CAM.Core.BaseClasses
{
    public class ObjectBase : DomainObject
    {
        [Required]
        public virtual Site Site { get; set; }
    }

    public class ObjectBaseStringId : DomainObjectWithTypedId<string>
    {
        [Required]
        public virtual Site Site { get; set; }
    }
}
