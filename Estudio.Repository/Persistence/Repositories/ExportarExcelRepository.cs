using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class ExportarExcelRepository
    {
        public List<List<Dictionary<string, object>>> ExportarExcelCalculadas(int numArchivo)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONEXCELCALCULADAS", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArchivo, ParameterDirection.Input));

            var list = CallStoreProcedureExport(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters);
            return list;
        }

        public List<List<Dictionary<string, object>>> ExportarExcelNoCalculadas(int numArchivo)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONEXCELNOCALCULADAS", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArchivo, ParameterDirection.Input));

            var list = CallStoreProcedureExport(StoredProcedures.CO_CatalogosCalcularAsignacionIntermediario, parameters);
            return list;
        }
        public List<List<Dictionary<string, object>>> ExportarExcelGenArch(string FecEnvio, int numArchivo)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "EXPORTAREXCELGENAR", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCalculo", SqlDbType.Int, FecEnvio, ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, numArchivo, ParameterDirection.Input));

            var list = CallStoreProcedureExport(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters);
            return list;
        }
        /// <summary>
        /// José Hernández Alvarado.
        /// 26-02-2019
        /// Método para ejecutar consulta en BD y obtener datos para generar el documento Excel del Proceso de Migración de Reservas.
        /// </summary>
        /// <returns>Lista con registros para generar Excel.</returns>
        public List<List<Dictionary<string, object>>> ExportarExcelReservas(string FecPeriodo)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "REPORTE_CSV", ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecPeriodo", SqlDbType.VarChar, FecPeriodo, ParameterDirection.Input));

            var list = CallStoreProcedureExport(StoredProcedures.CO_ConsultasReservas, parameters);
            return list;
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 19-06-2019
        /// Método para ejecutar consulta en BD y obtener datos para generar el documento Excel de Cálculo de Reservas (Flujos Nuevos).
        /// </summary>
        /// <returns>Lista con registros para generar Excel.</returns>
        public List<List<Dictionary<string, object>>> ConsultaCalculoRes()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_CALRES", ParameterDirection.Input));

            var list = CallStoreProcedureExport(StoredProcedures.CO_ConsultasReservas, parameters);
            return list;
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 21-06-2019
        /// Método para ejecutar consulta en BD y obtener datos para generar el documento Excel de Cálculo de Reservas (Flujos Antigüos).
        /// </summary>
        /// <returns>Lista con registros para generar Excel.</returns>
        public List<List<Dictionary<string, object>>> ConsultaCalculoResAnt()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_CALRES_ANT", ParameterDirection.Input));

            var list = CallStoreProcedureExport(StoredProcedures.CO_ConsultasReservas, parameters);
            return list;
        }


        public static List<List<Dictionary<string, object>>> CallStoreProcedureExport(string storedProcedure, List<SqlParameter> parameters)
        {
            using (SqlConnection Conexion = new SqlConnection(VCEConectionString.Connection()))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, Conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    parameters.ForEach(x => cmd.Parameters.Add(x));

                    SqlDataAdapter sa = new SqlDataAdapter(cmd);

                    DataSet ds = new DataSet();

                    sa.Fill(ds);

                    DataTable dt = ds.Tables[0];

                    var columnas = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();


                    var dicExp = new List<List<Dictionary<string, object>>>();

                    int i = 0;

                    foreach (DataRow rdr in dt.Rows)
                    {
                        var listaColumnas = new List<Dictionary<string, object>>();

                        foreach (string columna in columnas)
                        {
                            var col = new Dictionary<string, object>();

                            col.Add(columna, rdr[i]);
                            listaColumnas.Add(col);

                            i++;
                        }

                        dicExp.Add(listaColumnas);

                        i = 0;
                    }

                    return dicExp;
                }
            }
        }
    }
}
