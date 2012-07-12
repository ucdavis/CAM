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
        /// <summary>
        /// Gets a list of OUs, using user searching and returning those containers
        /// </summary>
        /// <returns></returns>
        List<AdOrganizationalUnit> GetOrganizationalUnits();
        /// <summary>
        /// Gets a list of all users
        /// </summary>
        /// <returns></returns>
        List<AdUser> GetUsers();

        AdUser GetUser(string userId);

        void AssignEmployeeId(string userId, string employeeId);
        void CreateUser(string firstName, string lastName, string email, string loginId, string container, string title, List<string> securityGroups);
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

            return results.ToList();
        }

        public List<AdGroup> GetDistributionLists()
        {
            var results = LoadGroups(false);

            return results.ToList();
        }

        public List<AdOrganizationalUnit> GetOrganizationalUnits()
        {
            var results = GetUsers();
            return results.Select(a => new {Container= a.GetContainer(), Path = a.GetContainerPath()}).Distinct()
                .Select(a => new AdOrganizationalUnit(a.Container, a.Path)).ToList();
        }

        private IEnumerable<AdGroup> LoadGroups(bool security)
        {
            if (Site == null || string.IsNullOrEmpty(Site.SecurityGroupOu)) { return new List<AdGroup>(); }

            var ous = Site.SecurityGroupOu.Split('|');
            var results = new List<AdGroup>();

            foreach(var ou in ous)
            {
                using(var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, ou, UserName, Password))
                {
                    var g = new GroupPrincipal(ad) { IsSecurityGroup = security };
                    var searcher = new PrincipalSearcher(g);
        
                    results.AddRange(searcher.FindAll().Select(a => new AdGroup((GroupPrincipal)a)));
                }
            }

            return results;
        }

        public List<AdUser> GetUsers()
        {
            var results = LoadUsers();

            return results.ToList();
        }

        public AdUser GetUser(string userId)
        {
            if (Site == null || string.IsNullOrEmpty(Site.UserOu)) { return null; }

            var ous = Site.UserOu.Split('|');

            foreach (var ou in ous)
            {
                using (var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, ou, UserName, Password))
                {
                    var u = UserPrincipal.FindByIdentity(ad, userId);
                    if (u != null) return new AdUser(u);
                }
            }

            return null;
        }

        public void AssignEmployeeId(string userId, string employeeId)
        {
            if (Site == null || string.IsNullOrEmpty(Site.UserOu)) { return; }

            var ous = Site.UserOu.Split('|');

            foreach (var ou in ous)
            {
                using (var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, ou, UserName, Password))
                {
                    var u = UserPrincipal.FindByIdentity(ad, userId);
                    if (u != null)
                    {
                        u.EmployeeId = employeeId;
                        u.Save();

                        return;
                    }
                }
            }
        }

        public void CreateUser(string firstName, string lastName, string email, string loginId, string container, string title, List<string> securityGroups)
        {
            using (var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, container, UserName, Password))
            {
                if (UserPrincipal.FindByIdentity(ad, lastName) != null)
                {
                    throw new Exception("user id already taken.");
                }

                var user = new UserPrincipal(ad);
                user.Name = string.Format("{0}, {1}", lastName, firstName);
                user.GivenName = firstName;
                user.Surname = lastName;
                
                user.EmployeeId = loginId.ToLower();
                user.Description = title;
                user.EmailAddress = string.IsNullOrEmpty(email) ? string.Format("{0}@caes.ucdavis.edu", lastName.ToLower()) : email;
                
                user.SamAccountName = lastName;
                user.SetPassword("Devel$$123456789");
                user.Save();
            }

            using (var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, container, UserName, Password))
            {
                var user = UserPrincipal.FindByIdentity(ad, lastName);
                user.Enabled = true;
                user.Save();
            }
        }

        private IEnumerable<AdUser> LoadUsers()
        {
            if (Site == null || string.IsNullOrEmpty(Site.UserOu)) {return new List<AdUser>();}

            var ous = Site.UserOu.Split('|');
            var results = new List<AdUser>();

            foreach(var ou in ous)
            {
                using(var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, ou, UserName, Password))
                {
                    var u = new UserPrincipal(ad) {Enabled = true};
                    var searcher = new PrincipalSearcher(u);

                    results.AddRange(searcher.FindAll().Select(a => new AdUser((UserPrincipal)a)));        
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
