using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;

namespace CAM.Controllers
{
    public class SiteController : ApplicationController
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SiteController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public ActionResult Settings()
        {
            return View(LoadSite());
        }

        public ActionResult Edit()
        {
            return View(LoadSite());
        }

        [HttpPost]
        public ActionResult Edit(Site siteValues, List<string> securityou, List<string> userou )
        {
            if (ModelState.IsValid)
            {
                var siteToEdit = LoadSite();

                siteToEdit.Name = siteValues.Name;
                siteToEdit.ActiveDirectoryServer = siteValues.ActiveDirectoryServer;
                siteToEdit.SecurityGroupOu = string.Join("|", securityou.Where(a => !string.IsNullOrEmpty(a)));
                siteToEdit.UserOu = string.Join("|", userou.Where(a => !string.IsNullOrEmpty(a)));
                siteToEdit.Username = siteValues.Username;
                siteToEdit.Password = siteValues.Password;

                _repositoryFactory.SiteRepository.EnsurePersistent(siteToEdit);
                Message = "Site has been updated.";
                return RedirectToAction("Settings");
            }

            return View(LoadSite());
        }


    }
}
