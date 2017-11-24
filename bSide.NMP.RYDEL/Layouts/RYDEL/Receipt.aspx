<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Receipt.aspx.cs" Inherits="bSide.NMP.RYDEL.Layouts.RYDEL.Receipt" DynamicMasterPageFile="~masterurl/custom.master" EnableSessionState="True" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script src="Scripts/jquery-ui-1.11.4/external/jquery/jquery.js"></script>
    <script src="Scripts/jquery-ui-1.11.4/jquery-ui.min.js"></script>
    <link href="Scripts/jquery-ui-1.11.4/jquery-ui.min.css" rel="stylesheet" />
    <style>
        /*Estilo para boton multilinea*/
        .btnMultiLine {
            white-space: normal !important;
            height: 50px !important;
            width: 150px !important;
        }
    </style>

    <script type="text/javascript">
        //Inicializa dialog (popup)
        $(function () {
            $("#dialog").dialog({
                autoOpen: true,
                modal: true,
                width: 570,
                buttons: [
                    {
                        text: "Aceptar",
                        click: function () {
                        $(this).dialog("close");
                        }
                    }
                ]
            });
        });

        //Corrige la descarga de archivos
        function setFormSubmitToFalse() {
            setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);
            return true;
        }
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="container">
        <div class="col-md-2 col-xs-12 text-center backGrey spaceAfter">
            <div style="padding: 12px; float: right;">
                <a class="lnk-001" href="#">
                    <img width="70" height="70" src="/PublishingImages/images2/ico-pago-linea.png" alt="" style="margin-bottom: 12px;" />
                    <br />
                    Pago en Línea </a>
            </div>
        </div>
        <div class="col-md-10 col-xs-12">
            <ul class="plecas">
                <li>
                    <h1>Consulta de empeño o refrendo en línea.</h1>
                </li>
                <li>
                    <h2></h2>
                </li>
            </ul>
        </div>
    </div>
    <div class="container">
        <div class="col-md-8 col-md-offset-3 col-xs-12 forms">
            <h3 class="h-smaller">
                <span class="red-color">Comprobante de operación</span> </h3>
            
                Guarda o imprime tu comprobante de operación, el cual es tu comprobante de pago.<br />
                Sigue las instrucciones de acuerdo al tipo de pago que realizaste.

            <h3>Número de contrato: 
         <span class="red-color"></span>
                <asp:Label ID="LblNumContrato" runat="server" Text=""></asp:Label></h3>
            <br class="clear" />

					<h3>Número de autorización: 
         <span class="red-color"></span>
                <asp:Label ID="LblFolioAutorizacion" runat="server" Text=""></asp:Label></h3>
            <br class="clear" />

            <asp:Panel ID="PnlComprobanteAbono" runat="server" Visible="false">
                <div class="col-md-1 col-xs-2 no-padding">
                    <img class="displayedCenter" src="/PublishingImages/images2/ico-s-comprobante-operacion.png" alt="" />&#160;
                </div>
                <div class="col-md-11  col-xs-10 no-padding ">
                    <h3 class="h-smaller">Si realizaste un <strong>abono</strong></h3>
                    Guarda tu comprobante de operación para tu control y para cualquier aclaración.
                    <br /><br />
                    <asp:Button ID="BtnComprobanteAbono" runat="server" CssClass="btn-generic displayedCenter spaceAfter" Text="Guardar comprobante" OnClick="BtnComprobanteAbono_Click" OnClientClick="javascript:setFormSubmitToFalse()"/>
                </div>
            </asp:Panel>

            <asp:Panel ID="PnlComprobanteRefDes" runat="server" Visible="false">
                <div class="col-md-1 col-xs-2 no-padding">
                    <img class="displayedCenter" src="/PublishingImages/images2/ico-s-comprobante-operacion.png" alt="" />&#160;
                </div>
                <div class="col-md-11  col-xs-10 no-padding spaceAfter">
                    <h3 class="h-smaller">Si tu abono cubre un <strong>refrendo o desempeño</strong></h3>
                    <ul class="listStyleOk p-smaller">
                        <li>Imprime tu Comprobante de Operación.</li>
                        <li>Asiste a tu sucursal.</li>
                        <li>Pasa a caja y acredita tu pago.</li>
                        <li>Recoge tu prenda en la sucursal donde la empeñaste.</li>
                    </ul>
                    <br />
                    <asp:Button ID="BtnComprobanteRefDes" runat="server" Text="Imprimir comprobante" CssClass="btn-generic displayedCenter" OnClick="BtnComprobanteRefDes_Click" OnClientClick="javascript:setFormSubmitToFalse()"/>
                </div>
            </asp:Panel>

            <div class="col-md-1 col-xs-2 no-padding">
                <img class="displayedCenter" src="/PublishingImages/images2/ico-s-factura-linea.png" alt="" />&#160;
            </div>
            <div class="col-md-11  col-xs-10 no-padding spaceAfter">
                <h3 class="h-smaller">
                    <strong>Facturación.</strong></h3>
                Recuerda que tienes 72 horas a partir de que se refleja tu pago para obtener tu factura. Después de este tiempo puedes obtener tu factura comunicándote al 23 3445 5646.
                <br /><br />
                <asp:Button ID="BtnFacturaLinea" runat="server" CssClass="btn-generic displayedCenter spaceAfter" Text="Factura en línea" />
            </div>
            <br class="clear" />
            <div class="separador">
                <asp:Button ID="btnInicio" runat="server" CssClass="btn-generic float-left margin40" Text="Inicio" OnClick="btnInicio_Click" PostBackUrl="ConsultaEmpenio.aspx" />
                <asp:Button ID="btnOtraOperacion" runat="server" CssClass="btn-generic float-right margin40" Text="Hacer otra operación" PostBackUrl="ConsultaEmpenio.aspx?actualiza=1" />
            </div>
        </div>
    </div>    ​
    <div id="dialog" title="Pago">
        <asp:Panel ID="PnlPagoExitoso" runat="server">
            <div class="col-md-3 col-xs-12 no-padding">
                <img class="displayedCenter" src="/PublishingImages/images2/ico-pago-exitoso.png" alt="" />&#160;
            </div>
            <div class="col-md-9  col-xs-12 no-padding spaceAfter">
                <h3>
                    <span class="red-color">¡Tu pago se realizó exitosamente!</span></h3>
            </div>
            <br class="clear" />
            <p>
                El número de folio de esta operación es
                <asp:Label ID="LblFolioOperacion" runat="server" Text=""></asp:Label>.
            </p>
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
        <asp:Panel ID="PnlPagoSinNotificar" runat="server">
            <div class="col-md-3 col-xs-12 no-padding">
                <img class="displayedCenter" src="/PublishingImages/images2/ico-pago-no-procesado.png" alt="" />&#160;
            </div>
            <div class="col-md-9  col-xs-12 no-padding">
                <p>
                    <span class="red-color" id ="PagoSinNotificarMsg" runat="server">Estimado usuario, el pago fue recibido de manera exitosa, por el momento el comprobante no se encuentra disponible favor de pasar a sucursal o llamar a la línea monte.</span></p>
            </div>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="HfComprobanteAbono" runat="server" />
    <asp:HiddenField ID="HfComprobanteRefrendo" runat="server" />
    <asp:HiddenField ID="HfComprobanteDesempenio" runat="server" />
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Respuesta de pago
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Respuesta de pago
</asp:Content>
