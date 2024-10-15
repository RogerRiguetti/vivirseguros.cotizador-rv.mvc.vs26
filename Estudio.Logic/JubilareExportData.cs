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

namespace Estudio.Logic
{
    public class JubilareExportData
    {

        //List<DataTable>
        public List<DataTable> ExportToExcel(string filePath, int idPlanilla)
        {
            //return idPlanilla;
            // Configura el contexto de licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // O LicenseContext.Commercial si tienes una licencia comercial

            // Llamar al procedimiento almacenado paginado
            List<DataTable> dataTables = GetDataStoredProcedure(idPlanilla);
            string[] nameSheets = { "Comisiones", "Premios" };

            BuildingSheets(dataTables, nameSheets, filePath); // ! Esta funcion es la encargada de la generacion del excel con hojas adicionales

            return dataTables;
        }

        public void BuildingSheets(List<DataTable> dataTables, string[] nameSheets, string filePath)
        {
            // Configura el contexto de licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Crear un nuevo archivo Excel
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Asegurarse de que la cantidad de hojas y DataTables coincidan
                if (dataTables.Count != nameSheets.Length)
                {
                    throw new ArgumentException("El número de DataTables debe coincidir con el número de hojas.");
                }

                // Agregar las hojas y escribir los datos de cada DataTable
                for (int i = 0; i < nameSheets.Length; i++)
                {
                    // Agregar la hoja
                    var worksheet = excelPackage.Workbook.Worksheets.Add(nameSheets[i]);

                    // Obtener el DataTable correspondiente
                    DataTable dataTable = dataTables[i];

                    // Imprimir los nombres de las columnas
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col + 1].Value = dataTable.Columns[col].ColumnName; // Las columnas empiezan en 1
                    }

                    // Imprimir los datos de cada fila
                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col]; // Las filas empiezan en 2
                        }
                    }
                }

                // Guardar el archivo en la ruta especificada
                FileInfo file = new FileInfo(filePath);
                excelPackage.SaveAs(file);
            }
        }

        // Ver la forma de programarlo de forma dinamica
        private List<DataTable> GetDataStoredProcedure(int idPlanilla)
        {
            var usp_Sel_JubilarePlanillaComision = SRVDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.usp_Sel_JubilarePlanillaComision, SqlParameters(idPlanilla));
            var usp_Sel_JubilarePlanillaPremios = SRVDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.usp_Sel_JubilarePlanillaPremios, SqlParameters(idPlanilla));

            List<DataTable> planilla = new List<DataTable>();

            planilla.Add(usp_Sel_JubilarePlanillaComision);
            planilla.Add(usp_Sel_JubilarePlanillaPremios);
            return planilla;
        }

        private List<SqlParameter> SqlParameters(int idPlanilla)
        {
            return new List<SqlParameter> { new SqlParameter("@ID_PLANILLA", idPlanilla) };
        }


    }

}
