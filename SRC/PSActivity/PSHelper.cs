using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;

namespace PSActivity
{
    public class PSHelper
    {
        PowerShell _powershell = null;
        Runspace _runspace = null;
        const Int32 _DefaultPort = 5985;
        const String _DefaultAppName = "/wsman";
        const String _ShellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";

        public PSHelper()
        {

        }

        public void CreatePowerShellRunspace(Boolean UseSSL, String Computername, Int32 Port, String AppName, String ShellUri, String UserName, String Password)
        {
            var con = new WSManConnectionInfo(UseSSL, Computername, Port, AppName, ShellUri, new PSCredential(UserName, ConvertStringToSecureString(Password)));

            //Changing authentication for backwards compatibility with Windows Server 2008/R2
            //http://blogs.msdn.com/b/varun_malhotra/archive/2010/06/10/configure-power-shell-for-remote-use-of-sp-2010.aspx
            //con.AuthenticationMechanism = AuthenticationMechanism.Kerberos;

            con.AuthenticationMechanism = AuthenticationMechanism.Credssp;

            _runspace = RunspaceFactory.CreateRunspace(con);

            _runspace.Open();

            if (_powershell == null) _powershell = PowerShell.Create();

            _powershell.Runspace = _runspace;
        }

        public void DisposePowerShellRunspace()
        {
            if(_powershell != null & _powershell.Runspace != null)_powershell.Runspace.Close();
        }

        public void AddCommandToPipeLine(String Command)
        {
            _powershell.AddCommand(Command);
        }

        public void ConvertPipeContentsToString()
        {
            _powershell.AddCommand("Out-String");
        }

        public Collection<PSObject> InvokePipeline()
        {
            return _powershell.Invoke();
        }

        public String ProcessResultsToString(Collection<PSObject> Results)
        {
            StringBuilder bob = new StringBuilder();
            foreach (PSObject obj in Results)
            {
                bob.AppendLine(obj.ToString());
            }

            return bob.ToString();
        }

        public void SetRunSpaceVariable(String Name, Object sender)
        {
            _powershell.AddCommand("Set-Variable");
            _powershell.AddParameter("Name", Name);
            _powershell.AddParameter("Value", sender);
        }

        public void AddScriptToPipeLine(String Script)
        {
            _powershell.AddScript(Script);
        }

        public SecureString ConvertStringToSecureString(String input)
        {
            SecureString output = new SecureString();
            for (int i = 0; i < input.Length; i++)
            {
                output.AppendChar(input[i]);
            }
            output.MakeReadOnly();
            return output;
        }

    }
}
