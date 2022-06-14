using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class Sistemas
    {
        public int IdSistema { get; set; }
        public string NombreSistema { get; set; }
        public string ClaveSistema { get; set; }
        public int Estado { get; set; }
    }
}
