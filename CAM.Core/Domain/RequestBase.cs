using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class RequestBase : DomainObject
    {
        public RequestBase()
        {
            DistributionLists = new List<DistributionList>();
            Software = new List<Software>();
            NetworkShares = new List<NetworkShare>();
        }

        public virtual Site Site { get; set; }
        public virtual Unit Unit { get; set; }

        public virtual HireType? HireType { get; set; }
        public virtual HardwareType? HardwareType { get; set; }
        public virtual EmployeeType? EmployeeType { get; set; }

        [Display(Name = "Email Account")]
        public virtual bool NeedsEmail { get; set; }

        [Display(Name = "Default Save Location")]
        public virtual string DefaultSave { get; set; }

        public virtual IList<DistributionList> DistributionLists { get; set; }
        public virtual IList<Software> Software { get; set; }
        public virtual IList<NetworkShare> NetworkShares { get; set; }
    }

    public enum HireType { NewPosition, Rehire };
    public enum HardwareType { Desktop, Laptop };
    public enum EmployeeType { Career, Temp, Volunteer, Student }
}
