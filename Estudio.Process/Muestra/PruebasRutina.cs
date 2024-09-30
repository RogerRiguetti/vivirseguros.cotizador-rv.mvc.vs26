using System;
using System.Collections.Generic;
using System.Linq;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Persistence.Repositories;
using System.Data;
using log4net;
using log4net.Config;
using System.Reflection;

namespace Estudio.Process.Muestra
{
    public class PruebasRutina
    {
        RutinaRepository _rutinaRepository = new RutinaRepository();
        CotizacionRepository _cotizacionRepository = new CotizacionRepository();
        BeneficiariosRepository _beneficiarioRepository = new BeneficiariosRepository();
        ModalidadesRepository _modalidadRepository = new ModalidadesRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<beResultados> Rutina(int idCotizacion)
        {
            XmlConfigurator.Configure();
            Cotizacion cotizacion = _cotizacionRepository.ConsultarCotizacion(idCotizacion);
            List<Beneficiario> beneficiarios = _beneficiarioRepository.ConsultarBeneficiariosModificar(idCotizacion);
            List<Modalidad> modalidades = _modalidadRepository.ConsultarModalidadesModificar(idCotizacion);

            string FecCal = cotizacion.FechaEstudio.ToString("yyyyMMdd");
            string TipPen = cotizacion.CodigoPension;
            string fechaDevengue = cotizacion.FechaDevengue.ToString("yyyy") + cotizacion.FechaDevengue.ToString("MM") + "01";

            int MortalVit_F, MortalTot_F, MortalPar_F, MortalBen_F, MortalVit_M, MortalTot_M, MortalPar_M, MortalBen_M;

            RutinaMortalidad RMortal = new RutinaMortalidad();
            RutinaActOficiales RActuarial = new RutinaActOficiales();
            beResultados rutina = new beResultados();
            List<beResultados> ResultadoCot = new List<beResultados>();

            #region Carga Matriz de Tablas de Mortalidad
            //List<beMortalidad> ListaMor = new List<beMortalidad>();
            //List<beMortalVar> LisTab = new List<beMortalVar>();
            //try
            //{
            //    LisTab = _rutinaRepository.ConsultaDetalleMatriz(FecCal);

            //    MortalVit_F = LisTab.Where(x => x.COD_TIPTABMOR == "RV" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalVit_M = LisTab.Where(x => x.COD_TIPTABMOR == "RV" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalTot_F = LisTab.Where(x => x.COD_TIPTABMOR == "MIT" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalTot_M = LisTab.Where(x => x.COD_TIPTABMOR == "MIT" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalPar_F = LisTab.Where(x => x.COD_TIPTABMOR == "MIP" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalPar_M = LisTab.Where(x => x.COD_TIPTABMOR == "MIP" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalBen_F = LisTab.Where(x => x.COD_TIPTABMOR == "B" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
            //    MortalBen_M = LisTab.Where(x => x.COD_TIPTABMOR == "B" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();

            //    //OBTIENE EL DETALLE DE LAS TABLAS DE MORTALIDAD

            //    List<beMortalidadDet> LisTabMD = new List<beMortalidadDet>();

            //    LisTabMD = _rutinaRepository.ConsultaCargaMatriz();

            //    ListaMor = RMortal.TablaMortalidad(LisTabMD, MortalVit_F, MortalTot_F, MortalPar_F, MortalBen_F, MortalVit_M, MortalTot_M, MortalPar_M, MortalBen_M);

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("{0}", ex.Message);
            //}

            #endregion

            #region Carga Tabla Mortaliadad Dinamicas


            int Mortal_M_S, Mortal_F_S, Mortal_M_I, Mortal_F_I;

            RutinaMortalidadDin RMortalDin = new RutinaMortalidadDin();
            List<beMortalidadDin> LisTabDin = new List<beMortalidadDin>();
            List<beMortalidadDinVal> ListaMor = new List<beMortalidadDinVal>();
            
            try
            {
                LisTabDin = _rutinaRepository.ConsultaTablaMortalidadDinamicas(FecCal);
                //LisTabDin = (from td in LisTabDinPar where Convert.ToInt32(td.FEC_INIVIG) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(td.FEC_FINVIG) select td).ToList();

                Mortal_M_S = LisTabDin.Where(x => x.COD_INVALIDEZ == "S" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_F_S = LisTabDin.Where(x => x.COD_INVALIDEZ == "S" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_M_I = LisTabDin.Where(x => x.COD_INVALIDEZ == "I" && x.COD_SEXO == "M").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();
                Mortal_F_I = LisTabDin.Where(x => x.COD_INVALIDEZ == "I" && x.COD_SEXO == "F").Select(x => x.NUM_CORRELATIVO).SingleOrDefault();

                //OBTIENE EL DETALLE DE LAS TABLAS DE MORTALIDAD

                List<beMortalidadDinDet> LisTabMD = new List<beMortalidadDinDet>();
                LisTabMD = _rutinaRepository.ConsultaDetTablaMortalidadDin();
                //LisTabMD = LisTabMDPar;

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

            foreach (var item in modalidades)
            {
                beDatosModalidad TablaPar = new beDatosModalidad();
                ListaPar = new List<beDatosModalidad>();

                
                beDatosModalidad datoModalidad = _rutinaRepository.ConsultaCodigoRegion(Convert.ToDecimal(item.ValorComision), cotizacion.Cic, cotizacion.IdDepartamento);

                if (datoModalidad != null)
                {
                    TablaPar.NumCot = idCotizacion.ToString();
                    TablaPar.FinTab = 1332;
                    TablaPar.Tippen = cotizacion.CodigoPension;
                    TablaPar.TipRen = item.CodigoTiposRenta;
                    TablaPar.TipMod = item.CodigoModalidad;
                    TablaPar.MesGar = item.AniosGarantizados * 12;
                    TablaPar.FecCot = cotizacion.FechaEstudio.ToString("yyyyMMdd");
                    TablaPar.MtoPri = Convert.ToDouble(cotizacion.Cic);

                    TablaPar.FecDev = fechaDevengue;
                    TablaPar.DevSol = fechaDevengue;
                    TablaPar.TipSex = cotizacion.ClaveSexo;
                    TablaPar.FecNac = cotizacion.FechaNacimiento.ToString("yyyyMMdd");
                    TablaPar.MtoGS = Convert.ToDouble(cotizacion.GastoSepelio);
                    TablaPar.RenAfp = Convert.ToDouble(item.PorcentajeRentabilidadAfp);
                    TablaPar.NumCor = item.IdModalidad;
                    TablaPar.PrcTas = 0;
                    TablaPar.PrcCom = Convert.ToDouble(item.ValorComision);
                    TablaPar.CodMon = item.ClaveMoneda;
                    TablaPar.DerCre = "N";
                    TablaPar.DerGra = item.DerGra;
                    TablaPar.IndCob = "S";
                    TablaPar.CodReg = datoModalidad.CodReg;
                    TablaPar.RegEst = "14";
                    TablaPar.TipRea = item.CodigoTipoReajuste;

                    // Valores Factores
                    valoresFactores = _rutinaRepository.ConsultaValoresFactores(cotizacion.FechaEstudio.ToString("yyyyMMdd"), item.ClaveMoneda);
                    TablaPar.PrcMen = valoresFactores.PrcMen;
                    TablaPar.PrcTri = valoresFactores.PrcTri;
                    TablaPar.PrcAnu = valoresFactores.PrcAnu;

                    TablaPar.EdaLim = cotizacion.FechaDevengue < Convert.ToDateTime("2013/08/01") ? 216 : 336;
                    TablaPar.MinRC = 0;
                    TablaPar.RepRC = 0;
                    TablaPar.ValCam = Convert.ToDouble(cotizacion.TipoCambio);
                    ListaPar.Add(TablaPar);

                    // Tipo de Renta
                    if (item.CodigoTiposRenta == "6")
                    {
                        TablaPar.MesDif = Convert.ToInt16(item.PrimerTramo) * 12;
                        TablaPar.RenTmp = Convert.ToInt16(item.SegundoTramo);
                    }
                    else
                    {
                        TablaPar.MesDif = item.AniosDiferidos * 12;
                        TablaPar.RenTmp = item.PorcentajeRentaTemporal;
                    }

                    if (TablaPar.MesDif == 0)
                    {
                        TablaPar.PrcCom = (Convert.ToDouble(3.2) + Convert.ToDouble(1.71)) * Convert.ToDouble(1.47);
                    }
                    else
                    {
                        TablaPar.PrcCom = (Convert.ToDouble(2.4) + Convert.ToDouble(1.71)) * Convert.ToDouble(1.47);
                    }
                    //TablaPar.PrccomS = Convert.ToDecimal(1.71);
                    //TablaPar.Prcfaclab = Convert.ToDecimal(1.47);
                    //TablaPar.Prc_Inicial_1 = Convert.ToDecimal(3.2);
                    //TablaPar.Prc_Inicial = Convert.ToDecimal(2.4);

                    ListaModalidades.Add(ListaPar);
                }
                else
                {
                    rutina.Mensaje = "No se encontró Código de Región para el CIC registrado.";
                    ResultadoCot.Add(rutina);

                    return ResultadoCot;
                }
            }

            #endregion

            #region Carga Beneficiario
            //OBTIENE EL DETALLE DE LAS TABLAS DE MORTALIDAD
            List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();
            LisTabPL = _rutinaRepository.ConsultaDetalleMortalidad(FecCal);

            List<beDatosBen> ListaBen = new List<beDatosBen>();
            ListaBen = ListaBeneficiarios(idCotizacion, fechaDevengue, TipPen, LisTabPL);

            #endregion

            #region Carga Gastos Tasas Indicadores

            List<beDatosTasasPar> ListaTas = new List<beDatosTasasPar>();
            ListaTas = _rutinaRepository.ConsultaTasasIndicadores(FecCal, TipPen);
            #endregion

            #region Carga Tasa Mercado
            string mes = FecCal.Substring(4, 2);

            if (mes.StartsWith("0"))
                mes = mes.Replace("0", "");

            string query = "SELECT COD_MONEDA, convert(numeric,COD_TIPREAJUSTE) COD_TIPREAJUSTE, PRC_MES" + mes +
                " AS PRC_MES FROM MA_TVAL_TASATM WHERE NUM_ANNO=" + FecCal.Substring(0, 4);

            List<beTasaMercado> ListaTM = new List<beTasaMercado>();
            ListaTM = _rutinaRepository.ConsultaTasaMercado(query);

            #endregion

            #region Carga Tasa Anclaje

            List<beTasaAnclaje> ListaTA = new List<beTasaAnclaje>();
            ListaTA = _rutinaRepository.ConsultaTasaAnclaje(FecCal);

            #endregion

            #region Carga Factor VAC
            List<beTasaFacVac> ListaVac = new List<beTasaFacVac>();
            //se debe crear la tabla de Factores Vacpara indexados
            ListaVac = _rutinaRepository.ConsultaFactorVac();
            #endregion

            #region Carga CPK's

            List<beCPK> ListaCPK = new List<beCPK>();
            ListaCPK = _rutinaRepository.ConsultaCPK(FecCal);

            #endregion

            #region Carga Rentabilidad

            List<beRentabilidad> ListaRen = new List<beRentabilidad>();
            ListaRen = _rutinaRepository.ConsultaRentabilidad(FecCal);

            #endregion


            #region Tasas Promedio 

            List<beTasasPromedio> ListaTasProm = new List<beTasasPromedio>();
            string FecCalMod = FecCal.Substring(0, 6) + "01";
            ListaTasProm = _rutinaRepository.ConsultaTasasPromedio(TipPen);
            //ListaTasProm = (from tasprom in ListaTasPromPar where tasprom.COD_TIPPENSION == TipPen select tasprom).ToList();
            #endregion

            #region Curva Tasas

            List<beCurvaTasas> ListaCurvaTasas = new List<beCurvaTasas>();
            ListaCurvaTasas = _rutinaRepository.ConsultaCurvaTasas(FecCal);
           // ListaCurvaTasas = (from cutas in ListaCurvaTasasPar where Convert.ToInt32(cutas.FEC_INIVIG) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(cutas.FEC_TERVIG) select cutas).ToList();
            #endregion

            //LLAMA LISTADO PAA OBTENER LOS DATOS DE COTIZACION
            if (ListaBen.Count == 1 && ListaBen[0].Mensaje != "" && ListaBen[0].Mensaje != null)
            {
                rutina = new beResultados();
                rutina.Mensaje = ListaBen[0].Mensaje;

                ResultadoCot.Add(rutina);
            }
            else
            {
                foreach (var item in ListaModalidades)
                {
                    RActuarial = new RutinaActOficiales();
                    rutina = RActuarial.RutinaPension(item, ListaBen, ListaTas, ListaTM, ListaTA, ListaMor, ListaCPK, ListaRen, ListaVac, LisTabPL, ListaTasProm, ListaCurvaTasas, "E",0, "", null);

                    ResultadoCot.Add(rutina);

                    Console.WriteLine("Memory used before collection:       {0:N0}",
                            GC.GetTotalMemory(false));

                    // Collect all generations of memory.
                    GC.Collect();
                    Console.WriteLine("Memory used after full collection:   {0:N0}",
                                      GC.GetTotalMemory(true));

                    if (rutina.Mensaje != null)
                        break;
                }
            }

            return ResultadoCot;
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-06-04
        /// Genera la lista de beneficiarios con porcentajes
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotización relacionada a los beneficiarios </param>
        /// <param name="FecDev"> Fecha de creación de la cotización </param>
        /// <param name="TipPen"> Codigo de la pensión </param>
        /// <param name="LisTabPL"> Lista de detalles de las tablas de mortalidad </param>
        /// <returns> Regresa una lista de beneficiarios con los porcentajes que les corresponde </returns>

        public List<beDatosBen> ListaBeneficiarios(int idCotizacion, string FecDev, string TipPen, List<bePorcenLegales> LisTabPL)
        {
            List<Beneficiario> beneficiarios = _beneficiarioRepository.ConsultarBeneficiariosModificar(idCotizacion).OrderByDescending(b => b.CodigoParentesco).ToList();
            List<beDatosBen> ListaBen = new List<beDatosBen>();

            foreach (var item in beneficiarios)
            {
                beDatosBen TablaBen = new beDatosBen();

                // HARDCODE (Eliminar)
                string SituacionInvalidez = "";
                int hijos = 0;
                if (item.CodigoParentesco == "99")
                {
                    switch (TipPen)
                    {
                        case "06": // INVALIDEZ TOTAL
                            SituacionInvalidez = "T";
                            break;

                        case "07": // INVALIDEZ PARCIAL
                            SituacionInvalidez = "P";
                            break;

                        default:
                            SituacionInvalidez = "N";
                            break;
                    }

                }
                else
                    SituacionInvalidez = item.ClaveSituacionInvalidez;

                if (item.Parentesco == "CÓNYUGE")
                {
                    hijos = (from b in beneficiarios where b.Parentesco == "HIJOS" select b).Count();

                    if (hijos == 0)
                        item.CodigoParentesco = "10";
                    else
                        item.CodigoParentesco = "11";
                }

                if (item.Parentesco == "PADRE")
                {
                    if (item.ClaveSexo == "M")
                        item.CodigoParentesco = "41";
                    else
                        item.CodigoParentesco = "42";
                }

                TablaBen.NumOrd = item.IdBeneficiario;
                TablaBen.CodPar = item.CodigoParentesco;
                TablaBen.FecNac = item.FechaNacimiento.ToString("yyyyMMdd");
                TablaBen.GruFam = item.Parentesco == "TITULAR" ? "00" : "01";
                TablaBen.TipSex = item.ClaveSexo;
                TablaBen.TipInv = SituacionInvalidez;
                TablaBen.FecInv = item.FechaInvalidezRut;
                TablaBen.DerPen = "99";
                TablaBen.PrcPen = 0;
                TablaBen.PrcLeg = 0;
                TablaBen.PrcGar = 0;
                TablaBen.NacHM = "";
                TablaBen.FacFal = item.FechaFallecimientoStr;
                TablaBen.derCre = "N";
                ListaBen.Add(TablaBen);
            }

            RutinaPorcentaje RP = new RutinaPorcentaje();
            List<beDatosBen> CopiaBen = new List<beDatosBen>();
            foreach (var itembencop in ListaBen)
            {
                CopiaBen.Add(new beDatosBen { CodPar = itembencop.CodPar, NacHM = itembencop.NacHM });
            }
            ListaBen = RP.PorcentajeBen(ListaBen, FecDev, "S", TipPen, 336, LisTabPL, CopiaBen);

            return ListaBen;
        }
    }
}
