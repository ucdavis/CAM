using System.Collections.Generic;
using System.Linq;
using CAM.Core.BaseClasses;
using CAM.Core.Repositories;
using CAM.Services;

namespace CAM.Models
{
    public class AdGroupViewModel
    {
        public List<AdGroupCompare> AdGroupCompares { get; set; }

        public static AdGroupViewModel Create(IRepositoryFactory repositoryFactory, string siteId, List<AdGroup> adGroups)
        {
            var viewModel = new AdGroupViewModel()
                {
                    AdGroupCompares = new List<AdGroupCompare>()
                };

            var groups = repositoryFactory.SecurityGroupRepository.Queryable.Where(a => a.Site.Id == siteId && a.IsActive).ToList();

            var add = adGroups.Where(a => !groups.Any(x => x.SID == a.SID)).ToList();
            viewModel.AdGroupCompares.AddRange(add.Select(a => new AdGroupCompare(){ SID = a.SID, Name = a.Name, Description = a.Description, ChangeType = ChangeType.Add}));

            var remove = groups.Where(a => !adGroups.Any(x => x.SID == a.SID)).ToList();
            viewModel.AdGroupCompares.AddRange(remove.Select(a => new AdGroupCompare() {SID = a.SID, GroupId = a.Id, Name = a.Name, Description = a.Description, ChangeType = ChangeType.Remove}));

            var overlap = adGroups.Where(a => groups.Any(x => x.SID == a.SID)).ToList();
            var gu = (
                    from g in overlap let sg = groups.FirstOrDefault(a => a.SID == g.SID) 
                    where sg.Name != g.Name || sg.Description != g.Description 
                    select new AdGroupCompare(){ SID = g.SID, Name = g.Name, Description = g.Description, ChangeType = ChangeType.Update}
                    ).ToList();
            viewModel.AdGroupCompares.AddRange(gu);

            return viewModel;
        }



    }

    public class AdGroupCompare
    {
        public string SID { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ChangeType ChangeType { get; set; }
    }

    public enum ChangeType { Add, Remove, Update }
}