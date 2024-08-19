using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // Configura el contexto de licencia
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // O LicenseContext.Commercial si tienes una licencia comercial

            // Crear y configurar el archivo Excel
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Cartera Completa");

            int rowNumber = 1;
            int offset = 0;
            bool hasData;

            do
            {
                DataTable dt = GetDataStoredProcedure(offset, batchSize);
                hasData = dt.Rows.Count > 0;

                if (hasData)
                {
                    // Agregar encabezados en la primera iteración
                    if (offset == 0)
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            worksheet.Cells[rowNumber, col + 1].Value = dt.Columns[col].ColumnName;
                        }
                        rowNumber++;
                    }

                    // Agregar los datos
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            worksheet.Cells[rowNumber, col + 1].Value = row[col];
                        }
                        rowNumber++;
                    }

                    offset += batchSize;
                }

            } while (hasData);

            // Guardar el archivo Excel
            package.SaveAs(new FileInfo(filePath));
        }

        //public void ExportToExcelPagination(string filePath)
        //{
        //    // Configura el contexto de licencia
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // O LicenseContext.Commercial si tienes una licencia comercial
        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("Cartera Completa");

        //        int rowNumber = 1;
        //        int offset = 0;

        //        do
        //        {
        //            DataTable dt = GetDataStoredProcedure(offset, batchSize);

        //            if (dt.Rows.Count == 0) break;

        //            if (offset == 0) // Agregar encabezados en la primera iteración
        //            {
        //                for (int col = 0; col < dt.Columns.Count; col++)
        //                {
        //                    worksheet.Cells[rowNumber, col + 1].Value = dt.Columns[col].ColumnName;
        //                }
        //                rowNumber++;
        //            }

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                for (int col = 0; col < dt.Columns.Count; col++)
        //                {
        //                    worksheet.Cells[rowNumber, col + 1].Value = row[col];
        //                }
        //                rowNumber++;
        //            }

        //            offset += batchSize;

        //        } while (true);

        //        // Guardar el archivo Excel
        //        package.SaveAs(new FileInfo(filePath));
        //    }
        //}

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
