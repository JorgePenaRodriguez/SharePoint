<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Process.aspx.cs" Inherits="bSide.NMP.RYDEL.Layouts.RYDEL.Process" DynamicMasterPageFile="~masterurl/custom.master" EnableSessionState="True" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
	
	<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.js"         type="text/javascript"></script>
		//<script type="text/javascript">
  	//$.getScript('https://banamex.dialectpayments.com/checkout/version/<%= Session["version"] %>/checkout.js');
  	//</script>
<%--  --%>

    <script
        data-cancel="cancelCallback"
        data-error="errorCallback"
        src="https://banamex.dialectpayments.com/checkout/version/<%= Session["version"] %>/checkout.js">
    </script>

    <script type="text/javascript">
    	var sesion = '<%= Session["formSession"] %>';
    	var ps = '<%= Session["partidaSaldos"] %>';

    	//Valida partida saldos
    	if (ps == '')
    		window.location.href = "ConsultaEmpenio.aspx";

    	//Valida sesión
    	if (sesion == '')
    		window.location.href = "ConsultaDatosRyD.aspx";

    	function errorCallback(error) {
    		console.log(JSON.stringify(error));
    	}

    	function cancelCallback() {
    		console.log('Pago cancelado');
    	}

    	//Inicializa pago tipo lightbox con api Banamex
    	function paywLightbox() {
    		Checkout.configure({
    			merchant: '<%= Session["merchantID"] %>',
            	session: {
            		id: '<%= Session["formSession"] %>'
                },
            	order: {
            		amount: '<%= Session["total"] %>',
                	currency: '<%= Session["currency"]%>',
                	description: '<%= Session["reference"]%>',
                	id: '<%= Convert.ToString(Session["order.id"]) %>',
                	reference: '<%= Session["reference"]%>'
                },
            	interaction: {
            		merchant: {
            			name: 'Nacional Monte de Piedad'
            		}
            	}
            });
							Checkout.showLightbox();
						}

						//window.onload = paywLightbox;
						window.onload = PageSetLoad();
						function PageSetLoad() {
							setTimeout(paywLightbox, 3000);
						}
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Procesando pago
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Procesando pago
</asp:Content>
