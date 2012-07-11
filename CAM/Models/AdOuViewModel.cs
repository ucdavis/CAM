using System.Collections.Generic;
using System.Linq;
using CAM.Core.Domain;
using CAM.Core.Repositories;
using CAM.Helpers;
using CAM.Services;

namespace CAM.Models
{
    public class AdOuViewModel
    {
        public List<AdOuCompare> AdOuCompares { get; set; }

        public static AdOuViewModel Create(IRepositoryFactory repositoryFactory, Site site, List<AdOrganizationalUnit> results)
        {
            var viewModel = new AdOuViewModel() { AdOuCompares = new List<AdOuCompare>()};

            var ous = site.OrganizationalUnits.Where(a => a.IsActive);

            var add = results.Where(a => !ous.Any(x => x.Path == a.Path)).ToList();
            viewModel.AdOuCompares.AddRange(add.Select(a => new AdOuCompare(){ Name = a.Name, Path = a.Path, ChangeType = ChangeType.Add}));

            var remove = ous.Where(a => !results.Any(x => x.Path == a.Path)).ToList();
            viewModel.AdOuCompares.AddRange(remove.Select(a => new AdOuCompare() { Name = a.Name, Path = a.Path, ChangeType = ChangeType.Remove }));

            return viewModel;
        }

    }

    public class AdOuCompare
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ChangeType ChangeType { get; set; }
    }
}