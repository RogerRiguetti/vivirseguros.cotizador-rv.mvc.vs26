using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using System.Data.SqlClient;
using System.Data;

namespace Estudio.Repository.Persistence.Repositories
{
    public class SISCORepository
    {
        public SISCO ConsultaSISCO(string num_opera, string Cuspp)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, "CONSISCO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, num_opera, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, Cuspp, ParameterDirection.Input));
                return VCEDBContext<SISCO>.CallStoreProcedure(StoredProcedures.CO_ConsultasSISCO, parameters, x => new SISCO
                {
                    SISCO_OK = x.GetInt16(0),
                    PensionSISCO = x.GetDecimal(1),
                    TasaInteres = x.GetDecimal(2),
                    SISCO_VS = x.GetInt16(3)

                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
