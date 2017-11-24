using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    public class notificarPagoResponseComprobantePDF
    {
        public long idOperacion { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("archivo", DataType = "base64Binary")]
        public byte[] archivo { get; set; }
    }
}
