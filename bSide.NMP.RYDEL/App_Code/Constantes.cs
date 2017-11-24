using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bSide.NMP.RYDEL.App_Code
{
    /// <summary>
    /// Clase que contiene las constantes, definiciones de nombres de listas y campos
    /// </summary>
    public class Constantes
    {
        public static string appName = "RYDEL";
        public static string mensajeError = "Ha ocurrido un error";
        public static string codigoBanamex = "002";

        public enum TipoPago
        {
            idDesempenio = 148,
            idRefrendo = 8,
            idAbono = 116
        }

        public class listConfiguracionPago
        {
            public static string nombrelista = "Configuración pago";

            public class Campos
            {
                public static string Titulo = "Title";
                public static string Valor = "Valor";
            }
                public class Registros
            {
                public static string merchantId = "Merchant id";
                public static string password = "Password";
                public static string apiUsername = "Api username";
                public static string version = "Version";
                public static string gatewayFormUrl = "Gateway form url";
                public static string gatewayUrl = "Gateway url";
                public static string gatewayHost = "Gateway host";
                public static string useSsl = "Use ssl";
                public static string useProxy = "Use proxy";
                public static string proxyHost = "Proxy host";
                public static string proxyUser = "Proxy user";
                public static string proxyPassword = "Proxy password";
                public static string proxyDomain = "Proxy domain";
                public static string currency = "Currency";
                public static string debugMode = "Debug mode";
                public static string gatewayPort = "Gateway port";
                public static string ignoreSslErrors = "IgnoreSslErrors";
                public static string payOperation = "Pay operation";
                public static string cancelUrl = "CancelUrl";
                public static string returnUrl = "ReturnUrl";
                public static string horarioInicioOp = "Horario inicio op";
                public static string horarioFinOp = "Horario fin op";
                public static string urlRedireccionHorario = "Url redireccion horario";
                public static string endPoint = "End point";
                public static string endPointCRM = "End point CRM";
                public static string rutaCertificado = "Ruta certificado";
                public static string urlFacturacionEnLinea = "Url facturación en línea";
            }
        }

        public class listConfiguracionMensajes
        {
            public static string nombrelista = "Mensajes";
            public class Campos
            {
                public static string Titulo = "Title";
                public static string Valor = "Valor";
            }

            public class Registros
            {
                public static string CE_CapturaDeDatos = "CE_CapturaDeDatos";
                public static string CE_CapturaDeDatosErronea = "CE_CapturaDeDatosErronea";
                public static string CE_VerificaDatos = "CE_VerificaDatos";
                public static string CE_NoAdeudos = "CE_NoAdeudos";

                public static string CD_PagosPendientes = "CD_PagosPendientes";
                public static string CD_PagosLibres = "CD_PagosLibres";
                public static string CD_Reempenio = "CD_Reempenio";
            }
        }
    }
}
