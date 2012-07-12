using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        public HomeController(IRepositoryFactory repositoryFactory, IActiveDirectoryService activeDirectoryService, IDirectorySearchService directorySearchService)
        {
            _repositoryFactory = repositoryFactory;
            _activeDirectoryService = activeDirectoryService;
            _directorySearchService = directorySearchService;
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

        public ActionResult Test(string userName, string password)
        {
            
            var site = LoadSite();
            _activeDirectoryService.Initialize(site.Username, site.Password, site);
            _activeDirectoryService.CreateUser("Johnny", "McFakerson", string.Empty, "mcfake", "OU=non-Admin Users - CRU,OU=Users,OU=AGDEAN,OU=DEPARTMENTS,DC=caesdo,DC=caes,DC=ucdavis,DC=edu", "Fake person", null);

            //var result = _activeDirectoryService.GetUser("lai");
            


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
