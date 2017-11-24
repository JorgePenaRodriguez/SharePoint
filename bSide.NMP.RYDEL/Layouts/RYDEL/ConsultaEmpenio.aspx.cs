using System;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using bSide.NMP.RYDEL.App_Code;
using System.Web;
using Microsoft.SharePoint;
using RYDEL.modelView;
using System.ServiceModel;
using RYDEL.modelView.DSRFPagoLinea;
using System.Diagnostics;
using System.Web.SessionState;

namespace bSide.NMP.RYDEL.Layouts.bSide.NMP.RYDEL
{
	public partial class ConsultaEmpenio : UnsecuredLayoutsPageBase
	{
		#region Eventos
		protected void Page_Load(object sender, EventArgs e)
		{
			LblMessage.Text = string.Empty;

			if (!IsPostBack)
			{
				try
				{
					SetPanels(Pantalla.Inicio);
					SetCaptchaText();

					if (Request.QueryString["actualiza"] != null && Request.QueryString["actualiza"] == "1")
					{
						Utils.ActualizaInfoPartida();
						ValidaAdeudos();
					}
				}

				catch (Exception ex)
				{
					RydelLog.LogError(ex, "Error en carga");
					LblMessage.Text = Constantes.mensajeError;
				}
			}
		}

		protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
		{
			try
			{
				SetCaptchaText();
				txtCaptcha.Focus();
			}
			catch (Exception ex)
			{
				RydelLog.LogError(ex, "Error al refrescar captcha");
				LblMessage.Text = Constantes.mensajeError;
			}
		}

		protected void BtnAceptar_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(HfContrato.Value))
				{
					if (!txtOculto.Value.Equals(txtCaptcha.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
					{
						LblMessage.Text = "El Captcha, no es correcto";
					}
					else
					{
						ValidaContrato();
					}
				}
			}
			catch (Exception ex)
			{
				RydelLog.LogError(ex, "Error al consultar contrato");
				LblMessage.Text = Constantes.mensajeError;
			}
		}

		protected void LbtnEditarEmail_Click(object sender, EventArgs e)
		{
			try
			{
				SetPanels(Pantalla.EditarEmail);
			}
			catch (Exception ex)
			{
				RydelLog.LogError(ex, "Error al mostrar edición email");
				LblMessage.Text = Constantes.mensajeError;
			}
		}

		protected void BtnEditarEmail_Click(object sender, EventArgs e)
		{
			try
			{
				var newEmail = TxbEmail.Text.Trim();
				bool success = Services.UpdateEmail(newEmail);

				if (success)
				{
					//Actualiza entidad en sesión con el nuevo email
					PartidaSaldos ps = Utils.GetPartidaSaldosFromSession();
					ps.correoElectronico = newEmail;
					Utils.SetPartidaSaldosToSession(ps);

					LblEmail.Text = newEmail;
					TxbEmail.Text = newEmail;
					SetPanels(Pantalla.VerificaDatos);
					LblMessage.Text = "El correo electrónico se ha guardado";
				}
				else
				{
					LblMessage.Text = "El correo electrónico no se ha podido guardar";
				}
			}
			catch (Exception ex)
			{
				RydelLog.LogError(ex, "Error al editar email");
				LblMessage.Text = Constantes.mensajeError;
			}
		}

		protected void BtnSiguiente_Click(object sender, EventArgs e)
		{
			try
			{
				ValidaAdeudos();
			}
			catch (Exception ex)
			{
				RydelLog.LogError(ex, "Error al validar adeudos");
				LblMessage.Text = Constantes.mensajeError;
			}
		}
		#endregion

		#region Métodos privados

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

		/// <summary>
		/// Establece Captcha en control
		/// </summary>
		private void SetCaptchaText()
		{
			Random oRandom = new Random();
			int iNumber = oRandom.Next(10000, 99999);
			txtOculto.Value = iNumber.ToString();
			txtCaptcha.Text = string.Empty;
			Byte[] bytes = Utils.CrearImagen(iNumber.ToString());
			string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
			imgCaptcha.ImageUrl = "data:image/jpeg;base64," + base64String;
		}

		/// <summary>
		/// Valida el contrato por número
		/// </summary>
		private void ValidaContrato()
		{
			long folioPartida = 0;
			folioPartida = long.Parse(TxbContrato.Text.Trim());
			var partidaSaldos = new PartidaSaldos();
			var valido = false;
			string faultCode = string.Empty;

			try
			{

#if RELEASE
                partidaSaldos = SetPartidaSaldosDummy(folioPartida);
                valido = true;
#else
				partidaSaldos = Services.GetPartidaCliente(folioPartida);
				Utils.SetPartidaSaldosToSession(partidaSaldos);
				//Session["partidaSaldos"] = obj;
				valido = true;
#endif

			}

			catch (System.Web.Services.Protocols.SoapException soapEx)
			{
				RydelLog.LogError(soapEx, "Error al consultar servicio, fault code: " + soapEx.Code);

				//if (soapEx.Detail.InnerText.Trim().Contains("NMP-220"))
				//{
				//	HfContrato.Value = string.Empty;
				//	faultCode = "NMP-220";
				//	//SetPanels(Pantalla.DatosErroneos, faultCode);
				//}



				if (soapEx.Code != null && !string.IsNullOrEmpty(soapEx.Code.Name))
					faultCode = soapEx.Code.Name.Trim();

				if (soapEx.Detail.InnerText.Trim().Contains("NMP-220"))
					faultCode = "NMP-220";

			}

			catch (Exception ex)
			{
				Debug.WriteLine("Error al consultar servicio: " + ex);
				RydelLog.LogError(ex, "Error al consultar servicio");
			}

			if (valido)
			{
				if (string.IsNullOrEmpty(partidaSaldos.correoElectronico))
					SetPanels(Pantalla.EditarEmail);
				else
					SetPanels(Pantalla.VerificaDatos);

				//HfPrestamoPagado.Value = partidaSaldos.prestamoPagado.ToString();
				HfContrato.Value = TxbContrato.Text.Trim();
				LblNumeroContrato.Text = TxbContrato.Text.Trim();
				LblCliente.Text = Utils.GetMascaraNombreCliente(partidaSaldos.nombre + " " + partidaSaldos.apellidoPaterno + " " + partidaSaldos.apellidoMaterno);
				LblEmail.Text = partidaSaldos.correoElectronico;
				TxbEmail.Text = partidaSaldos.correoElectronico;
			}
			else
			{
				HfContrato.Value = string.Empty;
				SetPanels(Pantalla.DatosErroneos, faultCode);
			}
		}

		private PartidaSaldos SetPartidaSaldosDummy(long folioPartida)
		{
			PartidaSaldos partidaSaldos = new PartidaSaldos();
			partidaSaldos.apellidoMaterno = "Lopez";
			partidaSaldos.apellidoPaterno = "Perez";
			partidaSaldos.aplicarReempeno = true;
			partidaSaldos.correoElectronico = "test@123.com";
			partidaSaldos.fechaComercializacion = DateTime.Now;
			partidaSaldos.folio = folioPartida;
			partidaSaldos.idCliente = 123;
			partidaSaldos.nombre = "Juan";
			partidaSaldos.numRefrendo = 5;
			partidaSaldos.prestamoPagado = false;
			partidaSaldos.sucursal = 1;
			partidaSaldos.tipoContrato = "tipo1";
			partidaSaldos.transaccion = 123;
			var ob = new OperacionesBancarias();
			var ob1 = new OperacionesBancariasDetalleOperacionBancaria();
			ob1.idOperacion = 6;
			ob1.montoMaximo = 10000;
			ob1.montoMinimo = 1;
			var ob2 = new OperacionesBancariasDetalleOperacionBancaria();
			ob2.idOperacion = 8;
			ob2.montoMaximo = 1000;
			ob2.montoMinimo = 1;
			var ob3 = new OperacionesBancariasDetalleOperacionBancaria();
			ob3.idOperacion = 8;
			ob3.montoMaximo = 10000;
			ob3.montoMinimo = 1;
			ob.detalleOperacionBancaria = new System.Collections.Generic.List<OperacionesBancariasDetalleOperacionBancaria>();
			ob.detalleOperacionBancaria.Add(ob1);
			ob.detalleOperacionBancaria.Add(ob2);
			ob.detalleOperacionBancaria.Add(ob3);
			partidaSaldos.operacionesBancariasDisponibles = ob;
			return partidaSaldos;
		}

		/// <summary>
		/// Método que valida si existen adeudos para el contrato
		/// </summary>
		private void ValidaAdeudos()
		{
			bool prestamoPagado = false;
			var ps = Utils.GetPartidaSaldosFromSession();
			if (ps == null)
				return;

			prestamoPagado = ps.prestamoPagado;

			if (prestamoPagado)
			{
				SetPanels(Pantalla.NoAdeudos);
				HfContrato.Value = string.Empty;
				txtCaptcha.Text = string.Empty;
			}
			else
			{
				//Redirecciona a página ConsultaDatosRyD
				Response.Redirect(SPContext.Current.Web.Url + "/_layouts/15/RYDEL/ConsultaDatosRyD.aspx", false);
				Context.ApplicationInstance.CompleteRequest();
			}
		}

		/// <summary>
		/// Establece los paneles visibles en la pantalla
		/// </summary>
		/// <param name="pantalla"></param>
		private void SetPanels(Pantalla pantalla, string faultCode = "")
		{
			switch (pantalla)
			{
				case Pantalla.Inicio:
					DivHeader2.InnerHtml = SharePointDA.GetMensaje(Constantes.listConfiguracionMensajes.Registros.CE_CapturaDeDatos);
					ImgHeader.ImageUrl = "/PublishingImages/images2/pago-en-linea.jpg";
					LblMessage.Text = string.Empty;
					PnlContrato.Visible = true;
					PnlCaptcha.Visible = true;
					PnlVerifica.Visible = false;
					PnlMostrarEmail.Visible = false;
					PnlEditarEmail.Visible = false;
					PnlNoAdeudo.Visible = false;
					PnlBtnAceptar.Visible = true;
					PnlBtnSiguiente.Visible = false;
					break;
				case Pantalla.VerificaDatos:
					DivHeader2.InnerHtml = SharePointDA.GetMensaje(Constantes.listConfiguracionMensajes.Registros.CE_VerificaDatos);
					ImgHeader.ImageUrl = "/PublishingImages/images2/pago-en-linea.jpg";
					LblMessage.Text = "Valida datos.";
					PnlContrato.Visible = false;
					PnlCaptcha.Visible = false;
					PnlVerifica.Visible = true;
					PnlMostrarEmail.Visible = true;
					PnlEditarEmail.Visible = false;
					PnlNoAdeudo.Visible = false;
					PnlBtnAceptar.Visible = false;
					PnlBtnSiguiente.Visible = true;
					break;
				case Pantalla.EditarEmail:
					DivHeader2.InnerHtml = SharePointDA.GetMensaje(Constantes.listConfiguracionMensajes.Registros.CE_VerificaDatos);
					ImgHeader.ImageUrl = "/PublishingImages/images2/pago-en-linea.jpg";
					LblMessage.Text = "Valida datos.";
					PnlContrato.Visible = false;
					PnlCaptcha.Visible = false;
					PnlVerifica.Visible = true;
					PnlMostrarEmail.Visible = false;
					PnlEditarEmail.Visible = true;
					PnlNoAdeudo.Visible = false;
					PnlBtnAceptar.Visible = false;
					PnlBtnSiguiente.Visible = false;
					break;
				case Pantalla.DatosErroneos:
					string faultMsg = string.Empty;
					if (!string.IsNullOrEmpty(faultCode))
						faultMsg = SharePointDA.GetMensaje(faultCode);
					DivHeader2.InnerHtml = string.IsNullOrEmpty(faultMsg) ? SharePointDA.GetMensaje(
							Constantes.listConfiguracionMensajes.Registros.CE_CapturaDeDatosErronea) : faultMsg;
					ImgHeader.ImageUrl = "/PublishingImages/images2/ico-wrong.png";
					LblMessage.Text = "Captura de datos.";
					PnlContrato.Visible = true;
					PnlCaptcha.Visible = true;
					PnlVerifica.Visible = false;
					PnlMostrarEmail.Visible = false;
					PnlEditarEmail.Visible = false;
					PnlNoAdeudo.Visible = false;
					PnlBtnAceptar.Visible = true;
					PnlBtnSiguiente.Visible = false;
					break;
				case Pantalla.NoAdeudos:
					DivHeader2.InnerHtml = SharePointDA.GetMensaje(Constantes.listConfiguracionMensajes.Registros.CE_NoAdeudos);
					ImgHeader.ImageUrl = "/PublishingImages/images2/ico-ok.png";
					LblMessage.Text = string.Empty;
					PnlContrato.Visible = true;
					PnlCaptcha.Visible = true;
					PnlVerifica.Visible = false;
					PnlMostrarEmail.Visible = false;
					PnlEditarEmail.Visible = false;
					PnlNoAdeudo.Visible = true;
					PnlBtnAceptar.Visible = true;
					PnlBtnSiguiente.Visible = false;
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Enumerador de tipos de pantallas
		/// </summary>
		private enum Pantalla
		{
			Inicio,
			VerificaDatos,
			EditarEmail,
			DatosErroneos,
			NoAdeudos,
		}
		#endregion
	}
}
