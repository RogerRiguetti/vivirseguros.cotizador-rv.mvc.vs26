using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class BeneficiariosRepository
    {
        /// <summary>
        /// Antonio Quezada
        /// 2018-03-12
        /// Método que registra o modifica Beneficiarios
        /// </summary>
        /// <param name="bandera"> Indica si se va a registrar o modificar un beneficiario </param>
        /// <param name="beneficiario"> Objeto que contiene la información del beneficiario </param>
        /// <returns> Regresa la información del beneficiario registrado o modificado </returns>

        public Beneficiario RegistrarModificarBeneficiario(char bandera, Beneficiario beneficiario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, bandera, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdBeneficiario", SqlDbType.Int, beneficiario.IdBeneficiario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, beneficiario.Nombres, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, beneficiario.Apellidos, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, beneficiario.Documento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaNacimiento", SqlDbType.DateTime, beneficiario.FechaNacimiento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSexo", SqlDbType.Int, beneficiario.IdSexo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdParentesco", SqlDbType.Int, beneficiario.IdParentesco, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, beneficiario.IdTipoDocumento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSituacionInvalidez", SqlDbType.Int, beneficiario.IdSituacionInvalidez, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeBeneficiario", SqlDbType.Float, beneficiario.PorcentajeBenDbl, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                if (string.IsNullOrEmpty(beneficiario.FechaInvalidezStr))
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInvalidez", SqlDbType.Date, DBNull.Value, ParameterDirection.Input));
                else
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInvalidez", SqlDbType.Date, beneficiario.FechaInvalidezStr, ParameterDirection.Input));

                if (string.IsNullOrEmpty(beneficiario.FechaFallecimientoStr))
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaFallecimiento", SqlDbType.Date, DBNull.Value, ParameterDirection.Input));
                else
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaFallecimiento", SqlDbType.Date, beneficiario.FechaFallecimientoStr, ParameterDirection.Input));

                return VCEDBContext<Beneficiario>.CallStoreProcedure(StoredProcedures.VCE_CatalogoBeneficiarios, parameters, x => new Beneficiario
                {
                    IdBeneficiario = x.GetInt32(0)
                }).FirstOrDefault();

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
        /// Consulta el beneficiario a modificar
        /// </summary>
        /// <param name="idBeneficiario"> Id del beneficiario a eliminar </param>
        /// <returns> Regresa el beneficiario a modificar </returns>

        public Beneficiario ConsultarBeneficiario(int idBeneficiario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'R', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdBeneficiario", SqlDbType.Int, idBeneficiario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaNacimiento", SqlDbType.DateTime, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSexo", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdParentesco", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSituacionInvalidez", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInvalidez", SqlDbType.Date, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaFallecimiento", SqlDbType.Date, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeBeneficiario", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<Beneficiario>.CallStoreProcedure(StoredProcedures.VCE_CatalogoBeneficiarios, parameters, x => new Beneficiario
                {
                    Nombres = x.GetString(0),
                    Apellidos = x.GetString(1),
                    Documento = x.IsDBNull(2) ? "" : x.GetString(2),
                    FechaNacimientoStr = x.GetDateTime(3).ToString("dd/MM/yyyy"),
                    FechaNacimiento = x.GetDateTime(3),
                    IdSexo = x.GetInt32(4),
                    IdParentesco = x.IsDBNull(5) ? 0 : x.GetInt32(5),
                    IdTipoDocumento = x.IsDBNull(6) ? 0 : x.GetInt32(6),
                    IdSituacionInvalidez = x.GetInt32(7),
                    FechaInvalidezStr = x.IsDBNull(8) ? "" : x.GetDateTime(8).ToString("dd/MM/yyyy"),
                    FechaInvalidezRut = x.IsDBNull(8) ? "" : x.GetDateTime(8).ToString("yyyyMMdd"),
                    FechaFallecimientoStr = x.IsDBNull(9) ? "" : x.GetDateTime(9).ToString("dd/MM/yyyy"),
                    CodigoElemento = x.IsDBNull(10) ? "" : x.GetString(10),
                    Parentesco = x.IsDBNull(11) ? "" : x.GetString(11),
                    ClaveSexo = x.GetString(12),
                    ClaveSituacionInvalidez = x.GetString(13),
                    PorcentajeBenDbl = x.GetDouble(14),
                    SituacionInvalidez = x.GetString(15)
                }).FirstOrDefault();
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
        /// Elimina al beneficiario de base de datos
        /// </summary>
        /// <param name="idBeneficiario"> Id del beneficiario a eliminar </param>
        /// <returns> Regresa un 1 si se eliminó el beneficiario y 0 si hubo un error </returns>

        public int EliminarBeneficiario(int idCotizacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'D', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdBeneficiario", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaNacimiento", SqlDbType.DateTime, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSexo", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdParentesco", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSituacionInvalidez", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInvalidez", SqlDbType.Date, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaFallecimiento", SqlDbType.Date, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeBeneficiario", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.VCE_CatalogoBeneficiarios, parameters);
                return idCotizacion;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public int EliminarBeneficiarioC(int idBeneficiario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, "DC", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdBeneficiario", SqlDbType.Int, idBeneficiario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaNacimiento", SqlDbType.DateTime, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSexo", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdParentesco", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSituacionInvalidez", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInvalidez", SqlDbType.Date, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaFallecimiento", SqlDbType.Date, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeBeneficiario", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.VCE_CatalogoBeneficiarios, parameters);
                return idBeneficiario;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }


        public int BajaTemporal(int idBeneficiario)
        {
            string querys = "";
            try
            {
               querys += "\nUPDATE Beneficiarios SET Estado = '0' WHERE IdBeneficiario = " + idBeneficiario;
                EjecutarScript(querys);
                
                return idBeneficiario;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ValidacionRegresar(int idCotizacion)
        {
            string querys = "";
            try
            {
                querys += "\nUPDATE Beneficiarios SET Estado = '1' WHERE IdCotizacion = " + idCotizacion;
                EjecutarScript(querys);
                return idCotizacion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EjecutarScript(string script)
        {
            try
            {
                VCEDBContext<DataTable>.CallSelectStatementDt(script, x => new DataTable());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-21
        /// Carga los beneficiarios del asegurado de Jubilare
        /// </summary>
        /// <param name="numeroDocumento"> Número del documento </param>
        /// <param name="tipoDocumento"> Tipo de Documento </param>
        /// <returns> Una lista de beneficiarios relacionados con el asegurado </returns>

        public List<Beneficiario> ConsultaBeneficiarios(string numeroDocumento, string tipoDocumento, string cuspp)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'J', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdBeneficiario", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, numeroDocumento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaNacimiento", SqlDbType.DateTime, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSexo", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdParentesco", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSituacionInvalidez", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, tipoDocumento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInvalidez", SqlDbType.Date, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaFallecimiento", SqlDbType.Date, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeBeneficiario", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, cuspp, ParameterDirection.Input));

                return VCEDBContext<Beneficiario>.CallStoreProcedure(StoredProcedures.VCE_CatalogoBeneficiarios, parameters, x => new Beneficiario
                {
                    Parentesco = x.IsDBNull(0) ? "" : x.GetString(0),
                    TipoDocumento = x.IsDBNull(1) ? "" : x.GetString(1),
                    Documento = x.GetString(2),
                    FechaNacimientoStr = x.GetDateTime(3).ToString("dd/MM/yyyy"),
                    Sexo = x.GetString(4),
                    SituacionInvalidez = x.GetString(5),
                    IdBeneficiario = x.GetInt32(6)
                    //PorcentajeBen = x.IsDBNull(7) ? "0%" : x.GetDecimal(7).ToString("N2") + "%"
                }).ToList(); ;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Lizbeth Morales 22/03/2018
        /// Metodo que nos trae la consulta de los beneficiarios que se modificaran 
        /// </summary>
        /// <param name="idCotizacion">Id de la cotizacion que se modificara</param>
        /// <returns>Regresa Los datos de los beneficiarios </returns>
        public List<Beneficiario> ConsultarBeneficiariosModificar(int idCotizacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'B', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdBeneficiario", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNombres", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pApellidos", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaNacimiento", SqlDbType.DateTime, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSexo", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdParentesco", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoDocumento", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSituacionInvalidez", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipoDocumento", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaInvalidez", SqlDbType.Date, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaFallecimiento", SqlDbType.Date, DateTime.Now, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeBeneficiario", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<Beneficiario>.CallStoreProcedure(StoredProcedures.VCE_CatalogoBeneficiarios, parameters, x => new Beneficiario
                {
                    Parentesco = x.IsDBNull(0) ? "" : x.GetString(0),
                    TipoDocumento = x.IsDBNull(1) ? "" : x.GetString(1),
                    Documento = x.GetString(2),
                    FechaNacimientoStr = x.GetDateTime(3).ToString("dd/MM/yyyy"),
                    FechaNacimiento = x.GetDateTime(3),
                    Sexo = x.GetString(4),
                    SituacionInvalidez = x.GetString(5),
                    IdBeneficiario = x.GetInt32(6),
                    ClaveSexo = x.GetString(7),
                    FechaInvalidezStr = x.IsDBNull(8) ? "" : x.GetDateTime(8).ToString("dd/MM/yyyy"),
                    FechaFallecimientoStr = x.IsDBNull(9) ? "" : x.GetDateTime(9).ToString("dd/MM/yyyy"),
                    FechaInvalidezRut = x.IsDBNull(8) ? "" : x.GetDateTime(8).ToString("yyyyMMdd"),
                    ClaveSituacionInvalidez = x.GetString(10),
                    CodigoParentesco = x.IsDBNull(11) ? "" : x.GetString(11),
                    PorcentajeBen = x.GetDouble(12).ToString("N2") + "%",
                    PorcentajeBenDbl = x.GetDouble(12)
                }).ToList(); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-23
        /// Realiza una copia de los beneficiarios de la cotización
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotización que se modificara (Beneficiarios)</param>
        /// <returns> Lista de beneficiarios registrados </returns>

        public List<Beneficiario> ClonarBeneficiarios(int idCotizacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, "CONBENEFICIARIOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));

                return VCEDBContext<Beneficiario>.CallStoreProcedure(StoredProcedures.VCE_ClonacionCotizaciones, parameters, x => new Beneficiario
                {
                    Parentesco = x.GetString(0),
                    TipoDocumento = x.IsDBNull(1) ? "" : x.GetString(1),
                    Documento = x.GetString(2),
                    FechaNacimientoStr = x.GetDateTime(3).ToString("dd/MM/yyyy"),
                    Sexo = x.GetString(4),
                    SituacionInvalidez = x.GetString(5),
                    IdBeneficiario = x.GetInt32(6),
                    PorcentajeBen = x.GetDouble(7).ToString("N2") + "%",
                    PorcentajeBenDbl = x.GetDouble(7)
                }).ToList(); ;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Lizbeth Morales
        /// 16/07/2018
        /// Metodo el cual consulta la clave de la pension seleccionada
        /// </summary>
        /// <param name="idTipoPension"> Id de la pension seleccionada </param>
        /// <returns>clave de la pension</returns>

        public Pension TipoPension(int idTipoPension)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoPension", SqlDbType.Int, idTipoPension, ParameterDirection.Input));

                return VCEDBContext<Pension>.CallStoreProcedure(StoredProcedures.VCE_Consulta_TipoPension, parameters, x => new Pension
                {
                    Clave = x.GetString(0),
                    Elemento = x.GetString(1)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
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

        public List<SituacionInvalidez> ConsultarSituacionesInvalidez(int idParentesco, int idPension)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pIdParentesco", SqlDbType.Int, idParentesco, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pIdPension", SqlDbType.Int, idPension, ParameterDirection.Input));

                return VCEDBContext<SituacionInvalidez>.CallStoreProcedure(StoredProcedures.VCE_ConsultaSituacionesInvalidez, parameters, x => new SituacionInvalidez
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

    }
}
