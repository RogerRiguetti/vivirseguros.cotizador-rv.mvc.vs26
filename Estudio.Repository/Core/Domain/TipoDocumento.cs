using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class TipoDocumento
    {
        public int IdTipoDocumento { get; set; }
        public string Elemento { get; set; }
        public int Longitud { get; set; }
        public byte IndicadorLongitudExacta { get; set; }
        public string Tipo { get; set; }
    }
}
