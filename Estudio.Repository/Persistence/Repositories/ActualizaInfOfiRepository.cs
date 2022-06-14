using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
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
using System.Web.Script.Serialization;

namespace Estudio.Repository.Persistence.Repositories
{
   public class ActualizaInfOfiRepository
    {
        public double nuevaVT, nuevaCO;
        public int numArchivo, numCorrelativo;
        public string numCot, caso, parametro = "";
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Lizbeth Morales
        /// 25-04-2019
        /// Metodo que busca los registros que esten guardades con esa operacion o el cusspp
        /// </summary>
        /// <param name="numOperacion">Numero de operación con la que se desea obtener información</param>
        /// <param name="codCUSPP">Codigo CUSPP con el que se desea obtener información</param>
        /// <returns> Retorna los datos obtenidos para mostrarlos en la vista </returns>
        public Response busqueda(string codCUSPP, string numCor, string numOperacion)
        {
            Response res = new Response();
            try
            {
                double douNumOperacion = numOperacion == "" ? 0 : Convert.ToDouble(numOperacion);
                List<ActualizaInfOfi> _ActualizaInfOfiList = new List<ActualizaInfOfi>();
                ActualizaInfOfiRepository _ActualizaInfOfiRepository = new ActualizaInfOfiRepository();
                var parameters = new List<SqlParameter>();
                if (numCor != "N")
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSQUEDATRAMO", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(douNumOperacion), ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCUSPP", SqlDbType.VarChar, codCUSPP, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, Convert.ToInt32(numCor), ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSQUEDA", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(douNumOperacion), ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCUSPP", SqlDbType.VarChar, codCUSPP, ParameterDirection.Input));
                }
                _ActualizaInfOfiList = VCEDBContext<ActualizaInfOfi>.CallStoreProcedure(StoredProcedures.CO_CatalogoActualizaInfo, parameters, x => new ActualizaInfOfi
                {
                    numOperacion = Convert.ToInt32(x.GetDecimal(0)),
                    dni = x.GetString(1),
                    afp = x.GetString(2),
                    cic = Convert.ToDouble(x.GetDecimal(3)),
                    asegurado = x.GetString(4),
                    cuspp = x.GetString(5),
                    sexo = x.GetString(6),
                    fechaNac = (x.GetString(7).Substring(6, 2) + "/" + x.GetString(7).Substring(4, 2) + "/" + x.GetString(7).Substring(0, 4)),
                    moneda = x.GetString(8),
                    modalidad = x.GetString(9),
                    periodoDiferido = Convert.ToString(x.GetInt32(10)),
                    rentaTMP = Convert.ToDouble(x.GetDecimal(11)),
                    perGarantizado = Convert.ToString(x.GetInt32(12)),
                    rentaEsc = Convert.ToDouble(x.GetDecimal(13)),
                    primaUnica = Convert.ToDouble(x.GetDecimal(14)),
                    renTmp1T = Convert.ToDouble(x.GetInt32(15)),
                    mtoPensio = Convert.ToDouble(x.GetDecimal(16)),
                    tasaVenta = Convert.ToDouble(x.GetDecimal(17)),
                    tir = Convert.ToDouble(x.GetDecimal(18)),
                    perdida = Convert.ToString(x.GetDecimal(19)),
                    comision = Convert.ToDouble(x.GetDecimal(20)),
                    codTipRen = x.GetString(21),
                    codTipCambio = Convert.ToDouble(x.GetDecimal(22)),
                    numCot = x.GetString(23),
                    numArchivo = x.GetInt32(24),
                    numCorrelativo = x.GetInt32(25),
                    Ind_Cob = x.GetString(26),
                    Ind_SISCO = x.GetInt32(27),
                    cod_tipreajuste = Convert.ToInt32(x.GetString(28)),
                    mes_esc = x.GetInt32(29)
                }).ToList();
                if (_ActualizaInfOfiList.Count != 0)
                {
                    List<string[]> datos = new List<string[]>();
                    string[] datosPer = { _ActualizaInfOfiList[0].numOperacion.ToString(), _ActualizaInfOfiList[0].dni, _ActualizaInfOfiList[0].afp, _ActualizaInfOfiList[0].cic.ToString(),
                                         _ActualizaInfOfiList[0].asegurado, _ActualizaInfOfiList[0].cuspp, _ActualizaInfOfiList[0].sexo, _ActualizaInfOfiList[0].fechaNac, _ActualizaInfOfiList[0].numCot, _ActualizaInfOfiList[0].numArchivo.ToString()};
                    datos.Add(datosPer);

                    CalculoCotizacionRepository _calculoCotizacionRepository = new CalculoCotizacionRepository();

                    string sumaPension = "0";
                    double mtoSumPen = 0;



                    for (int i = 0; i < _ActualizaInfOfiList.Count; i++)
                    {
                        ActualizaInfOfi NArchivo = _ActualizaInfOfiRepository.obtenerNA(_ActualizaInfOfiList[i].numOperacion.ToString(), _ActualizaInfOfiList[i].numCorrelativo.ToString());

                        decimal suma = 0;
                        double PrimaUnica = 0;
                        double PrimerTramo = 0;
                        double SegundoTramo = 0;
                        int AniosDif = 0;
                        int rentEsc = 0;

                        var listBeneficiarios = _calculoCotizacionRepository.getBeneficiarios(NArchivo.numArchivo);

                        sumaPension = _ActualizaInfOfiList[i].mtoPensio.ToString();
                        switch (NArchivo.codTipRen)
                        {
                            case "1":
                                {

                                    PrimaUnica = _ActualizaInfOfiList[i].cic;

                                    PrimerTramo = 0;
                                    SegundoTramo = Convert.ToDouble(sumaPension);
                                    AniosDif = Convert.ToInt32(_ActualizaInfOfiList[i].periodoDiferido);
                                    rentEsc = Convert.ToInt32(_ActualizaInfOfiList[i].rentaEsc);
                                    break;
                                }
                            case "2":
                                {
                                    if (_ActualizaInfOfiList[i].moneda != "S/.Aj." && _ActualizaInfOfiList[i].moneda != "S/.")
                                    {
                                        PrimaUnica = (_ActualizaInfOfiList[i].primaUnica) * _ActualizaInfOfiList[i].codTipCambio;
                                        PrimerTramo = ((Convert.ToDouble(sumaPension) * 2)) * _ActualizaInfOfiList[i].codTipCambio;
                                    }
                                    else
                                    {
                                        PrimaUnica = _ActualizaInfOfiList[i].primaUnica;
                                        PrimerTramo = (Convert.ToDouble(sumaPension) * 2);
                                    }

                                    SegundoTramo = Convert.ToDouble(sumaPension);
                                    AniosDif = Convert.ToInt32(_ActualizaInfOfiList[i].periodoDiferido);
                                    rentEsc = Convert.ToInt32(_ActualizaInfOfiList[i].rentaEsc);
                                    break;
                                }

                            case "6":
                                {

                                    PrimerTramo = Convert.ToDouble(sumaPension);
                                    SegundoTramo = ((Convert.ToDouble(sumaPension) * Convert.ToInt32(_ActualizaInfOfiList[i].rentaEsc)) / 100);

                                    PrimaUnica = _ActualizaInfOfiList[i].cic;
                                    AniosDif = Convert.ToInt32((_ActualizaInfOfiList[i].mes_esc / 12));
                                    rentEsc = Convert.ToInt32(_ActualizaInfOfiList[i].rentaEsc);
                                    break;
                                }
                        }

                        string[] dTabla = { _ActualizaInfOfiList[i].moneda, _ActualizaInfOfiList[i].modalidad.ToString(), AniosDif.ToString()/*_ExceptionList[i].periodoDiferido*/, _ActualizaInfOfiList[i].rentaTMP.ToString(),
                                    _ActualizaInfOfiList[i].perGarantizado, _ActualizaInfOfiList[i].rentaEsc.ToString(), PrimaUnica.ToString("N2")/*PrimaUnica.ToString()*/,  PrimerTramo.ToString("N2"),
                                   SegundoTramo.ToString("N2"), _ActualizaInfOfiList[i].tasaVenta.ToString(), _ActualizaInfOfiList[i].tir.ToString(), _ActualizaInfOfiList[i].perdida, _ActualizaInfOfiList[i].comision.ToString(), _ActualizaInfOfiList[i].numCorrelativo.ToString(), _ActualizaInfOfiList[i].mtoPensio.ToString("0.00"),
                                _ActualizaInfOfiList[i].Ind_SISCO.ToString(),_ActualizaInfOfiList[i].cod_tipreajuste.ToString()};
                        datos.Add(dTabla);

                       
                    }
                    res.Object = datos;
                    res.IsOk = true;
                }
                else
                {
                    res.IsOk = false;
                    res.Message = "No se encontraron registros con esos datos";
                }
                return res;
            }
            catch (Exception ex)
            {
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// Lizbeth Morales
        /// 25-04-2019
        /// Metodo que realiza update del re-calculo con los datos obtenidos
        /// </summary>
        /// <param name="datos">Todos los datos necesarios para hacer el Update</param>
        /// <returns> Actuliza los datos de la modalidad </returns>
        public Response calculoU(ActualizaInfOfi datos)
        {
            Response res = new Response();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ActualizaInfOfi d = new ActualizaInfOfi();
            XmlConfigurator.Configure();
            _log.Info("SE GUARDARA INFORMACION DE ACTUALIZA INFORMACION " + datos.numOperacion);
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_CALCULO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@TasaVTA", SqlDbType.Decimal, datos.PRC_TASAVTA, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@TasaTIR", SqlDbType.Decimal, datos.PRC_TASATIR, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@PrePercon", SqlDbType.Decimal, datos.PRC_PERCON, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, datos.numOperacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, datos.numCorrelativo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@MtoPension", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(datos.mtoPensio).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_PRIUNIDIF", SqlDbType.Decimal, datos.MTO_PRIUNIDIF, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_CTAINDAFP", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(datos.MTO_CTAINDAFP).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_RENTATMPAFP", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(datos.MTO_RENTATMPAFP).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vInd_filtroCotiza", SqlDbType.VarChar, datos.IND_FILTROCOTIZA, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vCod_estcot", SqlDbType.VarChar, datos.COD_ESTCOT, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@indMejex", SqlDbType.VarChar, datos.Ind_MejEx, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@MtoComision", SqlDbType.Decimal, datos.comision, ParameterDirection.Input));

                d = VCEDBContext<ActualizaInfOfi>.CallStoreProcedure(StoredProcedures.CO_CatalogoActualizaInfo, parameters, x => new ActualizaInfOfi
                {
                    modalidad = x.GetString(0),
                    moneda = x.GetString(1),
                    anosRT = x.GetInt32(2),
                    prcRVD = x.GetDecimal(3),
                    perGarantizado = x.GetString(4),
                    derechoCrecer = x.GetString(5),
                    gratificacion = x.GetString(6),
                    numCot = x.GetString(7),
                    tasaRT = x.GetDecimal(8),
                    periodoDiferido = x.GetInt32(9).ToString(),
                    codTipCambio = (double)x.GetDecimal(10)
                }).FirstOrDefault();

                double tasaRT = Convert.ToDouble(d.tasaRT);
                _log.Info("Se guardo correctamente comenzara a formar JSON del NumOperacion" + datos.numOperacion);
                double primeraPensionRT = 0;
                if (d.anosRT > 0)
                {
                    _log.Info("Tasa RT = " + d.tasaRT + "NumOperacion = "+ datos.numOperacion);
                    _log.Info("Moneda = " + d.moneda + "NumOperacion = " + datos.numOperacion);
                    if (d.moneda != "S/.Aj." && d.moneda != "S/.")
                    {
                        _log.Info("Monto Pension = " + datos.mtoPensio + "NumOperacion = " + datos.numOperacion);
                        _log.Info("Tipo de Cambio = " + d.codTipCambio + "NumOperacion = " + datos.numOperacion);
                        primeraPensionRT = (datos.mtoPensio * 2) * d.codTipCambio;
                        _log.Info("Primera pension RT = " + primeraPensionRT + "NumOperacion = " + datos.numOperacion);
                    }
                    else {
                        _log.Info("Monto Pension = " + datos.mtoPensio + "NumOperacion = " + datos.numOperacion);
                        primeraPensionRT = datos.mtoPensio * 2;
                        _log.Info("Primera pension RT = " + primeraPensionRT + "NumOperacion = " + datos.numOperacion);
                    }

                    if (d.modalidad == "RVE")
                    {
                        d.tasaRT = 0;
                    }

                }
                else
                {
                    d.tasaRT = 0;
                }
                _log.Info("PrimaUnicaAFPEESS = " + (Convert.ToDouble(datos.MTO_PRIUNIDIF) - Convert.ToDouble(d.periodoDiferido) * Convert.ToDouble(datos.MTO_RENTATMPAFP.Replace(",", ""))).ToString() + "NumOperacion = " + datos.numOperacion);
                _log.Info("Prima UnicaEESS  = " + datos.MTO_PRIUNIDIF + "NumOperacion = " + datos.numOperacion);
                object datosResult;
                if (d.modalidad != "RTVD")
                {
                    datosResult = new
                    {
                        modalidad = d.modalidad,
                        moneda = d.moneda,
                        anosRT = d.anosRT.ToString(),
                        porcentajeRVD = d.prcRVD.ToString(),
                        periodoGarantizado = d.perGarantizado,
                        derechoCrecer = d.derechoCrecer,
                        gratificacion = d.gratificacion,
                        cotizacionEESS = new
                        {
                            siCotizaNoCotiza = "S",
                            nroCotizacion = d.numCot,
                            primaUnicaAFPEESS = (Convert.ToDouble(datos.MTO_PRIUNIDIF) - Convert.ToDouble(d.periodoDiferido) * Convert.ToDouble(datos.MTO_RENTATMPAFP.Replace(",", ""))).ToString(),
                            primaUnicaEESS = datos.MTO_PRIUNIDIF.ToString(),
                            comision = datos.comision.ToString(),
                            primeraPensionRT = primeraPensionRT,
                            tasaInteresRT = d.tasaRT.ToString(),
                            primeraPensionRV = datos.mtoPensio.ToString(),
                            tasaInteresRV = datos.PRC_TASAVTA.ToString()
                        },
                        aceptado = "true"
                    };
                }
                else
                {
                    datosResult = new
                    {
                        modalidad = d.modalidad,
                        moneda = d.moneda,
                        anosRT = d.anosRT.ToString(),
                        porcentajeRVD = d.prcRVD.ToString(),
                        periodoGarantizado = d.perGarantizado,
                        derechoCrecer = d.derechoCrecer,
                        gratificacion = d.gratificacion,
                        cotizacionEESS = new
                        {
                            siCotizaNoCotiza = "S",
                            nroCotizacion = d.numCot,
                            primaUnicaAFPEESS = (Convert.ToDouble(datos.MTO_PRIUNIDIF) - Convert.ToDouble(d.periodoDiferido) * Convert.ToDouble(datos.MTO_RENTATMPAFP.Replace(",", ""))).ToString(),
                            primaUnicaEESS = datos.MTO_PRIUNIDIF.ToString(),
                            comision = datos.comision.ToString(),
                            primeraPensionRT = primeraPensionRT,
                            tasaInteresRT = d.tasaRT.ToString(),
                            primeraPensionRVD = datos.mtoPensio.ToString(),
                            tasaInteresRVD = datos.PRC_TASAVTA.ToString()
                        },
                        aceptado = "true"
                    };
                }
                res.Object = ser.Serialize(datosResult);
                res.Message = "Registro guardado con exito";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }

        public ActualizaInfOfi obtenerNA(string numOperacion, string numCor)
        {
            ActualizaInfOfi NA = new ActualizaInfOfi();
            string queryNum = "SELECT C.NUM_ARCHIVO, C.COD_TIPPENSION, DC.COD_TIPREN, C.IND_COB, ISNULL(DC.Ind_SISCO, 0) AS Ind_SISCO, C.COD_CUSPP,  CS.MTO_TIPCAMBIO FROM PT_TMAE_COTIZACION C " +
                              " JOIN PT_TMAE_DETCOTIZACION DC ON C.NUM_ARCHIVO = DC.NUM_ARCHIVO AND C.NUM_OPERACION = DC.NUM_OPERACION " +
                              "JOIN PT_THIS_CARGASOL CS ON CS.NUM_OPERACION = C.NUM_OPERACION " +
                              "WHERE C.NUM_OPERACION = " + numOperacion + " AND DC.NUM_CORRELATIVO = " + numCor;

            try
            {
                NA = SRVDBContext<ActualizaInfOfi>.CallSelectStatement(queryNum, x => new ActualizaInfOfi
                {
                    numArchivo = x.GetInt32(0),
                    codTP = x.GetString(1),
                    codTipRen = x.GetString(2),
                    Ind_Cob = x.GetString(3),
                    Ind_SISCO = x.GetInt32(4),
                    cuspp = x.GetString(5),
                    codTipCambio = (double)x.GetDecimal(6)
                }).FirstOrDefault();

                return NA;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Response busquedaMod(string numCor, string numOperacion)
        {
            Response res = new Response();
            try
            {
                double douNumOperacion = numOperacion == "" ? 0 : Convert.ToDouble(numOperacion);
                List<ActualizaInfOfi> _ActualizaInfOfiList = new List<ActualizaInfOfi>();
                ActualizaInfOfiRepository _ActualizaInfOfiRepository = new ActualizaInfOfiRepository();
                var parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSQUEDAMOD", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(douNumOperacion), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Decimal, Convert.ToDecimal(numCor), ParameterDirection.Input));
                _ActualizaInfOfiList = VCEDBContext<ActualizaInfOfi>.CallStoreProcedure(StoredProcedures.CO_CatalogoActualizaInfo, parameters, x => new ActualizaInfOfi
                {
                    //MTO_AJUSTEIPC = Convert.ToString(x.GetDecimal(0)),
                    MTO_CTAINDAFP = Convert.ToString(x.GetDecimal(1)),
                    mtoPensio = Convert.ToDouble(x.GetDecimal(2)),
                    MTO_PRIUNIDIF = Convert.ToString(x.GetDecimal(3)),
                    MTO_RENTATMPAFP = Convert.ToString(x.GetDecimal(4)),
                    //MTO_RESMAT = Convert.ToString(x.GetDecimal(5)),
                    PRC_PERCON = Convert.ToString(x.GetDecimal(6)),
                    //PRC_TASATCE = Convert.ToString(x.GetDecimal(7)),
                    PRC_TASATIR = Convert.ToString(x.GetDecimal(8)),
                    PRC_TASAVTA = Convert.ToString(x.GetDecimal(9)),
                    //Ind_SISCO = x.GetInt32(10),
                    IND_FILTROCOTIZA = x.GetString(11),
                    COD_ESTCOT = x.GetString(12),
                    Ind_MejEx = x.GetString(21),
                    comision = (double)x.GetDecimal(22),
                    codTipCambio = (double)x.GetDecimal(23)
                    //MTO_PENANUAL = Convert.ToString(x.GetDecimal(13)),
                    //MTO_PENSIONGAR = Convert.ToString(x.GetDecimal(14)),
                    //MTO_PRIUNISIM = Convert.ToString(x.GetDecimal(15)),
                    //MTO_RMGTOSEP = Convert.ToString(x.GetDecimal(16)),
                    //MTO_RMGTOSEPRV = Convert.ToString(x.GetDecimal(17)),
                    //MTO_VALREAJUSTEMEN = Convert.ToString(x.GetDecimal(18)),
                    //MTO_VALREAJUSTETRI = Convert.ToString(x.GetDecimal(19)),
                    //MTO_VALPREPENTMP = Convert.ToString(x.GetDecimal(20))
                }).ToList();
                
                    res.Object = _ActualizaInfOfiList;
                    res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }
    }
}
