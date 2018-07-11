# Install-NTXPowerShellActivity.Ps1 File Contents

{"
Add-PSSnapin Microsoft.SharePoint.PowerShell -ErrorAction SilentlyContinue

Add-SPSolution NTX.PowerShell.wsp
Install-SPSolution NTX.PowerShell.wsp -GACDeployment

[void](void)[System.Reflection.Assembly](System.Reflection.Assembly)::LoadWithPartialName("Microsoft.SharePoint")
[void](void)[System.Reflection.Assembly](System.Reflection.Assembly)::LoadWithPartialName("Nintex.Workflow")
[void](void)[System.Reflection.Assembly](System.Reflection.Assembly)::LoadWithPartialName("Nintex.Workflow.Administration")
[void](void)[System.Reflection.Assembly](System.Reflection.Assembly)::LoadWithPartialName("System.Xml.XmlDocument")


function Get-NtxPowerShellNwa(){

return "<NintexWorkflowActivity>
	<Name>PowerShell</Name>
	<Category>Operations</Category>
	<Description>Run PowerShell Script</Description>
	<ActivityType>NTX.PowerShell.Activity.PSActivity</ActivityType>
	<ActivityAssembly>NTX.PowerShell.Activity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b5ffc9fcd41d5fae</ActivityAssembly>
	<AdapterType>NTX.PowerShell.Adapter.PSAdapter</AdapterType>
	<AdapterAssembly>NTX.PowerShell.Adapter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b5ffc9fcd41d5fae</AdapterAssembly>
	<HandlerUrl>ActivityServer.ashx</HandlerUrl>
	<Icon>/_layouts/NintexWorkflow/CustomActions/NTX.PowerShell.Adapter/Images/NTX.PowerShell.Large.png</Icon>
	<ToolboxIcon>/_layouts/NintexWorkflow/CustomActions/NTX.PowerShell.Adapter/Images/NTX.PowerShell.Small.png</ToolboxIcon>
	<ConfigurationDialogUrl>CustomActions/NTX.PowerShell.Adapter/NTXPowerShellAdapterDialog.aspx</ConfigurationDialogUrl>
	<ShowInCommonActions>yes</ShowInCommonActions>
	<DocumentLibrariesOnly>no</DocumentLibrariesOnly>
</NintexWorkflowActivity>"
}

$nwaXml = New-Object -TypeName System.Xml.XmlDocument

$nwaXml.LoadXml($(Get-NtxPowerShellNwa))

$activityReference = [Nintex.Workflow.ActivityReference](Nintex.Workflow.ActivityReference)::ReadFromNWA($nwaXml)

[Void](Void)[Nintex.Workflow.ActivityReferenceCollection](Nintex.Workflow.ActivityReferenceCollection)::AddActivity($activityReference.Name, $activityReference.Description, $activityReference.Category, $activityReference.ActivityAssembly, $activityReference.ActivityType, $activityReference.AdapterAssembly, $activityReference.AdapterType, $activityReference.HandlerUrl, $activityReference.ConfigPage, $activityReference.RenderBehaviour, $activityReference.Icon, $activityReference.ToolboxIcon, $activityReference.WarningIcon, $activityReference.QuickAccess, $activityReference.ListTypeFilter, @(), [int](int)"1033")
[Void](Void)[Nintex.Workflow.ActivityReferenceCollection](Nintex.Workflow.ActivityReferenceCollection)::RefreshActivityCache()

$installedActivityReference = [Nintex.Workflow.ActivityReferenceCollection](Nintex.Workflow.ActivityReferenceCollection)::FindByAdapter($activityReference.AdapterType, $activityReference.AdapterAssembly)

$activationReference = New-Object Nintex.Workflow.ActivityActivationReference -ArgumentList @(,$installedActivityReference.ActivityId, [Guid](Guid)(Guid)::Empty, [Guid](Guid)(Guid)::Empty)

$activationReference.AddOrUpdateActivationReference()

"}