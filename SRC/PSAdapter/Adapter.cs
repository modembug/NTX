using System.Collections.Generic;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint;
using Nintex.Workflow;
using Nintex.Workflow.Activities.Adapters;
using Activity = PSActivity.Activity;

namespace PSAdapter
{
    public class Adapter : GenericRenderingAction
    {
        private const string PSScriptProperty = "PSScript";
        private const string ResultOutputProperty = "ResultOutput";
        private const string LoginUserNameProperty = "LoginUserName";
        private const string LoginPasswordProperty = "LoginPassword";
        private const string SSLEnabledProperty = "SSLEnabled";
        private const string ComputerNameProperty = "ComputerName";
        private const string PortNumberProperty = "PortNumber";
        private const string AppNameProperty = "AppName";
        private const string ShellUriProperty = "ShellUri";

        public override NWActionConfig GetDefaultConfig(GetDefaultConfigContext context)
        {
            // Build the default parameters for the action. Populate an array of ActivityParameters to represent each parameter. 

            NWActionConfig config = new NWActionConfig(this);

            config.Parameters = new ActivityParameter[9];

            config.Parameters[0] = new ActivityParameter();
            config.Parameters[0].Name = PSScriptProperty;
            config.Parameters[0].PrimitiveValue = new PrimitiveValue();
            config.Parameters[0].PrimitiveValue.Value = string.Empty;
            config.Parameters[0].PrimitiveValue.ValueType = SPFieldType.Text.ToString();

            config.Parameters[1] = new ActivityParameter();
            config.Parameters[1].Name = LoginUserNameProperty;
            config.Parameters[1].PrimitiveValue = new PrimitiveValue();
            config.Parameters[1].PrimitiveValue.Value = string.Empty;

            config.Parameters[2] = new ActivityParameter();
            config.Parameters[2].Name = LoginPasswordProperty;
            config.Parameters[2].PrimitiveValue = new PrimitiveValue();
            config.Parameters[2].PrimitiveValue.Value = string.Empty;

            config.Parameters[3] = new ActivityParameter();
            config.Parameters[3].Name = ResultOutputProperty;
            config.Parameters[3].Variable = new NWWorkflowVariable();

            config.Parameters[4] = new ActivityParameter();
            config.Parameters[4].Name = SSLEnabledProperty;
            config.Parameters[4].PrimitiveValue = new PrimitiveValue();
            config.Parameters[4].PrimitiveValue.Value = false.ToString();
            config.Parameters[4].PrimitiveValue.ValueType = SPFieldType.Boolean.ToString();

            config.Parameters[5] = new ActivityParameter();
            config.Parameters[5].Name = ComputerNameProperty;
            config.Parameters[5].PrimitiveValue = new PrimitiveValue();
            config.Parameters[5].PrimitiveValue.Value = string.Empty;
            config.Parameters[5].PrimitiveValue.ValueType = SPFieldType.Text.ToString();

            config.Parameters[6] = new ActivityParameter();
            config.Parameters[6].Name = PortNumberProperty;
            config.Parameters[6].PrimitiveValue = new PrimitiveValue();
            config.Parameters[6].PrimitiveValue.Value = "5985";
            config.Parameters[6].PrimitiveValue.ValueType = SPFieldType.Integer.ToString();

            config.Parameters[7] = new ActivityParameter();
            config.Parameters[7].Name = AppNameProperty;
            config.Parameters[7].PrimitiveValue = new PrimitiveValue();
            config.Parameters[7].PrimitiveValue.Value = "/wsman";
            config.Parameters[7].PrimitiveValue.ValueType = SPFieldType.Text.ToString();

            config.Parameters[8] = new ActivityParameter();
            config.Parameters[8].Name = ShellUriProperty;
            config.Parameters[8].PrimitiveValue = new PrimitiveValue();
            config.Parameters[8].PrimitiveValue.Value = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
            config.Parameters[8].PrimitiveValue.ValueType = SPFieldType.Text.ToString();


            // Set the default label for the action.
            config.TLabel = ActivityReferenceCollection.FindByAdapter(this).Name;
            return config;
        }

        public override NWActionConfig GetConfig(RetrieveConfigContext context)
        {
            // Read the properties from context.Activity and update the values in the NWActionConfig.

            NWActionConfig config = this.GetDefaultConfig(context);


            Dictionary<string, ActivityParameterHelper> parameters = config.GetParameterHelpers();

            parameters[PSScriptProperty].RetrieveValue(context.Activity, Activity.PSScriptProperty, context);
            parameters[ResultOutputProperty].RetrieveValue(context.Activity, Activity.ResultOutputProperty, context);
            parameters[LoginUserNameProperty].RetrieveValue(context.Activity, Activity.LoginUserNameProperty, context);
            parameters[LoginPasswordProperty].RetrieveValue(context.Activity, Activity.LoginPasswordProperty, context);
            parameters[SSLEnabledProperty].RetrieveValue(context.Activity, Activity.SSLEnabledProperty, context);
            parameters[ComputerNameProperty].RetrieveValue(context.Activity, Activity.ComputerNameProperty, context);
            parameters[PortNumberProperty].RetrieveValue(context.Activity, Activity.PortNumberProperty, context);
            parameters[AppNameProperty].RetrieveValue(context.Activity, Activity.AppNameProperty, context);
            parameters[ShellUriProperty].RetrieveValue(context.Activity, Activity.ShellUriProperty, context);

            return config;
        }

        public override bool ValidateConfig(ActivityContext context)
        {
            // Add logic to validate the configuration here.

            bool isValid = true;
            string containerPath = string.Empty;
            string adLoginUserName = string.Empty;
            string adLoginPassword = string.Empty;

            Dictionary<string, ActivityParameterHelper> parameters = context.Configuration.GetParameterHelpers();


            if (!parameters[PSScriptProperty].Validate(typeof(string), context))
            {
                isValid &= false;
                validationSummary.AddError("PSScript", ValidationSummaryErrorType.CannotBeBlank);
            }

            if (!parameters[ResultOutputProperty].Validate(typeof(string), context))
            {
                isValid &= false;
                validationSummary.AddError("ResultOutput", ValidationSummaryErrorType.CannotBeBlank);
            }

            if (!parameters[LoginUserNameProperty].Validate(typeof(string), context))
            {
                isValid &= false;
                validationSummary.AddError(string.Empty, ValidationSummaryErrorType.RequiredCredential);
            }
            else
            {
                adLoginUserName = parameters[LoginUserNameProperty].Value;

                if (!parameters[LoginPasswordProperty].Validate(typeof(string), context))
                {
                    if (!parameters[LoginUserNameProperty].Value.Contains("WFConstant"))
                    {
                        isValid &= false;
                        validationSummary.AddError(string.Empty, ValidationSummaryErrorType.RequiredCredential);
                    }
                }
                else
                {
                    adLoginPassword = parameters[LoginPasswordProperty].Value;
                }
            }

            return isValid;
        }

        public override CompositeActivity AddActivityToWorkflow(PublishContext context)
        {
            // Create an instance of the Activity and set its properties based on config. Add it to the parent activity.

            Dictionary<string, ActivityParameterHelper> parameters = context.Config.GetParameterHelpers();

            Activity activity = new Activity();

            //Assign values from the configuration.
            parameters[PSScriptProperty].AssignTo(activity, Activity.PSScriptProperty, context);
            parameters[ResultOutputProperty].AssignTo(activity, Activity.ResultOutputProperty, context);
            parameters[LoginUserNameProperty].AssignTo(activity, Activity.LoginUserNameProperty, context);
            parameters[LoginPasswordProperty].AssignTo(activity, Activity.LoginPasswordProperty, context);
            parameters[SSLEnabledProperty].AssignTo(activity, Activity.SSLEnabledProperty, context);
            parameters[ComputerNameProperty].AssignTo(activity, Activity.ComputerNameProperty, context);
            parameters[PortNumberProperty].AssignTo(activity, Activity.PortNumberProperty, context);
            parameters[AppNameProperty].AssignTo(activity, Activity.AppNameProperty, context);
            parameters[ShellUriProperty].AssignTo(activity, Activity.ShellUriProperty, context);

            // Set standard context items.
            activity.SetBinding(Activity.__ContextProperty, new ActivityBind(context.ParentWorkflow.Name, StandardWorkflowDataItems.__context));
            activity.SetBinding(Activity.__ListItemProperty, new ActivityBind(context.ParentWorkflow.Name, StandardWorkflowDataItems.__item));
            activity.SetBinding(Activity.__ListIdProperty, new ActivityBind(context.ParentWorkflow.Name, StandardWorkflowDataItems.__list));

            ActivityFlags f = new ActivityFlags();
            f.AddLabelsFromConfig(context.Config);
            f.AssignTo(activity);

            context.ParentActivity.Activities.Add(activity);
            return null;
        }

        public override ActionSummary BuildSummary(ActivityContext context)
        {
            // Construct an ActionSummary class to display details about this action.

            Dictionary<string, ActivityParameterHelper> parameters = context.Configuration.GetParameterHelpers();

            string displayMessage = string.Format("Execute PowerShell Script: '{0}'.", parameters[PSScriptProperty].Value);

            return new ActionSummary(displayMessage);
        }

    }
}
