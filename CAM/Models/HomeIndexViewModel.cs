using System.Collections.Generic;
using System.Linq;
using CAM.Core.Domain;
using CAM.Core.Repositories;

namespace CAM.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Site> Sites { get; set; }
        public IEnumerable<Unit> Units { get; set; }
        public Site Site { get; set; }

        public static HomeIndexViewModel Create(IRepositoryFactory repositoryFactory, string siteId)
        {
            var viewModel = new HomeIndexViewModel()
                {
                    Site = !string.IsNullOrEmpty(siteId) ? repositoryFactory.SiteRepository.GetNullableById(siteId.ToUpper()) : null
                };

            if(viewModel.HasSite())
            {
                viewModel.Units = viewModel.Site.Units.Where(a => a.IsActive);
            }
            else
            {
                viewModel.Sites = repositoryFactory.SiteRepository.GetAll();
            }

            return viewModel;
        }

        public bool HasSite()
        {
            return this.Site != null;
        }

    }
}