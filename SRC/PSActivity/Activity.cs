using System;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using Nintex.Workflow;
using Nintex.Workflow.Activities;

namespace PSActivity
{
    public class Activity : ProgressTrackingActivity
    {

        #region Dependencies

        public static DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(SPItemKey), typeof(Activity));

        public static DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(Activity));

        public static DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(Activity));

        public static DependencyProperty PSScriptProperty = DependencyProperty.Register("PSScript", typeof(string), typeof(Activity));

        public static DependencyProperty LoginUserNameProperty = DependencyProperty.Register("LoginUserName", typeof(string), typeof(Activity));

        public static DependencyProperty LoginPasswordProperty = DependencyProperty.Register("LoginPassword", typeof(string), typeof(Activity));

        public static DependencyProperty ResultOutputProperty = DependencyProperty.Register("ResultOutput", typeof(string), typeof(Activity));

        public static DependencyProperty SSLEnabledProperty = DependencyProperty.Register("SSLEnabled", typeof(bool), typeof(Activity));

        public static DependencyProperty ComputerNameProperty = DependencyProperty.Register("ComputerName", typeof(string), typeof(Activity));

        public static DependencyProperty PortNumberProperty = DependencyProperty.Register("PortNumber", typeof(int), typeof(Activity));

        public static DependencyProperty AppNameProperty = DependencyProperty.Register("AppName", typeof(string), typeof(Activity));

        public static DependencyProperty ShellUriProperty = DependencyProperty.Register("ShellUri", typeof(string), typeof(Activity));


        public SPItemKey __ListItem
        {
            get
            {
                return (SPItemKey)base.GetValue(__ListItemProperty);
            }
            set
            {
                SetValue(__ListItemProperty, value);
            }

        }

        public WorkflowContext __Context
        {
            get
            {
                return (WorkflowContext)base.GetValue(__ContextProperty);
            }
            set
            {
                SetValue(__ContextProperty, value);
            }
        }

        public string __ListId
        {
            get
            {
                return (string)base.GetValue(__ListIdProperty);
            }
            set
            {
                SetValue(__ListIdProperty, value);
            }
        }

        public string PSScript
        {
            get
            {
                return (string)base.GetValue(PSScriptProperty);
            }
            set
            {
                SetValue(PSScriptProperty, value);
            }
        }

        public string LoginUserName
        {
            get
            {
                return ((string)base.GetValue(LoginUserNameProperty));
            }
            set
            {
                base.SetValue(LoginUserNameProperty, value);
            }
        }

        public string LoginPassword
        {
            get
            {
                return ((string)(base.GetValue(LoginPasswordProperty)));
            }
            set
            {
                base.SetValue(LoginPasswordProperty, value);
            }
        }

        public string ResultOutput
        {
            get
            {
                return (string)base.GetValue(ResultOutputProperty);
            }
            set
            {
                base.SetValue(ResultOutputProperty, value);
            }
        }

        public bool SSLEnabled
        {
            get
            {
                return ((bool)base.GetValue(SSLEnabledProperty));
            }
            set
            {
                base.SetValue(SSLEnabledProperty, value);
            }
        }

        public string ComputerName
        {
            get
            {
                return (string)base.GetValue(ComputerNameProperty);
            }
            set
            {
                base.SetValue(ComputerNameProperty, value);
            }
        }

        public int PortNumber
        {
            get
            {
                return (int)base.GetValue(PortNumberProperty);
            }
            set
            {
                base.SetValue(PortNumberProperty, value);
            }
        }

        public string AppName
        {
            get
            {
                return (string)base.GetValue(AppNameProperty);
            }
            set
            {
                base.SetValue(AppNameProperty, value);
            }
        }

        public string ShellUri
        {
            get
            {
                return (string)base.GetValue(ShellUriProperty);
            }
            set
            {
                base.SetValue(ShellUriProperty, value);
            }
        }

        #endregion

        const string Cat = "Legacy Workflow Infrastructure";

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            //Check if activity is allowed to run
            ActivityActivationReference.IsAllowed(this, __Context.Web);

            //Resolve tags in string (variables, etc)
            NWWorkflowContext ctx = NWWorkflowContext.GetContext(__Context, new Guid(__ListId), __ListItem.Id, WorkflowInstanceId, this);

            //Activity has begun executing
            base.LogProgressStart(ctx);
            LogHelper.LogInfo(Cat, "NTX PowerShell running at: " + this.__Context.CurrentItemUrl);

            //Determine if a Workflow Constant is being used for authentication
            string runtimeUsername = null;
            string runtimePassword = null;

            if (LoginUserName.Contains("WFConstant") & LoginPassword == "")
            {
                CredentialValue.DetermineRuntimeCredentials(LoginUserName, LoginPassword, out runtimeUsername,
                    out runtimePassword, ctx.Context.Web.ID, ctx.Context.Site.ID);
            }
            else
            {
                runtimeUsername = LoginUserName;
                runtimePassword = LoginPassword;
            }


            String script = ctx.AddContextDataToString(PSScript);

            try
            {
                var ps = new PSHelper();
                LogHelper.LogInfo(Cat, "Executing script as " + runtimeUsername + " on " + ComputerName + AppName + ":" + PortNumber + " SSL Enabled: " + SSLEnabled);
                ps.CreatePowerShellRunspace(SSLEnabled, ComputerName, PortNumber, AppName, ShellUri, runtimeUsername, runtimePassword);
                ps.SetRunSpaceVariable("NTXWorkflowContext", __Context);
                ps.SetRunSpaceVariable("NTXListID", new Guid(__ListId));
                ps.SetRunSpaceVariable("NTXWorkflowInstanceID", new Guid(__ListId));
                LogHelper.LogInfo(Cat, "NTX PowerShell RunSpaceVariables loaded");
                ps.AddScriptToPipeLine(script);
                LogHelper.LogInfo(Cat, "NTX PowerShell Script loaded: " + script);
                ps.ConvertPipeContentsToString();
                ResultOutput = ps.ProcessResultsToString(ps.InvokePipeline());

                //Added to ensure runspaces are closed after execution completes. https://msdn.microsoft.com/en-us/library/system.management.automation.runspaces.runspacefactory(v=vs.85).aspx
                ps.DisposePowerShellRunspace();
            }
            catch (Exception e)
            {
                ResultOutput = e.ToString();
                LogHelper.LogException(Cat, e);
            }

            //activity has stopped executing
            base.LogProgressEnd(ctx, executionContext);
            LogHelper.LogInfo(Cat, "NTX PowerShell completed at: " + __Context.CurrentItemUrl);

            return ActivityExecutionStatus.Closed;
        }

        protected override ActivityExecutionStatus HandleFault(ActivityExecutionContext executionContext, Exception exception)
        {
            Nintex.Workflow.Diagnostics.ActivityErrorHandler.HandleFault(
            executionContext,
            exception,
            WorkflowInstanceId,
            "Error executing PowerShell",
            __ListItem.Id,
            __ListId,
            __Context);

            return base.HandleFault(executionContext, exception);
        }
    }

}
