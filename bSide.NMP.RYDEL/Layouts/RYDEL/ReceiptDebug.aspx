<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceiptDebug.aspx.cs" Inherits="bSide.NMP.RYDEL.Layouts.RYDEL.ReceiptDebug" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
        <script src="Scripts/jquery-ui-1.11.4/external/jquery/jquery.js"></script>
    <script src="Scripts/jquery-ui-1.11.4/jquery-ui.min.js"></script>
    <link href="Scripts/jquery-ui-1.11.4/jquery-ui.min.css" rel="stylesheet" />

    <script type="text/javascript">
        $(function () {
            $("#dialog").dialog({
                autoOpen: true,
                modal: true,
                width: 570,
                buttons: {
                    "Aceptar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
     <asp:Panel ID="panelSuccess" runat="server" Visible="False">
        <table width="50%" align="center" cellpadding="5" border="0">
            <tr class="title">
                <td colspan="3"><strong>Test venta</strong></td>
            </tr>
            <tr>
                <td align="center" class="shade" colspan="3" height="25">Su compra fue exitosa! Gracias. Haga clic <a href="./Default.aspx">aqu&iacute;</a> para comprar m&aacute;s pizzas.</td>
            </tr>
            <tr>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td colspan="3" align="center"><strong>Resultado</strong></td>
            </tr>
            <tr class="shade">
                <td align="right" width="50%" valign="top">Compra: </td>
                <td align="left" width="50%" valign="top" id="Approval"><strong>
                    <asp:Label ID="LabelResult" runat="server"></asp:Label></strong>
                </td>
            </tr>
            <tr class="shade">
                <td align="right" width="50%" valign="top">Receipt:</td>
                <td align="left" width="50%" valign="top" id="Receipt"><strong>
                    <asp:Label ID="LabelReceipt" runat="server"></asp:Label></strong>
                </td>
            </tr>
            <tr class="shade">
                <td align="right" width="50%" valign="top">Response Code:</td>
                <td align="left" width="50%" valign="top" id="ResponseCode"><strong>
                    <asp:Label ID="LabelResponseCode" runat="server"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td class="shade2" colspan="3"></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panelDeclined" runat="server" Visible="False">
        <table width="50%" align="center" cellpadding="5" border="0">
            <tr class="title">
                <td colspan="3"><strong>Pizzas</strong></td>
            </tr>
            <tr>
                <td align="center" class="shade" colspan="3" height="25">Your purchase was declined with response code 
          <asp:Label ID="LabelErrorResponse" runat="server"></asp:Label>. Click <a href="./Default.aspx">here</a> to attempt your purchase again.</td>
            </tr>
            <tr>
                <td class="shade2" colspan="3"></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panelError" runat="server" Visible="False">
        <table width="50%" align="center" cellpadding="5" border="0">
            <tr class="title">
                <td colspan="3"><strong>Pizzas</strong></td>
            </tr>
            <tr>
                <td align="center" class="shade" colspan="3" height="25">Your purchase has failed unexpectedly. Click <a href="./Default.aspx">here</a> to attempt your purchase again.</td>
            </tr>
            <tr>
                <td class="shade2" colspan="3"></td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="detailsPanel" Visible="false" runat="server"
        HorizontalAlign="Left">
        <table width="50%" align="center" cellpadding="5" border="0">
            <tr class="title" align="left">
                <td colspan="2"><strong>Debug Information</strong></td>
            </tr>
            <tr class="shade">
                <td height="25"><strong>Payment Transaction Request</strong></td>
                <td height="25"><strong>Payment Transaction Results</strong></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:GridView ID="gridViewPaymentRequest" runat="server">
                    </asp:GridView>
                </td>
                <td valign="top">
                    <asp:GridView ID="gridViewPayResult" runat="server" HorizontalAlign="left">
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panelAuthenticationDebug" Visible="false" runat="server"
        HorizontalAlign="Left">
        <div style="max-width: 800px; overflow: scroll; margin: auto;">
            <table width="50%" align="center" cellpadding="5" border="0">
                <tr class="shade">
                    <td><strong>3DS Authentication Request</strong></td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gridViewAuthenticationRequest" runat="server" RowStyle-CssClass="grid"
                            HorizontalAlign="Left">
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <div style="max-width: 800px; overflow: scroll; margin: auto;">
            <table width="50%" align="center" cellpadding="5" border="0">
                <tr class="shade">
                    <td><strong>3DS Authentication Result</strong></td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gridViewThreeDSresults" runat="server"
                            HorizontalAlign="left">
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <div id="dialog" title="Pago en abono">
        <asp:Panel ID="PnlPagoExitoso" runat="server">
            <div class="col-md-3 col-xs-12 no-padding">
                <img class="displayedCenter" src="/PublishingImages/images2/ico-pago-exitoso.png" alt="" />&#160;
            </div>
            <div class="col-md-9  col-xs-12 no-padding spaceAfter">
                <h3>
                    <span class="red-color">¡Tu pago se realizó exitosamente!</span></h3>
            </div>
            <br class="clear" />
            <p>El número de folio de esta operación es
                <asp:Label ID="LblFolioOperacion" runat="server" Text=""></asp:Label>.</p>
            <p>Tu comprobante de pago se envió al correo que registraste, favor de verificarlo.</p>
            <p>
                <strong>Recuerda guardar o imprimir tu comprobante de operación para cualquier aclaración.</strong>
            </p>

        </asp:Panel>
        <asp:Panel ID="PnlPagoNoProcesado" runat="server">
            <div class="col-md-3 col-xs-12 no-padding">
                <img class="displayedCenter" src="/PublishingImages/images2/ico-pago-no-procesado.png" alt="" />&#160;
            </div>
            <div class="col-md-9  col-xs-12 no-padding spaceAfter">
                <h3>
                    <span class="red-color">Tu pago no pudo ser procesado, verifica que tus datos sean correctos y vuelve a intentarlo.</span></h3>
            </div>
            <br class="clear" />
            <p>Si persiste el problema, por favor comunícate con tu banco emisor o marca al 01 800 EL MONTE (01 800 35 666 83) para cualquier duda o aclaración.</p>
        </asp:Panel>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
