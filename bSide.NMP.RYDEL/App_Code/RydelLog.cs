using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bSide.NMP.RYDEL.App_Code
{
    public static class RydelLog
    {
        /// <summary>
        /// Guarda error en bitácora de SharePoint
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="mensaje"></param>
        public static void LogError(Exception ex, string mensaje = "")
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
                    diagSvc.WriteTrace(0, new SPDiagnosticsCategory(Constantes.appName, TraceSeverity.Monitorable, EventSeverity.Error),
                        TraceSeverity.Monitorable,
                        "Ha ocurrido el siguiente error: {0}, {1}, {2}, {3}, {4}",
                        new object[] { mensaje, ex.Message, ex.StackTrace,
                            ex.InnerException != null? ex.InnerException.Message:string.Empty,
                            ex.InnerException != null? ex.InnerException.StackTrace : string.Empty });
                }
            });
        }

        /// <summary>
        /// Guarda mensaje en bitácora de SharePoint
        /// </summary>
        /// <param name="mensaje"></param>
        public static void LogMessage(string mensaje)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
                    diagSvc.WriteTrace(0, new SPDiagnosticsCategory(Constantes.appName, TraceSeverity.Verbose, EventSeverity.Information),
                        TraceSeverity.Verbose,
                        "Mensaje: {0}",
                        new object[] { mensaje });
                }
            });
        }
    }
}
