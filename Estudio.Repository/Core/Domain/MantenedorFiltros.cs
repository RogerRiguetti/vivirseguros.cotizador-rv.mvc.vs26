using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class MantenedorFiltros
    {
        public string codTabla { get; set; }
        public string codIndPlan { get; set; }
        public string codIndTipRenta { get; set; }
        public string codIndModalidad { get; set; }
        public string codIndCliente { get; set; }
        public string codIndPrima { get; set; }
        public double mtoPriDesde { get; set; }
        public double mtoPriHasta { get; set; }
        public string codIndSexo { get; set; }
        public string codIndEstCivil { get; set; }
        public int numEdadDesde { get; set; }
        public int numEdadHasta { get; set; }
        public string codIndMoneda { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public string codUsuario { get; set; }
        public string codElemento { get; set; }
        public string glsElemento { get; set; }
        public int codIndicador { get; set; }


        public string descripcion { get; set; }
        public string codigoElemento { get; set; }
        public string codigoTabla { get; set; }
        public string codigoPension { get; set; }
        
    }
}

