using System.Collections.Generic;
using System.Linq;
using CAM.Core.BaseClasses;
using CAM.Core.Repositories;
using CAM.Services;

namespace CAM.Models
{
    public class AdGroupViewModel
    {
        public IEnumerable<AdGroup> GroupsToAdd { get; set; }
        public IEnumerable<GroupBase> GroupsToRemove { get; set; }

        public static AdGroupViewModel Create(IRepositoryFactory repositoryFactory, string siteId, GroupType groupType, List<AdGroup> adGroups)
        {
            var viewModel = new AdGroupViewModel();

            var groups = new List<GroupBase>();

            groups = new List<GroupBase>(repositoryFactory.SecurityGroupRepository.Queryable.Where(a => a.Site.Id == siteId && a.IsActive));                

            viewModel.GroupsToAdd = adGroups.Where(a => !groups.Any(x => x.SID == a.SID)).ToList();
            viewModel.GroupsToRemove = groups.Where(a => !adGroups.Any(x => x.SID == a.SID)).ToList();

            return viewModel;
        }
    }
}