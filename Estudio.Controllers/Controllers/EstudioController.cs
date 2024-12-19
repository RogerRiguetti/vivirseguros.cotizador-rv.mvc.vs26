using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Controllers.Models;
using Estudio.Repository.Helpers;
using Estudio.Repository.Core.Domain.Views;
using System.Web.Security;
using Estudio.Logic;
using System.DirectoryServices;
using System.Collections;
using System.Text;
using Estudio.Repository.Core.Domain;
using Estudio.Repository;
using System.Data;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;

namespace Estudio.Controllers.Controllers
{
    public class EstudioController : Controller
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string ValidateSession()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
            {
                this.Session["UserId"] = null;
                this.Session["UserIdEncrypted"] = null;
                this.Session["Account"] = null;
                this.Session["Name"] = null;

                this.Session["IdTipoDocumento"] = null;
                this.Session["Documento"] = null;
                this.Session["Nombres"] = null;
                this.Session["Apellidos"] = null;
                this.Session["Asesor"] = null;
                this.Session["CUSPP"] = null;
                RedirectToAction("Login", "Estudio");
            }
            else
                res = Convert.ToString(this.Session["Account"]);
            return res;
      
        }


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.UserLogin = ValidateSession();

            return View();
        }

        [HttpGet]
        public ActionResult Menu()
        {
            return PartialView();
        }

        public ActionResult Login()
        {
            this.Session["UserId"] = null;
            this.Session["UserIdEncrypted"] = null;
            this.Session["Account"] = null;

            return View();
        }

        [HttpPost]
        public JsonResult ValidateLogin(string usuario, string password)
        {
            XmlConfigurator.Configure();
            _log.Info("VALIDACIÓN LOGIN");
            var res1 = new EstudioLogic().ValidateLogin(usuario, password);
            if (res1.IsOk || (res1.Message == "El password es incorrecto."))
            {
                _log.Info("Usuario encontrado en BD VCEstudioOficiales");
                // Creamos un objeto DirectoryEntry para conectarnos al directorio activo
                string path = @"LDAP://vcamara-pe.loc";
                string dominio = "vcamara-pe";
                string dominioUsuario = dominio + @"\" + usuario;


                bool valida = AutenticarUsAd(path, dominioUsuario, password, usuario);

                _log.Info("Usuario encontrado en AD");
                if (valida == true)
                {
                    bool status = true;
                    var res3 = new EstudioLogic().ValidateLoginAD(usuario, password);
                    if (res3.IsOk == true)
                    {
                        UserView obj2 = (UserView)res3.Object;
                        string passwordDecryp = VCEConectionString.Decrypt(obj2.Pass, out status);
                        if (passwordDecryp != password)
                        {
                            _log.Info("Usuario pasara a ser modificado AD");
                            string passwordEncryp2 = VCEConectionString.Encrypt(password, out status);
                            string script = "";
                            script += "UPDATE gzUsers SET Password = '" + passwordEncryp2 + "' Where Account = '" + usuario + "';";

                            VCEDBContext<DataTable>.CallSelectStatementDt(script, x => new DataTable());
                            _log.Info("Usuario se modifico correctamente en BD VCEstudioOficiales");
                        }
                    }
                }
                var res = new EstudioLogic().ValidateLogin(usuario, password);

                UserView obj = (UserView)res.Object;

                if (res.IsOk == true)
                {
                    // Creamos el ticket
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, obj.Account, DateTime.Now,
                    DateTime.Now.AddMinutes(20), true, "");

                    //Encriptamos el ticket
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    this.Session["encryptedTicket"] = encryptedTicket;
                    // Creamos una cookie para posteriormente agregar al ticket
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        Expires = authTicket.Expiration
                    };

                    // Tiempo para expirar Cookie  

                    // Redireccionamos a la página que lo invocó
                    Response.Cookies.Add(authCookie);

                    ////Agregamos en variable session nombre del usuario firmado
                    bool status;

                    var userId = Encrypt(obj.Id.ToString(), out status);

                    this.Session["UserId"] = obj.Id;
                    this.Session["UserIdEncrypted"] = userId;
                    this.Session["Account"] = obj.Account;
                    this.Session["Name"] = obj.Name;
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _log.Info("Usuario NO encontrado en BD pasa a AD");
                // Creamos un objeto DirectoryEntry para conectarnos al directorio activo
                string path = @"LDAP://vcamara-pe.loc";
                string dominio = "vcamara-pe";
                string dominioUsuario = dominio + @"\" + usuario;
                string rol = "";
                int UserId = -1;
                try
                {
                    DirectoryEntry de = new DirectoryEntry(path, dominioUsuario, password, AuthenticationTypes.Secure);
                    // Creamos un objeto DirectorySearcher para hacer una búsqueda en el directorio activo
                    DirectorySearcher adsSearch = new DirectorySearcher(de);

                    // Ponemos como filtro que busque el usuario actual
                    adsSearch.Filter = "samAccountName=" + usuario;

                    // Extraemos la primera coincidencia
                    SearchResult oResult;
                    oResult = adsSearch.FindOne();

                    _log.Info("Usuario encontrado en AD");

                    // Obtenemos el objeto de ese usuario
                    DirectoryEntry us = oResult.GetDirectoryEntry();

                    // Obtenemos la lista de SID de los grupos a los que pertenece
                    us.RefreshCache(new string[] { "tokenGroups" });

                    // Creamos una variable StringBuilder donde ir añadiendo los SID para crear un filtro de búsqueda
                    StringBuilder sids = new StringBuilder();
                    sids.Append("(|");
                    foreach (byte[] sid in us.Properties["tokenGroups"])
                    {
                        sids.Append("(objectSid=");
                        for (int indice = 0; indice < sid.Length; indice++)
                        {
                            sids.AppendFormat("\\{0}", sid[indice].ToString("X2"));
                        }
                        sids.AppendFormat(")");
                    }
                    sids.Append(")");

                    // Creamos un objeto DirectorySearcher con el filtro antes generado y buscamos todas la coincidencias
                    DirectorySearcher ds = new DirectorySearcher(de, sids.ToString());
                    SearchResultCollection src = ds.FindAll();

                    // Recorremos toda la lista de grupos devueltos
                    foreach (SearchResult sr in src)
                    {
                        _log.Info("El usuario AD pasara a ser validado");
                        _log.Info("Consultara ID del usuario ingresado por AD");
                        gzUser idUs = new gzUser();
                        string queryId = "SELECT Id FROM gzUsers WHERE Account ='" + ((string)sr.Properties["samAccountName"][0] + "'");
                        _log.Info("Usuario a consultar: " + ((string)sr.Properties["samAccountName"][0]));
                        idUs = VCEDBContext<gzUser>.CallSelectStatement(queryId, x => new gzUser
                        {
                            Id = x.GetInt32(0)
                        }).FirstOrDefault();
                        _log.Info("Se hizo la consulta correctamente el ID es " + idUs);
                        switch ((string)sr.Properties["samAccountName"][0])
                        {


                            //////////////////Producción///////////////
                            //case "AdminCotizador": UserId = 3079; rol = "AdminCotizador"; break;
                            //case "Supervisor": UserId = 3080; rol = "Supervisor"; break;
                            //case "Asesor": UserId = 3081; rol = "Asesor"; break;
                            //case "Gerente": UserId = 3082; rol = "Gerente"; break;
                            //case "Técnico": UserId = 3083; rol = "Técnico"; break;
                            //case "Operaciones": UserId = 3084; rol = "Operaciones"; break;
                            //case "Analista Comercial": UserId = 3085; rol = "Analista Comercial"; break;


                            case "AdminCotizador": UserId = 3079; rol = "AdminCotizador"; break;
                            case "Supervisor":
                                _log.Info("Usuario Supervisor");
                                _log.Info("Usuario pasara a ser buscado a BD JUBILARE");
                                _log.Info("Usuario" + usuario);
                                UserId = new EstudioLogic().getUserIdAsesor(usuario);
                                _log.Info("ID Usuario encontrado " + UserId);
                                if (UserId != 0 || UserId != -1)
                                {
                                    gzUser datosUs = new gzUser();
                                    string query = "SELECT num_agente, ape_paterno, ape_materno, nom_persona, glscorta" +
                                     " FROM JUBILARE.dbo.agente" +
                                     " WHERE cod_contrato = 'SUPERVIS' AND cod_vigencia_agente = 'S' and num_agente =" + UserId;

                                    datosUs = VCEDBContext<gzUser>.CallSelectStatement(query, x => new gzUser
                                    {
                                        NumeroAgente = x.GetInt32(0),
                                        LastNames = x.GetString(1) + " " + x.GetString(2),
                                        Names = x.GetString(3),
                                        Account = x.GetString(4)
                                    }).FirstOrDefault();

                                    bool status = true;

                                    string passwordEncryp = VCEConectionString.Encrypt(password, out status);

                                    if (datosUs != null)
                                    {
                                        string query2 = "INSERT INTO gzUsers (NumeroAgente, LastNames, Names, Account, Password, Active, UserProfile_UserId)" +
                                            "VALUES (" + datosUs.NumeroAgente + ", '" + datosUs.LastNames + "', '" + datosUs.Names + "', '" + datosUs.Account + "', '" + passwordEncryp + "' , 1 , 2)" +
                                            "SELECT Id from gzUsers where NumeroAgente = " + datosUs.NumeroAgente;

                                        int idusernew = VCEDBContext<gzUser>.CallSelectStatement(query2, x => new gzUser
                                        {
                                            Id = x.GetInt32(0)
                                        }).FirstOrDefault().Id;

                                        List<gzUser> datosUs2 = new List<gzUser>();
                                        string query3 = "SELECT num_agente, rut_jefe_directo" +
                                         " FROM JUBILARE.dbo.agente" +
                                         " WHERE cod_contrato = 'ASESOR' AND cod_vigencia_agente = 'S' and rut_jefe_directo =" + datosUs.NumeroAgente;

                                        datosUs2 = VCEDBContext<gzUser>.CallSelectStatement(query3, x => new gzUser
                                        {
                                            NumeroAgente = x.GetInt32(0)
                                        }).ToList();

                                        List<gzUser> datosUs4 = new List<gzUser>();


                                        string script = "";
                                        string query4 = "";
                                        for (int i = 0; i < datosUs2.Count; i++)
                                        {
                                            query4 = "";
                                            query4 = "SELECT Id" +
                                             " FROM gzUsers" +
                                             " WHERE NumeroAgente=" + datosUs2[i].NumeroAgente;

                                            datosUs4 = VCEDBContext<gzUser>.CallSelectStatement(query4, x => new gzUser
                                            {
                                                Id = x.GetInt32(0)
                                            }).ToList();

                                            if (datosUs4.Count != 0)
                                            {
                                                for (int l = 0; l < datosUs4.Count; l++)
                                                {
                                                    script = "";
                                                    script += "INSERT INTO SupervisoresAsesores " +
                                                          "(Supervisor, Asesor) " +
                                                          "VALUES (" + idusernew + ",'" + datosUs4[i].Id + "')" +
                                                          "\n";
                                                    VCEDBContext<DataTable>.CallSelectStatementDt(script, x => new DataTable());
                                                }
                                            }
                                        }
                                    }
                                }
                                var res3 = new EstudioLogic().ValidateLogin(usuario, password);
                                UserView obj1 = (UserView)res3.Object;
                                UserId = Convert.ToInt32(obj1.Id);
                                rol = "Supervisor";

                                break;
                            case "Asesor":
                                _log.Info("Usuario Asesor");
                                _log.Info("Usuario pasara a ser buscado a BD JUBILARE");
                                _log.Info("Usuario" + usuario);
                                UserId = new EstudioLogic().getUserIdAsesor(usuario);
                                _log.Info("ID Usuario encontrado " + UserId);

                                if (UserId != 0 || UserId != -1)
                                {
                                    gzUser datosUs = new gzUser();
                                    string query = "SELECT num_agente, ape_paterno, ape_materno, nom_persona, glscorta" +
                                     " FROM JUBILARE.dbo.agente" +
                                     " WHERE cod_contrato = 'ASESOR' AND cod_vigencia_agente = 'S' and num_agente =" + UserId;

                                    datosUs = VCEDBContext<gzUser>.CallSelectStatement(query, x => new gzUser
                                    {
                                        NumeroAgente = x.GetInt32(0),
                                        LastNames = x.GetString(1) + " " + x.GetString(2),
                                        Names = x.GetString(3),
                                        Account = x.GetString(4)
                                    }).FirstOrDefault();

                                    bool status = true;

                                    string passwordEncryp = VCEConectionString.Encrypt(password, out status);

                                    if (datosUs != null)
                                    {
                                        string query2 = "INSERT INTO gzUsers (NumeroAgente, LastNames, Names, Account, Password, Active, UserProfile_UserId)" +
                                            "VALUES (" + datosUs.NumeroAgente + ", '" + datosUs.LastNames + "', '" + datosUs.Names + "', '" + datosUs.Account + "', '" + passwordEncryp + "' , 1 , 3)" +
                                            "SELECT Id from gzUsers where NumeroAgente = " + datosUs.NumeroAgente;

                                        int idusernew = VCEDBContext<gzUser>.CallSelectStatement(query2, x => new gzUser
                                        {
                                            Id = x.GetInt32(0)
                                        }).FirstOrDefault().Id;


                                        string datosUs2 = "";
                                        string query3 = "SELECT num_agente, rut_jefe_directo" +
                                         " FROM JUBILARE.dbo.agente" +
                                         " WHERE cod_contrato = 'ASESOR' AND cod_vigencia_agente = 'S' and NUM_AGENTE =" + datosUs.NumeroAgente;

                                        datosUs2 = VCEDBContext<gzUser>.CallSelectStatement(query3, x => new gzUser
                                        {
                                            Supervisor = x.GetString(1)
                                        }).FirstOrDefault().Supervisor;


                                        int datosUs3 = 0;
                                        string query4 = "SELECT Id" +
                                         " FROM gzUsers" +
                                         " WHERE NumeroAgente =" + Convert.ToInt32(datosUs2);

                                        datosUs3 = VCEDBContext<gzUser>.CallSelectStatement(query4, x => new gzUser
                                        {
                                            Id = x.GetInt32(0)
                                        }).FirstOrDefault().Id;


                                        string script = "";
                                        script += "INSERT INTO SupervisoresAsesores " +
                                              "(Supervisor, Asesor) " +
                                              "VALUES (" + datosUs3 + ",'" + idusernew + "')" +
                                              "\n";
                                        VCEDBContext<DataTable>.CallSelectStatementDt(script, x => new DataTable());


                                    }
                                }
                                var res2 = new EstudioLogic().ValidateLogin(usuario, password);
                                UserView obj = (UserView)res2.Object;
                                UserId = Convert.ToInt32(obj.Id);
                                rol = "Asesor";
                                break;
                            case "Gerente": _log.Info("Usuario Gerente"); UserId = 3082; rol = "Gerente"; break;
                            case "Técnico": _log.Info("Usuario Técnico"); UserId = 3083; rol = "Técnico"; break;
                            case "Operaciones": _log.Info("Usuario Operaciones"); UserId = 3084; rol = "Operaciones"; break;
                            case "Analista Comercial": _log.Info("Usuario Analista Comercial"); UserId = 3085; rol = "Analista Comercial"; break;

                            /////////////////QA////////////////////
                            //case "AdminCotizador": UserId = 2065; rol = "AdminCotizador"; break;
                            //case "Supervisor": UserId = 2066; rol = "Supervisor"; break;
                            //case "Asesor": UserId = 2067; rol = "Asesor"; break;
                            //case "Gerente": UserId = 2068; rol = "Gerente"; break;
                            //case "Técnico": UserId = 2069; rol = "Técnico"; break;
                            //case "Operaciones": UserId = 2070; rol = "Operaciones"; break;
                            //case "Analista Comercial": UserId = 2071; rol = "Analista Comercial"; break;
                            //case "AdminCotizador": _log.Info("Usuario Administrador"); UserId = 2065; rol = "AdminCotizador"; break;
                            //case "Supervisor":
                            //    _log.Info("Usuario Supervisor");
                            //    _log.Info("Usuario pasara a ser buscado a BD JUBILARE");
                            //    _log.Info("Usuario" + usuario);
                            //    UserId = new EstudioLogic().getUserIdAsesor(usuario);
                            //    _log.Info("ID Usuario encontrado " + UserId);

                            //    if (UserId != 0 || UserId != -1)
                            //    {
                            //        gzUser datosUs = new gzUser();
                            //        string query = "SELECT num_agente, ape_paterno, ape_materno, nom_persona, glscorta" +
                            //         " FROM JUBILARE.dbo.agente" +
                            //         " WHERE cod_contrato = 'SUPERVIS' AND cod_vigencia_agente = 'S' and num_agente =" + UserId;

                            //        datosUs = VCEDBContext<gzUser>.CallSelectStatement(query, x => new gzUser
                            //        {
                            //            NumeroAgente = x.GetInt32(0),
                            //            LastNames = x.GetString(1) + " " + x.GetString(2),
                            //            Names = x.GetString(3),
                            //            Account = x.GetString(4)
                            //        }).FirstOrDefault();

                            //        bool status = true;

                            //        string passwordEncryp = VCEConectionString.Encrypt(password, out status);

                            //        if (datosUs != null)
                            //        {
                            //            string query2 = "INSERT INTO gzUsers (NumeroAgente, LastNames, Names, Account, Password, Active, UserProfile_UserId)" +
                            //                "VALUES (" + datosUs.NumeroAgente + ", '" + datosUs.LastNames + "', '" + datosUs.Names + "', '" + datosUs.Account + "', '" + passwordEncryp + "' , 1 , 2)" +
                            //                "SELECT Id from gzUsers where NumeroAgente = " + datosUs.NumeroAgente;

                            //            int idusernew = VCEDBContext<gzUser>.CallSelectStatement(query2, x => new gzUser
                            //            {
                            //                Id = x.GetInt32(0)
                            //            }).FirstOrDefault().Id;

                            //            List<gzUser> datosUs2 = new List<gzUser>();
                            //            string query3 = "SELECT num_agente, rut_jefe_directo" +
                            //             " FROM JUBILARE.dbo.agente" +
                            //             " WHERE cod_contrato = 'ASESOR' AND cod_vigencia_agente = 'S' and rut_jefe_directo =" + datosUs.NumeroAgente;

                            //            datosUs2 = VCEDBContext<gzUser>.CallSelectStatement(query3, x => new gzUser
                            //            {
                            //                NumeroAgente = x.GetInt32(0)
                            //            }).ToList();

                            //            List<gzUser> datosUs4 = new List<gzUser>();


                            //            string script = "";
                            //            string query4 = "";
                            //            for (int i = 0; i < datosUs2.Count; i++)
                            //            {
                            //                query4 = "";
                            //                query4 = "SELECT Id" +
                            //                 " FROM gzUsers" +
                            //                 " WHERE NumeroAgente=" + datosUs2[i].NumeroAgente;

                            //                datosUs4 = VCEDBContext<gzUser>.CallSelectStatement(query4, x => new gzUser
                            //                {
                            //                    Id = x.GetInt32(0)
                            //                }).ToList();

                            //                if (datosUs4.Count != 0)
                            //                {
                            //                    for (int l = 0; l < datosUs4.Count; l++)
                            //                    {
                            //                        script = "";
                            //                        script += "INSERT INTO SupervisoresAsesores " +
                            //                              "(Supervisor, Asesor) " +
                            //                              "VALUES (" + idusernew + ",'" + datosUs4[i].Id + "')" +
                            //                              "\n";
                            //                        VCEDBContext<DataTable>.CallSelectStatementDt(script, x => new DataTable());
                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //    var res3 = new EstudioLogic().ValidateLogin(usuario, password);
                            //    UserView obj1 = (UserView)res3.Object;
                            //    UserId = Convert.ToInt32(obj1.Id);
                            //    rol = "Supervisor";

                            //    break;
                            //case "Asesor":
                            //    _log.Info("Usuario Asesor");
                            //    _log.Info("Usuario pasara a ser buscado a BD JUBILARE");
                            //    _log.Info("Usuario" + usuario);
                            //    UserId = new EstudioLogic().getUserIdAsesor(usuario);
                            //    _log.Info("ID Usuario encontrado " + UserId);
                            //    if (UserId != 0 || UserId != -1)
                            //    {
                            //        gzUser datosUs = new gzUser();
                            //        string query = "SELECT num_agente, ape_paterno, ape_materno, nom_persona, glscorta" +
                            //         " FROM JUBILARE.dbo.agente" +
                            //         " WHERE cod_contrato = 'ASESOR' AND cod_vigencia_agente = 'S' and num_agente =" + UserId;

                            //        datosUs = VCEDBContext<gzUser>.CallSelectStatement(query, x => new gzUser
                            //        {
                            //            NumeroAgente = x.GetInt32(0),
                            //            LastNames = x.GetString(1) + " " + x.GetString(2),
                            //            Names = x.GetString(3),
                            //            Account = x.GetString(4)
                            //        }).FirstOrDefault();

                            //        bool status = true;

                            //        string passwordEncryp = VCEConectionString.Encrypt(password, out status);

                            //        if (datosUs != null)
                            //        {
                            //            string query2 = "INSERT INTO gzUsers (NumeroAgente, LastNames, Names, Account, Password, Active, UserProfile_UserId)" +
                            //                "VALUES (" + datosUs.NumeroAgente + ", '" + datosUs.LastNames + "', '" + datosUs.Names + "', '" + datosUs.Account + "', '" + passwordEncryp + "' , 1 , 3)" +
                            //                "SELECT Id from gzUsers where NumeroAgente = " + datosUs.NumeroAgente;

                            //            int idusernew = VCEDBContext<gzUser>.CallSelectStatement(query2, x => new gzUser
                            //            {
                            //                Id = x.GetInt32(0)
                            //            }).FirstOrDefault().Id;


                            //            string datosUs2 = "";
                            //            string query3 = "SELECT num_agente, rut_jefe_directo" +
                            //             " FROM JUBILARE.dbo.agente" +
                            //             " WHERE cod_contrato = 'ASESOR' AND cod_vigencia_agente = 'S' and NUM_AGENTE =" + datosUs.NumeroAgente;

                            //            datosUs2 = VCEDBContext<gzUser>.CallSelectStatement(query3, x => new gzUser
                            //            {
                            //                Supervisor = x.GetString(1)
                            //            }).FirstOrDefault().Supervisor;


                            //            int datosUs3 = 0;
                            //            string query4 = "SELECT Id" +
                            //             " FROM gzUsers" +
                            //             " WHERE NumeroAgente =" + Convert.ToInt32(datosUs2);

                            //            datosUs3 = VCEDBContext<gzUser>.CallSelectStatement(query4, x => new gzUser
                            //            {
                            //                Id = x.GetInt32(0)
                            //            }).FirstOrDefault().Id;


                            //            string script = "";
                            //            script += "INSERT INTO SupervisoresAsesores " +
                            //                  "(Supervisor, Asesor) " +
                            //                  "VALUES (" + datosUs3 + ",'" + idusernew + "')" +
                            //                  "\n";
                            //            VCEDBContext<DataTable>.CallSelectStatementDt(script, x => new DataTable());


                            //        }
                            //    }
                            //    var res2 = new EstudioLogic().ValidateLogin(usuario, password);
                            //    UserView obj = (UserView)res2.Object;
                            //    UserId = Convert.ToInt32(obj.Id);
                            //    rol = "Asesor";
                            //    break;
                            //case "Gerente": _log.Info("Usuario Gerente"); UserId = 2068; rol = "Gerente"; break;
                            //case "Técnico": _log.Info("Usuario Técnico"); UserId = 2069; rol = "Técnico"; break;
                            //case "Operaciones": _log.Info("Usuario Operaciones"); UserId = 2070; rol = "Operaciones"; break;
                            //case "Analista Comercial": _log.Info("Usuario Analista Comercial"); UserId = 2071; rol = "Analista Comercial"; break;


                        }
                    }
                    //Creamos el ticket
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, usuario, DateTime.Now,
                    DateTime.Now.AddMinutes(20), true, "");

                    //Encriptamos el ticket
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    this.Session["encryptedTicket"] = encryptedTicket;
                    // Creamos una cookie para posteriormente agregar al ticket
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        Expires = authTicket.Expiration
                    };

                    // Tiempo para expirar Cookie  

                    // Redireccionamos a la página que lo invocó
                    Response.Cookies.Add(authCookie);

                    //Llena las variables de sesion
                    this.Session["UserId"] = UserId;
                    this.Session["Account"] = usuario;
                    this.Session["Name"] = rol;

                    //Se crea el objeto que se va a retornar
                    object objRes = new
                    {
                        IsOk = true,
                        Object = new
                        {
                            Id = UserId,
                            Account = usuario,
                            Name = rol,
                            Active = "1"
                        }
                    };
                    return Json(objRes, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(res1, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult LogOut(string id)
        {
            this.Session["UserId"] = null;
            this.Session["Account"] = null;
            this.Session["Name"] = null;

            this.Session["IdTipoDocumento"] = null;
            this.Session["Documento"] = null;
            this.Session["Nombres"] = null;
            this.Session["Apellidos"] = null;
            this.Session["Asesor"] = null;
            this.Session["CUSPP"] = null;

            Response res = new Response();
            //es el nombre del usuario con id 10 reemplazar por otro que este consistente el nombre con su id
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RenewSession()
        {
            this.Session["UserId"] = this.Session["UserId"];
            this.Session["Account"] = this.Session["Account"];
            this.Session["Name"] = this.Session["Name"];
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();

            string urlOficialesLogin = System.Configuration.ConfigurationManager.AppSettings["UrlOficialesLogin"];

            return Redirect(urlOficialesLogin);
        }

        private bool AutenticarUsAd(string path, string dominioUsuario, string password, string usuario)
        {

            try
            {
                DirectoryEntry de = new DirectoryEntry(path, dominioUsuario, password, AuthenticationTypes.Secure);
                // Creamos un objeto DirectorySearcher para hacer una búsqueda en el directorio activo
                DirectorySearcher adsSearch = new DirectorySearcher(de);

                // Ponemos como filtro que busque el usuario actual
                adsSearch.Filter = "samAccountName=" + usuario;

                // Extraemos la primera coincidencia
                SearchResult oResult;
                oResult = adsSearch.FindOne();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public ActionResult Oficiales(String idUsuario)
        {
            EstudioLogic _estudioLogic = new EstudioLogic();

            bool status;
            String UsuarioDecrypted = Decrypt(idUsuario, out status);
            int idUsuarioDecrypted = Convert.ToInt32(UsuarioDecrypted);

            gzUser usuario = _estudioLogic.ConsultarUsuario(idUsuarioDecrypted);
            //Creamos el ticket
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, usuario.Account, DateTime.Now,
            DateTime.Now.AddMinutes(20), true, "");

            //Encriptamos el ticket
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            this.Session["encryptedTicket"] = encryptedTicket;
            // Creamos una cookie para posteriormente agregar al ticket
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                Expires = authTicket.Expiration
            };

            // Tiempo para expirar Cookie  

            // Redireccionamos a la página que lo invocó
            Response.Cookies.Add(authCookie);

            //Llena las variables de sesion
            this.Session["UserId"] = idUsuarioDecrypted;
            this.Session["UserIdEncrypted"] = idUsuario;
            this.Session["Account"] = usuario.Account;
            this.Session["Name"] = usuario.RolStr;

            return RedirectToAction("Index", "Estudio");
        }
        public static string Encrypt(String toEncryt, out bool status)
        {
            var result = string.Empty;
            try
            {
                var encrypted = System.Text.Encoding.UTF8.GetBytes(toEncryt);
                result = System.Convert.ToBase64String(encrypted);

                status = true;
            }
            catch (Exception)
            {
                status = false;
            }

            return result;
        }
        public static string Decrypt(string toDecryt, out bool status)
        {
            var result = string.Empty;
            try
            {
                var decryted = Convert.FromBase64String(toDecryt);
                result = System.Text.Encoding.UTF8.GetString(decryted);
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }

            return result;
        }

        #region "Prueba Jubilare"
        private string GetNameFile()
        {
            string fileName = "CarteraJubilare";
            DateTime now = DateTime.Now;

            string formattedDate = now.ToString("yyyyMMdd");
            string formattedTime = now.ToString("HHmm");
            return $"{fileName}_{formattedDate}_{formattedTime}.xlsx";
        }

        [HttpPost]
        public ActionResult GetCarteraCompleta()
        {
            _log.Info("Inicia solicitud de GetCartera Completa");
            string folderPath = @"D:\ExportacionesJubilare";

            _log.Info("Verificando la existencia del directorio ExportacionesJubilare en disco D");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                _log.Info("Se crea el directorio ExportacionesJubilare en disco D");
            }

            string fileName = GetNameFile();
            string filePath = Path.Combine(folderPath, fileName);
            _log.Info("Se genera el filePath");
            JubilareExportDataPaginationLogic jubilareExportData = new JubilareExportDataPaginationLogic();
            _log.Info("Se hace llamado del service de jubilareExportData");
            jubilareExportData.ExportToExcelPagination(filePath);
            _log.Info("Fin de Solicitud de GetCartera Completa");
            return File(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        #endregion

    }
}