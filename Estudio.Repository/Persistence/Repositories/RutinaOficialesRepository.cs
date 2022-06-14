using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
   public class RutinaOficialesRepository
    {
        public List<beMortalVar> ConsultaTablaMortalidad(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONTABMOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beMortalVar>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beMortalVar
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

        public List<beMortalidadDet> ConsultaDetalleMortalidad()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONDETMOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beMortalidadDet>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beMortalidadDet
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

        public List<beMortalidadDin> ConsultaTablaMortalidadDinamicas(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONTABMORDIN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beMortalidadDin>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beMortalidadDin
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
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONDETTABMOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beMortalidadDinDet>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beMortalidadDinDet
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

        public List<beDatosModalidad> ConsultaGastoSepelioMes(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONGASSEPMES", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beDatosModalidad>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beDatosModalidad
                {
                    MtoGS = (double)x.GetDecimal(0),
                    FEC_INICUOMOR = x.GetString(1),
                    FEC_TERCUOMOR = x.GetString(2)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<beDatosModalidad> ConsultaRentabilidadAfp(string codAfp)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONRENAFP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, codAfp, ParameterDirection.Input));

                return VCEDBContext<beDatosModalidad>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beDatosModalidad
                {
                    RenAfp = x.IsDBNull(0) ? 0 : (double)x.GetDecimal(0),
                    COD_ELEMENTO = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void actualiza_Rechazo_Gral(string Cod_Rechazo, string Num_Cot, string vgUsuario, int vgCero)
        {
            try
            {
                AsignacionIntermediario dr = new AsignacionIntermediario();
                DateTime fecha = DateTime.Now;
                string query = "UPDATE PT_TMAE_DETCOTIZACION SET COD_RECHAZO='" + Cod_Rechazo + "', " +
                               " COD_USUARIOMODI='" + vgUsuario + "'," +
                               " FEC_MODI='" + fecha.ToString("yyyyMMdd") + "'," +
                               " HOR_MODI='" + fecha.ToString("hhmmss") + "'" +
                               " WHERE NUM_COT='" + Num_Cot + "' " +
                               " AND COD_RECHAZO='" + vgCero.ToString() + "' ";
                dr = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                { }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public List<beDatosModalidad> ConsultaRegionTasas(double cic)
        {
            try
            {
                List<beDatosModalidad> d = new List<beDatosModalidad>();

                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONREGTAS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, cic, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                d = VCEDBContext<beDatosModalidad>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beDatosModalidad
                {
                    CodReg = x.GetString(0),
                    MTO_MINIMO = x.GetDecimal(1),
                    MTO_MAXIMO = x.GetDecimal(2),
                    CodReg_Asoc = x.GetString(3)

                }).ToList();
                if (d.Count == 0)
                {
                    d = new List<beDatosModalidad>();
                    beDatosModalidad datoModalidad = new beDatosModalidad();

                    datoModalidad.CodReg = "0";
                    d.Add(datoModalidad);
                    return d;
                }else
                {
                    return d;
                }
            }
            catch (Exception ex)
            {
                List<beDatosModalidad> d = new List<beDatosModalidad>();
                beDatosModalidad datoModalidad = new beDatosModalidad();

                datoModalidad.CodReg = "0";
                d.Add(datoModalidad);
                return d;
            }
        }

        public List<bePorcenLegales> ConsultaPorcentaje(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONPOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<bePorcenLegales>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new bePorcenLegales
                {
                    COD_PAR = int.Parse(x.GetString(0)),
                    COD_SITINV = x.GetString(1),
                    COD_SEXO = x.GetString(2),
                    PRC_PENSION = (double)x.GetDecimal(3),
                    fec_inivigpor = x.GetString(4),
                    fec_tervigpor = x.GetString(5)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<beDatosTasasPar> ConsultaGastosTasasInd(string fecCal, string tipoPen)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONGASTASIND", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, tipoPen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beDatosTasasPar>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beDatosTasasPar
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
                    TipRea = x.GetInt32(12)
                    //,
                    //ComMin = (double)x.GetDecimal(13),
                    //ComMax = (double)x.GetDecimal(14)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<beDatosTasasPar> ConsultaGastosTasasIndMejo(string fecCal, string tipoPen)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONGASTASINDMEJOINI", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, tipoPen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beDatosTasasPar>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beDatosTasasPar
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
                    ComMin = (double)x.GetDecimal(13),
                    ComMax = (double)x.GetDecimal(14)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<beDatosTasasPar> ConsultaGastosTasasIndM(string fecCal, string tipoPen, double tasaVenta)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONGASTASIND", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, tipoPen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beDatosTasasPar>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beDatosTasasPar
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
                    PrcTas = tasaVenta == 0 ? (double)x.GetDecimal(9) : tasaVenta,
                    PrcTir = (double)x.GetDecimal(11),
                    TipRea = x.GetInt32(12)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }
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
                throw;
            }
        }

        public List<beTasaAnclaje> ConsultaTasaAnclaje(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONTASAN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beTasaAnclaje>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beTasaAnclaje
                {
                    CodMon = x.GetString(0),
                    TipRea = (Int32)x.GetDecimal(1),
                    PrcVal = (double)x.GetDecimal(2),
                    fec_inivig = x.GetString(3),
                    fec_tervig = x.GetString(4)
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
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONFACVAC", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beTasaFacVac>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beTasaFacVac
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

        public List<beCPK> ConsultaCPKS(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONCPKS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beCPK>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beCPK
                {
                    COD_MONEDA = x.GetString(0),
                    COD_TIPREAJUSTE = (Int32)x.GetDecimal(1),
                    PRC_CPK = (double)x.GetDecimal(2),
                    NUM_ANNO = x.GetInt32(3),
                    FEC_INIVIG = x.GetString(4),
                    FEC_TERVIG = x.GetString(5)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<beRentabilidad> ConsultaRentabilidad(string fecCal)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONRENT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beRentabilidad>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beRentabilidad
                {
                    COD_MONEDA = x.GetString(0),
                    COD_TIPREAJUSTE = x.GetInt32(1),
                    PRC_TASAREN = (double)x.GetDecimal(2),
                    NUM_ANNO = x.GetInt32(3),
                    FEC_INIVIG = x.GetString(4),
                    FEC_TERVIG = x.GetString(5)
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
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONTASAPROM", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, tipoPension, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beTasasPromedio>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beTasasPromedio
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
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONCURVATAS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beCurvaTasas>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beCurvaTasas
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

        public List<beDatosTasasPar> ConsultaGastosTasasIndMej(string fecCal, string tipoPen)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONGASTASINDMEJ", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, tipoPen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beDatosTasasPar>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beDatosTasasPar
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
                    PrcTas =(double)x.GetDecimal(9),
                    PrcTir = (double)x.GetDecimal(11),
                    TipRea = x.GetInt32(12),
                    ComMin = (double)x.GetDecimal(13),
                    ComMax = (double)x.GetDecimal(14)
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<beDatosTasasPar> ConsultaGastosTasasIndMejo(string fecCal, string tipoPen, double tasaVenta)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONGASTASINDMEJ", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, tipoPen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beDatosTasasPar>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beDatosTasasPar
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
                    PrcTas = tasaVenta == 0 ? (double)x.GetDecimal(9) : tasaVenta,
                    PrcTir = (double)x.GetDecimal(11),
                    TipRea = x.GetInt32(12),
                    ComMin = (double)x.GetDecimal(13),
                    ComMax = (double)x.GetDecimal(14)
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<beDatosTasasPar> ConsultaGastosTasasIndEx(string fecCal, string tipoPen, double tasaVenta)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONGASTASINDEX", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, fecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, tipoPen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodAfp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<beDatosTasasPar>.CallStoreProcedure(StoredProcedures.CO_ConsultasRutinaOficiales, parameters, x => new beDatosTasasPar
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
                    PrcTas = tasaVenta == 0 ? (double)x.GetDecimal(9) : tasaVenta,
                    PrcTir = (double)x.GetDecimal(11),
                    TipRea = x.GetInt32(12)
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
