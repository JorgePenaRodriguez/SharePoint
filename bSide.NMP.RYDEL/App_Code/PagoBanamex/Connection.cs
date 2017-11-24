using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bSide.NMP.RYDEL.App_Code
{
    class Connection
    {
         private Merchant _merchant;

        public Connection(Merchant merchant)
        {
            this._merchant = merchant;
        }


        private string GetURL()
        {
            return "https://" + _merchant.gatewayHost + _merchant.gatewayUrl;
        }


        // callback utilizado para validar el certificado en una conversación SSL
        private bool ValidateRemoteCertificate(
            object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors policyErrors
        )
        {
            if (Convert.ToBoolean(_merchant.IgnoreSslErrors))        
            {
                // permitir cualquier viejo certificado ...
                return true;
            }
            else
            {
                return policyErrors == System.Net.Security.SslPolicyErrors.None;
            }
        }


        /// <summary>
        /// Toma un NameValueCollection y regresa una cadena de campos y valores del API delimitados por & y url encoded
        /// </summary>

        public string sendTransaction(string data)
        {
            Dictionary<string, string> responseFields = new Dictionary<string, string>();

            string body = String.Empty;

            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateRemoteCertificate);

            if (_merchant.useProxy)
            {
                WebProxy proxy = new WebProxy(_merchant.proxyHost, true);
                if (!String.IsNullOrEmpty(_merchant.proxyUser))
                {
                    if (String.IsNullOrEmpty(_merchant.proxyDomain))
                    {
                        proxy.Credentials = new NetworkCredential(_merchant.proxyUser, _merchant.proxyPassword);
                    }
                    else
                    {
                        proxy.Credentials = new NetworkCredential(_merchant.proxyUser, _merchant.proxyPassword, _merchant.proxyDomain);
                    }
                }
                WebRequest.DefaultWebProxy = proxy;
            }
            // Crear la petición web
            HttpWebRequest request = WebRequest.Create(_merchant.GatewayURI) as HttpWebRequest;


            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

            //request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
            //request.Credentials = new NetworkCredential("", apiPassword);
            //request.PreAuthenticate = true;

            string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("" + ":" + _merchant.password));
            request.Headers.Add("Authorization", "Basic " + credentials);

            // Crear un array de byte de los datos que queremos enviar
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data);

            // Ajuste la longitud del contenido en los request headers
            request.ContentLength = byteData.Length;


            // Escriba los datos
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            // Obtenga la respuesta
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Obtenga la secuencia de respuesta
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    body = reader.ReadToEnd();
                }
            }
            catch (WebException wex)
            {
                try
                {
                    StreamReader reader = new StreamReader(wex.Response.GetResponseStream());
                    body = reader.ReadToEnd();
										Debug.WriteLine(body);
                }
                catch
                {
									body = wex.Message;
									Debug.WriteLine(body);
                }
            }
            return body;
        }
    }
}
