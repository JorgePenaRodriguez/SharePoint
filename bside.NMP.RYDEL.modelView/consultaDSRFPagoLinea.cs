using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace RYDEL.modelView
{
	public class consultaDSRFPagoLinea
	{
		/// <summary>
		/// Método para obtener los datos de la contrato o partida del cliente
		/// </summary>
		/// <param name="pRutaCertificado">Ruta donde se encuentra el certificado</param>
		/// <param name="pEndPoint">URL del servicio web</param>
		/// <param name="folioPartida">Número de contrado</param>
		/// <param name="partidaSaldos">Retorno de datos de la partida</param>
		/// <returns>Cadena de respuesta</returns>
		public string getPartidaCliente(string pRutaCertificado, string pEndPoint, long folioPartida, out PartidaSaldos partidaSaldos)
		{
			string respuesta = string.Empty;

			DSRFPagoLinea.DSRFPagoLinea pagoLinea = new DSRFPagoLinea.DSRFPagoLinea();
			pagoLinea.Url = pEndPoint;

			if (pEndPoint.StartsWith("https:"))
			{
				certifica cert = new certifica();
				if (pRutaCertificado == "") { pRutaCertificado = "C:\\certificado_omega_oaesb\\oaesb.cer"; }
				cert.configuraSSL(pRutaCertificado, pagoLinea);
			}
			DSRFPagoLinea.HeaderMessage headerMessage = new DSRFPagoLinea.HeaderMessage();
			headerMessage.idConsumidor = "7";
			pagoLinea.headerMessage = headerMessage;
			DSRFPagoLinea.PartidaSaldosType partidaSaldosType = new DSRFPagoLinea.PartidaSaldosType();
			partidaSaldos = new PartidaSaldos();
			string nombre, apPaterno, apMaterno, correo = String.Empty;

			respuesta = pagoLinea.getPartidaCliente(folioPartida, out nombre, out apPaterno, out apMaterno, out correo, out partidaSaldosType);

			if (partidaSaldosType != null)
			{
				partidaSaldos.aplicarReempeno = partidaSaldosType.aplicarReempeno;
				partidaSaldos.fechaComercializacion = partidaSaldosType.fechaComercializacion;
				partidaSaldos.folio = partidaSaldosType.folio;
				partidaSaldos.idCliente = partidaSaldosType.idCliente;
				partidaSaldos.numRefrendo = partidaSaldosType.numRefrendo;
				partidaSaldos.estadoPago = partidaSaldosType.estadoPago;
				partidaSaldos.nombre = nombre;
				partidaSaldos.apellidoPaterno = apPaterno;
				partidaSaldos.apellidoMaterno = apMaterno;
				partidaSaldos.correoElectronico = correo;

				List<OperacionesBancariasDetalleOperacionBancaria> detalleOperacion = new List<OperacionesBancariasDetalleOperacionBancaria>();
				for (int i = 0; i < partidaSaldosType.operacionesBancariasDisponibles.Count(); i++)
				{
					detalleOperacion.Add(new OperacionesBancariasDetalleOperacionBancaria
					{
						idOperacion = partidaSaldosType.operacionesBancariasDisponibles[i].idOperacion,
						montoMaximo = partidaSaldosType.operacionesBancariasDisponibles[i].montoMaximo,
						montoMinimo = partidaSaldosType.operacionesBancariasDisponibles[i].montoMinimo,
						nombreOperacion = partidaSaldosType.operacionesBancariasDisponibles[i].nombreOperacion
					});
				}

				partidaSaldos.operacionesBancariasDisponibles = new OperacionesBancarias();
				partidaSaldos.operacionesBancariasDisponibles.detalleOperacionBancaria = detalleOperacion;
				partidaSaldos.prestamoPagado = partidaSaldosType.prestamoPagado;
				partidaSaldos.sucursal = partidaSaldosType.sucursal;
				partidaSaldos.tipoContrato = partidaSaldosType.tipoContrato;
				partidaSaldos.transaccion = partidaSaldosType.transaccion;
			}
			return respuesta;
		}

		public string notificarPago(string pRutaCertificado, string pEndPoint, OperacionPago pOperacionPago, string pCorreoElectronico, out List<notificarPagoResponseComprobantePDF> pPDF)
		{
			string respuesta = "";
			pPDF = new List<notificarPagoResponseComprobantePDF>();

			DSRFPagoLinea.DSRFPagoLinea pagoLinea = new DSRFPagoLinea.DSRFPagoLinea();
			pagoLinea.Url = pEndPoint;

			if (pEndPoint.StartsWith("https:"))
			{
				certifica cert = new certifica();
				if (pRutaCertificado == "") { pRutaCertificado = "C:\\certificado_omega_oaesb\\oaesb.cer"; }
				cert.configuraSSL(pRutaCertificado, pagoLinea);
			}

			DSRFPagoLinea.FileType[] pdf;

			DSRFPagoLinea.HeaderMessage headerMessage = new DSRFPagoLinea.HeaderMessage();
			headerMessage.idConsumidor = "7";
			pagoLinea.headerMessage = headerMessage;

			DSRFPagoLinea.OperacionPagoType opPagoType = new DSRFPagoLinea.OperacionPagoType();
			opPagoType.autorizacion = pOperacionPago.autorizacion;
			opPagoType.autorizacionSpecified = true;
			opPagoType.numeroTarjeta = pOperacionPago.numeroTarjeta;
			opPagoType.folio = pOperacionPago.folio;
			opPagoType.idOperacion = pOperacionPago.idOperacion;
			opPagoType.importe = pOperacionPago.importe;
			opPagoType.importeSpecified = true;
			opPagoType.origen = pOperacionPago.origen;
			opPagoType.transaccion = pOperacionPago.transaccion;
			opPagoType.transaccionBancaria = pOperacionPago.transaccionBancaria;

			string obj;
			var xmlserializer = new XmlSerializer(typeof(DSRFPagoLinea.OperacionPagoType));
			var stringWriter = new StringWriter();
			using (var writer = XmlWriter.Create(stringWriter))
			{
				xmlserializer.Serialize(writer, opPagoType);
				obj = stringWriter.ToString();
			}


			respuesta = pagoLinea.notificarPago(opPagoType, pCorreoElectronico, out pdf);


			for (int i = 0; i < pdf.Count(); i++)
			{
				pPDF.Add(new notificarPagoResponseComprobantePDF
				{
					idOperacion = pdf[i].idOperacion,
					archivo = pdf[i].archivo
				});
			}

			//pPDF.idOperacion = pdf.idOperacion;
			//pPDF.archivo = pdf.archivo;

			return respuesta;
		}

		/// <summary>
		/// Preregistro
		/// </summary>
		/// <param name="Transaccion"></param>
		/// <returns></returns>
		public string SetPreReg(string pRutaCertificado, string pEndPoint, DSRFPagoLinea.preregistroPagosTypeTransaccionPago Transaccion)
		{

			DSRFPagoLinea.DSRFPagoLinea pagoLinea = new DSRFPagoLinea.DSRFPagoLinea();
			pagoLinea.Url = pEndPoint;

			if (pEndPoint.StartsWith("https:"))
			{
				certifica cert = new certifica();
				if (pRutaCertificado == "") { pRutaCertificado = "C:\\certificado_omega_oaesb\\oaesb.cer"; }
				cert.configuraSSL(pRutaCertificado, pagoLinea);
			}


			//DSRFPagoLinea.DSRFPagoLinea pagoLinea = new DSRFPagoLinea.DSRFPagoLinea();

			XmlSerializer xsSubmit = new XmlSerializer(typeof(DSRFPagoLinea.preregistroPagosTypeTransaccionPago));
			var xml = "";
			using (var sww = new StringWriter())
			{
				using (XmlWriter writer = XmlWriter.Create(sww))
				{
					xsSubmit.Serialize(writer, Transaccion);
					xml = sww.ToString(); // Your XML
				}
			}

			//try
			//{
				DSRFPagoLinea.responseTypeRespuesta Res = pagoLinea.preregistrarPago(Transaccion);
				return Res.mensaje;
			//}
			//catch (Exception ex)
			//{
			//	return xml + ex.ToString() + ex.InnerException +  ex.Data.ToString() ;
			//	//throw;
			//}
			//Pasar los datos al WS
			

			
		}
	}
}

