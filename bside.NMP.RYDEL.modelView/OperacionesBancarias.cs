using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    [Serializable]
    public class OperacionesBancarias
    {
        public List<OperacionesBancariasDetalleOperacionBancaria> detalleOperacionBancaria { get; set; }
    }
}
