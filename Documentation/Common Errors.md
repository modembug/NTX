**Microsoft.SharePoint.SPException: <Error><CompilerError Line="0" Column="10" Text="The type or namespace name 'PSActivity' could not be found (are you missing a using directive or an assembly reference?)" />**

This error indicates that the web.config file for the Web Application executing NTX PowerShell does not have the required Authorized Type intalled. See this article for more information: [Authorized Assemblies](Authorized-Assemblies)

**"Add-SPSolution : Access denied." when running the installer.**

This error indicates that the PowerShell session does not have permissions to write to the GAC (Global Assembly Cache). Re-launch the PowerShell session as Administrator to correct the issue.