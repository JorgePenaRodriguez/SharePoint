using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    [Serializable]
    public class Domicilio
    {
        public string nombreDelaCalle { get; set; }
        public string numeroExterior { get; set; }
        public string numeroInterior { get; set; }
        public string entreCalle1 { get; set; }
        public string entreCalle2 { get; set; }
        public string estado { get; set; }
        public int idEstado { get; set; }
        public bool idEstadoSpecified { get; set; }

        public int idCodigoPostal { get; set; }
        public string codigoPostal { get; set; }
        public string tipoDomicilio { get; set; }
        public string ciudadPoblacion { get; set; }

        public string delegacionMunicipio { get; set; }
        public int idDelegacionMunicipio { get; set; }

        public string colonia { get; set; }
        public int idColonia { get; set; }
    }
}
