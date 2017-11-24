using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace bSide.NMP.RYDEL.App_Code
{
    class Merchant
    {
        private Dictionary<string, string> parms;
        public Merchant()
        {
            parms = SharePointDA.GetParametrosPago();
        }

        public string gatewayHost { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.gatewayHost, parms); } }

        public string gatewayUrl { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.gatewayUrl, parms); } }

        public string proxyHost { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.proxyHost, parms); } }

        public string proxyUser { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.proxyUser, parms); } }

        public string proxyPassword { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.proxyPassword, parms); } }

        public string proxyDomain { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.proxyDomain, parms); } }

        public bool useProxy { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.useProxy, parms).Equals("True", StringComparison.InvariantCultureIgnoreCase); } }

        public bool useSsl { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.useSsl, parms).Equals("True", StringComparison.InvariantCultureIgnoreCase); } }

        public string merchantId { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.merchantId, parms); } }

        public string password { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.password, parms); } }

        public string apiUsername { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.apiUsername, parms); } }

        public string gatewayFormURL { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.gatewayFormUrl, parms); } }

        public string currency { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.currency, parms); } }

        public string payOperation { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.payOperation, parms); } }

        public bool debug_mode { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.debugMode, parms).Equals("True", StringComparison.InvariantCultureIgnoreCase); } }

        public bool IgnoreSslErrors { get { return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.ignoreSslErrors, parms).Equals("True", StringComparison.InvariantCultureIgnoreCase); } }

        public string cancelUrlname { get { return SPContext.Current.Web.Url + SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.cancelUrl, parms); } }

        public string ReturnUrlname { get { return SPContext.Current.Web.Url + SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.returnUrl); } }

        public string GatewayURI
        {
            get
            {
                return SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.gatewayHost) +
                SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.gatewayUrl) +
                SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.version);
            }
        }

        
    }
}
