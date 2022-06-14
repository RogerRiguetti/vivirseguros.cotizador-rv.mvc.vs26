using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class Modalidad
    {
        public int IdModalidad { get; set; }
        public decimal PorcentajeRentabilidadAfp { get; set; }
        public string Gratificacion { get; set; }
        public decimal PrimerTramo { get; set; }
        public string PrimerTramoStr { get; set; }
        public decimal SegundoTramo { get; set; }
        public string SegundoTramoStr { get; set; }
        public int AniosDiferidos { get; set; }
        public int AniosGarantizados { get; set; }
        public int PorcentajeRentaTemporal { get; set; }
        public int IdMoneda { get; set; }
        public string Moneda { get; set; }
        public int IdTipoRenta { get; set; }
        public string TipoRenta { get; set; }
        public int IdModalidadCat { get; set; }
        public string ModalidadCat { get; set; }
        public int IdCotizacion { get; set; }
        public int IdPaquete { get; set; }
        public string CodigoTiposRenta { get; set; }
        public string CodigoModalidad { get; set; }
        public string ClaveMoneda { get; set; }
        public int CodigoTipoReajuste { get; set; }
        public int IdComision { get; set; }
        public decimal ValorComision { get; set; }
        public string DerGra { get; set; }
        public decimal ComisionInicial { get; set; }
        public decimal PrccomS { get; set; }
        public decimal Prcfaclab { get; set; }
        public decimal Prc_Inicial_1 { get; set; }
        public decimal Prc_Inicial { get; set; }
        public decimal Prc_Minimo_1 { get; set; }
        public decimal Prc_Minimo { get; set; }
    }
}
