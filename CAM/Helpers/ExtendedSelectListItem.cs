using System.Web.Mvc;

namespace CAM.Helpers
{
    public class ExtendedSelectListItem : SelectListItem
    {
        // used in the templates setup
        public bool Available { get; set; }
        public string Description { get; set; }

        // used on the request page
        public bool ForceSelect { get; set; }
        public string GroupId { get; set; }
    }
}