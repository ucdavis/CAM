using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CAM.Core.Domain;
using CAM.Core.Repositories;

namespace CAM.Models
{
    public class RequestTemplateViewModel
    {
        public RequestTemplate Request { get; set; }

        public IEnumerable<Unit> Units { get; set; }

        public static RequestTemplateViewModel Create(IRepositoryFactory repositoryFactory, string siteId, RequestTemplate requestTemplate = null)
        {
            var viewModel = new RequestTemplateViewModel()
                {
                    Request= requestTemplate ?? new RequestTemplate(),
                    Units = repositoryFactory.UnitRepository.Queryable.Where(a => a.Site.Id == siteId).ToList()
                };

            return viewModel;
        }

        public List<SelectListItem> GetUnits()
        {
            return Units.Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name, Selected = Request.Unit == a }).ToList();
        }
    }
}