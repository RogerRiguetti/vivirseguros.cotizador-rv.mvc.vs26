using Estudio.Repository;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Logic
{
    public class JubilareExportData
    {

        public List<DataTable> ExportToExcel(string filePath, int idPlanilla)
        {
            // Configura el contexto de licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // O LicenseContext.Commercial si tienes una licencia comercial

            // Llamar al procedimiento almacenado paginado
            List<DataTable> dt = GetDataStoredProcedure(idPlanilla);

            return dt;

        }

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
