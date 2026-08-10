using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Logic
{
    public class ExportarExcelLogic
    {
        // creamos el documento excel
        NPOI.HSSF.UserModel.HSSFWorkbook documentoExcel = new NPOI.HSSF.UserModel.HSSFWorkbook();

        public void ExportExcel(string path, List<List<List<Dictionary<string, object>>>> dataQuery, List<string> nombresHojas)
        {
            System.IO.FileStream streamDocumentoExcel = null;

            try
            {
                // creamos el documento excel en el sistema de archivos
                streamDocumentoExcel = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate);

                // establecemos los parámetros básicos del documento excel
                NPOI.HPSF.DocumentSummaryInformation dsi = NPOI.HPSF.PropertySetFactory.CreateDocumentSummaryInformation();
                documentoExcel.DocumentSummaryInformation = dsi;
                NPOI.HPSF.SummaryInformation si = NPOI.HPSF.PropertySetFactory.CreateSummaryInformation();
                si.Author = "VidaCamara";
                si.CreateDateTime = DateTime.Now;
                si.Title = "Reporte";
                documentoExcel.SummaryInformation = si;

                int numeroHoja = 0;

                foreach (var item in dataQuery)
                {
                    AgregarHoja(item, nombresHojas[numeroHoja]);
                    numeroHoja++;
                }
                // se escriben los datos en el documento
                documentoExcel.Write(streamDocumentoExcel);
                streamDocumentoExcel.Close();
            }
            catch (Exception ex)
            {
                if (streamDocumentoExcel != null) { streamDocumentoExcel.Close(); }
                Console.WriteLine(ex.Message);
                //throw ex;
            }
        }

        public void AgregarHoja( List<List<Dictionary<string, object>>> dataQuery, string Sistema)
        {
            // obtenemos la hoja
            NPOI.HSSF.UserModel.HSSFSheet hoja = (NPOI.HSSF.UserModel.HSSFSheet)documentoExcel.CreateSheet(Sistema);

            // estilos de los textos
            // CreateFont devuelve una implementación concreta (HSSFFont) que expone Boldweight.
            NPOI.HSSF.UserModel.HSSFFont letraNegritaBlanco = (NPOI.HSSF.UserModel.HSSFFont)documentoExcel.CreateFont();
            letraNegritaBlanco.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            // En versiones recientes de NPOI se usa la propiedad IsBold
            letraNegritaBlanco.IsBold = true;

            // estilos de las celdas
            //NPOI.HSSF.UserModel.HSSFCellStyle celdaTituloAzul = (NPOI.HSSF.UserModel.HSSFCellStyle)documentoExcel.CreateCellStyle();
            //celdaTituloAzul.SetFont(letraNegritaBlanco);
            //celdaTituloAzul.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            //celdaTituloAzul.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
            //celdaTituloAzul.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

            NPOI.HSSF.UserModel.HSSFCellStyle celdaTextoNormal = (NPOI.HSSF.UserModel.HSSFCellStyle)documentoExcel.CreateCellStyle();
            celdaTextoNormal.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            // se establecen la longitud de las columnas  
            if (dataQuery.Count != 0)
            {
                for (int i = 0; i <= dataQuery[0].Count; i++)
                {
                    hoja.SetColumnWidth(i, 15 * 290);
                }


                // creamos la fila para los titulos
                hoja.CreateRow(0);

                // se aplica el estilo a las celdas de titulo
                //for (int i = 0; i <= dataQuery[0].Count; i++) { hoja.GetRow(0).CreateCell(i).CellStyle = celdaTituloAzul; }

                for (int i = 0; i <= dataQuery[0].Count - 1; i++)
                {
                    for (int j = 0; j <= dataQuery[0][i].Count; j++)
                    {
                        hoja.GetRow(0).CreateCell(i).SetCellValue(dataQuery[0][i].First().Key);
                    }
                }
            }

            // creamos los titulos de las columnas para la primera hoja 
            int inc = 1;
            int inc2 = 0;
            foreach (var dato in dataQuery)
            {
                hoja.CreateRow(inc);

                foreach (var col in dato)
                {
                    var o = col.First().Value.ToString();
                    hoja.GetRow(inc).CreateCell(inc2).SetCellValue(o);
                    inc2++;
                }

                inc++;
                inc2 = 0;
            }
        }
    }
}
