using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;
using CAM.Models;

namespace CAM.Controllers
{
    public class RequestController : ApplicationController
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public RequestController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
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
                _repositoryFactory.RequestRepository.EnsurePersistent(request);
                Message = "Request has been successfully submitted.";
                return RedirectToAction("Index", "Home");
            }

            var viewModel = RequestViewModel.Create(_repositoryFactory, request, LoadSite(), id);
            return View(viewModel);
        }

        public ActionResult Index(bool viewAll = false)
        {
            var results = _repositoryFactory.RequestRepository.Queryable.Where(a => a.Site.Id == Site);

            if (!viewAll)
            {
                results = results.Where(a => a.Pending);
            }

            return View(results);
        }

        public ActionResult Review(int id)
        {
            var request = _repositoryFactory.RequestRepository.GetNullableById(id);

            if (request == null)
            {
                Message = "Request not found.";
                return RedirectToAction("Index");
            }
            
            return View(request);
        }

        [HttpPost]
        public ActionResult Review(int id, bool Approved)
        {
            var request = _repositoryFactory.RequestRepository.GetNullableById(id);

            if (request == null)
            {
                Message = "Request not found.";
                return RedirectToAction("Index");
            }

            request.Pending = false;
            request.Approved = Approved;
            _repositoryFactory.RequestRepository.EnsurePersistent(request);

            Message = string.Format("Requeste for {0} {1} has {2} approved.", request.FirstName, request.LastName, Approved ? "been" : "not been");
            return RedirectToAction("Index");
        }
    }
}
