using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;
using CAM.Filters;
using CAM.Services;
using UCDArch.Web.ActionResults;

namespace CAM.Controllers
{
    public class CloseAccountController : ApplicationController
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IRepositoryFactory _repositoryFactory;

        public CloseAccountController(IActiveDirectoryService activeDirectoryService, IRepositoryFactory repositoryFactory)
        {
            _activeDirectoryService = activeDirectoryService;
            _repositoryFactory = repositoryFactory;
        }

        [AdminOnly]
        public ActionResult Index()
        {
            var requests = _repositoryFactory.CloseRequestRepository.Queryable.Where(a => a.IsPending);
            return View(requests);
        }

        [AdminOnly]
        public ActionResult Details(int id)
        {
            var request = _repositoryFactory.CloseRequestRepository.GetNullableById(id);

            if (request == null)
            {
                Message = "Unable to load request, please try again.";
                return RedirectToAction("Index");
            }

            return View(request);
        }

        [AdminOnly]
        [HttpPost]
        public ActionResult Details(int id, bool approved)
        {
            var request = _repositoryFactory.CloseRequestRepository.GetNullableById(id);

            if (request == null)
            {
                Message = "Unable to load request, please try again.";
                return RedirectToAction("Index");
            }

            if (approved)
            {
                var site = LoadSite();
                _activeDirectoryService.Initialize(site.Username, site.GetPassword(EncryptionKey), site, null, null);
                _activeDirectoryService.DeactivateAccount(request.LoginId);
            }

            request.IsPending = false;
            _repositoryFactory.CloseRequestRepository.EnsurePersistent(request);

            Message = "Account has been deactivated.";
            return RedirectToAction("Index");
        }

        public ActionResult Request()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Request(string loginid)
        {
            var request = new CloseRequest()
                {
                    LoginId = loginid,
                    RequestedBy = User.Identity.Name
                };

            _repositoryFactory.CloseRequestRepository.EnsurePersistent(request);

            Message = "Close request has been submitted";
            return RedirectToAction("Index", "Home");
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
