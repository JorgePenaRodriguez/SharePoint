using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using bSide.NMP.RYDEL.App_Code;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using RYDEL.modelView;

namespace bSide.NMP.RYDEL.Layouts.RYDEL
{
	public partial class ConsultaDatosRyD : UnsecuredLayoutsPageBase
	{
		#region Variables
		string nombreCliente = "0";
		decimal AmountDesempenio = -1;
		decimal AmountRefrendo = -1;
		decimal AmountAbonoMin = -1;
		decimal AmountAbonoMax = -1;
		#endregion

		#region Eventos
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["partidaSaldos"] == null || string.IsNullOrEmpty(Session["partidaSaldos"].ToString()))
			{
				Response.Redirect(SPContext.Current.Web.Url + "/_layouts/15/RYDEL/ConsultaEmpenio.aspx", false);
				Context.ApplicationInstance.CompleteRequest();
			}

			LblMessage.Text = string.Empty;
			HfAbono.Value = "0";
			if (!IsPostBack)
			{
				try
				{
					SetData();
				}
				catch (Exception ex)
				{
					RydelLog.LogError(ex, "Error al cargar información");
					LblMessage.Text = Constantes.mensajeError;
				}
			}
		}

		protected void BtnPagar_Click(object sender, EventArgs e)
		{
			if (initiateCheckout())
			{ 


				SetData();

				PartidaSaldos pSaldos = Utils.GetPartidaSaldosFromSession();




				decimal monto = 0;
				if (RbtnRefrendo.Checked)
					monto = Convert.ToDecimal(TxbRefrendo.Text.Replace("$", ""));
				if (RbtnDesempenio.Checked)
					monto = Convert.ToDecimal(TxbDesempenio.Text.Replace("$", ""));
				if (RbtnAbono.Checked)
					monto = Convert.ToDecimal(TxbAbono.Text.Replace("$", ""));
				//PreRegistrar
				try
				{
					Services.SavePreRegistrer(pSaldos.correoElectronico, monto, Session["order.id"].ToString());
					//LblMessage.Text = Services.SavePreRegistrer(pSaldos.correoElectronico, monto, Session["order.id"].ToString());

					//return;
				}
				catch (Exception ex)
				{

					LblMessage.Text = "Ocurrio un error en el preregistro";// +ex.ToString();
					return;
				}

				Response.Redirect(SPContext.Current.Web.Url + "/_layouts/15/RYDEL/Process.aspx");
				//ScriptManager.RegisterStartupScript(this, this.GetType(), "pay", "Checkout.showLightbox();", true);
			}
		}
		#endregion

		#region Métodos privados
		/// <summary>
		/// Habilita el acceso anónimo
		/// </summary>
		protected override bool AllowAnonymousAccess
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Establece la información en pantalla
		/// </summary>
		private void SetData()
		{

			if (Request.QueryString["actualiza"] != null && Request.QueryString["actualiza"] == "1")
				Utils.ActualizaInfoPartida();

			PartidaSaldos pSaldos = Utils.GetPartidaSaldosFromSession();

			LblFechaVencimiento.Text = pSaldos.fechaComercializacion.ToString(@"dd \de MMMM \de yyyy");
			LblNumContrato.Text = pSaldos.folio.ToString();
			LblNumRefrendo.Text = pSaldos.numRefrendo.ToString();

			bool CanPay = Payment(pSaldos.estadoPago); //Dependiendo del estado define si puede hacer pagos o no 

			HfNumTransaccion.Value = pSaldos.transaccion.ToString();
			nombreCliente = pSaldos.nombre + " " + pSaldos.apellidoPaterno + " " + pSaldos.apellidoMaterno;
			bool muestraAbono = false;
			bool muestraDesempenio = false;
			bool muestraRefrendo = false;

			foreach (var item in pSaldos.operacionesBancariasDisponibles.detalleOperacionBancaria)
			{
				switch (item.idOperacion)
				{
					case (long)Constantes.TipoPago.idAbono:
						AmountAbonoMax = item.montoMaximo;
						AmountAbonoMin = item.montoMinimo;

						muestraAbono = true;
						break;
					case (long)Constantes.TipoPago.idDesempenio:
						AmountDesempenio = item.montoMaximo;
						muestraDesempenio = true;
						break;
					case (long)Constantes.TipoPago.idRefrendo:
						AmountRefrendo = item.montoMaximo;
						muestraRefrendo = true;
						break;
					default:
						break;
				}
			}

			TxbDesempenio.Text = AmountDesempenio.ToString("C");
			TxbRefrendo.Text = AmountRefrendo.ToString("C");
			RfvAbonoRange.MinimumValue = AmountAbonoMin.ToString();
			RfvAbonoRange.MaximumValue = AmountAbonoMax.ToString();

			var pantalla = Pantalla.PagosLibres;

			if (pSaldos.aplicarReempeno)
				if (muestraDesempenio && muestraRefrendo && !muestraAbono)
					pantalla = Pantalla.PagosPendientes;

			if (muestraDesempenio && !muestraAbono && !muestraRefrendo)
				pantalla = Pantalla.Reempenio;

			SetPaneles(pantalla, muestraAbono, muestraDesempenio, muestraRefrendo, CanPay);
		}

		/// <summary>
		/// Habilita o deshabilita los diferentes páneles en pantalla
		/// </summary>
		/// <param name="pantalla"></param>
		/// <param name="muestraAbono"></param>
		/// <param name="muestraDesempenio"></param>
		/// <param name="muestraRefrendo"></param>
		private void SetPaneles(Pantalla pantalla, bool muestraAbono, bool muestraDesempenio, bool muestraRefrendo, bool CanPay)
		{
			PnlAbono.Visible = muestraAbono;
			PnlDesempenio.Visible = muestraDesempenio;
			PnlRefrendo.Visible = muestraRefrendo;

			if (PnlDesempenio.Visible && !PnlRefrendo.Visible && !PnlAbono.Visible)
				RbtnDesempenio.Checked = true;

			if (PnlRefrendo.Visible && !PnlDesempenio.Visible && !PnlAbono.Visible)
				RbtnRefrendo.Checked = true;

			switch (pantalla)
			{
				case Pantalla.Reempenio:
					ImgHeader.ImageUrl = "/PublishingImages/images2/ico-reempeno.png";
					DivHeader2.InnerHtml = string.Format(SharePointDA.GetMensaje(Constantes.listConfiguracionMensajes.Registros.CD_Reempenio), nombreCliente);
					break;
				case Pantalla.PagosPendientes:
					ImgHeader.ImageUrl = "/PublishingImages/images2/ico-pagos-pendientes.png";
					DivHeader2.InnerHtml = string.Format(SharePointDA.GetMensaje(Constantes.listConfiguracionMensajes.Registros.CD_PagosPendientes), nombreCliente);
					break;
				case Pantalla.PagosLibres:
					ImgHeader.ImageUrl = "/PublishingImages/images2/ico-pagos-libres.png";
					DivHeader2.InnerHtml = string.Format(SharePointDA.GetMensaje(Constantes.listConfiguracionMensajes.Registros.CD_PagosLibres), nombreCliente);
					break;
				default:
					break;
			}

			//Puede pagar?
			if (CanPay)
				BtnPagar.Enabled = true;
			else
			{
				BtnPagar.Enabled = false;
				LblMessage.Text = "Lo sentimos, existe un pago pendiente";
			}
		}

		/// <summary>
		/// Inicializa el checkout del pago
		/// </summary>
		/// <returns></returns>
		private bool initiateCheckout()
		{
			IDictionary<string, string> resp = new Dictionary<string, string>();
			Merchant merchant = new Merchant();
			ApiOperation op = new ApiOperation();
			double amount = GetPayAmount();
			Session["version"] = SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.version);
			Session["total"] = amount;
			Session["reference"] = GetPayReference();
			// Inicia una sesión con el Payment Gateway
			resp = op.startSession(Session);
			//Guarda los resultados de la sesión para mostrarlos posteriormente en el modo debug
			Session["CreateSession"] = resp;
			Session["sessionRequest"] = op.lastRequest;
			//Si la sesion se creó correctamente, guarde el session ID y la petición
			if (resp.ContainsKey("session.id"))
			{
				//The session was created successfully
				Session["formSession"] = resp["session.id"];
				Session["successIndicator"] = resp["successIndicator"];
				//Session["order.id"] = resp["order.id"];                
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Obtiene la referencia de pago
		/// </summary>
		/// <returns></returns>
		private object GetPayReference()
		{
			int idPago = 0;
			if (RbtnAbono.Checked)
				idPago = (int)Constantes.TipoPago.idAbono;
			if (RbtnDesempenio.Checked)
				idPago = (int)Constantes.TipoPago.idDesempenio;
			if (RbtnRefrendo.Checked)
				idPago = (int)Constantes.TipoPago.idRefrendo;

			return idPago.ToString("000") + HfNumTransaccion.Value + Constantes.codigoBanamex;
		}

		/// <summary>
		/// Obtiene el monto de pago
		/// </summary>
		/// <returns></returns>
		private double GetPayAmount()
		{
			string payAmount = "0";
			if (RbtnAbono.Checked)
				payAmount = TxbAbono.Text;
			if (RbtnDesempenio.Checked)
				payAmount = TxbDesempenio.Text;
			if (RbtnRefrendo.Checked)
				payAmount = TxbRefrendo.Text;

			if (!string.IsNullOrEmpty(payAmount))
				return double.Parse(payAmount.Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol,
						string.Empty));
			return 0;
		}

		/// <summary>
		/// Enumerador de tipo de pantalla
		/// </summary>
		private enum Pantalla
		{
			Reempenio,
			PagosPendientes,
			PagosLibres
		}

		/// <summary>
		/// Dependiendo del Estatis se puede hacer o no Pagos
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		private bool Payment(string status)
		{
			//Estado
			//0=SIN PREREGISTRO
			//1=PENDIENTE
			//2=APROBADO
			//3=RECHAZADO
			//4=SUPERO EL MÁXIMO NO DE INTENTOS
			//5=NO EXISTE REGISTRO EN BANAMEX

			bool CanPay = false;
			switch (status)
			{
				case "":
				case "0":
				case "3":
				case "5":
					CanPay = true;
					break;
				default:
					CanPay = false;
					break;
			}
			return CanPay;
		}
		#endregion
	}
}