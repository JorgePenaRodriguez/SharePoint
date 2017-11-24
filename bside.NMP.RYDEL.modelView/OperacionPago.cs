using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    [Serializable]
    public class OperacionPago
    {
        public long folio { get; set; }

        /// <remarks/>
        public long idOperacion { get; set; }

        /// <remarks/>
        public decimal importe { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool importeSpecified { get; set; }

        /// <remarks/>
        public long transaccion { get; set; }

        /// <remarks/>
        public long autorizacion { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool autorizacionSpecified { get; set; }

				/// <remarks/>
				public string numeroTarjeta { get; set; }

        /// <remarks/>
        public string transaccionBancaria { get; set; }

        /// <remarks/>
        public string origen { get; set; }
    }
}
