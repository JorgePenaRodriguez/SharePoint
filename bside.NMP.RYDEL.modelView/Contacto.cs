using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    [Serializable]
    public class Contacto
    {
        public List<Telefono> ListaTelefonos { get; set; }
        public List<Domicilio> ListaDirecciones { get; set; }
    }
}
