<%@ Assembly Name="bSide.NMP.RYDEL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=82305f90d379a283" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaDatosRyD.aspx.cs" Inherits="bSide.NMP.RYDEL.Layouts.RYDEL.ConsultaDatosRyD" DynamicMasterPageFile="~masterurl/custom.master" EnableSessionState="True" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script src="Scripts/jquery-ui-1.11.4/external/jquery/jquery.js"></script>
    <script src="Scripts/jquery-ui-1.11.4/jquery-ui.min.js"></script>
    <link href="Scripts/jquery-ui-1.11.4/jquery-ui.min.css" rel="stylesheet" />
    <%-- Script de validaciones --%>
    <script type="text/javascript">
        function CustomValidatorSelection(source, args) {
            if ($("#<%=RbtnAbono.ClientID%>").is(":checked") || $("#<%=RbtnDesempenio.ClientID%>").is(":checked") || $("#<%=RbtnRefrendo.ClientID%>").is(":checked")) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        }

        function ValidatorSelection() {
            var valName = document.getElementById("<%=RfvAbono.ClientID%>");
            var valName2 = document.getElementById("<%=RfvAbonoRange.ClientID%>");
            ValidatorEnable(valName, ($("#<%=RbtnAbono.ClientID%>").is(":checked")));
            ValidatorEnable(valName2, ($("#<%=RbtnAbono.ClientID%>").is(":checked")));

            if(!$("#<%=RbtnAbono.ClientID%>").is(":checked"))
            {
                $("#<%=TxbAbono.ClientID%>").val("");
            }
        }

        function CustomValidatorAbono(source, args) {
            if ($("#<%=RbtnAbono.ClientID%>").is(":checked") && Page_ClientValidate("abono") && $("#<%=HfAbono.ClientID%>").val() == "0") {
                $("#<%=LblAbono.ClientID%>").html($("#<%=TxbAbono.ClientID%>").val()); 
                ($("#dialog").dialog("open"))
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
    </script>
    <%-- Script para mostrar popup confirmación pago (abono) --%>
    <script type="text/javascript">
        $(function () {
            $("#dialog").dialog({
                autoOpen: false,
                modal: true,
                width: 570,
                buttons: [
                    {
                        text: "Pagar",
                        click: function () {
                            $(this).dialog("close");
                            $("#<%=HfAbono.ClientID%>").val("1");
                            var clickButton = document.getElementById("<%= BtnPagar.ClientID %>");
                            clickButton.click();
                        }
                    },
                    {
                        text: "Cancelar",
                        click: function () {
                            $(this).dialog("close");    
                        }
                    }
                ]
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="spacer_24">&#160;</div>
    <div class="container">
        <div class="col-md-2 col-xs-12 text-center backGrey spaceAfter">
            <div style="padding: 12px; float: right;">
                <img width="70" height="70" src="/PublishingImages/images2/ico-pago-linea.png" alt="" style="margin-bottom: 12px;" /><br />
                Pago en Línea
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
    <div class="spacer_24">&#160;</div>
    <div class="container">

        <div class="col-md-8 col-md-offset-2 col-xs-12 forms">
            <div class="col-md-2 col-xs-2 no-padding">
                <asp:Image ID="ImgHeader" runat="server" CssClass="displayedCenter" ImageUrl="#" />&#160;
            </div>
            <div class="col-md-10  col-xs-10 no-padding">
                <div id="DivHeader2" runat="server" class="no-padding" style="padding-bottom:10px !important"></div>
                <h6 class="spaceAfter">Número de contrato:&nbsp<asp:Label ID="LblNumContrato" runat="server" Text=""></asp:Label></h6>
            </div>
            <br class="clear" />
            <div class="col-md-8 col-md-offset-2 col-xs-12 BigSpaceAfter ">
                <div class="grayBox">
                    <div class="step BigSpaceAfter BigspaceBefore">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                        <table class="table rd-table">
                            <tbody>
                                <asp:Panel ID="PnlAbono" runat="server">
                                    <tr>
                                        <th style="vertical-align:middle; width:180px">
                                            Realizar abono:
                                        </th>
                                        <td style="vertical-align:middle">
                                            <asp:TextBox ID="TxbAbono" CssClass="form-control float-left marginRight30" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="HfAbono" runat="server" />
                                            <asp:RequiredFieldValidator ID="RfvAbono" runat="server" ControlToValidate="TxbAbono" ErrorMessage="Indique el monto del abono" ForeColor="#FF0000" Enabled="false" Display="Dynamic" ValidationGroup="abono">Indique el monto del abono</asp:RequiredFieldValidator>
                                            <br />
                                            <asp:RangeValidator ID="RfvAbonoRange" runat="server" Type="Double" ErrorMessage="Monto inválido" ControlToValidate="TxbAbono" ForeColor="#FF0000" Enabled="false" Display="Dynamic" ValidationGroup="abono">El monto capturado NO es válido, por favor rectifique</asp:RangeValidator>
                                        </td>
                                        <td style="vertical-align:middle">
                                            <div class="checkbox">
                                                <asp:RadioButton ID="RbtnAbono" GroupName="opPago" runat="server" AutoPostBack="False" OnClick="ValidatorSelection();" />
                                            </div>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="PnlDesempenio" runat="server">
                                    <tr>
                                        <th style="vertical-align:middle">
                                            Desempeño:
                                        </th>
                                        <td style="vertical-align:middle">
                                            <asp:TextBox ID="TxbDesempenio" CssClass="form-control float-left disabledInput marginRight30" runat="server" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td style="vertical-align:middle">
                                            <div class="checkbox">
                                                <asp:RadioButton ID="RbtnDesempenio" GroupName="opPago" runat="server" AutoPostBack="False" OnClick="ValidatorSelection();" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="vertical-align:middle">
                                            <p class="p-small" style="margin-top: 0px">
                                                Fecha de vencimiento:&nbsp<asp:Label ID="LblFechaVencimiento" runat="server" Text=""></asp:Label>
                                            </p>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="PnlRefrendo" runat="server">
                                    <tr>
                                        <th style="vertical-align:middle">
                                            Refrendo No.&nbsp<asp:Label ID="LblNumRefrendo" runat="server" Text=""></asp:Label>:
                                        </th>
                                        <td style="vertical-align:middle">
                                            <asp:TextBox ID="TxbRefrendo" CssClass="form-control float-left disabledInput marginRight30" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="vertical-align:middle">
                                            <div class="checkbox">
                                                <asp:RadioButton ID="RbtnRefrendo" GroupName="opPago" runat="server" AutoPostBack="False" OnClick="ValidatorSelection();" />
                                            </div>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </tbody>
                        </table>
                        <div style="text-align: center">
                            <asp:CustomValidator ID="DiscountAmountCustomValidator"
                                runat="server"
                                ValidateEmptyText="True"
                                ClientValidationFunction="CustomValidatorSelection"
                                ErrorMessage="Porfavor seleccione un pago"
                                ForeColor="#FF0000"
                                ToolTip="Porfavor seleccione un pago"
                                Display="Dynamic">Por favor seleccione un pago</asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidatorAbono"
                                runat="server"
                                Display="None"
                                ClientValidationFunction="CustomValidatorAbono"
                                ErrorMessage="CustomValidator"></asp:CustomValidator>

                        </div>
                         
                        <asp:Label ID="LblMessage" CssClass="displayedCenter" runat="server" Text=""></asp:Label>
                        <asp:Button ID="BtnPagar" CssClass="btn-generic displayedCenter spaceAfter spaceBefore" runat="server" OnClick="BtnPagar_Click" Text="Pagar" />
                        </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <div class="waitingDiv">
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="spacer_24">&#160;</div>

    <div id="dialog" title="Pago en abono">
        <div class="col-md-3 col-xs-12 no-padding">
            <img class="displayedCenter" src="/PublishingImages/images2/ico-confirmar.png" alt="" />&#160;
        </div>
        <div class="col-md-9 col-xs-12 no-padding">
            <br />
            Confirma que el abono que deseas realizar es por <br />
            $<asp:Label ID="LblAbono" runat="server" Text=""></asp:Label>
        </div>
    </div>
    <asp:HiddenField ID="HfNumTransaccion" runat="server" />

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Consulta datos refrendo y desempeño
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Consulta datos refrento y desempeño
</asp:Content>
