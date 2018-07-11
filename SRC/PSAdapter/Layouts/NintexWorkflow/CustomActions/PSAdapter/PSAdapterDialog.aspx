<%@ Page Language="C#" DynamicMasterPageFile="~masterurl/default.master" AutoEventWireup="true" CodeBehind="PSAdapterDialog.aspx.cs" EnableEventValidation="false"
    Inherits="PSAdapter.PSAdapterDialog, $SharePoint.Project.AssemblyFullName$"  %>

<%@ Register TagPrefix="Nintex" Namespace="Nintex.Workflow.ServerControls" Assembly="Nintex.Workflow.ServerControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=913f6bae0ca5ae12" %>
<%@ Register Assembly="Nintex.Workflow.ApplicationPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=913f6bae0ca5ae12" Namespace="Nintex.Workflow.ApplicationPages" TagPrefix="Nintex" %>
<%@ Register TagPrefix="Nintex" TagName="ConfigurationPropertySection" src="~/_layouts/15/NintexWorkflow/ConfigurationPropertySection.ascx" %>
<%@ Register TagPrefix="Nintex" TagName="ConfigurationProperty" src="~/_layouts/15/NintexWorkflow/ConfigurationProperty.ascx" %>
<%@ Register TagPrefix="Nintex" TagName="DialogLoad" Src="~/_layouts/15/NintexWorkflow/DialogLoad.ascx" %>
<%@ Register TagPrefix="Nintex" TagName="DialogBody" Src="~/_layouts/15/NintexWorkflow/DialogBody.ascx" %>
<%@ Register TagPrefix="Nintex" TagName="CredentialControl" Src="~/_layouts/15/NintexWorkflow/CredentialControl.ascx" %> 

<%@ Register TagPrefix="Nintex" TagName="SingleLineInput" Src="~/_layouts/15/NintexWorkflow/SingleLineInput.ascx" %>
<%@ Register TagPrefix="Nintex" TagName="PlainTextWebControl" Src="~/_layouts/15/NintexWorkflow/PlainTextWebControl.ascx" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <Nintex:DialogLoad ID="DialogLoad1" runat="server" />

    <script type="text/javascript" language="javascript">
        function TPARetrieveConfig() {
            setPlainTextEditorText('<%=psScript.ClientID%>', configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='PSScript']/PrimitiveValue/@Value").text);
            document.getElementById('<%=resultOutput.ClientID%>').value = configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='ResultOutput']/Variable/@Name").text;

            if (document.getElementById('SSLEnabled'))
                document.getElementById('SSLEnabled').checked = (configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='SSLEnabled']/PrimitiveValue/@Value").text == "false");

            setRTEValue('<%=computerName.ClientID %>', configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='ComputerName']/PrimitiveValue/@Value").text);

            setRTEValue('<%=portNumber.ClientID %>', configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='PortNumber']/PrimitiveValue/@Value").text);

            setRTEValue('<%=appName.ClientID %>', configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='AppName']/PrimitiveValue/@Value").text);

            setRTEValue('<%=shellUri.ClientID %>', configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='ShellUri']/PrimitiveValue/@Value").text);


            cc_setUsername(credentialPicker, configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='LoginUserName']/PrimitiveValue/@Value").text);
            cc_setPassword(credentialPicker, configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='LoginPassword']/PrimitiveValue/@Value").text);
        }

        function TPAWriteConfig() {
            configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='PSScript']/PrimitiveValue/@Value").text = getStringFromPlainTextEditor('<%= psScript.ClientID %>');

            if (document.getElementById('SSLEnabled'))
                configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='SSLEnabled']/PrimitiveValue/@Value").text = (document.getElementById('SSLEnabled').checked ? false : true);

            configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='ComputerName']/PrimitiveValue/@Value").text = getRTEValue('<%=computerName.ClientID %>');

            configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='PortNumber']/PrimitiveValue/@Value").text = getRTEValue('<%=portNumber.ClientID %>');

            configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='AppName']/PrimitiveValue/@Value").text = getRTEValue('<%=appName.ClientID %>');

            configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='ShellUri']/PrimitiveValue/@Value").text = getRTEValue('<%=shellUri.ClientID %>');

            configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='LoginUserName']/PrimitiveValue/@Value").text = cc_getUsername(credentialPicker);
            configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='LoginPassword']/PrimitiveValue/@Value").text = cc_getPassword(credentialPicker);
            var resultOuputCtrl = document.getElementById('<%=resultOutput.ClientID%>');
            if (resultOuputCtrl.value.length > 0) {
                configXml.selectSingleNode("/NWActionConfig/Parameters/Parameter[@Name='ResultOutput']/Variable/@Name").text = resultOuputCtrl.value;
            }

            return true;
        }

        onLoadFunctions[onLoadFunctions.length] = function () {
            dialogSectionsArray["<%= MainControls1.ClientID %>"] = true;
        };
    </script>
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="PlaceHolderMain" runat="Server">

  <Nintex:ConfigurationPropertySection runat="server" Id="MainControls1">
    <TemplateRowsArea>
      
        <Nintex:ConfigurationProperty ID="ConfigurationProperty1" runat="server" FieldTitle="PowerShell Script" RequiredField="True">
            <TemplateControlArea>
                <Nintex:PlainTextWebControl runat="server" id="psScript" Width="100%"/>
            </TemplateControlArea>
        </Nintex:ConfigurationProperty>

        <Nintex:ConfigurationProperty ID="ConfigurationProperty2" runat="server" FieldTitle="SSL Enabled">
        <TemplateControlArea>
          <input type="checkbox" id="SSLEnabled" />
        </TemplateControlArea>
        </Nintex:ConfigurationProperty>
        
        <Nintex:ConfigurationProperty ID="ConfigurationProperty3" runat="server" FieldTitle="Computer Name" RequiredField="True">
        <TemplateControlArea>
            <Nintex:SingleLineInput runat="server" id="computerName"/>
        </TemplateControlArea>
        </Nintex:ConfigurationProperty>

        <Nintex:ConfigurationProperty ID="ConfigurationProperty4" runat="server" FieldTitle="Run Script As" FieldTitleResourceKey="" RequiredField="True">
        <TemplateControlArea>
          <Nintex:CredentialControl DisplayMode="dialog" Width="170px" runat="server" id="credentialPicker"></Nintex:CredentialControl>
        </TemplateControlArea>
      </Nintex:ConfigurationProperty>

        <Nintex:ConfigurationProperty ID="ConfigurationProperty5" runat="server" FieldTitle="Result Output" RequiredField="True">
            <TemplateControlArea>
                <Nintex:VariableSelector id="resultOutput" runat="server" IncludeTextVars="True"></Nintex:VariableSelector>
            </TemplateControlArea>
        </Nintex:ConfigurationProperty>
        
        <Nintex:ConfigurationProperty ID="ConfigurationProperty6" runat="server" FieldTitle="Port Number" RequiredField="True">
        <TemplateControlArea>
            <Nintex:SingleLineInput runat="server" id="portNumber"/>
        </TemplateControlArea>
        </Nintex:ConfigurationProperty>
        
        <Nintex:ConfigurationProperty ID="ConfigurationProperty7" runat="server" FieldTitle="App Name" RequiredField="True">
        <TemplateControlArea>
            <Nintex:SingleLineInput runat="server" id="appName"/>
        </TemplateControlArea>
        </Nintex:ConfigurationProperty>
        
        <Nintex:ConfigurationProperty ID="ConfigurationProperty8" runat="server" FieldTitle="Shell Uri" RequiredField="True">
        <TemplateControlArea>
            <Nintex:SingleLineInput runat="server" id="shellUri"/>
        </TemplateControlArea>
        </Nintex:ConfigurationProperty>


    </TemplateRowsArea>
  </Nintex:ConfigurationPropertySection>

  <Nintex:DialogBody runat="server" id="DialogBody">
  </Nintex:DialogBody>
</asp:Content>
