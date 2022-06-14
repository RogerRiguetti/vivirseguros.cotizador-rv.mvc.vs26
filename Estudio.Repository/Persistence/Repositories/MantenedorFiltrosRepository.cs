using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Estudio.Repository.Persistence.Repositories
{
    public class MantenedorFiltrosRepository
    {
        public List<string[]> getCombos(string codIndPlan)
        {

            try
            {
                List<string[]> resultados = new List<string[]>();
                resultados.Add(desdeHasta(codIndPlan)); //La edad y el monto prima (Desde - Hasta)
                resultados.Add(obtenerDatosCombo("TR", codIndPlan)); //Tipo de renta
                resultados.Add(obtenerDatosCombo("AL", codIndPlan)); //Modalidad
                resultados.Add(obtenerDatosCombo("SE", codIndPlan)); //Sexo
                resultados.Add(obtenerDatosCombo("EC", codIndPlan)); //Estado civil
                resultados.Add(obtenerDatosCombo("CL", codIndPlan)); //Cliente
                resultados.Add(obtenerDatosCombo("TM", codIndPlan)); //Tipo moneda
                return resultados;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string[] obtenerDatosCombo(string codTabla, string codIndPlan)
        {

            try
            {
                string[] resultados = { "" };
                List<MantenedorFiltros> datos = new List<MantenedorFiltros>();
                var parameters = new List<SqlParameter>();
                if (codTabla == "TM")
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "OBTENERDATOCOMBOM", ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "OBTENERDATOCOMBO", ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTabla", SqlDbType.VarChar, codTabla, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndPlan", SqlDbType.VarChar, codIndPlan, ParameterDirection.Input));
                datos = VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {
                    glsElemento = x.GetString(0),
                    codElemento = x.GetString(1)
                }).ToList();
                if (datos.Count != 0)
                {
                    resultados = new string[datos.Count];
                    for (int i = 0; i < datos.Count; i++)
                    {
                        resultados[i] = datos[i].codElemento + "#" + (datos[i].glsElemento);
                    }
                }
                return resultados;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string[] desdeHasta(string codIndPlan)
        {

            try
            {
                string[] resultados = new string[4];
                MantenedorFiltros datos = new MantenedorFiltros();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "DESDEHASTA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndPlan", SqlDbType.VarChar, codIndPlan, ParameterDirection.Input));
                datos = VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {
                    mtoPriDesde = Convert.ToDouble(x.GetDecimal(0)),
                    mtoPriHasta = Convert.ToDouble(x.GetDecimal(1)),
                    numEdadDesde = x.GetInt32(2),
                    numEdadHasta = x.GetInt32(3)
                }).FirstOrDefault();
                if (datos != null)
                {
                    resultados[0] = datos.mtoPriDesde.ToString();
                    resultados[1] = datos.mtoPriHasta.ToString();
                    resultados[2] = datos.numEdadDesde.ToString();
                    resultados[3] = datos.numEdadHasta.ToString();
                }
                return resultados;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string[]> getInfo(string codTabla)
        {

            try
            {
                List<string[]> resultados = new List<string[]>();
                List<MantenedorFiltros> datos = new List<MantenedorFiltros>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "INFORMACION", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTabla", SqlDbType.VarChar, codTabla, ParameterDirection.Input));
                datos = VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {
                    glsElemento = x.GetString(0),
                    codElemento = x.GetString(1)
                }).ToList();
                if (datos.Count != 0)
                {
                    for (int i = 0; i < datos.Count; i++)
                    {
                        string[] reg = { datos[i].glsElemento, datos[i].codElemento };
                        resultados.Add(reg);
                    }
                }
                return resultados;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string[]> getTipoMoneda()
        {

            try
            {
                List<string[]> resultados = new List<string[]>();
                List<MantenedorFiltros> datos = new List<MantenedorFiltros>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TIPOMONEDA", ParameterDirection.Input));
                datos = VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {
                    glsElemento = x.GetString(0),
                    codElemento = x.GetString(1)
                }).ToList();
                if (datos.Count != 0)
                {
                    for (int i = 0; i < datos.Count; i++)
                    {
                        string[] reg = { datos[i].glsElemento, datos[i].codElemento };
                        resultados.Add(reg);
                    }
                }
                return resultados;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<MantenedorFiltros> verificarDetalle()
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VERIFICADETALLE", ParameterDirection.Input));
                return VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {
                    codIndPlan = x.GetString(0)
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<MantenedorFiltros> aceptar()
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ACEPTAR", ParameterDirection.Input));
                return VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {
                    codIndPlan = x.GetString(0)
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<MantenedorFiltros> recuperarFiltro(string codTabla)
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RECUPERAFILTRO_1", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTabla", SqlDbType.VarChar, codTabla, ParameterDirection.Input));
                return VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {
                    codIndPlan = x.GetString(0),
                    codElemento = x.GetString(1)
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<MantenedorFiltros> recuperarFiltro()
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RECUPERAFILTRO_2", ParameterDirection.Input));
                return VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {
                    codIndPlan = x.GetString(0),
                    mtoPriDesde = Convert.ToDouble(x.GetDecimal(1)),
                    mtoPriHasta = Convert.ToDouble(x.GetDecimal(2)),
                    numEdadDesde = x.GetInt32(3),
                    numEdadHasta = x.GetInt32(4)
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void agregarNuevo(MantenedorFiltros datos, string usuario)
        {

            try
            {
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "AGREGARNUEVO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndPlan", SqlDbType.VarChar, datos.codIndPlan, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndTipRenta", SqlDbType.VarChar, datos.codIndTipRenta, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndModalidad", SqlDbType.VarChar, datos.codIndModalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndCliente", SqlDbType.VarChar, datos.codIndCliente, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndPrima", SqlDbType.VarChar, datos.codIndPrima, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoPriDesde", SqlDbType.Decimal, Convert.ToDecimal(datos.mtoPriDesde), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoPriHasta", SqlDbType.Decimal, Convert.ToDecimal(datos.mtoPriHasta), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndSexo", SqlDbType.VarChar, datos.codIndSexo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndEstCivil", SqlDbType.VarChar, datos.codIndEstCivil, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numEdadDesde", SqlDbType.Int, datos.numEdadDesde, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numEdadHasta", SqlDbType.Int, datos.numEdadHasta, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndMoneda", SqlDbType.VarChar, datos.codIndMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecha", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@hora", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {

                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void agregarNuevoFiltro(MantenedorFiltros datos)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "AGREGARNUEVOFILTRO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndPlan", SqlDbType.VarChar, datos.codIndPlan, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTabla", SqlDbType.VarChar, datos.codTabla, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codElemento", SqlDbType.VarChar, datos.codElemento, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndicador", SqlDbType.Int, datos.codIndicador, ParameterDirection.Input));


                VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {

                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void actualizar(MantenedorFiltros datos, string usuario)
        {

            try
            {
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ACTUALIZAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndPlan", SqlDbType.VarChar, datos.codIndPlan, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndTipRenta", SqlDbType.VarChar, datos.codIndTipRenta, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndModalidad", SqlDbType.VarChar, datos.codIndModalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndCliente", SqlDbType.VarChar, datos.codIndCliente, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndPrima", SqlDbType.VarChar, datos.codIndPrima, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoPriDesde", SqlDbType.Decimal, Convert.ToDecimal(datos.mtoPriDesde), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoPriHasta", SqlDbType.Decimal, Convert.ToDecimal(datos.mtoPriHasta), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndSexo", SqlDbType.VarChar, datos.codIndSexo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndEstCivil", SqlDbType.VarChar, datos.codIndEstCivil, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numEdadDesde", SqlDbType.Int, datos.numEdadDesde, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numEdadHasta", SqlDbType.Int, datos.numEdadHasta, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndMoneda", SqlDbType.VarChar, datos.codIndMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecha", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@hora", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {

                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool buscarFiltro(string codIndPlan)
        {

            try
            {
                List<MantenedorFiltros> _datos = new List<MantenedorFiltros>();
                bool flBuscaFiltro = false;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCAFILTRO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndPlan", SqlDbType.VarChar, codIndPlan, ParameterDirection.Input));
                _datos = VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {
                    codIndPlan = x.GetString(0)
                }).ToList();
                if (_datos.Count != 0)
                {
                    flBuscaFiltro = true;
                }
                return flBuscaFiltro;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void eliminar(string codIndPlan)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ELIMINAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codIndPlan", SqlDbType.VarChar, codIndPlan, ParameterDirection.Input));
                VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasMantenedorFiltros, parameters, x => new MantenedorFiltros
                {
                    codIndPlan = x.GetString(0)
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Metodo que trae la lista con los filtros para cotizar
        /// Lizbeth Morales
        /// 29/01/2019
        /// </summary>
        /// <returns>Lista Con filtros para cotizar</returns>
        public List<MantenedorFiltros> ConsultaDatosFiltros ()
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTADATOS", ParameterDirection.Input));
                return VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasFiltrosSNCotiza, parameters, x => new MantenedorFiltros
                {
                    descripcion = x.GetString(0),
                    codigoElemento = x.GetString(1),
                    codigoTabla = x.GetString(2),
                    codigoPension = x.GetString(3)
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Metodo que trae la lista con los filtros para cotizar (moneda)
        /// Lizbeth Morales
        /// 29/01/2019
        /// </summary>
        /// <returns>Lista Con filtros para cotizar</returns>
        public List<MantenedorFiltros> ConsultaMonedasFiltros()
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTAMONEDAS", ParameterDirection.Input));
                return VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasFiltrosSNCotiza, parameters, x => new MantenedorFiltros
                {
                    descripcion = x.GetString(0),
                    codigoElemento = x.GetString(1),
                    codigoPension = x.GetString(2)
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Metodo que trae la lista con los filtros para cotizar (Montos)
        /// Lizbeth Morales
        /// 29/01/2019
        /// </summary>
        /// <returns>Lista Con filtros para cotizar</returns>
        public List<MantenedorFiltros> ConsultaMontosFiltros()
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "DESDEHASTA", ParameterDirection.Input));
                return VCEDBContext<MantenedorFiltros>.CallStoreProcedure(StoredProcedures.CO_ConsultasFiltrosSNCotiza, parameters, x => new MantenedorFiltros
                {
                    mtoPriDesde = Convert.ToDouble(x.GetDecimal(0)),
                    mtoPriHasta = Convert.ToDouble(x.GetDecimal(1)),
                    numEdadDesde = x.GetInt32(2),
                    numEdadHasta = x.GetInt32(3),
                    codigoPension = x.GetString(4)
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
