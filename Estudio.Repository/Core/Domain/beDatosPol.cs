using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beDatosPol
    {
        public string NumPol { get; set; }
        public string TipPen { get; set; }
        public int NumBen { get; set; }
        public string EstPol { get; set; }
        public string FecVig { get; set; }
        public double MtoPri { get; set; }
        public double MtoPen { get; set; }
        public string TipRen { get; set; }
        public string TipMod { get; set; }
        public int NumDif { get; set; }
        public int NumGar { get; set; }
        public double PrcRet { get; set; }
        public double PrcTce { get; set; }
        public double PrcTas { get; set; }
        public double PrcTceDef { get; set; }
        public double PrcTasDef { get; set; }
        public string FecPag { get; set; }
        public double MtoGS { get; set; }
        public double prcFac { get; set; }
        public string FecCot { get; set; }
        public string DerCre { get; set; }
        public string DerGra { get; set; }
        public string IndCob { get; set; }
        public double MtoEll { get; set; }
        public double prcEll { get; set; }
        public string FecDev { get; set; }
        public int TipRea { get; set; }
        public double PrcTri { get; set; }
        public double PrcMen { get; set; }
        public int EdaLim { get; set; }
        public string TipMon { get; set; }
        public int PrcTaf { get; set; }
        public double PrcTasRes { get; set; }

        
        public string Tip { get; set; } //Variable para definir la tabla (Cartera)
        public string IndStock { get; set; } //Variable para obtener Stock
        public int NumeroCasoEspecial { get; set; }
    }
}
