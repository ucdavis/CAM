using System;
using System.ComponentModel.DataAnnotations;

namespace CAM.Core.Domain
{
    public class Request : RequestBase
    {
        public Request()
        {
            SetDefaults();
        }

        public Request(RequestTemplate template)
        {
            SetDefaults();

            NeedsEmail = template.NeedsEmail;
            AdditionalFolders = template.AdditionalFolders;

            HireType = template.HireType;
            HardwareType = template.HardwareType;

            foreach(var dl in template.DistributionLists) { DistributionLists.Add(dl); }
            foreach(var sf in template.Software) { Software.Add(sf); }
            foreach(var ns in template.NetworkShares) { NetworkShares.Add(ns);}
        }

        private void SetDefaults()
        {
            Start = DateTime.Now.AddDays(14);
        }

        [Required]
        [StringLength(50)]
        public virtual string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name="Position Title")]
        public string PositionTitle { get; set; }
        [Required]
        [StringLength(100)]
        public string Department { get; set; }
        [Required]
        [StringLength(100)]
        public string Unit { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name="Office Location")]
        public string OfficeLocation { get; set; }
        [Required]
        [StringLength(20)]
        public string Room { get; set; }
        [Required]
        [StringLength(50)]
        [Phone]
        [Display(Name="Contact Phone")]
        public string ContactPhone { get; set; }

        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
    }

    
}