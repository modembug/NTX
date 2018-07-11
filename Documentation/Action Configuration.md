![](Action Configuration_http://i.imgur.com/TTSvkVe.png)

**PowerShell Script:** The Script to execute remotely.

**SSL Enabled:** Configures whether or not the Windows Remote Management session should be encrypted with SSL.

**Computer Name:** The computer that will execute the PowerShell Script.

**Run Script As:** The account the PowerShell Script should run as on the machine specified in **Computer Name**. (Note: By Default only Local Administrators can do this)

**Result Output:** Stores the output of the PowerShell Session.

**Port Number:** The port Windows Remote Management is listening on the machine specified in **Computer Name.** 

**Note: Do not change this value unless you are sure WinRM is listening on a different port.**

**App Name:** See [WSMan](https://msdn.microsoft.com/en-us/library/aa384538%28v=vs.85%29.aspx)

**Shell Uri:** Specifies the resource URI for the shell operation. The resource URI can be used to retrieve plug-in configuration that is specific to the shell instance.



