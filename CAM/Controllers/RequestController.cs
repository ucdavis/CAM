using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;
using CAM.Filters;
using CAM.Models;
using CAM.Services;
using UCDArch.Web.ActionResults;

namespace CAM.Controllers
{
    public class RequestController : ApplicationController
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly ILyncService _lyncService;

        public RequestController(IRepositoryFactory repositoryFactory, IActiveDirectoryService activeDirectoryService, ILyncService lyncService)
        {
            _repositoryFactory = repositoryFactory;
            _activeDirectoryService = activeDirectoryService;
            _lyncService = lyncService;
        }

        public ActionResult SelectUnit()
        {
            var units = _repositoryFactory.UnitRepository.Queryable.Where(a => a.Site.Id == Site  && a.IsActive);
            return View(units);
        }

        public ActionResult Create(int? id)
        {
            var viewModel = RequestViewModel.Create(_repositoryFactory, null, LoadSite(), id);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(int? id, Request request)
        {
            if (ModelState.IsValid)
            {
                request.CreatedBy = User.Identity.Name;

                _repositoryFactory.RequestRepository.EnsurePersistent(request);
                Message = "Request has been successfully submitted.";
                return RedirectToAction("Index", "Home");
            }

            var viewModel = RequestViewModel.Create(_repositoryFactory, request, LoadSite(), id);
            return View(viewModel);
        }

        [AdminOnly]
        public ActionResult List(bool viewAll = false)
        {
            var results = _repositoryFactory.RequestRepository.Queryable.Where(a => a.Site.Id == Site);

            if (!viewAll)
            {
                results = results.Where(a => a.Pending);
            }

            return View(results);    
        }

        [AdminOnly]
        public ActionResult Review(int id)
        {
            var request = _repositoryFactory.RequestRepository.GetNullableById(id);

            if (request == null)
            {
                Message = "Request not found.";
                return RedirectToAction("SelectUnit");
            }
            
            return View(request);
        }

        [AdminOnly]
        [HttpPost]
        public ActionResult Review(int id, bool Approved)
        {
            var request = _repositoryFactory.RequestRepository.GetNullableById(id);

            if (request == null)
            {
                Message = "Request not found.";
                return RedirectToAction("SelectUnit");
            }

            // validate the necessary fields to create the necessary objects

            var site = LoadSite();

            // then create the objects
            var adUsr = new AdUser();
            AutoMapper.Mapper.Map(request, adUsr);
            _activeDirectoryService.Initialize(site.Username, site.GetPassword(EncryptionKey), site, site.LyncUri);
            _activeDirectoryService.CreateUser(adUsr, request.OrganizationalUnit.Path, request.SecurityGroups.Select(a => a.SID).ToList());

            request.Pending = false;
            request.Approved = Approved;
            _repositoryFactory.RequestRepository.EnsurePersistent(request);

            Message = string.Format("Request for {0} {1} has {2} approved.", request.FirstName, request.LastName, Approved ? "been" : "not been");
            return RedirectToAction("SelectUnit");
        }

        public ActionResult Edit(int id)
        {
            var request = _repositoryFactory.RequestRepository.GetNullableById(id);

            if (request == null)
            {
                Message = "The request was not found, please try your request again.";
                return RedirectToAction("SelectUnit");
            }

            var viewModel = RequestViewModel.Create(_repositoryFactory, request, LoadSite(), null);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, Request request)
        {
            var requestToEdit = _repositoryFactory.RequestRepository.GetNullableById(id);

            if (ModelState.IsValid && requestToEdit != null)
            {
                AutoMapper.Mapper.Map(request, requestToEdit);

                // deal with the lists
                //var securityadd = request.SecurityGroups.Where(a => !requestToEdit.SecurityGroups.Contains(a));
                //var securityremove = requestToEdit.SecurityGroups.Where(a => !request.SecurityGroups.Contains(a));
                //foreach(var a in securityadd) requestToEdit.SecurityGroups.Add(a);
                //foreach (var a in securityremove) requestToEdit.SecurityGroups.Remove(a);

                //var softwareadd = request.Software.Where(a => !requestToEdit.Software.Contains(a));
                //var softwareremove = requestToEdit.Software.Where(a => !request.Software.Contains(a));
                //foreach (var a in softwareadd) requestToEdit.Software.Add(a);
                //foreach (var a in softwareremove) requestToEdit.Software.Remove(a);

                _repositoryFactory.RequestRepository.EnsurePersistent(requestToEdit);
                Message = "Request has been updated.";
                return RedirectToAction("Review", new {id = id});
            }

            var viewModel = RequestViewModel.Create(_repositoryFactory, request, LoadSite(), null);
            return View(viewModel);
        }

        /*
         * Ajax Functions
         */

        public JsonNetResult LoadTemplates(int unitId)
        {
            var templates = _repositoryFactory.RequestTemplateRepository.Queryable.Where(a => a.Unit.Id == unitId);
            return new JsonNetResult(templates.Select(a => new { Id = a.Id, Name = a.Name }));
        }
    }
}
