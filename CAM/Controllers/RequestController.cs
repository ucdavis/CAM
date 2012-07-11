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

        public ActionResult Index(int? id)
        {
            var viewModel = RequestViewModel.Create(_repositoryFactory, null, LoadSite(), id);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(int? id, Request request)
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
    }
}
