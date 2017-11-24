using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using bSide.NMP.RYDEL.App_Code;
using System.Collections.Generic;
using RYDEL.modelView;
using System.IO;
using System.Web;
using System.ServiceModel;
using System.Diagnostics;

namespace bSide.NMP.RYDEL.Layouts.RYDEL
{
    public partial class Receipt : UnsecuredLayoutsPageBase
    {
        /// <summary>
        /// Habilita acceso anónimo
        /// </summary>
        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            Merchant merchant = new Merchant();
            if (Session["partidaSaldos"] == null || string.IsNullOrEmpty(Session["partidaSaldos"].ToString()))
            {
                Response.Redirect(SPContext.Current.Web.Url + "/_layouts/15/RYDEL/ConsultaEmpenio.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }

            if (!IsPostBack)
            {
                try
                {
                    GetTransaction();
                }
                catch (Exception ex)
                {
                    RydelLog.LogError(ex, "Error al obtener pago");
                }
            }
        }

        protected void BtnComprobanteAbono_Click(object sender, EventArgs e)
        {
            byte[] data = Convert.FromBase64String(HfComprobanteAbono.Value);
						Debug.WriteLine("Comprobante de Abono: " + HfComprobanteAbono.Value);
            DownloadFile(data, "ComprobanteAbonoEnLinea.pdf");
        }        

        protected void BtnComprobanteRefDes_Click(object sender, EventArgs e)
        {
            byte[] data = null;
            string filename = string.Empty;
						try
						{
							if (!string.IsNullOrEmpty(HfComprobanteDesempenio.Value))
							{
								data = Convert.FromBase64String(HfComprobanteDesempenio.Value);
								Debug.WriteLine("Comprobante de Desempenio: " + HfComprobanteAbono.Value);
								filename = "ComprobanteDesempenoEnLinea.pdf";
							}
							else if (!string.IsNullOrEmpty(HfComprobanteRefrendo.Value))
							{
								data = Convert.FromBase64String(HfComprobanteRefrendo.Value);
								Debug.WriteLine("Comprobante de Desempenio: " + HfComprobanteRefrendo.Value);
								filename = "ComprobanteRefrendoEnLinea.pdf";
							}
						}
						catch (Exception ex)
						{
							RydelLog.LogError(ex, "Error al cargar el comprobante de pago");
						}

            if (data == null)
                return;

            DownloadFile(data, filename);
        }

        protected void btnInicio_Click(object sender, EventArgs e)
        {
            Session["partidaSaldos"] = null;
            Response.Redirect(SPContext.Current.Web.Url + "/_layouts/15/RYDEL/ConsultaEmpenio.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Obtiene la transacción del pago
        /// </summary>
        private void GetTransaction()
        {
            try
            {
                BtnFacturaLinea.PostBackUrl = SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.urlFacturacionEnLinea);
            }
            catch (Exception ex)
            {
                RydelLog.LogError(ex, "Error al establecer url de facturación en línea");
            }

            //Se obtiene el resultado de la transacción
            ApiOperation op = new ApiOperation();

						// Operacion de pago
            IDictionary<string, string> receiptcollection = new Dictionary<string, string>(op.processTransaction(Session));

						// Si la transaccion es exitosa se almancena el numero de tarjeta en la Session
            if ((Convert.ToString(Session["successIndicator"]) == Request.QueryString["resultIndicator"]) && !(String.IsNullOrEmpty(Convert.ToString(Session["order.id"])))
                && (receiptcollection.ContainsKey("transaction[0].result") && receiptcollection["transaction[0].result"] == "SUCCESS"))
            {
							if (receiptcollection["sourceOfFunds.provided.card.number"] != null)
							{
								Session["sourceOfFunds.provided.card.number"] = receiptcollection["sourceOfFunds.provided.card.number"];
							}
              PagoExitoso(receiptcollection);
            }
            else
            {
                PnlPagoExitoso.Visible = false;
                PnlPagoNoProcesado.Visible = true;
                PnlPagoSinNotificar.Visible = false;
            }

            var ps = Utils.GetPartidaSaldosFromSession();
            LblNumContrato.Text = ps.folio.ToString();
        }

        /// <summary>
        /// Acciones al realizarse un pago exitoso
        /// </summary>
        /// <param name="receiptcollection"></param>
        private void PagoExitoso(IDictionary<string, string> receiptcollection)
        {
						// Folio de la operacion que se utiliza tras la realizacion de un pago exitos, funciona como parámetro para obtener la información de la tarjeta con la que se pagó.
            LblFolioOperacion.Text = receiptcollection["transaction[0].transaction.authorizationCode"];
						Session["transaction[0].transaction.authorizationCode"] = receiptcollection["transaction[0].transaction.authorizationCode"];

						LblFolioAutorizacion.Text = receiptcollection["transaction[0].transaction.authorizationCode"];

						var comprobantesPago = new List<notificarPagoResponseComprobantePDF>();
            try
            {
                comprobantesPago = Services.NotificaPago(receiptcollection);
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                RydelLog.LogError(soapEx, "Error al consultar servicio, fault code: " + soapEx.Code);
                PnlPagoExitoso.Visible = false;
                PnlPagoNoProcesado.Visible = false;

                if (soapEx.Code != null && !string.IsNullOrEmpty(soapEx.Code.Name))
                {
                    string faultMsg = SharePointDA.GetMensaje(soapEx.Code.Name.Trim());
                    if (!string.IsNullOrEmpty(faultMsg))
                        PagoSinNotificarMsg.InnerHtml = faultMsg;
                }               
                
                PnlPagoSinNotificar.Visible = true;
                return;
            }            
            catch (Exception ex)
            { 
                RydelLog.LogError(ex, "Error al notificar pago en servicio");
                PnlPagoExitoso.Visible = false;
                PnlPagoNoProcesado.Visible = false;
                PnlPagoSinNotificar.Visible = true;
                return;
            }

            PnlPagoExitoso.Visible = true;
            PnlPagoNoProcesado.Visible = false;
            PnlPagoSinNotificar.Visible = false;

            PnlComprobanteAbono.Visible = false;
            PnlComprobanteRefDes.Visible = false;

            foreach (var item in comprobantesPago)
            {
                if (item.idOperacion == (long)Constantes.TipoPago.idAbono && item.archivo != null)
                {
                    PnlComprobanteAbono.Visible = true;
                    HfComprobanteAbono.Value = Convert.ToBase64String(item.archivo);
                }
                if (item.idOperacion == (long)Constantes.TipoPago.idDesempenio && item.archivo != null)
                {
                    PnlComprobanteRefDes.Visible = true;
                    HfComprobanteDesempenio.Value = Convert.ToBase64String(item.archivo);
                }
                if (item.idOperacion == (long)Constantes.TipoPago.idRefrendo && item.archivo != null)
                {
                    PnlComprobanteRefDes.Visible = true;
                    HfComprobanteRefrendo.Value = Convert.ToBase64String(item.archivo);
                }
            }
        }

        /// <summary>
        /// Descarga un archivo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filename"></param>
        private void DownloadFile(byte[] data, string filename)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.AddHeader("content-length", data.Length.ToString());
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", filename));
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(data);
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion        
    }
}
