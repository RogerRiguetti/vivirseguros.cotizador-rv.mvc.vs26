using Estudio.Repository;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using Estudio.Helpers;

namespace Estudio.Logic
{
    public class CuadroComisiones
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public void ExportCuadroComisiones(string filePath)
        {
            _log.Info($"Exportando CarteraClientes Mkt");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            DataTable dataTable = SRVDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.usp_Sel_Cuadro_Comisiones, new List<SqlParameter> { });

            BuildingSheet(dataTable, filePath);
        }



        private void BuildingSheet(DataTable dataTable, string filePath)
        {
            // Configura el contexto de licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("CuadroComisiones");

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

    }
}
