using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;

namespace CAM.Models
{
    public class RequestViewModel
    {
        public Request Request { get; set; }

        // list values
        public IEnumerable<Software> Softwares { get; set; }
        public IEnumerable<NetworkShare> NetworkShares { get; set; }

        public static RequestViewModel Create(IRepositoryFactory repositoryFactory, Request request, Site site, int? templateId)
        {
            var viewModel = new RequestViewModel()
                {
                    Request = new Request()
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
                }
                else
                {
                    viewModel.Softwares = repositoryFactory.SoftwareRepository.Queryable.Where(a => a.Site == site).ToList();
                    viewModel.NetworkShares = repositoryFactory.NetworkShareRepository.Queryable.Where(a => a.Site == site).ToList();
                }
            }

            return viewModel;
        }

        public IEnumerable<SelectListItem> GetSoftwareList()
        {
            return Softwares.Select(s => new SelectListItem() { Selected = Request.Software.Contains(s), Text = s.Name, Value = s.Id.ToString() }).ToList();
        }

        public IEnumerable<CustomSelect> GetNetworkShareList()
        {
            return NetworkShares.Select(n => new CustomSelect() {Selected = Request.NetworkShares.Contains(n), Text = n.Name, Value = n.Id.ToString(), ForceSelect = n.ForceSelect}).ToList();
        }

    }

    public class CustomSelect : SelectListItem
    {
        public bool ForceSelect { get; set; }
    }
}