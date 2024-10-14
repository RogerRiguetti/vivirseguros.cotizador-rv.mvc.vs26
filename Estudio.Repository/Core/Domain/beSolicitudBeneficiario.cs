using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class beSolicitudBeneficiario
    {
        public int IdSolicitudBeneficiario { get; set; }
        public double PrcPension { get; set; }
        public double PensionAFP { get; set; }
        public double MtoPension { get; set; }

        public beSolicitudBeneficiario(int id, double prcPension, double pensionAFP, double mtoPension)
        {
            IdSolicitudBeneficiario = id;
            PrcPension = prcPension;
            PensionAFP = pensionAFP;
            MtoPension = mtoPension;
        }
    }
}