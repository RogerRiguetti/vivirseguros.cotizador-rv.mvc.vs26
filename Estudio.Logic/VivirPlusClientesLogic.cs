using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using Estudio.Repository;
using System.Data.SqlClient;
using OfficeOpenXml;
using log4net;
using System.Reflection;

namespace Estudio.Logic
{
    public class VivirPlusClientesLogic
    {

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Mapeo de fecha
        private static DateTime MapFecha(IDataRecord record)
        {
            return record.GetDateTime(record.GetOrdinal("fecha"));
        }

        public string FechaAnteriorUltimaHistorica(string formato)
        {
            string query = @"
            SELECT TOP 1 fecha
            FROM MKT_HISTORICO_FECHAS
            WHERE Fecha_id < (
                SELECT TOP 1 Fecha_id
                FROM MKT_HISTORICO_FECHAS
                ORDER BY Fecha_id DESC
            )  
            ORDER BY Fecha_id DESC";
            // Llamada a la base de datos para obtener las fechas
            IEnumerable<DateTime> fechaAnterior = SRVDBContext<DateTime>.CallSelectStatement(query, MapFecha);
            // Verificar si se obtuvo alguna fecha
            DateTime fecha = fechaAnterior.FirstOrDefault();
            // Si no hay fechas anteriores, retorna un valor por defecto o maneja el caso
            if (fecha == default) return "No hay fecha anterior";
            // Convertir la fecha a string en el formato deseado
            return fecha.ToString(formato);  // Aquí puedes cambiar el formato si lo prefieres
        }

        //DataTable
        private DataTable ExtractData()
        {
            // Procesamos el formato de fechas, dentro de estas funciones ya se extrae la ultima fecha anterior a la fecha de consulta
            string formato1 = "MM/dd/yyyy";
            string formato2 = "yyyyMMdd";

            string MM_dd_yyyy = FechaAnteriorUltimaHistorica(formato1);
            string yyyyMMdd = FechaAnteriorUltimaHistorica(formato2);
            bool _false = false;

            DataTable usp_Sel_CarteraClientesVS = CallStoredProcedure(StoredProcedures.usp_Sel_CarteraClientesVS, MM_dd_yyyy, _false);
            DataTable usp_Sel_SOAT = CallStoredProcedure(StoredProcedures.usp_Sel_SOAT, MM_dd_yyyy, _false);
            DataTable usp_Sel_VIVEMAX = CallStoredProcedure(StoredProcedures.usp_Sel_VIVEMAX, MM_dd_yyyy, true); // Aumenta en un año
            DataTable usp_Sel_RENTASVITALICIAS = CallStoredProcedure(StoredProcedures.usp_Sel_RENTASVITALICIAS, yyyyMMdd, _false); // yyyyMMdd
            DataTable usp_Sel_RENTAPRIVADA = CallStoredProcedure(StoredProcedures.usp_Sel_RENTAPRIVADA, MM_dd_yyyy, _false);

            // Crear un DataTable final con la misma estructura que las tablas de origen
            DataTable finalTable = usp_Sel_CarteraClientesVS.Clone(); // Clona la estructura de columnas

            if (usp_Sel_SOAT.Columns.Contains("PLACA")) usp_Sel_SOAT.Columns["PLACA"].MaxLength = int.MaxValue;

            if (finalTable.Columns.Contains("PLACA")) finalTable.Columns["PLACA"].MaxLength = int.MaxValue;

            // Ajustar MaxLength de la columna "PRODUCTO" para evitar el error
            if (finalTable.Columns.Contains("PRODUCTO")) finalTable.Columns["PRODUCTO"].MaxLength = int.MaxValue; // O un valor más grande, como 1000
                 
            // Concatenar todas las tablas
            try
            {
                _log.Info("EMPIEZA EL ADDTABLE");

                _log.Info($"REGISTROS DE RENTAS VITALICIAS {usp_Sel_RENTASVITALICIAS} | Registros: {usp_Sel_RENTASVITALICIAS.Rows.Count}");

                AddTableRows(finalTable, usp_Sel_CarteraClientesVS);
                AddTableRows(finalTable, usp_Sel_SOAT);
                AddTableRows(finalTable, usp_Sel_VIVEMAX);
                AddTableRows(finalTable, usp_Sel_RENTASVITALICIAS);
                AddTableRows(finalTable, usp_Sel_RENTAPRIVADA);
                _log.Info("FIN DE ADDTABLE");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                _log.Info($"ERROR: {ex.Message}");
            }

            
            return finalTable;
        }

        // Método auxiliar para agregar filas de una tabla origen a una tabla destino
        private void AddTableRows(DataTable destination, DataTable source)
        {
            if (source != null && source.Rows.Count > 0)
            {
                foreach (DataRow row in source.Rows) destination.ImportRow(row); // Importa las filas conservando el esquema
            }
        }

        // Funcion para insertar la fecha actual a la cual se hace consulta
        private void InsertFechaActual()
        {
            List<SqlParameter> parameters = new List<SqlParameter> { };

            DataTable result = SRVDBContext<object>.CallStoreProcedureDt(StoredProcedures.insertFechaActual, parameters);
        }

        // Funcion auxiliar para llamado de procedimientos almacenados
        private DataTable CallStoredProcedure(string storedProcedure, string fecha, bool op)
        {
            List<SqlParameter> sqlParameter = SqlParameters(fecha, op);
            return SRVDBContext<DataTable>.CallStoreProcedureDt(storedProcedure, sqlParameter);
        }

        // Funcion auxiliar para envio de parametros, ya que en un formato se envia mas un año
        private List<SqlParameter> SqlParameters(string fecha, bool op)
        {
            // fecha = "20241202"; // ! Fecha de prueba 11/05 - tener cuidado con el formato de rentasvitalicias
            string fechaAnioMas = fecha.Substring(0, fecha.Length - 4) + (int.Parse(fecha.Substring(fecha.Length - 4)) + 1);
            return (op) ?
                new List<SqlParameter> {
                    new SqlParameter("@fechaInicial", fecha),
                    new SqlParameter("@fechaAnioMas", fechaAnioMas)
                } :
                new List<SqlParameter> {
                    new SqlParameter("@fecha", fecha)
                };
        }

        private void BuildingSheet(DataTable dataTable, string filePath)
        {
            // Configura el contexto de licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("VivirPlusClientes");

                // Escribir los encabezados
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dataTable.Columns[col].ColumnName;
                }

                // Escribir filas
                for (int rowIdx = 0; rowIdx < dataTable.Rows.Count; rowIdx++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[rowIdx + 2, col + 1].Value = dataTable.Rows[rowIdx][col];
                    }
                }

                // Guardar el archivo
                package.SaveAs(new FileInfo(filePath));
            }

        }

        public void ExportExcel(string filePath)
        {
            // Primero insertamos la fecha de consulta actual
            InsertFechaActual();
            DataTable dataCateraClientes = ExtractData();
            BuildingSheet(dataCateraClientes, filePath);
        }

    }
}
