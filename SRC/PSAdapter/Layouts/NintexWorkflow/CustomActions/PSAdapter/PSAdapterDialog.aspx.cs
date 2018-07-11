using System;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Nintex.Workflow.ApplicationPages;

namespace PSAdapter
{
    public partial class PSAdapterDialog : Nintex.Workflow.ServerControls.NintexLayoutsBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptFiles.RegisterControlsForJS(this, new Control[] { credentialPicker });

        }

    }
}
