using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;

namespace CAM.Services
{
    public interface ILyncService
    {
        void Initialize(string username, string password, string uri);
        void EnableLync(string samName);
    }

    public class LyncService : ILyncService
    {
        private const string _schema = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
        
        private string _uri;
        private string _username;
        private string _password;


        public void Initialize(string username, string password, string uri)
        {
            _username = username;
            _password = password;
            _uri = uri;
        }

        public void EnableLync(string samName)
        {
            var password = new SecureString();
            foreach(var c in _password) password.AppendChar(c);

            var credentials = new PSCredential(_username, password);
            var rri = new WSManConnectionInfo(new Uri(_uri), _schema, credentials);
            rri.AuthenticationMechanism = AuthenticationMechanism.Negotiate;

            var remoteRunspace = RunspaceFactory.CreateRunspace(rri);
            remoteRunspace.Open();

            using (var powershell = PowerShell.Create())
            {
                powershell.Runspace = remoteRunspace;
                powershell.AddCommand("Enable-CsUser");

                powershell.AddParameter("Identity", samName);
                powershell.AddParameter("RegistrarPool", "lync.caesdo.caes.ucdavis.edu");
                powershell.AddParameter("SipAddressType", "SamAccountName");
                powershell.AddParameter("SipDomain", "caesdo.caes.ucdavis.edu");

                powershell.Invoke();
            }
            
            remoteRunspace.Close();
        }
    }
}