using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    [Serializable]
    public class OperacionesBancariasDetalleOperacionBancaria        
    {
        public long idOperacion { get; set; }

        /// <remarks/>
        public decimal montoMaximo { get; set; }

        /// <remarks/>
        public decimal montoMinimo { get; set; }

        /// <remarks/>
        public string nombreOperacion { get; set; }
    }
}
