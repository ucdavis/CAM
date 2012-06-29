using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class RequestBase : DomainObject
    {
        public RequestBase()
        {
            DistributionLists = new List<string>();
            Software = new List<string>();
            NetworkShares = new List<string>();
        }

        public virtual PositionType Type { get; set; }
        public virtual HardwareType HardwareType { get; set; }

        [Display(Name = "Email Account")]
        public virtual bool NeedsEmail { get; set; }

        [Display(Name = "Default Save Location")]
        public virtual string DefaultSave { get; set; }

        public virtual IList<string> DistributionLists { get; set; }
        public virtual IList<string> Software { get; set; }
        public virtual IList<string> NetworkShares { get; set; }
    }

    public enum PositionType { NewPosition = 0, Rehire = 1 };
    public enum HardwareType { Desktop = 0, Laptop = 1 };
}
