using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using System.Data.SqlClient;
using Estudio.Repository.Helpers;
using System.Data;

namespace Estudio.Repository.Persistence.Repositories
{
    public class ManFecAceptacionCotizacionRepository
    {
        public string[] Busca_Informacion(string cuspp, string NumOperacion, string NumCotizacion, string NumCorrelativo)
        {
            try
            {
                ManFecAceptacionCotizacion DatosCon = new ManFecAceptacionCotizacion();
                Response solicitudes = new Response();
                var parameters = new List<SqlParameter>();
                string[] info = new string[25];

                string queryBus = "SELECT DISTINCT(D.NUM_COT),D.NUM_CORRELATIVO,C.COD_CUSPP,D.NUM_MESDIF, D.COD_MODALIDAD,D.NUM_MESGAR,D.COD_MONEDA," +
                                   "D.PRC_RENTATMP,D.COD_COBERCON,D.COD_DERGRA,D.COD_DERCRE,C.FEC_SUSCRIPCION,C.FEC_ENVIO,D.COD_ESTCOT,C.MTO_PRIUNI,D.MTO_PENSION," +
                                   "D.PRC_TASAVTA,D.COD_TIPREN,D.MTO_PRIUNIDIF ,MTO_SUMPENSION,COD_TIPPENSION,ISNULL(D.FEC_ACEPTA, '') as FEC_ACEPTA," +
                                   "D.NUM_MESESC,D.PRC_RENTAESC, CC.IND_GANA, D.NUM_OPERACION " +
                                   "FROM PT_TMAE_DETCOTIZACION D " +
                                   "JOIN PT_TMAE_COTIZACION C ON D.NUM_OPERACION = C.NUM_OPERACION " +
                                   "JOIN PT_THIS_CIERRECIA CC ON D.NUM_OPERACION = CC.NUM_OPERACION " +
                                   "WHERE D.NUM_COT = C.NUM_COT AND D.COD_RECHAZO = '0' AND CC.IND_GANA = 'S' ";

                if (cuspp != "") { queryBus = queryBus + "AND C.COD_CUSPP = '" + cuspp + "' "; }

                if (NumOperacion != "") { queryBus = queryBus + "AND D.NUM_OPERACION = '" + NumOperacion + "' "; }

                if (NumCotizacion != "") { queryBus = queryBus + "AND D.NUM_COT = '" + NumCotizacion + "' "; }

                if (NumCorrelativo != "") { queryBus = queryBus + "AND  D.NUM_CORRELATIVO = '" + NumCorrelativo + "' "; }

                /*if (NumCorrelativo == "" && NumCotizacion == "")
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "B_CO", ParameterDirection.Input));
                }
                else if (NumCorrelativo != "" && NumCotizacion == "")
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "B_COCOR", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCor", SqlDbType.Int, int.Parse(NumCorrelativo), ParameterDirection.Input));
                }
                else if (NumCorrelativo == "" && NumCotizacion != "")
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "B_COCOT", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, NumCotizacion, ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TODOS", ParameterDirection.Input));
                }

                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(NumOperacion), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCuspp", SqlDbType.VarChar, cuspp, ParameterDirection.Input));*/


                DatosCon = SRVDBContext<ManFecAceptacionCotizacion>.CallSelectStatement(queryBus, x => new ManFecAceptacionCotizacion
                {
                    NUM_COT = x.GetString(0),
                    NUM_CORRELATIVO = x.GetInt32(1),
                    COD_CUSPP = x.GetString(2),
                    NUM_MESDIF = x.GetInt32(3),
                    COD_MODALIDAD = x.GetString(4),
                    NUM_MESGAR = x.GetInt32(5),
                    COD_MONEDA = x.GetString(6),
                    PRC_RENTATMP = Convert.ToDouble(x.GetDecimal(7)),
                    COD_COBERCON = x.GetString(8),
                    COD_DERGRA = x.GetString(9),
                    COD_DERCRE = x.GetString(10),
                    FEC_SUSCRIPCION = x.GetString(11),
                    FEC_ENVIO = x.GetString(12),
                    COD_ESTCOT = x.GetString(13),
                    MTO_PRIUNI = Convert.ToDouble(x.GetDecimal(14)),
                    MTO_PENSION = Convert.ToDouble(x.GetDecimal(15)),
                    PRC_TASAVTA = Convert.ToDouble(x.GetDecimal(16)),
                    COD_TIPREN = x.GetString(17),
                    MTO_PRIUNIDIF = Convert.ToDouble(x.GetDecimal(18)),
                    MTO_SUMPENSION = Convert.ToDouble(x.GetDecimal(19)),
                    COD_TIPPENSION = x.GetString(20),
                    FEC_ACEPTA = x.GetString(21),
                    NUM_MESESC = x.GetInt32(22),
                    PRC_RENTAESC = Convert.ToDouble(x.GetDecimal(23)),
                    IND_GANA = x.GetString(24),
                    NUM_OPERACION = Convert.ToString(x.GetDecimal(25))
                }).FirstOrDefault();

                if (DatosCon != null)
                {
                    info[0] = DatosCon.COD_CUSPP.ToString();
                    info[1] = DatosCon.NUM_MESDIF.ToString();
                    info[2] = DatosCon.COD_MODALIDAD.ToString();
                    info[3] = DatosCon.NUM_MESGAR.ToString();
                    info[4] = DatosCon.COD_MONEDA.ToString();
                    info[5] = DatosCon.PRC_RENTATMP.ToString();
                    info[6] = DatosCon.COD_COBERCON.ToString();
                    info[7] = DatosCon.COD_DERGRA.ToString();
                    info[8] = DatosCon.COD_DERCRE.ToString();
                    info[9] = DatosCon.FEC_SUSCRIPCION.ToString();
                    info[10] = DatosCon.FEC_ENVIO.ToString();
                    info[11] = DatosCon.COD_ESTCOT.ToString();
                    info[12] = DatosCon.MTO_PRIUNI.ToString();
                    info[13] = DatosCon.MTO_PENSION.ToString();
                    info[14] = DatosCon.PRC_TASAVTA.ToString();
                    info[15] = DatosCon.COD_TIPREN.ToString();
                    info[16] = DatosCon.MTO_PRIUNIDIF.ToString();
                    info[17] = DatosCon.MTO_SUMPENSION.ToString();
                    info[18] = DatosCon.COD_TIPPENSION.ToString();
                    info[19] = DatosCon.FEC_ACEPTA.ToString();
                    info[20] = DatosCon.NUM_MESESC.ToString();
                    info[21] = DatosCon.PRC_RENTAESC.ToString();
                    info[22] = DatosCon.NUM_OPERACION.ToString();
                    info[23] = DatosCon.NUM_CORRELATIVO.ToString();
                    info[24] = DatosCon.NUM_COT.ToString();
                }
                else
                {
                    info[0] = "Número de Solicitud de Oferta No Existe";
                    return info;
                }


                return info;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public string Actualizar_Informacion(string NumOperacion, string NumCotizacion, string NumCorrelativo, string FecCierre)
        {
            try
            {
                ManFecAceptacionCotizacion DatosCon = new ManFecAceptacionCotizacion();
                string[] info = new string[1];
                Response solicitudes = new Response();

                string query = "UPDATE PT_TMAE_DETCOTIZACION SET ";
                query = query + "FEC_ACEPTA = '" + FecCierre + "' ";
                query = query + "WHERE NUM_COT = '" + NumCotizacion + "' ";
                query = query + "AND NUM_CORRELATIVO = '" + NumCorrelativo + "' ";
                query = query + "AND NUM_OPERACION = '" + NumOperacion + "' ";
                query = query + "AND COD_ESTCOT = 'A'";

                DatosCon = SRVDBContext<ManFecAceptacionCotizacion>.CallSelectStatement(query, x => new ManFecAceptacionCotizacion
                {

                }).FirstOrDefault();

                return "";
            }
            catch (Exception ex)
            {
                return "Operación Cancelada.";
            }
        }
    }
}
