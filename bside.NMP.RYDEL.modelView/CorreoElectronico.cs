using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    [Serializable]
    public class CorreoElectronico
    {
        public string email { get; set; }
        public string tipoEmail { get; set; }
    }
}
