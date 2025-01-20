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

namespace Estudio.Logic
{
    public class CarteraClientesMktLogic
    {

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // List<DataTable>
        // string[]
        // void 
        public void ExportCarteraClientesMkt(string filePath)
        {
            _log.Info($"Exportando CarteraClientes Mkt");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<DataTable> dataTables = GetDataStoredProcedures();
            string[] nameSheets = { "RRVV", "RRPP", "FM", "SOAT" };

            BuildingSheets(dataTables, nameSheets, filePath);
        }


        private List<DataTable> GetDataStoredProcedures()
        {
            List<SqlParameter> parameters = new List<SqlParameter> { };
            DataTable usp_Sel_RRVV_BRD = SRVDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.usp_Sel_RRVV_BRD, parameters);
            DataTable usp_Sel_RRPP_BRD = SRVDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.usp_Sel_RRPP_BRD, parameters);
            DataTable usp_Sel_FM_BRD = SRVDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.usp_Sel_FM_BRD, parameters);
            DataTable usp_Sel_SOAT_BRD = SRVDBContext<DataTable>.CallStoreProcedureDt(StoredProcedures.usp_Sel_SOAT_BRD, parameters);

            List<DataTable> dataList = new List<DataTable>();
            dataList.Add(usp_Sel_RRVV_BRD);
            dataList.Add(usp_Sel_RRPP_BRD);
            dataList.Add(usp_Sel_FM_BRD);
            dataList.Add(usp_Sel_SOAT_BRD);
            return dataList;
        }

        public void BuildingSheets(List<DataTable> dataTables, string[] nameSheets, string filePath)
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                if (dataTables.Count != nameSheets.Length)
                {
                    throw new ArgumentException("El número de DataTables debe coincidir con el número de hojas.");
                }

                for (int i = 0; i < nameSheets.Length; i++)
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add(nameSheets[i]);

                    DataTable dataTable = dataTables[i];

                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col + 1].Value = dataTable.Columns[col].ColumnName;
                    }

                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                        }
                    }
                }

                FileInfo file = new FileInfo(filePath);
                excelPackage.SaveAs(file);
            }
        }




    }
}
