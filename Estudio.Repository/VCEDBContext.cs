using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository
{
    public class VCEDBContext<T>
    {
        /// <summary>
        /// Antonio Quezada
        /// 2018-02-13
        /// Ejecuta un procedimiento almacenado que necesita parámetros
        /// </summary>
        /// <param name="storedProcedure"> Nombre del procedimiento almacenado </param>
        /// <param name="parameters"> Parámetros requeridos por el procedimiento </param>
        /// <param name="copyRow"> Registros que regresa el procedimiento </param>
        /// <returns> Regresa los registros de base de datos </returns>
        
        public static IEnumerable<T> CallStoreProcedure(string storedProcedure, List<SqlParameter> parameters, Func<IDataRecord, T> copyRow)
        {
            using (SqlConnection Conexion = new SqlConnection(VCEConectionString.Connection()))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, Conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    parameters.ForEach(x => cmd.Parameters.Add(x));

                    Conexion.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            yield return copyRow(rdr);
                        }
                        rdr.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-02-13
        /// Ejecuta un procedimiento almacenado que no necesita parámetros
        /// </summary>
        /// <param name="storedProcedure"> Nombre del procedimiento almacenado </param>
        /// <param name="copyRow"> Registros que regresa el procedimiento </param>
        /// <returns> Regresa los registros de base de datos </returns>

        public static IEnumerable<T> CallStoreProcedure(string storedProcedure, Func<IDataRecord, T> copyRow)
        {
            using (SqlConnection Conexion = new SqlConnection(VCEConectionString.Connection()))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, Conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    Conexion.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            yield return copyRow(rdr);
                        }
                        rdr.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-02-13
        /// Ejecuta un procedimiento almacenado que no necesita parámetros
        /// </summary>
        /// <param name="storedProcedure"> Nombre del procedimiento almacenado </param>
        /// <param name="parameters"> Parámetros requeridos por el procedimiento </param>
        /// <returns> Regresa un DataTable con los registros de base de datos </returns>

        public static DataTable CallStoreProcedureDt(string storedProcedure, List<SqlParameter> parameters)
        {
            var dataTable = new DataTable();

            using (SqlConnection Conexion = new SqlConnection(VCEConectionString.Connection()))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, Conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    parameters.ForEach(x => cmd.Parameters.Add(x));

                    Conexion.Open();

                    var dataReader = cmd.ExecuteReader();
                    dataTable.Load(dataReader);
                }
            }

            return dataTable;
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 24-09-2018
        /// Ejecuta una consulta en base de datos.
        /// </summary>
        /// <param name="query">Consulta que se va a ejecutar en la base de datos.</param>
        /// <param name="copyRow">Registros que regresa el procedimiento.</param>
        /// <returns></returns>
        public static IEnumerable<T> CallSelectStatement(string query, Func<IDataRecord, T> copyRow)
        {
            using (SqlConnection Conexion = new SqlConnection(VCEConectionString.Connection()))
            {
                using (SqlCommand cmd = new SqlCommand(query, Conexion))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;

                    Conexion.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            yield return copyRow(rdr);
                        }
                        rdr.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-02-13
        /// </summary>
        /// <param name="parameterName"> Nombre del parámetro recibido en el procedimiento almacenado </param>
        /// <param name="parameterType"> Tipo de parámetro requerido por el procedimiento almacenado </param>
        /// <param name="parameterValue"> Valor que tendrá el parámetro </param>
        /// <param name="parameterDirection"> Direcciòn del parámetro </param>
        /// <returns> Regresa el parámetro con sus respectivas propiedades </returns>

        public static SqlParameter AddParams(string parameterName, SqlDbType parameterType, object parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter parameters = new SqlParameter();
            parameters.ParameterName = parameterName;
            parameters.SqlDbType = parameterType;
            parameters.Value = parameterValue;
            parameters.Direction = parameterDirection;

            return parameters;
        }

        /// <summary>
        /// Antonio Quezada
        /// 2019-01-03
        /// Ejecuta un query que regresa un DataTable
        /// </summary>
        /// <param name="query"> Query a ejecutar en Base de Datos </param>
        /// <param name="copyRow"></param>
        /// <returns> Regresa la consulta del query ejecutado en un DataTable </returns>

        public static DataTable CallSelectStatementDt(string query, Func<IDataRecord, T> copyRow)
        {
            var dataTable = new DataTable();

            using (SqlConnection Conexion = new SqlConnection(VCEConectionString.Connection()))
            {
                using (SqlCommand cmd = new SqlCommand(query, Conexion))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;

                    Conexion.Open();

                    var dataReader = cmd.ExecuteReader();
                    dataTable.Load(dataReader);
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Omar Figueroa Flores
        /// 14-03-2019
        /// Ejecuta una consulta en base de datos.
        /// </summary>
        /// <param name="query">Consulta que se va a ejecutar en la base de datos.</param>
        /// <param name="copyRow">Registros que regresa el procedimiento.</param>
        /// <returns></returns>
        public static IEnumerable<T> CallSelectStatementConection(string conectionString, string query, Func<IDataRecord, T> copyRow)
        {
            using (SqlConnection Conexion = new SqlConnection(conectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, Conexion))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;

                    Conexion.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            yield return copyRow(rdr);
                        }
                        rdr.Close();
                    }
                }
            }
        }
    }
}
