using System.Web.Mvc;

namespace CAM.Helpers
{
    public class ExtendedSelectListItem : SelectListItem
    {
        public bool Available { get; set; }
        public string Description { get; set; }
    }
}