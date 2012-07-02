using System.Collections.Generic;
using System.Linq;
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
                    Request = new Request(),
                    Softwares = repositoryFactory.SoftwareRepository.Queryable.Where(a => a.Site == site).ToList(),
                    NetworkShares = repositoryFactory.NetworkShareRepository.Queryable.Where(a => a.Site == site).ToList()
                };

            if (request == null)
            {
                if (templateId.HasValue)
                {
                    // load the template
                    var template = repositoryFactory.RequestTemplateRepository.GetNullableById(templateId.Value);
    
                    // copy the values
                    viewModel.Request = new Request(template);
                }
            }

            return viewModel;
        }

    }
}