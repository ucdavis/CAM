using CAM.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace CAM.Core.Repositories
{
    public interface IRepositoryFactory
    {
        IRepository<DistributionList> DistributionListRepository { get; set; }
        IRepository<NetworkShare> NetworkShareRepository { get; set; }
        IRepository<Request> RequestRepository { get; set; }
        IRepository<RequestTemplate> RequestTemplateRepository { get; set; }
        IRepository<SecurityGroup> SecurityGroupRepository { get; set; }
        IRepositoryWithTypedId<Site, string> SiteRepository { get; set; }
        IRepository<Software> SoftwareRepository { get; set; }
        IRepository<Unit> UnitRepository { get; set; }
    }

    public class RepositoryFactory : IRepositoryFactory
    {
        public IRepository<DistributionList> DistributionListRepository { get; set; }
        public IRepository<NetworkShare> NetworkShareRepository { get; set; }
        public IRepository<Request> RequestRepository { get; set; }
        public IRepository<RequestTemplate> RequestTemplateRepository { get; set; }
        public IRepository<SecurityGroup> SecurityGroupRepository { get; set; }
        public IRepositoryWithTypedId<Site, string> SiteRepository { get; set; }
        public IRepository<Software> SoftwareRepository { get; set; }
        public IRepository<Unit> UnitRepository { get; set; }
    }

}
