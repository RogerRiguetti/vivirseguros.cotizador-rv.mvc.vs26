using Estudio.Logic;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using log4net;
using System.Reflection;

namespace Estudio.Api.Controllers
{
    [RoutePrefix("api/Jubilare")]
    public class JubilareController : ApiController
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string GetNameFile(string fileName)
        {
            //string fileName = "CarteraJubilare";
            DateTime now = DateTime.Now;

            string formattedDate = now.ToString("yyyyMMdd");
            string formattedTime = now.ToString("HHmm");
            return $"{fileName}_{formattedDate}_{formattedTime}.xlsx";
        }

        [HttpGet]
        [Route("GetCarteraCompleta")]
        public IHttpActionResult GetCarteraCompleta()
        {
            _log.Info("Inicia solicitud de GetCartera Completa");

            string folderPath = @"D:\ExportacionesJubilareCarteraCompleta";

            // Verificar si el directorio existe, si no, crearlo
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                _log.Info("Se crea el directorio ExportacionesJubilare en disco D");
            }

            // Obtener el nombre del archivo
            string fileName = GetNameFile("CarteraJubilare");
            string filePath = Path.Combine(folderPath, fileName);

            // Llamar a la función para exportar datos paginados a Excel
            JubilareExportDataPagination jubilareExportDataPagination = new JubilareExportDataPagination();
            jubilareExportDataPagination.ExportToExcelPagination(filePath);

            // Retornar el archivo como descarga
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(File.ReadAllBytes(filePath))
                {
                    Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileName
                    },
                    ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                }
                }
            });
        }


        [HttpGet]
        [Route("Planilla/{id}")]
        public IHttpActionResult Planilla(int id)
        {
            string folderPath = @"D:\ExportacionesJubilarePlanilla";

            // Verificar si el directorio existe, si no, crearlo
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                _log.Info("Se crea el directorio ExportacionesJubilarePlanilla en disco D");
            }

            // Obtener el nombre del archivo
            string fileName = GetNameFile($"Planilla{id}");
            string filePath = Path.Combine(folderPath, fileName);

            //return Ok(filePath);

            JubilareExportData jubilareExportData = new JubilareExportData();

            var aux = jubilareExportData.ExportToExcel(filePath, id); // Generacion de la planilla


            return Ok(aux);
            //return Ok(planilla);

            //return Ok($"{aux}");
        }

        [HttpGet]
        [Route("Test")]
        public IHttpActionResult Test()
        {
            return Ok("tessat");
        }
    }

}