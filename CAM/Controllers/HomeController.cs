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

            return View(viewModel);
        }

        public JsonNetResult LoadTemplates(int unitId)
        {
            var templates = _repositoryFactory.RequestTemplateRepository.Queryable.Where(a => a.Unit.Id == unitId);
            return new JsonNetResult(templates.Select(a => new {Id = a.Id, Name = a.Name}));
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
            _activeDirectoryService.Initialize(site.Username, site.Password, site, null);
            _lyncService.Initialize(site.Username, site.Password, "https://lync.caesdo.caes.ucdavis.edu/OcsPowershell");

            var usr = _activeDirectoryService.GetUser("nononsense");

            _lyncService.EnableLync(usr.Id);

            
            //var groups = new List<string> { "Developers", "CRU" };
            ////_activeDirectoryService.CreateUser("Johnny", "McFakerson", string.Empty, "mcfake", "OU=non-Admin Users - CRU,OU=Users,OU=AGDEAN,OU=DEPARTMENTS,DC=caesdo,DC=caes,DC=ucdavis,DC=edu", "Fake person", "unit", groups.ToList());

            //var request = _repositoryFactory.RequestRepository.GetNullableById(3);
            //var adUser = new AdUser();
            //AutoMapper.Mapper.Map(request, adUser);
            //_activeDirectoryService.CreateUser(adUser, request.OrganizationalUnit.Path, groups);

            //_activeDirectoryService.GetUser("fakerson");

            //var result = _activeDirectoryService.GetUser("lai");
            
            //_activeDirectoryService.AssignUserToGroup("fakerson", "CRU");

            //if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) )
            //{
            //    var results = new List<string>();

            //    _activeDirectoryService.Initialize(userName, password, LoadSite());
            //    var result = _activeDirectoryService.GetUser("lai");

            //    if (result != null)
            //    {
            //        results.Add(result.Email);

            //        var du = _directorySearchService.FindUser(result.Email);
            //        results.Add(du.LoginId);
            //        results.Add(result.EmployeeId);
            //        results.Add("----------------");
            //    }
            //    else
            //    {
            //        results.Add("not found");
            //    }

            //    //_activeDirectoryService.AssignEmployeeId("lai", "anlai");

            //    return View(results);    
            //}

            return View();
        }
    }
}
