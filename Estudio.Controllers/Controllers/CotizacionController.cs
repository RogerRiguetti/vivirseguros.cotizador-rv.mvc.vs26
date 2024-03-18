using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using Estudio.Logic;
using Estudio.Repository.Core.Domain;

using CrystalDecisions.CrystalReports.Engine;
using Estudio.Repository;
using Estudio.Repository.Helpers;
using log4net;
using System.Reflection;
using log4net.Config;

namespace Estudio.Controllers.Controllers
{
    public class CotizacionController : Controller
    {
        CotizacionLogic _cotizacionLogic = new CotizacionLogic();
        BeneficiariosLogic _beneficiariosLogic = new BeneficiariosLogic();
        ModalidadesLogic _modalidadesLogic = new ModalidadesLogic();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Transaccionales
        /// <summary>
        /// Lizbeth Guadalupe Morales Montiel 09/03/2018
        /// Este metodo nos mostrara la informacion en la tabla de los usuarios dependiento de los filtros por los cuales se haya buscado 
        /// </summary>
        /// <param name="idTipoDocumento">Tipo de documento por el cual se hara la busqueda</param>
        /// <param name="documento">Numero del documento</param>
        /// <param name="nombres">Nombres del usuario</param>
        /// <param name="apellidos">Apellidos del usuario</param>
        /// <returns>Regresara la informacion filtrada</returns>

        public ActionResult Search(int idTipoDocumento, string documento, string nombres, string apellidos, int asesor, string cuspp)
        {
            try
            {
                string rol = this.Session["Name"].ToString();
                int idUsuario = Convert.ToInt32(this.Session["UserId"]);

                this.Session["IdTipoDocumento"] = idTipoDocumento;
                this.Session["Documento"] = documento;
                this.Session["Nombres"] = nombres;
                this.Session["Apellidos"] = apellidos;
                this.Session["Asesor"] = asesor;
                this.Session["CUSPP"] = cuspp;
                return Json(_cotizacionLogic.Search(idTipoDocumento, documento, nombres, apellidos, asesor, idUsuario, rol, cuspp),JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Beneficiarios

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// Método que registra o modifica Beneficiarios
        /// </summary>
        /// <param name="bandera"> Indica si se va a registrar o modificar un beneficiario </param>
        /// <param name="beneficiario"> Objeto que contiene la información del beneficiario </param>
        /// <param name="idPension"> Id de la pensión del asegurado </param>
        /// <param name="idTitular"> Id del titular </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public ActionResult RegistrarModificarBeneficiario(char bandera, Beneficiario beneficiario, int idPension, int idTitular, List<string> idsBeneficiarios)
        {
            try
            {
                var ben = _beneficiariosLogic.RegistrarModificarBeneficiario(bandera, beneficiario, idPension, idTitular, idsBeneficiarios);

                return Json(ben, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
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

        public JsonResult ConsultarBeneficiario(int idBeneficiario, int idPension)
        {
            try
            {
                var beneficiario = _beneficiariosLogic.ConsultarBeneficiario(idBeneficiario, idPension);

                return Json(beneficiario, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// Elimina al beneficiario de base de datos
        /// </summary>
        /// <param name="idBeneficiario"> Id del beneficiario a eliminar </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public JsonResult EliminarBeneficiario(int idBeneficiario)
        {
            try
            {
                List<Beneficiario> DatosCon = new List<Beneficiario>();
                string queryCon = "SELECT ISNULL(IdCotizacion, 0) AS IdCotizacion FROM Beneficiarios WHERE IdBeneficiario =" + idBeneficiario;

                DatosCon = VCEDBContext<Beneficiario>.CallSelectStatement(queryCon, x => new Beneficiario
                {
                    IdCotizacion = x.GetInt32(0)
                }).ToList();

                int IdC = (from id in DatosCon select id.IdCotizacion).FirstOrDefault();
                Response resultado;
                
                if (IdC == 0)
                {
                    resultado = _beneficiariosLogic.EliminarBeneficiarioC(idBeneficiario);
                }
                else
                {
                    resultado = _beneficiariosLogic.BajaTemporal(idBeneficiario);
                }


                //var resultado = _beneficiariosLogic.EliminarBeneficiario(idBeneficiario);
                

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
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

        public ActionResult ConsultaBeneficiarios(string numeroDocumento, string tipoDocumento, string cuspp )
        {
            try
            {
                var resultado = _beneficiariosLogic.ConsultaBeneficiarios(numeroDocumento, tipoDocumento, cuspp);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
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

        public ActionResult PorcentajesBeneficiarios(List<string> idsBeneficiarios, int idPension, string fechaFallecimiento, string FecDev)
        {
            try
            {
                var resultado = _beneficiariosLogic.PorcentajesBeneficiarios(idsBeneficiarios, idPension, fechaFallecimiento, FecDev);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
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

        public ActionResult ConsultarSituacionesInvalidez(int idParentesco, int idPension)
        {
            try
            {
                var resultado = _beneficiariosLogic.ConsultarSituacionesInvalidez(idParentesco, idPension);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-08
        /// Elimina todos los Beneficiarios que fueron registrados
        /// </summary>
        /// <param name="idsBeneficiarios"> Lista de ids de Beneficiarios registrados </param>
        /// <returns> Regresa un objeto que contiene una lista de los ids eliminados </returns>

        public ActionResult EliminarBeneficiarios(List<string> idsBeneficiarios)
        {
            try
            {
                var resultado = _beneficiariosLogic.EliminarBeneficiarios(idsBeneficiarios);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult EliminarBeneficiariosC(List<string> idsBeneficiarios)
        {
            try
            {
                var resultado = _beneficiariosLogic.EliminarBeneficiariosC(idsBeneficiarios);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult ValidacionRegresar(int idCotizacion)
        {
            try
            {
                var resultado = _beneficiariosLogic.ValidacionRegresar(idCotizacion);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
        #region Modalidades

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-15
        /// Registra o modifica modalidades dependiendo de la bandera
        /// </summary>
        /// <param name="bandera"> Indica si se va a registrar o modificar un beneficiario </param>
        /// <param name="idModalidad"> Id de la modalidad a modificar </param>
        /// <param name="aniosDiferidos"> Años diferidos </param>
        /// <param name="porcentajeAfp"> Porcentaje AFP (Administrador de Fondo de Pensión) </param>
        /// <param name="aniosGarantizados"> Años garantizados </param>
        /// <param name="gratificacion"> Gratificación </param>
        /// <param name="porcentajeTemporal"> Porcentaje temporal </param>
        /// <param name="idMoneda"> Id del tipo de moneda </param>
        /// <param name="idTipoRenta"> Id del tipo de renta </param>
        /// <param name="idModalidadCat"> Id del catálogo de modalidades </param>
        /// <param name="primerTramo"> Procentaje del primer tramo </param>
        /// <param name="segundoTramo"> Porcentaje del primer tramo </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public ActionResult RegistrarModificarModalidad(string bandera, int idModalidad, decimal primerTramo, Modalidad modalidad)
        {
            try
            {
                var mod = _modalidadesLogic.RegistrarModificarModalidad(bandera, idModalidad, modalidad);

                return Json(mod, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-15
        /// Consulta la modalidad a modificar
        /// </summary>
        /// <param name="idModalidad"> Id de la modalidad a eliminar </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public JsonResult ConsultarModalidad(int idModalidad)
        {
            try
            {
                var modalidad = _modalidadesLogic.ConsultarModalidad(idModalidad);

                return Json(modalidad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-15
        /// Elimina modalidad de base de datos
        /// </summary>
        /// <param name="idModalidad"> Id de la modalidad a eliminar </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public JsonResult EliminarModalidad(int idModalidad)
        {
            try
            {
                List<Modalidad> DatosCon = new List<Modalidad>();
                string queryCon = "SELECT ISNULL(IdCotizacion, 0) AS IdCotizacion FROM Modalidades WHERE IdModalidad ="+ idModalidad;

                DatosCon = VCEDBContext<Modalidad>.CallSelectStatement(queryCon, x => new Modalidad
                {
                    IdCotizacion = x.GetInt32(0)
                }).ToList();

                int IdC = (from id in DatosCon select id.IdCotizacion).FirstOrDefault();
                Response resultado;
                // var resultado = _modalidadesLogic.EliminarModalidad(idModalidad);
                if (IdC == 0)
                {
                    resultado = _modalidadesLogic.EliminarModalidadC(idModalidad);
                }
                else
                {
                    resultado = _modalidadesLogic.BajaTemporal(idModalidad);
                }
                

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult EliminarModalidadC(int idModalidad)
        {
            try
            {
                var resultado = _modalidadesLogic.EliminarModalidadC(idModalidad);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult ValidacionRegresarMod(int idCotizacion)
        {
            try
            {
                var resultado = _modalidadesLogic.ValidacionRegresarMod(idCotizacion);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-16
        /// Registra las modalidades del paquete
        /// 
        /// Antonio Quezada 2018-07-23
        /// Se agrega el tipo pensión para agregarlo a los paquetes
        /// </summary>
        /// <param name="idPaquete"> Id del paquete a registrar </param>
        /// <param name="TipoAfp"> Tipo de AFP seleccionada </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public ActionResult RegistrarPaquete(int idPaquete, string TipoAfp)
        {
            try
            {
                var resultado = _modalidadesLogic.RegistrarPaquete(idPaquete, TipoAfp);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-08
        /// Elimina todos las Modalidades que fueron registradas
        /// </summary>
        /// <param name="idsModalidades"> Lista de ids de Modalidades registradas </param>
        /// <returns> Regresa un objeto que contiene una lista de los ids eliminados </returns>

        public ActionResult EliminarModalidades(List<string> idsModalidades)
        {
            try
            {
                var resultado = _modalidadesLogic.EliminarModalidades(idsModalidades);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult EliminarModalidadesC(List<string> idsModalidades)
        {
            try
            {
                var resultado = _modalidadesLogic.EliminarModalidadesC(idsModalidades);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult ConsultarValidacionesModalidades(int idPension, int idRenta, int idMoneda)
        {
            try
            {
                var resultado = _modalidadesLogic.ConsultarValidacionesModalidades(idPension, idRenta, idMoneda);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
        #region Cotización

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-16
        /// Registra o modicia la cotización
        /// </summary>
        /// <param name="bandera"> Indica si se va a registrar o modificar una cotización </param>
        /// <param name="cotizacion"> Objeto que contiene la información de la cotización y del asegurado </param>
        /// <param name="idsBeneficiarios"> Lista de ids de beneficiarios pertenecientes a la cotización </param>
        /// <param name="idsModalidades"> Lista de ids de modalidades pertenecientes a la cotización </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public ActionResult RegistrarModificarCotizacion(string bandera, Cotizacion cotizacion, List<string> idsBeneficiarios, List<string> idsModalidades)
        {
            try
            {
                XmlConfigurator.Configure();
                _log.Info("Comenzara a ejecutar la creacion o modificacion de la cotizacion");
                var resultado = _cotizacionLogic.RegistrarModificarCotizacion(bandera, cotizacion, idsBeneficiarios, idsModalidades);
                _log.Info("Termino la ejecución la creacion o modificacion de la cotizacion");
                _log.Info("Resultados Extraoficiales Mensaje" + resultado.Message);
                _log.Info("Resultados Extraoficiales OK" + resultado.IsOk);
                _log.Info("Resultados Extraoficiales Object" + resultado.Object);
                _log.Info("Resultados Extraoficiales JSONRequest" + JsonRequestBehavior.AllowGet);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Info("Error en la creacionModificacion" + ex.Message);
                
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-22
        /// Consulta los datos de la cotización
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotización a consultar </param>
        /// <param name="operacion"> Tipo de operación 1 = Modificar, 2 = Clonar </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public ActionResult ConsultarCotizacion(int idCotizacion, int operacion)
        {
            try
            {
                var resultado = _cotizacionLogic.ConsultarCotizacion(idCotizacion, operacion);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Lizbeth Morales 05/04/2018
        /// 
        /// Antonio Quezada 2018-05-15
        /// Se agrega el parámetro de reporte para indicar si se debe de ejecutar el algoritmo o consultar de base de datos
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotización a la que se le generará el reporte </param>
        /// <param name="reporte"> Indica si se va a ejecutar el algoritmo o consultar de base de datos </param>
        /// <returns></returns>
        
        public ActionResult ReporteTasa(int idCotizacion, int reporte)
        {
            var resultExportacion = _cotizacionLogic.ConsultaRpt(idCotizacion, reporte);

            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/CotizacionReporte.rpt"));
            rpt.SetDataSource(resultExportacion);
            try
            {
                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Lizbeth Morales 05/04/2018
        /// 
        /// Antonio Quezada 2018-05-15
        /// Se agrega el parámetro de reporte para indicar si se debe de ejecutar el algoritmo o consultar de base de datos
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotización a la que se le generará el reporte </param>
        /// <param name="reporte"> Indica si se va a ejecutar el algoritmo o consultar de base de datos </param>
        /// <returns></returns>

        public ActionResult Reporte(int idCotizacion, int reporte)
        {
            var resultExportacion = _cotizacionLogic.ConsultaRpt(idCotizacion, reporte);

            ReportDocument rptC = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rptC.Load(Server.MapPath("~/Resources/reports/CotizacionReporteC.rpt"));
            rptC.SetDataSource(resultExportacion);
            try
            {
                Stream streamC = rptC.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stream.Seek(0, SeekOrigin.Begin);
                return File(streamC, "application/pdf");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-04-10
        /// Obtiene la información para validar el documento registrado
        /// </summary>
        /// <param name="idTipoDocumento"> Id del tipo de documento </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public ActionResult ValidaDocumento(int idTipoDocumento)
        {
            try
            {
                var resultado = _cotizacionLogic.ValidaDocumento(idTipoDocumento);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Lizbeth Morales 
        /// 10/07/2018
        /// Metodo el cual realiza la consulta del % de afp
        /// 
        /// Antonio Quezada 2018-07-23
        /// Modificación de modalidades previamente registradas
        /// </summary>
        /// <param name="afp">valor de afp seleccionado en el combo</param>
        /// <param name="idsModalidades"> Lista de ids de modalidades registradas hasta el momento </param>
        /// <returns>regresa el valor obtenido de la consulta (% afp)</returns>

        public ActionResult ConsultarAfp(string afp, List<string> idsModalidades)
        {
            try
            {
                var resultado = _modalidadesLogic.ConsultarAfp(afp, idsModalidades);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Lizbeth Morales
        /// 10/07/2018
        /// Metodo el cual filtra las provincias dependiendo del departamento seleccionado
        /// </summary>
        /// <param name="idDepartamento">Id del departamento seleccionado</param>
        /// <returns>las provincias pertenecientes al distrito seleccionado</returns>
        public ActionResult ConsultaProvincias(int idDepartamento)
        {
            try
            {
                var resultado = _cotizacionLogic.Provincias(idDepartamento);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Lizbeth Morales 
        /// 10/06/2018
        /// Metodo el cual filtra los distritos dependiendo de la provincia seleccionada
        /// </summary>
        /// <param name="idProvincia">Id de la provincia seleccionada</param>
        /// <returns>valores filtrados de los distritos</returns>
        public ActionResult ConsultaDistrito(int idProvincia)
        {
            try
            {
                var resultado = _cotizacionLogic.Distritos(idProvincia);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Lizbeth Morales / Antonio Quezada
        /// 2018-08-08
        /// Obtiene el Departamento, Provincia y Distrito que se deben cargar al inicio de la vista
        /// </summary>
        /// <returns> Regresa un objeto que contiene el Departamento, Provincia y Distrito que se deben cargar al inicio de la vista</returns>

        public ActionResult ConsultarLocalidad()
        {
            try
            {
                var resultado = _cotizacionLogic.ConsultarLocalidad();

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion
        #region Asegurado
        /// <summary>
        /// Antonio Quezada
        /// 2018-03-16
        /// Consulta el asegurado de la base de datos de jubilare
        /// </summary>
        /// <param name="tipoDocumento"> Tipo de documento </param>
        /// <param name="documento"> Número del documento </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public ActionResult ConsultarAsegurado(string tipoDocumento, string documento, string cuspp)
        {
            try
            {
                string rol = this.Session["Name"].ToString();
                int idUsuario = Convert.ToInt32(this.Session["UserId"]);
                var resultado = _cotizacionLogic.ConsultarAsegurado(tipoDocumento, documento, rol, idUsuario, cuspp);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult EliminaCotizacion(int idCotizacion)
        {
            try
            {
                string rol = this.Session["Name"].ToString();
                int idUsuario = Convert.ToInt32(this.Session["UserId"]);
                var resultado = _cotizacionLogic.EliminaCotizacion(idCotizacion);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-09-07
        /// </summary>
        /// <param name="term"> Parte del CUSPP con la que deben empezar los CUSPP a consultar </param>
        /// <returns> Regresa una lista con CUSPP </returns>

        public ActionResult ConsultaCUSPP(string term)
        {
            try
            {
                var resultado = _cotizacionLogic.ConsultaCUSPP(term);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
        #endregion

        #region No Transaccionales
        #region Index
        /// <summary>
        /// Antonio Quezada
        /// 2018-03-08
        /// </summary>
        /// <returns> Regresa la pantalla principal de Cotizaciones </returns>

        public ActionResult Index()
        {
            int idUsuario = Convert.ToInt32(this.Session["UserId"]);
            int idTipoDocumento = this.Session["IdTipoDocumento"] == null ? 0 : Convert.ToInt32(this.Session["IdTipoDocumento"]);
            int asesor = this.Session["Asesor"] == null ? 0 : Convert.ToInt32(this.Session["Asesor"]);
            string rol = this.Session["Name"].ToString();

            ViewBag.TiposDocumento = new SelectList(_cotizacionLogic.TiposDocumentos(), "IdTipoDocumento", "Elemento", idTipoDocumento);
            ViewBag.cmbxAsesores = new SelectList(_cotizacionLogic.Asesores(idUsuario, rol), "Id", "NombreCompleto", asesor);
            return View();
        }

        
        #endregion
        #region Create
        /// <summary>
        /// Antonio Quezada
        /// 2018-03-08
        /// </summary>
        /// <returns> Regresa la pantalla de Registrar Cotizaciones </returns>
        
        public ActionResult Create()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            string rol = this.Session["Name"].ToString();
            int idUsuario = Convert.ToInt32(this.Session["UserId"]);

            this.Session["IdTipoDocumento"] = null;
            this.Session["Documento"] = null;
            this.Session["Nombres"] = null;
            this.Session["Apellidos"] = null;
            this.Session["Asesor"] = null;

            int idDepartamento = 0;
            int idProvincia = 0;
            int idDistrito = 0;

            List<Departamento> departamentos = _cotizacionLogic.Departamentos();
            List<Provincia> provincias = _cotizacionLogic.Provincias();
            List<Distrito> distritos = _cotizacionLogic.Distritos();

            idDepartamento = (from d in departamentos where d.Elemento == "LIMA" select d.IdDepartamento).First();
            idProvincia = (from d in provincias where d.Elemento == "LIMA" select d.IdProvincia).First();
            idDistrito = (from d in distritos where d.Elemento == "LIMA" select d.IdDistrito).First();

            ViewBag.Asesores = new SelectList(_cotizacionLogic.Asesores(), "Id", "NombreCompleto", idUsuario);

            ViewBag.TiposDocumento = new SelectList(_cotizacionLogic.TiposDocumentos(), "IdTipoDocumento", "Elemento");
            ViewBag.Sexo = new SelectList(_cotizacionLogic.Sexos(), "IdSexo", "Elemento");
            ViewBag.Afp = new SelectList(_cotizacionLogic.AFP(), "IdAfp", "Elemento");
            ViewBag.Pensiones = new SelectList(_cotizacionLogic.Pensiones(), "IdPension", "Elemento");
            ViewBag.TiposRenta = new SelectList(_cotizacionLogic.TiposRenta(), "IdTipoRenta", "Elemento");
            ViewBag.Departamentos = new SelectList(departamentos, "IdDepartamento", "Elemento", idDepartamento);
            ViewBag.Provincias = new SelectList(provincias, "IdProvincia", "Elemento", idProvincia);
            ViewBag.Distritos = new SelectList(distritos, "IdDistrito", "Elemento", idDistrito);

            ViewBag.Parentescos = new SelectList(_cotizacionLogic.Parentescos(), "IdParentesco", "Elemento");
            ViewBag.SituacionesInvalidez = new SelectList(_cotizacionLogic.SituacionesInvalidez(), "IdSituacionInvalidez", "Elemento");

            ViewBag.Monedas = new SelectList(_cotizacionLogic.Monedas(), "IdMoneda", "Elemento");
            ViewBag.Modalidades = new SelectList(_cotizacionLogic.Modalidades(), "IdModalidadCat", "Elemento");

            ViewBag.Paquetes = new SelectList(_cotizacionLogic.Paquetes(), "IdPaquete", "Elemento");

            ViewBag.Comisiones = _cotizacionLogic.Comisiones();

            ViewBag.TipoCambio = _cotizacionLogic.ConsultaTipoCambio();
            ViewBag.GastoSepelio = _cotizacionLogic.ConsultaGastoSepelio();

            return View();
        }

        #endregion
        #region Edit
        /// <summary>
        /// Antonio Quezada
        /// 2018-03-22
        /// Carga la cotización en pantalla
        /// </summary>
        /// <param name="idCotizacion"></param>
        /// <returns> Regresa la vista </returns>

        [HttpGet]
        public ActionResult Edit(int idCotizacion, int operacion)
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");
            
            ViewBag.TiposDocumento = new SelectList(_cotizacionLogic.TiposDocumentos(), "IdTipoDocumento", "Elemento");
            ViewBag.Sexo = new SelectList(_cotizacionLogic.Sexos(), "IdSexo", "Elemento");
            ViewBag.Afp = new SelectList(_cotizacionLogic.AFP(), "IdAfp", "Elemento");
            ViewBag.Pensiones = new SelectList(_cotizacionLogic.Pensiones(), "IdPension", "Elemento");
            ViewBag.TiposhRenta = new SelectList(_cotizacionLogic.TiposRenta(), "IdTipoRenta", "Elemento");
            ViewBag.Departamentos = new SelectList(_cotizacionLogic.Departamentos(), "IdDepartamento", "Elemento");
            ViewBag.Provincias = new SelectList("");
            ViewBag.Distritos = new SelectList("");
            ViewBag.Asesores = new SelectList(_cotizacionLogic.Asesores(), "Id", "NombreCompleto");

            ViewBag.Parentescos = new SelectList(_cotizacionLogic.Parentescos(), "IdParentesco", "Elemento");
            ViewBag.SituacionesInvalidez = new SelectList(_cotizacionLogic.SituacionesInvalidez(), "IdSituacionInvalidez", "Elemento");

            ViewBag.Monedas = new SelectList(_cotizacionLogic.Monedas(), "IdMoneda", "Elemento");
            ViewBag.Modalidades = new SelectList(_cotizacionLogic.Modalidades(), "IdModalidadCat", "Elemento");

            ViewBag.Paquetes = new SelectList(_cotizacionLogic.Paquetes(), "IdPaquete", "Elemento");

            ViewBag.Comisiones = _cotizacionLogic.Comisiones();

            return View();
        }
        #endregion
        #endregion
    }
}