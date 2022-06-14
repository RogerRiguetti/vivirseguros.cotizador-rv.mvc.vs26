using System;
using System.Collections.Generic;
using System.Linq;
using Estudio.Repository.Core.Domain;
using System.Data.SqlClient;
using System.Data;

namespace Estudio.Repository.Persistence.Repositories
{
    public class RutinaRepository
    {
        /// <summary>
        /// Antonio Quezada
        /// 2018-04-13
        /// Carga matriz de tablas de mortalidad
        /// </summary>
        /// <param name="fecCal"></param>
        /// <returns></returns>

        public List<beMortalVar> ConsultaDetalleMatriz(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONDETMAT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beMortalVar>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beMortalVar
                {
                    GLS_NOMBRE = x.GetString(0),
                    NUM_CORRELATIVO = x.GetInt32(1),
                    COD_TIPTABMOR = x.GetString(2),
                    COD_SEXO = x.GetString(3),
                    COD_TIPOPER = x.GetString(4),
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-04-13
        /// Obtiene el detalle de las tablas de mortalidad
        /// </summary>
        /// <returns></returns>

        public List<beMortalidadDet> ConsultaCargaMatriz()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONCARMAT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beMortalidadDet>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beMortalidadDet
                {
                    numCor = x.GetInt32(0),
                    edad = x.GetInt32(1),
                    mto_lx = x.GetDecimal(2)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-04-13
        /// Obtiene el detalle de las tablas de mortalidad
        /// </summary>
        /// <param name="fecCal"></param>
        /// <returns></returns>

        public List<bePorcenLegales> ConsultaDetalleMortalidad(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONBENDETMOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<bePorcenLegales>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new bePorcenLegales
                {
                    COD_PAR = int.Parse(x.GetString(0)),
                    COD_SITINV = x.GetString(1),
                    COD_SEXO = x.GetString(2),
                    PRC_PENSION = (double)x.GetDecimal(3)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<beMortalidadDin> ConsultaTablaMortalidadDinamicas(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONTABMORDIN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beMortalidadDin>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beMortalidadDin
                {
                    GLS_DESCRIPCION = x.GetString(0),
                    NUM_CORRELATIVO = x.GetInt32(1),
                    COD_SEXO = x.GetString(2),
                    COD_INVALIDEZ = x.GetString(3),
                    NUM_ANNO = x.GetInt32(4)
                    //,
                    //FEC_INIVIG = x.GetString(5),
                    //FEC_FINVIG = x.GetString(6)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<beMortalidadDinDet> ConsultaDetTablaMortalidadDin()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONDETTABMOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beMortalidadDinDet>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beMortalidadDinDet
                {
                    numCor = x.GetInt32(0),
                    edad = x.GetInt32(1),
                    mto_lx = x.GetDecimal(2),
                    mto_ax = x.GetDecimal(3)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Antonio Quezada
        /// 2018-04-13
        /// Carga gastos tasas indicadores
        /// </summary>
        /// <param name="fecCal"></param>
        /// <param name="tipoPen"> Codigo del tipo de pensión </param>
        /// <returns></returns>

        public List<beDatosTasasPar> ConsultaTasasIndicadores(string fecCal, string tipoPen)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONGASTAIN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, tipoPen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beDatosTasasPar>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beDatosTasasPar
                {
                    CodMon = x.GetString(0),
                    TipPen = x.GetString(1),
                    CodReg = x.GetString(2),
                    PriMin = 0,
                    PriMax = 99999999.99,
                    MtoGad = (double)x.GetDecimal(3),
                    MtoGem = (double)x.GetDecimal(4),
                    PrcDeu = (double)x.GetDecimal(5),
                    MtoImp = (double)x.GetDecimal(6),
                    PrcPer = (double)x.GetDecimal(8),
                    PrcTas = (double)x.GetDecimal(9),
                    PrcTir = (double)x.GetDecimal(11),
                    TipRea = x.GetInt32(12),
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-04-13
        /// Carga tasa mercado
        /// </summary>
        /// <param name="query"> Consulta que se ejecutará en la base de datos </param>
        /// <returns></returns>

        public List<beTasaMercado> ConsultaTasaMercado(string query)
        {
            try
            {
                return SRVDBContext<beTasaMercado>.CallSelectStatement(query, x => new beTasaMercado
                {
                    CodMon = x.GetString(0),
                    TipRea = (Int32)x.GetDecimal(1),
                    PrcVal = (double)x.GetDecimal(2)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-04-13
        /// Carga tasa anclaje
        /// </summary>
        /// <param name="fecCal"></param>
        /// <returns></returns>

        public List<beTasaAnclaje> ConsultaTasaAnclaje(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONTAAN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beTasaAnclaje>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beTasaAnclaje
                {
                    CodMon = x.GetString(0),
                    TipRea = (Int32)x.GetDecimal(1),
                    PrcVal = (double)x.GetDecimal(2)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<beTasaFacVac> ConsultaFactorVac()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONFACVAC", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beTasaFacVac>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beTasaFacVac
                {
                    FEC_IPC = x.GetDateTime(0),
                    MTO_IPC = (double)x.GetDecimal(1),
                    PRC_IPC = (double)x.GetDecimal(2)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-04-13
        /// Carga CPK's
        /// </summary>
        /// <param name="fecCal"></param>
        /// <returns></returns>

        public List<beCPK> ConsultaCPK(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONCPKS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beCPK>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beCPK
                {
                    COD_MONEDA = x.GetString(0),
                    COD_TIPREAJUSTE = (Int32)x.GetDecimal(1),
                    PRC_CPK = (double)x.GetDecimal(2),
                    NUM_ANNO = x.GetInt32(3),
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-04-13
        /// Carga rentabilidad
        /// </summary>
        /// <param name="fecCal"></param>
        /// <returns></returns>

        public List<beRentabilidad> ConsultaRentabilidad(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONRENT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beRentabilidad>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beRentabilidad
                {
                    COD_MONEDA = x.GetString(0),
                    COD_TIPREAJUSTE = x.GetInt32(1),
                    PRC_TASAREN = (double)x.GetDecimal(2),
                    NUM_ANNO = x.GetInt32(3),
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<beTasasPromedio> ConsultaTasasPromedio(string tipoPension)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONTASAPROM", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, tipoPension, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beTasasPromedio>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beTasasPromedio
                {
                    COD_MONEDA = x.GetString(0),
                    COD_TIPPREAJUSTE = x.GetInt32(1),
                    MTO_VTAPROM = (double)x.GetDecimal(2),
                    COD_TIPPENSION = x.GetString(3)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<beCurvaTasas> ConsultaCurvaTasas(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONCURVATAS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));


                return VCEDBContext<beCurvaTasas>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beCurvaTasas
                {
                    COD_MONEDA = x.GetString(0),
                    COD_TIPPREAJUSTE = x.GetInt32(1),
                    NUM_MES = x.GetInt32(2),
                    MTO_VALOR = (double)x.GetDecimal(3),
                    FEC_INIVIG = x.GetString(4),
                    FEC_TERVIG = x.GetString(5)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Antonio Quezada
        /// 2018-04-17
        /// Obtiene los valores de Factores
        /// </summary>
        /// <param name="fechaCotizacion"> Fecha en la que se realizó la cotización </param>
        /// <param name="codigoMoneda"> Código de la moneda </param>
        /// <returns> Regresa la información de los factores en un elemento de tipo beDatosModalidad </returns>

        public beDatosModalidad ConsultaValoresFactores(string fechaCotizacion, string codigoMoneda)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONFACTORES", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, fechaCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, codigoMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beDatosModalidad>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beDatosModalidad
                {
                    PrcAnu = (double)x.GetDecimal(0),
                    PrcMen = (double)x.GetDecimal(1),
                    PrcTri = (double)x.GetDecimal(2)
                }).FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-05-16
        /// Obtiene el tipo de cambio
        /// </summary>
        /// <returns> Regresa un parámetro que contiene el tipo de cambio </returns>

        public Parametro ConsultaTipoCambio()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONTIPOCAMBIO", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, DateTime.Now, ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

            return VCEDBContext<Parametro>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new Parametro
            {
                Elemento = x.GetDecimal(0).ToString()
            }).FirstOrDefault();
        }

        /// <summary>
        /// Lizbeth Morales
        /// 10/07/2018
        /// Metodo que realiza la conexion de base de datos del gasto sepelio del asegurado 
        /// </summary>
        /// <returns>gasto sepelio de asegurado</returns>
        public Parametro GastoSepelio()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONGASTOSEPELIO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<Parametro>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new Parametro
                {
                    Elemento = x.GetDecimal(0).ToString()
                }).FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-05-23
        /// Obtiene el c+odigo de la región
        /// </summary>
        /// <param name="valorComision"> Valor de la comisión </param>
        /// <param name="cic"> Valor de Cic </param>
        /// <returns> Regresa un objeto de tipo beDatosModalidad que contiene el código de la región </returns>

        public beDatosModalidad ConsultaCodigoRegion(decimal valorComision, decimal cic, int idDepartamento)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONCODREG", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, DateTime.Now, ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, valorComision, ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, cic, ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, "", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDepartamento", SqlDbType.Int, idDepartamento, ParameterDirection.Input));

            return VCEDBContext<beDatosModalidad>.CallStoreProcedure(StoredProcedures.VCE_ConsultasRutinas, parameters, x => new beDatosModalidad
            {
                CodReg = x.GetString(0),
            }).FirstOrDefault();
        }
    }
}
