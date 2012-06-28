using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class Request : DomainObject
    {
        public Request()
        {
            Start = DateTime.Now;
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

        public PositionType Type { get; set; }
    }

    public enum PositionType { NewPosition = 0, Rehire = 1};
}