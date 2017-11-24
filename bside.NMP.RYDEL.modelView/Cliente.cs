using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RYDEL.modelView
{
    [Serializable]
    public class Cliente
    {
        public long idCliente { get; set; }
        public string genero { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public System.DateTime fechaDeNacimiento { get; set; }
        public string numeroDeCredencial { get; set; }
        public Cotitular Cotitular { get; set; }
        public List<Beneficiario> Beneficiarios { get; set; }
        public string email { get; set; }
        public List<CorreoElectronico> CorreosElectronicos { get; set; }
        public Contacto Contacto { get; set; }       
    }
}

