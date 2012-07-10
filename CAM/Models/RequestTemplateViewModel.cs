using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;
using CAM.Helpers;

namespace CAM.Models
{
    public class RequestTemplateViewModel
    {
        public RequestTemplate RequestTemplate { get; set; }

        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<Software> Softwares { get; set; }
        public IEnumerable<SecurityGroup> SecurityGroups { get; set; }

        public static RequestTemplateViewModel Create(IRepositoryFactory repositoryFactory, string siteId, RequestTemplate requestTemplate = null)
        {
            var site = repositoryFactory.SiteRepository.GetNullableById(siteId);

            var viewModel = new RequestTemplateViewModel()
                {
                    RequestTemplate= requestTemplate ?? new RequestTemplate() {Site = site},
                    Units = repositoryFactory.UnitRepository.Queryable.Where(a => a.Site.Id == siteId).ToList(),
                    Softwares = repositoryFactory.SoftwareRepository.Queryable.Where(a => a.Site.Id == siteId && a.IsActive).ToList(),
                    SecurityGroups = repositoryFactory.SecurityGroupRepository.Queryable.Where(a => a.Site.Id == siteId && a.IsActive).ToList()
                };

            return viewModel;
        }

        public List<SelectListItem> GetUnits()
        {
            return Units.Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name, Selected = RequestTemplate.Unit == a }).ToList();
        }

        public List<ExtendedSelectListItem> GetSoftware(bool web = false)
        {
            return Softwares.Where(a => a.WebApplication == web).Select(a => new ExtendedSelectListItem() { Value = a.Id.ToString(), Text = a.Name, Selected = RequestTemplate.Software.Contains(a), Available = RequestTemplate.AvailableSoftware.Contains(a)}).ToList();
        }

        public List<ExtendedSelectListItem> GetSecurityGroups()
        {
            return SecurityGroups.Select(a => new ExtendedSelectListItem() { Value = a.Id.ToString(), Text = a.Name, Selected = RequestTemplate.SecurityGroups.Contains(a), Available = RequestTemplate.AvailableSecurityGroups.Contains(a), Description = a.Description}).ToList();
        }
    }
}