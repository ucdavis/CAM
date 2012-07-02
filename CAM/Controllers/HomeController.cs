using System.Linq;
using System.Web.Mvc;
using CAM.Core.Repositories;
using CAM.Models;
using UCDArch.Web.ActionResults;

namespace CAM.Controllers
{
    public class HomeController : ApplicationController
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public HomeController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
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
    }
}
