using Microsoft.SharePoint;
using RYDEL.modelView;
using RYDEL.modelView.DSRFPagoLinea;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace bSide.NMP.RYDEL.App_Code
{
	/// <summary>
	/// Clase que contiene los métodos a las llamadas del proyecto de servicios
	/// </summary>
	class Services
	{
		/// <summary>
		/// Notifica el pago realizado
		/// </summary>
		/// <param name="receiptcollection"></param>
		/// <returns></returns>
		internal static List<notificarPagoResponseComprobantePDF> NotificaPago(IDictionary<string, string> receiptcollection)
		{
			var np = new List<notificarPagoResponseComprobantePDF>();
			SPSecurity.RunWithElevatedPrivileges(delegate()
			{
				using (SPSite site = new SPSite(SPContext.Current.Site.ID))
				{
					PartidaSaldos ps = Utils.GetPartidaSaldosFromSession();

					string rutacertificado = SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.rutaCertificado);
					string endPoint = SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.endPoint);
					string email = ps.correoElectronico;
					var opPago = new OperacionPago()
					{
						folio = ps.folio,
						idOperacion = Utils.GetIdOperacionFromSession(),
						importe = decimal.Parse(receiptcollection["amount"]),
						importeSpecified = true,
						autorizacion = long.Parse(receiptcollection["transaction[0].transaction.authorizationCode"]),
						autorizacionSpecified = true,
						transaccion = ps.transaccion,
						transaccionBancaria = receiptcollection["id"],
						// Inicio y terminación del número de tarjeta.
						// TODO: Validar obtencion de digitos de la tarjeta.
						numeroTarjeta = receiptcollection["sourceOfFunds.provided.card.number"],
						origen = Constantes.codigoBanamex
					};

					string obj;
					var xmlserializer = new XmlSerializer(typeof(OperacionPago));
					var stringWriter = new StringWriter();
					using (var writer = XmlWriter.Create(stringWriter))
					{
						xmlserializer.Serialize(writer, opPago);
						obj = stringWriter.ToString();
					}


					RydelLog.LogMessage(string.Format("LLamando a servicio consultaDSRFPagoLinea.notificarPago {0} {1} {2}", rutacertificado, endPoint, email, opPago.ToString()));
					consultaDSRFPagoLinea cp = new consultaDSRFPagoLinea();
					cp.notificarPago(rutacertificado, endPoint, opPago, email, out np);
					RydelLog.LogMessage(string.Format("Se ejecutó consultaDSRFPagoLinea.notificarPago {0} {1} comprobantes:{2}", rutacertificado, endPoint, email, np.Count.ToString()));
				}
			});
			return np;
		}

		/// <summary>
		/// Actualiza email del cliente
		/// </summary>
		/// <param name="newEmail"></param>
		/// <returns></returns>
		internal static bool UpdateEmail(string newEmail)
		{
			bool res = false;
			SPSecurity.RunWithElevatedPrivileges(delegate()
			{
				using (SPSite site = new SPSite(SPContext.Current.Site.ID))
				{
					//PartidaSaldos ps = Session["partidaSaldos"].ToString().XmlDeserializeFromString<PartidaSaldos>();
					PartidaSaldos pSaldos = Utils.GetPartidaSaldosFromSession();
					string rutacertificado = SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.rutaCertificado);
					string endPoint = SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.endPointCRM);

					RydelLog.LogMessage(string.Format("LLamando a servicio consultaClientes.updateCliente {0} {1} {2} {3}", rutacertificado, endPoint, newEmail, pSaldos.idCliente.ToString()));
					consultaClientes cs = new consultaClientes();
					res = cs.updateCliente(rutacertificado, endPoint, newEmail, pSaldos.idCliente);
					RydelLog.LogMessage(string.Format("Se ejecutó consultaClientes.updateCliente {0} {1} {2} {3}", rutacertificado, endPoint, newEmail, res.ToString()));
				}
			});
			return res;
		}

		/// <summary>
		/// Obtiene las partidas del cliente
		/// </summary>
		/// <param name="folioPartida"></param>
		/// <returns></returns>
		internal static PartidaSaldos GetPartidaCliente(long folioPartida)
		{
			var ps = new PartidaSaldos();
			SPSecurity.RunWithElevatedPrivileges(delegate()
			{
				using (SPSite site = new SPSite(SPContext.Current.Site.ID))
				{
					string rutacertificado = SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.rutaCertificado);
					string endPoint = SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.endPoint);
					ps = new PartidaSaldos();
					Debug.WriteLine("LLamando a servicio consultaDSRF.getPartidaCliente {0} {1} {2}", rutacertificado, endPoint, folioPartida);
					RydelLog.LogMessage(string.Format("LLamando a servicio consultaDSRF.getPartidaCliente {0} {1} {2}", rutacertificado, endPoint, folioPartida));

					consultaDSRFPagoLinea consultaDSRF = new consultaDSRFPagoLinea();
					consultaDSRF.getPartidaCliente(rutacertificado, endPoint, folioPartida, out ps);

					string obj;
					var xmlserializer = new XmlSerializer(typeof(PartidaSaldos));
					var stringWriter = new StringWriter();
					using (var writer = XmlWriter.Create(stringWriter))
					{
						xmlserializer.Serialize(writer, ps);
						obj = stringWriter.ToString();
					}

					RydelLog.LogMessage(string.Format("Se ejecutó consultaDSRF.getPartidaCliente {0} {1}", folioPartida.ToString(), obj));
				}
			});
			return ps;
		}

		/// <summary>
		/// Crear el preregistro
		/// </summary>
		/// <param name="transaccionBancaria"></param>
		/// <param name="correoElectronico"></param>
		/// <param name="monto"></param>
		/// <param name="trBancaria"></param>
		/// <returns></returns>
		internal static string SavePreRegistrer(string correoElectronico, decimal monto, string trBancaria)
		{
			string rutacertificado = string.Empty;
			string endPoint = string.Empty;

			PartidaSaldos ps = Utils.GetPartidaSaldosFromSession();
			SPSecurity.RunWithElevatedPrivileges(delegate()
			{
				using (SPSite site = new SPSite(SPContext.Current.Site.ID))
				{
					rutacertificado = SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.rutaCertificado);
					endPoint = SharePointDA.GetParametro(Constantes.listConfiguracionPago.Registros.endPoint);
				}
			});

			var o = new OperacionPago()
			{
				folio = ps.folio,
				idOperacion = Utils.GetIdOperacionFromSession(),
				importeSpecified = true,
				autorizacionSpecified = true,
				transaccion = ps.transaccion,
				transaccionBancaria = trBancaria,
				origen = Constantes.codigoBanamex
			};


			preregistroPagosTypeTransaccionPago tr = new preregistroPagosTypeTransaccionPago();
			tr.banco = new preregistroPagoTypeBanco { transaccionBancaria = trBancaria };
			tr.cliente = new preregistroPagoTypeCliente { correoElectronico = correoElectronico };
			preregistroPagoTypePartida[] dd = new preregistroPagoTypePartida[1];
			dd[0] = new preregistroPagoTypePartida { folio = (int)o.folio, idOperacion = (int)o.idOperacion, monto = monto, numeroTransaccion = o.transaccion.ToString(), origen = o.origen };
			tr.partidas = dd;



			consultaDSRFPagoLinea Consulta = new consultaDSRFPagoLinea();


			return Consulta.SetPreReg(rutacertificado, endPoint, tr);

		}

	}
}
