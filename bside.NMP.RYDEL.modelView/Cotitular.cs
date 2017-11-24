using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    [Serializable]
    public class Cotitular
    {
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public List<Telefono> ListaTelefonos { get; set; }
    }
}
