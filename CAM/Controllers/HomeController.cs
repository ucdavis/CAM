using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;
using CAM.Models;
using CAM.Services;
using CAM.Web.Services;
using UCDArch.Web.ActionResults;

namespace CAM.Controllers
{
    public class HomeController : ApplicationController
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IDirectorySearchService _directorySearchService;
        private readonly ILyncService _lyncService;

        public HomeController(IRepositoryFactory repositoryFactory, IActiveDirectoryService activeDirectoryService, IDirectorySearchService directorySearchService, ILyncService lyncService)
        {
            _repositoryFactory = repositoryFactory;
            _activeDirectoryService = activeDirectoryService;
            _directorySearchService = directorySearchService;
            _lyncService = lyncService;
        }

        public ActionResult Index()
        {
            var viewModel = HomeIndexViewModel.Create(_repositoryFactory, Site);

            if (!viewModel.HasSite() && _repositoryFactory.SiteRepository.Queryable.Count() == 1)
            {
                var site = _repositoryFactory.SiteRepository.Queryable.First();
                return RedirectToAction("Index", new {Site = site.Id});
            }

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your quintessential contact page.";

            return View();
        }

        public ActionResult Test()
        {
            var site = LoadSite();
            _activeDirectoryService.Initialize(site.Username, site.GetPassword(EncryptionKey), site, site.LyncUri, site.ExchangeUri);

            /*
             * Create User
             */
            var request = _repositoryFactory.RequestRepository.GetNullableById(8);
            var aduser = new AdUser();
            AutoMapper.Mapper.Map(request, aduser);

            var loginid = _activeDirectoryService.CreateUser(aduser, request.OrganizationalUnit.Path, request.SecurityGroups.Select(a => a.SID).ToList(), request.NeedsEmail, "Dean's Office Staff Mailboxes A-L");

            return View();
        }
    }
}
