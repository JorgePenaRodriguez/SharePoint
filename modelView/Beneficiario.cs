using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelView
{
    public class Beneficiario
    {
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public string IdCotitular { get; set; }
        public string IdCotitularFromClientes { get; set; }
        public string Nombre { get; set; }
        public string Parentesco { get; set; }
        public List<Telefono> Telefonos { get; set; }
    }
}
