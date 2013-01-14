using CAM.Core.Repositories;
using CAM.Services;
using CAM.Web.Services;
using Castle.Windsor;
using UCDArch.Core.CommonValidator;
using UCDArch.Core.DataAnnotationsValidator.CommonValidatorAdapter;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using Castle.MicroKernel.Registration;

namespace CAM
{
    internal static class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddGenericRepositoriesTo(container);

            container.Register(Component.For<IValidator>().ImplementedBy<Validator>().Named("validator"));
            container.Register(Component.For<IDbContext>().ImplementedBy<DbContext>().Named("dbContext"));
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container)
        {
            container.Register(Component.For(typeof(IRepositoryWithTypedId<,>)).ImplementedBy(typeof(RepositoryWithTypedId<,>)).Named("repositoryWithTypedId"));
            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).Named("repositoryType"));
            container.Register(Component.For<IRepository>().ImplementedBy<Repository>().Named("repository"));
            container.Register(Component.For<IRepositoryFactory>().ImplementedBy(typeof(RepositoryFactory)).Named("repositoryFactory"));
            container.Register(Component.For<IActiveDirectoryService>().ImplementedBy(typeof(ActiveDirectoryService)).Named("activeDirectoryService"));
            container.Register(Component.For<ILyncService>().ImplementedBy(typeof(LyncService)).Named("lyncService"));
            container.Register(Component.For<IDirectorySearchService>().ImplementedBy(typeof(DirectorySearchService)).Named("directorySearchService"));

        }
    }
}