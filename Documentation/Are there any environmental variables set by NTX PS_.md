# Environmental Variables

The following Environmental Variables are set for each PowerShell session:

**$NTXWorkflowContext**

Instantiates a [Microsoft.SharePoint.WorkflowActions.WorkflowContext](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.workflowactions.workflowcontext(v=office.15).aspx) object within the context of the workflow instance.

**$NTXListID**

Instantiates a [System.Guid](https://msdn.microsoft.com/en-us/library/system.guid%28v=vs.110%29.aspx) object that represents the List ID where the workflow is running (Will return an empty Guid object if this is a site workflow).

**$NTXWorkflowInstanceID**

Instantiates a [System.Guid](https://msdn.microsoft.com/en-us/library/system.guid%28v=vs.110%29.aspx) object that represents the Workflow Instance ID of the Workflow Instance executing NTX PowerShell.

