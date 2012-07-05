using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;
using CAM.Models;
using CAM.Services;

namespace CAM.Controllers
{
    public class ActiveDirectoryController : ApplicationController
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IActiveDirectoryService _activeDirectoryService;

        public ActiveDirectoryController(IRepositoryFactory repositoryFactory, IActiveDirectoryService activeDirectoryService)
        {
            _repositoryFactory = repositoryFactory;
            _activeDirectoryService = activeDirectoryService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DistributionLists()
        {
            var results = _activeDirectoryService.GetDistributionLists();
            var viewModel = AdGroupViewModel.Create(_repositoryFactory, Site, GroupType.Distribution, results);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DistributionLists(List<string> add, List<int> remove)
        {
            var results = _activeDirectoryService.GetDistributionLists();
            var site = _repositoryFactory.SiteRepository.GetNullableById(Site);

            if (add != null)
            {
                foreach (var s in add)
                {
                    var x = results.FirstOrDefault(y => y.SID == s);
                    var dist = _repositoryFactory.DistributionListRepository.Queryable.FirstOrDefault(a => a.Site.Id == Site && a.SID == s);
                    if (dist == null && x != null)
                    {
                        dist = new DistributionList() { Name = x.Name, SID = x.SID, Description = x.Description, Site = site };
                    }
                    else if (dist != null)
                    {
                        dist.IsActive = true;
                    }

                    if (dist != null)
                    {
                        _repositoryFactory.DistributionListRepository.EnsurePersistent(dist);    
                    }
                }    
            }
            
            if (remove != null)
            {
                foreach (var r in remove)
                {
                    var dist = _repositoryFactory.DistributionListRepository.GetNullableById(r);
                    if (dist != null)
                    {
                        dist.IsActive = false;
                        _repositoryFactory.DistributionListRepository.EnsurePersistent(dist);
                    }
                }    
            }

            var viewModel = AdGroupViewModel.Create(_repositoryFactory, Site, GroupType.Distribution, results);
            return View(viewModel);
        }

        public ActionResult SecurityGroups()
        {
            var results = _activeDirectoryService.GetSecurityGroups();
            var viewModel = AdGroupViewModel.Create(_repositoryFactory, Site, GroupType.Security, results);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SecurityGroups(List<string> add, List<int> remove)
        {
            var results = _activeDirectoryService.GetSecurityGroups();
            var site = _repositoryFactory.SiteRepository.GetNullableById(Site);

            if (add != null)
            {
                foreach (var s in add)
                {
                    var x = results.FirstOrDefault(y => y.SID == s);
                    var sgroup = _repositoryFactory.SecurityGroupRepository.Queryable.FirstOrDefault(a => a.Site.Id == Site && a.SID == s);
                    if (sgroup == null && x != null)
                    {
                        sgroup = new SecurityGroup() { Name = x.Name, SID = x.SID, Description = x.Description, Site = site };
                    }
                    else if (sgroup != null)
                    {
                        sgroup.IsActive = true;
                    }

                    if (sgroup != null)
                    {
                        _repositoryFactory.SecurityGroupRepository.EnsurePersistent(sgroup);
                    }
                }
            }

            if (remove != null)
            {
                foreach (var r in remove)
                {
                    var sgroup = _repositoryFactory.SecurityGroupRepository.GetNullableById(r);
                    if (sgroup != null)
                    {
                        sgroup.IsActive = false;
                        _repositoryFactory.SecurityGroupRepository.EnsurePersistent(sgroup);
                    }
                }
            }

            var viewModel = AdGroupViewModel.Create(_repositoryFactory, Site, GroupType.Security, results);
            return View(viewModel);
        }
    }
}
