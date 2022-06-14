using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using log4net;
using log4net.Config;

namespace Estudio.Repository.Persistence.Repositories
{


    public class ProCarArchivoRepository
    {
        public static string connectionString = "";  //definir la conexion
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #region MÉTODOS PARA REPORTES

        /// <summary>
        /// José Hernández Alvarado.
        /// 24-10-2018
        /// Retorna registro para mostrar datos en el reporte.
        /// </summary>
        /// <param name="numArchivo">Número de archivo XML leído.</param>
        /// <returns>Lista con datos para reporte.</returns>
        public List<ProCarArchivo> RptResumen(string numArchivo)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPTRESUMEN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArchivo", SqlDbType.VarChar, numArchivo, ParameterDirection.Input));
                return VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosReportesProCarArchivo, parameters, x => new ProCarArchivo
                {
                    Num_Archivo = x.GetInt32(0).ToString(),
                    Num_RegErrsol = x.GetInt32(1),
                    Num_RegOksol = x.GetInt32(2),
                    Num_RegErrmod = x.GetInt32(3),
                    Num_RegOkmod = x.GetInt32(4),
                    Num_RegErrcia = x.GetInt32(5),
                    Num_RegOkcia = x.GetInt32(6),
                    Num_RegErrafp = x.GetInt32(7),
                    Num_RegOkafp = x.GetInt32(8),
                    Total_Carga = x.GetInt32(2) + x.GetInt32(1),
                    Total_Afi = x.GetInt32(4) + x.GetInt32(3),
                    Total_Fondo = x.GetInt32(6) + x.GetInt32(5),
                    Total_Ben = x.GetInt32(8) + x.GetInt32(7)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 26-10-2018
        /// Retorna registro para mostrar datos en el reporte de solicitudes ganadas.
        /// </summary>
        /// <param name="numArchivo">Número de archivo XML leído.</param>
        /// <param name="codCIA">Código de compañía.</param>
        /// <returns></returns>
        public List<ProCarArchivo> RptGanadas(string numArchivo, string codCIA)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPTGANADAS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArchivo", SqlDbType.VarChar, numArchivo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodCia", SqlDbType.VarChar, codCIA, ParameterDirection.Input));
                return VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosReportesProCarArchivo, parameters, x => new ProCarArchivo
                {
                    Num_Archivo = numArchivo,
                    Num_Cotizacion = x.GetString(5),
                    Num_Corr = x.GetInt32(18),
                    Num_Operacion = Convert.ToInt32(x.GetDecimal(4)),
                    CUSPP = x.GetString(1),
                    Tipo_Pension = x.GetString(29),
                    Tipo_Renta = x.GetString(7),
                    Meses_DifEsc = x.GetString(6) == "6" ? (x.GetInt32(12) / 12) : (x.GetInt32(13) / 12),
                    Modalidad = x.GetString(11),
                    Meses_Gar = x.GetInt32(14) / 12,
                    Cob_Cony = x.GetString(15),
                    D_Crecer = x.GetString(16),
                    D_Gratif = x.GetString(17),
                    Moneda = x.GetString(9),
                    TIR = x.GetDecimal(19),
                    Tasa_Venta = x.GetDecimal(20),
                    Renta_Esc = x.GetDecimal(21),
                    Mto_Pension = x.GetString(28) == "08" ? x.GetDecimal(30) : x.GetDecimal(31),
                    Tasa_RT = x.GetDecimal(22) != 0 ? x.GetDecimal(23) : 0,
                    Mto_PensionRT = (x.GetDecimal(22)),// * x.GetDecimal(24)),
                    //Prima_Unica = x.GetString(6) == "1" ? x.GetDecimal(27) : (x.GetDecimal(25) * x.GetDecimal(24)),
                    Prima_Unica = x.GetDecimal(25),
                    Perdida_Contable = x.GetDecimal(26)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 29-10-2018
        /// Retorna registro para mostrar datos en el reporte (Solicitudes Perdidad por la Compañía CIA).
        /// </summary>
        /// <param name="numArchivo">Número de archivo XML leído.</param>
        /// <param name="codCIA">Código de compañía.</param>
        /// <returns></returns>
        public List<ProCarArchivo> RptPerdidasCia(string numArchivo, string codCIA)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPTPERDIDASCIA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArchivo", SqlDbType.VarChar, numArchivo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodCia", SqlDbType.VarChar, codCIA, ParameterDirection.Input));
                return VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosReportesProCarArchivo, parameters, x => new ProCarArchivo
                {
                    Num_Archivo = numArchivo,
                    Cod_Compañia = x.GetString(4),
                    Num_Cotizacion = x.GetString(5),
                    Num_Operacion = Convert.ToInt32(x.GetDecimal(6)),
                    CUSPP = x.GetString(1),
                    Tipo_Renta = x.GetString(13),
                    Meses_DifEsc = x.GetString(12) == "6" ? (x.GetInt32(18) / 12) : (x.GetInt32(19) / 12),
                    Modalidad = x.GetString(17),
                    Meses_Gar = x.GetInt32(20),
                    Cob_Cony = x.GetString(21),
                    D_Crecer = x.GetString(22),
                    D_Gratif = x.GetString(23),
                    Moneda = x.GetString(15),
                    Tasa_Venta = x.GetDecimal(7),
                    Renta_Esc = x.GetDecimal(25),
                    Mto_Pension = x.GetDecimal(8),
                    Tasa_RT = x.GetDecimal(9),
                    Mto_PensionRT = x.GetDecimal(10),
                    Prima_Unica = x.GetDecimal(11)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 30-10-2018
        /// Retorna registro para mostrar datos en el reporte (Solicitudes Perdidad por la Compañía AFP).
        /// </summary>
        /// <param name="numArchivo">Número de archivo XML leído.</param>
        /// <param name="codCIA">Código de compañía.</param>
        /// <returns></returns>
        public List<ProCarArchivo> RptPerdidasAfp(string numArchivo)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPTPERDIDASAFP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArchivo", SqlDbType.VarChar, numArchivo, ParameterDirection.Input));
                return VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosReportesProCarArchivo, parameters, x => new ProCarArchivo
                {
                    Num_Archivo = numArchivo,
                    Cod_Compañia = x.GetString(3),
                    Num_Cotizacion = x.GetString(4),
                    Num_Operacion = Convert.ToInt32(x.GetDecimal(5)),
                    CUSPP = x.GetString(6),
                    Tipo_Renta = x.GetString(7),
                    Meses_DifEsc = x.GetString(7) == "6" ? (x.GetInt32(8) / 12) : (x.GetInt32(9) / 12),
                    Modalidad = x.GetString(10),
                    Meses_Gar = x.GetInt32(11),
                    Cob_Cony = x.GetString(12),
                    D_Crecer = x.GetString(13),
                    D_Gratif = x.GetString(14),
                    Renta_Temp = x.GetDecimal(15),
                    Moneda = x.GetString(17),
                    Tasa_RP_RT = x.GetDecimal(18),
                    Renta_Esc = x.GetDecimal(19),
                    Nivel_Base = x.GetString(20),
                    Mto_Pension = x.GetDecimal(21)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public List<ProCarArchivo> RptOtrosCia(string numArchivo, string tipoRpt)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPTSOLICITUDESRECO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArchivo", SqlDbType.VarChar, numArchivo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodCierre", SqlDbType.VarChar, tipoRpt, ParameterDirection.Input));
                return VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosReportesProCarArchivo, parameters, x => new ProCarArchivo
                {
                    Num_Archivo = numArchivo,
                    Cod_Compañia = x.GetString(2),
                    Num_Cotizacion = x.IsDBNull(3) ? "" : x.GetString(3),
                    Num_Operacion = Convert.ToInt32(x.GetDecimal(4)),
                    CUSPP = x.GetString(5),
                    Tipo_Renta = x.GetString(6),
                    Meses_DifEsc = x.GetString(7) == "6" ? (x.GetInt32(8) / 12) : (x.GetInt32(9) / 12),
                    Modalidad = x.GetString(11),
                    Meses_Gar = x.GetInt32(12),
                    Cob_Cony = x.GetString(13),
                    D_Crecer = x.GetString(14),
                    D_Gratif = x.GetString(15),
                    Renta_Temp = x.GetDecimal(16),
                    Moneda = x.GetString(18),
                    Tasa_Venta = x.GetDecimal(19),
                    Renta_Esc = x.GetDecimal(20),
                    Mto_Pension = x.GetDecimal(21),
                    Tasa_RT = x.GetDecimal(22),
                    Mto_PensionRT = x.GetDecimal(23),
                    Prima_Unica = x.GetDecimal(24)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public List<ProCarArchivo> RptOtrosAFP(string numArchivo, string tipoRpt)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPTSOLICITUDRECOAFP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArchivo", SqlDbType.VarChar, numArchivo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodCierre", SqlDbType.VarChar, tipoRpt, ParameterDirection.Input));
                return VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosReportesProCarArchivo, parameters, x => new ProCarArchivo
                {
                    Num_Archivo = numArchivo,
                    AFP = x.GetString(2),
                    Num_Cotizacion = x.IsDBNull(3) ? "" : x.GetString(3),
                    Num_Operacion = Convert.ToInt32(x.GetDecimal(4)),
                    CUSPP = x.GetString(5),
                    Tipo_Renta = x.GetString(6),
                    Meses_DifEsc = x.GetString(6) == "6" ? (x.GetInt32(7) / 12) : (x.GetInt32(8) / 12),
                    Modalidad = x.GetString(9),
                    Meses_Gar = x.GetInt32(10),
                    Cob_Cony = x.GetString(11),
                    D_Crecer = x.GetString(12),
                    D_Gratif = x.GetString(13),
                    Renta_Temp = x.GetDecimal(14),
                    Moneda = x.GetString(15),
                    Tasa_RP_RT = x.GetDecimal(16),
                    Renta_Esc = x.GetDecimal(17),
                    Nivel_Base = x.GetString(18),
                    Mto_Pension = x.GetDecimal(19)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 25-10-2018
        /// </summary>
        /// <param name="numArchivo">Número de archivo de XML leído.</param>
        /// <param name="codCIA">Código de compañía.</param>
        /// <returns>Lista para validar si existen registros de solicitudes ganadas.</returns>
        public List<ProCarArchivo> validaReportes(string numArchivo, string codCIA, string bandera)
        {
            string query = "";
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

                string usc = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();
                string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();

                connectionString = "Data Source=" + ip + "; Initial Catalog="+BD+";uid=" + usc + ";pwd=" + pass + "";

                if (bandera == "Ganadas")
                {
                    query = "SELECT COUNT(NUM_ARCHIVO)AS NUM_REGISTROS FROM PT_THIS_CIERRECIA WHERE NUM_ARCHIVO = '" + numArchivo + "' AND IND_GANA ='S' AND COD_CIA = '" + codCIA + "' ";
                }


                return VCEDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                { }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al consultar Solicitudes Ganadas. " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 29-10-2018
        /// Retorna respuesta para verificar que hay registros a mostrar para generar reporte de solicitudes perdidas por la compañía.
        /// </summary>
        /// <param name="numArchivo">Número de archivo XML leído.</param>
        /// <param name="codCIA">Código de la empresa desde BD.</param>
        /// <returns></returns>
        public bool validaRptPerdidasCia(string numArchivo, string codCIA)
        {
            string query1 = "SELECT COUNT(NUM_ARCHIVO) AS NUM_REGISTROS FROM PT_THIS_CIERRECIA WHERE NUM_ARCHIVO = '" + numArchivo + "' AND IND_GANA ='S' AND COD_CIA <> '" + codCIA + "'";
            string query2 = "SELECT COUNT(NUM_ARCHIVO) AS NUM_REGISTROS FROM PT_THIS_CIERREAFP WHERE NUM_ARCHIVO = '" + numArchivo + "' AND IND_GANA ='S' ";
            bool rpt = false;
            List<ProCarArchivo> rptLista = new List<ProCarArchivo>();

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

                string usc = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();
                string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();

                connectionString = "Data Source=" + ip + "; Initial Catalog="+BD+";uid=" + usc + ";pwd=" + pass + "";

                rptLista = VCEDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query1, x => new ProCarArchivo
                {
                    Num_Reg = x.GetInt32(0)
                }).ToList();

                if (rptLista[0].Num_Reg == 0)
                {
                    rptLista = new List<ProCarArchivo>();
                    rptLista = VCEDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query2, x => new ProCarArchivo
                    {
                        Num_Reg = x.GetInt32(0)
                    }).ToList();
                    if (rptLista[0].Num_Reg == 0)
                    {
                        rpt = false;
                    }
                    else
                    {
                        rpt = true;
                    }
                }
                else
                {
                    rpt = true;
                }

                return rpt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al consultar Solicitudes Ganadas. " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 31-10-2018
        /// Retorna respuesta para verificar que hay registros a mostrar para generar reportes de solicitudes dinámicamente (RECOTIZADAS, DESISTIDAS, CADUCADAS).
        /// </summary>
        /// <param name="numArchivo">Número de archivo XML leído.</param>
        /// <param name="codCIA">Código de la empresa desde BD.</param>
        /// <returns></returns>
        public bool validaRptsCia(string numArchivo, string tipoRpt)
        {
            string query = "SELECT COUNT(NUM_ARCHIVO)AS NUM_REGISTROS FROM PT_THIS_CIERRESOL WHERE NUM_ARCHIVO = '" + numArchivo + "' AND COD_CIERRE = '" + tipoRpt + "'";

            bool rpt = false;
            List<ProCarArchivo> rptLista = new List<ProCarArchivo>();

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

                string usc = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();
                string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();
                connectionString = "Data Source=" + ip + "; Initial Catalog="+BD+";uid=" + usc + ";pwd=" + pass + "";

                rptLista = VCEDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                    Num_Reg = x.GetInt32(0)
                }).ToList();


                if (rptLista[0].Num_Reg == 0)
                {
                    rpt = false;
                }
                else
                {
                    rpt = true;
                }

                return rpt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al consultar Solicitudes RECOTIZADAS, DESISTIDAS, o CADUCADAS. " + ex.Message);
                throw;
            }
        }


        public bool validaRptsAfp(string numArchivo, string tipoRpt)
        {
            string query = "SELECT COUNT(S.NUM_ARCHIVO) AS NUM_REGISTROS FROM PT_THIS_CIERRESOL S INNER JOIN PT_THIS_CIERREAFP A ON S.NUM_ARCHIVO = A.NUM_ARCHIVO AND S.NUM_OPERACION = A.NUM_OPERACION WHERE S.NUM_ARCHIVO = '" + numArchivo + "' AND S.COD_CIERRE ='" + tipoRpt + "'";

            bool rpt = false;
            List<ProCarArchivo> rptLista = new List<ProCarArchivo>();

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

                string usc = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();
                string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();
                connectionString = "Data Source=" + ip + "; Initial Catalog="+BD+";uid=" + usc + ";pwd=" + pass + "";

                rptLista = VCEDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                    Num_Reg = x.GetInt32(0)
                }).ToList();


                if (rptLista[0].Num_Reg == 0)
                {
                    rpt = false;
                }
                else
                {
                    rpt = true;
                }

                return rpt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al consultar Solicitudes RECOTIZADAS, DESISTIDAS, o CADUCADAS. " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 26-10-2018
        /// Consulta nombre del archivo XML leído en BD.
        /// </summary>
        /// <param name="numArchivo">Número de archivo XML</param>
        /// <returns></returns>
        public string nombreArchivo(string numArchivo)
        {
            string val = "";
            ProCarArchivo objArchivo = new ProCarArchivo();
            try
            {
                string query = "SELECT GLS_NOMARCH FROM PT_THIS_ENTRADA WHERE NUM_ARCHIVO = '" + numArchivo + "' ";
                List<Parametro> DatosCon = new List<Parametro>();
                string queryCon = "SELECT ClaveParametro, Parametro FROM Parametros WHERE DescripcionParametro = 'CONQA'";

                DatosCon = VCEDBContext<Parametro>.CallSelectStatement(queryCon, x => new Parametro
                {
                    ClaveParametro = x.GetString(0),
                    Elemento = x.GetString(1),
                }).ToList();

                string pass = (from passw in DatosCon where passw.ClaveParametro == "PASS" select passw.Elemento).FirstOrDefault();

                string ip = (from ips in DatosCon where ips.ClaveParametro == "IPSERVQA" select ips.Elemento).FirstOrDefault();

                string usc = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();
                string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();

                connectionString = "Data Source=" + ip + "; Initial Catalog="+BD+";uid=" + usc + ";pwd=" + pass + "";

                objArchivo = VCEDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                    Nom_Archivo = x.GetString(0)
                }).FirstOrDefault();

                if (objArchivo != null)
                {
                    val = objArchivo.Nom_Archivo;
                }
                else
                {
                    val = "No se encontró el nombre del archivo.";
                }
                return val;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al consultar el nombre del archivo.");
                throw;
            }
        }

        #endregion


        #region Metodos utilizados para la carga del archivo 

        public int NumEntrada(string tipoArchivo, string NomArch, string FecCar, string HorCar, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            int numArchivo = 1;
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "NUMENTRADA", ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    numArchivo = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    numArchivo = _SolicitudCotizacion.numArchivo + 1;

                }

                List<Parametro> DatosCon = new List<Parametro>();
                string queryCon = "SELECT ClaveParametro, Parametro FROM Parametros WHERE DescripcionParametro = 'CONQA'";

                DatosCon = VCEDBContext<Parametro>.CallSelectStatement(queryCon, x => new Parametro
                {
                    ClaveParametro = x.GetString(0),
                    Elemento = x.GetString(1),
                }).ToList();

                string pass = (from passw in DatosCon where passw.ClaveParametro == "PASS" select passw.Elemento).FirstOrDefault();

                string ip = (from ips in DatosCon where ips.ClaveParametro == "IPSERVQA" select ips.Elemento).FirstOrDefault();

                string usc = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();
                string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();

                connectionString = "Data Source=" + ip + "; Initial Catalog="+BD+";uid=" + usc + ";pwd=" + pass + "";
                
                string query = CargaThis_entrada(numArchivo, tipoArchivo, NomArch, FecCar, HorCar, usuario);
                EjecutarScript(query);

                return numArchivo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Omar Figueroa Flores
        /// 18-10-2018
        /// Metodo que inserta datos a la tabla PT_TTMP_CIERREAFP
        /// 
        /// Antonio Quezada 2019-01-03
        /// Se agrega el return del query para ejecutarse en una sola exhibición
        /// </summary>
        /// <param name="datos">Todos los datos que se guardaran en la tabla</param>
        /// <param name="numArch">Numero de archivo con el que se esta ejecutando</param>
        /// <param name="iCount">Contador que sirve para insertar diferentes modalidades y la llave primaria se unica</param>
        /// <param name="NumCotizacion">Numero de la cotizacion extraida del xml</param>
        /// <returns> Regresa el query que será ejecutado </returns>



        public string guardarResultadosAFP(ProCarArchivo datos, int numArch, int iCount, string NumCotizacion)
        {
            try
            {
                string query = "INSERT INTO PT_TTMP_CIERREAFP (NUM_ARCHIVO, NUM_OPERACION,NUM_MODALIDAD,COD_AFP,IND_ATIENDE,";
                if (datos.strGana != "") { query = query + "IND_GANA,"; }
                if (datos.strNivBas != "") { query = query + "IND_NIVBASE,"; }
                if (NumCotizacion != "") { query = query + "NUM_COT,"; }
                if (datos.douPension != "0") { query = query + "MTO_PENSION,"; }
                if (datos.douTasa != "0") { query = query + "PRC_TASARPRT,"; }
                if (datos.douPriUnicaAFP != "0") { query = query + "MTO_PRIUNI,"; }
                query = query + " COD_ERROR) VALUES (" + numArch + "," + datos.intNumOpe + "," + iCount + ", '" + datos.strCodAfp + "', '" + datos.strAtiende + "',";
                if (datos.strGana != "") { query = query + "'" + datos.strGana + "',"; }
                if (datos.strNivBas != "") { query = query + "'" + datos.strNivBas + "',"; }
                if (NumCotizacion != "") { query = query + "'" + NumCotizacion + "',"; }
                if (datos.douPension != "0") { query = query + Convert.ToDouble(datos.douPension) + ","; }
                if (datos.douTasa != "0") { query = query + Convert.ToDouble(datos.douTasa) + ","; }
                if (datos.douPriUnicaAFP != "0") { query = query + Convert.ToDouble(datos.douPriUnicaAFP) + ","; }
                query = query + "'" + (datos.strError == null ? "0" : datos.strError) + "')";

                return query;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 18-10-2018
        /// Metodo que inserta datos a la tabla PT_TTMP_CIERRECIA
        /// 
        /// Antonio Quezada 2019-01-03
        /// Se agrega el return del query para ejecutarse en una sola exhibición
        /// </summary>
        /// <param name="datos">Todos los datos que se guardaran en la tabla</param>
        /// <param name="numArch">Numero de archivo con el que se esta ejecutando</param>
        /// <param name="iCount">Contador que sirve para insertar diferentes modalidades y la llave primaria se unica</param>
        /// <param name="NumCotizacion">Numero de la cotizacion extraida del xml</param>
        /// <returns> Regresa el query que será ejecutado </returns>

        public string guardarResultadosEESS(ProCarArchivo datos, int numArch, int iCount, string NumCotizacion)
        {
            try
            {
                string query = "INSERT INTO PT_TTMP_CIERRECIA (NUM_ARCHIVO, NUM_OPERACION,NUM_MODALIDAD,COD_CIA,IND_ATIENDE,";
                if (datos.strGana != "") { query = query + "IND_GANA,"; }
                if (datos.strCotiza != "") { query = query + "IND_COTIZA,"; }
                if (NumCotizacion != "") { query = query + "NUM_COT,"; }
                if (datos.douPrima != "0") { query = query + "MTO_PRIMA,"; }
                if (datos.douPension != "0") { query = query + "MTO_PENSION,"; }
                if (datos.douTasa != "0") { query = query + "PRC_TASACIA,"; }
                if (datos.douPensionRT != "0") { query = query + "MTO_PENSIONRT,"; }
                if (datos.douTasaRT != "0") { query = query + "PRC_TASACIART,"; }
                if (datos.douPriUniAFPEESS != "0") { query = query + "MTO_PRIUNIAFP,"; }
                query = query + " COD_ERROR ) VALUES ( " + numArch + "," + datos.intNumOpe + "," + iCount + ", '" + datos.strCodCia + "', '" + datos.strAtiende + "',";
                if (datos.strGana != "") { query = query + "'" + datos.strGana + "',"; }
                if (datos.strCotiza != "") { query = query + "'" + datos.strCotiza + "',"; }
                if (NumCotizacion != "") { query = query + "'" + NumCotizacion + "',"; }
                if (datos.douPrima != "0") { query = query + Convert.ToDouble(datos.douPrima) + ","; }
                if (datos.douPension != "0") { query = query + Convert.ToDouble(datos.douPension) + ","; }
                if (datos.douTasa != "0") { query = query + Convert.ToDouble(datos.douTasa) + ","; }
                if (datos.douPensionRT != "0") { query = query + Convert.ToDouble(datos.douPensionRT) + ","; }
                if (datos.douTasaRT != "0") { query = query + Convert.ToDouble(datos.douTasaRT) + ","; }
                if (datos.douPriUniAFPEESS != "0") { query = query + Convert.ToDouble(datos.douPriUniAFPEESS) + ","; }
                query = query + "'" + (datos.strError == null ? "0" : datos.strError) + "')";

                return query;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 18-10-2018
        /// Metodo que inserta datos a la tabla PT_TTMP_CIERREMOD
        /// </summary>
        /// <param name="datos">Todos los datos que se guardaran en la tabla</param>
        /// <param name="numArch">Numero de archivo con el que se esta ejecutando</param>
        /// <param name="iCount">Contador que sirve para insertar diferentes modalidades y la llave primaria se unica</param>
        /// <param name="NumCotizacion">Numero de la cotizacion extraida del xml</param>
        /// <returns>  </returns>
        public void guardarResulProducto(ProCarArchivo datos, int numArch, int iCount, string NumCotizacion)
        {
            try
            {
                string query = "INSERT INTO PT_TTMP_CIERREMOD (NUM_ARCHIVO, NUM_OPERACION,NUM_MODALIDAD,COD_MODALIDAD,COD_MONEDA,";
                if (datos.douanosRT != 0) { query = query + "NUM_ANOSRT,"; }
                if (datos.douporcentajeRVD != "0") { query = query + "PRC_RVD,"; }
                if (datos.intPerGar != 0) { query = query + "COD_PERGAR,"; }
                query = query + "COD_COBERCON,COD_DERGRA,COD_DERCRE,";
                if (datos.strParCapital != "") { query = query + "COD_PARCAP,"; }
                query = query + "COD_ERROR) VALUES ( " + numArch + "," + datos.intNumOpe + "," + iCount + ", '" + datos.strTipRen + "',";
                if (datos.strMoneda != "") { query = query + "'" + datos.strMoneda + "',"; }
                else
                {
                    if (datos.strTipRen == "RB") { query = query + "'US$',"; }
                }
                if (datos.douanosRT != 0) { query = query + Convert.ToDouble(datos.douanosRT) + ","; }
                if (datos.douporcentajeRVD != "0") { query = query + Convert.ToDouble(datos.douporcentajeRVD) + ","; }
                if (datos.intPerGar != 0) { query = query + datos.intPerGar + ","; }
                query = query + "'" + datos.strCobCony + "','" + datos.strDerGra + "','" + datos.strDerCre + "',";
                if (datos.strParCapital != "") { query = query + "'" + datos.strParCapital + "',"; }
                query = query + "'" + (datos.strError == null ? "0" : datos.strError) + "')";

                SRVDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                datos.strError = "113";
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 18-10-2018
        /// Metodo que inserta datos a la tabla PT_TTMP_CIERRESOL
        /// </summary>
        /// <param name="datos">Todos los datos que se guardaran en la tabla</param>
        /// <param name="numArch">Numero de archivo con el que se esta ejecutando</param>
        /// <param name="usuario">Codigo del usuario del sistema que esta ejecutando este metodo</param>
        /// <returns>  </returns>
        /// 
        public void guardarCIERRESOL(ProCarArchivo datos, int numArch, string usuario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "INSERTCIERRESOL", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(datos.intNumOpe), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCUSPP", SqlDbType.VarChar, datos.strCussp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCierre", SqlDbType.VarChar, datos.strDecision, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codError", SqlDbType.VarChar, (datos.strError == null ? "0" : datos.strError), ParameterDirection.Input));
                VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                datos.strError = "113";
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 18-10-2018
        /// Metodo que elimina registros de varias tablas mediante el numero de archivo 
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta ejecutando</param>
        /// <returns> Retorna vacio o algun error que surja en la ejecución </returns>
        public string EliminarNumArchivo(int numArch)
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ELIMINANUMARCHIVO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                }).FirstOrDefault();
                return "";
            }
            catch (Exception)
            {
                return "Error producido al eliminar registros de tablas temporales por Número de Archivo";
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        /// Trasfiere los errores a las diferentes tablas realizando updates
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta ejecutando</param>
        /// <returns> Retorna vacio o algun error que surja en la ejecución </returns>
        public string transfError(int numArch)
        {
            List<ProCarArchivo> _ProCarArchivo = new List<ProCarArchivo>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TRANSFERRORES", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    intNumOpe = x.GetInt32(0),
                    numArchivo = x.GetInt32(1),
                    strError = x.GetString(2)
                }).ToList();

                if (_ProCarArchivo.Count != 0)
                {
                    string query = "";
                    for (int i = 0; i < _ProCarArchivo.Count; i++)
                    {
                        query += "UPDATE PT_TTMP_CIERREMOD set COD_ERROR = '" + _ProCarArchivo[i].strError + "' where NUM_ARCHIVO = " + _ProCarArchivo[i].numArchivo + " and NUM_OPERACION = " + _ProCarArchivo[i].intNumOpe + "\n" +
                                "UPDATE PT_TTMP_CIERRECIA set COD_ERROR = '" + _ProCarArchivo[i].strError + "' where NUM_ARCHIVO = " + _ProCarArchivo[i].numArchivo + " and NUM_OPERACION = " + _ProCarArchivo[i].intNumOpe + "\n" +
                                "UPDATE PT_TTMP_CIERREAFP set COD_ERROR = '" + _ProCarArchivo[i].strError + "' where NUM_ARCHIVO = " + _ProCarArchivo[i].numArchivo + " and NUM_OPERACION = " + _ProCarArchivo[i].intNumOpe + "\n";
                    }
                    EjecutarScript(query);
                }
                return "";
            }
            catch (Exception)
            {
                return "Error al grabar error";
            }
        }
        public string CargarTHIS(int numArch, string tipoArchivo, string NomArch, string FecCar, string HorCar, string usuario)
        {
            try
            {
                string query = "";
                //query += "\n" + CargaThis_entrada(numArch, tipoArchivo, NomArch, FecCar, HorCar, usuario);
                query += "\n" + CargaThis_CierreSol(numArch);
                query += "\n" + CargaThis_CierreMod(numArch);
                query += "\n" + CargaThis_CierreCia(numArch);
                query += "\n" + CargaThis_CierreAfp(numArch);
                EjecutarScript(query);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        ///Se realiza un select a la tabla pt_ttmp_cierreafp para obtener los numeros de operacion
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta ejecutando</param>
        /// /// <returns> </returns>
        public string CargaThis_CierreAfp(int numArch)
        {
            try
            {
                string query = "";
                List<ProCarArchivo> _ProCarList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGATHISCIERREAFP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                _ProCarList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    intNumOpe = Convert.ToInt32(x.GetDecimal(0))
                }).ToList();
                if (_ProCarList.Count != 0)
                {
                    for (int i = 0; i < _ProCarList.Count; i++)
                    {
                        query += "\n" + insertThisAFP(_ProCarList[i].intNumOpe);
                    }
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        ///Se realiza un select a la tabla PT_TTMP_CIERREAFP
        /// </summary>
        /// <param name="intNumOpe">Numero de operacion obtenido del metodo CargaThis_CierreAfp</param>
        ///<returns> </returns>
        public string insertThisAFP(int intNumOpe)
        {
            try
            {
                string query2 = "";
                List<ProCarArchivo> _ProCarList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_THISAFP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(intNumOpe), ParameterDirection.Input));
                _ProCarList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    numArchivo = x.GetInt32(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strModalidad = Convert.ToString(x.GetInt32(2)),
                    strCodAfp = x.GetString(3),
                    strAtiende = x.GetString(4),
                    strGana = x.GetString(5),
                    strNivBas = x.GetString(6),
                    strNumCot = x.GetString(7),
                    douPension = Convert.ToString(x.GetDecimal(8)),
                    douTasaRT = Convert.ToString(x.GetDecimal(9)),
                    douPrima = Convert.ToString(x.GetDecimal(10))
                }).ToList();
                if (_ProCarList.Count != 0)
                {
                    for (int i = 0; i < _ProCarList.Count; i++)
                    {
                        try
                        {
                            query2 += "INSERT INTO PT_THIS_CIERREAFP(NUM_ARCHIVO, NUM_OPERACION, NUM_MODALIDAD,COD_AFP,IND_ATIENDE,";
                            if (_ProCarList[i].strGana != "") { query2 = query2 + "IND_GANA,"; }
                            if (_ProCarList[i].strNivBas != "") { query2 = query2 + "IND_NIVBASE,"; }
                            if (_ProCarList[i].strNumCot != "") { query2 = query2 + "NUM_COT,"; }
                            query2 = query2 + "MTO_PENSION,PRC_TASARPRT";
                            if (_ProCarList[i].douPrima != "0") { query2 = query2 + ",MTO_PRIUNI"; }
                            query2 = query2 + ")values(" + _ProCarList[i].numArchivo + "," + _ProCarList[i].intNumOpe + "," + _ProCarList[i].strModalidad + ",'" +
                                    _ProCarList[i].strCodAfp + "','" + _ProCarList[i].strAtiende + "',";
                            if (_ProCarList[i].strGana != "") { query2 = query2 + "'" + _ProCarList[i].strGana + "',"; }
                            if (_ProCarList[i].strNivBas != "") { query2 = query2 + "'" + _ProCarList[i].strNivBas + "',"; }
                            if (_ProCarList[i].strNumCot != "") { query2 = query2 + "'" + _ProCarList[i].strNumCot + "',"; }
                            query2 = query2 + Convert.ToDouble(_ProCarList[i].douPension) + "," + Convert.ToDouble(_ProCarList[i].douTasaRT);
                            if (_ProCarList[i].douPrima != "0") { query2 = query2 + "," + Convert.ToDouble(_ProCarList[i].douPrima); }
                            query2 = query2 + ")";

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                return query2;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        ///Se realiza un insert a la tabla pt_this_entrada
        /// </summary>
        /// <param name="numArchivo">Numero de archivo con el que se esta trabajando</param>
        /// <param name="tipoArchivo">Tipo de archivo con el que se esta trabajando</param>
        /// <param name="NomArch">Nombre de archivo con el que se esta trabajando</param>
        /// <param name="FecCar">Fecha en que se cargó el archivo</param>
        /// <param name="HorCar">Hora en que se cargó el archivo</param>
        /// <param name="usuario">Usuario del sistema que cargo el archivo</param>
        ///<returns> </returns>
        public string CargaThis_entrada(int numArchivo, string tipoArchivo, string NomArch, string FecCar, string HorCar, string usuario)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                return "insert into pt_this_entrada (num_archivo, cod_tiparch, gls_nomarch,fec_cararch,hor_cararch,cod_usuariocrea,fec_crea, hor_crea)values(" +
                        numArchivo + ",'" + tipoArchivo + "','" + NomArch + "','" + FecCar + "','" + HorCar + "','" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        ///Se realiza un select a la tabla pt_ttmp_cierrecia para obtener los numeros de operacion
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta ejecutando</param>
        /// <returns> </returns>
        public string CargaThis_CierreCia(int numArch)
        {
            try
            {
                string query = "";
                List<ProCarArchivo> _ProCarList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_THISCIERRECIA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                _ProCarList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    intNumOpe = Convert.ToInt32(x.GetDecimal(0))
                }).ToList();
                if (_ProCarList.Count != 0)
                {
                    for (int i = 0; i < _ProCarList.Count; i++)
                    {
                        query += "\n" + insertThisCIA(_ProCarList[i].intNumOpe);
                    }
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        ///Se realiza un select a la tabla pt_ttmp_cierrecia
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta ejecutando</param>
        /// <returns> </returns>
        public string insertThisCIA(int intNumOpe)
        {
            try
            {
                string query2 = "";
                List<ProCarArchivo> _ProCarList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_THISCIERRECIA2", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(intNumOpe), ParameterDirection.Input));
                _ProCarList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    numArchivo = x.GetInt32(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strModalidad = Convert.ToString(x.GetInt32(2)),
                    CodCia = x.GetString(3),
                    strAtiende = x.GetString(4),
                    strGana = x.GetString(5),
                    strCotiza = x.GetString(6),
                    strNumCot = x.GetString(7),
                    douPrima = Convert.ToString(x.GetDecimal(8)),
                    douPension = Convert.ToString(x.GetDecimal(9)),
                    strCodCia = Convert.ToString(x.GetDecimal(10)),
                    douPensionRT = Convert.ToString(x.GetDecimal(11)),
                    douTasaRT = Convert.ToString(x.GetDecimal(12)),
                    douPriUnicaAFP = Convert.ToString(x.GetDecimal(13))
                }).ToList();
                if (_ProCarList.Count != 0)
                {
                    for (int i = 0; i < _ProCarList.Count; i++)
                    {
                        try
                        {
                            query2 += "\nINSERT INTO PT_THIS_CIERRECIA(NUM_ARCHIVO, NUM_OPERACION, NUM_MODALIDAD,COD_CIA,IND_ATIENDE,";
                            if (_ProCarList[i].strGana != "") { query2 = query2 + "IND_GANA,"; }
                            if (_ProCarList[i].strCotiza != "") { query2 = query2 + "IND_COTIZA,"; }
                            if (_ProCarList[i].strNumCot != "") { query2 = query2 + "NUM_COT,"; }
                            query2 = query2 + " MTO_PRIMA,MTO_PENSION,PRC_TASACIA,MTO_PENSIONRT,PRC_TASACIART";
                            if (_ProCarList[i].douPriUnicaAFP != "") { query2 = query2 + ",MTO_PRIUNIAFP"; }
                            query2 = query2 + ")values(" + _ProCarList[i].numArchivo + "," + _ProCarList[i].intNumOpe + "," + _ProCarList[i].strModalidad + ",'" +
                                _ProCarList[i].CodCia + "','" + _ProCarList[i].strAtiende + "',";
                            if (_ProCarList[i].strGana != "") { query2 = query2 + "'" + _ProCarList[i].strGana + "',"; }
                            if (_ProCarList[i].strCotiza != "") { query2 = query2 + "'" + _ProCarList[i].strCotiza + "',"; }
                            if (_ProCarList[i].strNumCot != "") { query2 = query2 + "'" + _ProCarList[i].strNumCot + "',"; }
                            query2 = query2 + Convert.ToDouble(_ProCarList[i].douPrima) + "," + Convert.ToDouble(_ProCarList[i].douPension) + "," +
                                     Convert.ToDouble(_ProCarList[i].strCodCia) + "," + Convert.ToDouble(_ProCarList[i].douPensionRT) + "," + Convert.ToDouble(_ProCarList[i].douTasaRT);
                            if (_ProCarList[i].douPriUnicaAFP != "") { query2 = query2 + "," + _ProCarList[i].douPriUnicaAFP; }
                            query2 = query2 + ")";
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                return query2;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        ///Se realiza un select a la tabla pt_ttmp_cierresol para obtener los numeros de operacion
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta ejecutando</param>
        /// <returns> </returns>
        public string CargaThis_CierreSol(int numArch)
        {
            try
            {
                string query = "";
                List<ProCarArchivo> _ProCarList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_THISCIERRESOL", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                _ProCarList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    intNumOpe = Convert.ToInt32(x.GetDecimal(0))
                }).ToList();
                if (_ProCarList.Count != 0)
                {
                    for (int i = 0; i < _ProCarList.Count; i++)
                    {
                        query += "\n" + insertThisCierreSol(_ProCarList[i].intNumOpe);
                    }
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        ///Se realiza un select a la tabla PT_TTMP_CIERRESOL
        /// </summary>
        /// <param name="intNumOpe">Numero de operacion para realizar el select, recibido del metodo CargaThis_CierreSol</param>
        /// <returns> </returns>
        public string insertThisCierreSol(int intNumOpe)
        {
            try
            {
                string query = "";
                List<ProCarArchivo> _ProCarList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_THISCIERRESOL2", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(intNumOpe), ParameterDirection.Input));
                _ProCarList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    numArchivo = x.GetInt32(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strCussp = x.GetString(2),
                    strCodCierre = x.GetString(3),
                    strCodUsuario = x.GetString(4)
                }).ToList();
                if (_ProCarList.Count != 0)
                {
                    for (int i = 0; i < _ProCarList.Count; i++)
                    {
                        query += "\nINSERT INTO pt_this_cierresol(num_archivo, num_operacion, cod_cuspp,cod_cierre,cod_usuario) VALUES(" +
                            _ProCarList[i].numArchivo + "," + _ProCarList[i].intNumOpe + ",'" + _ProCarList[i].strCussp + "','" + _ProCarList[i].strCodCierre + "','" + _ProCarList[i].strCodUsuario + "')";
                    }
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        /// Se realiza un select a la tabla pt_ttmp_cierremod para obtener los numeros de operacion
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta trabajando</param>
        /// <returns> </returns>
        public string CargaThis_CierreMod(int numArch)
        {
            try
            {
                string query = "";
                List<ProCarArchivo> _ProCarList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_THISCIERREMOD", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                _ProCarList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    intNumOpe = Convert.ToInt32(x.GetDecimal(0))
                }).ToList();
                if (_ProCarList.Count != 0)
                {
                    for (int i = 0; i < _ProCarList.Count; i++)
                    {
                        query += "\n" + insertThisCierreMod(_ProCarList[i].intNumOpe);
                    }
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        ///Se realiza un select a la tabla pt_ttmp_cierremod
        /// </summary>
        /// <param name="intNumOpe">Numero de operacion recibida del metodo CargaThis_CierreMod</param>
        /// <returns> </returns>
        public string insertThisCierreMod(int intNumOpe)
        {
            try
            {
                string query2 = "";
                List<ProCarArchivo> _ProCarList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_THISCIERREMOD2", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(intNumOpe), ParameterDirection.Input));
                _ProCarList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    numArchivo = x.GetInt32(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strModalidad = Convert.ToString(x.GetInt32(2)),
                    strCodModalidad = x.GetString(3),
                    strMoneda = x.GetString(4),
                    numAnosRT = x.GetInt32(5),
                    prcRVD = Convert.ToDouble(x.GetDecimal(6)),
                    intPerGar = x.IsDBNull(7) ? -1 : Convert.ToInt32(x.GetString(7)),
                    codCoberCon = x.IsDBNull(8) ? "" : x.GetString(8),
                    strDerGra = x.IsDBNull(9) ? "" : x.GetString(9),
                    strDerCre = x.IsDBNull(10) ? "" : x.GetString(10),
                    strParCapital = x.IsDBNull(11) ? "" : x.GetString(11)
                }).ToList();
                if (_ProCarList.Count != 0)
                {
                    for (int i = 0; i < _ProCarList.Count; i++)
                    {
                        try
                        {
                            ProCarArchivo objPro = new ProCarArchivo();
                            if (homologarMoneda(_ProCarList[i].strMoneda, "", objPro) == false)
                            {
                                return "";
                            }
                            objPro.strTipRen = homologarCodigo(_ProCarList[i].strCodModalidad, "TR");
                            if (objPro.strTipRen == "")
                            {
                                objPro.strTipRen = "RP";
                            }
                            if (_ProCarList[i].intPerGar != -1)
                            {
                                objPro.strMod2 = "3";
                                objPro.intPerGar = _ProCarList[i].intPerGar * 12;
                            }
                            else
                            {
                                objPro.strMod2 = "1";
                                objPro.intPerGar = 0;
                            }
                            if (_ProCarList[i].numAnosRT != 0)
                            {
                                if (objPro.strTipRen == "6")
                                {
                                    objPro.intMesesDif = 0;
                                    objPro.douPorc = 0;
                                    objPro.intNumEsc = _ProCarList[i].numAnosRT * 12;
                                    objPro.douPrcRtaEsc = _ProCarList[i].prcRVD;
                                }
                                else
                                {
                                    //Para las demas Rentas (distintas a escalonadas) sigue haciendo lo mismo
                                    objPro.intMesesDif = _ProCarList[i].numAnosRT * 12;
                                    objPro.douPorc = _ProCarList[i].prcRVD;
                                    objPro.intNumEsc = 0;
                                    objPro.douPrcRtaEsc = 0;
                                }
                            }
                            else
                            {
                                objPro.intMesesDif = 0;
                                objPro.douPorc = 0;
                                objPro.intNumEsc = 0;
                                objPro.douPrcRtaEsc = 0;
                            }
                            if (_ProCarList[i].codCoberCon != "0")
                            {
                                objPro.strMod2 = "4";
                            }
                            query2 += "\ninsert into PT_THIS_CIERREMOD(NUM_ARCHIVO, NUM_OPERACION, NUM_MODALIDAD,COD_MONEDA,COD_TIPREN,NUM_MESDIF,COD_MODALIDAD,NUM_MESGAR," +
                                            "PRC_RENTATMP,COD_COBERCON,COD_DERGRA,COD_DERCRE";
                            if (_ProCarList[i].strParCapital != "") { query2 = query2 + ",COD_PARCAP"; }
                            if (objPro.strAjuste != "") { query2 = query2 + ",COD_TIPREAJUSTE"; }
                            query2 = query2 + ",NUM_MESESC,PRC_RENTAESC) VALUES (" + _ProCarList[i].numArchivo + "," + _ProCarList[i].intNumOpe + "," + _ProCarList[i].strModalidad + ",'" +
                                    objPro.strMoneda + "','" + objPro.strTipRen + "'," + objPro.intMesesDif + ",'" + objPro.strMod2 + "'," + objPro.intPerGar + "," + objPro.douPorc + ",'" + _ProCarList[i].codCoberCon + "','" +
                                    _ProCarList[i].strDerGra + "','" + _ProCarList[i].strDerCre + "'";
                            if (_ProCarList[i].strParCapital != "") { query2 = query2 + "," + _ProCarList[i].strParCapital; }
                            if (objPro.strAjuste != "") { query2 = query2 + "," + objPro.strAjuste; }
                            query2 = query2 + "," + objPro.intNumEsc + "," + objPro.douPrcRtaEsc + ")";

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                return query2;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        /// Homologa la moneda
        /// </summary>
        /// <param name="moneda">Codigo de la moneda a homologar</param>
        /// <returns> regresa un true si se pudo homologar</returns>
        public bool homologarMoneda(string moneda, string ajuste, ProCarArchivo objPro)
        {
            bool valor = false;
            try
            {
                ProCarArchivo _ProCarA = new ProCarArchivo();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "HOMOLOGARMONEDA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codScomp", SqlDbType.VarChar, moneda, ParameterDirection.Input));
                _ProCarA = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    strMoneda = x.GetString(0),
                    strAjuste = x.GetString(1)
                }).FirstOrDefault();

                if (_ProCarA != null)
                {
                    objPro.strMoneda = _ProCarA.strMoneda;
                    objPro.strAjuste = _ProCarA.strAjuste;
                    valor = true;
                }
                return valor;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 19-10-2018
        /// Homologa codigo de moneda
        /// </summary>
        /// <param name="strCod">Codigo de la moneda a homologar</param>
        /// <param name="vgCodTabla">Codigo de la tabla para homologar</param>
        /// <returns> regresa el valor del cofigo homologado</returns>
        public string homologarCodigo(string strCod, string vgCodTabla)
        {
            try
            {
                string valor = "";
                ProCarArchivo _ProCarA = new ProCarArchivo();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "HOMOLOGARCODIGO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTabla", SqlDbType.VarChar, vgCodTabla, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codScomp", SqlDbType.VarChar, strCod, ParameterDirection.Input));
                _ProCarA = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    strMoneda = x.GetString(0)
                }).FirstOrDefault();

                if (_ProCarA != null)
                {
                    valor = _ProCarA.strMoneda;
                }
                return valor;
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Calcula las estadisticas de las operaciones realizadas con exito y las erroneas
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta trabajando</param>
        /// <param name="usuario">Codigo de usuario que esta utilizando el sistema</param>
        /// <param name="codCia">Codigo cia del la compañia que utiliza el sistema</param>
        /// <returns> regresa un objeto con las estadisticas obtenidas </returns>
        public Response estadisticas(int numArch, string usuario, string codCia)
        {
            int intOKcierresol, intOKcierremod, intOKcierrecia, intOKcierreafp;
            int intERRcierresol, intERRcierremod, intERRcierrecia, intERRcierreafp;
            int intGanadas, intPerdidas, intAFP, intRecotizadas, intDesistidas, intCaducadas;

            Response resCarga = new Response();
            string[] infoCarga = new string[7];
            try
            {
                List<ProCarArchivo> _ProCarArchivo = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ESTADISTICAS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCia", SqlDbType.VarChar, codCia, ParameterDirection.Input));
                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    success = x.GetInt32(1)
                }).ToList();

                intOKcierresol = _ProCarArchivo[0].success;
                intERRcierresol = _ProCarArchivo[1].success;
                intOKcierremod = _ProCarArchivo[2].success;
                intERRcierremod = _ProCarArchivo[3].success;
                intOKcierrecia = _ProCarArchivo[4].success;
                intERRcierrecia = _ProCarArchivo[5].success;
                intOKcierreafp = _ProCarArchivo[6].success;
                intERRcierreafp = _ProCarArchivo[7].success;
                intGanadas = _ProCarArchivo[8].success;
                intPerdidas = _ProCarArchivo[9].success;
                intAFP = _ProCarArchivo[10].success;
                intRecotizadas = _ProCarArchivo[11].success;
                intDesistidas = _ProCarArchivo[12].success;
                intCaducadas = _ProCarArchivo[13].success;

                infoCarga[0] = (intGanadas + intPerdidas + intAFP + intRecotizadas + intDesistidas + intCaducadas).ToString();
                infoCarga[1] = (intPerdidas).ToString();
                infoCarga[2] = (intGanadas).ToString();
                infoCarga[3] = (intAFP).ToString();
                infoCarga[4] = (intRecotizadas).ToString();
                infoCarga[5] = (intDesistidas).ToString();
                infoCarga[6] = (intCaducadas).ToString();

                try
                {
                    DateTime fecha = DateTime.Now;
                    string query = "insert into pt_this_estcarcie(num_archivo, num_regoksol, num_regerrsol, num_regokmod,num_regerrmod,num_regokcia,num_regerrcia," +
                                    "num_regokafp,num_regerrafp,num_ganadas,num_perdidas,num_afp,num_recotizadas,num_desistidas,num_caducadas,cod_usuariocrea,fec_crea,hor_crea) values (" +
                                    numArch + "," + intOKcierresol + "," + intERRcierresol + "," + intOKcierremod + "," + intERRcierremod + "," + intOKcierrecia + "," + intERRcierrecia + "," +
                                    intOKcierreafp + "," + intERRcierreafp + "," + intGanadas + "," + intPerdidas + "," + intAFP + "," + intRecotizadas + "," +
                                    intDesistidas + "," + intCaducadas + ",'" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";

                    SRVDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                    {
                    }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw;
                }

                resCarga.Object = infoCarga;
                resCarga.IsOk = true;
                return resCarga;
            }
            catch (Exception ex)
            {
                Response resCarga2 = new Response();
                resCarga2.IsOk = false;
                resCarga2.Message = ex.Message;
                return resCarga2;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Obtiene estadisticas calculadas con exito
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta trabajando</param>
        /// <param name="tabla">Nombre de la tabla que se realizara la cosulta</param>
        /// <param name="par">Parametro que identifica que hacer con el parametro a obtener si un count o un count y un distinct</param>
        /// <returns> regresa el numero de estadisticas cargadas con exito </returns>
        public int estadisticasOK(string tabla, int numArch, string par)
        {
            try
            {
                ProCarArchivo _ProCarArchivo = new ProCarArchivo();
                string query = "SELECT " + par + " as total FROM " + tabla + " WHERE cod_error = '0' and num_archivo = " + numArch;
                int valor = 0;

                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_ProCarArchivo != null)
                {
                    valor = _ProCarArchivo.success;
                }

                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Obtiene estadisticas erroneas calculadas
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta trabajando</param>
        /// <param name="tabla">Nombre de la tabla que se realizara la cosulta</param>
        /// <param name="par">Parametro que identifica que hacer con el parametro a obtener si un count o un count y un distinct</param>
        /// <returns> regresa el numero de estadisticas cargadas con exito </returns>
        public int estadisticasERR(string tabla, int numArch, string par)
        {
            ProCarArchivo _ProCarArchivo = new ProCarArchivo();
            try
            {
                string query = "SELECT " + par + "  as total FROM " + tabla + " WHERE cod_error <> '0' and num_archivo = " + numArch;
                int valor = 0;

                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_ProCarArchivo != null)
                {
                    valor = _ProCarArchivo.success;
                }

                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Obtiene estadisticas calculadas con exito de la tabla PT_THIS_CIERRECIA
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta trabajando</param>
        /// <param name="dif">Parametro que identifica que hacer con el parametro a comparar, si un diferente o una igualacion</param>
        /// <param name="CodCia">Codigo cia que utiliza la empreasa que esta ejecutando el sistema</param>
        /// <returns> regresa el numero de estadisticas cargadas con exito </returns>
        public int ganadasCIA(string dif, int numArch, string CodCia)
        {
            ProCarArchivo _ProCarArchivo = new ProCarArchivo();
            try
            {
                string query = "SELECT COUNT(C.NUM_OPERACION) as total FROM PT_THIS_CIERRECIA C, PT_THIS_CIERRESOL S " +
                               " WHERE " + dif + CodCia + "' AND C.IND_GANA='S' AND C.NUM_ARCHIVO=S.NUM_ARCHIVO AND C.NUM_OPERACION= S.NUM_OPERACION " +
                               " AND S.COD_CIERRE='EL' AND C.NUM_ARCHIVO=" + numArch;
                int valor = 0;

                _ProCarArchivo = SRVDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_ProCarArchivo != null)
                {
                    valor = _ProCarArchivo.success;
                }

                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Obtiene estadisticas calculadas con exito de las tablas PT_THIS_CIERRESOL y PT_THIS_CIERREAFP
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta trabajando</param>
        /// <returns> regresa el numero de estadisticas cargadas con exito </returns>
        public int ganadasAFP(int numArch)
        {
            ProCarArchivo _ProCarArchivo = new ProCarArchivo();
            try
            {
                int valor = 0;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GANADASAFP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_ProCarArchivo != null)
                {
                    valor = _ProCarArchivo.success;
                }

                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Obtiene estadisticas calculadas con exito de las tablas PT_THIS_CIERRESOL
        /// </summary>
        /// <param name="numArch">Numero de archivo con el que se esta trabajando</param>
        /// <param name="codCierre">Codigo de cierre</param>
        /// <returns> regresa el numero de estadisticas cargadas con exito </returns>
        public int estadisticasRDC(int numArch, string codCierre)
        {
            ProCarArchivo _ProCarArchivo = new ProCarArchivo();
            try
            {
                string query = "SELECT count(distinct(num_operacion)) as total FROM PT_THIS_CIERRESOL" +
                               " WHERE COD_CIERRE='" + codCierre + "' AND NUM_ARCHIVO=" + numArch;
                int valor = 0;

                _ProCarArchivo = SRVDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_ProCarArchivo != null)
                {
                    valor = _ProCarArchivo.success;
                }

                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Obtiene el codigo cia
        /// </summary>
        /// <returns> regresa el codigo cia que utiliza la compañia que esta ejecutando el sistema</returns>
        public string vgCodInternoCia()
        {
            ProCarArchivo _ProCarArchivo = new ProCarArchivo();
            try
            {
                string valor = "";
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CODINTERNOCIA", ParameterDirection.Input));
                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    strCodCia = x.IsDBNull(0) ? "" : x.GetString(0)
                }).FirstOrDefault();

                if (_ProCarArchivo != null)
                {
                    valor = _ProCarArchivo.strCodCia;
                }

                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Elimina los registros de tablas que no tienen errores
        /// </summary>
        /// <returns> regresa una cadena si encontro error en al eliminar los registros </returns>
        public string eliminaSinError()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ELININASINERROR", ParameterDirection.Input));
                VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                }).FirstOrDefault();

                return "";
            }
            catch (Exception)
            {
                return "Error producido al eliminar registros sin error de tablas temporales";
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Realiza un select a las tablas PT_THIS_CIERRESOL, PT_THIS_CIERREMOD, PT_THIS_CIERRECIA 
        /// </summary>
        /// <param name="CodCia">Codigo cia interno</param>
        /// <param name="numArch">Numero de archivo con el que se esta trabajando</param>
        /// <param name="usuario">Usuario que esta ejecutando el sistema</param>
        /// <returns> regresa vacio o error dependiendo de el exito de las operaciones</returns>
        public string aceptaCotizacion(string CodCia, int numArch, string usuario)
        {
            try
            {
                XmlConfigurator.Configure();
                _log.Info("Consulta CiereSOL MOD Y CIA");
                _log.Info("parametros CodCia" + CodCia + "NumArch " + numArch + "Usuario " + usuario);
                List<ProCarArchivo> _ProCarArchivoList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_ACEPTACOTIZACION", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCia", SqlDbType.VarChar, CodCia, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                _ProCarArchivoList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    intNumOpe = Convert.ToInt32(x.GetDecimal(0)),
                    strNumCot = x.IsDBNull(1) ? "" : x.GetString(1),
                    douPrima = Convert.ToString(x.GetDecimal(2)),
                    douPension = Convert.ToString(x.GetDecimal(3)),
                    douTasaCia = Convert.ToDouble(x.GetDecimal(4)),
                    douPensionRT = Convert.ToString(x.GetDecimal(5)),
                    prcTasaCiaRT = Convert.ToString(x.GetDecimal(6)),
                    strMoneda = x.GetString(7),
                    strTipRen = x.GetString(8),
                    intMesesDif = x.GetInt32(9),
                    strCodModalidad = x.GetString(10),
                    numMesGar = x.GetInt32(11),
                    prcRentaTMP = Convert.ToDouble(x.GetDecimal(12)),
                    codCoberCon = x.GetString(13),
                    strDerGra = x.IsDBNull(14) ? "" : x.GetString(14),
                    strDerCre = x.IsDBNull(15) ? "" : x.GetString(15),
                    strCussp = x.GetString(16),
                    numMesesC = x.IsDBNull(17) ? 0 : x.GetInt32(17),
                    douPrcRtaEsc = x.IsDBNull(18) ? 0 : Convert.ToDouble(x.GetDecimal(18)),
                    codTipreajuste = x.IsDBNull(19) ? "" : x.GetString(19)
                }).ToList();
                _log.Info("informacion obtenida " + _ProCarArchivoList.Count);
                if (_ProCarArchivoList.Count != 0)
                {
                    for (int i = 0; i < _ProCarArchivoList.Count; i++)
                    {
                        aceptaCotizacion2(_ProCarArchivoList[i], "E", usuario);
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "error";
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Realiza un select a las tablas PT_TMAE_COTIZACION y PT_TMAE_DETCOTIZACION con los datos obtenidos en el metodo aceptaCotizacion
        /// </summary>
        /// <param name="datos">datos de la consulta que se hzo en el metodo aceptaCotizacion</param>
        /// <param name="vgEstEnv">Codigo del estado de la cotizacion</param>
        /// <param name="usuario">Usuario que esta ejecutando el sistema</param>
        /// <returns>  </returns>
        public void aceptaCotizacion2(ProCarArchivo datos, string vgEstEnv, string usuario)
        {
            try
            {
                XmlConfigurator.Configure();
                _log.Info("Select a las tablas DetCotizacion y Cotizacion");
                _log.Info("Conexion" + connectionString);
                ProCarArchivo _ProCarArchivo = new ProCarArchivo();
                string query = "SELECT C.NUM_COT, D.NUM_OPERACION,D.NUM_CORRELATIVO FROM PT_TMAE_COTIZACION C, PT_TMAE_DETCOTIZACION D" +
                               " WHERE C.NUM_COT=D.NUM_COT AND C.NUM_OPERACION=D.NUM_OPERACION AND C.NUM_COT='" + datos.strNumCot + "'  AND C.NUM_OPERACION=" + datos.intNumOpe +
                               " AND D.COD_MONEDA='" + datos.strMoneda + "' AND D.COD_TIPREN='" + datos.strTipRen + "' AND D.NUM_MESDIF=" + datos.intMesesDif + " AND D.COD_MODALIDAD='" + datos.strCodModalidad + "'" +
                               " AND D.NUM_MESGAR=" + datos.numMesGar + " AND D.PRC_RENTATMP=" + datos.prcRentaTMP + " AND D.COD_COBERCON='" + datos.codCoberCon + "' AND D.COD_DERGRA='" + datos.strDerGra + "'" +
                               " AND D.COD_DERCRE='" + datos.strDerCre + "' AND C.COD_CUSPP='" + datos.strCussp + "' AND D.NUM_MESESC=" + datos.numMesesC + " AND D.PRC_RENTAESC=" + datos.douPrcRtaEsc +
                               " AND D.COD_TIPREAJUSTE=" + datos.codTipreajuste + " AND D.COD_ESTCOT =  '" + vgEstEnv + "'";
                _ProCarArchivo = SRVDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                    strNumCot = x.GetString(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strNumCor = x.GetInt32(2)

                }).FirstOrDefault();
                _log.Info("Informacion de la consulta realizada" + query );
                if (_ProCarArchivo != null)
                {
                    _ProCarArchivo.fecCierre = busca_FechaServidor();
                    modSolicitud(_ProCarArchivo, vgEstEnv, usuario);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Realiza un select a las tablas PT_TMAE_DETCOTIZACION y PT_TMAE_COTBEN
        /// </summary>
        /// <param name="datos">datos de la consulta que se hizo en el metodo aceptaCotizacion2</param>
        /// <param name="vgEstEnv">Codigo del estado de la cotizacion</param>
        /// <param name="usuario">Usuario que esta ejecutando el sistema</param>
        /// <returns>  </returns>
        public void modSolicitud(ProCarArchivo datos, string vgEstEnv, string usuario)
        {
            XmlConfigurator.Configure();
            _log.Info("Select a DetCot y CotBen(Antes de cambiar el CodEstCot)");
            double douMtoPension, douPrcPension, douMtoPenFinal;
            int intNroOrden, intMesGar;
            douMtoPension = 0;
            douPrcPension = 0;
            intNroOrden = 0;
            douMtoPenFinal = 0;
            try
            {
                List<ProCarArchivo> _ProCarArchivoList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_MODSOLICITUD", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstcot", SqlDbType.VarChar, vgEstEnv, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datos.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCor", SqlDbType.Int, datos.strNumCor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, datos.intNumOpe, ParameterDirection.Input));
                _ProCarArchivoList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    strNumCot = x.GetString(0),
                    strNumCor = x.GetInt32(1),
                    douPension = Convert.ToString(x.GetDecimal(2)),
                    numOrden = x.GetInt32(3),
                    prcPension = Convert.ToDouble(x.GetDecimal(4)),
                    numMesesC = x.GetInt32(5),
                    indCalsofDif = x.IsDBNull(6) ? "" : x.GetString(6),
                    prcPensionDIF = x.IsDBNull(7) ? 0 : Convert.ToDouble(x.GetDecimal(7))
                }).ToList();
                if (_ProCarArchivoList.Count != 0)
                {
                    for (int i = 0; i < _ProCarArchivoList.Count; i++)
                    {
                        douMtoPension = _ProCarArchivoList[i].prcPensionDIF;
                        if (_ProCarArchivoList[i].indCalsofDif == "S")
                        {
                            douPrcPension = _ProCarArchivoList[i].prcPensionDIF;
                        }
                        else
                        {
                            douPrcPension = _ProCarArchivoList[i].prcPension;
                        }
                        intNroOrden = _ProCarArchivoList[i].numOrden;
                        intMesGar = _ProCarArchivoList[i].numMesGar;
                        douMtoPenFinal = douMtoPension * (douPrcPension / 100);
                        updateCotBen(douMtoPenFinal, datos.strNumCot, datos.intNumOpe, intNroOrden);
                    }
                }
                updateDetoCotizacion("A", datos.fecCierre, datos.strNumCot, datos.strNumCor, datos.intNumOpe, vgEstEnv);
                //Graba en la tabla de etapas de la cotización
                string cod_Etapa = "150";
                string fec_ini = busca_FechaServidor();
                string hora_ini = busca_HoraServidor();
                string cod_EtapaAnt = etapaAnterior(datos.strNumCot, datos.strNumCor);
                bool bandValidar = validar(datos.strNumCot, datos.strNumCor, cod_Etapa, fec_ini, hora_ini);
                if (bandValidar == false)
                {
                    //no existe etapa
                    insert_Ingreso_Envio(datos.strNumCot, datos.strNumCor, cod_Etapa, fec_ini, hora_ini, cod_EtapaAnt, usuario);
                }
                else
                {
                    //actualizar
                    actualizarEtapa(datos.strNumCot, datos.strNumCor, cod_Etapa, fec_ini, hora_ini, cod_EtapaAnt, usuario);
                }
                //Traspasa la información a las tablas temporales
                traspaso_aceptada(datos.strNumCot, datos.strNumCor, usuario);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Traspasa las cotizaciones aceptadas a las tablas  cotizacion y detcotizacion
        /// </summary>
        /// <param name="strNumCot">numero de cotizacion</param>
        /// <param name="strNumCor">numero correlativo </param>
        /// <param name="usuario">Usuario que esta ejecutando el sistema</param>
        /// <returns>  </returns>
        public void traspaso_aceptada(string strNumCot, int strNumCor, string usuario)
        {
            traspaso_aceptada_Cotizacion(strNumCot, strNumCor, usuario);
            traspaso_aceptada_Detcotizacion(strNumCot, strNumCor, usuario);
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Se realiza un select a la tabla PT_TMAE_COTIZACION para obtener la mayoria de sus registros mediante su numero de cotizacion
        /// </summary>
        /// <param name="strNumCot">numero de cotizacion</param>
        /// <param name="strNumCor">numero correlativo </param>
        /// <param name="usuario">Usuario que esta ejecutando el sistema</param>
        /// <returns>  </returns>
        public void traspaso_aceptada_Cotizacion(string strNumCot, int strNumCor, string usuario)
        {
            try
            {
                List<ProCarArchivo> _ProCarArchivoList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TRASACECOT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, strNumCot, ParameterDirection.Input));
                _ProCarArchivoList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    strNumCot = x.GetString(0),
                    numArchivo = x.GetInt32(1),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(2)),
                    fecSubscripcion = x.GetString(3),
                    fecEnvio = x.GetString(4),
                    fecCierre = x.GetString(5),
                    FecDev = x.GetString(6),
                    strCodAfp = x.GetString(7),
                    codIsapre = x.GetString(8),
                    codTipPension = x.GetString(9),
                    codVejez = x.IsDBNull(10) ? "" : x.GetString(10),
                    codEstCivil = x.IsDBNull(11) ? "" : x.GetString(11),
                    codCliente = x.GetString(12),
                    strCussp = x.GetString(13),
                    codTipoIden = x.GetInt32(14),
                    numIden = x.GetString(15),
                    direccion = x.IsDBNull(16) ? "" : x.GetString(16),
                    codDireccion = x.GetInt32(17),
                    glsFondo = x.IsDBNull(18) ? "" : x.GetString(18),
                    glsCorreo = x.IsDBNull(19) ? "" : x.GetString(19),
                    codViapago = x.GetString(20),
                    codTipCuenta = x.GetString(21),
                    codBanco = x.GetString(22),
                    numCuenta = x.IsDBNull(23) ? "" : x.GetString(23),
                    codSucursal = x.GetString(24),
                    numAnnoJub = x.GetInt32(25),
                    numCargas = x.GetInt32(26),
                    codTipoIdenCor = x.GetInt32(27),
                    numIdenCor = x.IsDBNull(28) ? "" : x.GetString(28),
                    codBenSocial = x.GetString(29),
                    codMonedaFon = x.GetString(30),
                    mtoMonedaFon = Convert.ToDouble(x.GetDecimal(31)),
                    mtoPriuniFon = Convert.ToDouble(x.GetDecimal(32)),
                    mtoCataIndFon = Convert.ToDouble(x.GetDecimal(33)),
                    mtoBonoFon = Convert.ToDouble(x.GetDecimal(34)),
                    mtoPriuni = Convert.ToDouble(x.GetDecimal(35)),
                    mtoCtaInd = Convert.ToDouble(x.GetDecimal(36)),
                    mtoBono = Convert.ToDouble(x.GetDecimal(37)),
                    prcTasaPRT = Convert.ToDouble(x.GetDecimal(38)),
                    mtoApoadi = Convert.ToDouble(x.GetDecimal(39)),
                    indCob = x.GetString(40),
                    CodTipCot = x.GetString(41),
                    strCodUsuario = x.IsDBNull(42) ? "" : x.GetString(42),
                    fecCrea = x.IsDBNull(43) ? "" : x.GetString(43),
                    horaCrea = x.IsDBNull(44) ? "" : x.GetString(44),
                    codUsuarioModi = x.IsDBNull(45) ? "" : x.GetString(45),
                    fecModi = x.IsDBNull(46) ? "" : x.GetString(46),
                    horModi = x.IsDBNull(47) ? "" : x.GetString(47),
                    indEstado = x.GetString(48),
                    codRegion = x.IsDBNull(49) ? "" : x.GetString(49)
                }).ToList();
                if (_ProCarArchivoList.Count != 0)
                {
                    for (int i = 0; i < _ProCarArchivoList.Count; i++)
                    {
                        insertCotizacion(_ProCarArchivoList[i], usuario);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Se realiza un insert a la tabla PT_TTMP_COTIZACION con los datos obtenidos en el metodo traspaso_aceptada_Cotizacion
        /// </summary>
        /// <param name="datos">Datos obtenidos en la consulta que se realiza en el metodo traspaso_aceptada_Cotizacion</param>
        /// <param name="usuario">Usuario que esta ejecutando el sistema</param>
        /// <returns>  </returns>
        public void insertCotizacion(ProCarArchivo datos, string usuario)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                string query = "INSERT INTO PT_TTMP_COTIZACION (NUM_COT,NUM_ARCHIVO,NUM_OPERACION,FEC_SUSCRIPCION,FEC_ENVIO,FEC_CIERRE,FEC_DEV,COD_AFP,COD_ISAPRE," +
                               "COD_TIPPENSION,COD_VEJEZ,COD_ESTCIVIL,COD_CLIENTE,COD_CUSPP,COD_TIPOIDEN,NUM_IDEN,GLS_DIRECCION,COD_DIRECCION,GLS_FONO,GLS_CORREO,COD_VIAPAGO," +
                               "COD_TIPCUENTA,COD_BANCO,NUM_CUENTA,COD_SUCURSAL,NUM_ANNOJUB,NUM_CARGAS,COD_TIPOIDENCOR,NUM_IDENCOR,COD_BENSOCIAL,COD_MONEDAFON,MTO_MONEDAFON,MTO_PRIUNIFON," +
                               "MTO_CTAINDFON,MTO_BONOFON,MTO_PRIUNI,MTO_CTAIND,MTO_BONO,PRC_TASARPRT,MTO_APOADI,IND_COB,COD_TIPCOT,";
                if (datos.codRegion != "") { query = query + "COD_REGION,"; }
                query = query + "COD_USUARIOCREA,FEC_CREA,HOR_CREA,COD_USUARIOMODI,FEC_MODI,HOR_MODI,IND_ESTADO,COD_USUARIOTRAS,FEC_TRAS,HOR_TRAS) VALUES ('" + datos.strNumCot + "'," +
                        datos.numArchivo + "," + datos.intNumOpe + ",'" + datos.fecSubscripcion + "','" + datos.fecEnvio + "','" + datos.fecCierre + "','" + datos.FecDev + "','" + datos.strCodAfp + "','" +
                        datos.codIsapre + "','" + datos.codTipPension + "','" + datos.codVejez + "','" + datos.codEstCivil + "','" + datos.codCliente + "','" + datos.strCussp + "'," + datos.codTipoIden + ",'" +
                        datos.numIden + "','" + datos.direccion + "'," + datos.codDireccion + ",'" + datos.glsFondo + "','" + datos.glsCorreo + "','" + datos.codViapago + "','" + datos.codTipCuenta + "','" + datos.codBanco + "','" +
                        datos.numCuenta + "','" + datos.codSucursal + "'," + datos.numAnnoJub + "," + datos.numCargas + "," + datos.codTipoIdenCor + ",'" + datos.numIdenCor + "','" + datos.codBenSocial + "','" +
                        datos.codMonedaFon + "'," + datos.mtoMonedaFon + "," + datos.mtoPriuniFon + "," + datos.mtoCataIndFon + "," + datos.mtoBonoFon + "," + datos.mtoPriuni + "," + datos.mtoCtaInd + "," +
                        datos.mtoBono + "," + datos.prcTasaPRT + "," + datos.mtoApoadi + ",'" + datos.indCob + "','" + datos.CodTipCot + "',";
                if (datos.codRegion != "") { query = query + "'" + datos.codRegion + "',"; }
                query = query + "'" + datos.strCodUsuario + "','" + datos.fecCrea + "','" + datos.horaCrea + "','" + datos.codUsuarioModi + "','" + datos.fecModi + "','" + datos.horModi + "','" +
                        datos.indEstado + "','" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "')";
                SRVDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 22-10-2018
        /// Se realiza un select a la tabla PT_TMAE_DETCOTIZACION para obtener todos sus registros
        /// </summary>
        /// <param name="strNumCot">Numero de cotizacion</param>
        /// <param name="strNumCor">Numero correlativo</param>
        /// <param name="usuario">Usuario que esta ejecutando el sistema</param>
        /// <returns>  </returns>
        public void traspaso_aceptada_Detcotizacion(string strNumCot, int strNumCor, string usuario)
        {
            try
            {
                List<ProCarArchivo> _ProCarArchivoList = new List<ProCarArchivo>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TRASACEDETCOT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCor", SqlDbType.Int, strNumCor, ParameterDirection.Input));
                _ProCarArchivoList = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    strNumCot = x.GetString(0),
                    strNumCor = x.GetInt32(1),
                    numArchivo = x.GetInt32(2),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(3)),
                    fecCalculo = x.IsDBNull(4) ? "" : x.GetString(4),
                    strMoneda = x.IsDBNull(5) ? "" : x.GetString(5),
                    mtoValMoneda = Convert.ToDouble(x.GetDecimal(6)),
                    mtoPriuni = Convert.ToDouble(x.GetDecimal(7)),
                    mtoCtaInd = Convert.ToDouble(x.GetDecimal(8)),
                    mtoBono = Convert.ToDouble(x.GetDecimal(9)),
                    prcCorCom = Convert.ToDouble(x.GetDecimal(10)),
                    prcCorComReal = Convert.ToDouble(x.GetDecimal(11)),
                    mtoCorCom = Convert.ToDouble(x.GetDecimal(12)),
                    strTipRen = x.GetString(13),
                    intMesesDif = x.GetInt32(14),
                    strCodModalidad = x.GetString(15),
                    numMesGar = x.GetInt32(16),
                    prcRentaAFP = Convert.ToDouble(x.GetDecimal(17)),
                    prcRentaAfpori = Convert.ToDouble(x.GetDecimal(18)),
                    prcRentaTMP = Convert.ToDouble(x.GetDecimal(19)),
                    mtoFacPenella = Convert.ToDouble(x.GetDecimal(20)),
                    prcFacPanella = Convert.ToDouble(x.GetDecimal(21)),
                    mtoCuomor = Convert.ToDouble(x.GetDecimal(22)),
                    prcTasaTce = Convert.ToDouble(x.GetDecimal(23)),
                    prcTasaVta = Convert.ToDouble(x.GetDecimal(24)),
                    codTipTir = x.IsDBNull(25) ? "" : x.GetString(25),
                    prcTasaTir = Convert.ToDouble(x.GetDecimal(26)),
                    prcTasaPerGar = Convert.ToDouble(x.GetDecimal(27)),
                    mtoCnu = Convert.ToDouble(x.GetDecimal(28)),
                    mtoPriuniSim = Convert.ToDouble(x.GetDecimal(29)),
                    mtoPriuniDif = Convert.ToDouble(x.GetDecimal(30)),
                    douPension = Convert.ToString(x.GetDecimal(31)),
                    mtoPensionGar = Convert.ToDouble(x.GetDecimal(32)),
                    mtoCtaIndAfp = Convert.ToDouble(x.GetDecimal(33)),
                    mtoRentaTmpAfp = Convert.ToDouble(x.GetDecimal(34)),
                    mtoResmat = Convert.ToDouble(x.GetDecimal(35)),
                    mtoValPrePenTMP = Convert.ToDouble(x.GetDecimal(36)),
                    mtoSumPension = Convert.ToDouble(x.GetDecimal(37)),
                    mtoPenAnual = Convert.ToDouble(x.GetDecimal(38)),
                    mtoRmPension = Convert.ToDouble(x.GetDecimal(39)),
                    mtoRmgtosep = Convert.ToDouble(x.GetDecimal(40)),
                    mtoPercon = Convert.ToDouble(x.GetDecimal(41)),
                    prcPercon = Convert.ToDouble(x.GetDecimal(42)),
                    codCoberCon = x.GetString(43),
                    strDerCre = x.GetString(44),
                    strDerGra = x.GetString(45),
                    codEstCot = x.GetString(46),
                    fecAcepta = x.IsDBNull(47) ? "" : x.GetString(47),
                    codRechazo = x.GetString(48),
                    strCodUsuario = x.IsDBNull(49) ? "" : x.GetString(49),
                    fecCrea = x.IsDBNull(50) ? "" : x.GetString(50),
                    horaCrea = x.IsDBNull(51) ? "" : x.GetString(51),
                    codUsuarioModi = x.IsDBNull(52) ? "" : x.GetString(52),
                    fecModi = x.IsDBNull(53) ? "" : x.GetString(53),
                    horModi = x.IsDBNull(54) ? "" : x.GetString(54),
                    mtoRmgTosepRV = Convert.ToDouble(x.GetDecimal(55)),
                    mtoAjusteIPC = Convert.ToDouble(x.GetDecimal(56)),
                    indCalSobDif = x.IsDBNull(57) ? "" : x.GetString(57),
                    codTipreajuste = x.IsDBNull(58) ? "" : x.GetString(58),
                    mtoValReajusteTri = x.IsDBNull(59) ? 0 : Convert.ToDouble(x.GetDecimal(59)),
                    mtoValReajusteMen = x.IsDBNull(60) ? 0 : Convert.ToDouble(x.GetDecimal(60)),
                    numMesesC = x.IsDBNull(61) ? 0 : x.GetInt32(61),
                    douPrcRtaEsc = x.IsDBNull(62) ? 0 : Convert.ToDouble(x.GetDecimal(62))
                }).ToList();

                if (_ProCarArchivoList.Count != 0)
                {
                    for (int i = 0; i < _ProCarArchivoList.Count; i++)
                    {
                        insertDetCotizacion(_ProCarArchivoList[i], usuario, strNumCot);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 23-10-2018
        /// Se realiza un insert a la tabla PT_TTMP_DETCOTIZACION con los datos obtenidos en el metodo traspaso_aceptada_Detcotizacion 
        /// </summary>
        /// <param name="d">Datos obtenidos en el metodo traspaso_aceptada_Detcotizacion</param>
        /// <param name="numCot">Numero de cotizacion</param>
        /// <param name="usuario">Usuario que esta ejecutando el sistema</param>
        /// <returns>  </returns>
        public void insertDetCotizacion(ProCarArchivo d, string usuario, string numCot)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                string query = "INSERT INTO PT_TTMP_DETCOTIZACION (NUM_COT,NUM_CORRELATIVO,NUM_ARCHIVO,NUM_OPERACION,FEC_CALCULO,COD_MONEDA,MTO_VALMONEDA,MTO_PRIUNIMOD,MTO_CTAINDMOD,MTO_BONOMOD," +
                               "PRC_CORCOM,PRC_CORCOMREAL,MTO_CORCOM,COD_TIPREN,NUM_MESDIF,COD_MODALIDAD,NUM_MESGAR,PRC_RENTAAFP,PRC_RENTAAFPORI,PRC_RENTATMP,MTO_FACPENELLA," +
                               "PRC_FACPENELLA,MTO_CUOMOR,PRC_TASATCE,PRC_TASAVTA,COD_TIPTIR,PRC_TASATIR,PRC_TASAPERGAR,MTO_CNU,MTO_PRIUNISIM,MTO_PRIUNIDIF,MTO_PENSION,MTO_PENSIONGAR," +
                               "MTO_CTAINDAFP,MTO_RENTATMPAFP,MTO_RESMAT,MTO_VALPREPENTMP,MTO_SUMPENSION,MTO_PENANUAL,MTO_RMPENSION,MTO_RMGTOSEP,MTO_PERCON,PRC_PERCON,COD_COBERCON,COD_DERCRE," +
                               "COD_DERGRA,COD_ESTCOT,FEC_ACEPTA,COD_RECHAZO,COD_USUARIOCREA,FEC_CREA,HOR_CREA,COD_USUARIOMODI,FEC_MODI,HOR_MODI,COD_USUARIOTRAS,FEC_TRAS,HOR_TRAS,MTO_RMGTOSEPRV,MTO_AJUSTEIPC," +
                               "IND_CALSOBDIF,COD_TIPREAJUSTE,MTO_VALREAJUSTETRI,MTO_VALREAJUSTEMEN,NUM_MESESC,PRC_RENTAESC) VALUES ('" + d.strNumCot + "'," + d.strNumCor + "," + d.numArchivo + "," + d.intNumOpe + ",'" +
                               d.fecCalculo + "','" + d.strMoneda + "'," + d.mtoValMoneda + "," + d.mtoPriuni + "," + d.mtoCtaInd + "," + d.mtoBono + "," + d.prcCorCom + "," + d.prcCorComReal + "," + d.mtoCorCom + ",'" + d.strTipRen + "'," +
                               d.intMesesDif + ",'" + d.strCodModalidad + "'," + d.numMesGar + "," + d.prcRentaAFP + "," + d.prcRentaAfpori + "," + d.prcRentaTMP + "," + d.mtoFacPenella + "," + d.prcFacPanella + "," + d.mtoCuomor + "," +
                               d.prcTasaTce + "," + d.prcTasaVta + ",'" + d.codTipTir + "'," + d.prcTasaTir + "," + d.prcTasaPerGar + "," + d.mtoCnu + "," + d.mtoPriuniSim + "," + d.mtoPriuniDif + "," + d.douPension + "," + d.mtoPensionGar + "," + d.mtoCtaIndAfp + "," +
                               d.mtoRentaTmpAfp + "," + d.mtoResmat + "," + d.mtoValPrePenTMP + "," + d.mtoSumPension + "," + d.mtoPenAnual + "," + d.mtoRmPension + "," + d.mtoRmgtosep + "," + d.mtoPercon + "," + d.prcPercon + ",'" + d.codCoberCon + "','" +
                               d.strDerCre + "','" + d.strDerGra + "','" + d.codEstCot + "','" + d.fecAcepta + "','" + d.codRechazo + "','" + d.strCodUsuario + "','" + d.fecCrea + "','" + d.horaCrea + "','" + d.codUsuarioModi + "','" +
                               d.fecModi + "','" + d.horModi + "','" + usuario + "','" + fecha.ToString("yyyyMMdd") + "','" + fecha.ToString("hhmmss") + "'," + d.mtoRmgTosepRV + "," + d.mtoAjusteIPC + ",'" + d.indCalSobDif + "'," + d.codTipreajuste + "," +
                               d.mtoValReajusteTri + "," + d.mtoValReajusteMen + "," + d.numMesesC + "," + d.douPrcRtaEsc + ")";

                SRVDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                }).FirstOrDefault();

                XmlConfigurator.Configure();
                _log.Info("EliminarDetCot");
                _log.Info("Elimina(Parametro) NumCot" + numCot);
                _log.Info("Elimina(Parametro) NumCorr" + d.strNumCor);
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ELIMINARDETCOT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, numCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCor", SqlDbType.Int, d.strNumCor, ParameterDirection.Input));
                VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                }).FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 23-10-2018
        /// Se realiza dos updates a la misma tabla, pero la segunda actualizacion depende de que ocurra un error en la primera
        /// </summary>
        /// <param name="strNumCor">Numero correlativo</param>
        /// <param name="strNumCot">Numero de cotizacion</param>
        /// <param name="cod_Etapa">Codigo de la nueva etapa</param>
        /// <param name="fec_ini">fecha de inicio</param>
        /// <param name="hora_ini">Hora de inicio</param>
        /// <param name="cod_EtapaAnt">Codigo de la etapa anterior</param>
        /// <param name="usuario">Usuario que esta ejecutando el sistema</param>
        /// <returns>  </returns>
        public void actualizarEtapa(string strNumCot, int strNumCor, string cod_Etapa, string fec_ini, string hora_ini, string cod_EtapaAnt, string usuario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_ACTUALIZARETAPA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@FecCrea", SqlDbType.VarChar, fec_ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, hora_ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCor", SqlDbType.Int, strNumCor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEtapa", SqlDbType.VarChar, cod_Etapa, ParameterDirection.Input));
                VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                }).FirstOrDefault();

                parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_ACTUALIZARETAPA2", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@FecCrea", SqlDbType.VarChar, fec_ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, hora_ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCor", SqlDbType.Int, strNumCor, ParameterDirection.Input));
                VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 23-10-2018
        /// Se realiza un insert y un update a la misma tabla, pero el update se realiza si no surge  un error en el insert
        /// </summary>
        /// <param name="strNumCor">Numero correlativo</param>
        /// <param name="strNumCot">Numero de cotizacion</param>
        /// <param name="cod_Etapa">Codigo de la nueva etapa</param>
        /// <param name="fec_ini">fecha de inicio</param>
        /// <param name="hora_ini">Hora de inicio</param>
        /// <param name="cod_EtapaAnt">Codigo de la etapa anterior</param>
        /// <param name="usuario">Usuario que esta ejecutando el sistema</param>
        /// <returns>  </returns>
        public void insert_Ingreso_Envio(string strNumCot, int strNumCor, string cod_Etapa, string fec_ini, string hora_ini, string cod_EtapaAnt, string usuario)
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

                string usc = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();
                string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();

                connectionString = "Data Source=" + ip + "; Initial Catalog="+BD+";uid=" + usc + ";pwd=" + pass + "";

                string query = "INSERT INTO PT_THIS_ETACOT (NUM_COT,NUM_CORRELATIVO,";
                //if (num_Endoso != 0){ query = query + "NUM_ENDOSO," ;} 
                query = query + "COD_ETAPA,FEC_INI,FEC_TER,HOR_INI,HOR_TER,COD_USUARIO)VALUES('" + strNumCot + "'," + strNumCor + ",";
                //if (num_Endoso != 0){ query = query + num_Endoso+",";} 
                query = query + "'" + cod_Etapa + "','" + fec_ini + "',Null,'" + hora_ini + "',Null,'" + usuario + "')";
                SRVDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                }).FirstOrDefault();

                query = "";
                query = "UPDATE PT_THIS_ETACOT SET FEC_TER = '" + fec_ini + "', HOR_TER='" + hora_ini + "'" +
                        " WHERE NUM_COT='" + strNumCot + "' AND NUM_CORRELATIVO =" + strNumCor;
                //if (num_Endoso != 0){ query = query + "AND NUM_ENDOSO="+ num_Endoso;} 
                query = query + " AND COD_ETAPA='" + cod_EtapaAnt + "'";
                SRVDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, query, x => new ProCarArchivo
                {
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 23-10-2018
        /// Valida si existe el codigo de la etapa
        /// </summary>
        /// <param name="strNumCor">Numero correlativo</param>
        /// <param name="strNumCot">Numero de cotizacion</param>
        /// <param name="cod_Etapa">Codigo de la nueva etapa</param>
        /// <param name="fec_ini">fecha de inicio</param>
        /// <param name="hora_ini">Hora de inicio</param>
        /// <returns> retorna un true si existe o un false si no existe </returns>
        public bool validar(string numCot, int numCor, string cod_Etapa, string fec_ini, string hora_ini)
        {
            //validar si se ha registrado anteriormente la misma etapa
            bool valor = false;
            try
            {
                ProCarArchivo _ProCarArchivo = new ProCarArchivo();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_VALIDAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEtapa", SqlDbType.VarChar, cod_Etapa, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, numCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCor", SqlDbType.Int, numCor, ParameterDirection.Input));
                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    etapaAnterior = x.GetString(0)
                }).FirstOrDefault();
                if (_ProCarArchivo != null)
                {
                    valor = true;
                }
                return valor;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 23-10-2018
        /// Realiza un update a la tabla PT_TMAE_COTBEN
        /// </summary>
        /// <param name="douMtoPenFinal">Monto de la pension final</param>
        /// <param name="strNumCot">Numero de cotizacion</param>
        /// <param name="intNumOpe">Numero de operacion</param>
        /// <param name="intNroOrden">Numro de orden</param>
        /// <returns>  </returns>
        public void updateCotBen(double douMtoPenFinal, string strNumCot, int intNumOpe, int intNroOrden)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_COTBEN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoPension", SqlDbType.Decimal, Convert.ToDecimal(douMtoPenFinal), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(intNumOpe), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOrden", SqlDbType.Int, intNroOrden, ParameterDirection.Input));
                VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                }).FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 23-10-2018
        /// Realiza un update a la tabla PT_TMAE_DETCOTIZACION
        /// </summary>
        /// <param name="vgEstAce">Codigo del estado nuevo de la cotizacion</param>
        /// <param name="fecCierre">Fecha de cierre</param>
        /// <param name="intNumOpe">Numero de operacion</param>
        /// <param name="strNumCor">Numro correlativo</param>
        /// <param name="vgEstEnv">Codigo del estado de la cotizacion</param>
        /// <returns>  </returns>
        public void updateDetoCotizacion(string vgEstAce, string fecCierre, string strNumCot, int strNumCor, int intNumOpe, string vgEstEnv)
        {
            try
            {
                XmlConfigurator.Configure();
                _log.Info("UPDATE DETCOTIZACION");
                _log.Info("CodEstCot" + vgEstAce);
                _log.Info("FecCierre" + fecCierre);
                _log.Info("NumCot" + strNumCot);
                _log.Info("NumCorr" + strNumCor);
                _log.Info("NumOpera" + intNumOpe);
                _log.Info("CodEstCot2" + vgEstEnv);
                string query = "UPDATE PT_TMAE_DETCOTIZACION SET COD_ESTCOT = '" + vgEstAce + "', FEC_ACEPTA = '" + fecCierre + "' WHERE" +
                               " NUM_COT = '" + strNumCot + "' AND NUM_CORRELATIVO =" + strNumCor + " AND NUM_OPERACION =" + intNumOpe + " AND COD_ESTCOT = '" + vgEstEnv + "'";
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_DETODET", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstcot", SqlDbType.VarChar, vgEstAce, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecCierre, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCor", SqlDbType.Int, strNumCor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(intNumOpe), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstcot2", SqlDbType.VarChar, vgEstEnv, ParameterDirection.Input));
                VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                }).FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 23-10-2018
        /// Obtiene la ultima etapa calculada
        /// </summary>
        /// <param name="numCot">Numero de cotizacion</param>
        /// <param name="numCor">Numro correlativo</param>
        /// <returns> retorna la etapa anterior calculada </returns>
        public string etapaAnterior(string numCot, int numCor)
        {
            try
            {
                string val = "";
                ProCarArchivo _ProCarArchivo = new ProCarArchivo();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ETAPAANTERIOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, numCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCor", SqlDbType.Int, numCor, ParameterDirection.Input));
                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    etapaAnterior = x.GetString(0)
                }).FirstOrDefault();
                if (_ProCarArchivo != null)
                {
                    val = _ProCarArchivo.etapaAnterior;
                }
                return val;

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 23-10-2018
        /// Obtiene la fecha actual del servidor
        /// </summary>
        /// <returns> retorna la fecha actual del servidor</returns>
        public string busca_FechaServidor()
        {
            try
            {
                ProCarArchivo _ProCarArchivo = new ProCarArchivo();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "FECHASERVIDOR", ParameterDirection.Input));
                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    fecCierre = x.GetString(0)
                }).FirstOrDefault();

                return _ProCarArchivo.fecCierre;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 23-10-2018
        /// Obtiene la hora actual del servidor
        /// </summary>
        /// <returns> retorna la hora actual del servidor</returns>
        public string busca_HoraServidor()
        {
            try
            {
                ProCarArchivo _ProCarArchivo = new ProCarArchivo();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "HORASERVIDOR", ParameterDirection.Input));
                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    fecCierre = x.GetString(0)
                }).FirstOrDefault();

                return _ProCarArchivo.fecCierre;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Response CargarArchivoB(string numArch)
        {
            Response res = new Response();
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

                string usc = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();
                string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();
                connectionString = "Data Source=" + ip + "; Initial Catalog="+BD+";uid=" + usc + ";pwd=" + pass + "";

                ProCarArchivo _ProCarArchivo = new ProCarArchivo();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETARCHIVOB", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, Convert.ToInt32(numArch), ParameterDirection.Input));
                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    intOKcierresol = x.GetInt32(0),
                    intERRcierresol = x.GetInt32(1),
                    intOKcierremod = x.GetInt32(2),
                    intERRcierremod = x.GetInt32(3),
                    intOKcierrecia = x.GetInt32(4),
                    intERRcierrecia = x.GetInt32(5),
                    intOKcierreafp = x.GetInt32(6),
                    intERRcierreafp = x.GetInt32(7),
                    intGanadas = x.GetInt32(8),
                    intPerdidas = x.GetInt32(9),
                    intAFP = x.GetInt32(10),
                    intRecotizadas = x.GetInt32(11),
                    intDesistidas = x.GetInt32(12),
                    intCaducadas = x.GetInt32(13),
                    Nom_Archivo = x.GetString(14),
                    strCodUsuario = x.GetString(15),
                    fecCrea = x.GetString(16).Substring(6, 2) + "/" + x.GetString(16).Substring(4, 2) + "/" + x.GetString(16).Substring(0, 4),
                    horaCrea = x.GetString(17).Substring(0, 2) + ":" + x.GetString(17).Substring(2, 2) + ":" + x.GetString(17).Substring(4, 2)
                }).FirstOrDefault();
                string[] infoCarga = new string[11];
                if (_ProCarArchivo != null)
                {
                    infoCarga[0] = (_ProCarArchivo.intGanadas + _ProCarArchivo.intPerdidas + _ProCarArchivo.intAFP + _ProCarArchivo.intRecotizadas + _ProCarArchivo.intDesistidas + _ProCarArchivo.intCaducadas).ToString();
                    infoCarga[1] = (_ProCarArchivo.intPerdidas).ToString();
                    infoCarga[2] = (_ProCarArchivo.intGanadas).ToString();
                    infoCarga[3] = (_ProCarArchivo.intAFP).ToString();
                    infoCarga[4] = (_ProCarArchivo.intRecotizadas).ToString();
                    infoCarga[5] = (_ProCarArchivo.intDesistidas).ToString();
                    infoCarga[6] = (_ProCarArchivo.intCaducadas).ToString();
                    infoCarga[7] = _ProCarArchivo.Nom_Archivo;
                    infoCarga[8] = _ProCarArchivo.strCodUsuario;
                    infoCarga[9] = _ProCarArchivo.fecCrea;
                    infoCarga[10] = _ProCarArchivo.horaCrea;
                }
                res.Object = infoCarga;
                res.IsOk = true;
                res.Message = "¡La busqueda se realizo con exito!";
                return res;
            }
            catch (Exception ex)
            {
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }

        public string[] getNumArchivosB(string Fec)
        {

            try
            {
                List<ProCarArchivo> _ProCarArchivo = new List<ProCarArchivo>();
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETNUMARCHS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCarArch", SqlDbType.VarChar, Fec, ParameterDirection.Input));
                _ProCarArchivo = VCEDBContext<ProCarArchivo>.CallStoreProcedure(StoredProcedures.CO_CatalogosProCarArchivo, parameters, x => new ProCarArchivo
                {
                    Num_Archivo = Convert.ToString(x.GetInt32(0)),
                    Nom_Archivo = x.GetString(1)
                }).ToList();
                if (_ProCarArchivo.Count != 0)
                {
                    string[] res = new string[_ProCarArchivo.Count];
                    for (int i = 0; i < _ProCarArchivo.Count; i++)
                    {
                        res[i] = _ProCarArchivo[i].Num_Archivo + " - " + _ProCarArchivo[i].Nom_Archivo;
                    }
                    return res;
                }
                else
                {
                    string[] res = new string[1];
                    res[0] = "-1";
                    return res;
                }
            }
            catch (Exception)
            {
                string[] res = new string[1];
                res[0] = "-1"; ;
                return res;
            }
        }
        /// <summary>
        /// Antonio Quezada
        /// 2019-01-03
        /// Ejecuta el script generado
        /// </summary>
        /// <param name="script"> Query a ejecutar en Base de Datos </param>

        public void EjecutarScript(string script)
        {
            try
            {
                SRVDBContext<ProCarArchivo>.CallSelectStatementConection(connectionString, script, x => new ProCarArchivo
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #endregion
    }
}
