using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    public class consultaAdminPagos
    {
        public string setOperacion(string pRutaCertificado, string pEndPoint, OperacionPago pOperacion, out string pMessage, out string pDetail)
        {
            string respuesta = "";
            pMessage = "";
            pDetail = "";

            AdminPagos.AdminPagos admPagos = new AdminPagos.AdminPagos();
            admPagos.Url = pEndPoint;

            if (pEndPoint.StartsWith("https:"))
            {
                certifica cert = new certifica();
                if (pRutaCertificado == "") { pRutaCertificado = "C:\\certificado_omega_oaesb\\oaesb.cer"; }
                cert.configuraSSL(pRutaCertificado, admPagos);
            }

            AdminPagos.HeaderMessage headerMessage = new AdminPagos.HeaderMessage();
            headerMessage.idConsumidor = "7";
            admPagos.headerMessage = headerMessage;

            AdminPagos.OperacionPagoType operacion = new AdminPagos.OperacionPagoType();
            operacion.autorizacion = pOperacion.autorizacion;
            operacion.autorizacionSpecified = true;
						operacion.numeroTarjeta = pOperacion.numeroTarjeta;
            operacion.folio = pOperacion.folio;
            operacion.idOperacion = pOperacion.idOperacion;
            operacion.importe = pOperacion.importe;
            operacion.importeSpecified = true;
            operacion.origen = pOperacion.origen;
            operacion.transaccion = pOperacion.transaccion;
            operacion.transaccionBancaria = pOperacion.transaccionBancaria;

            respuesta = admPagos.setOperacion(operacion, out pMessage, out pDetail); 
            
            return respuesta;
        }

        public PartidaSaldos getPartida(string pRutaCertificado, string pEndPoint, long pFolio)
        {
            PartidaSaldos partidaSaldos = null;
            AdminPagos.PartidaSaldosType partidaSaldosType = new AdminPagos.PartidaSaldosType();

            AdminPagos.AdminPagos admPagos = new AdminPagos.AdminPagos();
            admPagos.Url = pEndPoint;

            if (pEndPoint.StartsWith("https:"))
            {
                certifica cert = new certifica();
                if (pRutaCertificado == "") { pRutaCertificado = "C:\\certificado_omega_oaesb\\oaesb.cer"; }
                cert.configuraSSL(pRutaCertificado, admPagos);
            }

            AdminPagos.HeaderMessage headerMessage = new AdminPagos.HeaderMessage();
            headerMessage.idConsumidor = "7";
            admPagos.headerMessage = headerMessage;

            partidaSaldosType = admPagos.getPartida(pFolio);

            if (partidaSaldosType != null)
            {

                partidaSaldos = new PartidaSaldos();

                partidaSaldos.aplicarReempeno = partidaSaldosType.aplicarReempeno;
                partidaSaldos.fechaComercializacion = partidaSaldosType.fechaComercializacion;
                partidaSaldos.folio = partidaSaldosType.folio;
                partidaSaldos.idCliente = partidaSaldosType.idCliente;
                partidaSaldos.numRefrendo = partidaSaldosType.numRefrendo;
								partidaSaldos.estadoPago = partidaSaldosType.estadoPago;

                List<OperacionesBancariasDetalleOperacionBancaria> detalleOperacion = new List<OperacionesBancariasDetalleOperacionBancaria>();
                for (int i = 0; i < partidaSaldosType.operacionesBancariasDisponibles.Count(); i++)
                {
                    detalleOperacion.Add(
                        new OperacionesBancariasDetalleOperacionBancaria {
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

            return partidaSaldos;
        }

        public List<notificarPagoResponseComprobantePDF> getComprobantes(string pRutaCertificado, string pEndPoint, long pSerieImporte)
        {
            List<notificarPagoResponseComprobantePDF> respArchivo = new List<notificarPagoResponseComprobantePDF>();
            
            AdminPagos.AdminPagos admPagos = new AdminPagos.AdminPagos();
            admPagos.Url = pEndPoint;

            if (pEndPoint.StartsWith("https:"))
            {
                certifica cert = new certifica();
                if (pRutaCertificado == "") { pRutaCertificado = "C:\\certificado_omega_oaesb\\oaesb.cer"; }
                cert.configuraSSL(pRutaCertificado, admPagos);
            }

            AdminPagos.HeaderMessage headerMessage = new AdminPagos.HeaderMessage();
            headerMessage.idConsumidor = "7";
            admPagos.headerMessage = headerMessage;

             
            var archivo = admPagos.getComprobantes(pSerieImporte); 
            

            if (archivo != null)
            {
                for (int i = 0; i < archivo.Count(); i++)
                {
                    respArchivo.Add(new notificarPagoResponseComprobantePDF
                    {
                        idOperacion = archivo[i].idOperacion,
                        archivo = archivo[i].archivo
                    });
                }
            }
            else
            {
                return null;
            }

            return respArchivo;
        }
    }
}
