using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelView
{
    public class consultaClientes
    {
        public Cliente getClientesByNumCredencial(string pRutaCertificado, string pEndPoint, string pNumCredencial)
        {
            Clientes.Clientes clienteSvc = new Clientes.Clientes();
            Clientes.HeaderMessage headerMessage = new Clientes.HeaderMessage();
            headerMessage.idConsumidor = "03";
            clienteSvc.headerMessage = headerMessage;

            clienteSvc.Url = pEndPoint;
            if (pEndPoint.StartsWith("https:"))
            {
                certifica cert = new certifica();
                if (pRutaCertificado == "") { pRutaCertificado = "C:\\certificado_omega_oaesb\\oaesb.cer"; }
                cert.configuraSSL(pRutaCertificado, clienteSvc);
            }

            Clientes.getClientesResponseType clienteCRM = new Clientes.getClientesResponseType();
            Clientes.getClientesByIDRequestType clienteCredencial = new Clientes.getClientesByIDRequestType();

            try
            {
                clienteCredencial.idCliente = Convert.ToInt64(pNumCredencial);
                clienteCRM = clienteSvc.getClientesByID(clienteCredencial);
            }
            catch (Exception ex)
            {
                throw;
            }

            modelView.Cliente cliente = new modelView.Cliente();

            if (clienteCRM.Cliente != null)
            {
                List<modelView.Beneficiario> listaBeneficiarios = new List<modelView.Beneficiario>();
                modelView.Contacto contacto = new modelView.Contacto();
                modelView.CorreoElectronico email = new modelView.CorreoElectronico();

                cliente.apellidoMaterno = clienteCRM.Cliente.apellidoMaterno;
                cliente.apellidoPaterno = clienteCRM.Cliente.apellidoPaterno;

                foreach (Clientes.getClientesResponseTypeClienteBeneficiario beneficiaroCRM in clienteCRM.Cliente.Beneficiarios)
                {
                    modelView.Beneficiario beneficiario = new modelView.Beneficiario();
                    beneficiario.ApellidoMaterno = beneficiaroCRM.apellidoMaterno;
                    beneficiario.ApellidoPaterno = beneficiaroCRM.apellidoPaterno;
                    beneficiario.Nombre = beneficiaroCRM.nombre;
                    listaBeneficiarios.Add(beneficiario);
                }
                cliente.Beneficiarios = listaBeneficiarios;

                List<modelView.Telefono> listaTelefonos = new List<modelView.Telefono>();
                List<modelView.Domicilio> listaDirecciones = new List<modelView.Domicilio>();
                if (clienteCRM.Cliente.Contacto.ListaTelefonos != null)
                {
                    foreach (Clientes.TelefonoType telefonoCRM in clienteCRM.Cliente.Contacto.ListaTelefonos)
                    {
                        modelView.Telefono telefono = new modelView.Telefono();
                        telefono.codigoArea = telefonoCRM.codigoArea;
                        telefono.extension = telefonoCRM.extension;
                        telefono.numeroTelefonico = telefonoCRM.numeroTelefonico;
                        telefono.tipoTelefono = telefonoCRM.tipoTelefono;
                        listaTelefonos.Add(telefono);
                    }
                }

                if (clienteCRM.Cliente.Contacto.ListaDirecciones != null)
                {
                    foreach (Clientes.DireccionType direccionCRM in clienteCRM.Cliente.Contacto.ListaDirecciones)
                    {
                        modelView.Domicilio direccion = new modelView.Domicilio();
                        direccion.ciudadPoblacion = direccionCRM.ciudadPoblacion;

                        direccion.idCodigoPostal = direccionCRM.idCodigoPostal;
                        direccion.codigoPostal = direccionCRM.codigoPostal;

                        direccion.colonia = direccionCRM.colonia;
                        direccion.idColonia = direccionCRM.idColonia;

                        direccion.delegacionMunicipio = direccionCRM.delegacionMunicipio;
                        direccion.idDelegacionMunicipio = direccionCRM.idDelegacionMunicipio;

                        direccion.entreCalle1 = direccionCRM.entreCalle1;
                        direccion.entreCalle2 = direccionCRM.entreCalle2;
                        direccion.estado = direccionCRM.estado;
                        direccion.idEstado = direccionCRM.idEstado;

                        direccion.idEstadoSpecified = direccionCRM.idEstadoSpecified;
                        direccion.nombreDelaCalle = direccionCRM.nombreDelaCalle;
                        direccion.numeroExterior = direccionCRM.numeroExterior;
                        direccion.numeroInterior = direccionCRM.numeroInterior;
                        direccion.tipoDomicilio = direccionCRM.tipoDomicilio;
                        listaDirecciones.Add(direccion);
                    }
                }

                cliente.Contacto = new modelView.Contacto();

                cliente.Contacto.ListaDirecciones = listaDirecciones;
                cliente.Contacto.ListaTelefonos = listaTelefonos;

                List<modelView.CorreoElectronico> listaCorreos = new List<modelView.CorreoElectronico>();
                if (clienteCRM.Cliente.CorreosElectronicos != null)
                {
                    foreach (Clientes.getClientesResponseTypeClienteCorreoElectronico correoCRM in clienteCRM.Cliente.CorreosElectronicos)
                    {
                        modelView.CorreoElectronico correo = new modelView.CorreoElectronico();
                        correo.email = correoCRM.email;
                        correo.tipoEmail = correoCRM.tipoEmail;
                        listaCorreos.Add(correo);
                    }
                    cliente.CorreosElectronicos = listaCorreos;
                }

                modelView.Cotitular cotitular = new modelView.Cotitular();
                cotitular.apellidoMaterno = clienteCRM.Cliente.Cotitular.apellidoMaterno;
                cotitular.apellidoPaterno = clienteCRM.Cliente.Cotitular.apellidoPaterno;
                cotitular.nombre = clienteCRM.Cliente.Cotitular.nombre;
                List<modelView.Telefono> listaTelefonosCotitular = new List<modelView.Telefono>();
                if (clienteCRM.Cliente.Cotitular.ListaTelefonos != null)
                {
                    foreach (Clientes.TelefonoType telefonoCotitularCRM in clienteCRM.Cliente.Cotitular.ListaTelefonos)
                    {
                        modelView.Telefono telefonoCotitular = new modelView.Telefono();
                        telefonoCotitular.codigoArea = telefonoCotitularCRM.codigoArea;
                        telefonoCotitular.extension = telefonoCotitularCRM.extension;
                        telefonoCotitular.numeroTelefonico = telefonoCotitularCRM.numeroTelefonico;
                        telefonoCotitular.tipoTelefono = telefonoCotitularCRM.tipoTelefono;
                        listaTelefonosCotitular.Add(telefonoCotitular);
                    }
                    cotitular.ListaTelefonos = listaTelefonosCotitular;
                }

                cliente.Cotitular = cotitular;
                cliente.email = clienteCRM.Cliente.email;
                cliente.fechaDeNacimiento = clienteCRM.Cliente.fechaDeNacimiento;
                cliente.genero = clienteCRM.Cliente.genero;
                cliente.idCliente = clienteCRM.Cliente.idCliente;
                cliente.nombre = clienteCRM.Cliente.nombre;
                cliente.numeroDeCredencial = clienteCRM.Cliente.numeroDeCredencial;
            }
            else
            { cliente = null; }

            return cliente;
        }

        public bool updateCliente(string pRutaCertificado, string pEndPoint, modelView.Cliente pCliente)
        {
            int i = 0;
            string resultado = "";
            Clientes.updateClienteRequestType clienteCRM = new Clientes.updateClienteRequestType();
            Clientes.updateClienteRequestTypeCorreoElectronico[] CorreosElectronicos = new Clientes.updateClienteRequestTypeCorreoElectronico[pCliente.CorreosElectronicos.Count];
            clienteCRM.Contacto = new Clientes.updateClienteRequestTypeContacto();

            foreach (modelView.CorreoElectronico correoelectronico in pCliente.CorreosElectronicos)
            {
                CorreosElectronicos[i] = new Clientes.updateClienteRequestTypeCorreoElectronico();
                CorreosElectronicos[i].email = correoelectronico.email;
                CorreosElectronicos[i].tipoEmail = correoelectronico.tipoEmail;
                i++;
            }
            i = 0;
            clienteCRM.CorreosElectronicos = CorreosElectronicos;

            Clientes.Clientes clienteSvc = new Clientes.Clientes();
            Clientes.HeaderMessage headerMessage = new Clientes.HeaderMessage();
            headerMessage.idConsumidor = "07";
            clienteSvc.headerMessage = headerMessage;

            clienteSvc.Url = pEndPoint;
            if (pEndPoint.StartsWith("https:"))
            {
                certifica cert = new certifica();
                if (pRutaCertificado == "") { pRutaCertificado = "C:\\certificado_omega_oaesb\\oaesb.cer"; }
                cert.configuraSSL(pRutaCertificado, clienteSvc);
            }

            clienteCRM.idCliente = pCliente.idCliente;
            clienteCRM.idClienteSpecified = true;

            try
            {
                resultado = clienteSvc.updateCliente(clienteCRM).respuesta;
            }
            catch
            {
                throw;
            }

            return (resultado == "Actualización Correcta");
        }
    }
}
