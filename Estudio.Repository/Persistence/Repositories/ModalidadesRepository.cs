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
    public class ModalidadesRepository
    {
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
        /// <returns> Regresa la modalidad afectada en la operación </returns>

        public Modalidad RegistrarModificarModalidad(string bandera, int idModalidad, Modalidad modalidad)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, bandera, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidad", SqlDbType.Int, idModalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosDiferidos", SqlDbType.Int, modalidad.AniosDiferidos, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentabilidadAfp", SqlDbType.Decimal, modalidad.PorcentajeRentabilidadAfp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosGarantizados", SqlDbType.Int, modalidad.AniosGarantizados, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGratificacion", SqlDbType.VarChar, modalidad.Gratificacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentaTemporal", SqlDbType.Int, modalidad.PorcentajeRentaTemporal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdMoneda", SqlDbType.Int, modalidad.IdMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoRenta", SqlDbType.Int, modalidad.IdTipoRenta, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidadCat", SqlDbType.Int, modalidad.IdModalidadCat, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrimerTramo", SqlDbType.Decimal, modalidad.PrimerTramo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSegundoTramo", SqlDbType.Decimal, modalidad.SegundoTramo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPaquete", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdComision", SqlDbType.Int, modalidad.IdComision, ParameterDirection.Input));

                return VCEDBContext<Modalidad>.CallStoreProcedure(StoredProcedures.VCE_CatalogoModalidades, parameters, x => new Modalidad
                {
                    IdModalidad = x.GetInt32(0)
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
        /// 2018-03-15
        /// Consulta la modalidad a modificar
        /// </summary>
        /// <param name="idModalidad"> Id de la modalidad a consultar </param>
        /// <returns> Regresa la modalidad consultada </returns>

        public Modalidad ConsultarModalidad(int idModalidad)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'R', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidad", SqlDbType.Int, idModalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosDiferidos", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentabilidadAfp", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosGarantizados", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGratificacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentaTemporal", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdMoneda", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoRenta", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidadCat", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrimerTramo", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSegundoTramo", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPaquete", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdComision", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<Modalidad>.CallStoreProcedure(StoredProcedures.VCE_CatalogoModalidades, parameters, x => new Modalidad
                {
                    IdMoneda = x.GetInt32(0),
                    IdModalidadCat = x.GetInt32(1),
                    IdTipoRenta = x.GetInt32(2),
                    AniosGarantizados = x.GetInt32(3),
                    AniosDiferidos = x.GetInt32(4),
                    Gratificacion = x.GetString(5),
                    PorcentajeRentaTemporal = x.GetInt32(6),
                    PrimerTramo = x.GetDecimal(7),
                    SegundoTramo = x.GetDecimal(8),
                    PorcentajeRentabilidadAfp = x.GetDecimal(9),
                    IdComision = x.GetInt32(10)
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
        /// 2018-03-15
        /// Elimina modalidad de base de datos
        /// </summary>
        /// <param name="idModalidad"> Id de la modalidad a eliminar </param>
        /// <returns> Regresa un 1 si se eliminó la modalidad y 0 si hubo un error </returns>

        public int EliminarModalidad(int idCotizacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'D', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidad", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosDiferidos", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentabilidadAfp", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosGarantizados", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGratificacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentaTemporal", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdMoneda", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoRenta", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidadCat", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrimerTramo", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSegundoTramo", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPaquete", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdComision", SqlDbType.Int, 0, ParameterDirection.Input));

                VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.VCE_CatalogoModalidades, parameters);
                return idCotizacion;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public int EliminarModalidadC(int idModalidad)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, "DC", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidad", SqlDbType.Int, idModalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosDiferidos", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentabilidadAfp", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosGarantizados", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGratificacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentaTemporal", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdMoneda", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoRenta", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidadCat", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrimerTramo", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSegundoTramo", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPaquete", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdComision", SqlDbType.Int, 0, ParameterDirection.Input));

                VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.VCE_CatalogoModalidades, parameters);
                return idModalidad;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-15
        /// Registra todas las modalidades pertenecientes al paquete
        /// </summary>
        /// <param name="idPaquete"> Id del paquete </param>
        /// <param name="porcentajeAfp"> Porcentaje de rentabilidad AFP </param>
        /// <returns> Regresa la lista de las modalidades registradas </returns>

        public List<Modalidad> RegistrarPaquete(int idPaquete, decimal porcentajeAfp)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'P', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidad", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosDiferidos", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentabilidadAfp", SqlDbType.Decimal, porcentajeAfp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosGarantizados", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGratificacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentaTemporal", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdMoneda", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoRenta", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidadCat", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrimerTramo", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSegundoTramo", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPaquete", SqlDbType.Int, idPaquete, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdComision", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<Modalidad>.CallStoreProcedure(StoredProcedures.VCE_CatalogoModalidades, parameters, x => new Modalidad
                {
                    Moneda = x.GetString(0),
                    ModalidadCat = x.GetString(1),
                    TipoRenta = x.GetString(2),
                    AniosGarantizados = x.GetInt32(3),
                    AniosDiferidos = x.GetInt32(4),
                    Gratificacion = x.GetString(5).ToUpper(),
                    PorcentajeRentaTemporal = x.GetInt32(6),
                    PrimerTramoStr = x.GetDecimal(7) == 0 ? "-" : x.GetDecimal(7).ToString("N2"),
                    SegundoTramoStr = x.GetDecimal(8) == 0 ? "-" : x.GetDecimal(8).ToString("N2"),
                    PorcentajeRentabilidadAfp = x.GetDecimal(9),
                    IdModalidad = x.GetInt32(10)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Lizbeth Morales 22/03/2018
        /// Metodo que nos permite modificar las Modalidades
        /// </summary>
        /// <param name="idCotizacion">Id de la cotizacion que sera modificada (sus modalidades)</param>
        /// <returns>Regresa los valores de las modalidades</returns>
        public List<Modalidad> ConsultarModalidadesModificar(int idCotizacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'M', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidad", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosDiferidos", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentabilidadAfp", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAniosGarantizados", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pGratificacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPorcentajeRentaTemporal", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdMoneda", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdTipoRenta", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdModalidadCat", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPrimerTramo", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pSegundoTramo", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPaquete", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdComision", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<Modalidad>.CallStoreProcedure(StoredProcedures.VCE_CatalogoModalidades, parameters, x => new Modalidad
                {
                    Moneda = x.GetString(0),
                    ModalidadCat = x.GetString(1),
                    TipoRenta = x.GetString(2),
                    AniosGarantizados = x.GetInt32(3),
                    AniosDiferidos = x.GetInt32(4),
                    Gratificacion = x.GetString(5).ToUpper(),
                    PorcentajeRentaTemporal = x.GetInt32(6),
                    PrimerTramoStr = x.GetDecimal(7) == 0 ? "-" : x.GetDecimal(7).ToString("N2"),
                    PrimerTramo = x.GetDecimal(7),
                    SegundoTramoStr = x.GetDecimal(8) == 0 ? "-" : x.GetDecimal(8).ToString("N2"),
                    SegundoTramo = x.GetDecimal(8),
                    PorcentajeRentabilidadAfp = x.GetDecimal(9),
                    IdModalidad = x.GetInt32(10),
                    CodigoTiposRenta = x.GetString(11),
                    CodigoModalidad = x.GetString(12),
                    ClaveMoneda = x.GetString(13),
                    CodigoTipoReajuste = x.GetInt32(14),
                    ValorComision = x.GetDecimal(15),
                    DerGra = x.GetString(5) == "Si" ? "S" : "N"
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
        /// 2018-03-23
        /// Realiza una copia de las modalidades de la cotización
        /// </summary>
        /// <param name="idCotizacion"> Id de la cotización </param>
        /// <returns> Lista de modalidades registradas </returns>

        public List<Modalidad> ClonarModalidades(int idCotizacion)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, "CONMODALIDADES", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdCotizacion", SqlDbType.Int, idCotizacion, ParameterDirection.Input));
                
                return VCEDBContext<Modalidad>.CallStoreProcedure(StoredProcedures.VCE_ClonacionCotizaciones, parameters, x => new Modalidad
                {
                    Moneda = x.GetString(0),
                    ModalidadCat = x.GetString(1),
                    TipoRenta = x.GetString(2),
                    AniosGarantizados = x.GetInt32(3),
                    AniosDiferidos = x.GetInt32(4),
                    Gratificacion = x.GetString(5).ToUpper(),
                    PorcentajeRentaTemporal = x.GetInt32(6),
                    PrimerTramoStr = x.GetDecimal(7) == 0 ? "-" : x.GetDecimal(7).ToString("N2"),
                    SegundoTramoStr = x.GetDecimal(8) == 0 ? "-" : x.GetDecimal(8).ToString("N2"),
                    PorcentajeRentabilidadAfp = x.GetDecimal(9),
                    IdModalidad = x.GetInt32(10)
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
        /// 10/07/2018
        /// Metodo el cual realiza la consulta a base de datos del porcentaje de afp del asegurado
        /// dependiendo del tipo de afp seleccionado
        /// </summary>
        /// <param name="afp">afp seleccionado en el combo</param>
        /// <returns>informacion del % afp</returns>
        public DataTable CodigoAFP(string afp)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveConsulta", SqlDbType.VarChar, "CONAFP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecCal", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipPen", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pColumnaMes", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFechaCotizacion", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCodigoMoneda", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pValorComision", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCic", SqlDbType.Decimal, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAfp", SqlDbType.VarChar, afp, ParameterDirection.Input));

                return VCEDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.VCE_ConsultasRutinas, parameters);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<Modalidad> ConsultaValidacionesModalidades(int idPension, int idRenta, int idMoneda)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPension", SqlDbType.Int, idPension, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdRenta", SqlDbType.Int, idRenta, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdMoneda", SqlDbType.Int, idMoneda, ParameterDirection.Input));

                return VCEDBContext<Modalidad>.CallStoreProcedure(StoredProcedures.VCE_ConsultaValidacionesModalidades, parameters, x => new Modalidad
                {
                    AniosDiferidos = x.GetInt32(0),
                    PorcentajeRentaTemporal = x.GetInt32(1),
                    AniosGarantizados = x.GetInt32(2),
                    PrimerTramo = x.GetInt32(3),
                    SegundoTramo = x.GetInt32(4)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public int ValidacionRegresarMod(int idCotizacion)
        {
            string querys = "";
            try
            {
                querys += "\nUPDATE Modalidades SET Estado = '1' WHERE IdCotizacion = " + idCotizacion;
                EjecutarScript(querys);
                return idCotizacion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int BajaTemporal(int idBeneficiario)
        {
            string querys = "";
            try
            {
                querys += "\nUPDATE Modalidades SET Estado = '0' WHERE IdModalidad = " + idBeneficiario;
                EjecutarScript(querys);

                return idBeneficiario;
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

    }
}
