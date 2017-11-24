using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    [Serializable]
    public class certifica
    {
        public void configuraSSL(string pRutaCertificado, System.Web.Services.Protocols.SoapHttpClientProtocol pCliente)
        {
            string certPath = pRutaCertificado;
            X509Certificate cert = X509Certificate.CreateFromCertFile(certPath);
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            pCliente.ClientCertificates.Add(cert);
        }
    }
}
