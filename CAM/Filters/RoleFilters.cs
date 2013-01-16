using System.Web.Http;

namespace CAM.Filters
{
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        public AdminOnlyAttribute()
        {
            Roles = "Admin";
        }
    }

    public static class RoleNames
    {
        public static string Admin = "Admin";
    }
}