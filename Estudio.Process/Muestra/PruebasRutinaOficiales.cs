using Estudio.Repository;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Estudio.Process.Muestra
{
    public class PruebasRutinaOficiales
    {
        RutinaOficialesRepository _rutinaOficialesRepository = new RutinaOficialesRepository();
        SolicitudCotizacionesRepository _solicitudCotizacionesRepository = new SolicitudCotizacionesRepository();
        CalcularAsignacionIntermediarioRepository _calcularAsignacionIntermediarioRepository = new CalcularAsignacionIntermediarioRepository();
        ExcepcionesRepository _excepcionesRepository = new ExcepcionesRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        List<beResultados> ResultadoCotGlobal = new List<beResultados>();
        CalculoCotizacionRepository _calculoCotizacionRepository = new CalculoCotizacionRepository();
        List<beDatosBen> ListaBen_tmp = new List<beDatosBen>();


        [STAThread]
        public async Task<List<beResultados>> RutinaOficiales(int idSolicitud, string strNumCot, int NumArch, List<beMortalidadDin> LisTabDinPar, List<beMortalidadDinDet> LisTabMDPar, List<beDatosModalidad> VarMTGSPar,
            List<beDatosModalidad> VarAFPPar, List<beDatosModalidad> VarREGPar, List<bePorcenLegales> LisTabPLPar, List<beTasaAnclaje> ListaTAPar, List<beCPK> ListaCPKPar, List<beRentabilidad> ListaRenPar,
            List<beTasasPromedio> ListaTasPromPar, List<beCurvaTasas> ListaCurvaTasasPar, ExcepcionesRepository _ExcepcionesRepository, string bandera, int asesor)
        {
            XmlConfigurator.Configure();
            beResultados rutina = new beResultados();
            List<beResultados> ResultadoCot = new List<beResultados>();
            RutinaActOficiales RActuarial = new RutinaActOficiales();

            //SolicitudesCotizacion solicitudCotizacion = _solicitudCotizacionesRepository.
           
            AsignacionIntermediario cotizacion = new AsignacionIntermediario();
            List<Beneficiario> beneficiarios = new List<Beneficiario>();
            List<Modalidad> modalidades = new List<Modalidad>();
            Exceptiones cotizacionExcepciones = new Exceptiones();
            double Tasavta = 0;
            double Comi = 0;

            if (_ExcepcionesRepository == null)
            {
                cotizacion = _calcularAsignacionIntermediarioRepository.ConsultarCotizacion(idSolicitud.ToString());
                beneficiarios = _calcularAsignacionIntermediarioRepository.ConsultarBeneficiariosModificar(idSolicitud.ToString(), NumArch.ToString()); //idSolicitud(Num. Operación)
                modalidades = _calcularAsignacionIntermediarioRepository.ConsultarModalidadesModificar(idSolicitud.ToString());
                if (modalidades.Count() == 0)
                {
                    List<beResultados> erroresRutina = new List<beResultados>();
                    beResultados errorRutina = new beResultados();

                    errorRutina.Mensaje = "No se encontro ninguna modalidad cotizable";
                    errorRutina.Cod_Rechazo = "902";
                    errorRutina.FecCal = cotizacion.FechaEstudioStr;
                    errorRutina.NUM_COTESTUDIO = idSolicitud.ToString();
                    errorRutina.INDFILTROCOTIZA = "N";
                    erroresRutina.Add(errorRutina);
                    return erroresRutina;
                }
              
            }
            else
            {
                cotizacionExcepciones = _excepcionesRepository.ConsultarCotizacion(idSolicitud.ToString());
                beneficiarios = _excepcionesRepository.ConsultarBeneficiariosModificar(idSolicitud.ToString(), NumArch);
                modalidades = _excepcionesRepository.ConsultarModalidadesModificar(idSolicitud.ToString(), _ExcepcionesRepository.numCorrelativo);
                Tasavta = _ExcepcionesRepository.nuevaVT;
                Comi = _ExcepcionesRepository.nuevaCO;
                //  double PrcCom = _ExcepcionesRepository == null ? Convert.ToDouble(modalidades.) : _ExcepcionesRepository.nuevaCO;
            }
            
            string FecCal = cotizacion.FechaEstudioStr == null ? cotizacionExcepciones.FechaEstudioStr : cotizacion.FechaEstudioStr;
            string TipPen = cotizacion.CodigoPension == null ? cotizacionExcepciones.CodigoPension : cotizacion.CodigoPension;
            string cobertura = cotizacion.Ind_Cob == null ? cotizacionExcepciones.Ind_Cob : cotizacion.Ind_Cob;

            string fechaDevengueSol = cotizacion.Fec_DevSolStr == null ? cotizacionExcepciones.Fec_DevSol.ToString("yyyy") + cotizacionExcepciones.Fec_DevSol.ToString("MM") + "02" :
                   cotizacion.Fec_DevSol.ToString("yyyy") + cotizacion.Fec_DevSol.ToString("MM") + "02";

            string fechaDevengue = cotizacion.FechaDevengueStr == null ? cotizacionExcepciones.FechaDevengue.ToString("yyyy") + cotizacionExcepciones.FechaDevengue.ToString("MM") + "01" :
                    cotizacion.FechaDevengue.ToString("yyyy") + cotizacion.FechaDevengue.ToString("MM") + "01";

            int MortalVit_F, MortalTot_F, MortalPar_F, MortalBen_F, MortalVit_M, MortalTot_M, MortalPar_M, MortalBen_M;

            #region Carga Matriz de Tablas de Mortalidad


            //string Query = "";
            //int MortalVit_F, MortalTot_F, MortalPar_F, MortalBen_F, MortalVit_M, MortalTot_M, MortalPar_M, MortalBen_M;

            //RutinaMortalidad RMortal = new RutinaMortalidad();
            //List<beMortalidad> ListaMor = new List<beMortalidad>();
            //List<beMortalVar> LisTab = new List<beMortalVar>();
            //try
            //{

            //    Query = "SELECT GLS_NOMBRE,NUM_CORRELATIVO,COD_TIPTABMOR,COD_SEXO,COD_TIPOPER FROM MA_TVAL_MORTAL " +
            //                   "WHERE FEC_INI<= " + FecCal + " AND FEC_TER>= " + FecCal + " AND  COD_TIPTABMOR<>'IND' AND COD_TIPOPER='M'";
            //    cmd.CommandText = Query;
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Connection = conexion;
            //    //Console.WriteLine("{0}", "iNICIA");
            //    conexion.Open();
            //    reader = cmd.ExecuteReader();

            //    if (reader.HasRows ==  true)
            //    {
            //        while (reader.Read())
            //        {
            //            beMortalVar Tab = new beMortalVar();

            //            Tab.GLS_NOMBRE = reader.GetString(0);
            //            Tab.NUM_CORRELATIVO = reader.GetInt32(1);
            //            Tab.COD_TIPTABMOR = reader.GetString(2);
            //            Tab.COD_SEXO = reader.GetString(3);
            //            Tab.COD_TIPOPER = reader.GetString(4);
            //            LisTab.Add(Tab);
            //            //Console.WriteLine("{0}", reader.GetString(0));
            //        }
            //    }
            //    reader.Close();
            //    MortalVit_F = LisTab.Where(x => x.COD_TIPTABMOR == "RV" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalVit_M = LisTab.Where(x => x.COD_TIPTABMOR == "RV" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalTot_F = LisTab.Where(x => x.COD_TIPTABMOR == "MIT" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalTot_M = LisTab.Where(x => x.COD_TIPTABMOR == "MIT" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalPar_F = LisTab.Where(x => x.COD_TIPTABMOR == "MIP" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalPar_M = LisTab.Where(x => x.COD_TIPTABMOR == "MIP" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalBen_F = LisTab.Where(x => x.COD_TIPTABMOR == "B" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalBen_M = LisTab.Where(x => x.COD_TIPTABMOR == "B" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    conexion.Close();
            //    //OBTIENE EL DETALLE DE LAS TABLAS DE MORTALIDAD

            //    Query = "Select num_correlativo as numCor,num_edad AS edad,mto_lx from MA_TVAL_MORDET order by num_edad";
            //    cmd.CommandText = Query;
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Connection = conexion;
            //    conexion.Open();
            //    reader = cmd.ExecuteReader();


            //    List<beMortalidadDet> LisTabMD = new List<beMortalidadDet>();

            //    if (reader != null)
            //    {
            //        while (reader.Read())
            //        {
            //            beMortalidadDet TabMD = new beMortalidadDet();
            //            TabMD.numCor = reader.GetInt32(0);
            //            TabMD.edad = reader.GetInt32(1);
            //            TabMD.mto_lx = reader.GetDecimal(2);
            //            LisTabMD.Add(TabMD);
            //            //Console.WriteLine("{0}", reader.GetString(2));
            //        }
            //        reader.Close();
            //    }
            //    conexion.Close();
            //    //llamo al listado de Tablas de Mortalidad

            //    ListaMor = RMortal.TablaMortalidad(LisTabMD, MortalVit_F, MortalTot_F, MortalPar_F, MortalBen_F, MortalVit_M, MortalTot_M, MortalPar_M, MortalBen_M);

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("{0}", ex.Message);
            //    conexion.Close();

            //}


            #endregion

            #region Carga Tabla Mortaliadad Dinamicas


            int Mortal_M_S, Mortal_F_S, Mortal_M_I, Mortal_F_I;

            RutinaMortalidadDin RMortalDin = new RutinaMortalidadDin();
            List<beMortalidadDin> LisTabDin = new List<beMortalidadDin>();
            List<beMortalidadDinVal> ListaMor = new List<beMortalidadDinVal>();
            try
            {
                // LisTabDin = _rutinaOficialesRepository.ConsultaTablaMortalidadDinamicas(FecCal);
                LisTabDin = (from td in LisTabDinPar where Convert.ToInt32(td.FEC_INIVIG) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(td.FEC_FINVIG) select td).ToList();

                Mortal_M_S = LisTabDin.Where(x => x.COD_INVALIDEZ == "S" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_F_S = LisTabDin.Where(x => x.COD_INVALIDEZ == "S" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_M_I = LisTabDin.Where(x => x.COD_INVALIDEZ == "I" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_F_I = LisTabDin.Where(x => x.COD_INVALIDEZ == "I" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();

                //OBTIENE EL DETALLE DE LAS TABLAS DE MORTALIDAD

                List<beMortalidadDinDet> LisTabMD = new List<beMortalidadDinDet>();
                // LisTabMD = _rutinaOficialesRepository.ConsultaDetTablaMortalidadDin();
                LisTabMD = LisTabMDPar;

                //llamo al listado de Tablas de Mortalidad
                ListaMor = RMortalDin.TablaMortalidad(LisTabMD, Mortal_M_S, Mortal_F_S, Mortal_M_I, Mortal_F_I);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
            }

            #endregion

            #region Carga Parametros

            beDatosModalidad valoresFactores = new beDatosModalidad();
            List<beDatosModalidad> ListaPar = new List<beDatosModalidad>();
            List<List<beDatosModalidad>> ListaModalidades = new List<List<beDatosModalidad>>();
            List<beDatosTasasPar> ListaTas = new List<beDatosTasasPar>();

            foreach (var item in modalidades)
            {
                beDatosModalidad TablaPar = new beDatosModalidad();
                ListaPar = new List<beDatosModalidad>();

                //string fechaDevengue = cotizacion.FechaDevengueStr == null ? cotizacionExcepciones.FechaDevengue.ToString("yyyy") + cotizacionExcepciones.FechaDevengue.ToString("MM") + "01" :
                //     cotizacion.FechaDevengue.ToString("yyyy") + cotizacion.FechaDevengue.ToString("MM") + "01";

                //string fechaDevengueSol = cotizacion.FechaDevengueStr == null ? cotizacionExcepciones.FechaDevengue.ToString("yyyy") + cotizacionExcepciones.FechaDevengue.ToString("MM") + "01" :
                //    cotizacion.FechaDevengue.ToString("yyyy") + cotizacion.FechaDevengue.ToString("MM") + "02";

                TablaPar.NumCot = idSolicitud.ToString();// "9999999999";
                TablaPar.FinTab = 1332;
                TablaPar.Tippen = TipPen;
                TablaPar.TipRen = item.CodigoTiposRenta;
                TablaPar.TipMod = item.CodigoModalidad;
                TablaPar.MesGar = item.AniosGarantizados;
                if (item.CodigoTiposRenta == "6")
                {
                    TablaPar.MesDif = Convert.ToInt32(item.PrimerTramo);
                    TablaPar.RenTmp = Convert.ToInt32(item.SegundoTramo);
                }
                else
                {
                    TablaPar.MesDif = item.AniosDiferidos;
                    TablaPar.RenTmp = item.PorcentajeRentaTemporal;
                }
                TablaPar.FecCot = FecCal;
                TablaPar.MtoPri = cotizacion.Cic == 0 ? cotizacionExcepciones.cic : Convert.ToDouble(cotizacion.Cic);

                TablaPar.FecDev = fechaDevengue;
                TablaPar.DevSol = fechaDevengueSol;
                TablaPar.TipSex = cotizacion.ClaveSexo == null ? cotizacionExcepciones.sexo : cotizacion.ClaveSexo;
                TablaPar.FecNac = cotizacion.FechaNacimientoStr == null ? cotizacionExcepciones.fechaNac : cotizacion.FechaNacimientoStr;

                //obtiene EL GASTO DE SEPELIO DEL MES
                beDatosModalidad VarMTGS = new beDatosModalidad();
                // VarMTGS = _rutinaOficialesRepository.ConsultaGastoSepelioMes(FecCal);
                VarMTGS = (from mt in VarMTGSPar where Convert.ToInt32(mt.FEC_INICUOMOR) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(mt.FEC_TERCUOMOR) select mt).FirstOrDefault();
                TablaPar.MtoGS = VarMTGS.MtoGS;

                //OBTIENE LA AFP RENTAB
                beDatosModalidad VarAFP = new beDatosModalidad();
                // VarAFP = _rutinaOficialesRepository.ConsultaRentabilidadAfp(cotizacion.Afp);
                VarAFP = (from afp in VarAFPPar where afp.COD_ELEMENTO == (cotizacion.Afp == null ? cotizacionExcepciones.afp : cotizacion.Afp) select afp).FirstOrDefault();
                TablaPar.RenAfp = VarAFP.RenAfp;

                //

                TablaPar.NumCor = item.IdModalidad;
                TablaPar.PrcTas = 0;
                TablaPar.PrcCom = _ExcepcionesRepository == null ? Convert.ToDouble(item.ValorComision) : _ExcepcionesRepository.nuevaCO;
                TablaPar.ComisionInicial = item.ComisionInicial;
                TablaPar.CodMon = item.ClaveMoneda;
                TablaPar.DerCre = "N";
                TablaPar.DerGra = item.DerGra;
                if (TipPen == "04" || TipPen == "05")
                {
                    TablaPar.IndCob = "N";
                }
                else
                {
                    TablaPar.IndCob = cotizacion.Ind_Cob == null ? cotizacionExcepciones.Ind_Cob : cotizacion.Ind_Cob;
                }
                //obtiene la region de la tasa
                beDatosModalidad VarREG = new beDatosModalidad();
                // VarREG = _rutinaOficialesRepository.ConsultaRegionTasas(Convert.ToDouble(cotizacion.Cic));
                VarREG = (from reg in VarREGPar
                          where reg.MTO_MINIMO <= (cotizacion.Cic == 0 ? Convert.ToDecimal(cotizacionExcepciones.cic) : Convert.ToDecimal(cotizacion.Cic)) &&
                            (cotizacion.Cic == 0 ? Convert.ToDecimal(cotizacionExcepciones.cic) : Convert.ToDecimal(cotizacion.Cic)) <= reg.MTO_MAXIMO &&
                            (cotizacion.Cod_region == null ? cotizacionExcepciones.Cod_region : (cotizacion.Cod_region)) == reg.CodReg_Asoc
                          select reg).FirstOrDefault();


                if (VarREG == null)
                {
                    TablaPar.CodReg = "0";
                    TablaPar.CodRechazo = "999";
                    TablaPar.MensajeErr = "Codigo Región no encontrado.";
                }
                else
                {
                    TablaPar.CodReg = VarREG.CodReg;
                    TablaPar.CodRechazo = "0";
                }


                //
                TablaPar.RegEst = "14";
                TablaPar.TipRea = item.CodigoTipoReajuste;
                TablaPar.PrcMen = 0.16515813; //Pendiente (Se envían los datos fijos o dinámicos?)
                TablaPar.PrcTri = 0.49629316; //Pendiente (Se envían los datos fijos o dinámicos?)
                TablaPar.PrcAnu = 2; //Pendiente (Se envían los datos fijos o dinámicos?)

                string diaDeVS = fechaDevengueSol.Substring(6, 2);
                string mesDeVS = fechaDevengueSol.Substring(4, 2);
                string anioDeVS = fechaDevengueSol.Substring(0, 4);
                string fechaDeVS = anioDeVS + "/" + mesDeVS + "/" + diaDeVS;

                TablaPar.EdaLim = (cotizacion.Fec_DevSolStr == null ? Convert.ToDateTime(fechaDeVS) : Convert.ToDateTime(fechaDeVS)) < Convert.ToDateTime("2013/08/01") ? 216 : 336;
                TablaPar.MinRC = 0;
                TablaPar.RepRC = 0;
                TablaPar.ValCam = cotizacion.TipoCambio == 0 ? cotizacionExcepciones.codTipCambio : Convert.ToDouble(cotizacion.TipoCambio); //Pendiente 3.274 (Se envían los datos fijos o dinámicos?)
                #region validatasasInd
                if (_ExcepcionesRepository != null)
                {
                    #region Tablas Indicadores para Mejoras
                    if (_ExcepcionesRepository.caso == "mejoras")
                    {
                        List<beDatosTasasPar> ListaTas2 = new List<beDatosTasasPar>();

                        if (_ExcepcionesRepository.parametro == "Mej" || _ExcepcionesRepository.parametro == "")
                        {
                            ListaTas2 = _rutinaOficialesRepository.ConsultaGastosTasasIndMej(FecCal, TipPen);

                          // ListaTas2 = _rutinaOficialesRepository.ConsultaGastosTasasIndMej(FecCal, TipPen);

                        List<beDatosTasasPar> comisionErrorl = (from l in ListaTas2 where (l.CodMon == TablaPar.CodMon && l.TipPen == TablaPar.Tippen && l.CodReg == TablaPar.CodReg && l.TipRea == TablaPar.TipRea) select l).ToList();

                        if (bandera == "" || bandera == "WS")
                        {

                            int tasaError = (from t in comisionErrorl where t.PrcTas < Tasavta select t).Count();


                            //int tasaError = (from t in ListaTas2 where t.PrcTas < Tasavta select t).Count();
                            if (tasaError > 0)
                            {
                                List<beResultados> erroresRutina = new List<beResultados>();
                                beResultados errorRutina = new beResultados();

                                errorRutina.Mensaje = "Ingreso una tasa superior (Tasa Máxima: " + comisionErrorl[0].PrcTas + ")";
                                erroresRutina.Add(errorRutina);

                                return erroresRutina;
                            }
                        }
                        double nuecomi = 0;
                       //nuecomi = ((Comi + 0.2) * 1.42);
                        nuecomi = Math.Truncate(100 * (((Comi + 0.2) * 1.42))) / 100;


                            if (TablaPar.PrcCom != Convert.ToDouble(2.4))
                            {
                                int comisionError = (from l in comisionErrorl where l.ComMin > nuecomi || nuecomi > l.ComMax select l).Count();

                                if (comisionError > 0)
                                {
                                    List<beResultados> erroresRutina = new List<beResultados>();
                                    beResultados errorRutina = new beResultados();
                                    double comin = ((comisionErrorl[0].ComMin) / 1.42) - 0.2;
                                    double comax = ((comisionErrorl[0].ComMax) / 1.42) - 0.2;

                                    errorRutina.Mensaje = "Ingreso una comision invalida (Mínima: " + comin.ToString("N2") + " Máxima: " + comax.ToString("N2") + ")";
                                    erroresRutina.Add(errorRutina);

                                    return erroresRutina;
                                }
                            }
                        }
                        else
                        {
                            ListaTas2 = _rutinaOficialesRepository.ConsultaGastosTasasIndMejo(FecCal, TipPen);

                            //ListaTas2 = _rutinaOficialesRepository.ConsultaGastosTasasIndMej(FecCal, TipPen);

                            List<beDatosTasasPar> comisionErrorl = (from l in ListaTas2 where (l.CodMon == TablaPar.CodMon && l.TipPen == TablaPar.Tippen && l.CodReg == TablaPar.CodReg && l.TipRea == TablaPar.TipRea) select l).ToList();

                            if (bandera == "" || bandera == "WS")
                            {

                                int tasaError = (from t in comisionErrorl where t.PrcTas <= Tasavta select t).Count();


                                //int tasaError = (from t in ListaTas2 where t.PrcTas < Tasavta select t).Count();
                                if (tasaError > 0)
                                {
                                    List<beResultados> erroresRutina = new List<beResultados>();
                                    beResultados errorRutina = new beResultados();

                                    errorRutina.Mensaje = "Ingreso una tasa superior (Tasa Máxima: " + comisionErrorl[0].PrcTas + ")";
                                    erroresRutina.Add(errorRutina);

                                    return erroresRutina;
                                }
                            }
                            double nuecomi = 0;
                            nuecomi = Math.Truncate(100 * (((Comi + 0.2) * 1.42))) / 100;

                            if (TablaPar.PrcCom != Convert.ToDouble(2.4))
                            {
                                int comisionError = (from l in comisionErrorl where l.ComMin > nuecomi || nuecomi > l.ComMax select l).Count();

                                if (comisionError > 0)
                                {
                                    List<beResultados> erroresRutina = new List<beResultados>();
                                    beResultados errorRutina = new beResultados();
                                    double comin = ((comisionErrorl[0].ComMin) / 1.42) - 0.2;
                                    double comax = ((comisionErrorl[0].ComMax) / 1.42) - 0.2;

                                    errorRutina.Mensaje = "Ingreso una comision invalida (Mínima: " + comin.ToString("N2") + " Máxima: " + comax.ToString("N2") + ")";
                                    erroresRutina.Add(errorRutina);

                                    return erroresRutina;
                                }
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                TablaPar.PrccomS = item.PrccomS;
                TablaPar.Prcfaclab = item.Prcfaclab;
                if (_ExcepcionesRepository != null)
                {
                    if (_ExcepcionesRepository.caso == "excepciones" || 
                        (_ExcepcionesRepository.caso == "mejoras" && _ExcepcionesRepository.nuevaCO < Convert.ToDouble(TablaPar.ComisionInicial))
                        || (_ExcepcionesRepository.nuevaCO != Convert.ToDouble(TablaPar.ComisionInicial)))
                    {
                        TablaPar.Prc_Inicial_1 = Convert.ToDecimal(Comi);
                        TablaPar.Prc_Inicial = Convert.ToDecimal(Comi);
                    }
                    else
                    {
                        TablaPar.Prc_Inicial_1 = TablaPar.ComisionInicial;//item.Prc_Inicial_1;
                        TablaPar.Prc_Inicial = TablaPar.ComisionInicial;// item.Prc_Inicial;
                    }
                }
                else
                {
                    TablaPar.Prc_Inicial_1 = item.Prc_Inicial_1;
                    TablaPar.Prc_Inicial = item.Prc_Inicial;
                }
                TablaPar.Prc_Minimo = item.Prc_Minimo;
                TablaPar.Prc_Minimo_1 = item.Prc_Minimo_1;
                TablaPar.BanderaComision = _ExcepcionesRepository == null ? "" : "ME";
                
                ListaPar.Add(TablaPar);

                ListaModalidades.Add(ListaPar);
            }

            #endregion

            #region Carga Beneficiario

            List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();
            //LisTabPL = _rutinaOficialesRepository.ConsultaPorcentaje(FecCal);
            LisTabPL = (from por in LisTabPLPar where Convert.ToInt32(por.fec_inivigpor) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(por.fec_tervigpor) select por).ToList();
            
            List<beDatosBen> ListaBen = new List<beDatosBen>();
            ListaBen = ListaBeneficiarios(idSolicitud, FecCal, TipPen, LisTabPL, NumArch.ToString(), fechaDevengueSol, cobertura, fechaDevengue);

            #endregion

            #region Carga Gastos Tasas Indicadores

            double TirIni = 0;
            Response res = new Response();
            if (_ExcepcionesRepository != null)
            {
                #region Excepciones
                if (_ExcepcionesRepository.caso == "excepciones")
                {

                    ListaTas = _rutinaOficialesRepository.ConsultaGastosTasasIndEx(FecCal, TipPen, Tasavta);
                }
                #endregion
                #region Mejoras
                else if (_ExcepcionesRepository.caso == "mejoras")
                {
                    if (bandera == "ant")
                    {
                        double comision = _ExcepcionesRepository.nuevaCO;

                        
                        if (_ExcepcionesRepository.parametro == "Mej" || _ExcepcionesRepository.parametro == "")
                        {
                           
                            Tasavta = 0;
                            ListaTas = _rutinaOficialesRepository.ConsultaGastosTasasIndMejo(FecCal, TipPen, Tasavta);

                            string queryCon = "SELECT PRC_TIRINI FROM PT_TMAE_DETCOTIZACION where num_operacion = " + idSolicitud + "AND NUM_CORRELATIVO = " + _ExcepcionesRepository.numCorrelativo;

                            double DatosCon = SRVDBContext<beDatosTasasPar>.CallSelectStatement(queryCon, x => new beDatosTasasPar
                            {
                                PrcTir = (double)x.GetDecimal(0)
                            }).FirstOrDefault().PrcTir;
                            TirIni = DatosCon;
                        }
                        else
                        {
                            ListaTas = _rutinaOficialesRepository.ConsultaGastosTasasIndMejo(FecCal, TipPen);
                            string queryCon = "SELECT PRC_TIRINI FROM PT_TMAE_DETCOTIZACION where num_operacion = " + idSolicitud + "AND NUM_CORRELATIVO = " + _ExcepcionesRepository.numCorrelativo;

                            double DatosCon = SRVDBContext<beDatosTasasPar>.CallSelectStatement(queryCon, x => new beDatosTasasPar
                            {
                                PrcTir = (double)x.GetDecimal(0)
                            }).FirstOrDefault().PrcTir;
                            TirIni = DatosCon;
                        }
                    }
                    else
                    {
                        double comision = _ExcepcionesRepository.nuevaCO;
                        if (_ExcepcionesRepository.parametro == "Mej" || _ExcepcionesRepository.parametro == "")
                        {
                            ListaTas = _rutinaOficialesRepository.ConsultaGastosTasasIndMejo(FecCal, TipPen, Tasavta);
                            string queryCon = "SELECT PRC_TIRINI FROM PT_TMAE_DETCOTIZACION where num_operacion = " + idSolicitud + "AND NUM_CORRELATIVO = " + _ExcepcionesRepository.numCorrelativo;

                            double DatosCon = SRVDBContext<beDatosTasasPar>.CallSelectStatement(queryCon, x => new beDatosTasasPar
                            {
                                PrcTir = (double)x.GetDecimal(0)
                            }).FirstOrDefault().PrcTir;
                            TirIni = DatosCon;
                        }
                        else
                        {
                            ListaTas = _rutinaOficialesRepository.ConsultaGastosTasasIndM(FecCal, TipPen, Tasavta);
                            string queryCon = "SELECT PRC_TIRINI FROM PT_TMAE_DETCOTIZACION where num_operacion = " + idSolicitud + "AND NUM_CORRELATIVO = " + _ExcepcionesRepository.numCorrelativo;

                            double DatosCon = SRVDBContext<beDatosTasasPar>.CallSelectStatement(queryCon, x => new beDatosTasasPar
                            {
                                PrcTir = (double)x.GetDecimal(0)
                            }).FirstOrDefault().PrcTir;
                            TirIni = DatosCon;
                        }
                    }
                }
                #endregion
            }
            else
            {
                ListaTas = _rutinaOficialesRepository.ConsultaGastosTasasInd(FecCal, TipPen);
            }
            #endregion

            #region Carga Tasa Mercado

            string Mes = FecCal.Substring(4, 2).Replace("0", "").Trim();

            string query = "SELECT COD_MONEDA, convert(numeric,COD_TIPREAJUSTE) COD_TIPREAJUSTE, PRC_MES" + Mes +
                " AS PRC_MES FROM MA_TVAL_TASATM WHERE NUM_ANNO=" + FecCal.Substring(0, 4);

            List<beTasaMercado> ListaTM = new List<beTasaMercado>();
            ListaTM = _rutinaOficialesRepository.ConsultaTasaMercado(query);

            #endregion

            #region Carga Tasa Anclaje

            List<beTasaAnclaje> ListaTA = new List<beTasaAnclaje>();
            // ListaTA = _rutinaOficialesRepository.ConsultaTasaAnclaje(FecCal);
            ListaTA = (from ta in ListaTAPar where Convert.ToInt32(ta.fec_inivig) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(ta.fec_tervig) select ta).ToList();
            #endregion

            #region Carga Factor VAC
            List<beTasaFacVac> ListaVac = new List<beTasaFacVac>();
            //se debe crear la tabla de Factores Vacpara indexados
            ListaVac = _rutinaOficialesRepository.ConsultaFactorVac();
            #endregion

            #region Carga CPK's

            List<beCPK> ListaCPK = new List<beCPK>();
            // ListaCPK = _rutinaOficialesRepository.ConsultaCPKS(FecCal);
            ListaCPK = (from cpk in ListaCPKPar where Convert.ToInt32(cpk.FEC_INIVIG) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(cpk.FEC_TERVIG) select cpk).ToList();
            #endregion

            #region Carga Rentabilidad

            List<beRentabilidad> ListaRen = new List<beRentabilidad>();
            //ListaRen = _rutinaOficialesRepository.ConsultaRentabilidad(FecCal);
            ListaRen = (from ren in ListaRenPar where Convert.ToInt32(ren.FEC_INIVIG) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(ren.FEC_TERVIG) select ren).ToList();
            #endregion

            #region Tasas Promedio 

            List<beTasasPromedio> ListaTasProm = new List<beTasasPromedio>();
            string FecCalMod = FecCal.Substring(0, 6) + "01";
            //ListaTasProm = _rutinaOficialesRepository.ConsultaTasasPromedio(FecCalMod, TipPen);
            ListaTasProm = (from tasprom in ListaTasPromPar where tasprom.COD_TIPPENSION == TipPen select tasprom).ToList();
            #endregion

            #region Curva Tasas

            List<beCurvaTasas> ListaCurvaTasas = new List<beCurvaTasas>();
            //ListaCurvaTasas = _rutinaOficialesRepository.ConsultaCurvaTasas(FecCal);
            ListaCurvaTasas = (from cutas in ListaCurvaTasasPar where Convert.ToInt32(cutas.FEC_INIVIG) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(cutas.FEC_TERVIG) select cutas).ToList();
            #endregion

            #region Tareas 
            List<Task> tareas = new List<Task>();
            foreach (var item in ListaModalidades)
            {
                string caso = "";
                double nuevaCO = 0;
                double ComisionInicial = 0;
                if (_ExcepcionesRepository != null)
                {
                   caso = _ExcepcionesRepository.caso;
                    nuevaCO = _ExcepcionesRepository.nuevaCO;
                }
                var task = ResultadoCotMetodo(item, ListaBen, ListaTas, ListaTM, ListaTA, 
                                            ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, 
                                            ListaCurvaTasas, FecCal, caso, TirIni, nuevaCO, NumArch, item[0].CodMon, item[0].TipRea, item[0].ComisionInicial, asesor, bandera);
                tareas.Add(task);
                GC.Collect();
                //GC.WaitForPendingFinalizers();
                Console.WriteLine("Memory used after full collection:   {0:N0}",
                GC.GetTotalMemory(true));
            }
            while (tareas.Count > 0)
            {
                Task firstFinishedTask = await Task.WhenAny(tareas);
                tareas.Remove(firstFinishedTask);
                firstFinishedTask.Dispose();
            }

            //await Task.WhenAll(tareas);

            return ResultadoCotGlobal;
        }
        #endregion

        async Task ResultadoCotMetodo(List<beDatosModalidad> ListaModalidades, List<beDatosBen> ListaBen, List<beDatosTasasPar> ListaTas, List<beTasaMercado> ListaTM, List<beTasaAnclaje> ListaTA, 
            List<beMortalidadDinVal> ListaMor, List<beCPK> ListaCPK, List<beRentabilidad> ListaRen, List<beTasaFacVac> ListaVac, List<bePorcenLegales> LisTabPL, 
            List<beTasasPromedio> ListaTasProm, List<beCurvaTasas> ListaCurvaTasas, string FecCal, string casoMej, double tirInicial, double Comision, int NumArch, string codMoneda, int tiporeaj, decimal ComisionInicial, int asesor, string banderaws)
        {
            await Task.Factory.StartNew(() =>
            {
                RutinaActOficiales RActuarial = new RutinaActOficiales();
                RutinaActMej RActuarialMej = new RutinaActMej();
                beResultados rutina = new beResultados();
                var operacion = ListaModalidades[0].NumCot;
                var correlativo = Convert.ToInt32(ListaModalidades[0].NumCor);
                int hijo;
                int madre;
                try
                {
                    if (ListaBen[ListaBen.Count - 1].Mensaje == null)
                    {
                        hijo = (from b in ListaBen where b.CodPar == "30" select b).Count();

                        if (hijo != 0 && ListaBen[1].CodPar == "10")
                        {
                            ListaBen[1].CodPar = "11";
                        }
                        #region Rutina Mejoradas
                        if (casoMej == "mejoras" && Comision > Convert.ToDouble(ComisionInicial))
                        {
                            #region Mejoradas primer Flujo
                            if (codMoneda == "NS" && tiporeaj == 1)
                            {
                                rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Comision, tirInicial, 5, 0);
                                if (rutina.PRC_TASAVTA == 5)
                                {
                                    rutina.PRC_TASAVTA = 0.5;
                                }
                            }
                            else if (codMoneda == "NS" && tiporeaj == 2)
                            {
                                rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Comision, tirInicial, 10, 0);
                                if (rutina.PRC_TASAVTA == 10)
                                {
                                    rutina.PRC_TASAVTA = 0.5;
                                }
                            }
                            else
                            {
                                rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Comision, tirInicial, 7, 0);
                                if (rutina.PRC_TASAVTA == 7)
                                {
                                    rutina.PRC_TASAVTA = 0.5;
                                }
                            }
                            #endregion

                            #region Comparacion Mejoradas SISCO
                            string querySisco = "";
                            querySisco = "SELECT CASE WHEN IND_SISCO = 1 THEN DC.MTO_SUMPENSION ELSE dc.MTO_PENSION END, dc.ind_sisco, c.cod_tippension, c.ind_cob FROM PT_TMAE_DETCOTIZACION DC" +
                            " JOIN PT_TMAE_COTIZACION C ON C.NUM_OPERACION = DC.NUM_OPERACION"+
                            " WHERE DC.NUM_OPERACION = " + rutina.NUM_COTESTUDIO + " and DC.num_correlativo = " + rutina.NUM_CORRELATIVO + " and DC.IND_FILTROCOTIZA = 'S'";

                            List<SolicitudesCotizacion> sis = new List<SolicitudesCotizacion>();
                            sis = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(querySisco, x => new SolicitudesCotizacion
                            {
                                pensionSis = (double)x.GetDecimal(0),
                                 Ind_Sis = x.GetInt32(1),
                                 strTipPen = x.GetString(2),
                                 strCobCon = x.GetString(3)
                            }).ToList();
                            int valsisco = sis[0].Ind_Sis;
                            double mtopensis = sis[0].pensionSis;
                            string tippension = sis[0].strTipPen;
                            string cobertura = sis[0].strCobCon;
                            rutina.IND_SISCO = valsisco;
                            if (valsisco == 1)
                            {
                                #region Validación pension Primer Flujo
                                var listBeneficiarios = _calculoCotizacionRepository.getBeneficiariosRutina(Convert.ToInt32(rutina.NUM_COTESTUDIO));
                                double suma = 0;
                                double pensionrutina = rutina.MTO_PENSION;
                                switch (tippension)
                                {
                                    case "08":
                                        for (int i = 0; i < listBeneficiarios.Count; i++)
                                        {

                                            if (listBeneficiarios[i].Num_Corr == rutina.NUM_CORRELATIVO && listBeneficiarios[i].Num_Operacion == Convert.ToInt32(rutina.NUM_COTESTUDIO))
                                            {
                                                suma = suma + pensionrutina * Convert.ToDouble((listBeneficiarios[i].prc_Pension / 100));
                                                pensionrutina = suma;
                                            }
                                        }
                                        break;
                                    case "07":
                                        if (cobertura == "S")
                                            pensionrutina = (Convert.ToDouble(pensionrutina) * (0.5));
                                        else
                                            pensionrutina = Convert.ToDouble(pensionrutina);
                                        break;
                                    case "06":
                                        if (cobertura == "S")
                                            pensionrutina = (Convert.ToDouble(pensionrutina) * (0.7));
                                        else
                                            pensionrutina = Convert.ToDouble(pensionrutina);
                                        break;
                                    case "05":
                                        pensionrutina = Convert.ToDouble(pensionrutina);
                                        break;
                                    case "04":
                                        pensionrutina = Convert.ToDouble(pensionrutina);
                                        break;
                                }
                                #endregion

                                #region Validacion pension

                                if (mtopensis > pensionrutina)
                                {
                                    #region Validacion Comision mayor 

                                    if (Comision > Convert.ToDouble(ComisionInicial))
                                    {
                                    if (codMoneda == "NS" && tiporeaj == 1)
                                    {
                                        rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Comision, tirInicial, 5, mtopensis);
                                        if (rutina.PRC_TASAVTA == 5)
                                        {
                                            rutina.PRC_TASAVTA = 0.5;
                                        }
                                        rutina.MTO_PENSION = mtopensis;
                                    }
                                    else if (codMoneda == "NS" && tiporeaj == 2)
                                    {
                                        rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Comision, tirInicial, 10, mtopensis);
                                        if (rutina.PRC_TASAVTA == 10)
                                        {
                                            rutina.PRC_TASAVTA = 0.5;
                                        }
                                        rutina.MTO_PENSION = mtopensis;
                                    }
                                    else {
                                        rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Comision, tirInicial, 7, mtopensis);
                                            if (rutina.PRC_TASAVTA == 7)
                                            {
                                                rutina.PRC_TASAVTA = 0.5;
                                            }
                                            rutina.MTO_PENSION = mtopensis;
                                        }
                                    }
                                    #endregion
                                    else
                                    {
                                        if (codMoneda == "NS" && tiporeaj == 1)
                                        {
                                            rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Comision, 0.00001, 5, mtopensis);
                                            if (rutina.PRC_TASAVTA == 5)
                                            {
                                                rutina.PRC_TASAVTA = 0.5;
                                            }
                                            rutina.MTO_PENSION = mtopensis;
                                        }
                                        else if (codMoneda == "NS" && tiporeaj == 2)
                                        {
                                            rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Comision, 0.00001, 10, mtopensis);
                                            if (rutina.PRC_TASAVTA == 10)
                                            {
                                                rutina.PRC_TASAVTA = 0.5;
                                            }

                                            rutina.MTO_PENSION = mtopensis;
                                        }
                                        else
                                        {
                                            rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Comision, 0.00001, 7, mtopensis);
                                            if (rutina.PRC_TASAVTA == 7)
                                            {
                                                rutina.PRC_TASAVTA = 0.5;
                                            }
                                            rutina.MTO_PENSION = mtopensis;
                                        }
                                    }
                                }
                                #endregion

                            }
                            rutina.IND_SISCO = valsisco;
                            #endregion
                        }
                        #endregion

                        #region Rutina Flujo Normal
                        else
                        {
                            rutina.MTO_PENSION = 0;
                            List<beDatosBen> ListaBen2 = new List<beDatosBen>();
                            ListaBen2 = ListaBen_tmp;
                            ListaBen = ListaBen2;
                            rutina = RActuarial.RutinaPension(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, null,asesor, banderaws, casoMej);
                            if (rutina.MTO_PENSION == 0)
                            { string mje = "";
                                mje = rutina.Mensaje; }
                            #region Excel
                        //if ((ListaModalidades[0].NumCor == 4) && (rutina.MTO_PENSION != 505.04062181086226))
                        //{
                        //    // Crear un objeto SqlConnection, y luego pasar la ConnectionString al constructor.            
                        //    //SqlConnection Conection = new SqlConnection(CadenaString);

                        //    //// Utilizar una variable para almacenar la instrucción SQL.
                        //    //string SelectString = "SELECT id_pregunta, descripcion, opcion, valor_respuesta, COUNT(id_estudiante) AS Numero_Votos FROM respuestas_encuentas, preguntas WHERE respuestas_encuentas.id_pregunta = preguntas.id GROUP BY id_pregunta, opcion, descripcion, valor_respuesta";

                        //    //SqlDataAdapter Adaptador = new SqlDataAdapter(SelectString, Conection);

                        //    //DataSet DS = new DataSet();

                        //    //// Abrir la conexión.
                        //    //Conection.Open();
                        //    //Adaptador.Fill(DS);
                        //    //Conection.Close();

                        //    // Creamos un objeto Excel.
                        //    Excel.Application Mi_Excel = default(Excel.Application);
                        //    // Creamos un objeto WorkBook. Para crear el documento Excel.           
                        //    Excel.Workbook LibroExcel = default(Excel.Workbook);
                        //    // Creamos un objeto WorkSheet. Para crear la hoja del documento.
                        //    Excel.Worksheet HojaExcel = default(Excel.Worksheet);

                        //    // Iniciamos una instancia a Excel, y Hacemos visibles para ver como se va creando el reporte, 
                        //    // podemos hacerlo visible al final si se desea.
                        //    Mi_Excel = new Excel.Application();
                        //    Mi_Excel.Visible = true;

                        //    /* Ahora creamos un nuevo documento y seleccionamos la primera hoja del 
                        //     * documento en la cual crearemos nuestro informe. 
                        //     */
                        //    // Creamos una instancia del Workbooks de excel.            
                        //    LibroExcel = Mi_Excel.Workbooks.Add();
                        //    // Creamos una instancia de la primera hoja de trabajo de excel            
                        //    HojaExcel = LibroExcel.Worksheets[1];
                        //    HojaExcel.Visible = Excel.XlSheetVisibility.xlSheetVisible;


                        //    int i = 1;
                        //    foreach (var item in ListaModalidades)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "A"] = item.NumCot; // NumCot
                        //        // ID
                        //        HojaExcel.Cells[i, "B"] = item.FinTab; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "C"] = item.Tippen;
                        //        // Opciones
                        //        HojaExcel.Cells[i, "D"] = item.TipRen;
                        //        // Valor de la Respuesta
                        //        HojaExcel.Cells[i, "E"] = item.TipMod;
                        //        HojaExcel.Cells[i, "F"] = item.MesGar;
                        //        HojaExcel.Cells[i, "G"] = item.FecCot;
                        //        HojaExcel.Cells[i, "H"] = item.MtoPri;
                        //        HojaExcel.Cells[i, "I"] = item.MesDif;
                        //        HojaExcel.Cells[i, "J"] = item.FecDev;
                        //        HojaExcel.Cells[i, "K"] = item.DevSol;
                        //        HojaExcel.Cells[i, "L"] = item.TipSex;
                        //        HojaExcel.Cells[i, "M"] = item.FecNac;
                        //        HojaExcel.Cells[i, "N"] = item.MtoGS;
                        //        HojaExcel.Cells[i, "O"] = item.RenAfp;
                        //        HojaExcel.Cells[i, "P"] = item.RenTmp;
                        //        HojaExcel.Cells[i, "Q"] = item.NumCor;
                        //        HojaExcel.Cells[i, "R"] = item.PrcTas;
                        //        HojaExcel.Cells[i, "S"] = item.PrcCom;
                        //        HojaExcel.Cells[i, "T"] = item.CodMon;
                        //        HojaExcel.Cells[i, "U"] = item.DerCre;
                        //        HojaExcel.Cells[i, "V"] = item.DerGra;
                        //        HojaExcel.Cells[i, "W"] = item.IndCob;
                        //        HojaExcel.Cells[i, "X"] = item.CodReg;
                        //        HojaExcel.Cells[i, "Y"] = item.CodRechazo;
                        //        HojaExcel.Cells[i, "Z"] = item.RegEst;
                        //        HojaExcel.Cells[i, "AA"] = item.TipRea;
                        //        HojaExcel.Cells[i, "AB"] = item.PrcMen;
                        //        HojaExcel.Cells[i, "AC"] = item.PrcTri;
                        //        HojaExcel.Cells[i, "AD"] = item.PrcAnu;
                        //        HojaExcel.Cells[i, "AE"] = item.EdaLim;
                        //        HojaExcel.Cells[i, "AF"] = item.MinRC;
                        //        HojaExcel.Cells[i, "AG"] = item.RepRC;
                        //        HojaExcel.Cells[i, "AH"] = item.ValCam;
                        //        HojaExcel.Cells[i, "AI"] = item.FEC_INICUOMOR;
                        //        HojaExcel.Cells[i, "AJ"] = item.FEC_TERCUOMOR;
                        //        HojaExcel.Cells[i, "AK"] = item.COD_ELEMENTO;
                        //        HojaExcel.Cells[i, "AL"] = item.MTO_MINIMO;
                        //        HojaExcel.Cells[i, "AM"] = item.MTO_MAXIMO;
                        //        HojaExcel.Cells[i, "AN"] = item.MensajeErr;
                        //        HojaExcel.Cells[i, "AO"] = item.ComisionInicial;
                        //        // Numero Votos

                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //    i = 1;
                        //    foreach (var item in ListaBen)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "AQ"] = item.NumOrd;
                        //        // ID
                        //        HojaExcel.Cells[i, "AR"] = item.CodPar; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "AS"] = item.FecNac;
                        //        // Opciones
                        //        HojaExcel.Cells[i, "AT"] = item.GruFam;
                        //        // Valor de la Respuesta
                        //        HojaExcel.Cells[i, "AU"] = item.TipSex;
                        //        HojaExcel.Cells[i, "AV"] = item.TipInv;
                        //        HojaExcel.Cells[i, "AW"] = item.FecInv;
                        //        HojaExcel.Cells[i, "AX"] = item.DerPen;
                        //        HojaExcel.Cells[i, "AY"] = item.PrcPen;
                        //        HojaExcel.Cells[i, "AZ"] = item.PrcLeg;
                        //        HojaExcel.Cells[i, "BA"] = item.PrcGar;
                        //        HojaExcel.Cells[i, "BB"] = item.NacHM;
                        //        HojaExcel.Cells[i, "BC"] = item.FacFal;
                        //        HojaExcel.Cells[i, "BD"] = item.TipSex;
                        //        HojaExcel.Cells[i, "BE"] = item.derCre;
                        //        HojaExcel.Cells[i, "BF"] = item.Mensaje;

                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }


                        //    i = 1;
                        //    foreach (var item in ListaTas)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "BH"] = item.CodMon;
                        //        // ID
                        //        HojaExcel.Cells[i, "BI"] = item.TipPen; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "BJ"] = item.CodReg;
                        //        // Opciones
                        //        HojaExcel.Cells[i, "BK"] = item.PriMin;
                        //        // Valor de la Respuesta
                        //        HojaExcel.Cells[i, "BL"] = item.PriMax;
                        //        HojaExcel.Cells[i, "BM"] = item.MtoGad;
                        //        HojaExcel.Cells[i, "BN"] = item.PrcDeu;
                        //        HojaExcel.Cells[i, "BO"] = item.MtoImp;
                        //        HojaExcel.Cells[i, "BP"] = item.MtoGem;
                        //        HojaExcel.Cells[i, "BQ"] = item.PrcTir;
                        //        HojaExcel.Cells[i, "BR"] = item.PrcTas;
                        //        HojaExcel.Cells[i, "BS"] = item.PrcPer;
                        //        HojaExcel.Cells[i, "BT"] = item.TipRea;
                        //        HojaExcel.Cells[i, "BU"] = item.ComMax;
                        //        HojaExcel.Cells[i, "BV"] = item.ComMin;
                        //        HojaExcel.Cells[i, "BW"] = item.TasaU;

                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //    i = 1;
                        //    foreach (var item in ListaTM)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "CA"] = item.CodMon;
                        //        // ID
                        //        HojaExcel.Cells[i, "CB"] = item.TipRea; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "CC"] = item.PrcVal;

                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //    i = 1;
                        //    foreach (var item in ListaTA)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "CE"] = item.CodMon;
                        //        // ID
                        //        HojaExcel.Cells[i, "CF"] = item.TipRea; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "CG"] = item.PrcVal;
                        //        HojaExcel.Cells[i, "CH"] = item.fec_inivig;
                        //        HojaExcel.Cells[i, "CI"] = item.fec_tervig;


                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //    i = 1;
                        //    foreach (var item in ListaMor)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "CK"] = item.Cor;
                        //        // ID
                        //        HojaExcel.Cells[i, "CL"] = item.i; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "CM"] = item.j;
                        //        // Opciones
                        //        HojaExcel.Cells[i, "CN"] = item.k;
                        //        // Valor de la Respuesta
                        //        HojaExcel.Cells[i, "CO"] = item.MtoLx;
                        //        HojaExcel.Cells[i, "CP"] = item.MtoAx;

                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //    i = 1;
                        //    foreach (var item in ListaCPK)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "CR"] = item.COD_MONEDA;
                        //        // ID
                        //        HojaExcel.Cells[i, "CS"] = item.COD_TIPREAJUSTE; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "CT"] = item.NUM_ANNO;
                        //        // Opciones
                        //        HojaExcel.Cells[i, "CU"] = item.PRC_CPK;
                        //        // Valor de la Respuesta
                        //        HojaExcel.Cells[i, "CV"] = item.FEC_INIVIG;
                        //        HojaExcel.Cells[i, "CW"] = item.FEC_TERVIG;

                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //    i = 1;
                        //    foreach (var item in ListaRen)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "DA"] = item.COD_MONEDA;
                        //        // ID
                        //        HojaExcel.Cells[i, "DB"] = item.COD_TIPREAJUSTE; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "DC"] = item.NUM_ANNO;
                        //        // Opciones
                        //        HojaExcel.Cells[i, "DD"] = item.PRC_TASAREN;
                        //        // Valor de la Respuesta
                        //        HojaExcel.Cells[i, "DF"] = item.Cod_Scomp;
                        //        HojaExcel.Cells[i, "DG"] = item.Gls_Descripcion;
                        //        HojaExcel.Cells[i, "DH"] = item.FEC_INIVIG;
                        //        HojaExcel.Cells[i, "DI"] = item.FEC_TERVIG;
                        //        HojaExcel.Cells[i, "DJ"] = item.Periodo;
                        //        HojaExcel.Cells[i, "DK"] = item.IDPeriodo;
                        //        HojaExcel.Cells[i, "DL"] = item.Usuario;

                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //    i = 1;
                        //    foreach (var item in ListaVac)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "DN"] = item.FEC_INICUOMOR;
                        //        // ID
                        //        HojaExcel.Cells[i, "DO"] = item.FEC_TERCUOMOR; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "DP"] = item.MTO_FACTOR;
                        //        // Opciones
                        //        HojaExcel.Cells[i, "DQ"] = item.FEC_IPC;
                        //        // Valor de la Respuesta
                        //        HojaExcel.Cells[i, "DR"] = item.PRC_IPC;

                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //    i = 1;
                        //    foreach (var item in LisTabPL)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "DT"] = item.COD_PAR;
                        //        // ID
                        //        HojaExcel.Cells[i, "DU"] = item.COD_SITINV; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "DV"] = item.COD_SEXO;
                        //        // Opciones
                        //        HojaExcel.Cells[i, "DW"] = item.PRC_PENSION;
                        //        // Valor de la Respuesta
                        //        HojaExcel.Cells[i, "DX"] = item.fec_inivigpor;
                        //        HojaExcel.Cells[i, "DY"] = item.fec_tervigpor;

                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //    i = 1;
                        //    foreach (var item in ListaTasProm)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "EA"] = item.COD_MONEDA;
                        //        // ID
                        //        HojaExcel.Cells[i, "EB"] = item.COD_TIPPREAJUSTE; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "EC"] = item.MTO_VTAPROM;
                        //        // Opciones
                        //        HojaExcel.Cells[i, "ED"] = item.FEC_VTAPROM;
                        //        // Valor de la Respuesta
                        //        HojaExcel.Cells[i, "EE"] = item.COD_TIPPENSION;

                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //    i = 1;
                        //    foreach (var item in ListaCurvaTasas)
                        //    {
                        //        // Asignar los valores de los registros a las celdas
                        //        HojaExcel.Cells[i, "EG"] = item.COD_MONEDA;
                        //        // ID
                        //        HojaExcel.Cells[i, "EH"] = item.COD_TIPPREAJUSTE; // FinTab
                        //        // Pregunta
                        //        HojaExcel.Cells[i, "EI"] = item.NUM_MES;
                        //        // Opciones
                        //        HojaExcel.Cells[i, "EJ"] = item.MTO_VALOR;
                        //        // Valor de la Respuesta
                        //        HojaExcel.Cells[i, "EK"] = item.FEC_INIVIG;
                        //        HojaExcel.Cells[i, "EL"] = item.FEC_TERVIG;
                        //        HojaExcel.Cells[i, "EM"] = item.MTO_CUOMOR;
                        //        HojaExcel.Cells[i, "EN"] = item.FEC_INICUOMOR;
                        //        HojaExcel.Cells[i, "EO"] = item.FEC_TERCUOMOR;

                        //        // Numero Votos
                        //        // Avanzamos una fila
                        //        i++;
                        //    }

                        //}
        
        #endregion
                            string querySisco = "";
                            querySisco = "SELECT dc.MTO_PENSION, dc.ind_sisco, c.cod_tippension, c.ind_cob, dc.prc_tasavta, isnull(CODIGO.COD_SCOMP, ' ') FROM PT_TMAE_DETCOTIZACION DC" +
                            " JOIN PT_TMAE_COTIZACION C ON C.NUM_OPERACION = DC.NUM_OPERACION" +
                            " JOIN MA_TPAR_TABCOD CODIGO ON DC.cod_tipren = CODIGO.COD_ELEMENTO and cod_tabla = 'TR'" +
                            " WHERE DC.NUM_OPERACION = " + rutina.NUM_COTESTUDIO + " and DC.num_correlativo = " + rutina.NUM_CORRELATIVO + " and (DC.IND_FILTROCOTIZA = 'S' OR IND_SISCO = 1)";
                            

                            List<SolicitudesCotizacion> sis = new List<SolicitudesCotizacion>();
                            sis = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(querySisco, x => new SolicitudesCotizacion
                            {
                                pensionSis = (double)x.GetDecimal(0),
                                Ind_Sis = x.GetInt32(1),
                                strTipPen = x.GetString(2),
                                strCobCon = x.GetString(3),
                                tasaSis = (double)x.GetDecimal(4),
                                strMod = x.GetString(5)
                            }).ToList();
                            
                            int valsisco = sis[0].Ind_Sis;
                            double mtopensis = sis[0].pensionSis;
                            string tippension = sis[0].strTipPen;
                            string cobertura = sis[0].strCobCon;
                            double tasaVtaAnterior = sis[0].tasaSis;

                            if (valsisco == 1)
                            {
                                var listBeneficiarios = _calculoCotizacionRepository.getBeneficiariosRutina(Convert.ToInt32(rutina.NUM_COTESTUDIO));
                                double suma = 0;
                                double pensionrutina = rutina.MTO_PENSION;
                                switch (tippension)
                                {
                                    case "08":
                                        for (int i = 0; i < listBeneficiarios.Count; i++)
                                        {

                                            if (listBeneficiarios[i].Num_Corr == rutina.NUM_CORRELATIVO && listBeneficiarios[i].Num_Operacion == Convert.ToInt32(rutina.NUM_COTESTUDIO))
                                            {
                                                suma = suma + pensionrutina * Convert.ToDouble((listBeneficiarios[i].prc_Pension / 100));
                                                pensionrutina = suma;
                                            }
                                        }
                                        break;
                                    case "07":
                                        if (cobertura == "S")
                                            pensionrutina = (Convert.ToDouble(pensionrutina) * (0.5));
                                        else
                                            pensionrutina = Convert.ToDouble(pensionrutina);
                                        break;
                                    case "06":
                                        if (cobertura == "S")
                                            pensionrutina = (Convert.ToDouble(pensionrutina) * (0.7));
                                        else
                                            pensionrutina = Convert.ToDouble(pensionrutina);
                                        break;
                                    case "05":
                                        pensionrutina = Convert.ToDouble(pensionrutina);
                                        break;
                                    case "04":
                                        pensionrutina = Convert.ToDouble(pensionrutina);
                                        break;
                                }

                                if (mtopensis > pensionrutina)
                                {
                                    if (codMoneda == "NS" && tiporeaj == 1)
                                    {
                                        rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Convert.ToDouble(ComisionInicial), 0.00001, 5, mtopensis);
                                        rutina.MTO_PENSION = mtopensis;
                                        if (rutina.PRC_TASAVTA == 5)
                                        {
                                            rutina.PRC_TASAVTA = 0.5;
                                        }
                                        Console.WriteLine("Memory used before collection:       {0:N0}",
                                        GC.GetTotalMemory(true));
                                    }
                                    else if (codMoneda == "NS" && tiporeaj == 2)
                                    {
                                        rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Convert.ToDouble(ComisionInicial), 0.00001, 10, mtopensis);
                                        rutina.MTO_PENSION = mtopensis;
                                        if (rutina.PRC_TASAVTA == 10)
                                        {
                                            rutina.PRC_TASAVTA = 0.5;
                                        }
                                        Console.WriteLine("Memory used before collection:       {0:N0}",
                                        GC.GetTotalMemory(true));
                                    }
                                    else
                                    {
                                        rutina = RActuarialMej.RutinaPension_mej(ListaModalidades, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, Convert.ToDouble(ComisionInicial), 0.00001, 7, mtopensis);
                                        rutina.MTO_PENSION = mtopensis;
                                        if (rutina.PRC_TASAVTA == 7)
                                        {
                                            rutina.PRC_TASAVTA = 0.5;
                                        }
                                        Console.WriteLine("Memory used before collection:       {0:N0}",
                                        GC.GetTotalMemory(true));
                                    }
                                }

                            }

                            if (rutina.PRC_TASAVTA < 0.5)
                            {
                                //rutina.PRC_TASAVTA = 0.5;
                                rutina.FecCal = FecCal;
                                rutina.Cod_Rechazo = "999";
                                rutina.Mensaje = "No se cotiza por tasa Negativa";
                                rutina.NUM_COTESTUDIO = operacion;
                                rutina.NUM_CORRELATIVO = correlativo;
                                rutina.INDFILTROCOTIZA = "N";
                            }
                            
                            if ((tasaVtaAnterior >= rutina.PRC_TASAVTA || mtopensis >= rutina.MTO_PENSION) && casoMej == "mejoras" )
                            {
                                rutina.PRC_TASAVTA = tasaVtaAnterior;
                                rutina.MTO_PENSION = mtopensis;

                                rutina.Mensaje = "La pensión de la modalidad " + sis[0].strMod + " con " + ListaModalidades[0].MesDif + " renta temporal y " + ListaModalidades[0].MesGar + " periodo garantizado ya no se puede mejorar";
                            }
                            
                        }
                        #endregion

                        string querySisco4 = "";
                        querySisco4 = "SELECT dc.MTO_PENSION, dc.ind_sisco, c.cod_tippension, c.ind_cob FROM PT_TMAE_DETCOTIZACION DC" +
                        " JOIN PT_TMAE_COTIZACION C ON C.NUM_OPERACION = DC.NUM_OPERACION" +
                        " WHERE DC.NUM_OPERACION = " + rutina.NUM_COTESTUDIO + " and DC.num_correlativo = " + rutina.NUM_CORRELATIVO + " and DC.IND_FILTROCOTIZA = 'S'";

                        List<SolicitudesCotizacion> sis2 = new List<SolicitudesCotizacion>();
                        sis2 = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(querySisco4, x => new SolicitudesCotizacion
                        {
                            pensionSis = (double)x.GetDecimal(0),
                            Ind_Sis = x.GetInt32(1),
                            strTipPen = x.GetString(2),
                            strCobCon = x.GetString(3)
                        }).ToList();
                        rutina.IND_SISCO = sis2[0].Ind_Sis;
                        int valsisco4 = sis2[0].Ind_Sis;
                        double mtopensis4 = sis2[0].pensionSis;
                        string tippension4 = sis2[0].strTipPen;
                        string cobertura4 = sis2[0].strCobCon;

                        if (rutina.PRC_TASATIR < 0 && valsisco4 == 0)
                        {
                            rutina.FecCal = FecCal;
                            rutina.Cod_Rechazo = "999";
                            rutina.Mensaje = "No se cotiza por Rentabilidad Negativa";
                            rutina.NUM_COTESTUDIO = operacion;
                            rutina.NUM_CORRELATIVO = correlativo;
                            rutina.INDFILTROCOTIZA = "N";
                        }

                        else if (rutina.Mensaje != null)
                        {
                            rutina.FecCal = FecCal;
                            rutina.Cod_Rechazo = "999";
                            rutina.NUM_COTESTUDIO = operacion;
                            rutina.NUM_CORRELATIVO = correlativo;
                            rutina.INDFILTROCOTIZA = "N";
                        }
                        else
                        {
                            rutina.FecCal = FecCal;
                            rutina.Cod_Rechazo = "0";
                            //rutina.INDFILTROCOTIZA = "S";
                        }
                        Console.WriteLine("Memory used before collection:       {0:N0}",
                        GC.GetTotalMemory(false));
                    }
                    else
                    {
                        rutina.Mensaje = ListaBen[ListaBen.Count - 1].Mensaje;
                        rutina.FecCal = FecCal;
                        rutina.Cod_Rechazo = "900";
                        rutina.NUM_COTESTUDIO = operacion;
                        rutina.NUM_CORRELATIVO = correlativo;
                        rutina.INDFILTROCOTIZA = "N";
                    }
                }
                catch (Exception ex)
                {
                    rutina.FecCal = FecCal;
                    rutina.Cod_Rechazo = "999";
                    rutina.Mensaje = "ERROR DE CALCULO";//"Error de calculo";
                    _log.Info("Error de calculo");
                    _log.Info(ex.ToString());
                    rutina.NUM_COTESTUDIO = operacion;
                    rutina.NUM_CORRELATIVO = correlativo;
                    rutina.INDFILTROCOTIZA = "N";
                }

                if (rutina.NUM_COTESTUDIO == null)
                {
                    rutina.FecCal = FecCal;
                    rutina.Mensaje = "Error de calculo";
                    _log.Info("Cayo en error de Numero de operacion == null");
                    rutina.Cod_Rechazo = "999";
                    rutina.NUM_COTESTUDIO = operacion;
                    rutina.NUM_CORRELATIVO = correlativo;
                    rutina.INDFILTROCOTIZA = "N";
                }

                GC.Collect();
                Console.WriteLine("Memory used after full collection:   {0:N0}",
                GC.GetTotalMemory(true));
                ResultadoCotGlobal.Add(rutina);
            });
        }

        public List<beDatosBen> ListaBeneficiarios(int idSolicitud, string FecCal, string TipPen, List<bePorcenLegales> LisTabPL, string NumArch, string fechaSol, string cober, string fechaDevengue)
        {
            string diaDeVS = fechaSol.Substring(6, 2);
            string mesDeVS = fechaSol.Substring(4, 2);
            string anioDeVS = fechaSol.Substring(0, 4);
            string fechaDeVS = anioDeVS + "/" + mesDeVS + "/" + diaDeVS;

            int EdaLim = Convert.ToDateTime(fechaDeVS) < Convert.ToDateTime("2013/08/01") ? 216 : 336;

            List<Beneficiario> beneficiarios = _calcularAsignacionIntermediarioRepository.ConsultarBeneficiariosModificar(idSolicitud.ToString(), NumArch).OrderByDescending(b => b.CodigoParentesco).ToList();
            List<beDatosBen> ListaBen = new List<beDatosBen>();

            foreach (var item in beneficiarios)
            {
                beDatosBen TablaBen = new beDatosBen();

                // HARDCODE (Eliminar)
                string SituacionInvalidez = "";
                string fechaInv = "";
                string fechafa = "";
                int hijos = 0;
                if (item.CodigoParentesco == "99")
                {
                    switch (TipPen)
                    {
                        case "04": // Jubilacion Legal
                            if (item.ClaveSituacionInvalidez == "T")
                            {
                                SituacionInvalidez = "T";
                                fechaInv = "";
                                fechafa = "";
                            }
                            else
                            {
                                SituacionInvalidez = "N";
                                fechaInv = "";
                                fechafa = "";
                            }

                            break;
                        case "05": // Jubilacion Anticipada 
                            if (item.ClaveSituacionInvalidez == "T")
                            {
                                SituacionInvalidez = "T";
                                fechaInv = "";
                                fechafa = "";
                            }
                            else
                            {
                                SituacionInvalidez = "N";
                                fechaInv = "";
                                fechafa = "";
                            }
                            break;
                        case "06": // INVALIDEZ TOTAL
                            SituacionInvalidez = "T";
                            fechaInv = fechaDevengue;
                            fechafa = "";
                            break;

                        case "07": // INVALIDEZ PARCIAL
                            SituacionInvalidez = "P";
                            fechaInv = fechaDevengue;
                            fechafa = "";
                            break;
                        case "08": // SOBREVIVENCIA
                            SituacionInvalidez = "N";
                            fechaInv = "";
                            fechafa = fechaDevengue;
                            break;

                        default:
                            SituacionInvalidez = "N";
                            fechaInv = "";
                            fechafa = "";
                            break;
                    }
                }
                else if (item.CodigoParentesco != "99" && item.ClaveSituacionInvalidez == "S")
                {
                    SituacionInvalidez = "T";
                }
                else
                    SituacionInvalidez = item.ClaveSituacionInvalidez;

                
                TablaBen.NumOrd = item.IdBeneficiario;
                if (item.CodigoParentesco == "21")
                {
                    TablaBen.CodPar = "11";
                }
                else if (item.CodigoParentesco == "20")
                {
                    hijos = (from b in beneficiarios where b.CodigoParentesco == "30" select b).Count();
                    if (hijos == 0)
                    {
                        TablaBen.CodPar = "10";
                    }
                    else
                    {
                        TablaBen.CodPar = "11";
                    }
                }
                else
                {
                    TablaBen.CodPar = item.CodigoParentesco;
                }
                //TablaBen.CodPar = item.CodigoParentesco;
                TablaBen.FecNac = item.FechaNacimiento.ToString("yyyyMMdd");
                //if ((item.CodigoParentesco != "99") && (Convert.ToInt32(fechaDevengue) < Convert.ToInt32(TablaBen.FecNac)))
                //{
                //    TablaBen.Mensaje = "Hijo NO nato";
                //}
                TablaBen.GruFam = item.Parentesco == "99" ? "00" : "01";
                TablaBen.TipSex = item.ClaveSexo;
                TablaBen.TipInv = SituacionInvalidez;
                TablaBen.FecInv = fechaInv;
                TablaBen.DerPen = "99";
                TablaBen.PrcPen = 0;
                TablaBen.PrcLeg = 0;
                TablaBen.PrcGar = 0;
                TablaBen.NacHM = "";
                TablaBen.FacFal = fechafa;
                TablaBen.derCre = "N";
                ListaBen.Add(TablaBen);
            }

            RutinaPorcentaje RP = new RutinaPorcentaje();
            List<beDatosBen> CopiaBen = new List<beDatosBen>();
            foreach (var itembencop in ListaBen)
            {
                CopiaBen.Add(new beDatosBen { CodPar = itembencop.CodPar, NacHM = itembencop.NacHM });
            }
            ListaBen = RP.PorcentajeBen(ListaBen, fechaDevengue, cober, TipPen, EdaLim, LisTabPL, CopiaBen);

            
            for (int i = 0; i < ListaBen.Count; i++)
            {
                _calcularAsignacionIntermediarioRepository.pensionBen(Convert.ToDecimal(ListaBen[i].PrcPen), int.Parse(NumArch), ListaBen[i].NumOrd, idSolicitud.ToString(), Convert.ToDecimal(ListaBen[i].PrcLeg));
            }
            ListaBen_tmp = ListaBen;
            return ListaBen;
        }

    }
}
