using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        ////
        //// POST: /RequestTemplate/Create

        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
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
            return View();
        }

        ////
        //// POST: /RequestTemplate/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        ////
        //// GET: /RequestTemplate/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /RequestTemplate/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        public JsonNetResult SearchDistributionList(string term)
        {
            var results = _repositoryFactory.DistributionListRepository.Queryable.Where(a => a.NameLower.Contains(term.ToLower())).OrderBy(a => a.Name).Select(a => new {value = a.Name, label = a.Name}).ToList();

            if (results.Any())
            {
                return new JsonNetResult(results);
            }

            return new JsonNetResult(new {value=string.Empty, label="No Results Found"});
        }

        public JsonNetResult SearchSecurityGroup(string term)
        {
            var results = _repositoryFactory.SecurityGroupRepository.Queryable.Where(a => a.NameLower.Contains(term.ToLower())).OrderBy(a => a.Name).Select(a => new { value = a.Name, label = a.Name }).ToList();

            if (results.Any())
            {
                return new JsonNetResult(results);
            }

            return new JsonNetResult(new { value = string.Empty, label = "No Results Found" });
        }
    }
}
