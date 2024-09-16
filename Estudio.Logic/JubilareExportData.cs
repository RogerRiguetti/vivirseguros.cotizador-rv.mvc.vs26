using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Estudio.Repository;
using OfficeOpenXml;

namespace Estudio.Logic
{
    public class JubilareExportData
    {
        // Tamaño del lote para la paginación
        private const int batchSize = 10000;

        public void ExportToExcelPagination(string filePath)
        {
            // Configura el contexto de licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // O LicenseContext.Commercial si tienes una licencia comercial

            // Crear y configurar el archivo Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Cartera Completa");

                int rowNumber = 1;  // Fila donde empezar a escribir
                int offset = 0;  // Valor inicial del offset para la paginación
                bool hasData;  // Variable para verificar si hay datos en cada paginación

                do
                {
                    // Llamar al procedimiento almacenado paginado
                    DataTable dt = GetDataStoredProcedure(offset, batchSize);
                    hasData = dt.Rows.Count > 0; // Si el DataTable tiene filas, continuamos

                    if (hasData)
                    {
                        // Agregar encabezados en la primera iteración
                        if (offset == 0)
                        {
                            for (int col = 0; col < dt.Columns.Count; col++)
                            {
                                worksheet.Cells[rowNumber, col + 1].Value = dt.Columns[col].ColumnName; // Escribe los nombres de las columnas
                            }
                            rowNumber++; // Avanzamos a la siguiente fila después de los encabezados
                        }

                        // Agregar los datos de la paginación al archivo Excel
                        foreach (DataRow row in dt.Rows)
                        {
                            for (int col = 0; col < dt.Columns.Count; col++)
                            {
                                worksheet.Cells[rowNumber, col + 1].Value = row[col]; // Escribir cada celda
                            }
                            rowNumber++; // Avanzamos a la siguiente fila
                        }

                        // Aumentar el offset para la siguiente paginación
                        offset += batchSize;
                    }

                } while (hasData);  // Continuar mientras haya datos

                // Guardar el archivo Excel
                package.SaveAs(new FileInfo(filePath));
            }
        }

        // Método para obtener datos de la paginación desde el procedimiento almacenado
        private DataTable GetDataStoredProcedure(int offset, int batchSize)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Offset", offset),
                new SqlParameter("@FetchNext", batchSize)
            };

            return SRVDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.usp_Jub_Sel_CarteraCompleta_Paginado, parameters);
        }
    }
}
