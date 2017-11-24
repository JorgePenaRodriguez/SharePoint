using bSide.NMP.RYDEL.App_Code;
using Microsoft.SharePoint;
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace bSide.NMP.RYDEL.WebParts.HorarioOperacion
{
    public partial class HorarioOperacionUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SPContext.Current.Web.CurrentUser == null && !SharePointDA.IsHorarioOperacion())
            {
                string url = SPContext.Current.Web.Url + "/" + SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.urlRedireccionHorario);
                try
                {
                    Response.Redirect(url);
                }
                catch (Exception ex)
                {
                    RydelLog.LogError(ex, string.Format("No se puede redireccionar, url inválida: {0} ", url));
                }
            }
        }
    }
}
