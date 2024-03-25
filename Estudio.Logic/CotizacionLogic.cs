using Estudio.Process.Muestra;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Estudio.Logic
{
    public class CotizacionLogic
    {
        CotizacionRepository _cotizacionRepository = new CotizacionRepository();
        BeneficiariosRepository _beneficiarioRepository = new BeneficiariosRepository();
        ModalidadesRepository _modalidadRepository = new ModalidadesRepository();
        PruebasRutina _pruebaRutinaProcess = new PruebasRutina();
        RutinaRepository _rutinaRepository = new RutinaRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Lizbeth Guadalupe Morales Montiel 09/03/2018
        /// Metodo que mandara la informacion recibida del reporitory al controller y biceversa 
        /// </summary>
        /// <param name="idTipoDocumento">parametro que obtiene el tipo de documento que se buscara </param>
        /// <param name="documento">Numero del documento</param>
        /// <param name="nombres">Nombres del usuario</param>
        /// <param name="apellidos">Apellidos del usuario</param>
        /// <param name="supervisor">id del usuario logeado</param>
        /// <param name="rol">Rol del usuario logeado</param>
        /// <returns>Nos regresará la informacion que contenga la informacion de los parametros(o parametro)</returns>

        public Response Search(int idTipoDocumento, string documento, string nombres, string apellidos, int asesor, int supervisor, string rol, string cuspp)
        {
            try
            {
                XmlConfigurator.Configure();
                _log.Info("Comenzara a ejecutar las consultas de cotizaciones: ");
                Response response = new Response();
                response.IsOk = true;
                if (rol == "Administrador" || rol == "Gerente")
                {
                    if (idTipoDocumento == 0 && documento == "" && nombres == "" && apellidos == "" && asesor == 0 && cuspp == "")
                    {
                        response.Object = _cotizacionRepository.ConsultarCotizaciones("CONSULTADMINISTRADOR", 0, "", "", "", 0, 0, "");
                    }
                    else
                        response.Object = _cotizacionRepository.ConsultarCotizaciones("CONADMISTRADOR", idTipoDocumento, documento, nombres, apellidos, asesor, supervisor, cuspp);

                }
                else if (rol == "Supervisor")
                    response.Object = _cotizacionRepository.ConsultarCotizaciones("CONSUPERVISOR", idTipoDocumento, documento, nombres, apellidos, asesor, supervisor, cuspp);

                else
                    response.Object = _cotizacionRepository.ConsultarCotizaciones("CONASESOR", idTipoDocumento, documento, nombres, apellidos, asesor, supervisor, cuspp);

                response.Message = "Consulta Exitosa";
                return response;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR " + ex);
                Response response = new Response();
                response.IsOk = false;
                response.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return response;
            }

        }

        /// <summary>
        /// Lizbeth Morales 14/03/2018
        /// Metodo que nos compara si el usuario logeado es supervisor asesor o administrador
        /// </summary>
        /// <param name="supervisor">id del usuario</param>
        /// <param name="rol">Rol del usuario</param>
        /// <returns>Información de los asesores dependendo del rol del usuario logeado</returns>

        public List<gzUser> Asesores(int supervisor, string rol)
        {
            if (rol == "Supervisor")
                return _cotizacionRepository.Asesores(supervisor);

            else if (rol == "Asesor")
                return _cotizacionRepository.Ejecutivo(supervisor);
            else
                return _cotizacionRepository.Asesores();

        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-16-03
        /// Registra o modicia la cotización
        /// </summary>
        /// <param name="bandera"> Indica si se va a registrar o modificar una cotización </param>
        /// <param name="cotizacion"> Objeto que contiene la información de la cotización y del asegurado </param>
        /// <param name="idsBeneficiarios"> Lista de ids de beneficiarios pertenecientes a la cotización </param>
        /// <param name="idsModalidades"> Lista de ids de modalidades pertenecientes a la cotización </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public Response RegistrarModificarCotizacion(string bandera, Cotizacion cotizacion, List<string> idsBeneficiarios, List<string> idsModalidades)
        {
            try
            {
                Response res = new Response();
                Cotizacion cotizacionRpt = new Cotizacion();
                res.IsOk = true;

                // Beneficiarios
                XmlConfigurator.Configure();
                _log.Info("*******************************Se comenzará a cotizar EXTRAOFICIALES***************************************");
                _log.Info("Registro de Beneficiarios");
                DataTable idsBeneficiariosDT = new DataTable();
                idsBeneficiariosDT.Columns.Add("IdBeneficiario", typeof(int));

                if (idsBeneficiarios != null)
                {
                    foreach (var item in idsBeneficiarios)
                    {
                        DataRow row = idsBeneficiariosDT.NewRow();
                        row["IdBeneficiario"] = item;
                        idsBeneficiariosDT.Rows.Add(row);
                    }
                }

                // Modalidades
                _log.Info("Registro de Modalidades");
                DataTable idsModalidadesDT = new DataTable();
                idsModalidadesDT.Columns.Add("IdModalidad", typeof(int));

                foreach (var item in idsModalidades)
                {
                    DataRow row = idsModalidadesDT.NewRow();
                    row["IdModalidad"] = item;
                    idsModalidadesDT.Rows.Add(row);
                }

                cotizacionRpt = _cotizacionRepository.RegistrarModificarCotizacion(bandera, cotizacion, idsBeneficiariosDT, idsModalidadesDT);
                res.Object = cotizacionRpt;
                _log.Info("Registro de cotizacion");
                if (bandera == "C")
                    res.Message = "Cotización creada con éxito.";
                else
                {
                    _beneficiarioRepository.EliminarBeneficiario(cotizacionRpt.IdCotizacion);
                    _modalidadRepository.EliminarModalidad(cotizacionRpt.IdCotizacion);
                    res.Message = "Cotización modificada con éxito.";
                }

                // Cálculo de la rutina

                int idCotizacion = cotizacionRpt.IdCotizacion;
                string mensaje = "";

                List<beResultados> rutina = new List<beResultados>();

                DataTable rutinaDt = new DataTable();
                rutinaDt.Columns.Add("Marcasob", typeof(string));
                rutinaDt.Columns.Add("MtoAjusteipc", typeof(double));
                rutinaDt.Columns.Add("MtoPension", typeof(double));
                rutinaDt.Columns.Add("MtoPriunidif", typeof(double));
                rutinaDt.Columns.Add("MtoResmat", typeof(double));
                rutinaDt.Columns.Add("NumCorrelativo", typeof(int));
                rutinaDt.Columns.Add("NumCotestudio", typeof(string));
                rutinaDt.Columns.Add("PrcPercon", typeof(double));
                rutinaDt.Columns.Add("PrcTasatce", typeof(double));
                rutinaDt.Columns.Add("PrcTasatir", typeof(double));
                rutinaDt.Columns.Add("PrcTasavta", typeof(double));
                rutinaDt.Columns.Add("PrimaUnica", typeof(double));
                // rutinaDt.Columns.Add("MtoRentaTmpAfp", typeof(double));
                _log.Info("Comenzara a ejecutar la rutina");
                rutina = _pruebaRutinaProcess.Rutina(idCotizacion);
                _log.Info("Termino ejecucion de rutina");
                foreach (var item in rutina)
                {
                    if (item.Mensaje == null)
                    {
                        DataRow row = rutinaDt.NewRow();
                        row["Marcasob"] = item.MARCASOB;
                        row["MtoAjusteipc"] = item.MTO_AJUSTEIPC;
                        row["MtoPension"] = item.MTO_PENSION;
                        row["MtoPriunidif"] = item.MTO_PRIUNIDIF;
                        row["MtoResmat"] = item.MTO_RESMAT;
                        row["NumCorrelativo"] = item.NUM_CORRELATIVO;
                        row["NumCotestudio"] = item.NUM_COTESTUDIO;
                        row["PrcPercon"] = item.PRC_PERCON;
                        row["PrcTasatce"] = item.PRC_TASATCE;
                        row["PrcTasatir"] = item.PRC_TASATIR;
                        row["PrcTasavta"] = item.PRC_TASAVTA;
                        row["PrimaUnica"] = item.PRIMA_UNICA;
                        // row["MtoRentaTmpAfp"] = item.MTO_RENTATMPAFP;

                        rutinaDt.Rows.Add(row);
                    }
                    else
                    {
                        mensaje = item.Mensaje;
                        break;
                    }
                }

                if (mensaje == "" || mensaje == null)
                {
                    _log.Info("Se guardara informacion");
                    _cotizacionRepository.RegistroRutina(idCotizacion, rutinaDt);
                    _log.Info("Se guardo informacion");
                }
                else
                {
                    res.Message = res.Message + " Ocurrió el siguiente error en el cálculo de la tasa: \n" + mensaje;
                    res.Object = new { idCotizacion = idCotizacion, bandera = bandera };
                    res.IsOk = false;
                }
                
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                _log.Info("ERROR Registro cotizacion:" + ex);
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";

                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-16
        /// Consulta el asegurado de la base de datos de jubilare
        /// </summary>
        /// <param name="tipoDocumento"> Tipo de documento </param>
        /// <param name="documento"> Número del documento </param>
        /// <param name="rol"> Rol del usuario que inicio sesión en la aplicación </param>
        /// <param name="idUsuario"> Id del usuario que inicio sesión en la aplicación </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public Response ConsultarAsegurado(string tipoDocumento, string documento, string rol, int idUsuario, string cuspp)
        {
            try
            {
                Response response = new Response();
                Cotizacion asegurado = new Cotizacion();
                List<Cotizacion> asegurados = new List<Cotizacion>();

                DataTable afpDT = new DataTable();
                string codafp = "";

                int idAsesor = 0;

                if (rol == "Asesor")
                    idAsesor = idUsuario;

                asegurados = _cotizacionRepository.ConsultarAsegurado(tipoDocumento, documento, idAsesor, cuspp);
                if (asegurados.Count == 1)
                {
                    asegurado = asegurados[0];

                    afpDT = _modalidadRepository.CodigoAFP(asegurado.Afp);
                    if (afpDT.Rows.Count != 0)
                        codafp = afpDT.Rows[0][0] == null ? "" : (afpDT.Rows[0][0]).ToString();
                    else
                        codafp = "0";

                    asegurado.PorAfp = codafp;
                }

                response.Object = asegurados;
                response.Message = "Consulta Exitosa";
                return response;
            }
            catch (Exception ex)
            {

                Response response = new Response();
                response.IsOk = false;
                response.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return response;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-22
        /// Consulta los datos de la cotización
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotización a consultar </param>
        /// /// <param name="operacion"> Tipo de operación 1 = Modificar, 2 = Clonar </param>
        /// <returns> Regrsa un objeto de la clase Cotizacion con la información a mostrar </returns>

        public Response ConsultarCotizacion(int idCotizacion, int operacion)
        {
            try
            {
                Response res = new Response();
                DataTable afpDT = new DataTable();
                Cotizacion cotizacion = new Cotizacion();
                List<Beneficiario> beneficiarios = new List<Beneficiario>();
                List<Modalidad> modalidades = new List<Modalidad>();

                string codafp;

                cotizacion = _cotizacionRepository.ConsultarCotizacion(idCotizacion);
                afpDT = _modalidadRepository.CodigoAFP(cotizacion.Afp);
                codafp = (afpDT.Rows[0][0]).ToString();
                cotizacion.PorAfp = codafp;

                if (operacion == 1)
                {
                    beneficiarios = _beneficiarioRepository.ConsultarBeneficiariosModificar(idCotizacion);
                    modalidades = _modalidadRepository.ConsultarModalidadesModificar(idCotizacion);
                }
                else
                {
                    beneficiarios = _beneficiarioRepository.ClonarBeneficiarios(idCotizacion);
                    modalidades = _modalidadRepository.ClonarModalidades(idCotizacion);
                }

                var objeto = new { cotizacion = cotizacion, beneficiarios = beneficiarios, modalidades = modalidades };

                res.IsOk = true;
                res.Message = "Operación realizada con éxito";
                res.Object = objeto;
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }
        }

        /// <summary>
        /// Lizbeth Morales 05/04/2018
        /// Metodo que nos obtiene la informacion del reporte a generar
        /// 
        /// Antonio Quezada 2018-04-17
        /// Se agrega la rutina al reporte
        /// 
        /// Antonio Quezada 2018-05-15
        /// Se agrega la funcionalidad para obtener la información de base de datos si esta ya existe
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotizacion </param>
        /// <param name="reporte"> Indica si la información del reporte ya existe </param>
        /// <returns>Regresa la informacion de la cotizacion en la pantalla del pdf </returns>

        public List<CotizacionRpt> ConsultaRpt(int idCotizacion, int reporte)
        {
            try
            {
                List<CotizacionRpt> registros = _cotizacionRepository.ConsultaRpt(idCotizacion);
                List<beResultados> rutina = new List<beResultados>();
                List<bePorcenLegales> LisTabPL = new List<bePorcenLegales>();
                List<Beneficiario> beneficiarios = new List<Beneficiario>();

                beneficiarios = _beneficiarioRepository.ConsultarBeneficiariosModificar(idCotizacion);
                string clavePension = registros[0].CodigoPension;

                DataTable rutinaDt = new DataTable();
                rutinaDt.Columns.Add("Marcasob", typeof(string));
                rutinaDt.Columns.Add("MtoAjusteipc", typeof(double));
                rutinaDt.Columns.Add("MtoPension", typeof(double));
                rutinaDt.Columns.Add("MtoPriunidif", typeof(double));
                rutinaDt.Columns.Add("MtoResmat", typeof(double));
                rutinaDt.Columns.Add("NumCorrelativo", typeof(int));
                rutinaDt.Columns.Add("NumCotestudio", typeof(string));
                rutinaDt.Columns.Add("PrcPercon", typeof(double));
                rutinaDt.Columns.Add("PrcTasatce", typeof(double));
                rutinaDt.Columns.Add("PrcTasatir", typeof(double));
                rutinaDt.Columns.Add("PrcTasavta", typeof(double));
                rutinaDt.Columns.Add("PrimaUnica", typeof(double));

                if (reporte == 1)
                    rutina = _cotizacionRepository.ConsultaRutina(idCotizacion, rutinaDt);

                foreach (var item in rutina)
                {
                    List<CotizacionRpt> registrosRpt = new List<CotizacionRpt>();
                    registrosRpt = (from r in registros where r.IdModalidad == item.NUM_CORRELATIVO select r).ToList();

                    string signo = "S/ ";

                    foreach (var registroRpt in registrosRpt)
                    {
                        string moneda = registroRpt.Moneda;
                        string tipoCambio = registroRpt.TipoCambio;

                        if (moneda == "US")
                        {
                            registroRpt.MontoPension = Convert.ToDouble(item.MTO_PENSION);
                            signo = "USD ";
                        }
                        else
                            registroRpt.MontoPension = Convert.ToDouble(item.MTO_PENSION);

                        double porcentaje = (from b in beneficiarios where b.IdBeneficiario == registroRpt.IdBeneficiario select b.PorcentajeBenDbl).First();

                        string tipRen = registroRpt.CodigoTiposRenta;
                        switch (tipRen)
                        {
                            case "1":
                                {
                                    registroRpt.MontoPensionStr = registroRpt.Cic.ToString("N2");
                                    registroRpt.PrimerTramoStr = registroRpt.PrimerTramo.ToString("N2");
                                    //registroRpt.MontoPensionStr = signo + (Convert.ToDecimal(registroRpt.MontoPension * (porcentaje / 100))).ToString("N2");
                                    //if (moneda == "US")
                                    //{
                                    //    registroRpt.SegundoTramoStr = signo + (Convert.ToDecimal((registroRpt.MontoPension * (porcentaje / 100)) * Convert.ToDouble(tipoCambio))).ToString("N2");//signo + registroRpt.PrimerTramo.ToString("N2");
                                    //    registroRpt.SegundoTramoStr = signo + (Convert.ToDecimal((registroRpt.MontoPension * (porcentaje / 100)) * Convert.ToDouble(tipoCambio))).ToString("N2");
                                    //}
                                    //else
                                    registroRpt.SegundoTramoStr = signo + (Convert.ToDecimal(registroRpt.MontoPension * (porcentaje / 100))).ToString("N2");

                                    registroRpt.AniosDiferidos = registroRpt.AniosDiferidos;
                                    registroRpt.RentaEscalonada = registroRpt.RentaEscalonada;
                                    break;
                                }
                            case "2":
                                {

                                    registroRpt.MontoPensionStr = item.MTO_PRIUNIDIF.ToString("N2");/*((registroRpt.Cic * registroRpt.PorcentajeRentaTemporal) / 100).ToString("N2");*/
                                    if (moneda == "US")
                                    {
                                        registroRpt.PrimerTramoStr = (Convert.ToDecimal(((registroRpt.MontoPension * (porcentaje / 100)) * 2) * Convert.ToDouble(tipoCambio))).ToString("N2");//signo + registroRpt.PrimerTramo.ToString("N2");
                                        registroRpt.MontoPensionStr = (Convert.ToDouble(item.MTO_PRIUNIDIF) * Convert.ToDouble(tipoCambio)).ToString("N2");
                                    }
                                    else
                                    {
                                        registroRpt.PrimerTramoStr = (Convert.ToDecimal(((registroRpt.MontoPension * (porcentaje / 100)) * 2))).ToString("N2");
                                        registroRpt.MontoPensionStr = (Convert.ToDouble(item.MTO_PRIUNIDIF)).ToString("N2");
                                    }

                                    // registroRpt.MontoPensionStr = signo + (Convert.ToDecimal(registroRpt.MontoPension * (porcentaje / 100))).ToString("N2");
                                    registroRpt.SegundoTramoStr = signo + (Convert.ToDecimal(registroRpt.MontoPension * (porcentaje / 100))).ToString("N2");
                                    registroRpt.AniosDiferidos = registroRpt.AniosDiferidos;
                                    registroRpt.RentaEscalonada = registroRpt.RentaEscalonada;
                                    break;
                                }
                            case "6":
                                {
                                    //if (moneda == "US")
                                    //{
                                    //    registroRpt.PrimerTramoStr = (Convert.ToDecimal(registroRpt.MontoPension * (porcentaje / 100)) * Convert.ToDecimal(tipoCambio)).ToString("N2");
                                    //    registroRpt.SegundoTramoStr = signo + (((registroRpt.MontoPension * (porcentaje / 100)) * (Convert.ToDouble(registroRpt.SegundoTramo) / 100)) * Convert.ToDouble(tipoCambio)).ToString("N2");
                                    //}
                                    //else
                                    //{
                                    registroRpt.PrimerTramoStr = (Convert.ToDecimal(registroRpt.MontoPension * (porcentaje / 100))).ToString("N2");
                                    registroRpt.SegundoTramoStr = signo + ((registroRpt.MontoPension * (porcentaje / 100)) * (Convert.ToDouble(registroRpt.SegundoTramo) / 100)).ToString("N2");
                                    //}
                                    registroRpt.MontoPensionStr = registroRpt.Cic.ToString("N2");
                                    registroRpt.AniosDiferidos = Convert.ToInt32(registroRpt.PrimerTramo);
                                    registroRpt.RentaEscalonada = registroRpt.SegundoTramo;
                                    break;
                                }

                        }

                        registroRpt.TasaVenta = item.PRC_TASAVTA.ToString("N2") + "%";
                        registroRpt.PRC_TASATIR = item.PRC_TASATIR;
                        registroRpt.PRC_PERCON = item.PRC_PERCON;
                        registroRpt.MTO_PENSION = item.MTO_PENSION;
                        registroRpt.MTO_RENTATMPAFP = item.MTO_RENTATMPAFP;
                        // registroRpt.SegundoTramo = (Convert.ToDecimal(item.MTO_PENSION)) * (porcentaje / 100);

                        registroRpt.PorcentajeBeneficiario = Convert.ToDecimal(porcentaje);
                        if (tipRen == "6")
                            registroRpt.MontoPension = registroRpt.MontoPension * (Convert.ToDouble(registroRpt.SegundoTramo) / 100);
                        else
                            registroRpt.MontoPension = registroRpt.MontoPension * (porcentaje / 100);
                    }

                    if (clavePension == "08")
                    {
                        double sumaPension = (from r in registrosRpt where r.Parentesco != "TITULAR" select r.MontoPension).Sum();
                        double sumaPrima = (from p in registrosRpt select Convert.ToDouble(p.MontoPensionStr)).Sum();
                        double sumaPT = (from pt in registrosRpt select Convert.ToDouble(pt.PrimerTramoStr)).Sum();

                        CotizacionRpt titular = (from t in registrosRpt where t.Parentesco == "TITULAR" select t).First();
                        titular.Parentesco = "GRUPO FAMILIAR";
                        // titular.MontoPensionStr = //sumaPrima.ToString("N2");//signo + "0.00";
                        titular.PrimerTramoStr = sumaPT.ToString("N2");
                        titular.SegundoTramoStr = signo + sumaPension.ToString("N2");
                        titular.FechaNacimientoBen = "-----";
                        titular.SituacionInvalidez = "-----";
                        titular.Sexo = "--";

                    }
                }

                return registros;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas");
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

        public Response ValidaDocumento(int idTipoDocumento)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _cotizacionRepository.ValidaDocumento(idTipoDocumento);
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
        /// 2018-05-18
        /// Obtiene el tipo de cambio actual
        /// </summary>
        /// <returns> Regresa una cadena que contiene el tipo de campo actual </returns>

        public string ConsultaTipoCambio()
        {
            return _rutinaRepository.ConsultaTipoCambio().Elemento;
        }

        public Response EliminaCotizacion(int idCotizacion)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                _cotizacionRepository.EliminaCotizacion(idCotizacion);
                res.Message = "Cotización eliminada con éxito";
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

        public string ConsultaGastoSepelio()
        {
            return _rutinaRepository.GastoSepelio().Elemento;
        }

        /// <summary>
        /// Lizbeth Morales / Antonio Quezada
        /// 2018-08-08
        /// Obtiene el Departamento, Provincia y Distrito que se deben cargar al inicio de la vista
        /// </summary>
        /// <returns> Regresa un objeto que contiene el Departamento, Provincia y Distrito que se deben cargar al inicio de la vista</returns>

        public Response ConsultarLocalidad()
        {
            try
            {
                Response res = new Response();
                List<Departamento> departamentos = new List<Departamento>();
                List<Provincia> provincias = new List<Provincia>();
                List<Distrito> distritos = new List<Distrito>();

                int idDepartamento = 0;
                int idProvincia = 0;
                int idDistrito = 0;

                departamentos = _cotizacionRepository.Departamentos();
                provincias = _cotizacionRepository.Provincias();
                distritos = _cotizacionRepository.Distritos();

                idDepartamento = (from d in departamentos where d.Elemento == "LIMA" select d.IdDepartamento).First();
                idProvincia = (from p in provincias where p.Elemento == "LIMA" select p.IdProvincia).First();
                idDistrito = (from d in distritos where d.Elemento == "LIMA" select d.IdDistrito).First();

                res.IsOk = true;
                res.Message = "Operación realizada con éxito";
                res.Object = new { idDepartamento = idDepartamento, provincias = provincias, idProvincia = idProvincia, distritos = distritos, idDistrito = idDistrito };

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

        #region Catalogos

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Departamentos </returns>

        public List<Departamento> Departamentos()
        {
            return _cotizacionRepository.Departamentos();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-07
        /// </summary>
        /// <returns> Regresa una lista de Provincias </returns>

        public List<Provincia> Provincias()
        {
            return _cotizacionRepository.Provincias();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-07
        /// </summary>
        /// <returns> Regresa una lista de Distritos </returns>

        public List<Distrito> Distritos()
        {
            return _cotizacionRepository.Distritos();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Tipos de Documentos </returns>

        public List<TipoDocumento> TiposDocumentos()
        {
            return _cotizacionRepository.TiposDocumentos();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Provincias </returns>

        public Response Provincias(int idDepartamento)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _cotizacionRepository.Provincias(idDepartamento);
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
        /// </summary>
        /// <returns> Regresa una lista de Distritos </returns>

        public Response Distritos(int idProvincia)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _cotizacionRepository.Distritos(idProvincia);
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
        /// </summary>
        /// <returns> Regresa una lista de AFP's </returns>

        public List<Afp> AFP()
        {
            return _cotizacionRepository.AFP();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Pensiones </returns>

        public List<Pension> Pensiones()
        {
            return _cotizacionRepository.Pensiones();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Parentescos </returns>

        public List<Parentesco> Parentescos()
        {
            return _cotizacionRepository.Parentescos();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Situaciones de Invalidez </returns>

        public List<SituacionInvalidez> SituacionesInvalidez()
        {

            return _cotizacionRepository.SituacionesInvalidez();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Monedas </returns>

        public List<Moneda> Monedas()
        {
            return _cotizacionRepository.Monedas();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Modalidades </returns>

        public List<ModalidadCat> Modalidades()
        {
            return _cotizacionRepository.Modalidades();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Tipos de Renta </returns>

        public List<TipoRenta> TiposRenta()
        {
            return _cotizacionRepository.TiposRenta();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Paquetes </returns>

        public List<Paquete> Paquetes()
        {
            return _cotizacionRepository.Paquetes();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Sexos </returns>

        public List<Sexo> Sexos()
        {
            return _cotizacionRepository.Sexos();
        }

        /// <summary>
        /// Lizbeth Morales
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Asesores </returns>

        public List<gzUser> Asesores()
        {
            return _cotizacionRepository.Asesores();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-04-20
        /// </summary>
        /// <returns> Regresa una lista de comisiones </returns>

        public int Comisiones()
        {
            return _cotizacionRepository.Comisiones().IdComision;
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-09-07
        /// </summary>
        /// <param name="term"> Parte del CUSPP con la que deben empezar los CUSPP a consultar </param>
        /// <returns> Regresa una lista con CUSPP </returns>

        public List<string> ConsultaCUSPP(string term)
        {
            try
            {
                List<string> listaCuspp = new List<string>();
                DataTable listaCusppDt = new DataTable();

                listaCusppDt = _cotizacionRepository.ConsultaCUSPP(term);

                foreach (DataRow item in listaCusppDt.Rows)
                    listaCuspp.Add(item[0].ToString());

                return listaCuspp;
            }
            catch (Exception ex)
            {
                Console.Write("Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas");
                return null;
            }
        }

        #endregion

        public string ConsultarDataCotizacionExtraOficial(string bandera, string parametro)
        {
            var result = _cotizacionRepository.ConsultarDataCotizacionExtraOficial(bandera, parametro);

            if (result != null)
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// Adrian Mechato Valencia
        /// 2023-01-20
        /// Registra o modicia la cotización
        /// </summary>
        /// <param name="bandera"> Indica si se va a registrar o modificar una cotización </param>
        /// <param name="cotizacion"> Objeto que contiene la información de la cotización y del asegurado </param>
        /// <param name="idsBeneficiarios"> Lista de ids de beneficiarios pertenecientes a la cotización </param>
        /// <param name="idsModalidades"> Lista de ids de modalidades pertenecientes a la cotización </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto representante al detalle de una cotización </returns>

        public Response RegistrarModificarCotizacionDetalle(string bandera, Cotizacion cotizacion, List<string> idsBeneficiarios, List<string> idsModalidades)
        {
            try
            {
                Response res = new Response();
                Cotizacion cotizacionRpt = new Cotizacion();
                res.IsOk = true;

                // Beneficiarios
                XmlConfigurator.Configure();
                _log.Info("*******************************Se comenzará a cotizar EXTRAOFICIALES***************************************");
                _log.Info("Registro de Beneficiarios");
                DataTable idsBeneficiariosDT = new DataTable();
                idsBeneficiariosDT.Columns.Add("IdBeneficiario", typeof(int));

                if (idsBeneficiarios != null)
                {
                    foreach (var item in idsBeneficiarios)
                    {
                        DataRow row = idsBeneficiariosDT.NewRow();
                        row["IdBeneficiario"] = item;
                        idsBeneficiariosDT.Rows.Add(row);
                    }
                }

                // Modalidades
                _log.Info("Registro de Modalidades");
                DataTable idsModalidadesDT = new DataTable();
                idsModalidadesDT.Columns.Add("IdModalidad", typeof(int));

                foreach (var item in idsModalidades)
                {
                    DataRow row = idsModalidadesDT.NewRow();
                    row["IdModalidad"] = item;
                    idsModalidadesDT.Rows.Add(row);
                }

                cotizacionRpt = _cotizacionRepository.RegistrarModificarCotizacion(bandera, cotizacion, idsBeneficiariosDT, idsModalidadesDT);
                res.Object = cotizacionRpt;
                _log.Info("Registro de cotizacion");
                if (bandera == "C")
                    res.Message = "Cotización creada con éxito.";
                else
                {
                    _beneficiarioRepository.EliminarBeneficiario(cotizacionRpt.IdCotizacion);
                    _modalidadRepository.EliminarModalidad(cotizacionRpt.IdCotizacion);
                    res.Message = "Cotización modificada con éxito.";
                }

                // Cálculo de la rutina
                int idCotizacion = cotizacionRpt.IdCotizacion;
                string mensaje = "";

                List<beResultados> rutina = new List<beResultados>();

                DataTable rutinaDt = new DataTable();
                rutinaDt.Columns.Add("Marcasob", typeof(string));
                rutinaDt.Columns.Add("MtoAjusteipc", typeof(double));
                rutinaDt.Columns.Add("MtoPension", typeof(double));
                rutinaDt.Columns.Add("MtoPriunidif", typeof(double));
                rutinaDt.Columns.Add("MtoResmat", typeof(double));
                rutinaDt.Columns.Add("NumCorrelativo", typeof(int));
                rutinaDt.Columns.Add("NumCotestudio", typeof(string));
                rutinaDt.Columns.Add("PrcPercon", typeof(double));
                rutinaDt.Columns.Add("PrcTasatce", typeof(double));
                rutinaDt.Columns.Add("PrcTasatir", typeof(double));
                rutinaDt.Columns.Add("PrcTasavta", typeof(double));
                rutinaDt.Columns.Add("PrimaUnica", typeof(double));

                _log.Info("Comenzara a ejecutar la rutina");
                rutina = _pruebaRutinaProcess.Rutina(idCotizacion);
                _log.Info("Termino ejecucion de rutina");
                foreach (var item in rutina)
                {
                    if (item.Mensaje == null)
                    {
                        DataRow row = rutinaDt.NewRow();
                        row["Marcasob"] = item.MARCASOB;
                        row["MtoAjusteipc"] = item.MTO_AJUSTEIPC;
                        row["MtoPension"] = item.MTO_PENSION;
                        row["MtoPriunidif"] = item.MTO_PRIUNIDIF;
                        row["MtoResmat"] = item.MTO_RESMAT;
                        row["NumCorrelativo"] = item.NUM_CORRELATIVO;
                        row["NumCotestudio"] = item.NUM_COTESTUDIO;
                        row["PrcPercon"] = item.PRC_PERCON;
                        row["PrcTasatce"] = item.PRC_TASATCE;
                        row["PrcTasatir"] = item.PRC_TASATIR;
                        row["PrcTasavta"] = item.PRC_TASAVTA;
                        row["PrimaUnica"] = item.PRIMA_UNICA;

                        rutinaDt.Rows.Add(row);
                    }
                    else
                    {
                        mensaje = item.Mensaje;
                        break;
                    }
                }

                // Serializar la tabla de datos a JSON
                string jsonRutina = JsonConvert.SerializeObject(rutinaDt, Formatting.Indented);

                // Imprimir el JSON resultante
                Console.WriteLine(jsonRutina);



                if (mensaje == "" || mensaje == null)
                {
                    _log.Info("Se guardara informacion");
                    _cotizacionRepository.RegistroRutina(idCotizacion, rutinaDt);
                    _log.Info("Se guardo informacion");
                }
                else
                {
                    res.Message = res.Message + " Ocurrió el siguiente error en el cálculo de la tasa: \n" + mensaje;
                    res.Object = new { idCotizacion = idCotizacion, bandera = bandera };
                    res.IsOk = false;
                }
                res.Object = rutinaDt;
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                _log.Info("ERROR Registro cotizacion:" + ex);
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";

                return res;
            }
        }
    }
}