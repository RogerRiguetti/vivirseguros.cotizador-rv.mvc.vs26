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
    public class ManCurvaTasasController : Controller
    {
        static string mensaje;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ManCurvaTasasLogic _DatosExcel = new ManCurvaTasasLogic();
        public ActionResult Index()
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.usuario = getUsuario();
            return View();
        }
        public string getUsuario()
        {
            try
            {
                string usuario = "";
                if (Convert.ToString(this.Session["Account"]).Length < 10)
                {
                    usuario = Convert.ToString(this.Session["Account"]);
                }
                else
                {
                    usuario = Convert.ToString(this.Session["Account"]).Substring(0, 10);
                }
                return usuario;
            }
            catch (Exception) { return null; }
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
           string user = getUsuario();
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            string hora = DateTime.Now.ToString("hhmmss");
            List<ManCurvaTasas> Datos = new List<ManCurvaTasas>();
            var records = new List<ManCurvaTasas>();
            DateTime Fec_IniVigTMP = new DateTime();
            string Fec_IniVig = "";
            string query = "";
            _log.Info("Se pasará a guardar los datos en variables para realizar las querys");
            using (var stream = System.IO.File.Open(Path.Combine(Server.MapPath("~/Files/"), fileName), FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var i = 0;
                    while (reader.Read())
                    {

                        if (i == 0)
                        {
                            //Fec_IniVig = reader.GetString(2);//.ToString();

                            Datos.Add(new ManCurvaTasas() { Fec_IniVig = reader.GetDateTime(2) });
                            Fec_IniVigTMP = reader.GetDateTime(2);
                            Fec_IniVig = Fec_IniVigTMP.ToString("yyyyMMdd");
                            query += "UPDATE pt_tval_curva_tasas SET ";
                            query += "FEC_TERVIG = '" + Fec_IniVigTMP.AddDays(-1).ToString("yyyyMMdd") + "',  ";
                            query += "COD_USUARIOMODI = '" + user + "',  ";
                            query += "FEC_MODI = '" + fecha + "',  ";
                            query += "HOR_MODI = '" + hora + "' ";
                            query += "WHERE FEC_TERVIG = '99991231' ";

                        }
                        if (i >= 3)
                        {
                            /*Datos.Add(new ManCurvaTasas()
                            {
                                Mes = Convert.ToInt32(reader.GetDouble(0)),
                                TasaAnualSolesIndex = Convert.ToDecimal(reader.GetDouble(1)),
                                TasaAnualSolesAj = Convert.ToDecimal(reader.GetDouble(2)),
                                TasaAnualDolaresAj = Convert.ToDecimal(reader.GetDouble(3))

                            });*/
                            query += "\nINSERT INTO pt_tval_curva_tasas (FEC_INIVIG, FEC_TERVIG , COD_MONEDA, COD_TIPREAJUSTE, NUM_MES, MTO_VALOR, COD_USUARIOCREA, FEC_CREA, HOR_CREA )VALUES('";
                            query += Fec_IniVig + "', '99991231', 'NS', 1,";
                            query += Convert.ToInt32(reader.GetDouble(0)) + ",";
                            query += (Convert.ToDecimal(reader.GetDouble(1)) * 100) + ",";
                            query += "'" + user + "', ";
                            query += "'" + fecha + "', ";
                            query += "'" + hora + "' )";


                            query += "\nINSERT INTO pt_tval_curva_tasas (FEC_INIVIG, FEC_TERVIG , COD_MONEDA, COD_TIPREAJUSTE, NUM_MES, MTO_VALOR, COD_USUARIOCREA, FEC_CREA, HOR_CREA)VALUES('";
                            query += Fec_IniVig + "', '99991231', 'NS', 2,";
                            query += Convert.ToInt32(reader.GetDouble(0)) + ",";
                            query += (Convert.ToDecimal(reader.GetDouble(2)) * 100) + ",";
                            query += "'" + user + "', ";
                            query += "'" + fecha + "', ";
                            query += "'" + hora + "' )";


                            query += "\nINSERT INTO pt_tval_curva_tasas (FEC_INIVIG, FEC_TERVIG , COD_MONEDA, COD_TIPREAJUSTE, NUM_MES, MTO_VALOR,  COD_USUARIOCREA, FEC_CREA, HOR_CREA)VALUES('";
                            query += Fec_IniVig + "', '99991231', 'US', 2,";
                            query += Convert.ToInt32(reader.GetDouble(0)) + ",";
                            query += (Convert.ToDecimal(reader.GetDouble(3)) * 100) + ",";
                            query += "'" + user + "', ";
                            query += "'" + fecha + "', ";
                            query += "'" + hora + "' )";
                        }
                        i++;
                    }
                    _log.Info("Los datos estan listos para pasarlos a querys");
                    var resultado = _DatosExcel.DatosQ(query);
                    _log.Info("se termino de realizar las querys y la ejecucion en base de datos");
                    _log.Info("el resultado fue: " + resultado.Message);
                    mensaje = resultado.Message;
                }
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
            var resultado = _DatosExcel.CargaTablaLogic(Fec.ToString("yyyyMMdd"));
            return Json(resultado);
        }
    }
}