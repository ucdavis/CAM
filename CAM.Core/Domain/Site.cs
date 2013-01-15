using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CAM.Core.Helpers;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace CAM.Core.Domain
{
    public class Site : DomainObjectWithTypedId<string>
    {
        public Site()
        {
            Units = new List<Unit>();
            OrganizationalUnits = new List<OrganizationalUnit>();
        }

        public virtual string Name { get; set; }
        public virtual IList<Unit> Units { get; set; }

        [Display(Name="Active Directory Server")]
        public virtual string ActiveDirectoryServer { get; set; }
        [Display(Name="Security Group OU(s)")]
        public virtual string SecurityGroupOu { get; set; }
        [Display(Name="User Group OU(s)")]
        public virtual string UserOu { get; set; }

        public virtual string Username { get; set; }
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }

        [Display(Name = "Lync Server Uri")]
        public string LyncUri { get; set; }

        public virtual IList<OrganizationalUnit> OrganizationalUnits { get; set; }

        public virtual bool HasUnits()
        {
            return Units.Any();
        }

        public virtual List<string> GetSecurityOus()
        {
            return SecurityGroupOu.Split('|').ToList();
        }

        public virtual List<string> GetUserOus()
        {
            var test = UserOu.Split('|').ToList();
            return test;
        }

        public virtual bool HasCredentials()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        public virtual string GetPassword(string key)
        {
            return Crypto.DecryptString(Password, key);
        }

        public virtual void SetPassword(string password, string key)
        {
            Password = Crypto.EncryptString(password, key);
        }
    }

    public class SiteMap : ClassMap<Site>
    {
        public SiteMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            HasMany(x => x.Units);
            Map(x => x.ActiveDirectoryServer);
            Map(x => x.SecurityGroupOu);
            Map(x => x.UserOu);
            Map(x => x.LyncUri);

            Map(x => x.Username);
            Map(x => x.Password);

            HasMany(x => x.OrganizationalUnits);
        }
    }
}
