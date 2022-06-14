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
using Estudio.Repository;
using Estudio.Process;

namespace Estudio.Logic
{
    public class ActualizaInfOfiLogic
    {
        private ActualizaInfOfiRepository _ActualizaInfOfiRepository = new ActualizaInfOfiRepository();
        PruebasRutinaOficiales _pruebasRutinaOficiales = new PruebasRutinaOficiales();
        RutinaOficialesRepository _rutinaOficialesRepository = new RutinaOficialesRepository();
        LimiteCotizacionMejoradaRepository _LimiteCotizacionMejoradaRepository = new LimiteCotizacionMejoradaRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        CalcularAsignacionIntermediarioRepository _calcularAsignacionIntermediarioRepository = new CalcularAsignacionIntermediarioRepository();

        public Response busqueda(string codCUSPP, string numCor, string numOperacion)
        {
            Response res = new Response();

            List<bePorcenLegales> LisTabPLPar = new List<bePorcenLegales>();
            RutinaOficialesRepository _rutinaOficialesRepository = new RutinaOficialesRepository();
            try
            {
                var datosBusqueda = _ActualizaInfOfiRepository.busqueda(codCUSPP, numCor, numOperacion);

                #region Beneficiarios
                //Calculo de porcentajes para los beneficiarios
                LisTabPLPar = _rutinaOficialesRepository.ConsultaPorcentaje("");
                AsignacionIntermediario info = new AsignacionIntermediario();
                string query;
                if (numOperacion == "" && codCUSPP != "")
                {
                    query = "SELECT C.COD_TIPPENSION, C.FEC_DEV, CS.FEC_DEVSOL, C.IND_COB, C.NUM_ARCHIVO, C.NUM_OPERACION" +
                   " FROM PT_TMAE_COTIZACION C" +
                   " JOIN PT_THIS_CARGASOL CS ON CS.NUM_OPERACION = C.NUM_OPERACION" +
                   " WHERE C.COD_CUSPP = '" + codCUSPP + "'";
                }
                else if (numOperacion != "" && codCUSPP == "")
                {
                    query = "SELECT C.COD_TIPPENSION, C.FEC_DEV, CS.FEC_DEVSOL, C.IND_COB, C.NUM_ARCHIVO, C.NUM_OPERACION" +
                     " FROM PT_TMAE_COTIZACION C" +
                     " JOIN PT_THIS_CARGASOL CS ON CS.NUM_OPERACION = C.NUM_OPERACION" +
                     " WHERE C.num_operacion = " + numOperacion;
                }
                else
                {
                    query = "SELECT C.COD_TIPPENSION, C.FEC_DEV, CS.FEC_DEVSOL, C.IND_COB, C.NUM_ARCHIVO, C.NUM_OPERACION" +
                     " FROM PT_TMAE_COTIZACION C" +
                     " JOIN PT_THIS_CARGASOL CS ON CS.NUM_OPERACION = C.NUM_OPERACION" +
                     " WHERE C.num_operacion = " + numOperacion + " and c.COD_CUSPP = '" + codCUSPP + "'";
                }

                info = SRVDBContext<AsignacionIntermediario>.CallSelectStatement(query, x => new AsignacionIntermediario
                {
                    CodigoPension = x.GetString(0),
                    FechaDevengueStr = x.GetString(1),
                    FechaDevengue = Convert.ToDateTime(x.GetString(1).Substring(6, 2) + "/" + x.GetString(1).Substring(4, 2) + "/" + x.GetString(1).Substring(0, 4)),
                    Fec_DevSolStr = x.GetString(2),
                    Fec_DevSol = Convert.ToDateTime(x.GetString(2).Substring(6, 2) + "/" + x.GetString(2).Substring(4, 2) + "/" + x.GetString(2).Substring(0, 4)),
                    Ind_Cob = x.GetString(3),
                    NumArch = x.GetInt32(4),
                    Num_Operacion = Convert.ToInt32(x.GetDecimal(5))
                }).FirstOrDefault();


                if (numOperacion == "")
                {
                    numOperacion = info.Num_Operacion.ToString();
                }
                #region Carga Beneficiario

                string FecCal = DateTime.Now.ToString("yyyyMMdd");
                List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();
                //LisTabPL = _rutinaOficialesRepository.ConsultaPorcentaje(FecCal);
                LisTabPL = (from por in LisTabPLPar where Convert.ToInt32(por.fec_inivigpor) <= Convert.ToInt32(FecCal) && Convert.ToInt32(FecCal) <= Convert.ToInt32(por.fec_tervigpor) select por).ToList();


                List<beDatosBen> ListaBen = new List<beDatosBen>();
                ListaBen = ListaBeneficiarios(numOperacion, FecCal, info.CodigoPension, LisTabPL, info.NumArch.ToString(), info.Fec_DevSolStr, info.Ind_Cob, info.FechaDevengueStr);

                #endregion


                #endregion
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
        public Response Guardar(ActualizaInfOfi datos)
        {
            XmlConfigurator.Configure();
            _log.Info("Metodo Guardar num Operacion = " + datos.numOperacion);
            Response res = new Response();
            _log.Info("COD_ESTCOT = " + datos.COD_ESTCOT + "Del numero de Operacion = "+ datos.numOperacion);
           
            if (datos.COD_ESTCOT != "S" && datos.COD_ESTCOT != "N" && datos.COD_ESTCOT != "E")
            {
                _log.Info("Entró a la opcion no puede ser modificada = " + datos.numOperacion);
                res.IsOk = false;
                res.Message = "No puede ser modificada esta modalidad";
                return res;
            }
            else
            {
                _log.Info("Entró a la opcion a realizar el update = " + datos.numOperacion);
                res.IsOk = true;
                res = _ActualizaInfOfiRepository.calculoU(datos);
                return res;
            }
            
        }

        public Response busquedaMod(string numCor, string numOperacion)
        {
            Response res = new Response();
            try
            {
                var datosBusqueda = _ActualizaInfOfiRepository.busquedaMod(numCor, numOperacion);
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

        public List<beDatosBen> ListaBeneficiarios(string idSolicitud, string FecCal, string TipPen, List<bePorcenLegales> LisTabPL, string NumArch, string fechaSol, string cober, string fechaDevengue)
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
            return ListaBen;
        }
    }
}
