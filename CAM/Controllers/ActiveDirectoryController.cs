using System.Collections.Generic;
using System.Linq;
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

        public ActionResult SecurityGroups()
        {
            var site = LoadSite();
            if (site.HasCredentials())
            {
                _activeDirectoryService.Initialize(site.Username, site.Password, site, null);
                var results = _activeDirectoryService.GetSecurityGroups();
                var viewModel = AdGroupViewModel.Create(_repositoryFactory, Site, results);
                return View(viewModel);    
            }

            return View();
        }

        [HttpPost]
        public ActionResult SecurityGroups(string userName, string password, List<string> add, List<string> update, List<int> remove)
        {
            var site = LoadSite();
            if (site.HasCredentials())
            {
                _activeDirectoryService.Initialize(site.Username, site.Password, site, null);    
            }
            else
            {
                _activeDirectoryService.Initialize(userName, password, site, null);
            }
            
            var results = _activeDirectoryService.GetSecurityGroups();
            
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

            if (update != null)
            {
                foreach(var s in update)
                {
                    var x = results.FirstOrDefault(y => y.SID == s);
                    var sgroup = _repositoryFactory.SecurityGroupRepository.Queryable.FirstOrDefault(a => a.Site.Id == Site && a.SID == s);

                    if (x != null && sgroup != null)
                    {
                        sgroup.Name = x.Name;
                        sgroup.Description = x.Description;
                        sgroup.IsActive = true;

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

            var viewModel = AdGroupViewModel.Create(_repositoryFactory, Site, results);
            return View(viewModel);
        }

        public ActionResult OrganizationalUnits()
        {
            var site = LoadSite();

            if (site.HasCredentials())
            {
                _activeDirectoryService.Initialize(site.Username, site.Password, LoadSite(), null);
                
                var results = _activeDirectoryService.GetOrganizationalUnits();
                var viewModel = AdOuViewModel.Create(_repositoryFactory, LoadSite(), results);
                return View(viewModel);
            }

            return View();
        }

        [HttpPost]
        public ActionResult OrganizationalUnits(string userName, string passWord, List<string> add, List<string> remove)
        {
            var site = LoadSite();

            if (site.HasCredentials())
            {
                _activeDirectoryService.Initialize(site.Username, site.Password, site, null);    
            }
            else
            {
                _activeDirectoryService.Initialize(userName, passWord, site, null);    
            }
            var results = _activeDirectoryService.GetOrganizationalUnits();

            if (add != null)
            {
                foreach(var a in add)
                {
                    var x = results.FirstOrDefault(y => y.Path == a);
                    var ou = _repositoryFactory.OrganizationalUnitRepository.Queryable.FirstOrDefault(z => z.Site.Id == Site && z.Path == a);
                    if (ou == null && x != null)
                    {
                        ou= new OrganizationalUnit() {Name = x.Name, Path = x.Path, Site = site};
                    }
                    else if (ou != null)
                    {
                        ou.IsActive = true;
                    }

                    if (ou!= null)
                    {
                        _repositoryFactory.OrganizationalUnitRepository.EnsurePersistent(ou);
                    }
                }
            }
            
            if (remove != null)
            {
                foreach(var r in remove)
                {
                    var ou = _repositoryFactory.OrganizationalUnitRepository.Queryable.FirstOrDefault(a => a.Site.Id == Site && a.Path == r);
                    if (ou != null)
                    {
                        ou.IsActive = false;
                        _repositoryFactory.OrganizationalUnitRepository.EnsurePersistent(ou);
                    }
                }
            }

            var viewModel = AdOuViewModel.Create(_repositoryFactory, LoadSite(), results);
            return View(viewModel);
        }
    }
}
