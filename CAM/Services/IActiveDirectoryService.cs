using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using CAM.Core.Domain;

namespace CAM.Services
{
    public interface IActiveDirectoryService
    {
        void Initialize(string userName, string password, Site site);

        /// <summary>
        /// Loads a list of security groups according to site filters
        /// </summary>
        /// <returns></returns>
        List<AdGroup> GetSecurityGroups();
        /// <summary>
        /// Loads a list of distribution lists according to site filters
        /// </summary>
        /// <returns></returns>
        List<AdGroup> GetDistributionLists();

        List<AdOrganizationalUnit> GetOrganizationalUnits();
        List<AdUser> GetUsers();
    }

    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly string _aduser = ConfigurationManager.AppSettings["aduser"];
        private readonly string _adpass = ConfigurationManager.AppSettings["adpass"];

        private Site Site { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }

        public void Initialize(string userName, string password, Site site)
        {
            Site = site;
            UserName = userName;
            Password = password;
        }

        public List<AdGroup> GetSecurityGroups()
        {
            var results = LoadGroups(true);

            return results.Select(a => new AdGroup(a)).ToList();
        }

        public List<AdGroup> GetDistributionLists()
        {
            var results = LoadGroups(false);

            return results.Select(a => new AdGroup(a)).ToList();
        }

        public List<AdOrganizationalUnit> GetOrganizationalUnits()
        {
            var results = GetUsers();
            return results.Select(a => new AdOrganizationalUnit(a.GetContainer(), a.GetContainerPath())).Distinct().ToList();
        }

        private IEnumerable<GroupPrincipal> LoadGroups(bool security)
        {
            if (Site == null || string.IsNullOrEmpty(Site.SecurityGroupOu)) { return new List<GroupPrincipal>(); }

            var ous = Site.SecurityGroupOu.Split('|');
            var results = new List<GroupPrincipal>();

            foreach(var ou in ous)
            {
                using(var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, Site.SecurityGroupOu, UserName, Password))
                {
                    var g = new GroupPrincipal(ad) { IsSecurityGroup = security };
                    var searcher = new PrincipalSearcher(g);
        
                    results.AddRange(searcher.FindAll().Select(a => (GroupPrincipal)a));
                }
            }

            return results;
        }

        public List<AdUser> GetUsers()
        {
            var results = LoadUsers();

            return results.Select(a => new AdUser(a)).ToList();
        }

        private IEnumerable<UserPrincipal> LoadUsers()
        {
            if (Site == null || string.IsNullOrEmpty(Site.UserOu)) {return new List<UserPrincipal>();}

            var ous = Site.UserOu.Split('|');
            var results = new List<UserPrincipal>();

            foreach(var ou in ous)
            {
                using(var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, ou, UserName, Password))
                {
                    var u = new UserPrincipal(ad) {Enabled = true};
                    var searcher = new PrincipalSearcher(u);

                    results.AddRange(searcher.FindAll().Select(a => (UserPrincipal)a));        
                }
            }

            return results;
        }
    } 

    public class AdGroup
    {
        public AdGroup(GroupPrincipal groupPrincipal)
        {
            Id = groupPrincipal.SamAccountName;
            Name = groupPrincipal.Name;
            Description = groupPrincipal.Description;
            SID = groupPrincipal.Sid.Value;
        }

        /// <summary>
        /// AD Sam Account Name
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Display Name of the Object
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of the object
        /// </summary>
        public string Description { get; set; }

        public string SID { get; set; }
    }

    public class AdUser
    {
        public AdUser(UserPrincipal userPrincipal)
        {
            Id = userPrincipal.SamAccountName;
            EmployeeId = userPrincipal.EmployeeId;

            FirstName = userPrincipal.GivenName;
            LastName = userPrincipal.Surname;
            Email = userPrincipal.EmailAddress;
            Description = userPrincipal.Description;
            SID = userPrincipal.Sid.Value;

            ContainerPath = userPrincipal.DistinguishedName;

            Expiration = userPrincipal.AccountExpirationDate;
            Enabled = userPrincipal.Enabled;
        }

        /// <summary>
        /// SAM account name
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Employee ID field from AD
        /// </summary>
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// User's description
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Security ID
        /// </summary>
        public string SID { get; set; }
        public string ContainerPath { get; set; }
        /// <summary>
        /// Account expiration
        /// </summary>
        public DateTime? Expiration { get; set; }
        public bool? Enabled { get; set; }

        public string GetContainer()
        {
            var startIndex = ContainerPath.IndexOf("OU=") + 3;
            var endIndex = ContainerPath.IndexOf(',', startIndex);

            return ContainerPath.Substring(startIndex, endIndex - startIndex);
        }

        public string GetContainerPath()
        {
            var startIndex = ContainerPath.IndexOf("OU=");
            var endIndex = ContainerPath.Length;

            return ContainerPath.Substring(startIndex, endIndex - startIndex);
        }
    }

    public class AdOrganizationalUnit
    {
        public AdOrganizationalUnit(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; set; }
        public string Path { get; set; }
    }
}
