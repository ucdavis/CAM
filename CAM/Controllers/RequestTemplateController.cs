using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;
using CAM.Models;
using UCDArch.Web.ActionResults;

namespace CAM.Controllers
{
    public class RequestTemplateController : ApplicationController
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public RequestTemplateController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        //
        // GET: /RequestTemplate/

        public ActionResult Index()
        {
            var templates = _repositoryFactory.RequestTemplateRepository.Queryable.Where(a => a.Site.Id == Site).ToList();

            return View(templates);
        }

        ////
        //// GET: /RequestTemplate/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //
        // GET: /RequestTemplate/Create

        public ActionResult Create()
        {
            var viewModel = RequestTemplateViewModel.Create(_repositoryFactory, Site);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(RequestTemplate requestTemplate)
        {
            if (ModelState.IsValid)
            {
                _repositoryFactory.RequestTemplateRepository.EnsurePersistent(requestTemplate);
                return Redirect("index");
            }

            var viewModel = RequestTemplateViewModel.Create(_repositoryFactory, Site, requestTemplate);
            return View(viewModel);
        }

        ////
        //// POST: /RequestTemplate/Create

        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("SelectUnit");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        ////
        //// GET: /RequestTemplate/Edit/5

        public ActionResult Edit(int id)
        {
            var template = _repositoryFactory.RequestTemplateRepository.GetNullableById(id);
            var viewModel = RequestTemplateViewModel.Create(_repositoryFactory, Site, template);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, RequestTemplate requestTemplate)
        {
            var templateToEdit = _repositoryFactory.RequestTemplateRepository.GetNullableById(id);

            if (ModelState.IsValid)
            {
                AutoMapper.Mapper.Map(requestTemplate, templateToEdit);

                _repositoryFactory.RequestTemplateRepository.EnsurePersistent(templateToEdit);
                return RedirectToAction("Index");
            }

            var viewModel = RequestTemplateViewModel.Create(_repositoryFactory, Site, templateToEdit);
            return View(viewModel);
        }

        public JsonNetResult SearchSecurityGroup(string term)
        {
            var results = _repositoryFactory.SecurityGroupRepository.Queryable.Where(a => a.SearchableName.Contains(term.ToLower())).OrderBy(a => a.Name).Select(a => new { value = a.Name, label = a.Name }).ToList();

            if (results.Any())
            {
                return new JsonNetResult(results);
            }

            return new JsonNetResult(new { value = string.Empty, label = "No Results Found" });
        }
    }

}
