using CAM.Core.Domain;
using UCDArch.Core.DomainModel;

namespace CAM.Core.BaseClasses
{
    public class GroupBase : ObjectBase
    {
        public GroupBase()
        {
            IsActive = true;
        }

        /// <summary>
        /// Display Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// AD Security Identifier
        /// </summary>
        public virtual string SID { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string SearchableName { get; set; }
    }
}
