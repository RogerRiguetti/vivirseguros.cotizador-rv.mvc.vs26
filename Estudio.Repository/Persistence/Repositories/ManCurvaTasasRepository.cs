using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using log4net;
using log4net.Config;
using System.Reflection;

namespace Estudio.Repository.Persistence.Repositories
{
    public class ManCurvaTasasRepository
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Response DatosQ(string query)
        {
            try
            {
                List<Parametro> DatosCon = new List<Parametro>();
                string queryCon = "SELECT ClaveParametro, Parametro FROM Parametros WHERE DescripcionParametro = 'CONQA'";

                DatosCon = VCEDBContext<Parametro>.CallSelectStatement(queryCon, x => new Parametro
                {
                    ClaveParametro = x.GetString(0),
                    Elemento = x.GetString(1),
                }).ToList();
                
                string pass = (from passw in DatosCon where passw.ClaveParametro == "PASS" select passw.Elemento).FirstOrDefault();

               string ip = (from ips in DatosCon where ips.ClaveParametro == "IPSERVQA" select ips.Elemento).FirstOrDefault();

               string us = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();

                string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();

                string conexion = "Data Source="+ip+"; Initial Catalog="+ BD +";uid="+us+";pwd="+pass+"";

                //bool statusC = true;
                //string conex = VCEConectionString.Encrypt(conexion, out statusC);


                //bool status;
                //string ConnectionString = conex;  //definir la conexion
                //ConnectionString = Decrypt(ConnectionString, out status);


                VCEDBContext<AsignacionIntermediario>.CallSelectStatementConection(conexion, query, x => new AsignacionIntermediario
                {
                }).FirstOrDefault();

                _log.Info("Proceso terminado");
                Response res2 = new Response();
                res2.IsOk = true;
                res2.Message = "Carga Terminada con Éxito";
                return res2;
            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }

        public List<ManCurvaTasas> CargaTableRepository(string fecha)
        {
            try
            {
                XmlConfigurator.Configure();
                _log.Info("*************Consulta Curvas Tasas*****************");
                List<ManCurvaTasas> Datos = new List<ManCurvaTasas>();

                _log.Info("Se realizara busqueda con la fecha" + fecha);
                string query = "SELECT DISTINCT T.NUM_MES, T.MTO_VALOR, M.COD_SCOMP";
                query = query + " FROM pt_tval_curva_tasas T";
                query = query + " INNER JOIN MA_TPAR_MONEDATIPOREAJU M ON T.COD_MONEDA = M.COD_MONEDA AND T.COD_TIPREAJUSTE = M.COD_TIPREAJUSTE";
                query = query + " WHERE FEC_INIVIG = '" + fecha + "' and FEC_TERVIG = '99991231' order by T.NUM_MES asc ";

                Datos = SRVDBContext<ManCurvaTasas>.CallSelectStatement(query, x => new ManCurvaTasas
                {
                    Num_Mes = x.GetInt32(0),
                    Mto_valor = x.GetDecimal(1),
                    COD_SCOMP = x.GetString(2)
                }).ToList();
                _log.Info("Datos encontrados"+ Datos.Count);
                _log.Info("Informacion de datos" + Datos);
                return Datos;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string Decrypt(string toDecryt, out bool status)
        {
            var result = string.Empty;
            try
            {
                var decryted = Convert.FromBase64String(toDecryt);
                result = System.Text.Encoding.UTF8.GetString(decryted);
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }

            return result;
        }
    }
}
