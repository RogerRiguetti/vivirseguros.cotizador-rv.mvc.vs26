using Estudio.Logic;
using Estudio.Repository;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Estudio.Controllers
{
    
    public class clXML
    {
        wsXML.AdmIntegracionCotizador _wsXML = new wsXML.AdmIntegracionCotizador();
        prueba.WebServiceOficiales _calculosWSL = new prueba.WebServiceOficiales();
        /// <summary>
        /// Omar Figueroa Flores
        /// 16-01-2019
        /// Envia el documento xml
        /// </summary>
        /// <param name="docXML">Parametro que contendra el documento xml en una cadena</param>
        /// <returns>"OK# o KO#ERRORO"</returns>
        public Response cargaCotizacionesOficiales(string docXML, string nombre)
        {
            Response res = new Response();
            res.IsOk = true;
            try
            {
                string valor = _wsXML.CargarCotizacionesOficiales(docXML, nombre);
                res.Message = valor;
                return res;
            }
            catch (Exception ex)
            {
                res.Message = "KO#" +ex.Message;
                return res;
            }
        }
        public string calculo(string __docXML, string nombreArchivo, string usuario)
        {
            try
            {
                _calculosWSL.Timeout = 900000;
                string res = _calculosWSL.calculoXML(__docXML, nombreArchivo, usuario).ToString();



                return res;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public object calculoMejoras()
        {
            try
            {
                _calculosWSL.Timeout = 600000;
                var obj1 = _calculosWSL.calculoMejoras("197401PRQAS0", 145853, "RVE", "S/.Aj.", 15, 50.00, 15, "N", "N", true, 3);
                var obj = calculoMejoras("197401PRQAS0", 145853, "RVE", "S/.Aj.", 15, 50.00, 15, "N", "N", true, 3);
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public object calculoMejoras(string cuspp, int num_oper, string modalidad, string moneda, int aniosRT, double porcentajeRVD, int periodoGarantizado, string derechoCrecer, string gratificacion, bool prc_Tv, double prc_Com)
        {
            //XmlConfigurator.Configure();

            Exceptiones mod = new Exceptiones();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Response datos = new Response();
            try
            {
                ExcepcionesLogic _ExcepcionesLogic = new ExcepcionesLogic();
                var parameters = new List<SqlParameter>();
                if (periodoGarantizado != 0)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARMODALIDADT", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@periodoGarantizado", SqlDbType.Int, periodoGarantizado, ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARMODALIDAD", ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCUSPP", SqlDbType.VarChar, cuspp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Int, num_oper, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@modalidad", SqlDbType.VarChar, modalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@moneda", SqlDbType.VarChar, moneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@aniosRT", SqlDbType.Int, aniosRT, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@porcentajeRVD", SqlDbType.Decimal, Convert.ToDecimal(porcentajeRVD), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@derechoCrecer", SqlDbType.VarChar, derechoCrecer, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@gratificacion", SqlDbType.VarChar, gratificacion, ParameterDirection.Input));
                
                mod = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_WebService, parameters, x => new Exceptiones
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
                    tasaRT = x.GetDecimal(26)
                }).FirstOrDefault();
                string strBand = "";
                if (prc_Tv == true)
                {
                    strBand = "true";
                }
                else
                {
                    strBand = "false";
                }
                if (mod == null)
                {
                    var datosResult = new
                    {
                        error = "NO SE ENCONTRÓ NINGUNA MODALIDAD CON ESOS DATOS"
                    };
                    return ser.Serialize(datosResult);
                }
                else
                {
                    datos = Task.Run(() => _ExcepcionesLogic.calculo(strBand, prc_Com.ToString(), mod, "mejoras", "")).Result;
                }
                //var datos = await _ExcepcionesLogic.calculo(strBand, prc_Com.ToString(), mod, "mejoras", "");
                if (datos.IsOk == false)
                {
                    object datosResult = new
                    {
                        modalidad = modalidad,
                        moneda = moneda,
                        anosRT = aniosRT.ToString(),
                        porcentajeRVD = porcentajeRVD.ToString(),
                        periodoGarantizado = periodoGarantizado.ToString(),
                        derechoCrecer = derechoCrecer,
                        gratificacion = gratificacion,
                        cotizacionEESS = new
                        {
                            siCotizaNoCotiza = "S",
                            nroCotizacion = mod.numCot,
                            primaUnicaAFPEESS = "0",
                            primaUnicaEESS = "0",
                            primeraPensionRT = "0",
                            tasaInteresRT = mod.tasaRT.ToString(),
                            primeraPensionRVD = "0",
                            tasaInteresRVD = "0"
                        },
                        error = datos.Message
                    };
                    return ser.Serialize(datosResult);
                }
                else
                {

                    var info = (List<string[]>)datos.Object;


                    /* DATOS QUE SALEN DE LA CONSULTA
                    datos.moneda, datos.modalidad.ToString(), AniosDif.ToString(), datos.rentaTMP.ToString(),
                                    datos.perGarantizado, rentEsc.ToString(), primaUni.ToString("N2"), /sumaPension,/PrimerTramo.ToString("N2"),
                                    SegundoTramo.ToString("N2"),tasvtarut.ToString()/_ExcepcionesRepository.nuevaVT.ToString()/, rutinaOficiales[0][i].PRC_TASATIR.ToString(),
                                    rutinaOficiales[0][i].PRC_PERCON.ToString(), _ExcepcionesRepository.nuevaCO.ToString(), "", mtoPen.ToString(),"",datos.cod_tipreajuste.ToString(),
                                    //lo new
                      rutinaOficiales[0][i].MTO_AJUSTEIPC.ToString(),rutinaOficiales[0][i].MTO_CTAINDAFP.ToString(),rutinaOficiales[0][i].MTO_RENTATMPAFP.ToString(),
                                    rutinaOficiales[0][i].MTO_RESMAT.ToString(),rutinaOficiales[0][i].PRC_TASATCE.ToString(),
                                    rutinaOficiales[0][i].FecCal, rutinaOficiales[0][i].MTO_PENANUAL.ToString(), rutinaOficiales[0][i].MTO_PENSIONGAR.ToString(), rutinaOficiales[0][i].MTO_PRIUNISIM.ToString(),
                                    rutinaOficiales[0][i].MTO_RMGTOSEP.ToString(), rutinaOficiales[0][i].MTO_RMGTOSEPRV.ToString(),MTO_VALREAJUSTEMEN,MTO_VALREAJUSTETRI,MTO_VALPREPENTMP              
                     */
                    double primeraPensionRT = 0;
                    
                    if (Convert.ToInt32(info[0][2]) > 0 || aniosRT > 0)
                    {
                        if (moneda == "S/." || moneda == "S/.Aj.")
                        {
                            primeraPensionRT = (Convert.ToDouble(info[0][8].Replace(",", "")) * 2);
                        }
                        else
                        {
                            primeraPensionRT = ((Convert.ToDouble(info[0][8].Replace(",", "")) * 2) * mod.codTipCambio);
                        }
                    }
                    string primaUnicaAFP = "";
                    string primaUnicaES = "";

                    //if (moneda == "S/." || moneda == "S/.Aj.")
                    //{

                    //    _log.Info("PrimerTramo " + Convert.ToDouble(info[0][7]));
                    //    _log.Info("Prima Unica " + (Convert.ToDouble(info[0][6].Replace(",", ""))));
                    //    primaUnicaAFP = ((Convert.ToDouble(info[0][2])) * primeraPensionRT).ToString();
                    //    if (Convert.ToDouble(info[0][2]) == 0 || modalidad == "RVE")
                    //    {
                    //        primaUnicaAFP = "0";
                    //    }
                    //    primaUnicaES = (mod.cic - Convert.ToDouble(primaUnicaAFP)).ToString();// (Convert.ToDouble(info[0][6].Replace(",", "")) /*- Convert.ToDouble(primaUnicaAFP)*/).ToString();

                    //}

                    //else
                    //{
                    //    if (Convert.ToDouble(info[0][2]) > 0 || modalidad != "RVE")
                    //    {
                    //        _log.Info("PrimerTramo " + Convert.ToDouble(info[0][7]));
                    //        _log.Info("Tipo de Cambio " + mod.codTipCambio);
                    //        primaUnicaAFP = (((Convert.ToDouble(info[0][2])) * primeraPensionRT)).ToString(); //* mod.codTipCambio).ToString();
                    //    }
                    //    else { primaUnicaAFP = "0"; }

                    //    _log.Info("Prima Unica " + (Convert.ToDouble(info[0][6].Replace(",", ""))));
                    //    primaUnicaES = ((mod.cic - Convert.ToDouble(primaUnicaAFP))).ToString(); //* mod.codTipCambio).ToString();//(Convert.ToDouble(info[0][6].Replace(",", "")) /*- Convert.ToDouble(primaUnicaAFP))*/ /* * mod.codTipCambio)*/).ToString();


                    //}
                    if (moneda == "S/." || moneda == "S/.Aj.")
                    {
                        primaUnicaAFP = ((mod.cic - Convert.ToDouble(info[0][6].Replace(",", ""))) / mod.codTipCambio).ToString();
                    }
                    else
                    {
                        primaUnicaAFP = (mod.cic - (Convert.ToDouble(info[0][6].Replace(",", "")))).ToString();
                    }
                    
                    primaUnicaES = (mod.cic - Convert.ToDouble(primaUnicaAFP)).ToString();

                    object datosResult = new
                    {
                        modalidad = modalidad,
                        moneda = moneda,
                        anosRT = aniosRT.ToString(),
                        porcentajeRVD = porcentajeRVD.ToString(),
                        periodoGarantizado = periodoGarantizado.ToString(),
                        derechoCrecer = derechoCrecer,
                        gratificacion = gratificacion,
                        cotizacionEESS = new
                        {
                            siCotizaNoCotiza = "S",
                            nroCotizacion = mod.numCot,
                            primaUnicaAFPEESS = primaUnicaAFP.ToString(),//(Convert.ToDouble(info[0][6]) - Convert.ToDouble(info[0][2]) * Convert.ToDouble(info[0][7].Replace(",",""))).ToString(),
                            primaUnicaEESS = primaUnicaES.ToString(),//info[0][6].Replace(",", ""),
                            primeraPensionRT = primeraPensionRT.ToString(),
                            tasaInteresRT = mod.tasaRT.ToString(),
                            primeraPensionRVD = info[0][8].Replace(",", ""),
                            tasaInteresRVD = info[0][9]
                        }

                    };
                    return ser.Serialize(datosResult);
                }
            }
            catch (Exception ex)
            {
                object datosResult = new
                {
                    modalidad = modalidad,
                    moneda = moneda,
                    anosRT = aniosRT.ToString(),
                    porcentajeRVD = porcentajeRVD.ToString(),
                    periodoGarantizado = periodoGarantizado.ToString(),
                    derechoCrecer = derechoCrecer,
                    gratificacion = gratificacion,
                    cotizacionEESS = new
                    {
                        siCotizaNoCotiza = "S",
                        nroCotizacion = mod.numCot,
                        primaUnicaAFPEESS = "0",
                        primaUnicaEESS = "0",
                        primeraPensionRT = "0",
                        tasaInteresRT = mod.tasaRT.ToString(),
                        primeraPensionRVD = "0",
                        tasaInteresRVD = "0"
                    },
                    error = ex.Message
                };
                return ser.Serialize(datosResult);
            }
        }
    }
}