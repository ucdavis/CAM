using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
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
        AdUser GetUserByEmployeeId(string employeeId);

        void AssignEmployeeId(string userId, string employeeId);
        void CreateUser(AdUser adUser, string container, List<string> securityGroups );
        void AssignUserToGroup(string userId, string groupId);
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
                    var test = u.Manager();
                    if (u != null) return new AdUser(u);
                }
            }

            return null;
        }

        public AdUser GetUserByEmployeeId(string employeeId)
        {
            if (Site == null || string.IsNullOrEmpty(Site.UserOu)) { return null; }

            var ous = Site.UserOu.Split('|');

            foreach (var ou in ous)
            {
                using (var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, ou, UserName, Password))
                {
                    var user = new UserPrincipal(ad) {EmployeeId = employeeId};
                    var searcher = new PrincipalSearcher(user);

                    var result = searcher.FindOne();
                    if (result  !=  null)
                    {
                        return new AdUser((UserPrincipal)result);
                    }
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

        public void CreateUser(AdUser adUser, string container, List<string> securityGroups)
        {
            string loginId, manager = string.Empty;

            // find the supervisor
            if (!string.IsNullOrEmpty(adUser.ManagerKerb))
            {
                var supervisor = GetUserByEmployeeId(adUser.ManagerKerb);
                if (supervisor != null) manager = supervisor.DistinguishedName;

            }

            using (var upc = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, container, UserName, Password))
            {
                loginId = CheckForExistingUser(adUser.FirstName, adUser.LastName, upc);

                if (loginId == null)
                {
                    throw new DuplicateNameException("Unable to determine a valid userid for the requested user.");
                }

                var user = new UserPrincipal(upc);
                AutoMapper.Mapper.Map(adUser, user);

                user.SamAccountName = loginId;
                if (adUser.LastName.ToLower() != loginId)
                {
                    user.Name = string.Format("{0}, {1} ({2})", adUser.LastName, adUser.FirstName, loginId);
                }

                user.SetPassword(GeneratePassword(16));
                
                if (adUser.NeedsEmail)
                {
                    user.EmailAddress = string.Format("{0}@caes.ucdavis.edu", loginId);
                }

                user.Save();

                foreach (var groupId in securityGroups)
                {
                    AddToGroup(user, groupId);
                }
            }

            //// create exchange mailbox if needed

            using (var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, container, UserName, Password))
            {
                var user = UserPrincipal.FindByIdentity(ad, loginId);

                // set the extended properties that cannot be done before first save
                user.OfficeLocation(adUser.OfficeLocation);
                user.Manager(manager);

                // disable the account by default
                user.Enabled = false;
                user.Save();
            }
        }

        public void AssignUserToGroup(string userId, string groupId)
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
                        AddToGroup(u, groupId);
                    }
                }
            }

        }

        /// <summary>
        /// Find a login id we can use
        /// </summary>
        /// <remarks>
        /// example John Smith
        /// lastname
        /// jsmith
        /// josmith
        /// johsmith
        /// </remarks>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        private string CheckForExistingUser(string firstname, string lastname, PrincipalContext pc)
        {
            var up = new UserPrincipal(pc);
            var searcher = new PrincipalSearcher(up);

            // first try the last name
            up.SamAccountName = lastname;
            up.Name = string.Format("{0}, {1}", lastname, firstname);

            if (!searcher.FindAll().Any())
            {
                return lastname.ToLower();
            }

            // start trying combo's with the first name
            for (int i = 1; i <= firstname.Length; i++)
            {
                var login = string.Format("{0}{1}", firstname.Substring(0, i), lastname).ToLower();
                up.SamAccountName = login;
                up.Name = string.Format("{0}, {1} ({2})", lastname, firstname, login);

                if (!searcher.FindAll().Any())
                {
                    return login;
                }
            }

            /*
            // first just try the last name
            if (UserPrincipal.FindByIdentity(pc, lastname) == null)
            {
                return lastname.ToLower();
            }

            // start trying combo's with the first name
            for (int i = 1; i <= firstname.Length; i++ )
            {
                var login = string.Format("{0}{1}", firstname.Substring(0, i), lastname).ToLower();
                if (UserPrincipal.FindByIdentity(pc, login) == null)
                {
                    return login;
                }
            }
            */

            return null;
        }

        private void AddToGroup(UserPrincipal user, string groupId)
        {
            if (Site == null || string.IsNullOrEmpty(Site.UserOu)) { return; }

            var ous = Site.SecurityGroupOu.Split('|');

            foreach (var ou in ous)
            {
                using (var ad = new PrincipalContext(ContextType.Domain, Site.ActiveDirectoryServer, ou, UserName, Password))
                {
                    var group = GroupPrincipal.FindByIdentity(ad, groupId);

                    if (user != null && group != null)
                    {
                        if (!group.Members.Contains(user))
                        {
                            group.Members.Add(user);
                            group.Save();

                            return;
                        }
                    }
                }
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

        private readonly Random _rng = new Random();
        private const string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ?><:[]!@#$%^&*()=+;";
        private string GeneratePassword(int size)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
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
        public AdUser()
        {
            
        }

        public AdUser(UserPrincipal userPrincipal)
        {
            AutoMapper.Mapper.Map(userPrincipal, this);
            OfficeLocation = userPrincipal.OfficeLocation();
            Manager = userPrincipal.Manager();
        }

        /// <summary>
        /// SAM account name
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Full ldap query string to the user
        /// </summary>
        public string DistinguishedName { get; set; }
        /// <summary>
        /// Employee ID field from AD, we're using kerb ids
        /// </summary>
        public string EmployeeId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Unit + Title
        /// </summary>
        public string Description { get; set; }
        public string Email { get; set; }

        public string HomeDrive { get; set; }
        public string HomeDirectory { get; set; }

        public string Phone { get; set; }
        public string OfficeLocation { get; set; }
        public string Manager { get; set; }

        /// <summary>
        /// Account expiration
        /// </summary>
        public DateTime? Expiration { get; set; }
        public bool? Enabled { get; set; }

        public string GetContainer()
        {
            var startIndex = DistinguishedName.IndexOf("OU=") + 3;
            var endIndex = DistinguishedName.IndexOf(',', startIndex);

            return DistinguishedName.Substring(startIndex, endIndex - startIndex);
        }
        public string GetContainerPath()
        {
            var startIndex = DistinguishedName.IndexOf("OU=");
            var endIndex = DistinguishedName.Length;

            return DistinguishedName.Substring(startIndex, endIndex - startIndex);
        }

        // === Support fields
        public string PositionTitle { get; set; }
        public string Unit { get; set; }
        public bool NeedsEmail { get; set; }
        public string ManagerKerb { get; set; }
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

    public static class AccountManagementExtensions
    {
        public static String GetProperty(this Principal principal, String property)
        {
            var directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
            if (directoryEntry.Properties.Contains(property))
                return directoryEntry.Properties[property].Value.ToString();
            
            return String.Empty;
        }

        public static void SetProperty(this Principal principal, string property, string value)
        {
            var directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
            directoryEntry.Properties[property].Value = value;
        }

        private const string _officeLocation = "physicalDeliveryOfficeName";
        private const string _company = "company";
        private const string _department = "department";
        private const string _manager = "manager";

        private static string ProcessProperty(Principal principal, string property, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                principal.SetProperty(property, value);
            }

            return principal.GetProperty(property);
        }

        public static string OfficeLocation(this Principal principal, string value = null)
        {
            return ProcessProperty(principal, _officeLocation, value);
        }

        public static string Manager(this Principal principal, string value = null)
        {
            return ProcessProperty(principal, _manager, value);
        }

        public static String GetCompany(this Principal principal, string value = null)
        {
            return ProcessProperty(principal, _company, value);
        }

        public static String GetDepartment(this Principal principal, string value = null)
        {
            return ProcessProperty(principal, _department, value);
        }
    }
}
