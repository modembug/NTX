# Reproduce the issue in WinRM

To help isolate whether or not this is an issue with the action or a WinRM configuration you can emulate the behavior of the action payload by connecting to the remote machine from one of the SharePoint Servers running the SharePoint Workflow Timer Service. To accomplish this do the following:

# Connect to one of the SharePoint Servers running the SharePoint Workflow Timer Service.
# Open a Windows PowerShell Prompt.
# Execute "$credentials = Get-Credential -Credential domain\username
#Execute "Enter-PSSession -ComputerName targetComputerName -Credential $credentials

This will open a remote session to the target server in the same way NTX PowerShell does.

Once you have the remote PowerShell session open; copy and paste your script into the window and run it.

