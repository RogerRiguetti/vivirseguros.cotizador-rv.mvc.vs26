using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class CalculoCotizaciones
    {
        //Variables para resultados de solicitudes cotizadas y no cotizadas.
        public int Num_Orden { get; set; }
        public string intermediarioNident { get; set; }
        public string indMej { get; set; }
        public string Num_Cotizacion { get; set; }
        public int Num_Corr { get; set; }
        public int Num_Operacion { get; set; }
        public string CUSPP { get; set; }
        public string Tipo_Pension { get; set; }
        public string Tipo_Renta { get; set; }
        public int Anios_DifEsc { get; set; }
        public int Years_Dif { get; set; }
        public string Modalidad { get; set; }
        public int Years_Gar { get; set; }
        public string Cob_Cony { get; set; }
        public string D_Crecer { get; set; }
        public string D_Gratif { get; set; }
        public string Moneda { get; set; }
        public decimal TIR { get; set; }
        public decimal Renta_Esc { get; set; }
        public decimal Tasa_Venta { get; set; }
        public decimal Mto_Pension { get; set; }
        public decimal Tasa_RT { get; set; }
        public decimal Mto_PensionRT { get; set; }
        public float Prima_Unica { get; set; }
        public decimal Perdida_Contable { get; set; }
        public string Intermediario { get; set; }
        public decimal PRC_Com { get; set; }
        public string Ind_Mej { get; set; }
        public string Error_NoCotizadas { get; set; }
        public string Num_Archivo { get; set; }
        public string Nom_Archivo { get; set; }
        public string codTipReajuste { get; set; }
        public string MensajeErr { get; set; }
        public decimal prc_Pension { get; set; }
        public decimal Mto_Cic { get; set; }
        public decimal tipCambio { get; set; }
        public int Ind_SISCO { get; set; }
        public string Cod_TipRen { get; set; }
        public string Cod_TipPen { get; set; }
    }
}
