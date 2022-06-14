using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class Parametro
    {
        public int IdParametro { get; set; }
        public string ClaveParametro { get; set; }
        public string Elemento { get; set; }
        public string DescripcionParametro { get; set; }
        public int Estado { get; set; }
    }
}
