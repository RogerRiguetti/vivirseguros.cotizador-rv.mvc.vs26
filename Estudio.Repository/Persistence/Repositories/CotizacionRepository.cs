using Estudio.Repository.Core.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
   public class CotizacionRepository
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Lizbeth Guadalupe Morales Montiel 09/03/2018
        /// Metodo que manda y trae la informacion a la base de datos de la consulta de las cotizaciones 
        /// </summary>
        /// <param name="idTipoDocumento">Parametro que tiene el tipo de documento que manejara (DNI, Pasaporte, etc.)</param>
        /// <param name="documento">Paramentro que tiene el valor del tipo de documento </param>
        /// <param name="nombres">Parametro que contiene el(los) nombre(s) del usuario  </param>
        /// <param name="apellidos">Parametro que contiene los apellidos del usuario </param>
        /// <param name="asesor">Parametro que contiene el id del usuario </param>
        /// <param name="supervisor">Parametro que contiene los roles  del usuario </param>
        /// <returns>Nos regresara la informacion filtrada ya sea por numero de documento, nombres o apellidos</returns>

        public List<Cotizacion> ConsultarCotizaciones(string claveConsulta, int idTipoDocumento, string documento, string nombres, string apellidos, int asesor, int supervisor, string cuspp)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, claveConsulta, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, idTipoDocumento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, documento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, nombres, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, apellidos, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, asesor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSupervisor", SqlDbType.Int, supervisor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdAsesor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, cuspp, ParameterDirection.Input));
                return VCEDBContext<Cotizacion>.CallStoreProcedure(StoredProcedures.VCE_ConsultasCotizacion, parameters, x => new Cotizacion
                {
                    IdCotizacion = x.GetInt32(0),
                    TipoDocumento = x.GetString(1),
                    Documento = x.GetString(2),
                    CUSPP = x.GetString(3),
                    Nombres = x.GetString(4),
                    ApellidoPaterno = x.GetString(5),
                    ApellidoMaterno = x.GetString(6),
                    FechaCotizacion = x.GetDateTime(7),
                    FechaCotizacionStr = x.GetDateTime(7).ToString("yyyy/MM/dd"),
                    Asesor = x.GetString(8),
                    TipoCambio = x.GetDouble(9).ToString(),
                    Estado = x.GetByte(10)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta cotizaciones " + ex);
                throw;
            }

        }
        
        /// <summary>
        /// Lizbeth Morales 14/03/2018
        /// Metodo que nos trae la informacion cuando el usuario logeado es un supervisor
        /// </summary>
        /// <param name="supervisor"> el id del supervisor</param>
        /// <returns>nos regresa la informacion de los asesores del supervisor logeado</returns>
        public List<gzUser> Asesores(int supervisor)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.Char, "CMBXSUPERVISORES", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, "" , ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSupervisor", SqlDbType.Int, supervisor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdAsesor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_ConsultasCotizacion, parameters, x => new gzUser
                {
                    Id = x.GetInt32(0),
                    NombreCompleto = x.GetString(1)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Lizbeth Morales 15/03/2018
        /// Metodo que nos trae informacion cuando el usuario logeado es un asesor
        /// </summary>
        /// <param name="supervisor">id del asesor</param>
        public List<gzUser> Ejecutivo(int supervisor)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.Char, "CMBXASESORES", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSupervisor", SqlDbType.Int, supervisor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdAsesor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_ConsultasCotizacion, parameters, x => new gzUser
                {
                    Id = x.GetInt32(0),
                    NombreCompleto = x.GetString(1)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Lizbeth Morales
        /// 2018-03-15
        /// Obtiene los asesores existentes en el sistema
        /// </summary>
        /// <returns> Regresa una lista de usuarios con el perfil de asesor </returns>

        public List<gzUser> Asesores()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "ASESORES", ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new gzUser
                {
                    Id = x.GetInt32(0),
                    NombreCompleto = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
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
        /// <returns> Regresa el id de la cotización involucrada </returns>

        public Cotizacion RegistrarModificarCotizacion (char bandera, Cotizacion cotizacion, DataTable idsBeneficiarios, DataTable idsModalidades)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, bandera, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, cotizacion.Documento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCUSPP", SqlDbType.VarChar, cotizacion.CUSPP, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, cotizacion.Nombres, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidoPaterno", SqlDbType.VarChar, cotizacion.ApellidoPaterno, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidoMaterno", SqlDbType.VarChar, cotizacion.ApellidoMaterno, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaNacimiento", SqlDbType.DateTime, cotizacion.FechaNacimiento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaDevengue", SqlDbType.DateTime, cotizacion.FechaDevengue, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCIC", SqlDbType.Decimal, cotizacion.Cic, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdDepartamento", SqlDbType.Char, cotizacion.IdDepartamento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdProvincia", SqlDbType.Int, cotizacion.IdProvincia, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdDistrito", SqlDbType.Int, cotizacion.IdDistrito, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSexo", SqlDbType.Int, cotizacion.IdSexo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdAfp", SqlDbType.Int, cotizacion.IdAfp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPension", SqlDbType.Int, cotizacion.IdPension, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, cotizacion.IdTipoDocumento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGastoSepelio", SqlDbType.Money, cotizacion.GastoSepelio, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoCambio", SqlDbType.Float, cotizacion.TipoCambio, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdAsesor", SqlDbType.Int, cotizacion.IdAsesor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@BeneficiariosType", SqlDbType.Structured, idsBeneficiarios, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@ModalidadesType", SqlDbType.Structured, idsModalidades, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, cotizacion.IdCotizacion, ParameterDirection.Input));

                return VCEDBContext<Cotizacion>.CallStoreProcedure(StoredProcedures.VCE_CatalogoCotizaciones, parameters, x => new Cotizacion
                {
                    IdCotizacion = x.GetInt32(0)
                }).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-16
        /// Consulta el asegurado de la base de datos de jubilare
        /// </summary>
        /// <param name="tipoDocumento"> Tipo de documento </param>
        /// <param name="documento"> Número del documento </param>
        /// <param name="idAsegurado"> Id del asesor en caso de que sea este rol el que ingrese al modulo </param>
        /// <returns> Regresa un objeto que contiene la información del asegurado </returns>

        public List<Cotizacion> ConsultarAsegurado (string tipoDocumento, string documento, int idAsegurado, string cuspp)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONASEGURADO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, documento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSupervisor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, tipoDocumento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdAsesor", SqlDbType.Int, idAsegurado, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, cuspp, ParameterDirection.Input));

                return VCEDBContext<Cotizacion>.CallStoreProcedure(StoredProcedures.VCE_ConsultasCotizacion, parameters, x => new Cotizacion
                {
                    CUSPP = x.GetString(0),
                    Nombres = x.GetString(1),
                    ApellidoPaterno = x.GetString(2),
                    ApellidoMaterno = x.GetString(3),
                    FechaNacimientoStr = x.GetDateTime(4).ToString("dd/MM/yyyy"),
                    FechaDevengueStr = x.IsDBNull(5) ? "" : x.GetDateTime(5).ToString("dd/MM/yyyy"),
                    Cic = x.GetDecimal(6),
                    IdDepartamento = x.IsDBNull(7) ? 0 : x.GetInt32(7),
                    IdProvincia = x.IsDBNull(8) ? 0 : x.GetInt32(8),
                    IdDistrito = x.IsDBNull(9) ? 0 : x.GetInt32(9),
                    IdSexo = x.GetInt32(10),
                    IdAfp = x.IsDBNull(11) ? 0 : x.GetInt32(11),
                    IdPension = x.GetInt32(12),
                    IdAsesor = x.IsDBNull(13) ? 0 : x.GetInt32(13),
                    Afp = x.IsDBNull(14) ? "" : x.GetString(14),
                    Documento = x.GetString(15),
                    IdTipoDocumento = x.GetInt32(16)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-22
        /// Consulta los datos de la cotización
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotización a consultar </param>
        /// <returns> Regrsa un objeto de la clase Cotizacion con la información a mostrar </returns>

        public Cotizacion ConsultarCotizacion(int idCotizacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONMODIFICAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSupervisor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdAsesor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<Cotizacion>.CallStoreProcedure(StoredProcedures.VCE_ConsultasCotizacion, parameters, x => new Cotizacion
                {
                    CUSPP = x.GetString(0),
                    Nombres = x.GetString(1),
                    ApellidoPaterno = x.GetString(2),
                    ApellidoMaterno = x.GetString(3),
                    FechaNacimientoStr = x.GetDateTime(4).ToString("dd/MM/yyyy"),
                    FechaNacimiento = x.GetDateTime(4),
                    FechaDevengueStr = x.GetDateTime(5).ToString("dd/MM/yyyy"),
                    FechaDevengue = x.GetDateTime(5),
                    Cic = x.GetDecimal(6),
                    IdDepartamento = x.GetInt32(7),
                    IdProvincia = x.GetInt32(8),
                    IdDistrito = x.GetInt32(9),
                    IdSexo = x.GetInt32(10),
                    IdAfp = x.GetInt32(11),
                    IdPension = x.GetInt32(12),
                    FechaEstudioStr = x.GetDateTime(13).ToString("dd/MM/yyyy"),
                    FechaEstudio = x.GetDateTime(13),
                    GastoSepelio = x.GetDecimal(14),
                    IdAsesor = x.GetInt32(15),
                    IdTipoDocumento = x.GetInt32(16),
                    Documento = x.GetString(17),
                    CodigoPension = x.GetString(18),
                    ClaveSexo = x.GetString(19),
                    TipoCambio = x.GetDouble(20).ToString(),
                    Afp = x.GetString(21)
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Lizbeth Morales 04/04/2018
        /// Metodo que realiza la consulta para realizar el reporte 
        /// </summary>
        /// <param name="idCotizacion">De la cotizacion a imprimir</param>
        /// <returns>Datos de la consulta para el reporte</returns>
        public List<CotizacionRpt> ConsultaRpt(int idCotizacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONREPORTE", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSupervisor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdAsesor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<CotizacionRpt>.CallStoreProcedure(StoredProcedures.VCE_ConsultasCotizacion, parameters, x => new CotizacionRpt
                {
                    NombreCompletoAse = x.GetString(0),
                    Cuspp = x.GetString(1),
                    Afp = x.GetString(2),
                    // Tasa = x.GetDecimal(3).ToString("N2") + " %",
                    Pension = x.GetString(3),
                    FechaDevengue = x.GetDateTime(4).ToString("dd/MM/yyyy"),
                    Cic = x.GetDecimal(5),
                    FechaEstudio = x.GetDateTime(6).ToString("dd/MM/yyyy"),
                    NombreCompletoBene = x.GetString(7),
                    Moneda = x.GetString(8),
                    Modalidad = x.GetString(9),
                    AniosDiferidos = x.GetInt32(10),
                    PorcentajeRentaTemporal = x.GetInt32(11),
                    AniosGarantizados = x.GetInt32(12),
                    RentaEscalonada = x.GetInt32(13),
                    Parentesco = x.GetString(14),
                    SituacionInvalidez = x.GetString(15),
                    Sexo = x.GetString(16),
                    FechaNacimientoBen = x.GetDateTime(17).ToString("dd/MM/yyyy"),
                    PorcentajeRentabilidadAfp = x.GetDecimal(18),
                    PrimerTramo = x.GetDecimal(19),
                    SegundoTramo = x.GetDecimal(20),
                    IdModalidad = x.GetInt32(21),
                    TipoCambio = x.GetDouble(22).ToString(),
                    IdBeneficiario = x.GetInt32(23),
                    CodigoPension = x.GetString(24),
                    CodigoTiposRenta = x.GetString(25)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-04-10
        /// Obtiene la información para validar el documento registrado
        /// </summary>
        /// <param name="idTipoDocumento"> Id del tipo de documento </param>
        /// <returns> Regresa un objeto que contiene la información del tipo de documento </returns>

        public TipoDocumento ValidaDocumento(int idTipoDocumento)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "VALIDADOCUMENTO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, idTipoDocumento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSupervisor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdAsesor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<TipoDocumento>.CallStoreProcedure(StoredProcedures.VCE_ConsultasCotizacion, parameters, x => new TipoDocumento
                {
                    Longitud = x.GetInt32(0),
                    IndicadorLongitudExacta = x.GetByte(1),
                    Tipo = x.GetString(2)
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-05-15
        /// Consulta la información obtenida del algoritmo de la rutina
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotización a consultar </param>
        /// <param name="pruebaRutina"> Información obtenida en la ejecución de la rutina </param>
        /// <returns> Regresa la información que se obtuvo en la rutina </returns>

        public List<beResultados> ConsultaRutina(int idCotizacion, DataTable pruebaRutina)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.Char, 'R', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@PruebaRutinaType", SqlDbType.Structured, pruebaRutina, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaRutina", SqlDbType.DateTime, DateTime.Now, ParameterDirection.Input));

                return VCEDBContext<beResultados>.CallStoreProcedure(StoredProcedures.VCE_CatalogoRutina, parameters, x => new beResultados
                {
                    MARCASOB = x.GetString(0),
                    MTO_AJUSTEIPC = x.GetDouble(1),
                    MTO_PENSION = x.GetDouble(2),
                    MTO_PRIUNIDIF = x.GetDouble(3),
                    MTO_RESMAT = x.GetDouble(4),
                    NUM_CORRELATIVO = x.GetInt32(5),
                    NUM_COTESTUDIO = x.GetString(6),
                    PRC_PERCON = x.GetDouble(7),
                    PRC_TASATCE = x.GetDouble(8),
                    PRC_TASATIR = x.GetDouble(9),
                    PRC_TASAVTA = x.GetDouble(10),
                    PRIMA_UNICA = x.GetDouble(11)
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-05-15
        /// Registra la información obtenida en la ejecución de la rutina
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotización </param>
        /// <param name="pruebaRutina"> Información obtenida en la ejecución de la rutina </param>

        public void RegistroRutina(int idCotizacion, DataTable pruebaRutina)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.Char, 'C', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@PruebaRutinaType", SqlDbType.Structured, pruebaRutina, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaRutina", SqlDbType.DateTime, DateTime.Now, ParameterDirection.Input));

                VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.VCE_CatalogoRutina, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void EliminaCotizacion(int idCotizacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "ELIMINACOTI", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSupervisor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdAsesor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.VCE_ConsultasCotizacion, parameters);

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-09-07
        /// </summary>
        /// <param name="term"> Parte del CUSPP con la que deben empezar los CUSPP a consultar </param>
        /// <returns> Regresa un DataTable con CUSPP </returns>

        public DataTable ConsultaCUSPP(string term)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCUSPP", SqlDbType.VarChar, term, ParameterDirection.Input));

                return VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.VCE_ConsultaCUSPP, parameters);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
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
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "DEPARTAMENTOS", ParameterDirection.Input));

                return VCEDBContext<Departamento>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Departamento
                {
                    IdDepartamento = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-07
        /// </summary>
        /// <returns> Regresa una lista de Provincias </returns>

        public List<Provincia> Provincias()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "PROVINCIAS", ParameterDirection.Input));

                return VCEDBContext<Provincia>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Provincia
                {
                    IdProvincia = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-07
        /// </summary>
        /// <returns> Regresa una lista de Distritos </returns>

        public List<Distrito> Distritos()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "DISTRITOS", ParameterDirection.Input));

                return VCEDBContext<Distrito>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Distrito
                {
                    IdDistrito = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Tipos de Documentos </returns>

        public List<TipoDocumento> TiposDocumentos()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "TIPODOC", ParameterDirection.Input));

                return VCEDBContext<TipoDocumento>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new TipoDocumento
                {
                    IdTipoDocumento = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
     

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// Lizbeth Morales 
        /// 31/05/2018
        /// </summary>
        /// <returns> Regresa una lista de Provincias </returns>

        public List<Provincia> Provincias(int idDepartamento)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'P', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdDireccion", SqlDbType.Int, idDepartamento, ParameterDirection.Input));

                return VCEDBContext<Provincia>.CallStoreProcedure(StoredProcedures.VCE_ConsultasDirecciones, parameters, x => new Provincia
                {
                    IdProvincia = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// Lizbeth Morales 31/05/2018
        /// </summary>
        /// <returns> Regresa una lista de Distritos </returns>

        public List<Distrito> Distritos(int idProvincia)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'D', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdDireccion", SqlDbType.Int, idProvincia, ParameterDirection.Input));

                return VCEDBContext<Distrito>.CallStoreProcedure(StoredProcedures.VCE_ConsultasDirecciones, parameters, x => new Distrito
                {
                    IdDistrito = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de AFP's </returns>

        public List<Afp> AFP()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "AFP", ParameterDirection.Input));

                return VCEDBContext<Afp>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Afp
                {
                    IdAfp = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Pensiones </returns>

        public List<Pension> Pensiones()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "PENSIONES", ParameterDirection.Input));

                return VCEDBContext<Pension>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Pension
                {
                    IdPension = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Parentescos </returns>

        public List<Parentesco> Parentescos()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "PARENTESCOS", ParameterDirection.Input));

                return VCEDBContext<Parentesco>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Parentesco
                {
                    IdParentesco = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Situaciones de Invalidez </returns>

        public List<SituacionInvalidez> SituacionesInvalidez()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "SITINVALIDEZ", ParameterDirection.Input));

                return VCEDBContext<SituacionInvalidez>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new SituacionInvalidez
                {

                    IdSituacionInvalidez = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Monedas </returns>

        public List<Moneda> Monedas()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "MONEDAS", ParameterDirection.Input));

                return VCEDBContext<Moneda>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Moneda
                {
                    IdMoneda = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Modalidades </returns>

        public List<ModalidadCat> Modalidades()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "MODALIDADES", ParameterDirection.Input));

                return VCEDBContext<ModalidadCat>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new ModalidadCat
                {
                    IdModalidadCat = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Tipos de Renta </returns>

        public List<TipoRenta> TiposRenta()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "TIPOSRENTA", ParameterDirection.Input));

                return VCEDBContext<TipoRenta>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new TipoRenta
                {
                    IdTipoRenta = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Paquetes </returns>

        public List<Paquete> Paquetes()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "PAQUETES", ParameterDirection.Input));

                return VCEDBContext<Paquete>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Paquete
                {
                    IdPaquete = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// </summary>
        /// <returns> Regresa una lista de Sexos </returns>

        public List<Sexo> Sexos()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "SEXO", ParameterDirection.Input));

                return VCEDBContext<Sexo>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Sexo
                {
                    IdSexo = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-04-20
        /// </summary>
        /// <returns> Regresa una lista de comisiones </returns>

        public Comision Comisiones()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "COMISIONES", ParameterDirection.Input));

                return VCEDBContext<Comision>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Comision
                {
                    IdComision = x.GetInt32(0),
                    Elemento = x.GetString(1)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #endregion
    }
}
