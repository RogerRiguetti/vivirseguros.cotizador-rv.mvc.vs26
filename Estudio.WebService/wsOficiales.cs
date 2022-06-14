using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Estudio.WebService
{
    public class wsOficiales
    {
        public string docXML { get; set; }
        public int numArchivo { get; set; }
        //Variables para extraer modalidad con WebService
        public int Correlativo { get; set; }
        public string CUSPP { get; set; }
        public int Num_Operacion { get; set; }
        public string moneda { get; set; }
        public string modalidad { get; set; }
        public string periodoDiferido { get; set; }
        public double rentaTMP { get; set; }
        public string perGarantizado { get; set; }
        public double rentaEsc { get; set; }
        public double primaUnica { get; set; }
        public double renTmp1T { get; set; }
        public double mtoPensio { get; set; }
        public double tasaVenta { get; set; }
        public double tir { get; set; }
        public string perdida { get; set; }
        public double comision { get; set; }
    }
}