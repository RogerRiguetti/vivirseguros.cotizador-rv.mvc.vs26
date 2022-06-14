using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class MantenedorIPC
    {
        public string FechaIpc { get; set; }
        public decimal MontoIpc { get; set; }
        public decimal VariacionIpc { get; set; }
        public string Codigo { get; set; }
    }
}
