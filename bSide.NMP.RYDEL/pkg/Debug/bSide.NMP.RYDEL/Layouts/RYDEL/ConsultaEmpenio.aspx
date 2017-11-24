<%@ Assembly Name="bSide.NMP.RYDEL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=82305f90d379a283" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaEmpenio.aspx.cs" Inherits="bSide.NMP.RYDEL.Layouts.bSide.NMP.RYDEL.ConsultaEmpenio" DynamicMasterPageFile="~masterurl/custom.master" EnableSessionState="True" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script src="../RYDEL/Scripts/jquery-ui-1.11.4/external/jquery/jquery.js"></script>
    <script src="../RYDEL/Scripts/jquery-ui-1.11.4/jquery-ui.min.js"></script>
    <link href="../RYDEL/Scripts/jquery-ui-1.11.4/jquery-ui.min.css" rel="stylesheet" />
    <%-- Estilo tooltip ayuda --%>
    <style>
        .ui-tooltip {
            max-width: 500px;
            width: 500px;
        }
    </style>

    <script type="text/javascript">
        //Función tooltip ayuda
        $(function () {
            $("#<%=ImgQuestion.ClientID%>").tooltip({
                content: "<img src='../RYDEL/Images/bannerNoContrato.png' />",
                position: {
                    my: "right+35 top+30",
                    at: "right center",
                    open: function (event, ui) {
                        ui.tooltip.css("max-width", "500px");
                    }
                }
                })
        });

        //Función que valida entrada numérica
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="spacer_24">&#160;</div>
    <div id="DivHeader1" class="container">
        <div class="col-md-2 col-xs-12 text-center backGrey spaceAfter">
            <div style="padding: 12px; float: right;">
                <img src="/PublishingImages/images2/ico-pago-linea.png" alt="" style="margin-bottom: 12px;" />
                <br />
                Pago en Línea
            </div>
        </div>
        <div class="col-md-10 col-xs-12">
            <ul class="plecas">
                <li>
                    <h1>Consulta de empeño o refrendo en línea.</h1>
                </li>
                <li>
                    <h2>
                        <asp:Label ID="LblHeader" runat="server" Text="Por favor, llena los siguientes datos:"></asp:Label>
                    </h2>
                </li>
            </ul>
        </div>
    </div>
    <div class="spacer_24">&#160;</div>
    <div class="container">
        <div class="col-md-8 col-md-offset-2 col-xs-12 forms">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="PnlHeader2" runat="server">
                        <div class="no-padding">
                            <asp:Image ID="ImgHeader" runat="server" ImageUrl="#" Style="margin-top: 0px" />
                        </div>
                        <div id="DivHeader2" runat="server" class="no-padding">
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PnlContrato" runat="server">
                        <div id="DivContrato" class="col-md-8 col-md-offset-2 col-xs-12">
                            <asp:Panel ID="PnlNoAdeudo" runat="server" Visible="false">
                                <h6 class="spaceAfter">Realizar otra consulta&#58;</h6>
                            </asp:Panel>
                            <table style="width: 100%">
                                <tr>
                                    <td colspan="2" style="margin-left: 20px">Número de contrato:  
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="TxbContrato" CssClass="form-control" runat="server" onkeypress="return isNumber(event)"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvContrato" runat="server" ControlToValidate="TxbContrato" ErrorMessage="Contrato requerido" ForeColor="#FF0000">Ingrese el número de contrato</asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 60px">
                                        <asp:Image ID="ImgQuestion" ToolTip="<img src='../RYDEL/Images/bannerNoContrato.png' />" runat="server" class="item" ImageUrl="../RYDEL/Images/cuestion.png" Width="34px" Height="34px" Style="margin: -20px 0px 0px 20px; cursor: pointer;" />
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="HfContrato" runat="server" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PnlCaptcha" runat="server">
                        <div id="DivCaptcha" class="col-md-8 col-md-offset-2 col-xs-12">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <div id="divCaptcha" runat="server">
                                            <asp:Image ID="imgCaptcha" runat="server" width="90%"/>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="Ingrese los números que se muestran arriba:"></asp:Label></td>
                                    <td style="width: 120px">
                                        <asp:TextBox ID="txtCaptcha" runat="server" CssClass="Validar" MaxLength="6" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">¿Problemas para leerlo?
                                        <asp:ImageButton ID="btnRefresh" ImageUrl="Images/btn_reloadNMP.png" runat="server" OnClick="btnRefresh_Click" Style="display: inline;" CssClass="claseRefresca" CausesValidation="False" />
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="txtOculto" runat="server" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PnlVerifica" runat="server" Visible="false">
                        <div class="col-md-8 col-md-offset-2 col-xs-12">
                            <div class="BigSpaceAfter spaceBefore">
                                <div class="spaceAfter">Número de contrato&#58; <span class="red-color">
                                    <asp:Label ID="LblNumeroContrato" runat="server" Text=""></asp:Label>
                                </span></div>
                                Cliente&#58;                                
				<p>
                    <asp:Label ID="LblCliente" runat="server" Text=""></asp:Label>
                </p>
                            </div>
                            <div class="BigSpaceAfter">
                                Correo Electrónico&#58;<br class="clear">
                                <asp:Panel ID="PnlMostrarEmail" runat="server" Visible="false">
                                    <div class="col-md-9 col-xs-12">
                                        <p>
                                            <asp:Label ID="LblEmail" runat="server" Text=""></asp:Label>
                                        </p>
                                    </div>
                                    <div class="col-md-3 col-xs-12 BigSpaceAfter">
                                        <asp:LinkButton ID="LbtnEditarEmail" CssClass="lonly-link" runat="server" OnClick="LbtnEditarEmail_Click">Editar</asp:LinkButton>
                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="PnlEditarEmail" runat="server" Visible="false">
                                    <div class="col-md-9 col-xs-12 no-padding">
                                        <div class="form-group">
                                            <asp:TextBox ID="TxbEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RfvEmail" runat="server" Display="Dynamic" ControlToValidate="TxbEmail" ErrorMessage="Favor de indicar una cuenta de correo electrónico para poder enviar tus comprobantes" ForeColor="#FF0000">Favor de indicar una cuenta de correo electrónico para poder enviar tus comprobantes</asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ControlToValidate="TxbEmail" ForeColor="#FF0000" ErrorMessage="Email inválido" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">Email inválido</asp:RegularExpressionValidator>
                                            &#160;
                                        </div>
                                        &#160;
                                    </div>
                                    <div class="col-md-3 col-xs-12 spaceAfter">
                                        <asp:Button ID="BtnEditarEmail" runat="server" Text="Guardar" CssClass="btn-generic displayedCenter spaceAfter" OnClick="BtnEditarEmail_Click" />
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </asp:Panel>
                    <br class="clear">
                    <div id="DivMessage" style="text-align: center">
                        <br />
                        <asp:Label ID="LblMessage" CssClass="displayedCenter spaceAfter" runat="server" Text=""></asp:Label>
                    </div>
                    <asp:Panel ID="PnlBtnAceptar" runat="server">
                        <asp:Button ID="BtnAceptar" CssClass="btn-generic displayedCenter spaceAfter" OnClick="BtnAceptar_Click" runat="server" Text="Consultar" />
                    </asp:Panel>
                    <asp:Panel ID="PnlBtnSiguiente" runat="server">
                        <asp:Button ID="BtnSiguiente" CssClass="btn-generic displayedCenter spaceAfter" OnClick="BtnSiguiente_Click" runat="server" Text="Siguiente" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div class="waitingDiv">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>    
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Consulta empeño
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Consulta empeño
</asp:Content>
