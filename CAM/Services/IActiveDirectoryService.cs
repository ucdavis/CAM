using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace CAM.Services
{
    public interface IActiveDirectoryService
    {
        //List<string> ReadAllUsers();
        //List<string> ReadAllGroups();
        //void ReadAllDistributionLists();

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

    }

    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly string _server = ConfigurationManager.AppSettings["adserver"];
        private readonly string _aduser = ConfigurationManager.AppSettings["aduser"];
        private readonly string _adpass = ConfigurationManager.AppSettings["adpass"];
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// http://social.msdn.microsoft.com/Forums/en-IE/csharpgeneral/thread/e8855f8a-3c0f-4744-9513-dbc1322c267c
        /// </remarks>
        /// <returns></returns>
        //private string GetLdap()
        //{
        //    var context = new DirectoryContext(DirectoryContextType.Domain, _server, _aduser, _adpass);
        //    var domain = Domain.GetDomain(context);

        //    foreach (DomainController dc in domain.DomainControllers)
        //    {
        //        if (!string.IsNullOrEmpty(dc.Name))
        //        {
        //            return string.Format("LDAP://{0}/CN=Users/DC={1},DC={2}", dc.Name, _server.Substring(0, _server.Length - 4), dc.Name.Substring(dc.Name.Length - 3, 3));
        //        }
        //    }

        //    return null;
        //}

        //public List<string> ReadAllUsers()
        //{
        //    var de = new DirectoryEntry("LDAP://philip.caesdo.caes.ucdavis.edu/DC=caesdo,DC=caes,DC=ucdavis,DC=edu", _aduser, _adpass);
        //    var ds = new DirectorySearcher(de);
        //    //ds.Filter = "(objectClass=user)";
        //    ds.Filter = "(objectCategory=person)";

        //    //ds.PropertiesToLoad.Clear();
        //    //ds.PropertiesToLoad.Add("name");                // name
        //    //ds.PropertiesToLoad.Add("distinguishedName");   // ldap, key?
        //    //ds.PropertiesToLoad.Add("cn");                    // login id

        //    var results = new List<string>();

        //    foreach (SearchResult result in ds.FindAll())
        //    {
        //        try
        //        {
        //            var test = string.Format("{0}({1})",result.Properties["name"][0].ToString(), result.Properties["cn"][0]);
                    
        //            results.Add(test);
        //        }
        //        catch
        //        {
        //        }

        //    }

        //    return results;
        //}

        //public List<string> ReadAllGroups()
        //{

        //    var de = new DirectoryEntry("LDAP://philip.caesdo.caes.ucdavis.edu/DC=caesdo,DC=caes,DC=ucdavis,DC=edu", _aduser, _adpass);
        //    var ds = new DirectorySearcher(de);
        //    ds.Filter = "(objectClass=group)";
        //    var results = new List<string>();

        //    foreach (SearchResult result in ds.FindAll())
        //    {
        //        try
        //        {
        //            var test = result.Properties["objectClass"][0];
        //            results.Add(test.ToString());
        //        }
        //        catch
        //        {
        //        }

        //    }

        //    return results;
            
        //}

        //public void ReadAllDistributionLists()
        //{
        //    throw new NotImplementedException();
        //}

        public List<AdGroup> GetSecurityGroups()
        {
            var results = LoadGroups(true);

            return results.Select(a => new AdGroup(a.SamAccountName, a.Name, a.Description)).ToList();
        }

        public List<AdGroup> GetDistributionLists()
        {
            var results = LoadGroups(false);

            return results.Select(a => new AdGroup(a.SamAccountName, a.Name, a.Description)).ToList();
        }

        private IEnumerable<GroupPrincipal> LoadGroups(bool security)
        {
            var ad = new PrincipalContext(ContextType.Domain, _server, "OU=DEPARTMENTS,DC=caesdo,DC=caes,DC=ucdavis,DC=edu", _aduser, _adpass);

            var g = new GroupPrincipal(ad) {IsSecurityGroup = security};
            var searcher = new PrincipalSearcher(g);

            var results = searcher.FindAll();

            return results.Select(a => (GroupPrincipal) a).ToList();
        }
    } 

    public class AdGroup
    {
        public AdGroup(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
