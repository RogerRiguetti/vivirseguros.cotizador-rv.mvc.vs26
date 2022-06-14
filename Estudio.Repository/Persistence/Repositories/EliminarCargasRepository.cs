using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class EliminarCargasRepository
    {
        public Response busqueda(string caso, string numArch, string nomArch, string tipoArchivo)
        {
            Response res = new Response();
            try
            {
                EliminarCargas d = new EliminarCargas();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_" + caso.Trim(), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch == "" ? -1 : Convert.ToInt32(numArch.Trim()), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@nomArch", SqlDbType.VarChar, nomArch.Trim(), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTipArch", SqlDbType.VarChar, tipoArchivo.Trim(), ParameterDirection.Input));
                d = VCEDBContext<EliminarCargas>.CallStoreProcedure(StoredProcedures.CO_ConsultasEliminaCargas, parameters, x => new EliminarCargas
                {
                    archivo = x.GetString(0),
                    tipoArch = x.GetString(1),
                    usuario = x.GetString(2),
                    fecCrea = x.GetString(3).Substring(6, 2) + "/" + x.GetString(3).Substring(4, 2) + "/" + x.GetString(3).Substring(0, 4),
                    horCrea = x.GetString(4).Length == 5 ? "0"+x.GetString(4).Substring(0, 1) + ":" + x.GetString(4).Substring(1, 2) + ":" + x.GetString(4).Substring(3, 2) :
                    x.GetString(4).Substring(0, 2) + ":" + x.GetString(4).Substring(2, 2) + ":" + x.GetString(4).Substring(4, 2),
                    num1 = x.IsDBNull(5)? "0": x.GetString(5),
                    num2 = x.IsDBNull(6) ? "0" : x.GetString(6),
                    num3 = x.IsDBNull(7) ? "0" : x.GetString(7)
                }).FirstOrDefault();

                if (d != null)
                {
                    string[] datos = { d.archivo, d.tipoArch, d.usuario, d.fecCrea, d.horCrea, d.num1, (Convert.ToDouble(d.num2)).ToString("N2"), d.num3 };
                    res.Object = datos;
                    res.Message = "Se encontraron los datos con exito";
                    res.IsOk = true;
                }
                else
                {
                    res.Message = "El archivo ingresado NO existe";
                    res.IsOk = false;
                }
                
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.IsOk = false;
                return res;
            }
        }
        public Response eliminar(string caso, string numArch, string numArchS)
        {
            Response res = new Response();
            var parameters = new List<SqlParameter>();
            try
            {
                //Verifica si existen modalidades aceptadas
                EliminarCargas verifica = new EliminarCargas();
                if (caso != "RESULTADOS")
                {
                    parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "V_MODACEPTADAS", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, Convert.ToInt32(numArch.Trim()), ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstCot", SqlDbType.VarChar, "A", ParameterDirection.Input));
                    verifica = VCEDBContext<EliminarCargas>.CallStoreProcedure(StoredProcedures.CO_ConsultasEliminaCargas, parameters, x => new EliminarCargas
                    {
                        Num_Archivo = Convert.ToString(x.GetInt32(0))
                    }).FirstOrDefault();
                }
                else
                {
                    parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "V_MODACEPTADASR", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, Convert.ToInt32(numArch.Trim()), ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstCot", SqlDbType.VarChar, "P", ParameterDirection.Input));
                    verifica = VCEDBContext<EliminarCargas>.CallStoreProcedure(StoredProcedures.CO_ConsultasEliminaCargas, parameters, x => new EliminarCargas
                    {
                        Num_Archivo = Convert.ToString(x.GetInt32(0))
                    }).FirstOrDefault();
                }

                if (caso == "CARGA" && numArchS != "")
                {
                    res.Message = "El archivo ya fue enviado al Meler, No se puede eliminar la carga.";
                }
                else if (caso == "ENVIOMELER" && verifica != null)
                {
                    res.Message = "Existen solicitudes aceptadas para el archivo de entrada y salida ingresados, no se puede eliminar el archivo Meler."; 
                }
                else if (caso == "RESULTADOS" && verifica != null)
                {
                    res.Message = "Existen solicitudes que ya se encuentran en la etapa de producción, no se puede eliminar el archivo de resultados.";
                }
                else
                {
                    //Realiza la elimianción del archivo 
                    EliminarCargas d = new EliminarCargas();
                    parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "E_" + caso.Trim(), ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, Convert.ToInt32(numArch.Trim()), ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArchS", SqlDbType.Int, numArchS == "" ? 0 : Convert.ToInt32(numArchS.Trim()), ParameterDirection.Input));
                    VCEDBContext<EliminarCargas>.CallStoreProcedure(StoredProcedures.CO_ConsultasEliminaCargas, parameters, x => new EliminarCargas
                    {
                    }).FirstOrDefault();

                    res.IsOk = true;
                    res.Message = "El proceso de eliminación término correctamente";
                    return res;
                }
                res.IsOk = false;
                return res;
            }
            catch (Exception)
            {
                res.Message = "Error en el proceso de eliminación";
                res.IsOk = false;
                return res;
            }
        }
        public string[] busquedaDeNumsArchs(string Fec, string caso)
        {
            try
            {
                List<EliminarCargas> _EliminarCargas = new List<EliminarCargas>();
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "NUMA_"+caso.Trim(), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, Fec.Trim(), ParameterDirection.Input));
                _EliminarCargas = VCEDBContext<EliminarCargas>.CallStoreProcedure(StoredProcedures.CO_ConsultasEliminaCargas, parameters, x => new EliminarCargas
                {
                    Num_Archivo = Convert.ToString(x.GetInt32(0)),
                    Nom_Archivo = x.GetString(1)
                }).ToList();
                if (_EliminarCargas.Count != 0)
                {
                    string[] res = new string[_EliminarCargas.Count];
                    for (int i = 0; i < _EliminarCargas.Count; i++)
                    {
                        res[i] = _EliminarCargas[i].Num_Archivo + " - " + _EliminarCargas[i].Nom_Archivo;
                    }
                    return res;
                }
                else
                {
                    string[] res = new string[1];
                    res[0] = "-1";
                    return res;
                }
            }
            catch (Exception)
            {
                string[] res = new string[1];
                res[0] = "-1"; 
                return res;
            }
        }
    }
}
