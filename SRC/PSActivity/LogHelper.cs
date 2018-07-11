using System;
using Microsoft.SharePoint.Administration;

namespace PSActivity
{
    public static class LogHelper
    {

        public static void LogInfo(String category, String data)
        {
            const TraceSeverity tSeverity = TraceSeverity.Verbose;
            const EventSeverity eSeverity = EventSeverity.Verbose;
            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(category, tSeverity, eSeverity), tSeverity, "Data:  {0}", new object[] { data });
        }

        public static void LogException(String category, Exception exception)
        {
            const TraceSeverity tSeverity = TraceSeverity.Unexpected;
            const EventSeverity eSeverity = EventSeverity.Error;
            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(category, tSeverity, eSeverity), tSeverity, "Error Data:  {0}", new object[] { exception.ToString() });
        }

        public static void LogCustom(String category, TraceSeverity tSeverity, EventSeverity eSeverity, object data)
        {
            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(category, tSeverity, eSeverity), tSeverity, "Data:  {0}", new object[] { data.ToString() });
        }
    }
}
