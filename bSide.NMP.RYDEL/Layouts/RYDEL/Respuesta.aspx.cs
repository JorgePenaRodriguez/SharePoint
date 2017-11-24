using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;
using System.Web;
using bSide.NMP.RYDEL.App_Code;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace bSide.NMP.RYDEL.Layouts.bSide.NMP.RYDEL
{
    public partial class Respuesta : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string merchantId = Session["merchantID"].ToString();
            string orderId = Session["order.id"].ToString();
            string sessionId = Session["formSession"].ToString();
            string transactionId = GetTransactionId(merchantId, sessionId);

            //int tran = 1;
            //int.TryParse(transactionId, out tran);
            //tran = tran > 1? tran -1: tran;
            //transactionId = tran.ToString();

            string GatewayCode = GetResponseGatewayCode(merchantId, orderId, transactionId);

            LblRespuesta.Text = GatewayCode;            
        }

        private string GetResponseGatewayCode(string merchantId, string orderId, string transactionId)
        {
            string gatewayCode = string.Empty;
            string endPoint = string.Format("https://secure.na.tnspayments.com/api/rest/version/32/merchant/{0}/order/{1}/transaction/{2}", merchantId, orderId, transactionId);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(endPoint);

                    // Agrega header formato JSON
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    var byteArray = Encoding.ASCII.GetBytes("merchant.TESTVENTA_TEST:08594fad7c0fce7d47ee565963186d00");
                    var header = new AuthenticationHeaderValue(
                               "Basic", Convert.ToBase64String(byteArray));
                    client.DefaultRequestHeaders.Authorization = header;

                    // Respuesta
                    HttpResponseMessage response = client.GetAsync("").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        LblRespuesta.Text = string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                        var responseValue = string.Empty;

                        if (response != null)
                        {
                            Task task = response.Content.ReadAsStreamAsync().ContinueWith(t =>
                            {
                                var stream = t.Result;
                                using (var reader = new StreamReader(stream))
                                {
                                    responseValue = reader.ReadToEnd();
                                }
                            });

                            task.Wait();
                        }
                        JObject o = JObject.Parse(responseValue);
                        gatewayCode = (string)o["response"]["gatewayCode"];
                    }
                    else
                    {
                        throw new Exception("No se pudo obtener gatewayCode " + response.StatusCode + " " + response.RequestMessage);
                    }
                }
            });

            return gatewayCode;
        }

        private string GetTransactionId(string merchantId, string sessionId)
        {
            string transactionId = string.Empty;
            string endPoint = string.Format("https://secure.na.tnspayments.com/api/rest/version/32/merchant/{0}/session/{1}", merchantId, sessionId);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(endPoint);

                    // Agrega header formato JSON
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    var byteArray = Encoding.ASCII.GetBytes("merchant.TESTVENTA_TEST:08594fad7c0fce7d47ee565963186d00");
                    var header = new AuthenticationHeaderValue(
                               "Basic", Convert.ToBase64String(byteArray));
                    client.DefaultRequestHeaders.Authorization = header;

                    // Respuesta
                    HttpResponseMessage response = client.GetAsync("").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        LblRespuesta.Text = string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                        var responseValue = string.Empty;

                        if (response != null)
                        {
                            Task task = response.Content.ReadAsStreamAsync().ContinueWith(t =>
                            {
                                var stream = t.Result;
                                using (var reader = new StreamReader(stream))
                                {
                                    responseValue = reader.ReadToEnd();
                                }
                            });

                            task.Wait();
                        }
                        JObject o = JObject.Parse(responseValue);
                        transactionId = (string)o["transaction"]["id"];
                    }
                    else
                    {
                        throw new Exception("No se pudo obtener transactionId " + response.StatusCode + " " + response.RequestMessage);
                    }
                }
            });            
            
            return transactionId;
        }
    }
}
