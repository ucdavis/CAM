using CAM.Core.BaseClasses;
using FluentNHibernate.Mapping;

namespace CAM.Core.Domain
{
    public class Software : ObjectBase
    {
        public Software()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            IsActive = true;
            WebApplication = false;
        }

        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual bool WebApplication { get; set; }
        public virtual WebRoleManager WebRoleManager { get; set; }
    }

    public class SoftwareMap : ClassMap<Software>
    {
        public SoftwareMap()
        {
            Table("Software");

            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.IsActive);
            References(x => x.Site);
            Map(x => x.WebApplication);
            Map(x => x.WebRoleManager).CustomType<NHibernate.Type.EnumStringType<WebRoleManager>>();
        }
    }

    public enum WebRoleManager
    {
        Catbert
    }
}
