using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beDatosBen
    {
        public int NumOrd { get; set; }
        public string CodPar { get; set; }
        public string FecNac { get; set; }
        public string GruFam { get; set; }
        public string TipSex { get; set; }
        public string TipInv { get; set; }
        public string FecInv { get; set; }
        public string DerPen { get; set; }
        public double PrcPen { get; set; }
        public double PrcLeg { get; set; }
        public double PrcGar { get; set; }
        public string NacHM { get; set; }
        public string FacFal { get; set; }
        public string derCre { get; set; }
        public string Mensaje { get; set; }

        //Reservas
        public string DerCre { get; set; }
        public double PenLeg { get; set; }
        public double PenGar { get; set; }
        public string NumPol { get; set; }
        public string Tope18 { get; set; }
        public string Estudi { get; set; }

    }
}
