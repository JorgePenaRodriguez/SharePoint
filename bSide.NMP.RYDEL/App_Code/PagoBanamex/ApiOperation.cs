using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace bSide.NMP.RYDEL.App_Code
{
    class ApiOperation
    {
        private Merchant merchant = new Merchant();
        private IDictionary<string, string> _lastRequest = new Dictionary<string, string>();
        public IDictionary<string, string> lastRequest
        {
            get
            {
                return _lastRequest;
            }
            set
            {
                _lastRequest = value;
            }
        }

        /* /// <summary>
         /// Checks whether the card is enrolled in 3DS via either its token or form session
         /// </summary>
         /// <param name="Session">The current HTTP Session object</param>
         /// <param name="responseURL">Used to build the HTML form which redirects the card holder to the ACS</param>
         /// <returns>A dictionary including the HTML redirection from, enrollment check results, and the paReq value.</returns>
         public IDictionary<string, string> checkEnrollment(System.Web.SessionState.HttpSessionState Session, string responseURL)
         {
             Connection connection = new Connection(merchant);
             IDictionary<string, string> request = new Dictionary<string, string>();
             string SecureID = System.Guid.NewGuid().ToString();
             request.Add("3DSecure.authenticationRedirect.pageGenerationMode", "SIMPLE");
             request.Add("3DSecure.authenticationRedirect.simple.redirectDisplayBackgroundColor", "#000000");
             request.Add("3DSecure.authenticationRedirect.simple.redirectDisplayContinueButtonText", "Continue");
             request.Add("3DSecure.authenticationRedirect.simple.redirectDisplayTitle", "Waiting");
             request.Add("3DSecure.authenticationRedirect.responseUrl", responseURL);
             request.Add("3DSecureId", SecureID);
             request.Add("apiOperation", "CHECK_3DS_ENROLLMENT");
             if (!string.IsNullOrEmpty(Convert.ToString(Session["cardToken"])))
             {
                 request.Add("sourceOfFunds.token", Convert.ToString(Session["cardToken"]));
                 request.Add("sourceOfFunds.session", Convert.ToString(Session["CSCformSession"]));
             }
             else
             {
                 request.Add("sourceOfFunds.session", Convert.ToString(Session["formSession"]));
             }
             request.Add("merchant", merchant.merchantId);
             request.Add("transaction.amount", Convert.ToString(Session["total"]));
             request.Add("transaction.currency", merchant.currency);
             request.Add("apiPassword", merchant.password);
             request.Add("apiUsername", merchant.apiUsername);
             lastRequest = request;
             string receipt = connection.sendTransaction(ParseRequest(request));
             return new Dictionary<string, string>(this.parseResponse(receipt));
         }*/

        /// <summary>
        /// Conectarse al Payment Gateway e iniciar una sesión
        /// </summary>
        /// <returns>Diccionario de la respuesta del Payment Gateway</returns>

        public IDictionary<string, string> startSession(System.Web.SessionState.HttpSessionState Session)
        {
            Connection connection = new Connection(merchant);
            Guid g;
            g = Guid.NewGuid();
            Session["order.id"] = g.ToString();
            Session["merchantID"] = this.merchant.merchantId;
            Session["currency"] = this.merchant.currency;
            Session["merchantName"] = this.merchant.apiUsername;
            IDictionary<string, string> request = new Dictionary<string, string>();
            request.Add("apiOperation", "CREATE_CHECKOUT_SESSION");
            request.Add("merchant", this.merchant.merchantId);
            request.Add("apiPassword", this.merchant.password);
            request.Add("apiUsername", this.merchant.apiUsername);
            request.Add("order.id", Convert.ToString(Session["order.id"]));
            request.Add("order.amount", Convert.ToString(Session["total"]));
            request.Add("order.currency", merchant.currency);
            request.Add("interaction.cancelUrl", merchant.cancelUrlname);
            request.Add("interaction.returnUrl", merchant.ReturnUrlname);

            lastRequest = request;
            string resp = connection.sendTransaction(ParseRequest(request));
            return new Dictionary<string, string>(this.parseResponse(resp));
        }

        /// <summary>
        /// Parses and URL Decodess the string response values
        /// </summary>
        /// <param name="response">The string returned by the Payment Server</param>
        /// <returns>A dictionary of</returns>
        public IDictionary<string, string> parseResponse(string response)
        {
            Connection connection = new Connection(merchant);
            IDictionary<string, string> dict = new Dictionary<string, string>();
            if (response != null && response.Length > 0)
            {
                string[] responses = response.Split('&');
                foreach (string responseField in responses)
                {
                    string[] field = responseField.Split(new char[] { '=' }, 2);
                    dict.Add(field[0], HttpUtility.UrlDecode(field[1]));

                }
            }
						Debug.WriteLine("Start Session. Respuesta: " + dict.ToString());
            return dict;
        }
        /// <summary>
        /// Convert the values to be sent to the Payment Gateway into a properly formatted and encoded string
        /// </summary>
        /// <param name="request">The dictionary containing the values to be posted</param>
        /// <returns>A formatted and encoded string ready to be submitted to the Payment Gateway</returns>
        public string ParseRequest(IDictionary<string, string> request)
        {
            StringBuilder postData = new StringBuilder();
            foreach (string Key in request.Keys)
            {
                postData.Append(Key + "=" + HttpUtility.UrlEncode(request[Key]) + "&");
            }
            postData.Remove(postData.Length - 1, 1);
						Debug.WriteLine("ParseRequest. PostData: " + postData.ToString());
            return postData.ToString();
        }

        /// <summary>
        /// Returns a token for the currently stored card details
        /// </summary>
        /// <param name="formSession">The current form session</param>
        /// <returns></returns>
        public IDictionary<string, string> saveCard(string formSession)
        {
					try
					{
						Connection connection = new Connection(merchant);
						IDictionary<string, string> request = new Dictionary<string, string>();
						request.Add("apiOperation", "TOKENIZE");
						request.Add("sourceOfFunds.session", formSession);
						request.Add("sourceOfFunds.type", "CARD");
						request.Add("merchant", this.merchant.merchantId);
						request.Add("apiPassword", this.merchant.password);
						request.Add("apiUsername", this.merchant.apiUsername);
						lastRequest = request;
						string resp = connection.sendTransaction(ParseRequest(request));
						Dictionary<string, string> response = new Dictionary<string, string>(parseResponse(resp));
						return new Dictionary<string, string>(parseResponse(resp));
					}
					catch (Exception ex)
					{
						RydelLog.LogError(ex, "Error al obtener el token de la tarjeta almacenada actualmente.");
						return new Dictionary<string, string>();
					}
            
        }

        public IDictionary<string, string> processTransaction(System.Web.SessionState.HttpSessionState Session)
        {
					try
					{
						Connection connection = new Connection(merchant);
						IDictionary<string, string> request = new Dictionary<string, string>();
						//Añadir todos los valores necesarios para la colección
						request.Add("apiOperation", merchant.payOperation);
						request.Add("merchant", merchant.merchantId);
						request.Add("apiPassword", merchant.password);
						request.Add("apiUsername", merchant.apiUsername);
						request.Add("order.id", Convert.ToString(Session["order.id"]));
						lastRequest = request;
						//Enviar la colección para que sea convertido a una cadena urlencoded
						string receipt = HttpUtility.UrlDecode(connection.sendTransaction(ParseRequest(request)));
						Debug.WriteLine("saveCard. Receipt: " + receipt);
						return new Dictionary<string, string>(this.parseResponse(receipt));
					}
					catch (Exception ex)
					{
						RydelLog.LogError(ex, "Error al procesar la transacción con el API bancario MIDAS.");
						return new Dictionary<string, string>();
					}
        }

        public IDictionary<string, string> ProcessACSResult(string SecureID, string paRes)
        {
            Connection connection = new Connection(merchant);
            IDictionary<string, string> request = new Dictionary<string, string>();
            request.Add("apiOperation", "PROCESS_ACS_RESULT");
            request.Add("3DSecure.paRes", paRes);
            request.Add("3DSecureId", SecureID);
            request.Add("merchant", merchant.merchantId);
            request.Add("apiPassword", merchant.password);
            request.Add("apiUsername", merchant.apiUsername);
            lastRequest = request;
            string receipt = HttpUtility.UrlDecode(connection.sendTransaction(ParseRequest(request)));
						Debug.WriteLine("ProcessACSResult. Receipt" + receipt);
            return new Dictionary<string, string>(this.parseResponse(receipt));

        }
    }
}
