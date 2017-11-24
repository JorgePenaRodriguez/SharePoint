using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using bSide.NMP.RYDEL.App_Code;
using System.Collections.Generic;

namespace bSide.NMP.RYDEL.Layouts.RYDEL
{
    public partial class ReceiptDebug : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Merchant merchant = new Merchant();
            if (Session.Keys.Count == 0)
            {
                Response.Redirect(SPContext.Current.Web.Url);
            }

            //Obtener el resultado de la transacción
            ApiOperation op = new ApiOperation();
            IDictionary<string, string> receiptcollection = new Dictionary<string, string>(op.processTransaction(Session));
            gridViewPaymentRequest.DataSource = op.lastRequest;
            gridViewPaymentRequest.DataBind();
            gridViewPayResult.DataSource = receiptcollection;
            gridViewPayResult.DataBind();

            if ((Convert.ToString(Session["successIndicator"]) == Request.QueryString["resultIndicator"]) && !(String.IsNullOrEmpty(Convert.ToString(Session["order.id"]))))
            {
                if (receiptcollection.ContainsKey("transaction[0].result") && receiptcollection["transaction[0].result"] == "SUCCESS")
                {
                    LabelResult.Text = receiptcollection["transaction[0].response.gatewayCode"];
                    LabelResponseCode.Text = receiptcollection["transaction[0].response.acquirerCode"];
                    panelSuccess.Visible = true;
                }

                var comprobantesPago = Services.NotificaPago(receiptcollection);

                LblFolioOperacion.Text = receiptcollection["transaction[0].transaction.authorizationCode"];
                PnlPagoExitoso.Visible = true;
                PnlPagoNoProcesado.Visible = false;
            }
            else
            {
                LabelErrorResponse.Text = "UNKNOWN";

                if (receiptcollection.ContainsKey("transaction[0].response.acquirerCode") && receiptcollection.ContainsKey("transaction[0].response.acquirerCode"))
                {
                    LabelErrorResponse.Text = receiptcollection["transaction[0].response.acquirerCode"];
                }
                panelDeclined.Visible = true;

                PnlPagoExitoso.Visible = false;
                PnlPagoNoProcesado.Visible = true;
            }

            if (receiptcollection.ContainsKey("error.cause"))
            {
                panelError.Visible = true;
            }

            //Debug info
            if (merchant.debug_mode)
            {
                gridViewThreeDSresults.DataSource = Session["3DSResults"];
                gridViewThreeDSresults.DataBind();
                gridViewAuthenticationRequest.DataSource = Session["process3DSResultRequest"];
                gridViewAuthenticationRequest.DataBind();
                detailsPanel.Visible = true;
                if (Convert.ToString(Session["cardEnrolled"]) == "CARD_ENROLLED")
                {
                    panelAuthenticationDebug.Visible = true;
                }
            }
        }

        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }
    }
}

