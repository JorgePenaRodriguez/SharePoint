using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    [Serializable]
    public class PartidaSaldos
    {
        public long folio { get; set; }

        /// <remarks/>
        public string tipoContrato { get; set; }

        /// <remarks/>
        public long sucursal { get; set; }

        /// <remarks/>
        public long transaccion { get; set; }

        /// <remarks/>
        public long idCliente { get; set; }

        /// <remarks/>
        public bool aplicarReempeno { get; set; }

        /// <remarks/>
        public bool prestamoPagado { get; set; }

        /// <remarks/>
        public int numRefrendo { get; set; }

				public string estadoPago{get;set;}

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime fechaComercializacion { get; set; }

        /// <remarks/>
        public OperacionesBancarias operacionesBancariasDisponibles { get; set; }

        public string nombre
        { get; set; }

        public string apellidoPaterno
        { get; set; }

        public string apellidoMaterno
        { get; set; }

        public string correoElectronico
        { get; set; }
    }
}
