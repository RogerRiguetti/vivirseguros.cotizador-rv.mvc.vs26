using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class ReportesReservas
    {
        #region Reporte Flujos de Pasivos
        public string AnoMes { get; set; }
        public int Fila { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public double Jubilacion_SolesIndexados { get; set; }
        public double Jubilacion_Dolares { get; set; }
        public double Jubilacion_SolesAjustados { get; set; }
        public double Jubilacion_GastosSepelio { get; set; }
        public double Sobrevivencia_SolesIndexados { get; set; }
        public double Sobrevivencia_Dolares { get; set; }
        public double Sobrevivencia_SolesAjustados { get; set; }
        public double Invalidez_SolesIndexados { get; set; }
        public double Invalidez_Dolares { get; set; }
        public double Invalidez_SolesAjustados { get; set; }
        public double Invalidez_GastosSepelio { get; set; }
        public double GastosOperacion { get; set; }
        public string LineaArchivoTxt { get; set; }
        #endregion

        #region Reporte Resumen Reservas
        public string CodigoMoneda { get; set; }
        public string CodigoReajuste { get; set; }
        public string Prestacion { get; set; }
        public decimal MontoReservaBase { get; set; }
        public int NumeroPolizas { get; set; }
        public decimal SI_Insuficiencia { get; set; }
        public decimal SA_Insuficiencia { get; set; }
        public decimal DolaresEnSoles_Insuficiencia { get; set; }
        #endregion

        #region Reporte Contable
        public string Paquete { get; set; }
        public string Asiento { get; set; }
        public DateTime FechaContable { get; set; }
        public string TipoAsiento { get; set; }
        public string TipoContabilidad { get; set; }
        public string ClaseAsiento { get; set; }
        public string Fuente { get; set; }
        public string Referencia { get; set; }
        public string Contribuyente { get; set; }
        public string CentroCosto { get; set; }
        public string CuentaContable { get; set; }
        public decimal DebitoLocal { get; set; }
        public decimal CreditoLocal { get; set; }
        public decimal DebitoDolar { get; set; }
        public decimal CreditoDolar { get; set; }
        public decimal MontoUnidades { get; set; }
        #endregion
    }
}
