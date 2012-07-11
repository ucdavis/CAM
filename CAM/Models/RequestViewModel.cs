using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;
using CAM.Helpers;

namespace CAM.Models
{
    public class RequestViewModel
    {
        public Request Request { get; set; }

        // list values
        public IEnumerable<Software> Softwares { get; set; }
        public IEnumerable<NetworkShare> NetworkShares { get; set; }
        public IEnumerable<SecurityGroup> SecurityGroups { get; set; }

        public static RequestViewModel Create(IRepositoryFactory repositoryFactory, Request request, Site site, int? templateId)
        {
            var viewModel = new RequestViewModel()
            {
                Request = new Request() {Site = site}
            };

            if (request == null)
            {
                if (templateId.HasValue)
                {
                    // load the template
                    var template = repositoryFactory.RequestTemplateRepository.GetNullableById(templateId.Value);

                    // copy the values
                    viewModel.Request = new Request(template);

                    viewModel.Softwares = template.AvailableSoftware;
                    viewModel.NetworkShares = template.AvailableNetworkShares;
                    viewModel.SecurityGroups = template.AvailableSecurityGroups;
                }
                else
                {
                    viewModel.Softwares = repositoryFactory.SoftwareRepository.Queryable.Where(a => a.Site == site && a.IsActive).ToList();
                    viewModel.NetworkShares = repositoryFactory.NetworkShareRepository.Queryable.Where(a => a.Site == site && a.IsActive).ToList();
                    viewModel.SecurityGroups = repositoryFactory.SecurityGroupRepository.Queryable.Where(a => a.Site == site && a.IsActive).ToList();
                }
            }

            return viewModel;
        }

        public IEnumerable<SelectListItem> GetSecurityGroups()
        {
            return SecurityGroups.Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name, Selected = Request.SecurityGroups.Contains(a)}).ToList();
        }

        public IEnumerable<SelectListItem> GetSoftwareList(bool webApplication = false)
        {
            return Softwares.Where(a => a.WebApplication == webApplication).Select(s => new SelectListItem() { Selected = Request.Software.Contains(s), Text = s.Name, Value = s.Id.ToString() }).ToList();
        }

        public IEnumerable<ExtendedSelectListItem> GetNetworkShareList()
        {
            return NetworkShares.Select(n => new ExtendedSelectListItem() { Selected = Request.NetworkShares.Contains(n), Text = n.Name, Value = n.Id.ToString(), ForceSelect = n.ForceSelect, GroupId = n.GroupId }).ToList();
        }

    }
}