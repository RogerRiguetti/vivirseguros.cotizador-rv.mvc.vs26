using Estudio.Repository.Helpers;
using System;
using Estudio.Repository.Persistence.Repositories;
using Estudio.Repository.Core.Domain;
using Estudio.Process;
using System.Collections.Generic;
using System.Linq;

namespace Estudio.Logic
{
    public class BeneficiariosLogic
    {
        BeneficiariosRepository _beneficiariosRepository = new BeneficiariosRepository();
        RutinaRepository _rutinaRepository = new RutinaRepository();

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// Método que registra o modifica Beneficiarios
        /// </summary>
        /// <param name="bandera"> Indica si se va a registrar o modificar un beneficiario </param>
        /// <param name="beneficiario"> Objeto que contiene la información del beneficiario </param>
        /// <param name="idPension"> Id de la pensión del asegurado </param>
        /// <param name="idTitular"> Id del titular </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación de registro o modificación de Beneficiario</returns>

        public Response RegistrarModificarBeneficiario(char bandera, Beneficiario beneficiario, int idPension, int idTitular, List<string> idsBeneficiarios)
        {
            try
            {
                if (beneficiario.Parentesco != "TITULAR" && idTitular == 0)
                {
                    Response res = new Response();
                    res.IsOk = false;
                    res.Message = "Es necesario registrar primero un titular";
                    return res;
                }
                else
                {
                    List<Beneficiario> beneficiarios = new List<Beneficiario>();
                    List<Parentesco> parentescos = new List<Parentesco>();
                    List<beDatosBen> ListaBen = new List<beDatosBen>();

                    Beneficiario pensionBen = new Beneficiario();
                    beDatosBen TablaBen = new beDatosBen();
                    Response res = new Response();

                    int idBeneficiario;
                    double porcentaje;
                    string clavePension;
                    string mensaje = "";
                    int hijos = 0;
                    string fecCal = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd");
                    string FecDev = Convert.ToDateTime(beneficiario.FecDev).ToString("yyyyMMdd");

                    if (beneficiario.IdSituacionInvalidez == 0)
                    {
                        SituacionInvalidez situacionInv = _beneficiariosRepository.ConsultarSituacionesInvalidez(beneficiario.IdParentesco, idPension)[0];
                        int idSituacionInvalidez = situacionInv.IdSituacionInvalidez;
                        string situacionInvalidez = situacionInv.Elemento;

                        beneficiario.IdSituacionInvalidez = idSituacionInvalidez;
                        beneficiario.SituacionInvalidez = situacionInvalidez;
                    }

                    idBeneficiario = _beneficiariosRepository.RegistrarModificarBeneficiario(bandera, beneficiario).IdBeneficiario;
                    clavePension = _beneficiariosRepository.TipoPension(idPension).Clave;
                    pensionBen = _beneficiariosRepository.ConsultarBeneficiario(idBeneficiario);

                    if (bandera == 'C')
                    {
                        if (idsBeneficiarios == null)
                            idsBeneficiarios = new List<string>();

                        idsBeneficiarios.Add(idBeneficiario.ToString());
                    }

                    foreach (var item in idsBeneficiarios)
                    {
                        beneficiario = new Beneficiario();
                        beneficiario = _beneficiariosRepository.ConsultarBeneficiario(Convert.ToInt32(item));
                        if (beneficiario.Parentesco != "")
                        {
                            beneficiario.IdBeneficiario = Convert.ToInt32(item);
                            beneficiarios.Add(beneficiario);
                        }
                    }

                    beneficiarios = (from b in beneficiarios select b).OrderByDescending(b => b.CodigoElemento).ToList();

                    foreach (var item in beneficiarios)
                    {
                        if (item.Parentesco == "CÓNYUGE")
                        {
                            hijos = (from b in beneficiarios where b.Parentesco == "HIJOS" select b).Count();

                            if (hijos == 0)
                                item.CodigoElemento = "10";
                            else
                                item.CodigoElemento = "11";
                        }

                        if (item.Parentesco == "PADRE")
                        {
                            if (item.ClaveSexo == "M")
                                item.CodigoElemento = "41";
                            else
                                item.CodigoElemento = "42";
                        }

                        TablaBen = new beDatosBen();
                        TablaBen.NumOrd = item.IdBeneficiario;
                        TablaBen.CodPar = item.CodigoElemento;
                        TablaBen.FecNac = item.FechaNacimiento.ToString("yyyyMMdd");
                        TablaBen.GruFam = item.Parentesco == "TITULAR" ? "00" : "01";
                        TablaBen.TipSex = item.ClaveSexo;
                        TablaBen.TipInv = item.ClaveSituacionInvalidez;
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
                    List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();
                    List<beDatosBen> CopiaBen = new List<beDatosBen>();
                    foreach (var itembencop in ListaBen)
                    {
                        CopiaBen.Add(new beDatosBen { CodPar = itembencop.CodPar, NacHM = itembencop.NacHM });
                    }
                    LisTabPL = _rutinaRepository.ConsultaDetalleMortalidad(fecCal);

                    ListaBen = RP.PorcentajeBen(ListaBen, FecDev, "S", clavePension, 336, LisTabPL, CopiaBen);

                    if (ListaBen[0].Mensaje == null)
                    {
                        foreach (var item in ListaBen)
                        {
                            porcentaje = item.PrcLeg;
                            if (item.CodPar == "99")
                            {
                                if (clavePension == "06")
                                    porcentaje = item.PrcPen * 0.7;

                                if (clavePension == "07")
                                    porcentaje = item.PrcPen * 0.5;
                            }

                            beneficiario = new Beneficiario();

                            beneficiario = (from b in beneficiarios where b.IdBeneficiario == item.NumOrd select b).First();
                            beneficiario.PorcentajeBen = porcentaje.ToString("N2") + "%";
                            beneficiario.PorcentajeBenDbl = porcentaje;

                            if (item.NumOrd == idBeneficiario)
                            {
                                pensionBen.PorcentajeBen = porcentaje.ToString("N2") + "%";
                                pensionBen.PorcentajeBenDbl = porcentaje;
                            }

                            _beneficiariosRepository.RegistrarModificarBeneficiario('U', beneficiario);
                        }
                    }
                    else
                    {
                        pensionBen.PorcentajeBen = "0%";
                        pensionBen.PorcentajeBenDbl = 0;

                        mensaje = ListaBen[0].Mensaje;
                    }

                    pensionBen.IdBeneficiario = idBeneficiario;

                    res.Object = new { pensionBen = pensionBen, beneficiarios = beneficiarios };
                    res.Message = mensaje;
                    return res;
                }

            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// Consulta el beneficiario a modificar
        /// 
        /// Antonio Quezada 2018-08-07
        /// Se agrega idPension para cargar el combo de Situación de Invalidez junto con el beneficiario
        /// </summary>
        /// <param name="idBeneficiario"> Id del beneficiario a eliminar </param>
        /// <param name="idPension"> Id de la Pensión seleccionada </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public Response ConsultarBeneficiario(int idBeneficiario, int idPension)
        {
            try
            {
                Response res = new Response();
                Beneficiario beneficiario = _beneficiariosRepository.ConsultarBeneficiario(idBeneficiario);
                List<SituacionInvalidez> situacionesInvalidez = _beneficiariosRepository.ConsultarSituacionesInvalidez(beneficiario.IdParentesco, idPension);

                res.IsOk = true;
                res.Object = new { beneficiario = beneficiario, situacionesInvalidez = situacionesInvalidez };
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// Elimina al beneficiario de base de datos
        /// </summary>
        /// <param name="idBeneficiario"> Id del beneficiario a eliminar </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public Response EliminarBeneficiario(int idBeneficiario)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _beneficiariosRepository.EliminarBeneficiario(idBeneficiario);
                res.Message = "Beneficiario eliminado con éxito";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        public Response EliminarBeneficiarioC(int idBeneficiario)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _beneficiariosRepository.EliminarBeneficiarioC(idBeneficiario);
                res.Message = "Beneficiario eliminado con éxito";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        public Response BajaTemporal(int idBeneficiario)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _beneficiariosRepository.BajaTemporal(idBeneficiario);
                // _beneficiariosRepository.EjecutarScript(query);
                //res.Object = idBeneficiario;
                //PorcentajesBeneficiarios(ListidsBeneficiarios, int idPension, string fechaFallecimiento, string FecDev)
                res.Message = "Beneficiario eliminado con éxito";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-21
        /// Carga los beneficiarios del asegurado de Jubilare
        /// </summary>
        /// <param name="numeroDocumento"> Número del documento </param>
        /// <param name="tipoDocumento"> Tipo de Documento </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public Response ConsultaBeneficiarios(string numeroDocumento, string tipoDocumento, string cuspp)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _beneficiariosRepository.ConsultaBeneficiarios(numeroDocumento, tipoDocumento, cuspp);
                res.Message = "Operación realizada con éxito";

                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";

                return res;
            }
        }


        public Response Pension(int idTipoPension)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _beneficiariosRepository.TipoPension(idTipoPension);
                res.Message = "Información cargada con éxito";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-07-20
        /// Actualiza todos los beneficiarios registrados al cambiar de pensión
        /// </summary>
        /// <param name="idsBeneficiarios"> Lista de ids de beneficiarios registrados </param>
        /// <param name="idPension"> Id de la pensión </param>
        /// <param name="fechaFallecimiento"> Fecha de Fallecimiento del Titular </param>
        /// <returns> Regresa los beneficiarios con el porcentaje de pensión actualizado </returns>

        public Response PorcentajesBeneficiarios(List<string> idsBeneficiarios, int idPension, string fechaFallecimiento, string FecDev)
        {
            try
            {
                Response res = new Response();
                List<Beneficiario> beneficiarios = new List<Beneficiario>();
                List<beDatosBen> ListaBen = new List<beDatosBen>();
                beDatosBen TablaBen = new beDatosBen();
                Beneficiario beneficiario = new Beneficiario();
                Pension pension = new Pension();

                //bool codigoElemento = false;
                string mensaje = "";
                bool IsOk = true;
                string TipoPension = "";
                string clavePension = "";
                int hijos = 0;
                int titular = 0;
                if (FecDev != "")
                {
                    FecDev = Convert.ToDateTime(FecDev).ToString("yyyyMMdd");
                }
                pension = _beneficiariosRepository.TipoPension(idPension);
                TipoPension = pension.Elemento;
                clavePension = pension.Clave;

                foreach (var item in idsBeneficiarios)
                {
                    beneficiario = new Beneficiario();

                    beneficiario = _beneficiariosRepository.ConsultarBeneficiario(Convert.ToInt32(item));
                    beneficiario.IdBeneficiario = Convert.ToInt32(item);
                    
                    //FecDev = Convert.ToDateTime(beneficiario.FecDev).ToString("yyyyMMdd");
                    if (beneficiario.Parentesco == "TITULAR")
                    {
                        switch (TipoPension)
                        {
                            case "INVALIDEZ PARCIAL":
                                beneficiario.FechaInvalidezStr = (DateTime.Now).ToString("yyyy/MM/dd");
                                beneficiario.FechaFallecimientoStr = "";
                                break;

                            case "INVALIDEZ TOTAL":
                                beneficiario.FechaInvalidezStr = (DateTime.Now).ToString("yyyy/MM/dd");
                                beneficiario.FechaFallecimientoStr = "";
                                break;

                            case "SOBREVIVENCIA":
                                beneficiario.FechaInvalidezStr = "";
                                beneficiario.FechaFallecimientoStr = fechaFallecimiento;
                                break;

                            default:
                                beneficiario.FechaInvalidezStr = "";
                                beneficiario.FechaFallecimientoStr = "";
                                break;
                        }

                        SituacionInvalidez situacionInv = _beneficiariosRepository.ConsultarSituacionesInvalidez(beneficiario.IdParentesco, idPension)[0];
                        int idSituacionInvalidez = situacionInv.IdSituacionInvalidez;
                        string situacionInvalidez = situacionInv.Elemento;

                        beneficiario.IdSituacionInvalidez = idSituacionInvalidez;
                        beneficiario.SituacionInvalidez = situacionInvalidez;
                        _beneficiariosRepository.RegistrarModificarBeneficiario('U', beneficiario);
                    }

                    //if (beneficiario.Parentesco != "")
                    //{
                    beneficiario.IdBeneficiario = Convert.ToInt32(item);
                    beneficiarios.Add(beneficiario);
                    //}

                    // beneficiarios.Add(beneficiario);

                    //if (beneficiario.CodigoElemento == "")
                    //    codigoElemento = true;
                }

                titular = (from b in beneficiarios where b.Parentesco == "TITULAR" select b).Count();

                if (titular > 0)
                {
                    //beneficiarios = (from b in beneficiarios select b).OrderByDescending(b => b.CodigoElemento).ToList();

                    foreach (var item in beneficiarios)
                    {
                        if (item.Parentesco == "CÓNYUGE")
                        {
                            hijos = (from b in beneficiarios where b.Parentesco == "HIJOS" select b).Count();

                            if (hijos == 0)
                                item.CodigoElemento = "10";
                            else
                                item.CodigoElemento = "11";
                        }

                        if (item.Parentesco == "PADRE")
                        {
                            if (item.ClaveSexo == "M")
                                item.CodigoElemento = "41";
                            else
                                item.CodigoElemento = "42";
                        }
                        TablaBen = new beDatosBen();

                        TablaBen.NumOrd = item.IdBeneficiario;
                        TablaBen.CodPar = item.CodigoElemento;
                        TablaBen.FecNac = item.FechaNacimiento.ToString("yyyyMMdd");
                        TablaBen.GruFam = item.Parentesco == "TITULAR" ? "00" : "01";
                        TablaBen.TipSex = item.ClaveSexo;
                        TablaBen.TipInv = item.ClaveSituacionInvalidez;
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
                    List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();

                    string fecCal = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd");

                    LisTabPL = _rutinaRepository.ConsultaDetalleMortalidad(fecCal);
                    ListaBen = (from b in ListaBen where b.CodPar != "" select b).ToList();
                    if (FecDev == "00010101" || FecDev == "")
                    {
                        FecDev = fecCal;
                    }
                    //CAMBIO PARA BENEFICIARIOS TIR - COPIABEN
                    List<beDatosBen> CopiaBen = new List<beDatosBen>();
                    foreach (var itembencop in ListaBen)
                    {
                        CopiaBen.Add(new beDatosBen { CodPar = itembencop.CodPar, NacHM = itembencop.NacHM });
                    }
                    ListaBen = RP.PorcentajeBen(ListaBen, FecDev, "S", clavePension, 336, LisTabPL, CopiaBen);

                    if (ListaBen[0].Mensaje == null)
                    {
                        foreach (var item in ListaBen)
                        {
                            double porcentaje = item.PrcPen;

                            if (clavePension == "06")
                                porcentaje = item.PrcPen * 0.7;

                            if (clavePension == "07")
                                porcentaje = item.PrcPen * 0.5;

                            beneficiario = new Beneficiario();

                            beneficiario = (from b in beneficiarios where b.IdBeneficiario == item.NumOrd select b).First();
                            beneficiario.PorcentajeBen = porcentaje.ToString("N2") + "%";
                            beneficiario.PorcentajeBenDbl = porcentaje;

                            _beneficiariosRepository.RegistrarModificarBeneficiario('U', beneficiario);
                        }
                    }
                    else
                    {
                        mensaje = ListaBen[0].Mensaje + " Al momento de recalcular los porcentajes.";
                        IsOk = false;
                    }
                }

                res.IsOk = IsOk;
                res.Object = beneficiarios;
                res.Message = mensaje;
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-06
        /// Consulta las Situaciones de Invalidez con base al Parentesco y Pensión seleccionadas
        /// </summary>
        /// <param name="idParentesco"> Id del Parentesco seleccionado </param>
        /// <param name="idPension"> Id de la Pensión seleccionada </param>
        /// <returns> Regresa una lista de Situaciones de Invalidez </returns>

        public Response ConsultarSituacionesInvalidez(int idParentesco, int idPension)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _beneficiariosRepository.ConsultarSituacionesInvalidez(idParentesco, idPension);
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-08
        /// Elimina todos los Beneficiarios que fueron registrados
        /// </summary>
        /// <param name="idsBeneficiarios"> Lista de ids de Beneficiarios registrados </param>
        /// <returns> Regresa un objeto que contiene una lista de los ids eliminados </returns>

        public Response EliminarBeneficiarios(List<string> idsBeneficiarios)
        {
            try
            {
                Response res = new Response();

                if (idsBeneficiarios != null)
                {
                    foreach (var item in idsBeneficiarios)
                    {
                        int idBeneficiario = Convert.ToInt32(item);
                        //_beneficiariosRepository.EliminarBeneficiario(idBeneficiario);
                        _beneficiariosRepository.BajaTemporal(idBeneficiario);
                    }
                }

                res.IsOk = true;
                res.Object = idsBeneficiarios;
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }


        public Response EliminarBeneficiariosC(List<string> idsBeneficiarios)
        {
            try
            {
                Response res = new Response();

                if (idsBeneficiarios != null)
                {
                    foreach (var item in idsBeneficiarios)
                    {
                        int idBeneficiario = Convert.ToInt32(item);
                        _beneficiariosRepository.EliminarBeneficiarioC(idBeneficiario);
                    }
                }

                res.IsOk = true;
                res.Object = idsBeneficiarios;
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        public Response ValidacionRegresar(int idCotizacion)
        {
            try
            {
                Response res = new Response();
                _beneficiariosRepository.ValidacionRegresar(idCotizacion);
                res.IsOk = true;
                res.Object = idCotizacion;
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        public object RegistrarInsertBeneficiario(string bandera, Beneficiario beneficiario)
        {
            try
            {
                char charDeBandera = bandera[0];
                Response res = new Response();
                res.IsOk = true;
                res.Object = _beneficiariosRepository.RegistrarModificarBeneficiario(charDeBandera, beneficiario);
                res.Message = "Operación exitosa";
                return res.Object; // Devuelve solo el objeto contenido en la respuesta
            }
            catch (Exception ex)
            {
                // Manejar el error si es necesario
                return null; // O devuelve otro valor predeterminado si se produce un error
            }
        }
    }
}
