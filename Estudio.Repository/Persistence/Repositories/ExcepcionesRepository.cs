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
using System.Web.Script.Serialization;

namespace Estudio.Repository.Persistence.Repositories
{
    public class ExcepcionesRepository
    {
        public double nuevaVT, nuevaCO;
        public int numArchivo, numCorrelativo;
        public string numCot, caso, parametro = "";
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Omar Figueroa Flores
        /// 25-10-2018
        /// Metodo que busca los registros que esten guardades con esa operacion o el cusspp
        /// </summary>
        /// <param name="numOperacion">Numero de operación con la que se desea obtener información</param>
        /// <param name="codCUSPP">Codigo CUSPP con el que se desea obtener información</param>
        /// <returns> Retorna los datos obtenidos para mostrarlos en la vista </returns>
        public Response busqueda(string codCUSPP, string numCor, string numOperacion)
        {
            Response res = new Response();
            XmlConfigurator.Configure();
            _log.Info("BUSQUEDA");
            _log.Info("PARAMETROS: Cuspp: " + codCUSPP);
            _log.Info("NumCorrelativo: " + numCor);
            _log.Info("NumOperacion: " + numOperacion);
            try
            {
                double douNumOperacion = numOperacion == "" ? 0 : Convert.ToDouble(numOperacion);
                List<Exceptiones> _ExceptionList = new List<Exceptiones>();
                ExcepcionesRepository _ExcepcionesRepository = new ExcepcionesRepository();
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
                _ExceptionList = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Exceptiones
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
                    mes_esc = x.GetInt32(29),
                    asesor = x.GetInt32(30).ToString(),
                    prestacion = x.GetString(31),
                    fechaCierre = (x.GetString(32).Substring(6, 2) + "/" + x.GetString(32).Substring(4, 2) + "/" + x.GetString(32).Substring(0, 4)),
                    departamentoMeler = x.GetString(33),
                    departamentoAsignado = x.GetString(34),
                    asesorDesc = x.GetString(35),
                    supervisor = x.GetString(36)
                }).ToList();
                if (_ExceptionList.Count != 0)
                {
                    List<string[]> datos = new List<string[]>();
                    string[] datosPer = { _ExceptionList[0].numOperacion.ToString(), _ExceptionList[0].dni, _ExceptionList[0].afp, _ExceptionList[0].cic.ToString(),
                                         _ExceptionList[0].asegurado, _ExceptionList[0].cuspp, _ExceptionList[0].sexo, _ExceptionList[0].fechaNac, _ExceptionList[0].numCot, _ExceptionList[0].numArchivo.ToString(),
                                         _ExceptionList[0].prestacion, _ExceptionList[0].fechaCierre, _ExceptionList[0].departamentoMeler, _ExceptionList[0].departamentoAsignado, _ExceptionList[0].asesorDesc, _ExceptionList[0].supervisor
                                        };
                    datos.Add(datosPer);

                    CalculoCotizacionRepository _calculoCotizacionRepository = new CalculoCotizacionRepository();

                    string sumaPension = "0";
                    double mtoSumPen = 0;



                    for (int i = 0; i < _ExceptionList.Count; i++)
                    {
                        Exceptiones NArchivo = _ExcepcionesRepository.obtenerNA(_ExceptionList[i].numOperacion.ToString(), _ExceptionList[i].numCorrelativo.ToString());

                        decimal suma = 0;
                        double PrimaUnica = 0;
                        double PrimerTramo = 0;
                        double SegundoTramo = 0;
                        int AniosDif = 0;
                        int rentEsc = 0;

                        //if (_ExceptionList[i].Ind_SISCO == 1)
                        //{
                        //    pensionSisco = _ExceptionList[i].mtoSumPension;

                        //}

                        var listBeneficiarios = _calculoCotizacionRepository.getBeneficiarios(NArchivo.numArchivo);

                        //for (int b = 0; b < listBeneficiarios.Count; b++)
                        //{
                        sumaPension = _ExceptionList[i].mtoPensio.ToString();

                        //switch (NArchivo.codTP)
                        //{
                        //    case "04":
                        //        sumaPension = _ExceptionList[i].mtoPensio.ToString();
                        //        break;
                        //    case "05":
                        //        sumaPension = _ExceptionList[i].mtoPensio.ToString();
                        //        break;
                        //    case "06":
                        //        if(_ExceptionList[i].Ind_Cob == "S")
                        //        sumaPension = Convert.ToDecimal(Convert.ToDouble(_ExceptionList[i].mtoPensio.ToString()) * (0.7)).ToString();
                        //        else
                        //            sumaPension = Convert.ToDecimal(Convert.ToDouble(_ExceptionList[i].mtoPensio.ToString())).ToString();
                        //        break;
                        //    case "07":
                        //        if(_ExceptionList[i].Ind_Cob=="S")
                        //        sumaPension = Convert.ToDecimal(Convert.ToDouble(_ExceptionList[i].mtoPensio.ToString()) * (0.5)).ToString();
                        //        else
                        //            sumaPension = Convert.ToDecimal(Convert.ToDouble(_ExceptionList[i].mtoPensio.ToString())).ToString();
                        //        break;
                        //    case "08":
                        //        if (listBeneficiarios[b].Num_Corr == _ExceptionList[i].numCorrelativo && listBeneficiarios[b].Num_Operacion == _ExceptionList[i].numOperacion)
                        //        {
                        //            suma = suma + listBeneficiarios[b].Mto_Pension * (listBeneficiarios[b].prc_Pension / 100);
                        //            sumaPension = suma.ToString();
                        //        }
                        //        break;
                        //}


                        switch (NArchivo.codTipRen)
                        {
                            case "1":
                                {
                                    _log.Info("INMEDIATA");
                                    //if (_ExceptionList[i].moneda != "S/.Aj." && _ExceptionList[i].moneda != "S/.")
                                    PrimaUnica = _ExceptionList[i].cic;

                                    PrimerTramo = 0 /*(Convert.ToDouble(sumaPension) * 2)*/;
                                    SegundoTramo = Convert.ToDouble(sumaPension);
                                    AniosDif = Convert.ToInt32(_ExceptionList[i].periodoDiferido);
                                    rentEsc = Convert.ToInt32(_ExceptionList[i].rentaEsc);
                                    break;
                                }
                            case "2":
                                {
                                    _log.Info("VITALICIA CON TEMPORAL");
                                    _log.Info("Moneda " + _ExceptionList[i].moneda);
                                    _log.Info("Suma pensión " + sumaPension);
                                    _log.Info("Monto de Cambio" + _ExceptionList[i].codTipCambio);
                                    _log.Info("Estado de la modalidad BD" + NArchivo.Ind_Estado);
                                    _log.Info("Estado de la modalidad " + NArchivo.Ind_Estado.Replace(" ", ""));
                                    _log.Info("Prima BD" + _ExceptionList[i].primaUnica);

                                    if (_ExceptionList[i].moneda != "S/.Aj." && _ExceptionList[i].moneda != "S/.")
                                    {
                                        //PrimaUnica = (_ExceptionList[i].primaUnica) * _ExceptionList[i].codTipCambio;
                                        PrimerTramo = ((Convert.ToDouble(sumaPension) * 2)) * _ExceptionList[i].codTipCambio;
                                    }
                                    else
                                    {
                                        // PrimaUnica = _ExceptionList[i].primaUnica;
                                        PrimerTramo = (Convert.ToDouble(sumaPension) * 2);
                                    }

                                    if (NArchivo.Ind_Estado.Replace(" ", "") == "E")
                                    {
                                        _log.Info("Entro a la validacion de archivo descargado");
                                        PrimaUnica = _ExceptionList[i].primaUnica;
                                    }
                                    else
                                    {
                                        _log.Info("Archivo no descargado aplicara validaciones");
                                        if (_ExceptionList[i].moneda != "S/.Aj." && _ExceptionList[i].moneda != "S/.")
                                        {
                                            PrimaUnica = (_ExceptionList[i].primaUnica) * _ExceptionList[i].codTipCambio;
                                        }
                                        else
                                        {
                                            PrimaUnica = _ExceptionList[i].primaUnica;
                                        }
                                    }


                                    SegundoTramo = Convert.ToDouble(sumaPension);
                                    AniosDif = Convert.ToInt32(_ExceptionList[i].periodoDiferido);
                                    rentEsc = Convert.ToInt32(_ExceptionList[i].rentaEsc);
                                    break;
                                }

                            case "6":
                                {
                                    //if (_ExceptionList[i].moneda != "S/.Aj." && _ExceptionList[i].moneda != "S/.")
                                    //{
                                    //    PrimerTramo = (Convert.ToDouble(sumaPension)) * _ExceptionList[i].codTipCambio;
                                    //    SegundoTramo = ((Convert.ToDouble(sumaPension) * Convert.ToInt32(_ExceptionList[i].rentaEsc)) / 100) * _ExceptionList[i].codTipCambio;
                                    //}
                                    //else
                                    //{
                                    PrimerTramo = Convert.ToDouble(sumaPension);
                                    // modalidades = _excepcionesRepository.ConsultarModalidadesModificar(idSolicitud.ToString(), _ExcepcionesRepository.numCorrelativo);
                                    SegundoTramo = ((Convert.ToDouble(sumaPension) * Convert.ToInt32(_ExceptionList[i].rentaEsc)) / 100);
                                    //}

                                    PrimaUnica = _ExceptionList[i].cic;
                                    AniosDif = Convert.ToInt32((_ExceptionList[i].mes_esc / 12));
                                    rentEsc = Convert.ToInt32(_ExceptionList[i].rentaEsc);
                                    break;
                                }
                        }
                        //}
                        //if (_ExceptionList[i].Ind_SISCO == 1)
                        //{

                        //double primafp = AniosDif * PrimerTramo;
                        //if (NArchivo.codTipRen == "6")
                        //{
                        //    primafp = 0;
                        //}
                        //PrimaUnica = _ExceptionList[i].cic - primafp;

                        string[] dTabla = { _ExceptionList[i].moneda, _ExceptionList[i].modalidad.ToString(), AniosDif.ToString()/*_ExceptionList[i].periodoDiferido*/, _ExceptionList[i].rentaTMP.ToString(),
                                    _ExceptionList[i].perGarantizado, _ExceptionList[i].rentaEsc.ToString(), PrimaUnica.ToString("N2")/*PrimaUnica.ToString()*/,  PrimerTramo.ToString("N2"),
                                   SegundoTramo.ToString("N2"), _ExceptionList[i].tasaVenta.ToString(), _ExceptionList[i].tir.ToString(), _ExceptionList[i].perdida, _ExceptionList[i].comision.ToString(), _ExceptionList[i].numCorrelativo.ToString(), _ExceptionList[i].mtoPensio.ToString("0.00"),
                                _ExceptionList[i].Ind_SISCO.ToString(),_ExceptionList[i].cod_tipreajuste.ToString()};
                        datos.Add(dTabla);

                        //}
                        //else
                        //{

                        //string[] dTabla = { _ExceptionList[i].moneda, _ExceptionList[i].modalidad.ToString(), _ExceptionList[i].periodoDiferido, _ExceptionList[i].rentaTMP.ToString(),
                        //        _ExceptionList[i].perGarantizado, _ExceptionList[i].rentaEsc.ToString(), PrimaUnica.ToString("N2")/*PrimaUnica.ToString()*/,  PrimerTramo.ToString("N2"),
                        //       SegundoTramo.ToString("N2"), _ExceptionList[i].tasaVenta.ToString(), _ExceptionList[i].tir.ToString(), _ExceptionList[i].perdida, _ExceptionList[i].comision.ToString(), _ExceptionList[i].numCorrelativo.ToString(), _ExceptionList[i].mtoPensio.ToString("0.00"), };
                        //datos.Add(dTabla);
                        //}

                        ///////////////////////////////////////////////////
                        //////////////////////////////////////////////
                        ///////////////////////////////////////////////

                        //if (_ExceptionList[i].codTipRen == "1") {
                        //    string[] dTabla = { _ExceptionList[i].moneda, _ExceptionList[i].modalidad.ToString(), _ExceptionList[i].periodoDiferido, _ExceptionList[i].rentaTMP.ToString(),
                        //            _ExceptionList[i].perGarantizado, _ExceptionList[i].rentaEsc.ToString(), _ExceptionList[i].primaUnica.ToString(), _ExceptionList[i].mtoPensio.ToString(), _ExceptionList[i].renTmp1T.ToString(),
                        //            _ExceptionList[i].mtoPensio.ToString(), _ExceptionList[i].tasaVenta.ToString(), _ExceptionList[i].tir.ToString(), _ExceptionList[i].perdida, _ExceptionList[i].comision.ToString(), _ExceptionList[i].numCorrelativo.ToString() };
                        //    datos.Add(dTabla);
                        //    //string[] dTabla = { _ExceptionList[i].moneda, _ExceptionList[i].modalidad.ToString(), _ExceptionList[i].periodoDiferido, _ExceptionList[i].rentaTMP.ToString(),
                        //    //        _ExceptionList[i].perGarantizado, _ExceptionList[i].rentaEsc.ToString(), _ExceptionList[i].cic.ToString(), _ExceptionList[i].mtoPensio.ToString(), _ExceptionList[i].renTmp1T.ToString(),
                        //    //        _ExceptionList[i].mtoPensio.ToString(), _ExceptionList[i].tasaVenta.ToString(), _ExceptionList[i].tir.ToString(), _ExceptionList[i].perdida, _ExceptionList[i].comision.ToString(), _ExceptionList[i].numCorrelativo.ToString() };
                        //    //datos.Add(dTabla);
                        //}
                        //if (_ExceptionList[i].codTipRen == "2")
                        //{
                        //    if (_ExceptionList[i].moneda == "US") {
                        //        string[] dTabla = { _ExceptionList[i].moneda, _ExceptionList[i].modalidad.ToString(), _ExceptionList[i].periodoDiferido, _ExceptionList[i].rentaTMP.ToString(),
                        //            _ExceptionList[i].perGarantizado, _ExceptionList[i].rentaEsc.ToString(), _ExceptionList[i].primaUnica.ToString(), _ExceptionList[i].mtoPensio.ToString(), (_ExceptionList[i].mtoPensio * 2 * _ExceptionList[i].codTipCambio).ToString(),
                        //            _ExceptionList[i].mtoPensio.ToString(), _ExceptionList[i].tasaVenta.ToString(), _ExceptionList[i].tir.ToString(), _ExceptionList[i].perdida, _ExceptionList[i].comision.ToString(), _ExceptionList[i].numCorrelativo.ToString()  };
                        //        datos.Add(dTabla);
                        //    }else
                        //    {
                        //        string[] dTabla = { _ExceptionList[i].moneda, _ExceptionList[i].modalidad.ToString(), _ExceptionList[i].periodoDiferido, _ExceptionList[i].rentaTMP.ToString(),
                        //            _ExceptionList[i].perGarantizado, _ExceptionList[i].rentaEsc.ToString(), _ExceptionList[i].primaUnica.ToString(), _ExceptionList[i].mtoPensio.ToString(), (_ExceptionList[i].mtoPensio * 2).ToString(),
                        //            _ExceptionList[i].mtoPensio.ToString(), _ExceptionList[i].tasaVenta.ToString(), _ExceptionList[i].tir.ToString(), _ExceptionList[i].perdida, _ExceptionList[i].comision.ToString(), _ExceptionList[i].numCorrelativo.ToString()  };
                        //        datos.Add(dTabla);
                        //    }
                        //}
                        //if (_ExceptionList[i].codTipRen == "6")
                        //{
                        //    if (_ExceptionList[i].moneda == "US")
                        //    {
                        //        string[] dTabla = { _ExceptionList[i].moneda, _ExceptionList[i].modalidad.ToString(), _ExceptionList[i].periodoDiferido, _ExceptionList[i].rentaTMP.ToString(),
                        //            _ExceptionList[i].perGarantizado, _ExceptionList[i].rentaEsc.ToString(), _ExceptionList[i].primaUnica.ToString(), _ExceptionList[i].mtoPensio.ToString(), (_ExceptionList[i].mtoPensio * _ExceptionList[i].codTipCambio).ToString(),
                        //            (_ExceptionList[i].mtoPensio * (_ExceptionList[i].rentaEsc)/100).ToString(), _ExceptionList[i].tasaVenta.ToString(), _ExceptionList[i].tir.ToString(), _ExceptionList[i].perdida, _ExceptionList[i].comision.ToString(), _ExceptionList[i].numCorrelativo.ToString()  };
                        //        datos.Add(dTabla);
                        //    }
                        //    else
                        //    {
                        //        string[] dTabla = { _ExceptionList[i].moneda, _ExceptionList[i].modalidad.ToString(), _ExceptionList[i].periodoDiferido, _ExceptionList[i].rentaTMP.ToString(),
                        //            _ExceptionList[i].perGarantizado, _ExceptionList[i].rentaEsc.ToString(), _ExceptionList[i].primaUnica.ToString(), _ExceptionList[i].mtoPensio.ToString(), (_ExceptionList[i].mtoPensio).ToString(),
                        //            (_ExceptionList[i].mtoPensio * (_ExceptionList[i].rentaEsc)/100).ToString(), _ExceptionList[i].tasaVenta.ToString(), _ExceptionList[i].tir.ToString(), _ExceptionList[i].perdida, _ExceptionList[i].comision.ToString(), _ExceptionList[i].numCorrelativo.ToString()  };
                        //        datos.Add(dTabla);
                        //    }
                        //}
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
        /// Omar Figueroa Flores
        /// 25-10-2018
        /// Metodo que realiza update del re-calculo con los datos obtenidos
        /// </summary>
        /// <param name="datos">Todos los datos necesarios para hacer el Update</param>
        /// <returns> Actuliza los datos de la modalidad </returns>
        public Response calculoU(Exceptiones datos, Exceptiones infoRut, string indMejex)
        {
            Response res = new Response();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Exceptiones d = new Exceptiones();
            XmlConfigurator.Configure();
            _log.Info("Comenzara a Guardar " + indMejex);
            try
            {
                int modalidad = 0;
                if (datos.modalidad != "1" && datos.modalidad != "3")
                {
                    modalidad = int.Parse(homologarmodalidad(datos.modalidad));
                }
                else
                {
                    modalidad = int.Parse(datos.modalidad);
                }

                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_CALCULO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@MtoPension", SqlDbType.Decimal, datos.mtoPensio, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@TasaVTA", SqlDbType.Decimal, datos.tasaVenta, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@TasaTIR", SqlDbType.Decimal, datos.tir, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@PrePercon", SqlDbType.Decimal, datos.perdida, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@MtoComision", SqlDbType.Decimal, datos.comision, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, datos.numOperacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, datos.numCorrelativo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@Modalidad", SqlDbType.Int, modalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSumaPension", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(datos.mtoSumPension).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_PRIUNIDIF", SqlDbType.Decimal, datos.primaUnica, ParameterDirection.Input));

                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_AJUSTEIPC", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_AJUSTEIPC).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_CTAINDAFP", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_CTAINDAFP).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_RENTATMPAFP", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_RENTATMPAFP).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_RESMAT", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_RESMAT).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vPRC_TASATCE", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.PRC_TASATCE).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFEC_CALCULO", SqlDbType.VarChar, infoRut.FecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_PENANUAL", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_PENANUAL).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_PENSIONGAR", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_PENSIONGAR).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_PRIUNISIM", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_PRIUNISIM).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_RMGTOSEP", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_RMGTOSEP).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_RMGTOSEPRV", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_RMGTOSEPRV).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_VALREAJUSTEMEN", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_VALREAJUSTEMEN).ToString("0.00000000")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_VALREAJUSTETRI", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_VALREAJUSTETRI).ToString("0.00000000")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_VALPREPENTMP", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_VALPREPENTMP).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@indMejex", SqlDbType.VarChar, indMejex, ParameterDirection.Input));
                //Parametros 
                d = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Exceptiones
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
                    codTipCambio = Convert.ToDouble(x.GetDecimal(10)),
                    cic = Convert.ToDouble(x.GetDecimal(11))
                }).FirstOrDefault();

                var primaUnicaAFP = (d.cic - datos.primaUnica);
                var primaUnicaES = (d.cic - Convert.ToDouble(primaUnicaAFP)).ToString();

                double primeraPensionRT = 0;

                double tasaRT = Convert.ToDouble(d.tasaRT);
                if (d.anosRT > 0)
                {
                    if (d.moneda != "S/.Aj." && d.moneda != "S/.")
                    {
                        primeraPensionRT = (datos.mtoPensio * 2) * d.codTipCambio;
                    }
                    else
                    {
                        primeraPensionRT = (datos.mtoPensio * 2);
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
                _log.Info("Comenzara a formar el JSON ");
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
                            primaUnicaAFPEESS = primaUnicaAFP.ToString(),//(Convert.ToDouble(info[0][6]) - Convert.ToDouble(info[0][2]) * Convert.ToDouble(info[0][7].Replace(",",""))).ToString(),
                            primaUnicaEESS = primaUnicaES.ToString(),//info[0][6].Replace(",", ""),
                            //primaUnicaAFPEESS = (datos.primaUnica - Convert.ToDouble(datos.periodoDiferido) * Convert.ToDouble(datos.primerTramo.Replace(",", ""))).ToString(),
                            //primaUnicaEESS = datos.primaUnica.ToString(),
                            comision = datos.comision.ToString(),
                            primeraPensionRT = primeraPensionRT,
                            tasaInteresRT = d.tasaRT.ToString(),
                            primeraPensionRV = datos.mtoPensio.ToString(),
                            tasaInteresRV = datos.tasaVenta.ToString()
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
                            primaUnicaAFPEESS = primaUnicaAFP.ToString(),//(Convert.ToDouble(info[0][6]) - Convert.ToDouble(info[0][2]) * Convert.ToDouble(info[0][7].Replace(",",""))).ToString(),
                            primaUnicaEESS = primaUnicaES.ToString(),//info[0][6].Replace(",", ""),
                            //primaUnicaAFPEESS = (datos.primaUnica - Convert.ToDouble(datos.periodoDiferido) * Convert.ToDouble(datos.primerTramo.Replace(",", ""))).ToString(),
                            //primaUnicaEESS = datos.primaUnica.ToString(),
                            comision = datos.comision.ToString(),
                            primeraPensionRT = primeraPensionRT,
                            tasaInteresRT = d.tasaRT.ToString(),
                            primeraPensionRVD = datos.mtoPensio.ToString(),
                            tasaInteresRVD = datos.tasaVenta.ToString()
                        },
                        aceptado = "true"
                    };
                }
                _log.Info("JSON a enviar por pantalla " + ser.Serialize(datosResult));
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
        public string homologarmodalidad(string glsElemento)
        {
            string res = "";
            try
            {
                Exceptiones _excepciones = new Exceptiones();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "HOMOLOGARMODALIDAD", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@glsElemento", SqlDbType.VarChar, glsElemento, ParameterDirection.Input));

                _excepciones = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Exceptiones
                {
                    cuspp = x.GetString(0)
                }).FirstOrDefault();
                if (_excepciones != null)
                {
                    res = _excepciones.cuspp;
                }
                return res;
            }
            catch (Exception)
            {
                return res;
            }
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 29-10-2018
        /// Devuelve objeto de la clase de Excepciones (Entity).
        /// </summary>
        /// <param name="strNumCot">Número de cotización.</param>
        /// <returns>Objeto con valores a utilizar en la rutina.</returns>
        public Exceptiones ConsultarCotizacion(string strNumOp)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTARCOTIZACION", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(strNumOp), ParameterDirection.Input));

                return VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Exceptiones
                {
                    cuspp = x.GetString(0),
                    fechaNac = x.GetString(1),
                    fechaNacDate = Convert.ToDateTime(x.GetString(1).Substring(6, 2) + "/" + x.GetString(1).Substring(4, 2) + "/" + x.GetString(1).Substring(0, 4)),
                    FechaDevengueStr = x.GetString(2),
                    FechaDevengue = Convert.ToDateTime(x.GetString(2).Substring(6, 2) + "/" + x.GetString(2).Substring(4, 2) + "/" + x.GetString(2).Substring(0, 4)),
                    cic = Convert.ToDouble(x.GetDecimal(3)),
                    FechaEstudioStr = x.GetString(4),
                    FechaEstudio = Convert.ToDateTime(x.GetString(4).Substring(6, 2) + "/" + x.GetString(4).Substring(4, 2) + "/" + x.GetString(4).Substring(0, 4)),
                    CodigoPension = x.GetString(5),
                    sexo = x.GetString(6),
                    codTipCambio = Convert.ToDouble(x.GetDecimal(7)),
                    afp = x.GetString(8),
                    Ind_Cob = x.GetString(9),
                    Fec_DevSolStr = x.GetString(10),
                    Fec_DevSol = Convert.ToDateTime(x.GetString(10).Substring(6, 2) + "/" + x.GetString(10).Substring(4, 2) + "/" + x.GetString(10).Substring(0, 4)),
                    Cod_region = x.GetString(11)
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 29-10-2018
        /// Devuelve objeto de la clase de Beneficiario (Entity).
        /// </summary>
        /// <param name="idCotizacion">Número de cotización.</param>
        /// <param name="strNumArch">Número de archivo de la cotizacion</param>
        /// <returns>Objeto con valores a utilizar en la rutina.</returns>
        public List<Beneficiario> ConsultarBeneficiariosModificar(string idCotizacion, int strNumArch)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTABENEFICIARIOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(idCotizacion), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, strNumArch, ParameterDirection.Input));
                List<Beneficiario> b = new List<Beneficiario>();
                b = VCEDBContext<Beneficiario>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Beneficiario
                {
                    TipoDocumento = x.IsDBNull(0) ? "" : x.GetString(0),
                    Documento = x.GetString(1),
                    FechaNacimientoStr = x.GetString(2),
                    FechaNacimiento = Convert.ToDateTime(x.GetString(2).Substring(6, 2) + "/" + x.GetString(2).Substring(4, 2) + "/" + x.GetString(2).Substring(0, 4)),
                    Sexo = x.GetString(4) == "M" ? "Masculino" : "Femenino",
                    IdBeneficiario = x.GetInt32(3),
                    ClaveSexo = x.GetString(4),
                    FechaFallecimientoStr = x.GetString(5) == "" ? "" : x.GetString(5).Substring(6, 2) + "/" + x.GetString(5).Substring(4, 2) + "/" + x.GetString(5).Substring(0, 4),
                    FechaInvalidezRut = x.IsDBNull(5) ? "" : x.GetString(5),
                    ClaveSituacionInvalidez = x.GetString(6),
                    CodigoParentesco = x.IsDBNull(7) ? "" : x.GetString(7),
                    Parentesco = x.IsDBNull(7) ? "" : x.GetString(7)
                }).ToList();
                return b;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 29-10-2018
        /// Devuelve objeto de la clase de Modalidad (Entity).
        /// </summary>
        /// <param name="idCotizacion">Número de cotización.</param>
        /// <returns>Objeto con valores a utilizar en la rutina.</returns>
        public List<Modalidad> ConsultarModalidadesModificar(string idCotizacion, int numCor)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTARMODALIDADESMOD", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.VarChar, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, numCor, ParameterDirection.Input));

                return VCEDBContext<Modalidad>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Modalidad
                {
                    Moneda = x.GetString(0),
                    AniosGarantizados = x.GetInt32(1),
                    AniosDiferidos = x.GetInt32(2),
                    PorcentajeRentaTemporal = Convert.ToInt32(x.GetDecimal(3)),
                    IdModalidad = x.GetInt32(4),
                    CodigoTiposRenta = x.GetString(5),
                    CodigoModalidad = x.GetString(6),
                    ClaveMoneda = x.GetString(7),
                    CodigoTipoReajuste = Convert.ToInt32(x.GetString(8) == "" ? "3" : x.GetString(8)),
                    ValorComision = x.GetDecimal(9),
                    DerGra = x.GetString(10),
                    PrimerTramo = x.GetInt32(11),
                    SegundoTramo = x.GetDecimal(12),
                    ComisionInicial = x.GetDecimal(13),
                    PrccomS = x.GetDecimal(14),
                    Prcfaclab = x.GetDecimal(15),
                    Prc_Inicial_1 = x.GetDecimal(16),
                    Prc_Inicial = x.GetDecimal(17),
                    Prc_Minimo_1 = x.GetDecimal(18),
                    Prc_Minimo = x.GetDecimal(19),
                    Excepsiscovs = x.GetString(20)

                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///  Omar Figueroa Flores
        /// 15-11-2018
        /// Metodo que busca los registros que esten guardados desde el Web service en la base de datos
        /// </summary>
        /// <returns> Regresa una lista de registros </returns>
        public List<Exceptiones> obtenerDatos()
        {
            List<Exceptiones> _Excepciones = new List<Exceptiones>();
            try
            {
                int count = 0;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETEXCEPCIONESEXTERNAS", ParameterDirection.Input));
                _Excepciones = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Exceptiones
                {
                    regTablaEx = count = count + 1,
                    cuspp = x.GetString(0),
                    numCorrelativo = x.GetInt32(1),
                    numOperacion = Convert.ToInt32(x.GetDecimal(2)),
                    motivo = x.GetString(3)
                }).ToList();

                return _Excepciones;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public Exceptiones obtenerNA(string numOperacion, string numCor)
        {
            Exceptiones NA = new Exceptiones();
            string queryNum = "SELECT C.NUM_ARCHIVO, C.COD_TIPPENSION, DC.COD_TIPREN, C.IND_COB, ISNULL(DC.Ind_SISCO, 0) AS Ind_SISCO, C.COD_CUSPP,  CS.MTO_TIPCAMBIO, DC.cod_estcot, DC.PRC_TASAVTA FROM PT_TMAE_COTIZACION C " +
                              " JOIN PT_TMAE_DETCOTIZACION DC ON C.NUM_ARCHIVO = DC.NUM_ARCHIVO AND C.NUM_OPERACION = DC.NUM_OPERACION " +
                              "JOIN PT_THIS_CARGASOL CS ON CS.NUM_OPERACION = C.NUM_OPERACION " +
                              "WHERE C.NUM_OPERACION = " + numOperacion + " AND DC.NUM_CORRELATIVO = " + numCor;

            try
            {
                NA = SRVDBContext<Exceptiones>.CallSelectStatement(queryNum, x => new Exceptiones
                {
                    numArchivo = x.GetInt32(0),
                    codTP = x.GetString(1),
                    codTipRen = x.GetString(2),
                    Ind_Cob = x.GetString(3),
                    Ind_SISCO = x.GetInt32(4),
                    cuspp = x.GetString(5),
                    codTipCambio = (double)x.GetDecimal(6),
                    Ind_Estado = x.GetString(7),
                    tasaVenta = (double)x.GetDecimal(8)
                }).FirstOrDefault();

                return NA;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Exceptiones VALIDACIONGUARDAR(int num_operacion, int num_correlativo)
        {
            Exceptiones Valida = new Exceptiones();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VALIDACIONGUARDAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Int, num_operacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, num_correlativo, ParameterDirection.Input));

                Valida = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Exceptiones
                {
                    Ind_Estado = x.GetString(0),
                    Ind_SISCO = x.GetInt32(1)
                }).FirstOrDefault();
                return Valida;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Response cancelar(string numCor, string numOperacion, string periodoDiferido, string primerTramo)
        {
            try
            {
                _log.Info("CANCELAR EXCEPCIONES");
                _log.Info("Comenzara a Cancelar y generar el JSON de WS excepciones");
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Exceptiones d = new Exceptiones();
                Response res = new Response();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CANCELAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, numOperacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, numCor, ParameterDirection.Input));

                //Parametros 
                d = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Exceptiones
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
                    primaUnica = Convert.ToDouble(x.GetDecimal(9)),
                    mtoPensio = Convert.ToDouble(x.GetDecimal(10)),
                    tasaVenta = Convert.ToDouble(x.GetDecimal(11)),
                    codTipCambio = Convert.ToDouble(x.GetDecimal(12)),
                    comision = Convert.ToDouble(x.GetDecimal(13))
                }).FirstOrDefault();
                _log.Info("Tasa RT " + Convert.ToDouble(d.tasaRT));
                _log.Info("Modalidad " + d.modalidad);
                _log.Info("Comisión " + d.comision.ToString());

                double tasaRT = Convert.ToDouble(d.tasaRT);
                double primeraPensionRT = 0;
                if (d.anosRT > 0)
                {
                    if (d.moneda != "S/.Aj." && d.moneda != "S/.")
                    {
                        primeraPensionRT = (d.mtoPensio * 2) * d.codTipCambio;
                    }
                    else
                    {
                        primeraPensionRT = d.mtoPensio * 2;
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
                _log.Info("Primera Pension RT " + primeraPensionRT.ToString());
                _log.Info("Tasa de Venta  " + d.tasaVenta.ToString());
                _log.Info("Periodo diferido " + periodoDiferido.ToString());
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
                            primaUnicaAFPEESS = (d.primaUnica - Convert.ToDouble(periodoDiferido) * Convert.ToDouble(primerTramo.Replace(",", ""))).ToString(),
                            primaUnicaEESS = d.primaUnica.ToString(),
                            comision = d.comision.ToString(),
                            primeraPensionRT = primeraPensionRT,
                            tasaInteresRT = d.tasaRT.ToString(),
                            primeraPensionRV = d.mtoPensio.ToString(),
                            tasaInteresRV = d.tasaVenta.ToString()
                        },
                        aceptado = "false"
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
                            primaUnicaAFPEESS = (d.primaUnica - Convert.ToDouble(periodoDiferido) * Convert.ToDouble(primerTramo.Replace(",", ""))).ToString(),
                            primaUnicaEESS = d.primaUnica.ToString(),
                            comision = d.comision.ToString(),
                            primeraPensionRT = primeraPensionRT,
                            tasaInteresRT = d.tasaRT.ToString(),
                            primeraPensionRVD = d.mtoPensio.ToString(),
                            tasaInteresRVD = d.tasaVenta.ToString()
                        },
                        aceptado = "false"
                    };
                }
                _log.Info("JSON a enviar Cancelar Excepciones " + ser.Serialize(datosResult));
                res.Object = ser.Serialize(datosResult);
                res.Message = "Los datos del calculo fueron cancelados";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                _log.Info("Error cancelar Excepciones " + ex.Message);
                throw;
            }
        }




        public Response calculoUExternas(Exceptiones datos, Exceptiones infoRut, string indMejex)
        {
            _log.Info("Calculo Excepciones Externas");
            Response res = new Response();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Exceptiones d = new Exceptiones();
            try
            {
                int modalidad = 0;
                if (datos.modalidad != "1" && datos.modalidad != "3")
                {
                    modalidad = int.Parse(homologarmodalidad(datos.modalidad));
                }
                else
                {
                    modalidad = int.Parse(datos.modalidad);
                }
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_CALCULOEXT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@MtoPension", SqlDbType.Decimal, datos.mtoPensio, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@TasaVTA", SqlDbType.Decimal, datos.tasaVenta, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@TasaTIR", SqlDbType.Decimal, datos.tir, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@PrePercon", SqlDbType.Decimal, datos.perdida, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@MtoComision", SqlDbType.Decimal, datos.comision, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, datos.numOperacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, datos.numCorrelativo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@Modalidad", SqlDbType.Int, modalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSumaPension", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(datos.mtoSumPension).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_PRIUNIDIF", SqlDbType.Decimal, datos.primaUnica, ParameterDirection.Input));

                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_AJUSTEIPC", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_AJUSTEIPC).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_CTAINDAFP", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_CTAINDAFP).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_RENTATMPAFP", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_RENTATMPAFP).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_RESMAT", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_RESMAT).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vPRC_TASATCE", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.PRC_TASATCE).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFEC_CALCULO", SqlDbType.VarChar, infoRut.FecCal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_PENANUAL", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_PENANUAL).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_PENSIONGAR", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_PENSIONGAR).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_PRIUNISIM", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_PRIUNISIM).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_RMGTOSEP", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_RMGTOSEP).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_RMGTOSEPRV", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_RMGTOSEPRV).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_VALREAJUSTEMEN", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_VALREAJUSTEMEN).ToString("0.00000000")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_VALREAJUSTETRI", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_VALREAJUSTETRI).ToString("0.00000000")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vMTO_VALPREPENTMP", SqlDbType.Decimal, Convert.ToDecimal(Convert.ToDouble(infoRut.MTO_VALPREPENTMP).ToString("0.00")), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@indMejex", SqlDbType.VarChar, indMejex, ParameterDirection.Input));
                //Parametros 
                d = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Exceptiones
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
                    codTipCambio = Convert.ToDouble(x.GetDecimal(9))
                }).FirstOrDefault();
                _log.Info("Tasa RT EE " + Convert.ToDouble(d.tasaRT));
                _log.Info("Modalidad " + d.modalidad);
                _log.Info("Comisión Calculo " + datos.comision.ToString());

                double tasaRT = Convert.ToDouble(d.tasaRT);
                double primeraPensionRT = 0;
                if (d.anosRT > 0)
                {
                    if (d.moneda != "S/.Aj." && d.moneda != "S/.")
                    {
                        primeraPensionRT = (datos.mtoPensio * 2) * d.codTipCambio;
                    }
                    else
                    {
                        primeraPensionRT = (datos.mtoPensio * 2);
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
                            primaUnicaAFPEESS = (datos.primaUnica - Convert.ToDouble(datos.periodoDiferido) * Convert.ToDouble(datos.primerTramo.Replace(",", ""))).ToString(),
                            primaUnicaEESS = datos.primaUnica.ToString(),
                            comision = datos.comision.ToString(),
                            primeraPensionRT = primeraPensionRT,
                            tasaInteresRT = d.tasaRT.ToString(),
                            primeraPensionRV = datos.mtoPensio.ToString(),
                            tasaInteresRV = datos.tasaVenta.ToString()
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
                            primaUnicaAFPEESS = (datos.primaUnica - Convert.ToDouble(datos.periodoDiferido) * Convert.ToDouble(datos.primerTramo.Replace(",", ""))).ToString(),
                            primaUnicaEESS = datos.primaUnica.ToString(),
                            comision = datos.comision.ToString(),
                            primeraPensionRT = primeraPensionRT,
                            tasaInteresRT = d.tasaRT.ToString(),
                            primeraPensionRVD = datos.mtoPensio.ToString(),
                            tasaInteresRVD = datos.tasaVenta.ToString()
                        },
                        aceptado = "true"
                    };
                }
                _log.Info("JSON a enviar Cancelar Excepciones " + ser.Serialize(datosResult));
                res.Object = ser.Serialize(datosResult);

                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                _log.Info("Error calculo Excepciones Externas " + ex.Message);
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }

        public string ConsultaAsesor(string numOperacion)
        {
            string res = "";
            try
            {
                Exceptiones _excepciones = new Exceptiones();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTA_ASESOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(numOperacion), ParameterDirection.Input));

                _excepciones = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_CatalogosExcepciones, parameters, x => new Exceptiones
                {
                    asesor = x.GetString(0)
                }).FirstOrDefault();
                if (_excepciones != null)
                {
                    res = _excepciones.asesor;
                }
                return res;
            }
            catch (Exception ex)
            {
                return res;
            }
        }
    }
}
