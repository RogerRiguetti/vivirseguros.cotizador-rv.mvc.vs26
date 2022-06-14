using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Estudio.Logic
{
    public class GenArMelerLogic
    {
        public GenArMelerRepository _GenerarXML = new GenArMelerRepository();
        ExportarExcelRepository _exportarExcelRepository = new ExportarExcelRepository();
        private static bool bandXML = false;
        string queryFinal = "";
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Response getNumArchivos()
        {
            GenArMelerRepository _GenArMelerLogic = new GenArMelerRepository();
            try
            {
                Response res = new Response();

                var Datos = _GenArMelerLogic.getNumArchivos();

                res.Object = Datos;
                res.Message = "";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }
        public Response actualizarGrilla()
        {
            GenArMelerRepository _GenArMelerLogic = new GenArMelerRepository();
            try
            {
                Response res = new Response();

                var Datos = _GenArMelerLogic.actualizarGrilla();

                res.Object = Datos;
                res.Message = "";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }
        public Response BuscarNumerosArchivo(string fecha)
        {
            GenArMelerRepository _GenArMelerLogic = new GenArMelerRepository();
            try
            {
                Response res = new Response();
                //ManFecAceptacionCotizacion Datos = new ManFecAceptacionCotizacion();

                var Datos = _GenArMelerLogic.getNumArchivosB(fecha);

                res.Object = Datos;
                res.Message = "";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }
        public Response BuscarArchivo(string Fec, string numArchivo, string caso)
        {
            GenArMelerRepository _GenArMelerLogic = new GenArMelerRepository();
            try
            {
                Response res = new Response();
                //ManFecAceptacionCotizacion Datos = new ManFecAceptacionCotizacion();

                var Datos = _GenArMelerLogic.Busca_Informacion(Fec, numArchivo, caso);

                res.Object = Datos;
                res.Message = "";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }
        public Response GenerarXML(List<GenArMeler> informacion, List<GenArMeler> NoCotiza, string bandWS)
        {
            try
            {
                Response res = new Response();
                string querysSINO = "";
                //_GenerarXML.ActualizaNoVig(); //Actualiza las cotizaciones(modalidades) con fecha cierre menor a fecha actual

                XmlDocument docXML = new XmlDocument();
                XmlDeclaration xmlDeclaration = docXML.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = docXML.DocumentElement;
                docXML.InsertBefore(xmlDeclaration, root);
                XmlElement cargaCotizaciones = docXML.CreateElement(string.Empty, "cargaCotizaciones", string.Empty);
                XmlAttribute atr = docXML.CreateAttribute("xmlns:xsi");
                atr.Value = "http://www.w3.org/2001/XMLSchema-instance";
                cargaCotizaciones.Attributes.SetNamedItem(atr);
                docXML.AppendChild(cargaCotizaciones);
                string[] cotizacionesI = new string[informacion.Count];
                int numArchSal = _GenerarXML.ultimoNarchivoSal(); //Obtiene el ultimo el ultimo nº de archivo de la tabla de salida
                _GenerarXML.creaRegistroSalida(informacion[0], numArchSal); //Crea un registro de salida
                queryFinal = "";
                XmlConfigurator.Configure();
                _log.Info("Comenzara el primer Update del número de archivo" + informacion[0].numArch);
                _GenerarXML.UpdateDetCotizacion1(informacion[0].numArch);
                _log.Info("Se ejecuto correctamente UPDATE MtoPri");
                for (int i = 0; i < informacion.Count; i++)
                {
                    queryFinal += "\n" + _GenerarXML.llenarTablas(informacion[i], numArchSal);  //Llena las tablas de salida(Genera nro de archivo de salida)

                    //if (bandWS != "W")
                    //{
                    //    queryFinal += "\n" + _GenerarXML.Actualiza_EstEnv(informacion[i]); //Actualiza estado de Cotizaciones a Enviadas
                    //}
                    cotizacionesI[i] = informacion[i].numCot;
                }

                _log.Info("Se ejecutara el script "+ queryFinal);
                _GenerarXML.EjecutarScript(queryFinal);

                string[] cotizaciones = _GenerarXML.getNumCotizaciones(informacion[0].numArch);
                for (int i = 0; i < cotizacionesI.Length; i++)
                {
                    for (int j = 0; j < cotizaciones.Length; j++)
                    {
                        //Genera el archivo XML
                        if (cotizaciones[j] == cotizacionesI[i])
                        {
                            querysSINO += XMLMelerdoc(docXML, informacion[0].numArch, cargaCotizaciones, cotizaciones[j], NoCotiza, bandWS);
                            cotizaciones[j] = "Listo";
                        }
                    }
                }

                queryFinal = "";
                for (int i = 0; i < informacion.Count; i++)
                {
                    if (bandWS != "W")
                    {
                        queryFinal += "\n" + _GenerarXML.Actualiza_EstEnv(informacion[i]); //Actualiza estado de Cotizaciones a Enviadas
                    }
                }
                if (queryFinal != "")
                {
                    _log.Info("Se ejecutara el script del estado codestcot" + queryFinal);
                    _GenerarXML.EjecutarScript(queryFinal);
                }

                _log.Info("Se ejecutara el scriptSINO " + querysSINO);
                _GenerarXML.ActualizaIndCotizaEjecutar(querysSINO);

                //if (bandWS != "W")
                //{
                    _log.Info("Se ejecutara el UPDATEDETCOT ");
                    _GenerarXML.UpdateDetCotizacion(informacion[0].numArch);
                //}

                validarXML(docXML.InnerXml); //Valida schema xml
                if (bandXML == false)
                {
                    res.Message = "No se generó ninguna cotización";
                    res.IsOk = false;
                    return res;
                }
                XmlConfigurator.Configure();
                _log.Info("Reversion de envio a MELER");
                if (bandWS == "W")
                {
                    EliminarCargasRepository _eliCargasRep = new EliminarCargasRepository();
                    _eliCargasRep.eliminar("ENVIOMELER", informacion[0].numArch.ToString(), numArchSal.ToString());
                    _log.Info("Eliminacion correcta de envio a MELER");
                }
                res.Message = docXML.InnerXml.ToString();
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = "Operación cancelada: "+ex.Message;
                return res2;
            }
        }
        public string XMLMelerdoc(XmlDocument docXML, int strNumArch, XmlElement cargaCotizaciones, string numCot, List<GenArMeler> NoCotiza, string bandWS)
        {
            try
            {
                string querysSINO = "";
                double MtoPrima = 0, MtoMoneda = 0, MtoPrimaDif = 0, MtoPensionRT = 0, MtoPensionRT_RtaEsc = 0, MtoPrimaAFPEESS = 0, Prc_RtaEsc = 0;
                double MtoPensionRT_Aux, MtoTasaIntRT_Aux;
                double MtoPensionRVD_Aux, MtoTasaIntRVD_Aux;
                double MtoTasaParCap_Aux, MtoPriUniAFPEESS_Aux, MtoPriUniEESS_Aux;
                GenArMeler valoresRMB = _GenerarXML.getValoresRMB(); //Obtiene los valores para RM y RB (renta mixta y bimoneda)
                MtoPensionRT_Aux = valoresRMB.mtoPension;
                MtoTasaIntRT_Aux = valoresRMB.prcTasaRT;
                MtoPensionRVD_Aux = valoresRMB.mtoPensionRVD;
                MtoTasaIntRVD_Aux = valoresRMB.prcTasaRVD;
                GenArMeler datosThisSalida = _GenerarXML.datosThisSalida(numCot);
                if (datosThisSalida != null)
                {
                    XmlElement cotizaciones = docXML.CreateElement(string.Empty, "cotizaciones", string.Empty);
                    crearElementoXML(docXML, cargaCotizaciones, "", cotizaciones, null);

                    XmlElement numOperacion = docXML.CreateElement(string.Empty, "nroOperacion", string.Empty);
                    crearElementoXML(docXML, null, datosThisSalida.numOperacion.ToString(), numOperacion, cotizaciones);

                    XmlElement codCuspp = docXML.CreateElement(string.Empty, "CUSPP", string.Empty);
                    crearElementoXML(docXML, null, datosThisSalida.codCUSPP.ToString(), codCuspp, cotizaciones);

                    List<GenArMeler> prodsCots = _GenerarXML.getProductosCotizados(datosThisSalida.numCot, "TR"); //obtener los productos cotizados de esa cotizacion
                    for (int i = 0; i < prodsCots.Count; i++)
                    {
                        XmlElement productoCotizado = docXML.CreateElement(string.Empty, "productoCotizado", string.Empty);
                        crearElementoXML(docXML, cotizaciones, "", productoCotizado, null); //Inicio Producto Cotizado

                        XmlElement modalidad = docXML.CreateElement(string.Empty, "modalidad", string.Empty);
                        crearElementoXML(docXML, null, prodsCots[i].codTipRen, modalidad, productoCotizado);
                        //Moneda
                        if (prodsCots[i].codTipRen != "RB") //Cuando es una renta bimoneda no informa la moneda
                        {
                            XmlElement moneda = docXML.CreateElement(string.Empty, "moneda", string.Empty);
                            crearElementoXML(docXML, null, prodsCots[i].codMoneda, moneda, productoCotizado);
                        }
                        else //Cuando se trate de una renta Bimoneda, primero se debe consultar si tiene reajuste
                        {    //ya que para estos casos si se debe informar la moneda reajustada
                            if (prodsCots[i].codTipReajuste != "0")
                            {
                                XmlElement moneda = docXML.CreateElement(string.Empty, "moneda", string.Empty);
                                crearElementoXML(docXML, null, prodsCots[i].codMoneda, moneda, productoCotizado);
                            }
                        }
                        //SI ES RENTA VITALICIA DIFERIDA
                        if (prodsCots[i].numMesDif > 0)
                        {
                            XmlElement anosRT = docXML.CreateElement(string.Empty, "anosRT", string.Empty);
                            crearElementoXML(docXML, null, (prodsCots[i].numMesDif / 12).ToString(), anosRT, productoCotizado); //En años
                            XmlElement porcentajeRVD = docXML.CreateElement(string.Empty, "porcentajeRVD", string.Empty);
                            crearElementoXML(docXML, null, (prodsCots[i].prcRentaTmp).ToString(), porcentajeRVD, productoCotizado); //prc_rentatmp
                        }
                        //SI ES RENTA ESCALONADA
                        if (prodsCots[i].numMesesC > 0)
                        {
                            XmlElement anosRT = docXML.CreateElement(string.Empty, "anosRT", string.Empty);
                            crearElementoXML(docXML, null, (prodsCots[i].numMesesC / 12).ToString(), anosRT, productoCotizado); //En años
                            XmlElement porcentajeRVD = docXML.CreateElement(string.Empty, "porcentajeRVD", string.Empty);
                            crearElementoXML(docXML, null, (prodsCots[i].prcRentaEsc).ToString(), porcentajeRVD, productoCotizado); //prc_rentaesc
                        }
                        //SI ES GARANTIZADA
                        if (prodsCots[i].numMesGar > 0)
                        {
                            XmlElement periodoGarantizado = docXML.CreateElement(string.Empty, "periodoGarantizado", string.Empty);
                            crearElementoXML(docXML, null, (prodsCots[i].numMesGar / 12).ToString(), periodoGarantizado, productoCotizado); //En años
                        }
                        //NUMERO DE AÑOS DEL PERIODO GARANTIZADO
                        if (prodsCots[i].codCoberCon != "0")
                        {
                            XmlElement coberturaConyuge = docXML.CreateElement(string.Empty, "coberturaConyuge", string.Empty);
                            crearElementoXML(docXML, null, prodsCots[i].codCoberCon, coberturaConyuge, productoCotizado);
                        }
                        XmlElement derechoCrecer = docXML.CreateElement(string.Empty, "derechoCrecer", string.Empty);
                        crearElementoXML(docXML, null, prodsCots[i].codDerCre, derechoCrecer, productoCotizado);
                        XmlElement gratificacion = docXML.CreateElement(string.Empty, "gratificacion", string.Empty);
                        crearElementoXML(docXML, null, prodsCots[i].codDerGra, gratificacion, productoCotizado);
                        //Se debe informar la "particionCapital" para Rta Mixta, Rta Bimoneda y Rta Combinada
                        if (prodsCots[i].codTipRen == "RB" || prodsCots[i].codTipRen == "RM" || prodsCots[i].codTipRen == "RC")
                        {
                            if (prodsCots[i].codParCap != "")
                            {
                                XmlElement particionCapital = docXML.CreateElement(string.Empty, "particionCapital", string.Empty);
                                crearElementoXML(docXML, null, prodsCots[i].codParCap, particionCapital, productoCotizado);
                            }
                        }
                        XmlElement cotizacionEESS = docXML.CreateElement(string.Empty, "cotizacionEESS", string.Empty);
                        crearElementoXML(docXML, productoCotizado, "", cotizacionEESS, null); //Inicio EESS

                        bool banS = false;
                        string siNo = "";

                        if (bandWS == "W")
                        {
                            switch (prodsCots[i].indFiltroCotiza)
                            {
                                case "N":
                                    XmlElement siCotizaNoCotiza = docXML.CreateElement(string.Empty, "siCotizaNoCotiza", string.Empty);
                                    crearElementoXML(docXML, null, "N", siCotizaNoCotiza, cotizacionEESS); //'S/N
                                    siNo = "N";
                                    banS = true;
                                    break;
                                case "S":
                                    banS = false;
                                    break;
                            }
                        }
                        else
                        {
                            int numCotiza = NoCotiza == null ? 0 : NoCotiza.Count;
                            //if (numCotiza == 0)
                            //{
                            //    XmlElement siCotizaNoCotiza = docXML.CreateElement(string.Empty, "siCotizaNoCotiza", string.Empty);
                            //    crearElementoXML(docXML, null, "S", siCotizaNoCotiza, cotizacionEESS); //'S/N
                            //    siNo = "S";
                            //    banS = false;
                            //}
                            //else
                            //{
                                for (int n = 0; n < numCotiza; n++)
                                {
                                    if (NoCotiza[n].Correlativo == prodsCots[i].Correlativo && NoCotiza[n].numOperacion == prodsCots[i].numOperacion && NoCotiza[n].numArch == prodsCots[i].numArch)
                                    {
                                        XmlElement siCotizaNoCotiza = docXML.CreateElement(string.Empty, "siCotizaNoCotiza", string.Empty);
                                        crearElementoXML(docXML, null, "N", siCotizaNoCotiza, cotizacionEESS); //'S/N
                                        siNo = "N";
                                        banS = true;
                                        break;
                                    }
                                    else
                                    {
                                        banS = false;
                                    }
                                }
                            //}
                        }
                        if (banS == false)
                        {
                            XmlElement siCotizaNoCotiza = docXML.CreateElement(string.Empty, "siCotizaNoCotiza", string.Empty);
                            crearElementoXML(docXML, null, "S", siCotizaNoCotiza, cotizacionEESS); //'S/N
                            siNo = "S";
                            //TIPO RENTA NO CALCULADAS: SI ES RENTA BIMONEDA, MIXTA O COMBINADA Y TIENE AL MENOS 1 PRODUCTO CALCULADO
                            if ((prodsCots[i].codEstCot != "") || ((prodsCots[i].codTipRen == "RB" || prodsCots[i].codTipRen == "RM" || prodsCots[i].codTipRen == "RC") && prodsCots[i].numCalculados > 0))
                            {
                                /*XmlElement siCotizaNoCotiza = docXML.CreateElement(string.Empty, "siCotizaNoCotiza", string.Empty);
                                crearElementoXML(docXML, null, "S", siCotizaNoCotiza, cotizacionEESS); //S/N*/
                                XmlElement nroCotizacion = docXML.CreateElement(string.Empty, "nroCotizacion", string.Empty);
                                crearElementoXML(docXML, null, prodsCots[i].numCot, nroCotizacion, cotizacionEESS);
                                //SE AGREGA COMBINADA Y ESCALONADA
                                if (prodsCots[i].codTipRen == "RB" || prodsCots[i].codTipRen == "RM" || prodsCots[i].codTipRen == "RC" || prodsCots[i].codTipRen == "RVE")
                                {
                                    //Informa las Escalonadas y Combinada, de acuerdo a documento 
                                    if (prodsCots[i].codTipRen == "RVE")
                                    {
                                        //********** SI ES RENTA ESCALONADA:**********
                                        MtoPrima = prodsCots[i].mtoPriUni;
                                        if (prodsCots[i].moneda == "NS")
                                        {
                                            MtoPensionRT = prodsCots[i].mtoPension;
                                        }
                                        else
                                        {
                                            MtoPensionRT = prodsCots[i].mtoPension;// * prodsCots[i].codTipCambio;
                                        }
                                        Prc_RtaEsc = prodsCots[i].prcRentaEsc;
                                        XmlElement primaUnicaEESS = docXML.CreateElement(string.Empty, "primaUnicaEESS", string.Empty);
                                        crearElementoXML(docXML, null, MtoPrima.ToString("0.00"), primaUnicaEESS, cotizacionEESS); //Monto Prima Unica          *EN SOLES*
                                        XmlElement tasaInteresRV = docXML.CreateElement(string.Empty, "tasaInteresRV", string.Empty);
                                        crearElementoXML(docXML, null, prodsCots[i].prcTasaVTA.ToString(), tasaInteresRV, cotizacionEESS); //Valor de Tasa de Venta
                                                                                                                                           //REVISAR (PENSION 1ER TRAMO)
                                        XmlElement primeraPensionRT = docXML.CreateElement(string.Empty, "primeraPensionRT", string.Empty);
                                        crearElementoXML(docXML, null, MtoPensionRT.ToString("0.00"), primeraPensionRT, cotizacionEESS); //Monto Renta Temporal de la AFP 
                                                                                                                                   //REVISAR (PENSION 2DO TRAMO)
                                        MtoPensionRT_RtaEsc = Convert.ToDouble((MtoPensionRT * (Prc_RtaEsc / 100)).ToString("0.00"));
                                        XmlElement primeraPensionRVD = docXML.CreateElement(string.Empty, "primeraPensionRVD", string.Empty);
                                        crearElementoXML(docXML, null, MtoPensionRT_RtaEsc.ToString(), primeraPensionRVD, cotizacionEESS);
                                    }
                                    else
                                    {
                                        //********** SI ES RENTA BIMONEDA, RENTA MIXTA O RENTA COMBINADA:**********
                                        MtoPrima = prodsCots[i].mtoPriUni;
                                        MtoTasaParCap_Aux = prodsCots[i].prcParCapCia;
                                        MtoPriUniEESS_Aux = Convert.ToDouble((MtoPrima * (MtoTasaParCap_Aux / 100)).ToString("0.00"));
                                        MtoPriUniAFPEESS_Aux = Convert.ToDouble((MtoPrima - MtoPriUniEESS_Aux).ToString("0.00"));
                                        XmlElement primaUnicaAFPEESS = docXML.CreateElement(string.Empty, "primaUnicaAFPEESS", string.Empty);
                                        crearElementoXML(docXML, null, MtoPriUniAFPEESS_Aux.ToString(), primaUnicaAFPEESS, cotizacionEESS);
                                        XmlElement primaUnicaEESS = docXML.CreateElement(string.Empty, "primaUnicaEESS", string.Empty);
                                        crearElementoXML(docXML, null, MtoPriUniEESS_Aux.ToString(), primaUnicaEESS, cotizacionEESS);
                                        XmlElement primeraPensionRT = docXML.CreateElement(string.Empty, "primeraPensionRT", string.Empty);
                                        crearElementoXML(docXML, null, MtoPensionRT_Aux.ToString(), primeraPensionRT, cotizacionEESS); //Monto Renta Temporal de la AFP 
                                        XmlElement tasaInteresRT = docXML.CreateElement(string.Empty, "tasaInteresRT", string.Empty);
                                        crearElementoXML(docXML, null, MtoTasaIntRT_Aux.ToString(), tasaInteresRT, cotizacionEESS); //Valor de Tasa de Rentabilidad de la AFP
                                        XmlElement primeraPensionRVD = docXML.CreateElement(string.Empty, "primeraPensionRVD", string.Empty);
                                        crearElementoXML(docXML, null, MtoPensionRVD_Aux.ToString(), primeraPensionRVD, cotizacionEESS); //Pension
                                        XmlElement tasaInteresRVD = docXML.CreateElement(string.Empty, "tasaInteresRVD", string.Empty);
                                        crearElementoXML(docXML, null, MtoTasaIntRVD_Aux.ToString(), tasaInteresRVD, cotizacionEESS);
                                    }
                                }
                                else
                                {
                                    //**** SI ES RENTA VITALICIA O RENTA VITALICIA CON RENTA TEMPORAL:****
                                    MtoMoneda = prodsCots[i].mtoValMoneda;
                                    MtoPrima = prodsCots[i].mtoPriUni;
                                    if (prodsCots[i].moneda == "NS")
                                    {
                                        MtoPrimaDif = prodsCots[i].mtoPriuniDif;
                                        MtoPensionRT = prodsCots[i].mtoPension * 2;
                                    }
                                    else
                                    {
                                        MtoPrimaDif = Convert.ToDouble((prodsCots[i].mtoPriuniDif * MtoMoneda).ToString("0.0"));
                                        MtoPensionRT = (prodsCots[i].mtoPension * 2) * prodsCots[i].codTipCambio;
                                    }
                                     //Convert.ToDouble((prodsCots[i].mtoRentaTmpAfp * MtoMoneda).ToString("0.0"));
                                    if (prodsCots[i].numMesDif > 0) //Si es Diferida
                                    {
                                        MtoPrimaAFPEESS = Convert.ToDouble((MtoPrima - MtoPrimaDif).ToString("0.0"));
                                        XmlElement primaUnicaAFPEESS = docXML.CreateElement(string.Empty, "primaUnicaAFPEESS", string.Empty);
                                        crearElementoXML(docXML, null, MtoPrimaAFPEESS.ToString(), primaUnicaAFPEESS, cotizacionEESS);
                                        XmlElement primaUnicaEESS = docXML.CreateElement(string.Empty, "primaUnicaEESS", string.Empty);
                                        crearElementoXML(docXML, null, MtoPrimaDif.ToString(), primaUnicaEESS, cotizacionEESS); //Monto Pri Unica Diferida   *EN SOLES*
                                    }
                                    else
                                    {
                                        XmlElement primaUnicaEESS = docXML.CreateElement(string.Empty, "primaUnicaEESS", string.Empty);
                                        crearElementoXML(docXML, null, MtoPrima.ToString(), primaUnicaEESS, cotizacionEESS); //Monto Prima Unica          *EN SOLES*
                                    }
                                    if (prodsCots[i].codTipPension == "08")
                                    {
                                        if (prodsCots[i].numMesDif == 0)
                                        {
                                            XmlElement primeraPensionRV = docXML.CreateElement(string.Empty, "primeraPensionRV", string.Empty);
                                            crearElementoXML(docXML, null, prodsCots[i].mtoSumPension.ToString(), primeraPensionRV, cotizacionEESS); //Pension
                                            XmlElement tasaInteresRV = docXML.CreateElement(string.Empty, "tasaInteresRV", string.Empty);
                                            crearElementoXML(docXML, null, prodsCots[i].prcTasaVTA.ToString(), tasaInteresRV, cotizacionEESS); //Valor de Tasa de Venta
                                        }
                                        else  //Si es Diferida con Renta Temporal
                                        {
                                            XmlElement primeraPensionRT = docXML.CreateElement(string.Empty, "primeraPensionRT", string.Empty);
                                            crearElementoXML(docXML, null, MtoPensionRT.ToString("0.00"), primeraPensionRT, cotizacionEESS); //Monto Renta Temporal de la AFP
                                            XmlElement tasaInteresRT = docXML.CreateElement(string.Empty, "tasaInteresRT", string.Empty);
                                            crearElementoXML(docXML, null, prodsCots[i].prcRentaAFP.ToString(), tasaInteresRT, cotizacionEESS); //Valor de Tasa de Rentabilidad de la AFP
                                            XmlElement primeraPensionRVD = docXML.CreateElement(string.Empty, "primeraPensionRVD", string.Empty);
                                            crearElementoXML(docXML, null, prodsCots[i].mtoSumPension.ToString(), primeraPensionRVD, cotizacionEESS); //Pension
                                            XmlElement tasaInteresRVD = docXML.CreateElement(string.Empty, "tasaInteresRVD", string.Empty);
                                            crearElementoXML(docXML, null, prodsCots[i].prcTasaVTA.ToString(), tasaInteresRVD, cotizacionEESS); //Valor de Tasa de Venta
                                        }
                                    }
                                    else
                                    {
                                        if (prodsCots[i].numMesDif == 0)
                                        {
                                            XmlElement primeraPensionRV = docXML.CreateElement(string.Empty, "primeraPensionRV", string.Empty);
                                            crearElementoXML(docXML, null, prodsCots[i].mtoPension.ToString(), primeraPensionRV, cotizacionEESS); //Pension
                                            XmlElement tasaInteresRV = docXML.CreateElement(string.Empty, "tasaInteresRV", string.Empty);
                                            crearElementoXML(docXML, null, prodsCots[i].prcTasaVTA.ToString(), tasaInteresRV, cotizacionEESS); //Valor de Tasa de Venta
                                        }
                                        else   //Si es Diferida con Renta Temporal
                                        {
                                            XmlElement primeraPensionRT = docXML.CreateElement(string.Empty, "primeraPensionRT", string.Empty);
                                            crearElementoXML(docXML, null, MtoPensionRT.ToString("0.00"), primeraPensionRT, cotizacionEESS); //Monto Renta Temporal de la AFP
                                            XmlElement tasaInteresRT = docXML.CreateElement(string.Empty, "tasaInteresRT", string.Empty);
                                            crearElementoXML(docXML, null, prodsCots[i].prcRentaAFP.ToString(), tasaInteresRT, cotizacionEESS); //Valor de Tasa de Rentabilidad de la AFP
                                            XmlElement primeraPensionRVD = docXML.CreateElement(string.Empty, "primeraPensionRVD", string.Empty);
                                            crearElementoXML(docXML, null, prodsCots[i].mtoPension.ToString(), primeraPensionRVD, cotizacionEESS); //Pension
                                            XmlElement tasaInteresRVD = docXML.CreateElement(string.Empty, "tasaInteresRVD", string.Empty);
                                            crearElementoXML(docXML, null, prodsCots[i].prcTasaVTA.ToString(), tasaInteresRVD, cotizacionEESS); //Valor de Tasa de Venta
                                        }
                                    }
                                }
                            }
                        }
                        querysSINO += "\n" + _GenerarXML.ActualizaIndCotiza(strNumArch, numCot, prodsCots[i].Correlativo, siNo);
                    }
                }
                return querysSINO;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void crearElementoXML(XmlDocument docXML, XmlElement elementoPri, string txtNodo, XmlElement elementoNuevo, XmlElement elementoSec)
        {
            if (txtNodo == "")
            {
                elementoPri.AppendChild(elementoNuevo);
            }
            else
            {
                XmlText txtElemento = docXML.CreateTextNode(txtNodo);
                elementoNuevo.AppendChild(txtElemento);
                elementoSec.AppendChild(elementoNuevo);
            }
        }
        public static void validarXML(string doc)
        {
            string ruta = AppContext.BaseDirectory + @"Resources\handler\cargaCotizaciones23.xsd";
            StringReader sr = new StringReader(doc);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add("", ruta);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

            XmlReader reader = XmlReader.Create(sr, settings);

            bandXML = true;
            while (reader.Read()) ;
        }
        public static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            bandXML = false;
        }

        public List<List<Dictionary<string, object>>> ExportarExcelGenArch(string FecEnvio, int numArchivo)
        {
            return _exportarExcelRepository.ExportarExcelGenArch(FecEnvio, numArchivo);
        }
    }
}
