using Estudio.Repository.Core.Domain;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class RutinaReservasRepository
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<beMortalidadDin> ConsultaTablaMortalidadDinamicas()
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_TBLMORDIN", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, "", ParameterDirection.Input));

                return SRVDBContext<beMortalidadDin>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beMortalidadDin
                {
                    GLS_DESCRIPCION = x.GetString(0),
                    NUM_CORRELATIVO = x.GetInt32(1),
                    COD_SEXO = x.GetString(2),
                    COD_INVALIDEZ = x.GetString(3),
                    NUM_ANNO = x.GetInt32(4),
                    FEC_INIVIG = x.GetString(5),
                    FEC_FINVIG = x.GetString(6)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en tablas de mortalidad: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beMortalidadDinDet> ConsultaDetTablaMortalidadDin()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_TBLMORDINDET", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, "", ParameterDirection.Input));

                return SRVDBContext<beMortalidadDinDet>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beMortalidadDinDet
                {
                    numCor = x.GetInt32(0),
                    edad = x.GetInt32(1),
                    mto_lx = x.GetDecimal(2),
                    mto_ax = x.GetDecimal(3)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en tablas de mortalidad DIN: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beMortalVar> ConsultaTablaMortalidad()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_TBLMOR", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, "", ParameterDirection.Input));

                return SRVDBContext<beMortalVar>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beMortalVar
                {
                    GLS_NOMBRE = x.GetString(0),
                    NUM_CORRELATIVO = x.GetInt32(1),
                    COD_TIPTABMOR = x.GetString(2),
                    COD_SEXO = x.GetString(3),
                    COD_TIPOPER = x.GetString(4),
                    FEC_INI = x.GetString(5),
                    FEC_FIN = x.GetString(6)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en tablas de mortalidad: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beMortalidadDet> ConsultaDetTablaMortalidad()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_TBLMORDET", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, "", ParameterDirection.Input));

                return SRVDBContext<beMortalidadDet>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beMortalidadDet
                {
                    numCor = x.GetInt32(0),
                    edad = x.GetInt32(1),
                    mto_lx = x.GetDecimal(2)
            }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en tablas de mortalidad: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beTasaFacVac> ConsultaFactorVac()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_FACTORVAC", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, "", ParameterDirection.Input));

                return SRVDBContext<beTasaFacVac>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beTasaFacVac
                {
                    FEC_IPC = x.GetDateTime(0),
                    MTO_IPC = (double)x.GetDecimal(1),
                    PRC_IPC = (double)x.GetDecimal(2)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en consulta factor vac: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beTasasPromedio> ConsultaTasasPromedio(string pFechaPeriodo)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_TASASPROM", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecMesAct", SqlDbType.VarChar, pFechaPeriodo, ParameterDirection.Input));

                return SRVDBContext<beTasasPromedio>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beTasasPromedio
                {
                    COD_MONEDA = x.GetString(0),
                    COD_TIPPREAJUSTE = x.GetInt32(1),
                    MTO_VTAPROM = (double)x.GetDecimal(2),
                    COD_TIPPENSION = x.GetString(3)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en tasas promedio: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beCurvaTasas> ConsultaCurvaTasas()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_CURVATASAS", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, "", ParameterDirection.Input));

                return SRVDBContext<beCurvaTasas>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beCurvaTasas
                {
                    COD_MONEDA = x.GetString(0),
                    COD_TIPPREAJUSTE = x.GetInt32(1),
                    NUM_MES = x.GetInt32(2),
                    MTO_VALOR = (double)x.GetDecimal(3),
                    FEC_INIVIG = x.GetString(4),
                    FEC_TERVIG = x.GetString(5)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en consulta tasas: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beDatosPol> ConsultaPolizas(string numTbl)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_CARGAPOL", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, numTbl, ParameterDirection.Input));

                return SRVDBContext<beDatosPol>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beDatosPol
                {
                    NumPol = x.GetString(0),
                    TipPen = x.GetString(1),
                    NumBen = x.GetInt32(2),
                    EstPol = x.GetString(3),
                    FecVig = x.GetString(4),
                    MtoPri = (double)x.GetDecimal(5),
                    MtoPen = (double)x.GetDecimal(6),
                    TipRen = x.GetString(7),
                    TipMod = x.GetString(8),
                    NumDif = x.GetInt32(9),
                    NumGar = x.GetInt32(10),
                    PrcRet = (double)x.GetDecimal(11),
                    PrcTce = (double)x.GetDecimal(12),
                    PrcTas = (double)x.GetDecimal(13),
                    PrcTceDef = (double)x.GetDecimal(14),
                    PrcTasDef = (double)x.GetDecimal(15),
                    FecPag = x.GetString(16),
                    MtoGS = (double)x.GetDecimal(17),
                    prcFac = (double)x.GetDecimal(18),
                    FecCot = x.GetString(19),
                    DerCre = x.GetString(20),
                    DerGra = x.GetString(21),
                    IndCob = x.GetString(22),
                    MtoEll = (double)x.GetDecimal(23),
                    prcEll = (double)x.GetDecimal(24),
                    FecDev = x.GetString(25),
                    TipRea = x.GetInt32(26),
                    PrcTri = (double)x.GetDecimal(27),
                    PrcMen = (double)x.GetDecimal(28),
                    TipMon = x.GetString(29),
                    EdaLim = x.GetInt32(30),
                    PrcTaf = x.GetInt32(31),
                    PrcTasRes = (double)x.GetDecimal(32),
                    Tip = x.GetString(33)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en consultar pólizas: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beCurvaTasas> ConsultaCurvas(string FecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTA_CURVATASAS", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, FecCal, ParameterDirection.Input));

                return SRVDBContext<beCurvaTasas>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beCurvaTasas
                {
                    NUM_MES = x.GetInt32(0)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en consulta curvas: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 06-05-2019
        /// Método para poder obtener el Stock desde BD y así poder manipularlo en la rutina.
        /// </summary>
        /// <param name="anno">Año del periodo actual.</param>
        /// <param name="mes">Mes del periodo actual.</param>
        /// <returns></returns>
        public List<beDatosPol> ConsultaPolFlujos(string anno, int mes, string FecPer)
        {
            try
            {
                string FecAnt = mes < 11 ? anno + "0" + (mes - 1).ToString() : anno + (mes - 1).ToString();

                if (mes == 1) { FecAnt = (Convert.ToInt32(anno) - 1).ToString() + "12";  };

                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_CARGAPOL_FLU", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecMesAct", SqlDbType.VarChar, FecPer, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecMesAnt", SqlDbType.VarChar, FecAnt + "01", ParameterDirection.Input));

                return SRVDBContext<beDatosPol>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beDatosPol
                {
                    NumPol = x.GetString(0),
                    TipPen = x.GetString(1),
                    NumBen = x.GetInt32(2),
                    EstPol = x.GetString(3),
                    FecVig = x.GetString(4),
                    MtoPri = (double)x.GetDecimal(5),
                    MtoPen = (double)x.GetDecimal(6),
                    TipRen = x.GetString(7),
                    TipMod = x.GetString(8),
                    NumDif = x.GetInt32(9),
                    NumGar = x.GetInt32(10),
                    PrcRet = (double)x.GetDecimal(11),
                    PrcTce = (double)x.GetDecimal(12),
                    PrcTas = (double)x.GetDecimal(13),
                    PrcTceDef = (double)x.GetDecimal(14),
                    PrcTasDef = (double)x.GetDecimal(15),
                    FecPag = x.GetString(16),
                    MtoGS = (double)x.GetDecimal(17),
                    prcFac = (double)x.GetDecimal(18),
                    FecCot = x.GetString(19),
                    DerCre = x.GetString(20),
                    DerGra = x.GetString(21),
                    IndCob = x.GetString(22),
                    MtoEll = (double)x.GetDecimal(23),
                    prcEll = (double)x.GetDecimal(24),
                    FecDev = x.GetString(25),
                    TipRea = x.GetInt32(26),
                    PrcTri = (double)x.GetDecimal(27),
                    PrcMen = (double)x.GetDecimal(28),
                    TipMon = x.GetString(29),
                    EdaLim = x.GetInt32(30),
                    PrcTaf = (int)x.GetDecimal(31),
                    PrcTasRes = (double)x.GetDecimal(32),
                    Tip = x.GetString(33),
                    IndStock = x.GetString(34),
                    NumeroCasoEspecial = x.GetInt32(35)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en consulta pol flujos: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beDatosBen> ConsultaBen(string numTbl)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_BEN1", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, numTbl, ParameterDirection.Input));

                return SRVDBContext<beDatosBen>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beDatosBen
                {
                    NumOrd = x.GetInt32(0),
                    CodPar = x.GetString(1),
                    FecNac = x.GetString(2),
                    GruFam = x.GetString(3),
                    TipSex = x.GetString(4),
                    TipInv = x.GetString(5),
                    FecInv = x.IsDBNull(6) ? "" : x.GetString(6),
                    DerPen = x.GetString(7),
                    PrcPen = (double)x.GetDecimal(8),
                    PrcLeg = (double)x.GetDecimal(9),
                    PrcGar = (double)x.GetDecimal(10),
                    NacHM = x.IsDBNull(11) ? "" : x.GetString(11),
                    FacFal = x.IsDBNull(12) ? "" : x.GetString(12),
                    DerCre = x.GetString(13),
                    PenLeg = (double)x.GetDecimal(14),
                    PenGar = (double)x.GetDecimal(15),
                    NumPol = x.GetString(16),
                    Tope18 = x.GetString(17),
                    Estudi = x.GetString(18)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en consultas beneficiarios: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beResultadosFlujos> ConsultaCargaFlujosPensiones(string numTbl, string anno, int mes, string FecPer)
        {
            string FecAnt = mes < 11 ? anno + "0" + (mes - 1).ToString() : anno + (mes - 1).ToString();

            if (mes == 1) { FecAnt = (Convert.ToInt32(anno) - 1).ToString() + "12"; };
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_CFLUJOSPEN", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, numTbl, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecMesAct", SqlDbType.VarChar, FecPer, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecMesAnt", SqlDbType.VarChar, FecAnt + "01", ParameterDirection.Input));

                return SRVDBContext<beResultadosFlujos>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beResultadosFlujos
                {
                    numPol = x.GetString(0),
                    numOrd = x.GetInt32(1),
                    //TablaFlu.numEdad = x.GetInt32(1),
                    numMes = x.GetInt32(2),
                    mtoPen = (double)x.GetDecimal(3),
                    prcFac = (double)x.GetDecimal(4),
                    GtoSep = (double)x.GetDecimal(5),
                    fluPen = (double)x.GetDecimal(6),
                    tasTce = (double)x.GetDecimal(7),
                    Tip = x.GetString(8)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en consultar flujos pensiones: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beResultadosFlujos> ConsultaCargaFlujosPenAnt(string numTbl)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_CFLUJOSPEN_ANT", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, numTbl, ParameterDirection.Input));

                return SRVDBContext<beResultadosFlujos>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beResultadosFlujos
                {
                    numPol = x.GetString(0),
                    numOrd = x.GetInt32(1),
                    //TablaFlu.numEdad = x.GetInt32(1),
                    numMes = x.GetInt32(2),
                    mtoPen = (double)x.GetDecimal(3),
                    prcFac = (double)x.GetDecimal(4),
                    GtoSep = (double)x.GetDecimal(5),
                    fluPen = (double)x.GetDecimal(6),
                    tasTce = (double)x.GetDecimal(7),
                    Tip = x.GetString(8)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en consulta de carga flujos pen ant: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<beCurvaTasas> ConsultaGtoSepelio()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_GTOSEPELIO", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTipTbl", SqlDbType.VarChar, "", ParameterDirection.Input));

                return SRVDBContext<beCurvaTasas>.CallStoreProcedure(StoredProcedures.CR_ConsultasRutinaReservas, parameters, x => new beCurvaTasas
                {
                    MTO_CUOMOR = (double)x.GetDecimal(0),
                    FEC_INICUOMOR = x.GetString(1),
                    FEC_TERCUOMOR = x.GetString(2)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error en consulta de gasto de sepelio: " + ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }


    }
}
