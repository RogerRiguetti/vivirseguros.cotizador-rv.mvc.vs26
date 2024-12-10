using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Estudio.Repository;
using OfficeOpenXml;


namespace Estudio.Logic
{
    public class VivirPlusClientesExport
    {
        // Mapeo de fecha
        private static DateTime MapFecha(IDataRecord record)
        {
            return record.GetDateTime(record.GetOrdinal("fecha"));
        }

        private string fechaAnteriorUltimaHistorica(string formato)
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
            if (fecha == default)
            {
                return "No hay fecha anterior";
            }

            // Convertir la fecha a string en el formato deseado
            return fecha.ToString(formato);  // Aquí puedes cambiar el formato si lo prefieres
        }

        public string extractData()
        {
            // Primero insertamos la fecha de consulta actual
            InsertFechaActual(); 

            // Procesamos el formato de fechas, dentro de estas funciones ya se extrae la ultima fecha anterior a la fecha de consulta
            string formato1 = "MM/dd/yyyy";
            string formato2 = "yyyyMMdd";
        
            var MM_dd_yyyy = fechaAnteriorUltimaHistorica(formato1);
            var yyyyMMdd = fechaAnteriorUltimaHistorica(formato2);

            var usp_Sel_CarteraClientesVS = "";
            var usp_Sel_SOAT = ""; 
            var usp_Sel_VIVEMAX = ""; // weetheen yyyy_mm_dd (yyyy+1_mm_dd) => dentro del procedimiento
            var usp_Sel_RENTASVITALICIAS = ""; // yyyyMMdd
            var usp_Sel_RENTAPRIVADA = "";

            return MM_dd_yyyy;            
        }

        private void InsertFechaActual()
        {
            //List<SqlParameter> parameters = new List<SqlParameter> { };

            //DataTable result = SRVDBContext<object>.CallStoreProcedureDt(StoredProcedures.insertFechaActual, parameters);
        }

    }
}
