using System.Linq;
using System.Web.Mvc;
using CAM.Services;
using UCDArch.Web.ActionResults;

namespace CAM.Controllers
{
    public class CloseAccountController : ApplicationController
    {
        private readonly IActiveDirectoryService _activeDirectoryService;

        public CloseAccountController(IActiveDirectoryService activeDirectoryService)
        {
            _activeDirectoryService = activeDirectoryService;
        }

        public ActionResult Request()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Request(string loginid)
        {
            var site = LoadSite();
            _activeDirectoryService.Initialize(site.Username, site.GetPassword(EncryptionKey), site, null, null);

            if (_activeDirectoryService.DeactivateAccount(loginid))
            {
                Message = "Account has been deactivated.";
                return RedirectToAction("Index", "Home");    
            }

            Message = "Error locating account, please try again.";
            return View();
        }

        public JsonNetResult Search(string loginId, string firstname, string lastname)
        {
            if (string.IsNullOrEmpty(loginId) && string.IsNullOrEmpty(firstname) && string.IsNullOrEmpty(lastname))
            {
                return new JsonNetResult(false);
            }

            var site = LoadSite();
            _activeDirectoryService.Initialize(site.Username, site.GetPassword(EncryptionKey), site, null, null);

            if (!string.IsNullOrEmpty(loginId))
            {
                var user = _activeDirectoryService.GetUser(loginId);
                if (user != null)
                {
                    return new JsonNetResult(new { user.FirstName, user.LastName, user.Id, user.Email});    
                }
            }
            else
            {
                var users = _activeDirectoryService.SearchUserByName(firstname, lastname);
                return new JsonNetResult(users.Select(a => new { a.FirstName, a.LastName, a.Id, a.Email}));
            }

            return new JsonNetResult(false);
        }

    }
}
