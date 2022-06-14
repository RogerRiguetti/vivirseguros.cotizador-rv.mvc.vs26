using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Repository.Core.Domain;
using ExcelDataReader;
using System.IO;
using Estudio.Logic;
using log4net;
using System.Reflection;
using log4net.Config;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class ManCurvaTasasResController : Controller
    {
        static string mensaje ="";
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ManCurvaTasasResLogic _manCurvaTasasResLogic = new ManCurvaTasasResLogic();

        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            ViewBag.Mensaje = mensaje;
            mensaje = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            XmlConfigurator.Configure();
            if (file != null)
            {
                _log.Info("****************** Nueva carga de mantenedor de curvas de tasas*******************");
                if (!file.FileName.EndsWith(".xls") && !file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".XLS") && !file.FileName.EndsWith(".XLSX"))
                    return View();

                var fileName = DateTime.Now.ToString("yyyyMMddHHmm.") + file.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last();
                _log.Info("Se guardará el archivo");
                SaveFile(file, fileName);
                _log.Info("El archivo se guardo correctamente");
                UploadRecordsToDataBase(fileName);
                return RedirectToAction("Index");
            }

            // Tu podras decidir que hacer aqui
            // si el archivo es nulo
            return View();

        }

        private void SaveFile(HttpPostedFileBase file, string fileName)
        {
            var path = System.IO.Path.Combine(Server.MapPath("~/Files/"), fileName);
            var data = new byte[file.ContentLength];
            file.InputStream.Read(data, 0, file.ContentLength);

            using (var sw = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                sw.Write(data, 0, data.Length);
            }
        }

        private void UploadRecordsToDataBase(string fileName)
        {
            List<ManCurvaTasasRes> Datos = new List<ManCurvaTasasRes>();
            var records = new List<ManCurvaTasasRes>();
            DateTime Fec_IniVigTMP = new DateTime();
            string Fec_IniVig = "";
            string query = "";

            XmlConfigurator.Configure();
            _log.Info("Se pasará a guardar los datos en variables para realizar las querys");
            try
            {
                using (var stream = System.IO.File.Open(Path.Combine(Server.MapPath("~/Files/"), fileName), FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var i = 0;
                        while (reader.Read())
                        {

                            if (i == 0)
                            {
                                Datos.Add(new ManCurvaTasasRes() { Fec_IniVig = reader.GetDateTime(2) });
                                Fec_IniVigTMP = reader.GetDateTime(2);
                                Fec_IniVig = Fec_IniVigTMP.ToString("yyyyMMdd");
                                query += "UPDATE PR_TVAL_CURVA_TASAS SET ";
                                query += "FEC_TERVIG = '" + Fec_IniVigTMP.AddDays(-1).ToString("yyyyMMdd") + "' ";
                                query += "WHERE FEC_TERVIG = '99991231' ";

                            }
                            if (i >= 3)
                            {
                                query += "\nINSERT INTO PR_TVAL_CURVA_TASAS (FEC_INIVIG, FEC_TERVIG , COD_MONEDA, COD_TIPREAJUSTE, NUM_MES, MTO_VALOR)VALUES('";
                                query += Fec_IniVig + "', '99991231', 'NS', 1,";
                                query += Convert.ToInt32(reader.GetDouble(0)) + ",";
                                query += (Convert.ToDecimal(reader.GetDouble(1)) * 100) + ")";


                                query += "\nINSERT INTO PR_TVAL_CURVA_TASAS (FEC_INIVIG, FEC_TERVIG , COD_MONEDA, COD_TIPREAJUSTE, NUM_MES, MTO_VALOR)VALUES('";
                                query += Fec_IniVig + "', '99991231', 'NS', 2,";
                                query += Convert.ToInt32(reader.GetDouble(0)) + ",";
                                query += (Convert.ToDecimal(reader.GetDouble(2)) * 100) + ")";


                                query += "\nINSERT INTO PR_TVAL_CURVA_TASAS (FEC_INIVIG, FEC_TERVIG , COD_MONEDA, COD_TIPREAJUSTE, NUM_MES, MTO_VALOR)VALUES('";
                                query += Fec_IniVig + "', '99991231', 'US', 2,";
                                query += Convert.ToInt32(reader.GetDouble(0)) + ",";
                                query += (Convert.ToDecimal(reader.GetDouble(3)) * 100) + ")";
                            }
                            i++;
                        }
                        _log.Info("Los datos estan listos para pasarlos a querys");
                        var resultado = _manCurvaTasasResLogic.CargarCurvaTasasReservas(query);
                        _log.Info("se termino de realizar las querys y la ejecucion en base de datos");
                        _log.Info("el resultado fue: " + resultado.Message);
                        mensaje = resultado.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Info("Error en la lectura del archivo para Carga Masiva de Tasas de Reservas: " + ex.Message);
                mensaje = "Error en la lectura del archivo para Carga Masiva de Tasas de Reservas: " + ex.Message;
            }

            if (records.Any())
            {
                //db.Users.AddRange(records);
                //db.SaveChanges();
            }
        }

        public ActionResult CargaTabla(string fecha)
        {
            DateTime Fec = new DateTime();
            Fec = Convert.ToDateTime(fecha);
            var resultado = _manCurvaTasasResLogic.CargarTablaCurvaTasasReservas(Fec.ToString("yyyyMMdd"));
            return Json(resultado);
        }
    }
}