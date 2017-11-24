using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    public class consultaClientes
    {
        const string TIPOEMAIL = "RF y DL";
        /// <summary>
        /// Método para actualizar los datos del cliente
        /// </summary>
        /// <param name="pRutaCertificado">Ruta del certficado</param>
        /// <param name="pEndPoint">Url del servicio</param>
        /// <param name="correoElectronico">Nuevo correo electrónico del cliente</param>
        /// <param name="idCliente">Id del cliente al que se actualizará el correo electrónico</param>
        /// <returns>Booleano </returns>
        public bool updateCliente(string pRutaCertificado, string pEndPoint, string correoElectronico, long idCliente)
        {
            //int i = 0;
            string resultado = "";
            Clientes.updateClienteRequestType clienteCRM = new Clientes.updateClienteRequestType();
            Clientes.updateClienteRequestTypeCorreoElectronico[] CorreosElectronicos = new Clientes.updateClienteRequestTypeCorreoElectronico[1];
            clienteCRM.Contacto = new Clientes.updateClienteRequestTypeContacto();

            CorreosElectronicos[0] = new Clientes.updateClienteRequestTypeCorreoElectronico();
            CorreosElectronicos[0].email = correoElectronico;
            CorreosElectronicos[0].tipoEmail = TIPOEMAIL;
            
            clienteCRM.CorreosElectronicos = CorreosElectronicos;

            Clientes.Clientes clienteSvc = new Clientes.Clientes();
            Clientes.HeaderMessage headerMessage = new Clientes.HeaderMessage();
            headerMessage.idConsumidor = "7";
						//Usuario y idDestino 
						headerMessage.idDestino = "";
						headerMessage.usuario = "";
					clienteSvc.headerMessage = headerMessage;

            clienteSvc.Url = pEndPoint;
            if (pEndPoint.StartsWith("https:"))
            {
                certifica cert = new certifica();
                if (pRutaCertificado == "") { pRutaCertificado = "C:\\certificado_omega_oaesb\\oaesb.cer"; }
                cert.configuraSSL(pRutaCertificado, clienteSvc);
            }

            clienteCRM.idCliente = idCliente;
            clienteCRM.idClienteSpecified = true;

            Clientes.StandardResponseType resp = clienteSvc.updateCliente(clienteCRM);

            resultado = (resp != null) ? resp.respuesta : "";

            return (resultado == "Actualización Correcta");
        }
    }
}
