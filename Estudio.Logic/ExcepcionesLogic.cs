using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using System;
using Estudio.Process.Muestra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Data;
using log4net;
using System.Reflection;
using log4net.Config;

namespace Estudio.Logic
{
    public class ExcepcionesLogic
    {
        private ExcepcionesRepository _ExcepcionesRepository = new ExcepcionesRepository();
        PruebasRutinaOficiales _pruebasRutinaOficiales = new PruebasRutinaOficiales();
        RutinaOficialesRepository _rutinaOficialesRepository = new RutinaOficialesRepository();
        LimiteCotizacionMejoradaRepository _LimiteCotizacionMejoradaRepository = new LimiteCotizacionMejoradaRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Response busqueda(string codCUSPP, string numCor, string numOperacion)
        {
            Response res = new Response();
            try
            {
                var datosBusqueda = _ExcepcionesRepository.busqueda(codCUSPP, numCor, numOperacion);
                if (datosBusqueda.IsOk == false)
                {
                    res.IsOk = false;
                    res.Message = datosBusqueda.Message;
                    return res;
                }

                res.Object = datosBusqueda.Object;
                res.IsOk = true;
                res.Message = "Datos encontrados con exito";
                return res;
            }
            catch (Exception ex)
            {
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }

        public Response obtenerDatos()
        {
            Response resultado = new Response();
            try
            {
                resultado.Object = _ExcepcionesRepository.obtenerDatos();
                resultado.IsOk = true;
            }
            catch (Exception ex)
            {
                resultado.Message = ex.ToString();
                resultado.IsOk = false;
            }

            return resultado;
        }
        public async Task<Response> calculo(string nuevaTV, string nuevaCo, Exceptiones datos, string caso, string parametro)
        {
            Response res = new Response();
            if (caso == "mejoras" && (nuevaTV == "true" || nuevaTV == "True" || nuevaTV == "TRUE"))
                nuevaTV = "";
            else if (caso == "mejoras" && (nuevaTV == "false" || nuevaTV == "False" || nuevaTV == "FALSE"))
            {
                nuevaTV = "";
                parametro = "INI";
            }
            _ExcepcionesRepository.caso = caso;
            _ExcepcionesRepository.parametro = parametro;
            switch (caso)
            {
                case "excepciones": res = await excepciones(nuevaTV, nuevaCo, datos); break;
                case "mejoras": res = await mejoras(nuevaTV, nuevaCo, datos); break;
            }
            return res;
        }
        public Response Guardar(Exceptiones datos, string caso, Exceptiones infoRut)
        {
            Response res = new Response();
            Exceptiones valida = _ExcepcionesRepository.VALIDACIONGUARDAR(datos.numOperacion, datos.numCorrelativo);
            if (valida.Ind_Estado != "I")
            {
                res.IsOk = false;
                res.Message = "No puede ser modificada esta modalidad (Archivo enviado al Meler)";
                return res;
            }
            else
            {
                Exceptiones NArchivo = _ExcepcionesRepository.obtenerNA(datos.numOperacion.ToString(), datos.numCorrelativo.ToString());

                SISCORepository _SISCORepository = new SISCORepository();

                List<SISCO> CotSISCO = new List<SISCO>();
                string montoPenSISCO = "";
                string montoTasV = "";
                SISCO valSISCO = new SISCO();

                valSISCO = _SISCORepository.ConsultaSISCO(datos.numOperacion.ToString(), NArchivo.cuspp);

                if (valSISCO == null)
                {
                    _ExcepcionesRepository.caso = caso;
                    switch (caso)
                    {
                        case "excepciones": res = _ExcepcionesRepository.calculoU(datos, infoRut, "E"); break;
                        case "excepcionesExternas": res = _ExcepcionesRepository.calculoUExternas(datos, infoRut, "E"); break;
                        case "mejoras": res = _ExcepcionesRepository.calculoU(datos, infoRut, "M"); break;
                    }
                    return res;
                }
                else
                {
                    if (valida.Ind_SISCO == 1)
                    {
                        montoPenSISCO = valSISCO.PensionSISCO.ToString();
                        montoTasV = valSISCO.TasaInteres.ToString();

                        if (Convert.ToDouble(datos.mtoPensio) < Convert.ToDouble(montoPenSISCO))
                        {
                            res.IsOk = false;
                            res.Message = "No puede ser guardado es un caso SISCO, la pensión minima es " + valSISCO.PensionSISCO.ToString("N2");
                            return res;
                        }
                        else
                        {
                            _ExcepcionesRepository.caso = caso;
                            switch (caso)
                            {
                                case "excepciones": res = _ExcepcionesRepository.calculoU(datos, infoRut, "E"); break;
                                case "excepcionesExternas": res = _ExcepcionesRepository.calculoUExternas(datos, infoRut, "E"); break;
                                case "mejoras": res = _ExcepcionesRepository.calculoU(datos, infoRut, "M"); break;
                            }
                            res.Message = "Los datos a modificar pertenecen a un caso SISCO";
                            return res;
                        }
                    }
                    else
                    {
                        _ExcepcionesRepository.caso = caso;
                        switch (caso)
                        {
                            case "excepciones": res = _ExcepcionesRepository.calculoU(datos, infoRut, "E"); break;
                            case "excepcionesExternas": res = _ExcepcionesRepository.calculoUExternas(datos, infoRut, "E"); break;
                            case "mejoras": res = _ExcepcionesRepository.calculoU(datos, infoRut, "M"); break;
                        }
                        return res;
                    }
                }
            }
        }
        public async Task<Response> excepciones(string nuevaTV, string nuevaCo, Exceptiones datos)
        {
            Response res = new Response();
            try
            {
                _ExcepcionesRepository.numArchivo = datos.numArchivo;
                if (nuevaCo == "")
                {
                    _ExcepcionesRepository.nuevaCO = datos.comision;
                }
                else
                {
                    _ExcepcionesRepository.nuevaCO = Convert.ToDouble(nuevaCo);
                }

                if (nuevaTV == "")
                {
                    _ExcepcionesRepository.nuevaVT = datos.tasaVenta;
                }
                else
                {
                    _ExcepcionesRepository.nuevaVT = Convert.ToDouble(nuevaTV);
                }


                _ExcepcionesRepository.numCot = datos.numCot;
                _ExcepcionesRepository.numCorrelativo = datos.numCorrelativo;

                List<List<beResultados>> rutinaOficiales = new List<List<beResultados>>();
                List<beMortalidadDin> LisTabDin = new List<beMortalidadDin>();
                List<beMortalidadDinDet> LisTabMD = new List<beMortalidadDinDet>();
                List<beDatosModalidad> VarMTGS = new List<beDatosModalidad>();
                List<beDatosModalidad> VarAFP = new List<beDatosModalidad>();
                List<beDatosModalidad> VarREG = new List<beDatosModalidad>();
                List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();
                List<beTasaAnclaje> ListaTA = new List<beTasaAnclaje>();
                List<beCPK> ListaCPK = new List<beCPK>();
                List<beRentabilidad> ListaRen = new List<beRentabilidad>();
                List<beTasasPromedio> ListaTasProm = new List<beTasasPromedio>();
                List<beCurvaTasas> ListaCurvaTasas = new List<beCurvaTasas>();

                LisTabDin = _rutinaOficialesRepository.ConsultaTablaMortalidadDinamicas("");
                LisTabMD = _rutinaOficialesRepository.ConsultaDetTablaMortalidadDin();
                VarMTGS = _rutinaOficialesRepository.ConsultaGastoSepelioMes("");
                VarAFP = _rutinaOficialesRepository.ConsultaRentabilidadAfp("");
                VarREG = _rutinaOficialesRepository.ConsultaRegionTasas(0);
                LisTabPL = _rutinaOficialesRepository.ConsultaPorcentaje("");
                ListaTA = _rutinaOficialesRepository.ConsultaTasaAnclaje("");
                ListaCPK = _rutinaOficialesRepository.ConsultaCPKS("");
                ListaRen = _rutinaOficialesRepository.ConsultaRentabilidad("");
                ListaTasProm = _rutinaOficialesRepository.ConsultaTasasPromedio("");
                ListaCurvaTasas = _rutinaOficialesRepository.ConsultaCurvaTasas("");

                datos.asesor = _ExcepcionesRepository.ConsultaAsesor(datos.numOperacion.ToString());

                Console.WriteLine("Memory used before collection:       {0:N0}",
                GC.GetTotalMemory(false));

                rutinaOficiales.Add(await _pruebasRutinaOficiales.RutinaOficiales(datos.numOperacion, datos.numCot, datos.numArchivo, LisTabDin, LisTabMD, VarMTGS, VarAFP, VarREG, LisTabPL, ListaTA, ListaCPK, ListaRen, ListaTasProm, ListaCurvaTasas, _ExcepcionesRepository, "",Convert.ToInt32(datos.asesor)));

                double mtoPen = 0;
                double primaUni = 0;
                double tasvtarut = 0;

                foreach (var cotizacion in rutinaOficiales)
                {
                    foreach (var modalidad in cotizacion)
                    {
                        mtoPen = modalidad.MTO_PENSION;
                        primaUni = modalidad.MTO_PRIUNIDIF;
                        tasvtarut = modalidad.PRC_TASAVTA;
                    }

                }
                CalculoCotizacionRepository _calculoCotizacionRepository = new CalculoCotizacionRepository();

                double sumaPension = 0;

                Exceptiones NArchivo = _ExcepcionesRepository.obtenerNA(datos.numOperacion.ToString(), datos.numCorrelativo.ToString());


                decimal suma = 0;
                double PrimaUnica = 0;
                double PrimerTramo = 0;
                double SegundoTramo = 0;
                int AniosDif = 0;
                int rentEsc = 0;
                var listBeneficiarios = _calculoCotizacionRepository.getBeneficiarios(NArchivo.numArchivo);
                //for (int b = 0; b < listBeneficiarios.Count; b++)
                //{
                    switch (NArchivo.codTP)
                    {
                        case "04":
                            sumaPension = mtoPen;
                            break;
                        case "05":
                            sumaPension = mtoPen;
                            break;
                        case "06":
                            if (NArchivo.Ind_Cob == "S")
                                sumaPension = Convert.ToDouble(mtoPen) * (0.7);
                            else
                                sumaPension = Convert.ToDouble(mtoPen);
                            break;
                        case "07":
                            if (NArchivo.Ind_Cob == "S")
                                sumaPension = Convert.ToDouble(mtoPen) * (0.5);
                            else
                                sumaPension = Convert.ToDouble(mtoPen);
                            break;
                        case "08":
                        for (int b = 0; b < listBeneficiarios.Count; b++)
                        {
                            if (listBeneficiarios[b].Num_Corr == datos.numCorrelativo && listBeneficiarios[b].Num_Operacion == datos.numOperacion)
                            {
                                suma = suma + Convert.ToDecimal(mtoPen) * (listBeneficiarios[b].prc_Pension / 100);
                                sumaPension = Convert.ToDouble(suma);
                            }
                        }
                            break;
                    }

                //}

                switch (NArchivo.codTipRen)
                {
                    case "1":
                        {
                            if (datos.moneda == "S/.Aj." || datos.moneda == "S/.")
                            {

                                PrimaUnica = primaUni;
                            }
                            else
                            {
                                PrimaUnica = primaUni * Convert.ToDouble(NArchivo.codTipCambio);
                            }
                            PrimerTramo = 0 /*(Convert.ToDouble(sumaPension) * 2)*/;
                            SegundoTramo = Convert.ToDouble(sumaPension);
                            AniosDif = Convert.ToInt32(datos.periodoDiferido);
                            rentEsc = Convert.ToInt32(datos.rentaEsc);
                            break;
                        }
                    case "2":
                        {
                            if (datos.moneda != "S/.Aj." && datos.moneda != "S/.")
                            {
                                PrimerTramo = (Convert.ToDouble(sumaPension) * 2) * Convert.ToDouble(NArchivo.codTipCambio);
                                PrimaUnica = primaUni * Convert.ToDouble(NArchivo.codTipCambio);
                            }
                            else
                            {
                                PrimerTramo = (Convert.ToDouble(sumaPension) * 2);
                                PrimaUnica = primaUni;
                            }
                            SegundoTramo = Convert.ToDouble(sumaPension);
                            AniosDif = Convert.ToInt32(datos.periodoDiferido);
                            rentEsc = Convert.ToInt32(datos.rentaEsc);
                            break;
                        }

                    case "6":
                        {
                            if (datos.moneda != "S/.Aj." && datos.moneda != "S/.")
                            {
                                PrimaUnica = primaUni * Convert.ToDouble(NArchivo.codTipCambio);
                                
                            }
                            else
                            {
                                PrimaUnica = primaUni;
                            }
                            PrimerTramo = Convert.ToDouble(sumaPension);
                            SegundoTramo = ((Convert.ToDouble(sumaPension) * Convert.ToInt32(datos.rentaEsc)) / 100);
                            
                            AniosDif = Convert.ToInt32(datos.mes_esc);
                            rentEsc = Convert.ToInt32(datos.rentaEsc);
                            break;
                        }
                }

                for (int i = 0; i < rutinaOficiales[0].Count; i++)
                {
                    int codrea = 0;
                    if (datos.moneda == "S/.Aj." || datos.moneda == "US$Aj.")
                    {
                        codrea = 2;
                    }
                    else if (datos.moneda == "S/.")
                    {
                        codrea = 1;
                    }

                    //double primafp = AniosDif * PrimerTramo;
                    //if (NArchivo.codTipRen == "6")
                    //{
                    //    primafp = 0;
                    //}
                    //PrimaUnica = datos.cic - primafp;

                    string MTO_VALREAJUSTEMEN = codrea == 2 ? "0.16515813" : "0";
                    string MTO_VALREAJUSTETRI = codrea == 2 ? "0.49629316" : "0";
                    string MTO_VALPREPENTMP = rutinaOficiales[0][i].MTO_RENTATMPAFP == 0 ? "0" : (rutinaOficiales[0][i].MTO_CTAINDAFP / rutinaOficiales[0][i].MTO_RENTATMPAFP).ToString("N2");

                    string[] dExepcionCalculo = { datos.moneda, datos.modalidad.ToString(), AniosDif.ToString(), datos.rentaTMP.ToString(),
                                    datos.perGarantizado, rentEsc.ToString(), PrimaUnica.ToString("N2"), /*sumaPension,*/ PrimerTramo.ToString("N2"),
                                    SegundoTramo.ToString("N2"),tasvtarut.ToString()/*_ExcepcionesRepository.nuevaVT.ToString()*/, rutinaOficiales[0][i].PRC_TASATIR.ToString(),
                                    rutinaOficiales[0][i].PRC_PERCON.ToString(), _ExcepcionesRepository.nuevaCO.ToString(), "", mtoPen.ToString(),"",datos.cod_tipreajuste.ToString(),
                                    //lo new
                                    rutinaOficiales[0][i].MTO_AJUSTEIPC.ToString(),rutinaOficiales[0][i].MTO_CTAINDAFP.ToString(),rutinaOficiales[0][i].MTO_RENTATMPAFP.ToString(),
                                    rutinaOficiales[0][i].MTO_RESMAT.ToString(),rutinaOficiales[0][i].PRC_TASATCE.ToString(),
                                    rutinaOficiales[0][i].FecCal, rutinaOficiales[0][i].MTO_PENANUAL.ToString(), rutinaOficiales[0][i].MTO_PENSIONGAR.ToString(), rutinaOficiales[0][i].MTO_PRIUNISIM.ToString(),
                                    rutinaOficiales[0][i].MTO_RMGTOSEP.ToString(), rutinaOficiales[0][i].MTO_RMGTOSEPRV.ToString(),MTO_VALREAJUSTEMEN,MTO_VALREAJUSTETRI,MTO_VALPREPENTMP};

                    List<string[]> datosCalculo = new List<string[]>();

                    datosCalculo.Add(dExepcionCalculo);

                    res.Object = datosCalculo;
                }

                res.Message = "Calculo realizado con exito.";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }
        public async Task<Response> mejoras(string nuevaTV, string nuevaCo, Exceptiones datos)
        {
            Response res = new Response();
            try
            {
                string bandera = "";
                //string banderaCom = "";

                _ExcepcionesRepository.numArchivo = datos.numArchivo;
                if (nuevaCo == "")
                {
                    _ExcepcionesRepository.nuevaCO = datos.comision;
                }
                else
                {
                    _ExcepcionesRepository.nuevaCO = Convert.ToDouble(nuevaCo);
                    //if (Convert.ToDouble(nuevaCo) > 2.4)
                    //{
                    //    banderaCom = "TIRANT";
                    //}

                }
                if (nuevaTV == "")
                {
                    bandera = "ant";
                    _ExcepcionesRepository.nuevaVT = datos.tasaVenta;
                }
                else
                {
                    _ExcepcionesRepository.nuevaVT = Convert.ToDouble(nuevaTV);
                }
                _ExcepcionesRepository.numCot = datos.numCot;
                _ExcepcionesRepository.numCorrelativo = datos.numCorrelativo;

                List<List<beResultados>> rutinaOficiales = new List<List<beResultados>>();
                List<beMortalidadDin> LisTabDin = new List<beMortalidadDin>();
                List<beMortalidadDinDet> LisTabMD = new List<beMortalidadDinDet>();
                List<beDatosModalidad> VarMTGS = new List<beDatosModalidad>();
                List<beDatosModalidad> VarAFP = new List<beDatosModalidad>();
                List<beDatosModalidad> VarREG = new List<beDatosModalidad>();
                List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();
                List<beTasaAnclaje> ListaTA = new List<beTasaAnclaje>();
                List<beCPK> ListaCPK = new List<beCPK>();
                List<beRentabilidad> ListaRen = new List<beRentabilidad>();
                List<beTasasPromedio> ListaTasProm = new List<beTasasPromedio>();
                List<beCurvaTasas> ListaCurvaTasas = new List<beCurvaTasas>();

                LisTabDin = _rutinaOficialesRepository.ConsultaTablaMortalidadDinamicas("");
                LisTabMD = _rutinaOficialesRepository.ConsultaDetTablaMortalidadDin();
                VarMTGS = _rutinaOficialesRepository.ConsultaGastoSepelioMes("");
                VarAFP = _rutinaOficialesRepository.ConsultaRentabilidadAfp("");
                VarREG = _rutinaOficialesRepository.ConsultaRegionTasas(0);
                LisTabPL = _rutinaOficialesRepository.ConsultaPorcentaje("");
                ListaTA = _rutinaOficialesRepository.ConsultaTasaAnclaje("");
                ListaCPK = _rutinaOficialesRepository.ConsultaCPKS("");
                ListaRen = _rutinaOficialesRepository.ConsultaRentabilidad("");
                ListaTasProm = _rutinaOficialesRepository.ConsultaTasasPromedio("");
                ListaCurvaTasas = _rutinaOficialesRepository.ConsultaCurvaTasas("");
                datos.asesor = _ExcepcionesRepository.ConsultaAsesor(datos.numOperacion.ToString());

                Console.WriteLine("Memory used before collection:       {0:N0}",
                GC.GetTotalMemory(false));

                List<beResultados> rutina = await _pruebasRutinaOficiales.RutinaOficiales(datos.numOperacion, datos.numCot, datos.numArchivo, LisTabDin, LisTabMD, VarMTGS, VarAFP, VarREG, LisTabPL, ListaTA, ListaCPK, ListaRen, ListaTasProm, ListaCurvaTasas, _ExcepcionesRepository, bandera,Convert.ToInt32(datos.asesor));

                string mensaje = (from r in rutina where r.Mensaje != null select r.Mensaje).FirstOrDefault();
                if (mensaje != null)
                {
                    res.IsOk = false;
                    res.Message = mensaje;

                    return res;
                }

                rutinaOficiales.Add(rutina);

                // Collect all generations of memory.
                GC.Collect();
                Console.WriteLine("Memory used after full collection:   {0:N0}",
                                    GC.GetTotalMemory(true));
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect(2, GCCollectionMode.Forced, true, true);
                double mtoPen = 0;
                double primaUni = 0;
                double tasvtarut = 0;
                int valSiscoRutina = 0;
                DataTable rutinaOf = new DataTable();
                rutinaOf.Columns.Add("MTO_AJUSTEIPC", typeof(double));
                rutinaOf.Columns.Add("MTO_CTAINDAFP", typeof(double));
                rutinaOf.Columns.Add("MTO_PENSION", typeof(double));
                rutinaOf.Columns.Add("MTO_PRIUNIDIF", typeof(double));
                rutinaOf.Columns.Add("MTO_RENTATMPAFP", typeof(double));
                rutinaOf.Columns.Add("MTO_RESMAT", typeof(double));
                rutinaOf.Columns.Add("NUM_CORRELATIVO", typeof(int));
                rutinaOf.Columns.Add("NUM_COT", typeof(string));
                rutinaOf.Columns.Add("PRC_PERCON", typeof(double));
                rutinaOf.Columns.Add("PRC_TASATCE", typeof(double));
                rutinaOf.Columns.Add("PRC_TASATIR", typeof(double));
                rutinaOf.Columns.Add("PRC_TASAVTA", typeof(double));
                rutinaOf.Columns.Add("COD_RECHAZO", typeof(string));
                rutinaOf.Columns.Add("ERR_DESCRIP", typeof(string));
                rutinaOf.Columns.Add("FEC_CALCULO", typeof(string));
                rutinaOf.Columns.Add("PRC_CORCOM", typeof(string));
                rutinaOf.Columns.Add("IND_SISCO", typeof(string));
                rutinaOf.Columns.Add("IND_FILTROCOTIZA", typeof(string));
                rutinaOf.Columns.Add("COD_ESTCOT", typeof(string));
                //rutinaOf.Columns.Add("MTO_VALPREPENTMP", typeof(string));


                rutinaOf.Columns.Add("MTO_PENANUAL", typeof(double));
                rutinaOf.Columns.Add("MTO_PENSIONGAR", typeof(double));
                rutinaOf.Columns.Add("MTO_PRIUNISIM", typeof(double));
                rutinaOf.Columns.Add("MTO_RMGTOSEP", typeof(string));
                rutinaOf.Columns.Add("MTO_RMGTOSEPRV", typeof(string));
                rutinaOf.Columns.Add("MTO_VALREAJUSTEMEN", typeof(string));
                rutinaOf.Columns.Add("MTO_VALREAJUSTETRI", typeof(string));


                foreach (var cotizacion in rutinaOficiales)
                {
                    foreach (var modalidad in cotizacion)
                    {
                        DataRow row = rutinaOf.NewRow();
                        row["MTO_AJUSTEIPC"] = modalidad.MTO_AJUSTEIPC;
                        row["MTO_CTAINDAFP"] = modalidad.MTO_CTAINDAFP;
                        row["MTO_PENSION"] = modalidad.MTO_PENSION;
                        row["MTO_PRIUNIDIF"] = modalidad.MTO_PRIUNIDIF;
                        row["MTO_RENTATMPAFP"] = modalidad.MTO_RENTATMPAFP;
                        row["MTO_RESMAT"] = modalidad.MTO_RESMAT;
                        row["NUM_CORRELATIVO"] = modalidad.NUM_CORRELATIVO;
                        row["NUM_COT"] = modalidad.NUM_COTESTUDIO;
                        row["PRC_PERCON"] = modalidad.PRC_PERCON;
                        row["PRC_TASATCE"] = modalidad.PRC_TASATCE;
                        row["PRC_TASATIR"] = modalidad.PRC_TASATIR;
                        row["PRC_TASAVTA"] = modalidad.PRC_TASAVTA;
                        row["COD_RECHAZO"] = modalidad.Cod_Rechazo;
                        row["FEC_CALCULO"] = modalidad.FecCal;
                        row["PRC_CORCOM"] = _ExcepcionesRepository.nuevaCO;
                        row["IND_SISCO"] = "S";
                        if (modalidad.Cod_Rechazo != "0")
                        {
                            row["IND_FILTROCOTIZA"] = modalidad.INDFILTROCOTIZA;
                        }
                        else
                        {
                            row["IND_FILTROCOTIZA"] = "S";
                        }
                        row["COD_ESTCOT"] = "S";
                        //row["MTO_VALPREPENTMP"] = 


                        row["MTO_PENANUAL"] = modalidad.MTO_PENANUAL;
                        row["MTO_PENSIONGAR"] = modalidad.MTO_PENSIONGAR;
                        row["MTO_PRIUNISIM"] = modalidad.MTO_PRIUNISIM;
                        row["MTO_RMGTOSEP"] = modalidad.MTO_RMGTOSEP;
                        row["MTO_RMGTOSEPRV"] = modalidad.MTO_RMGTOSEPRV;
                        row["MTO_VALREAJUSTEMEN"] = modalidad.MTO_VALREAJUSTEMEN;
                        row["MTO_VALREAJUSTETRI"] = modalidad.MTO_VALREAJUSTETRI;

                        rutinaOf.Rows.Add(row);
                        mtoPen = modalidad.MTO_PENSION;
                        primaUni = modalidad.MTO_PRIUNIDIF;
                        tasvtarut = modalidad.PRC_TASAVTA;
                        valSiscoRutina = modalidad.IND_SISCO;
                        
                    }

                }

                CalculoCotizacionRepository _calculoCotizacionRepository = new CalculoCotizacionRepository();
                SISCORepository _SISCORepository = new SISCORepository();
                double sumaPension = mtoPen;

                Exceptiones NArchivo = _ExcepcionesRepository.obtenerNA(datos.numOperacion.ToString(), datos.numCorrelativo.ToString());

                double PensionR = mtoPen;
                List<SISCO> CotSISCO = new List<SISCO>();
                string montoPenSISCO = "";
                string montoTasV = "";
                SISCO valSISCO = new SISCO();


                decimal suma = 0;
                double PrimaUnica = 0;
                double PrimerTramo = 0;
                double SegundoTramo = 0;
                double tasaV = 0;
                int AniosDif = 0;
                int rentEsc = 0;
                var listBeneficiarios = _calculoCotizacionRepository.getBeneficiarios(NArchivo.numArchivo);
               
                    valSISCO = _SISCORepository.ConsultaSISCO(datos.numOperacion.ToString(), datos.cuspp);

                    tasaV = _ExcepcionesRepository.nuevaVT;
                if (valSiscoRutina == 0)
                {
                    switch (NArchivo.codTP)
                    {
                        case "04":
                            sumaPension = mtoPen;
                            break;
                        case "05":
                            sumaPension = mtoPen;
                            break;
                        case "06":
                            if (NArchivo.Ind_Cob == "S")
                                sumaPension = Convert.ToDouble(mtoPen) * (0.7);
                            else
                                sumaPension = Convert.ToDouble(mtoPen);
                            break;
                        case "07":
                            if (NArchivo.Ind_Cob == "S")
                                sumaPension = Convert.ToDouble(mtoPen) * (0.5);
                            else
                                sumaPension = Convert.ToDouble(mtoPen);
                            break;
                        case "08":
                            for (int b = 0; b < listBeneficiarios.Count; b++)
                            {
                                if (listBeneficiarios[b].Num_Corr == datos.numCorrelativo && listBeneficiarios[b].Num_Operacion == datos.numOperacion)
                                {
                                    suma = suma + Convert.ToDecimal(mtoPen) * (listBeneficiarios[b].prc_Pension / 100);
                                    sumaPension = Convert.ToDouble(suma);
                                }
                            }
                            break;
                    }
                }
                  
                    XmlConfigurator.Configure();
                    switch (NArchivo.codTipRen)
                    {

                        case "1":
                            {
                                _log.Info("Moneda(Mejoras) " + datos.moneda);
                                _log.Info("Prima Unica(Mejoras) " + primaUni);
                                if (datos.moneda == "S/.Aj." || datos.moneda == "S/.")
                                {

                                    PrimaUnica = primaUni;
                                }
                                else
                                {
                                    PrimaUnica = primaUni * Convert.ToDouble(NArchivo.codTipCambio);
                                }

                                SegundoTramo = Convert.ToDouble(sumaPension);
                                tasaV = tasvtarut;
                                PrimerTramo = 0;

                                AniosDif = Convert.ToInt32(datos.periodoDiferido);
                                rentEsc = Convert.ToInt32(datos.rentaEsc);
                                break;
                            }
                        case "2":
                            {
                                if (datos.moneda == "S/.Aj." || datos.moneda == "S/.")
                                {
                                    PrimerTramo = (Convert.ToDouble(sumaPension) * 2);
                                    PrimaUnica = primaUni;
                                }
                                else
                                {
                                    PrimerTramo = (Convert.ToDouble(sumaPension) * 2) * Convert.ToDouble(NArchivo.codTipCambio);
                                    PrimaUnica = primaUni * Convert.ToDouble(NArchivo.codTipCambio);
                                }
                                SegundoTramo = Convert.ToDouble(sumaPension);
                                AniosDif = Convert.ToInt32(datos.periodoDiferido);
                                rentEsc = Convert.ToInt32(datos.rentaEsc);
                                break;
                            }

                        case "6":
                            {
                                if (datos.moneda == "S/.Aj." || datos.moneda == "S/.")
                                {
                                    PrimaUnica = primaUni;
                                }
                                else
                                {
                                    PrimaUnica = primaUni * Convert.ToDouble(NArchivo.codTipCambio);
                                }

                               

                                PrimerTramo = Convert.ToDouble(sumaPension);
                                SegundoTramo = ((Convert.ToDouble(sumaPension) * Convert.ToInt32(datos.rentaEsc)) / 100);

                                AniosDif = Convert.ToInt32(datos.periodoDiferido);
                                rentEsc = Convert.ToInt32(datos.rentaEsc);
                                break;
                            }
                    }

                    for (int i = 0; i < rutinaOficiales[0].Count; i++)
                    {
                        int codrea = 0;
                        if (datos.moneda == "S/.Aj." || datos.moneda == "US$Aj.")
                        {
                            codrea = 2;
                        }
                        else if (datos.moneda == "S/.")
                        {
                            codrea = 1;
                        }


                       
                        string MTO_VALREAJUSTEMEN = codrea == 2 ? "0.16515813" : "0";
                        string MTO_VALREAJUSTETRI = codrea == 2 ? "0.49629316" : "0";
                        string MTO_VALPREPENTMP = rutinaOficiales[0][i].MTO_RENTATMPAFP == 0 ? "0" : (rutinaOficiales[0][i].MTO_CTAINDAFP / rutinaOficiales[0][i].MTO_RENTATMPAFP).ToString("N2");

                        string[] dMEjoraCalculo = { datos.moneda, datos.modalidad.ToString(), AniosDif.ToString(), datos.rentaTMP.ToString(),
                                    datos.perGarantizado, rentEsc.ToString(), PrimaUnica.ToString("N2")/*primaUni.ToString("N2")*/, /*sumaPension,*/ PrimerTramo.ToString("N2"),
                                    SegundoTramo.ToString("N2"),tasvtarut.ToString()/*_ExcepcionesRepository.nuevaVT.ToString()*/, rutinaOficiales[0][i].PRC_TASATIR.ToString(),
                                    rutinaOficiales[0][i].PRC_PERCON.ToString(), _ExcepcionesRepository.nuevaCO.ToString(), "", mtoPen.ToString(),"",datos.cod_tipreajuste.ToString(),
                                    //lo new
                                    rutinaOficiales[0][i].MTO_AJUSTEIPC.ToString(),rutinaOficiales[0][i].MTO_CTAINDAFP.ToString(),rutinaOficiales[0][i].MTO_RENTATMPAFP.ToString(),
                                    rutinaOficiales[0][i].MTO_RESMAT.ToString(),rutinaOficiales[0][i].PRC_TASATCE.ToString(),
                                    rutinaOficiales[0][i].FecCal, rutinaOficiales[0][i].MTO_PENANUAL.ToString(), rutinaOficiales[0][i].MTO_PENSIONGAR.ToString(), rutinaOficiales[0][i].MTO_PRIUNISIM.ToString(),
                                    rutinaOficiales[0][i].MTO_RMGTOSEP.ToString(), rutinaOficiales[0][i].MTO_RMGTOSEPRV.ToString(),MTO_VALREAJUSTEMEN,MTO_VALREAJUSTETRI,MTO_VALPREPENTMP};

                        List<string[]> datosCalculo = new List<string[]>();

                        datosCalculo.Add(dMEjoraCalculo);

                        res.Object = datosCalculo;
                    }

                    res.Message = "Calculo realizado con exito.";
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
        public Response cancelar(string numCor, string numOperacion, string periodoDiferido, string primerTramo)
        {
            Response res = new Response();
            try
            {
                return _ExcepcionesRepository.cancelar(numCor, numOperacion, periodoDiferido,  primerTramo);
            }
            catch
            {
                throw;
            }
        }

    }
}
