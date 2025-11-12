using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Helpers;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Estudio.Repository.Persistence.Repositories
{

    public class SolicitudCotizacionesRepository
    {
        private static string vgCodTabla_TipPen = "TP";
        private static string vgCodTabla_TipVejez = "TV";
        private static string vgCodTabla_AFP = "AF";  //administradora de fondos de pensiones
        private static string vgCodTabla_Moneda = "TM";   //tipo moneda
        private static string vgCodTabla_TipRen = "TR";   //tipo de rentabilidad

        MantenedorFiltrosRepository _mantenedorFiltros = new MantenedorFiltrosRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public int NumEntrada(string archivo, string nombre, string us, string tipo, string fecha, string hora, int numArchent)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            int numArch = 1;
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "NUMENTRADA", ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    numArchivo = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    numArch = _SolicitudCotizacion.numArchivo + 1;

                }

                CargaThis_entrada(numArch, archivo, nombre, us, tipo, fecha, hora, numArchent);
                return numArch;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public bool ExisteNumOpe(int intNumOpe)
        {
            bool valida = false;
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "EXISTENUMOPE", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOpe", SqlDbType.Int, intNumOpe, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    existeNumOpe = Convert.ToInt32(x.GetDecimal(0))
                }).FirstOrDefault();
                if (_SolicitudCotizacion != null)
                {
                    valida = true;
                }

                return valida;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public bool ExisteNumArch(int intNumOpe, int numArch)
        {
            bool valida = false;
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "EXISTENUMARCH", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOpe", SqlDbType.Int, intNumOpe, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    existeNumOpe = Convert.ToInt32(x.GetDecimal(0))
                }).FirstOrDefault();
                if (_SolicitudCotizacion != null)
                {
                    valida = true;
                }
                return valida;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public string BuscaCodDepto(string dpto)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCACODDEPTO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@region", SqlDbType.VarChar, dpto, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    strDepto = x.GetString(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion == null)
                {
                    return "0";

                }
                else
                {
                    return _SolicitudCotizacion.strDepto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void transfError(int numArch)
        {
            List<SolicitudesCotizacion> _SolicitudCotizacion = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TRANSFERROR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    numArchivo = x.GetInt32(0),
                    strError = x.GetString(2)
                }).ToList();

                if (_SolicitudCotizacion != null)
                {
                    for (int i = 0; i < _SolicitudCotizacion.Count; i++)
                    {
                        ActErrAfi(_SolicitudCotizacion[i].intNumOpe, _SolicitudCotizacion[i].numArchivo, _SolicitudCotizacion[i].strError);
                        ActErrFon(_SolicitudCotizacion[i].intNumOpe, _SolicitudCotizacion[i].numArchivo, _SolicitudCotizacion[i].strError);
                        ActErrBen(_SolicitudCotizacion[i].intNumOpe, _SolicitudCotizacion[i].numArchivo, _SolicitudCotizacion[i].strError);
                        ActErrPro(_SolicitudCotizacion[i].intNumOpe, _SolicitudCotizacion[i].numArchivo, _SolicitudCotizacion[i].strError);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void ActErrAfi(int intNumOpe, int numArch, string strError)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ACTERRAFI", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strError", SqlDbType.VarChar, strError, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, intNumOpe, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                var succes = _SolicitudCotizacion.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al grabar error en tabla ttmp_afiliado");
                throw;
            }
        }
        public void ActErrFon(int intNumOpe, int numArch, string strError)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ACTERRAFON", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strError", SqlDbType.VarChar, strError, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, intNumOpe, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                var succes = _SolicitudCotizacion.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al grabar error en tabla ttmp_fondosol");
                throw;
            }
        }
        public void ActErrBen(int intNumOpe, int numArch, string strError)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ACTERRABEN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strError", SqlDbType.VarChar, strError, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, intNumOpe, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                var succes = _SolicitudCotizacion.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al grabar error en tabla ttmp_beneficiario");
                throw;
            }
        }
        public void ActErrPro(int intNumOpe, int numArch, string strError)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ACTERRAPRO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strError", SqlDbType.VarChar, strError, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, intNumOpe, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                var succes = _SolicitudCotizacion.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al grabar error en tabla ttmp_prodsol");
                throw;
            }
        }
        public void insertAfiliado(int numArch, SolicitudesCotizacion datosSC)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_AFILIADO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, datosSC.intNumOpe, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipoDoc", SqlDbType.VarChar, datosSC.strTipoDoc, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNumDoc", SqlDbType.VarChar, datosSC.strNumDoc, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strApPat", SqlDbType.VarChar, (datosSC.strApPat != "" || datosSC.strApPat != null) ? datosSC.strApPat : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strApMat", SqlDbType.VarChar, (datosSC.strApMat != "" || datosSC.strApMat != null) ? datosSC.strApMat : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNom", SqlDbType.VarChar, (datosSC.strNom != "" || datosSC.strNom != null) ? datosSC.strNom : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNomSec", SqlDbType.VarChar, (datosSC.strNomSec != "" || datosSC.strNomSec != null) ? datosSC.strNomSec : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strSexo", SqlDbType.VarChar, datosSC.strSexo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecNac", SqlDbType.VarChar, datosSC.strFecNac, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strGraInv", SqlDbType.VarChar, datosSC.strGraInv, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strSitInv", SqlDbType.VarChar, datosSC.strSitInv, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strEstSob", SqlDbType.VarChar, datosSC.strEstSob, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strError", SqlDbType.VarChar, (datosSC.strError != null) ? datosSC.strError : "0", ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void insertFondo(int numArch, SolicitudesCotizacion datosSC)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();

                if (datosSC.strBonAct == null && datosSC.strTipCamAA == null && datosSC.strApoAdi == null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_FONDO1", ParameterDirection.Input));
                }
                else if (datosSC.strBonAct != null && datosSC.strTipCamAA == null && datosSC.strApoAdi == null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_FONDO2", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strBonAct", SqlDbType.Decimal, datosSC.strBonAct, ParameterDirection.Input));
                }
                else if (datosSC.strBonAct == null && datosSC.strTipCamAA != null && datosSC.strApoAdi == null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_FONDO3", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipCamAA", SqlDbType.Decimal, datosSC.strTipCamAA, ParameterDirection.Input));
                }
                else if (datosSC.strBonAct == null && datosSC.strTipCamAA == null && datosSC.strApoAdi != null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_FONDO4", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strApoAdi", SqlDbType.Decimal, datosSC.strApoAdi, ParameterDirection.Input));
                }
                else if (datosSC.strBonAct == null && datosSC.strTipCamAA != null && datosSC.strApoAdi != null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_FONDO5", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipCamAA", SqlDbType.Decimal, datosSC.strTipCamAA, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strApoAdi", SqlDbType.Decimal, datosSC.strApoAdi, ParameterDirection.Input));
                }
                else if (datosSC.strBonAct != null && datosSC.strTipCamAA == null && datosSC.strApoAdi != null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_FONDO6", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strBonAct", SqlDbType.Decimal, datosSC.strBonAct, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strApoAdi", SqlDbType.Decimal, datosSC.strApoAdi, ParameterDirection.Input));
                }
                else if (datosSC.strBonAct != null && datosSC.strTipCamAA != null && datosSC.strApoAdi == null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_FONDO7", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strBonAct", SqlDbType.Decimal, datosSC.strBonAct, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipCamAA", SqlDbType.Decimal, datosSC.strTipCamAA, ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_FONDO8", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strBonAct", SqlDbType.Decimal, datosSC.strBonAct, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipCamAA", SqlDbType.Decimal, datosSC.strTipCamAA, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strApoAdi", SqlDbType.Decimal, datosSC.strApoAdi, ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, datosSC.intNumOpe, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCodMon", SqlDbType.VarChar, datosSC.strCodMon, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCapPen", SqlDbType.Decimal, datosSC.strCapPen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMtoCIC", SqlDbType.Decimal, datosSC.strMtoCIC, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMtoCuo", SqlDbType.Decimal, datosSC.strMtoCuo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMtoSal", SqlDbType.Decimal, datosSC.strMtoSal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCodCob", SqlDbType.VarChar, datosSC.strCodCob, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCodCiaCob", SqlDbType.VarChar, (datosSC.strCodCiaCob != "" || datosSC.strCodCiaCob != null) ? datosSC.strCodCiaCob : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strError", SqlDbType.VarChar, (datosSC.strError != null) ? datosSC.strError : "0", ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void insertBeneficiario(int numArch, SolicitudesCotizacion datosSC)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_BENEFICIARIO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, datosSC.intNumOpe, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPatBen", SqlDbType.VarChar, datosSC.strPatBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMatBen", SqlDbType.VarChar, (datosSC.strMatBen != "" || datosSC.strMatBen != null) ? datosSC.strMatBen : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNomBen", SqlDbType.VarChar, datosSC.strNomBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNomSecBen", SqlDbType.VarChar, (datosSC.strNomSecBen != "" || datosSC.strNomSecBen != null) ? datosSC.strNomSecBen : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strParBen", SqlDbType.VarChar, datosSC.strParBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strSitInvBen", SqlDbType.VarChar, datosSC.strSitInvBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecNacBen", SqlDbType.VarChar, DateTime.Parse(datosSC.strFecNacBen).ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strSexoBen", SqlDbType.VarChar, datosSC.strSexoBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strError", SqlDbType.VarChar, (datosSC.strError != null) ? datosSC.strError : "0", ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void insertProducto(int numArch, SolicitudesCotizacion datosSC)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                if (datosSC.strannosRT == null && datosSC.strPrcRVD == null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_PRODUCTO1", ParameterDirection.Input));
                }
                else if (datosSC.strannosRT != null && datosSC.strPrcRVD == null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_PRODUCTO2", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strannosRT", SqlDbType.Int, datosSC.strannosRT, ParameterDirection.Input));
                }
                else if (datosSC.strannosRT == null && datosSC.strPrcRVD != null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_PRODUCTO3", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPrcRVD", SqlDbType.Decimal, datosSC.strPrcRVD, ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_PRODUCTO4", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strannosRT", SqlDbType.Int, datosSC.strannosRT, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPrcRVD", SqlDbType.Decimal, datosSC.strPrcRVD, ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, datosSC.intNumOpe, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCodMod", SqlDbType.VarChar, datosSC.strCodMod, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMonPro", SqlDbType.VarChar, (datosSC.strMonPro != "" || datosSC.strMonPro != null) ? datosSC.strMonPro : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPerGar", SqlDbType.VarChar, (datosSC.strPerGar != "" || datosSC.strPerGar != null) ? datosSC.strPerGar : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCobCon", SqlDbType.VarChar, (datosSC.strCobCon != "" || datosSC.strCobCon != null) ? datosSC.strCobCon : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strDerCre", SqlDbType.VarChar, (datosSC.strDerCre != "" || datosSC.strDerCre != null) ? datosSC.strDerCre : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strGratif", SqlDbType.VarChar, (datosSC.strGratif != "" || datosSC.strGratif != null) ? datosSC.strGratif : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPartCapital", SqlDbType.VarChar, (datosSC.strPartCapital != "" || datosSC.strPartCapital != null) ? datosSC.strPartCapital : "NULL", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strError", SqlDbType.VarChar, (datosSC.strError != null) ? datosSC.strError : "0", ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void cargaSol(int numArch, SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGASOL", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, Convert.ToDecimal(datosSC.intNumOpe), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strAfp", SqlDbType.VarChar, datosSC.strAfp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCussp", SqlDbType.VarChar, datosSC.strCussp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipBen", SqlDbType.VarChar, datosSC.strTipBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCamMod", SqlDbType.VarChar, datosSC.strCamMod, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPenPre", SqlDbType.VarChar, datosSC.strPenPre, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTasaRPRT", SqlDbType.Decimal, Convert.ToDecimal(datosSC.strTasaRPRT), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecDev", SqlDbType.VarChar, datosSC.strFecDev, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecSus", SqlDbType.VarChar, datosSC.strFecSus, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecEnv", SqlDbType.VarChar, datosSC.strFecEnv, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecCie", SqlDbType.VarChar, datosSC.strFecCie, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipCam", SqlDbType.Decimal, Convert.ToDecimal(datosSC.strTipCam), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vgUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strError", SqlDbType.VarChar, (datosSC.strError != null) ? datosSC.strError : "0", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecCita", SqlDbType.VarChar, datosSC.strFecCita, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strHorCita", SqlDbType.VarChar, datosSC.strHorCita, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strLugCita", SqlDbType.VarChar, datosSC.strLugCita, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strDepto", SqlDbType.VarChar, (datosSC.strDepto != "" || datosSC.strDepto != null) ? datosSC.strDepto : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNumMensualidad", SqlDbType.VarChar, (datosSC.strNumMensualidad != "" || datosSC.strNumMensualidad != null) ? datosSC.strNumMensualidad : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipoFondo", SqlDbType.VarChar, (datosSC.strTipoFondo != "" || datosSC.strTipoFondo != null) ? datosSC.strTipoFondo : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecDevSol", SqlDbType.VarChar, datosSC.strFecDevSol, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void cargaTHIS(int numArch, string archivo, string nombre, string us, string tipo, string fecha, string hora, int numArchent)
        {
            //CargaThis_entrada(numArch, archivo, nombre, us, tipo, fecha, hora, numArchent);
            CargaThis_CargaSol(numArch);
            CargaThis_FondoSol(numArch);
            CargaThis_Afiliado(numArch);
            CargaThis_Beneficiario(numArch);
            CargaThis_prodsol(numArch);
        }
        public void CargaThis_entrada(int numArch, string archivo, string nombre, string us, string tipo, string fecha, string hora, int numArchent)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            int anio = DateTime.Today.Year;
            var mes = DateTime.Today.Month;
            var dia = DateTime.Today.Day;
            string d = "";
            string m = "";
            if (dia <= 9) { d = "0" + dia; } else { d = dia + ""; }
            if (mes <= 9) { m = "0" + mes; } else { m = mes + ""; }

            string fechaActual = "" + anio + "" + m + "" + d + "";
            string horaActual = hora.ToString();
            try
            {
                var parameters = new List<SqlParameter>();
                if (numArchent != 0)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "THISENTRADA1", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArchent", SqlDbType.Int, numArchent, ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "THISENTRADA2", ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@nombre", SqlDbType.VarChar, nombre, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@us", SqlDbType.VarChar, us, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@tipo", SqlDbType.VarChar, tipo.Substring(0, 3), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecha", SqlDbType.VarChar, DateTime.Parse(fecha).ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@hora", SqlDbType.VarChar, DateTime.Parse(hora).ToString("hhmmss"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fechaActual", SqlDbType.VarChar, fechaActual, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horaActual", SqlDbType.VarChar, DateTime.Parse(horaActual).ToString("hhmmss"), ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        private void CargaThis_CargaSol(int numArch)
        {
            List<SolicitudesCotizacion> _SolicitudCotizacion = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "THISCARGASOL", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    numArchivo = x.GetInt32(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strAfp = x.GetString(2),
                    strCussp = x.GetString(3),
                    strTipBen = x.GetString(4),
                    strCamMod = x.GetString(5),
                    strPenPre = x.GetString(6),
                    strTasaRPRT = x.GetDecimal(7).ToString(),
                    strFecDev = x.GetString(8),
                    strFecSus = x.GetString(9),
                    strFecEnv = x.GetString(10),
                    strFecCie = x.GetString(11),
                    strTipCam = x.GetDecimal(12).ToString(),
                    usuario = x.GetString(13),
                    strFecCita = x.GetString(14),
                    strHorCita = x.GetString(15),
                    strLugCita = x.GetString(16),
                    strDepto = x.GetString(17),
                    strNumMensualidad = x.GetString(18),
                    strTipoFondo = x.GetString(19),
                    strFecDevSol = x.GetString(20)
                }).ToList();

                if (_SolicitudCotizacion != null)
                {
                    for (int i = 0; i < _SolicitudCotizacion.Count; i++)
                    {
                        cargaSolTTP(_SolicitudCotizacion[i]);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void cargaSolTTP(SolicitudesCotizacion datosSC)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGASOLTTP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, datosSC.numArchivo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, datosSC.intNumOpe, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strAfp", SqlDbType.VarChar, datosSC.strAfp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCussp", SqlDbType.VarChar, datosSC.strCussp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipBen", SqlDbType.VarChar, datosSC.strTipBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCamMod", SqlDbType.VarChar, datosSC.strCamMod, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPenPre", SqlDbType.VarChar, datosSC.strPenPre, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTasaRPRT", SqlDbType.Decimal, datosSC.strTasaRPRT, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecDev", SqlDbType.VarChar, datosSC.strFecDev, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecSus", SqlDbType.VarChar, datosSC.strFecSus, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecEnv", SqlDbType.VarChar, datosSC.strFecEnv, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecCie", SqlDbType.VarChar, datosSC.strFecCie, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipCam", SqlDbType.Decimal, datosSC.strTipCam, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vgUsuario", SqlDbType.VarChar, datosSC.usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strError", SqlDbType.VarChar, (datosSC.strError != "") ? datosSC.strError : "0", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecCita", SqlDbType.VarChar, datosSC.strFecCita, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strHorCita", SqlDbType.VarChar, datosSC.strHorCita, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strLugCita", SqlDbType.VarChar, datosSC.strLugCita, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strDepto", SqlDbType.VarChar, (datosSC.strDepto != "") ? datosSC.strDepto : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNumMensualidad", SqlDbType.VarChar, (datosSC.strNumMensualidad != "") ? datosSC.strNumMensualidad : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipoFondo", SqlDbType.VarChar, (datosSC.strTipoFondo != "") ? datosSC.strTipoFondo : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecDevSol", SqlDbType.VarChar, datosSC.strFecDevSol, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        private void CargaThis_FondoSol(int numArch)
        {
            List<SolicitudesCotizacion> _SolicitudCotizacion = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "THISCARGAFONDO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    numArchivo = x.GetInt32(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strCodMon = x.GetString(2),
                    strCapPen = x.GetDecimal(3).ToString(),
                    strMtoCIC = x.GetDecimal(4).ToString(),
                    strMtoCuo = x.GetDecimal(5).ToString(),
                    strMtoSal = x.GetDecimal(6).ToString(),
                    strBonAct = x.GetDecimal(7).ToString(),
                    strCodCob = x.GetString(8),
                    strTipCamAA = x.GetDecimal(9).ToString(),
                    strCodCiaCob = x.GetString(10),
                    strApoAdi = x.GetDecimal(11).ToString()
                }).ToList();

                if (_SolicitudCotizacion != null)
                {
                    for (int i = 0; i < _SolicitudCotizacion.Count; i++)
                    {
                        cargaFondoTTP(_SolicitudCotizacion[i]);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void cargaFondoTTP(SolicitudesCotizacion datosSC)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGAFONDOTTP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, datosSC.numArchivo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, datosSC.intNumOpe, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCodMon", SqlDbType.VarChar, datosSC.strCodMon, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCapPen", SqlDbType.Decimal, datosSC.strCapPen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMtoCIC", SqlDbType.Decimal, datosSC.strMtoCIC, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMtoCuo", SqlDbType.Decimal, datosSC.strMtoCuo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strBonAct", SqlDbType.Decimal, datosSC.strBonAct, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipCamAA", SqlDbType.Decimal, datosSC.strTipCamAA, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMtoSal", SqlDbType.Decimal, datosSC.strMtoSal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCodCob", SqlDbType.VarChar, datosSC.strCodCob, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strApoAdi", SqlDbType.Decimal, datosSC.strApoAdi, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCodCiaCob", SqlDbType.VarChar, (datosSC.strCodCiaCob != "" || datosSC.strCodCiaCob != null) ? datosSC.strCodCiaCob : null, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        private void CargaThis_Afiliado(int numArch)
        {
            List<SolicitudesCotizacion> _SolicitudCotizacion = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "THISCARGAAFILIADO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    numArchivo = x.GetInt32(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strTipoDoc = x.GetString(2),
                    strNumDoc = x.GetString(3),
                    strApPat = x.GetString(4),
                    strApMat = x.GetString(5),
                    strNom = x.GetString(6),
                    strNomSec = x.GetString(7),
                    strSexo = x.GetString(8),
                    strFecNac = x.GetString(9),
                    strGraInv = x.GetString(10),
                    strSitInv = x.GetString(11),
                    strEstSob = x.GetString(12)
                }).ToList();

                if (_SolicitudCotizacion != null)
                {
                    for (int i = 0; i < _SolicitudCotizacion.Count; i++)
                    {
                        cargaAfiliadoTTP(_SolicitudCotizacion[i]);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void cargaAfiliadoTTP(SolicitudesCotizacion datosSC)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGAAFILIADOTTP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, datosSC.numArchivo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, datosSC.intNumOpe, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipoDoc", SqlDbType.VarChar, datosSC.strTipoDoc, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNumDoc", SqlDbType.VarChar, datosSC.strNumDoc, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strApPat", SqlDbType.VarChar, datosSC.strApPat, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strApMat", SqlDbType.VarChar, (datosSC.strApMat != "") ? datosSC.strApMat : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNom", SqlDbType.VarChar, datosSC.strNom, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNomSec", SqlDbType.VarChar, (datosSC.strNomSec != "") ? datosSC.strNomSec : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strSexo", SqlDbType.VarChar, datosSC.strSexo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecNac", SqlDbType.VarChar, datosSC.strFecNac, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strGraInv", SqlDbType.VarChar, (datosSC.strGraInv != "") ? datosSC.strGraInv : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strSitInv", SqlDbType.VarChar, (datosSC.strSitInv != "") ? datosSC.strSitInv : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strEstSob", SqlDbType.VarChar, (datosSC.strEstSob != "") ? datosSC.strEstSob : null, ParameterDirection.Input));
                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        private void CargaThis_Beneficiario(int numArch)
        {
            List<SolicitudesCotizacion> _SolicitudCotizacion = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "THISCARGABEN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    numArchivo = x.GetInt32(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strPatBen = x.GetString(2),
                    strMatBen = x.GetString(3),
                    strNomBen = x.GetString(4),
                    strNomSecBen = x.GetString(5),
                    strParBen = x.GetString(6),
                    strSitInvBen = x.GetString(7),
                    strFecNacBen = x.GetString(8),
                    strSexoBen = x.GetString(9),
                    contador = x.GetInt64(10)
                }).ToList();

                if (_SolicitudCotizacion != null)
                {
                    for (int i = 0; i < _SolicitudCotizacion.Count; i++)
                    {
                        cargaBenTTP(_SolicitudCotizacion[i], Convert.ToInt32(_SolicitudCotizacion[i].contador));
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void cargaBenTTP(SolicitudesCotizacion datosSC, int i)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGABENTTP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, datosSC.numArchivo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, datosSC.intNumOpe, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOrden", SqlDbType.Int, i, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPatBen", SqlDbType.VarChar, datosSC.strPatBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMatBen", SqlDbType.VarChar, (datosSC.strMatBen != "") ? datosSC.strMatBen : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNomBen", SqlDbType.VarChar, datosSC.strNomBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNomSecBen", SqlDbType.VarChar, (datosSC.strNomSecBen != "") ? datosSC.strNomSecBen : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strParBen", SqlDbType.VarChar, datosSC.strParBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strSitInvBen", SqlDbType.VarChar, datosSC.strSitInvBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecNacBen", SqlDbType.VarChar, datosSC.strFecNacBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strSexoBen", SqlDbType.VarChar, datosSC.strSexoBen, ParameterDirection.Input));
                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        private void CargaThis_prodsol(int numArch)
        {
            List<SolicitudesCotizacion> _SolicitudCotizacion = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "THISCARGAPRODSOL", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    numArchivo = x.GetInt32(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strCodMod = x.GetString(2),
                    strMonPro = x.GetString(3),
                    strannosRT = x.GetInt32(4).ToString(),
                    strPrcRVD = x.GetDecimal(5).ToString(),
                    strPerGar = x.GetString(6),
                    strCobCon = x.GetString(7),
                    strGratif = x.GetString(8),
                    strDerCre = x.GetString(9),
                    strPartCapital = x.GetString(10),
                    contador = x.GetInt64(11)
                }).ToList();

                if (_SolicitudCotizacion != null)
                {
                    for (int i = 0; i < _SolicitudCotizacion.Count; i++)
                    {
                        cargaProductoTTP(_SolicitudCotizacion[i], Convert.ToInt32(_SolicitudCotizacion[i].contador));
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void cargaProductoTTP(SolicitudesCotizacion datosSC, int i)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGAPROTTP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, datosSC.numArchivo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, datosSC.intNumOpe, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numMod", SqlDbType.Int, i, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCodMod", SqlDbType.VarChar, datosSC.strCodMod, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strannosRT", SqlDbType.Int, datosSC.strannosRT, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPrcRVD", SqlDbType.Decimal, datosSC.strPrcRVD, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMonPro", SqlDbType.VarChar, (datosSC.strMonPro != "") ? datosSC.strMonPro : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPerGar", SqlDbType.VarChar, (datosSC.strPerGar != "") ? datosSC.strPerGar : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCobCon", SqlDbType.VarChar, (datosSC.strCobCon != "") ? datosSC.strCobCon : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strDerCre", SqlDbType.VarChar, (datosSC.strDerCre != "") ? datosSC.strDerCre : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strGratif", SqlDbType.VarChar, (datosSC.strGratif != "") ? datosSC.strGratif : null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPartCapital", SqlDbType.VarChar, (datosSC.strPartCapital != "") ? datosSC.strPartCapital : null, ParameterDirection.Input));
                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public Response estadisticas(int numArch, string usuario)
        {
            Response solicitudes = new Response();
            string[] infoSolicitudes = new string[4];
            SolicitudesCotizacion datosSC = new SolicitudesCotizacion();
            try
            {   //Registro CARGASOL
                datosSC.intOKcarga = estadisticasOK("count(distinct(num_operacion)) as total FROM pt_ttmp_cargasol", numArch);
                datosSC.intERRcarga = estadisticasERR("count(distinct(num_operacion)) as total FROM pt_ttmp_cargasol", numArch);
                //Registro FONDO
                datosSC.intOKfondo = estadisticasOK("count(num_operacion) as total FROM pt_ttmp_fondosol", numArch);
                datosSC.intERRfondo = estadisticasERR("count(num_operacion) as total FROM pt_ttmp_fondosol", numArch);
                //Registro BENEFICIARIO
                datosSC.intOKbeneficiario = estadisticasOK("count(num_operacion) as total FROM pt_ttmp_beneficiario", numArch);
                datosSC.intERRbeneficiario = estadisticasERR("count(num_operacion) as total FROM pt_ttmp_beneficiario", numArch);
                //Registro AFILIADO
                datosSC.intOKafiliado = estadisticasOK("count(num_operacion) as total FROM pt_ttmp_afiliado", numArch);
                datosSC.intERRafiliado = estadisticasERR("count(num_operacion) as total FROM pt_ttmp_afiliado", numArch);
                //Registro PRODUCTO
                datosSC.intOKproducto = estadisticasOKRP("pt_ttmp_prodsol", numArch, "<>");
                datosSC.intERRproducto = estadisticasERRRP("pt_ttmp_prodsol", numArch, "<>");
                //Registro RP
                datosSC.intOKRP = estadisticasOKRP("pt_ttmp_prodsol", numArch, "=");
                datosSC.intERRRP = estadisticasERRRP("pt_ttmp_prodsol", numArch, "=");


                datosSC.regTotal = datosSC.intOKcarga + datosSC.intERRcarga;
                datosSC.regErr = datosSC.intERRcarga;
                datosSC.regOk = datosSC.intOKcarga;

                insertESTCARGASOL(numArch, datosSC, usuario);

                infoSolicitudes[0] = datosSC.regTotal.ToString();
                infoSolicitudes[1] = datosSC.regOk.ToString();
                infoSolicitudes[2] = datosSC.regErr.ToString();
                infoSolicitudes[3] = numArch.ToString();

                solicitudes.Object = infoSolicitudes;
                solicitudes.IsOk = true;
                return solicitudes;
            }
            catch (Exception ex)
            {
                Response solicitudes2 = new Response();
                solicitudes2.IsOk = false;
                solicitudes2.Message = ex.Message;
                return solicitudes2;
            }
        }
        public void insertESTCARGASOL(int numArch, SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            DateTime fecha = DateTime.Now;
            try
            {

                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ESTCARSOL", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numRegerrcar", SqlDbType.Int, datosSC.intERRcarga, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numRegokar", SqlDbType.Int, datosSC.intOKcarga, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numRegerrafi", SqlDbType.Int, datosSC.intERRafiliado, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numRegokafi", SqlDbType.Int, datosSC.intOKafiliado, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numRegerrfon", SqlDbType.Int, datosSC.intERRfondo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numRegokfon", SqlDbType.Int, datosSC.intOKfondo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numRegerrben", SqlDbType.Int, datosSC.intERRbeneficiario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numRegokben", SqlDbType.Int, datosSC.intOKbeneficiario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@num_regerrprod", SqlDbType.Int, datosSC.intERRproducto, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@num_regokprod", SqlDbType.Int, datosSC.intOKproducto, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numRegerrrp", SqlDbType.Int, datosSC.intERRRP, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numRegokrp", SqlDbType.Int, datosSC.intOKRP, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public int estadisticasOK(string tabla, int numArch)
        {
            try
            {
                SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
                string query = "SELECT " + tabla + " WHERE cod_error = '0' and num_archivo = " + numArch;
                int valor = 0;

                _SolicitudCotizacion = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = _SolicitudCotizacion.success;
                }

                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public int estadisticasERR(string tabla, int numArch)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                string query = "SELECT " + tabla + " WHERE cod_error <> '0' and num_archivo = " + numArch;
                int valor = 0;

                _SolicitudCotizacion = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = _SolicitudCotizacion.success;
                }

                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }



        public int estadisticasOKRP(string tabla, int numArch, string condicion)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                string query = "SELECT count(num_operacion) as total FROM " + tabla + " WHERE cod_error = '0' and num_archivo = " + numArch + " and cod_modalidad " + condicion + "'RP'";
                int valor = 0;

                _SolicitudCotizacion = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = _SolicitudCotizacion.success;
                }

                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public int estadisticasERRRP(string tabla, int numArch, string condicion)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                string query = "SELECT count(num_operacion) as total FROM " + tabla + " WHERE cod_error <> '0' and num_archivo = '" + numArch + "' and cod_modalidad " + condicion + " 'RP'";
                int valor = 0;

                _SolicitudCotizacion = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = _SolicitudCotizacion.success;
                }

                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void eliminaSinError()
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ELIMINARTTMP", ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = Convert.ToInt32(0)
                }).FirstOrDefault();
                var eliminados = _SolicitudCotizacion.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public void traspasoCotizacion(int numArch, string usuario)
        {
            List<SolicitudesCotizacion> _SolicitudCotizacion = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_MASTER", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    intNumOpe = Convert.ToInt32(x.GetDecimal(0)),
                    strAfp = x.GetString(1),
                    strCussp = x.GetString(2),
                    strTipBen = x.GetString(3),
                    strTasaRPRT = x.GetDecimal(4).ToString(),
                    strFecDev = x.GetString(5),
                    strFecSus = x.GetString(6),
                    strFecEnv = x.GetString(7),
                    strFecCie = x.GetString(8),
                    strTipCam = x.GetDecimal(9).ToString(),
                    existeNumOpe = Convert.ToInt32(x.GetDecimal(10)),
                    strCodMod = x.GetString(11),
                    strCapPen = x.GetDecimal(12).ToString(),
                    strMtoCIC = x.GetDecimal(13).ToString(),
                    strBonAct = x.GetDecimal(14).ToString(),
                    strCobCon = x.GetString(15),
                    strApoAdi = x.GetDecimal(16).ToString(),
                    strTipoDoc = x.GetString(17),
                    strNumDoc = x.GetString(18),
                    strSexo = x.GetString(19),
                    strFecNac = x.GetString(20),
                    strGraInv = x.GetString(21),
                    strGratif = x.GetString(22),
                    strEstSob = x.GetString(23),
                    strApPat = x.GetString(24),
                    strApMat = x.GetString(25),
                    strNomBen = x.GetString(26),
                    strNomSecBen = x.GetString(27),
                    strNumMensualidad = x.GetString(28),
                    strTipoFondo = x.GetString(29),
                    strDepto = x.GetString(30)
                }).ToList();

                if (_SolicitudCotizacion != null)
                {
                    for (int i = 0; i < _SolicitudCotizacion.Count; i++)
                    {
                        SolicitudesCotizacion datosSC = new SolicitudesCotizacion();
                        datosSC.strReajuste = "";
                        datosSC.strValReajusteMen = 0;
                        datosSC.strValReajusteTri = 0;
                        datosSC.intCor = 0;
                        datosSC.strAfp = homologarCodigo(_SolicitudCotizacion[i].strAfp, vgCodTabla_AFP);
                        if (_SolicitudCotizacion[i].strTipBen != "A" && _SolicitudCotizacion[i].strTipBen != "C" && _SolicitudCotizacion[i].strTipBen != "D")
                        {
                            datosSC.strTipPen = homologarCodigo("B", vgCodTabla_TipPen);
                            datosSC.strCodVejez = homologarCodigo(_SolicitudCotizacion[i].strTipBen, vgCodTabla_TipVejez);
                        }
                        else
                        {
                            datosSC.strTipPen = homologarCodigo(_SolicitudCotizacion[i].strTipBen, vgCodTabla_TipPen);
                            datosSC.strCodVejez = "S";
                        }
                        datosSC.strMon = homologarCodigo(_SolicitudCotizacion[i].strCodMod, vgCodTabla_Moneda);
                        datosSC.intTipIde = homologarTipoIden(_SolicitudCotizacion[i].strTipoDoc);
                        if (datosSC.strTipPen == "06")
                        {
                            datosSC.strSitInv = _SolicitudCotizacion[i].strGraInv;
                        }
                        else
                        {
                            datosSC.strSitInv = homologarSitInv(_SolicitudCotizacion[i].strGratif, _SolicitudCotizacion[i].strGraInv);
                        }
                        if (datosSC.strSitInv == "P" && datosSC.strTipPen == "06")
                        {
                            datosSC.strTipPen = "07";
                        }
                        datosSC.strNumCot = generarCorrelativo(datosSC, usuario);
                        datosSC.intNumOpe = _SolicitudCotizacion[i].intNumOpe;
                        datosSC.iNumOrden = 1;
                        grabarCotizacion(numArch, _SolicitudCotizacion[i], datosSC, usuario);
                        grabarCausante(numArch, _SolicitudCotizacion[i], datosSC, usuario);
                        consBeneficiario(numArch, _SolicitudCotizacion[i], datosSC, usuario);
                        consModalidades(numArch, _SolicitudCotizacion[i], datosSC, usuario);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public string homologarCodigo(string strCod, string vgCodTabla)
        {
            string valor = "";
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "HOMOLOGARCODIGO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vgCodTabla", SqlDbType.VarChar, vgCodTabla, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCod", SqlDbType.VarChar, strCod, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    strSuccess = x.GetString(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = _SolicitudCotizacion.strSuccess;
                }
                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public int homologarTipoIden(string strCod)
        {
            int valor = 0;
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "HOMOLOGARTIPOIDEN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCod", SqlDbType.VarChar, strCod, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = _SolicitudCotizacion.success;
                }
                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public string homologarSitInv(string strInv, string strGrado)
        {
            string valor = "";
            switch (strInv)
            {
                case "S":
                    switch (strGrado)
                    {
                        case "T": valor = "T"; break;
                        case "P": valor = "P"; break;
                        case "": valor = "T"; break;
                    }
                    break;
                case "N": valor = "N"; break;
                default: valor = "N"; break;
            }
            return valor;
        }
        public string generarCorrelativo(SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            datosSC.strOperacion = "";
            DateTime fecha = DateTime.Now;
            datosSC.intAnno = int.Parse(fecha.ToString("yyyy"));
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GENCORRELATIVO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intAnno", SqlDbType.Int, datosSC.intAnno, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    datosSC.strNumCot = (_SolicitudCotizacion.success + 1).ToString();
                    datosSC.strOperacion = "A";
                }
                else
                {
                    datosSC.strNumCot = 1.ToString();
                    datosSC.strOperacion = "I";
                }

                if (datosSC.strOperacion == "I")
                {
                    generarCorrelativo_I(datosSC, usuario);
                }
                else
                {
                    generarCorrelativo_U(datosSC, usuario);
                }
                datosSC.strCor = datosSC.intAnno + datosSC.strNumCot;
                return datosSC.strCor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void generarCorrelativo_I(SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            DateTime fecha = DateTime.Now;
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GENCORRELATIVO_I", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numAnno", SqlDbType.Int, datosSC.intAnno, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCotizacion", SqlDbType.Int, int.Parse(datosSC.strNumCot), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecha", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@hora", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void generarCorrelativo_U(SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            DateTime fecha = DateTime.Now;
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GENCORRELATIVO_U", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numAnno", SqlDbType.Int, datosSC.intAnno, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCotizacion", SqlDbType.Int, int.Parse(datosSC.strNumCot), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioModi", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fechaModi", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horaModi", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void grabarCotizacion(int numArch, SolicitudesCotizacion _SolicitudesCotizacion, SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion insertSC = new SolicitudesCotizacion();
            DateTime fecha = DateTime.Now;
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "G_COTIZACION", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, Convert.ToDecimal(datosSC.intNumOpe), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecSusc", SqlDbType.VarChar, _SolicitudesCotizacion.strFecSus, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecEnv", SqlDbType.VarChar, _SolicitudesCotizacion.strFecEnv, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecCie", SqlDbType.VarChar, _SolicitudesCotizacion.strFecCie, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecDev", SqlDbType.VarChar, _SolicitudesCotizacion.strFecDev, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codAfp", SqlDbType.VarChar, datosSC.strAfp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTipPen", SqlDbType.VarChar, datosSC.strTipPen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codVejez", SqlDbType.VarChar, datosSC.strCodVejez, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstCivil", SqlDbType.VarChar, "S", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strCussp", SqlDbType.VarChar, _SolicitudesCotizacion.strCussp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTipIden", SqlDbType.Int, datosSC.intTipIde, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numIden", SqlDbType.VarChar, _SolicitudesCotizacion.strNumDoc, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numIdenCor", SqlDbType.VarChar, "0", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codMonFon", SqlDbType.VarChar, datosSC.strMon, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoMonFon", SqlDbType.Decimal, (datosSC.strMon != "NS") ? Convert.ToDecimal(_SolicitudesCotizacion.strTipCam) : Convert.ToDecimal(1), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoCtaIndFon", SqlDbType.Decimal, Convert.ToDecimal(_SolicitudesCotizacion.strMtoCIC), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoBonoFon", SqlDbType.Decimal, Convert.ToDecimal(_SolicitudesCotizacion.strBonAct), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoPriunifon", SqlDbType.Decimal, Convert.ToDecimal(_SolicitudesCotizacion.strCapPen), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@prcTasarprt", SqlDbType.Decimal, Convert.ToDecimal(_SolicitudesCotizacion.strTasaRPRT), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strApoAdi", SqlDbType.Decimal, Convert.ToDecimal(_SolicitudesCotizacion.strApoAdi), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@indCob", SqlDbType.VarChar, _SolicitudesCotizacion.strCobCon, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codtipCot", SqlDbType.VarChar, "C", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codNumen", SqlDbType.VarChar, ((_SolicitudesCotizacion.strNumMensualidad != "" || _SolicitudesCotizacion.strNumMensualidad != null) ? _SolicitudesCotizacion.strNumMensualidad : null), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strTipoFondo", SqlDbType.VarChar, ((_SolicitudesCotizacion.strTipoFondo != "" || _SolicitudesCotizacion.strTipoFondo != null) ? _SolicitudesCotizacion.strTipoFondo : null), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strDepto", SqlDbType.VarChar, ((_SolicitudesCotizacion.strDepto != "" || _SolicitudesCotizacion.strDepto != null) ? _SolicitudesCotizacion.strDepto : null), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                insertSC = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = insertSC.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }

        }
        public void grabarCausante(int numArch, SolicitudesCotizacion _SolicitudesCotizacion, SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            DateTime fecha = DateTime.Now;
            try
            {
                var parameters = new List<SqlParameter>();
                if (datosSC.strannosRT == null && datosSC.strPrcRVD == null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "G_CAUSANTE1", ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "G_CAUSANTE2", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codDerpen", SqlDbType.VarChar, datosSC.strPrcRVD, ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOrden", SqlDbType.Int, datosSC.iNumOrden, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, Convert.ToDecimal(datosSC.intNumOpe), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codPar", SqlDbType.VarChar, "99", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codGruFam", SqlDbType.VarChar, "00", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codSexo", SqlDbType.VarChar, _SolicitudesCotizacion.strSexo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codSitinv", SqlDbType.VarChar, datosSC.strSitInv, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPatBen", SqlDbType.VarChar, _SolicitudesCotizacion.strApPat, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMatBen", SqlDbType.VarChar, ((_SolicitudesCotizacion.strApMat != "" || _SolicitudesCotizacion.strApMat != null) ? _SolicitudesCotizacion.strApMat : null), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNomBen", SqlDbType.VarChar, _SolicitudesCotizacion.strNomBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNomSecBen", SqlDbType.VarChar, ((_SolicitudesCotizacion.strNomSecBen != "" || _SolicitudesCotizacion.strNomSecBen != null) ? _SolicitudesCotizacion.strNomSecBen : null), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTipIden", SqlDbType.Int, datosSC.intTipIde, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numIden", SqlDbType.VarChar, _SolicitudesCotizacion.strNumDoc, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecNacBen", SqlDbType.VarChar, _SolicitudesCotizacion.strFecNac, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codESTSOB", SqlDbType.VarChar, ((_SolicitudesCotizacion.strEstSob != "" || _SolicitudesCotizacion.strEstSob != null) ? _SolicitudesCotizacion.strEstSob : null), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));


                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _SolicitudCotizacion.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void consBeneficiario(int numArch, SolicitudesCotizacion _SolicitudesCotizacion, SolicitudesCotizacion datosSC, string usuario)
        {
            List<SolicitudesCotizacion> _SCBeneficiario = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSBEN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, Convert.ToDecimal(datosSC.intNumOpe), ParameterDirection.Input));

                _SCBeneficiario = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    iNumOrden = x.GetInt32(0),
                    strPatBen = x.GetString(1),
                    strMatBen = x.GetString(2),
                    strNomBen = x.GetString(3),
                    strNomSecBen = x.GetString(4),
                    strParBen = x.GetString(5),
                    strSitInvBen = x.GetString(6),
                    strFecNacBen = x.GetString(7),
                    strSexoBen = x.GetString(8)

                }).ToList();

                if (_SCBeneficiario != null)
                {
                    for (int i = 0; i < _SCBeneficiario.Count; i++)
                    {
                        grabarBeneficiario(numArch, _SCBeneficiario[i], datosSC, usuario);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void grabarBeneficiario(int numArch, SolicitudesCotizacion _SCBeneficiario, SolicitudesCotizacion datosSC, string usuario)
        {
            datosSC.iNumOrden = datosSC.iNumOrden + 1;
            datosSC.strPar = homologarPar(_SCBeneficiario.strParBen, _SCBeneficiario.strSexoBen);
            datosSC.strSitInvBen = homologarSitInv(_SCBeneficiario.strSitInvBen, "T");
            SolicitudesCotizacion insertSC = new SolicitudesCotizacion();
            DateTime fecha = DateTime.Now;
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_CONSBEN", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOrden", SqlDbType.Int, datosSC.iNumOrden, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, Convert.ToDecimal(datosSC.intNumOpe), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codPar", SqlDbType.VarChar, datosSC.strPar.Replace(" ",""), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codGruFam", SqlDbType.VarChar, "01", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codSexo", SqlDbType.VarChar, _SCBeneficiario.strSexoBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codSitinv", SqlDbType.VarChar, _SCBeneficiario.strSitInvBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPatBen", SqlDbType.VarChar, _SCBeneficiario.strPatBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strMatBen", SqlDbType.VarChar, ((_SCBeneficiario.strMatBen != "" || _SCBeneficiario.strMatBen != null) ? _SCBeneficiario.strMatBen : null), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNomBen", SqlDbType.VarChar, _SCBeneficiario.strNomBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strNomSecBen", SqlDbType.VarChar, ((_SCBeneficiario.strNomSecBen != "" || _SCBeneficiario.strNomSecBen != null) ? _SCBeneficiario.strNomSecBen : null), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecNacBen", SqlDbType.VarChar, _SCBeneficiario.strFecNacBen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                insertSC = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = insertSC.success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public string homologarPar(string strPar, string strSexo)
        {
            string valor = "";
            switch (strPar)
            {
                case "CY": valor = "11"; break;
                case "HI": valor = "30"; break;
                case "CB": valor = "21"; break;
                default:
                    if (strSexo == "F")
                    {
                        valor = "42";
                    }
                    else
                    {
                        valor = "41";
                    }
                    break;
            }
            return valor;
        }
        public void consModalidades(int numArch, SolicitudesCotizacion _SolicitudesCotizacion, SolicitudesCotizacion datosSC, string usuario)
        {
            List<SolicitudesCotizacion> _SCModalidad = new List<SolicitudesCotizacion>();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSMOD", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, Convert.ToDecimal(datosSC.intNumOpe), ParameterDirection.Input));

                _SCModalidad = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    strCodMod = x.GetString(0),
                    strCodMon = x.GetString(1),
                    strannosRT = x.GetInt32(2).ToString(),
                    strPerGar = x.GetString(3),
                    strCobCon = x.GetString(4),
                    strGraInv = x.GetString(5),
                    strGratif = x.GetString(6),
                    strPrcRVD = x.GetDecimal(7).ToString(),
                    strPartCapital = x.GetString(8)
                }).ToList();

                if (_SCModalidad != null)
                {
                    for (int i = 0; i < _SCModalidad.Count; i++)
                    {
                        grabarModalidades(numArch, _SCModalidad[i], datosSC, usuario, _SolicitudesCotizacion);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public void grabarModalidades(int numArch, SolicitudesCotizacion _SCModalidad, SolicitudesCotizacion datosSC, string usuario, SolicitudesCotizacion _SolicitudesCotizacion)
        {
            datosSC.strTipRen = homologarCodigo(_SCModalidad.strCodMod, vgCodTabla_TipRen);
            if (_SCModalidad.strPerGar != "")
            {
                datosSC.strMod = "3";
                datosSC.intNumGar = int.Parse(_SCModalidad.strPerGar) * 12;
            }
            else
            {
                datosSC.strMod = "1";
                datosSC.intNumGar = 0;
            }
            if (_SCModalidad.strCobCon != "")
            {
                datosSC.strCobCon = _SCModalidad.strCobCon;
                if (datosSC.strMod == "3") { datosSC.strMod = "4"; }
            }
            else
            {
                datosSC.strCobCon = "";
            }
            if (_SCModalidad.strCodMon != "")
            {
                datosSC.strMonMod = _SCModalidad.strCodMon;
                if (homologarMoneda(datosSC.strMonMod, datosSC.strReajuste, datosSC) == false)
                {
                    return;
                }
            }
            else
            {
                if (datosSC.strTipRen == "4")
                {
                    datosSC.strMonMod = "US";
                    datosSC.strReajuste = "0";
                }
            }
            datosSC.intCor = datosSC.intCor + 1;
            if (_SCModalidad.strannosRT != "0")
            {
                if (datosSC.strTipRen == "6")
                {
                    //Tipo Renta Escalonada
                    datosSC.intNumDif = 0;
                    datosSC.douPrcRVD = 0;
                    datosSC.intNumEsc = int.Parse(_SCModalidad.strannosRT) * 12;
                    datosSC.douPrcRtaEsc = double.Parse(_SCModalidad.strPrcRVD);
                }
                else
                {
                    //Para las demas Rentas (distintas a escalonadas) sigue haciendo lo mismo
                    datosSC.intNumDif = int.Parse(_SCModalidad.strannosRT) * 12;
                    datosSC.douPrcRVD = double.Parse(_SCModalidad.strPrcRVD);
                    datosSC.intNumEsc = 0;
                    datosSC.douPrcRtaEsc = 0;
                }
            }
            else
            {
                datosSC.intNumDif = 0;
                datosSC.douPrcRVD = 0;
                datosSC.intNumEsc = 0;
                datosSC.douPrcRtaEsc = 0;
            }
            SolicitudesCotizacion _insertSC = new SolicitudesCotizacion();
            DateTime fecha = DateTime.Now;
            try
            {
                var parameters = new List<SqlParameter>();
                if (datosSC.strannosRT == null && datosSC.strPrcRVD == null)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_CONSMOD2", ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_CONSMOD1", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCoberCon", SqlDbType.VarChar, datosSC.strCobCon, ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intCor", SqlDbType.Int, datosSC.intCor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, Convert.ToDecimal(datosSC.intNumOpe), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codMonFon", SqlDbType.VarChar, datosSC.strMonMod, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoMonFon", SqlDbType.VarChar, ((datosSC.strMonMod != "NS") ? _SolicitudesCotizacion.strTipCam : "1"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTipren", SqlDbType.VarChar, datosSC.strTipRen, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numMesDif", SqlDbType.Int, datosSC.intNumDif, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codModalidad", SqlDbType.VarChar, datosSC.strMod, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numMesGar", SqlDbType.Int, datosSC.intNumGar, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codDergra", SqlDbType.VarChar, _SCModalidad.strGraInv, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codDergre", SqlDbType.VarChar, _SCModalidad.strGratif, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstCot", SqlDbType.VarChar, "N", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@prcRentaTMP", SqlDbType.Decimal, Convert.ToDecimal(datosSC.douPrcRVD), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numMesEsc", SqlDbType.Int, datosSC.intNumEsc, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@prcRentaEsc", SqlDbType.Decimal, Convert.ToDecimal(datosSC.douPrcRtaEsc), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codRechazo", SqlDbType.VarChar, "0", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codPerCap", SqlDbType.VarChar, ((_SCModalidad.strPartCapital != "" || _SolicitudesCotizacion.strPartCapital != null) ? _SolicitudesCotizacion.strPartCapital : null), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTipreAjuste", SqlDbType.VarChar, ((datosSC.strReajuste != "" || datosSC.strReajuste != null) ? datosSC.strReajuste : null), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoValReajusteTri", SqlDbType.Decimal, Convert.ToDecimal(datosSC.strValReajusteTri), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoValReajusteMen", SqlDbType.Decimal, Convert.ToDecimal(datosSC.strValReajusteMen), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@prcCorCom", SqlDbType.Decimal, 2.4, ParameterDirection.Input));

                _insertSC = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();
                var su = _insertSC.success;

                //SEGUIMIENTO -- ETAPAS
                datosSC.Cod_Etapa = "10"; //vgIngresoOA
                datosSC.strNumCot = datosSC.strNumCot;
                datosSC.Num_Correlativo = datosSC.intCor.ToString();
                datosSC.strFecSeg = busca_FechaServidor(datosSC);
                datosSC.Fec_Ini = datosSC.strFecSeg;
                datosSC.Hor_Ini = busca_HoraSer();
                datosSC.strEtapa = etapa_Anterior(datosSC);
                datosSC.Cod_EtapaAnt = datosSC.strEtapa;
                //validar si se ha registrado anteriormente la misma etapa
                datosSC.bolSw = validarEtapas(datosSC);
                if (datosSC.bolSw == false)
                {   //no existe etapa
                    insert_Ingreso(datosSC, usuario);
                }
                else
                {   //Actualiza
                    actualizarEtapa(datosSC, usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public bool homologarMoneda(string ioCodMoneda, string ioCodReajuste, SolicitudesCotizacion datosSC)
        {
            var valor = false;
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "HOMOLOGARMONEDA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@ioCodMoneda", SqlDbType.VarChar, ioCodMoneda, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    ioCodMoneda = x.GetString(0),
                    ioCodReajuste = x.GetString(1),
                    strCodSCOMP = x.GetString(2)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    datosSC.strMonMod = _SolicitudCotizacion.ioCodMoneda;
                    datosSC.strReajuste = _SolicitudCotizacion.ioCodReajuste;
                    valor = true;
                }
                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Carga de Archivo de Solicitudes");
                throw;
            }
        }
        public bool validarEtapas(SolicitudesCotizacion datosSC)
        {
            var valor = false;
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VALETAPAS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEtapa", SqlDbType.VarChar, datosSC.Cod_Etapa, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, datosSC.Num_Correlativo, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    strSuccess = x.GetString(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = true;
                }
                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public string busca_FechaServidor(SolicitudesCotizacion datosSC)
        {
            var valor = "";
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "FECSERV", ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    strSuccess = x.GetString(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    datosSC.strFecha = _SolicitudCotizacion.strSuccess;
                    valor = _SolicitudCotizacion.strSuccess;

                }
                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public string busca_HoraSer()
        {
            var valor = "";
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "HORSERV", ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    strSuccess = x.GetString(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = _SolicitudCotizacion.strSuccess;

                }
                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public string etapa_Anterior(SolicitudesCotizacion datosSC)
        {
            var valor = "";
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ETAPAANTERIOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, datosSC.Num_Correlativo, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    strSuccess = x.GetString(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    datosSC.strEtapa = _SolicitudCotizacion.strSuccess;
                    valor = _SolicitudCotizacion.strSuccess;

                }
                return valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void insert_Ingreso(SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_INGRESO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.Int, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, datosSC.Num_Correlativo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEtapa", SqlDbType.VarChar, datosSC.Cod_Etapa, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecha", SqlDbType.VarChar, datosSC.Fec_Ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fechaT", SqlDbType.VarChar, null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@hora", SqlDbType.VarChar, datosSC.Fec_Ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horaT", SqlDbType.VarChar, null, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    var exitoso = _SolicitudCotizacion.success;
                }
                if (datosSC.Cod_EtapaAnt != "")
                {
                    insert_IngresoU(datosSC, usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void insert_IngresoU(SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_INGRESO", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.Int, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, datosSC.Num_Correlativo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEtapa", SqlDbType.VarChar, datosSC.Cod_Etapa, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fechaT", SqlDbType.VarChar, datosSC.Fec_Ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horaT", SqlDbType.VarChar, datosSC.Fec_Ini, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    var exitoso = _SolicitudCotizacion.success;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void actualizarEtapa(SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ACTUALIZARETAPA1", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.Int, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, datosSC.Num_Correlativo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEtapa", SqlDbType.VarChar, datosSC.Cod_Etapa, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecha", SqlDbType.VarChar, datosSC.Fec_Ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@hora", SqlDbType.VarChar, datosSC.Fec_Ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    var exitoso = _SolicitudCotizacion.success;
                }
                if (datosSC.Cod_EtapaAnt != "")
                {
                    actualizarEtapa2(datosSC, usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void actualizarEtapa2(SolicitudesCotizacion datosSC, string usuario)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ACTUALIZARETAPA2", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.Int, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, datosSC.Num_Correlativo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEtapa", SqlDbType.VarChar, datosSC.Cod_Etapa, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecha", SqlDbType.VarChar, datosSC.Fec_Ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@hora", SqlDbType.VarChar, datosSC.Fec_Ini, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    var exitoso = _SolicitudCotizacion.success;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void validacionesFinales(int numArch, string usuario)
        {
            calculoPrima(numArch, usuario);
            calculoPrimaMod(numArch, usuario);
            validaGrupoFam(numArch, usuario);
            validaSobFecFalCau(numArch);
        }
        public void calculoPrima(int numArch, string usuario)
        {
            List<SolicitudesCotizacion> _primaSC = new List<SolicitudesCotizacion>();
            try
            {
                string query = "select num_cot,num_operacion,cod_monedafon,mto_monedafon,mto_ctaindfon,mto_priunifon,mto_bonofon from pt_tmae_cotizacion where num_archivo = '" + numArch + "'";
                string mto = "";

                _primaSC = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    strNumCot = x.GetString(0),
                    intNumOpe = Convert.ToInt32(x.GetDecimal(1)),
                    strCodMon = x.GetString(2),
                    strMon = Convert.ToString(x.GetDecimal(3)),
                    strMtoCuo = Convert.ToString(x.GetDecimal(4)),
                    strMtoSal = Convert.ToString(x.GetDecimal(5)),
                    strMtoCIC = Convert.ToString(x.GetDecimal(6))
                }).ToList();

                if (_primaSC != null)
                {
                    for (int i = 0; i < _primaSC.Count; i++)
                    {
                        calculoPrimaU(_primaSC[i], usuario, mto);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void calculoPrimaU(SolicitudesCotizacion _primaSC, string usuario, string mto)
        {
            SolicitudesCotizacion datosSC = new SolicitudesCotizacion();

            if (_primaSC.strCodMon != "NS")
            {
                datosSC.strNumCot = _primaSC.strNumCot;
                datosSC.intNumOpe = _primaSC.intNumOpe;
                mto = _primaSC.strMon;
                datosSC.strMtoCuo = (float.Parse(_primaSC.strMtoCuo) * float.Parse(mto)).ToString("0.00");
                datosSC.strMtoSal = (float.Parse(_primaSC.strMtoSal) * float.Parse(mto)).ToString("0.00");
                datosSC.strMtoCIC = (float.Parse(_primaSC.strMtoCIC) * float.Parse(mto)).ToString("0.00");
            }
            else
            {
                datosSC.strNumCot = _primaSC.strNumCot;
                datosSC.intNumOpe = _primaSC.intNumOpe;
                datosSC.strMtoCuo = _primaSC.strMtoCuo;
                datosSC.strMtoSal = _primaSC.strMtoSal;
                datosSC.strMtoCIC = _primaSC.strMtoCIC;
            }
            DateTime fecha = DateTime.Now;
            var valor = 0;
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CALCULOPRIMA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@intNumOper", SqlDbType.Decimal, Convert.ToDecimal(datosSC.intNumOpe), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoPriUni", SqlDbType.Decimal, Convert.ToDecimal(datosSC.strMtoSal), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoCtaInd", SqlDbType.Decimal, Convert.ToDecimal(datosSC.strMtoCuo), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoBono", SqlDbType.Decimal, Convert.ToDecimal(datosSC.strMtoCIC), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = _SolicitudCotizacion.success;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void calculoPrimaMod(int numArch, string usuario)
        {
            List<SolicitudesCotizacion> _primaModSC = new List<SolicitudesCotizacion>();
            string mto = "";
            try
            {
                string query = "select c.num_cot,d.num_correlativo,d.cod_moneda,d.mto_valmoneda,c.mto_ctaind,c.mto_priuni,c.mto_bono" +
                              " from pt_tmae_cotizacion c, pt_tmae_detcotizacion d " +
                              "where c.num_archivo = '" + numArch +
                              "' and d.num_archivo=c.num_archivo" +
                              " and c.num_cot=d.num_cot" +
                              " order by c.num_cot,d.num_correlativo";

                _primaModSC = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    strNumCot = x.GetString(0),  // strNumCot
                    Num_Correlativo = Convert.ToString(x.GetInt32(1)), //num_correlativo
                    strCodMon = x.GetString(2),  //cod_moneda
                    strMod = Convert.ToString(x.GetDecimal(3)),  //mto_valmoneda
                    strMtoCuo = Convert.ToString(x.GetDecimal(4)),  //mto_ctaind
                    strMtoSal = Convert.ToString(x.GetDecimal(5)),  //mto_priuni
                    strMtoCIC = Convert.ToString(x.GetDecimal(6))  //mto_bono
                }).ToList();

                if (_primaModSC != null)
                {
                    for (int i = 0; i < _primaModSC.Count; i++)
                    {
                        calculoPrimaModU(_primaModSC[i], usuario, mto);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void calculoPrimaModU(SolicitudesCotizacion _primaModSC, string usuario, string mto)
        {
            SolicitudesCotizacion datosSC = new SolicitudesCotizacion();
            if (_primaModSC.strCodMon != "NS")
            {
                datosSC.strNumCot = _primaModSC.strNumCot;
                datosSC.Num_Correlativo = _primaModSC.Num_Correlativo;
                mto = _primaModSC.strMod;
                datosSC.strMtoCuo = (float.Parse(_primaModSC.strMtoCuo) / float.Parse(mto)).ToString("0.00");
                datosSC.strMtoSal = (float.Parse(_primaModSC.strMtoSal) / float.Parse(mto)).ToString("0.00");
                datosSC.strMtoCIC = (float.Parse(_primaModSC.strMtoCIC) / float.Parse(mto)).ToString("0.00");

            }
            else
            {
                datosSC.strNumCot = _primaModSC.strNumCot;
                datosSC.Num_Correlativo = _primaModSC.Num_Correlativo;
                datosSC.strMtoCuo = _primaModSC.strMtoCuo;
                datosSC.strMtoSal = _primaModSC.strMtoSal;
                datosSC.strMtoCIC = _primaModSC.strMtoCIC;
            }
            DateTime fecha = DateTime.Now;
            var valor = 0;
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CALCULOPRIMAMOD", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datosSC.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, Convert.ToInt32(datosSC.Num_Correlativo), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoPriUni", SqlDbType.Decimal, Convert.ToDecimal(datosSC.strMtoSal), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoCtaInd", SqlDbType.Decimal, Convert.ToDecimal(datosSC.strMtoCuo), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoBono", SqlDbType.Decimal, Convert.ToDecimal(datosSC.strMtoCIC), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = _SolicitudCotizacion.success;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void validaGrupoFam(int numArch, string usuario)
        {
            List<SolicitudesCotizacion> dr = new List<SolicitudesCotizacion>();
            try
            {
                string query = "SELECT DISTINCT(num_cot) FROM PT_TMAE_COTBEN WHERE num_archivo = '" + numArch + "'";

                dr = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    strNumCot = x.GetString(0)
                }).ToList();

                if (dr != null)
                {
                    for (int i = 0; i < dr.Count; i++)
                    {
                        validarGF(dr[i], usuario, numArch);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void validarGF(SolicitudesCotizacion dr, string usuario, int numArch)
        {
            SolicitudesCotizacion dr3 = new SolicitudesCotizacion();
            int cntPar;
            try
            {
                string query = "SELECT COUNT(DISTINCT (COD_PAR))AS PAR FROM PT_TMAE_COTBEN WHERE num_archivo = '" + numArch + "' " +
                                "AND num_cot= '" + dr.strNumCot + "' AND COD_PAR<> '99' AND COD_PAR<>'41' AND COD_PAR<>'42' order by PAR asc";

                dr3 = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    strPar = Convert.ToString(x.GetInt32(0))
                }).FirstOrDefault();

                if (dr3 != null)
                {
                    cntPar = int.Parse(dr3.strPar);
                    validarGF2(dr, usuario, numArch, cntPar);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void validarGF2(SolicitudesCotizacion dr, string usuario, int numArch, int cntPar)
        {
            List<SolicitudesCotizacion> dr2 = new List<SolicitudesCotizacion>();
            int intCnt = 0;
            try
            {
                string query = "SELECT DISTINCT (COD_PAR) FROM PT_TMAE_COTBEN WHERE num_archivo = '" + numArch + "' " +
                                "AND num_cot= '" + dr.strNumCot + "' AND COD_PAR<> '99' AND COD_PAR<>'41' AND COD_PAR<>'42' order by COD_PAR asc";

                dr2 = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    strPar = x.GetString(0)
                }).ToList();

                if (dr2 != null)
                {
                    for (int i = 0; i < dr2.Count; i++)
                    {
                        validarGFU(dr2[i], usuario, intCnt, cntPar, dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void validarGFU(SolicitudesCotizacion dr2, string usuario, int intCnt, int cntPar, SolicitudesCotizacion dr)
        {
            DateTime fecha = DateTime.Now;
            SolicitudesCotizacion datosSC = new SolicitudesCotizacion();
            intCnt = intCnt + 1;
            string strPar = "";
            if (dr2.strPar == "11")
            {
                if (cntPar < 2)
                {
                    strPar = " 10";
                }
            }
            if (dr2.strPar == "21")
            {
                if (cntPar < 2)
                {
                    strPar = " 20";
                }
            }
            if (dr2.strPar == "30")
            {
                if (intCnt == 1)
                {
                    strPar = " 30";
                }
            }
            if (strPar != "")
            {
                SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
                int valor;
                try
                {
                    var parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "VALGRUPFAM", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, dr.strNumCot, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@strPar", SqlDbType.VarChar, dr2.strPar, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codPar", SqlDbType.VarChar, strPar, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codUsuarioCrea", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                    _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                    {
                        success = x.GetInt32(0)
                    }).FirstOrDefault();

                    if (_SolicitudCotizacion != null)
                    {
                        valor = _SolicitudCotizacion.success;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        public void validaSobFecFalCau(int numArch)
        {
            List<SolicitudesCotizacion> sobFecFalCau = new List<SolicitudesCotizacion>();
            try
            {
                string query = "SELECT DISTINCT num_cot,FEC_DEV FROM PT_TMAE_COTIZACION WHERE num_archivo = '" + numArch + "'" +
                                " AND COD_TIPPENSION = '08'";

                sobFecFalCau = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    strNumCot = x.GetString(0),
                    strFecDev = x.GetString(1)
                }).ToList();

                if (sobFecFalCau != null)
                {
                    for (int i = 0; i < sobFecFalCau.Count; i++)
                    {
                        if (sobFecFalCau[i].strFecDev != "")
                        {
                            validaSobFecFalCauU(sobFecFalCau[i], numArch);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void validaSobFecFalCauU(SolicitudesCotizacion sobFecFalCau, int numArch)
        {
            SolicitudesCotizacion _SolicitudCotizacion = new SolicitudesCotizacion();
            int valor;
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "SOBFECFALCAU", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, sobFecFalCau.strNumCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@strFecDev", SqlDbType.VarChar, sobFecFalCau.strFecDev, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArch", SqlDbType.Int, numArch, ParameterDirection.Input));

                _SolicitudCotizacion = VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new SolicitudesCotizacion
                {
                    success = x.GetInt32(0)
                }).FirstOrDefault();

                if (_SolicitudCotizacion != null)
                {
                    valor = _SolicitudCotizacion.success;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 13-12-2018
        /// </summary>
        /// <param name="numArchivo">Número de archivo genarado para el archivo cargado.</param>
        /// <returns>Lista de valores para la generación del reporte de resumen de carga.</returns>
        public List<SolicitudesCotizacion> RptResumenCarga(string numArchivo)
        {
            string query = "";
            try
            {
                query = "SELECT * FROM PT_THIS_ESTCARSOL WHERE NUM_ARCHIVO = '" + numArchivo + "' ";

                return SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    NUM_ARCH_RESUMEN = x.GetInt32(0),
                    NUM_REGERRCAR = x.GetInt32(1),
                    NUM_REGOKCAR = x.GetInt32(2),
                    TotalCarga = x.GetInt32(2) + x.GetInt32(1),
                    NUM_REGERRAFI = x.GetInt32(3),
                    NUM_REGOKAFI = x.GetInt32(4),
                    TotalAfi = x.GetInt32(4) + x.GetInt32(3),
                    NUM_REGERRFON = x.GetInt32(5),
                    NUM_REGOKFON = x.GetInt32(6),
                    TotalFondo = x.GetInt32(6) + x.GetInt32(5),
                    NUM_REGERRBEN = x.GetInt32(7),
                    NUM_REGOKBEN = x.GetInt32(8),
                    TotalBen = x.GetInt32(8) + x.GetInt32(7),
                    NUM_REGERRPROD = x.GetInt32(9),
                    NUM_REGOKPROD = x.GetInt32(10),
                    TotalProd = x.GetInt32(10) + x.GetInt32(9),
                    NUM_REGERRRP = x.GetInt32(17),
                    NUM_REGOKRP = x.GetInt32(18),
                    TotalRP = x.GetInt32(18) + x.GetInt32(17)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al generar reporte de resumen de carga de solicitudes." + ex.Message);
                throw;
            }
        }
        public List<GenArMeler> informacionGenerar(string numArchivo, string fec)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCART", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCalculo", SqlDbType.VarChar, fec, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, int.Parse(numArchivo), ParameterDirection.Input));
                return VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    numOperacion = Convert.ToInt32(x.GetDecimal(0)),
                    Correlativo = x.GetInt32(1),
                    nomArchivo = x.GetString(17),
                    tipoArchivo = x.GetString(18),
                    fechaCrea = x.GetString(19),
                    horaCrea = x.GetString(20),
                    usuarioCrea = x.GetString(21),
                    numArch = x.GetInt32(22),
                    numCot = x.GetString(23)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public string getFechaEnvio(string numArchivo)
        {
            try
            {
                GenArMeler dato = new GenArMeler();
                string query = "select distinct fec_envio from PT_TMAE_COTIZACION where num_archivo = '" + numArchivo + "' ";

                dato = SRVDBContext<GenArMeler>.CallSelectStatement(query, x => new GenArMeler
                {
                    fecEnvio = x.GetString(0)
                }).FirstOrDefault();

                return dato.fecEnvio;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string validacionesSISCO(int numArch)
        {
            string querys = "";
            try
            {
                XmlConfigurator.Configure();
                _log.Info("VALIDACIONES SISCO");
                List<SolicitudesCotizacion> datos = new List<SolicitudesCotizacion>();
                string query = "SELECT C.NUM_OPERACION, C.COD_CUSPP, DC.COD_TIPREN, DC.NUM_MESDIF, DC.NUM_MESGAR, DC.NUM_CORRELATIVO, DC.COD_TIPREAJUSTE, DC.COD_TIPREAJUSTE " +
                               "FROM PT_TMAE_COTIZACION C " +
                               "INNER JOIN PT_TMAE_DETCOTIZACION DC ON C.NUM_ARCHIVO = DC.NUM_ARCHIVO AND C.NUM_OPERACION = DC.NUM_OPERACION " +
                               "where C.NUM_ARCHIVO = '" + numArch + "' ";

                datos = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    intNumOpe = Convert.ToInt32(x.GetDecimal(0)),
                    strCussp = x.GetString(1),
                    strTipRen = x.GetString(2),
                    intNumDif = x.GetInt32(3),
                    intNumGar = x.GetInt32(4),
                    intCor = x.GetInt32(5),
                    strReajuste = x.GetString(6),
                    codtipreajuste = x.GetString(7)
                }).ToList();

                _log.Info("Datos de la consulta" + datos.ToList());

                if (datos.Count != 0)
                {
                    for (int i = 0; i < datos.Count; i++)
                    {
                        if (datos[i].strTipRen == "1" && datos[i].intNumDif == 0 && datos[i].intNumGar == 0)
                        {
                            SolicitudesCotizacion dato = new SolicitudesCotizacion();
                            var parameters = new List<SqlParameter>();
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.VarChar, "CONSISCO", ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumOpera", SqlDbType.VarChar, datos[i].intNumOpe.ToString(), ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCuspp", SqlDbType.VarChar, datos[i].strCussp, ParameterDirection.Input));

                            dato =  VCEDBContext<SolicitudesCotizacion>.CallStoreProcedure(StoredProcedures.CO_ConsultasSISCO, parameters, x => new SolicitudesCotizacion
                            {
                                SISok = x.GetInt16(0),
                                pensionSis = Convert.ToDouble(x.GetDecimal(1)),
                                tasaSis = Convert.ToDouble(x.GetDecimal(2)),
                                SISVSok = x.GetInt16(3),
                                MtoCIc = Convert.ToDouble(x.GetDecimal(4))
                            }).FirstOrDefault();

                            _log.Info("Datos SISCO " + dato);

                            if (dato != null)
                            {
                                if (dato.SISok == 0)
                                {
                                    querys += "\nUPDATE PT_TMAE_DETCOTIZACION SET MTO_PENSION = " + dato.pensionSis.ToString("0.00") +
                                                   ", IND_SISCO = 1, IND_FILTROCOTIZA = 'S', PRC_TASAVTA = " + dato.tasaSis.ToString("0.00") + " WHERE NUM_ARCHIVO = " + numArch + " AND NUM_OPERACION = " + datos[i].intNumOpe + " AND NUM_CORRELATIVO = " + datos[i].intCor;
                                }
                                else
                                {
                                    if (dato.SISVSok == 1)
                                    {
                                        if(datos[i].codtipreajuste == "1")
                                        {
                                            if(dato.MtoCIc <= 100000)
                                            {
                                                querys += "\nUPDATE PT_TMAE_DETCOTIZACION SET MTO_PENSION = " + dato.pensionSis.ToString("0.00") +
                                                            ", IND_SISCO = 1, IND_FILTROCOTIZA = 'S', PRC_TASAVTA = " + dato.tasaSis.ToString("0.00") + " WHERE NUM_ARCHIVO = " + numArch + " AND NUM_OPERACION = " + datos[i].intNumOpe + " AND NUM_CORRELATIVO = " + datos[i].intCor;
                                            }
                                        }
                                        else
                                        {
                                            if (dato.MtoCIc <= 30000)
                                            {
                                                querys += "\nUPDATE PT_TMAE_DETCOTIZACION SET MTO_PENSION = " + dato.pensionSis.ToString("0.00") +
                                                            ", IND_SISCO = 1, IND_FILTROCOTIZA = 'S', PRC_TASAVTA = " + dato.tasaSis.ToString("0.00") + " WHERE NUM_ARCHIVO = " + numArch + " AND NUM_OPERACION = " + datos[i].intNumOpe + " AND NUM_CORRELATIVO = " + datos[i].intCor;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //querys += "\nUPDATE PT_TMAE_DETCOTIZACION SET MTO_PENSION = " + dato.pensionSis.ToString("0.00") +
                                        //            ", IND_SISCO = 0, IND_FILTROCOTIZA = 'N', COD_RECHAZO=0, PRC_TASAVTA = " + dato.tasaSis.ToString("0.00") + " WHERE NUM_ARCHIVO = " + numArch + " AND NUM_OPERACION = " + datos[i].intNumOpe + " AND NUM_CORRELATIVO = " + datos[i].intCor;
                                        querys += "\nUPDATE PT_TMAE_DETCOTIZACION SET IND_SISCO = 0 WHERE NUM_ARCHIVO = " + numArch + " AND NUM_OPERACION = " + datos[i].intNumOpe + " AND NUM_CORRELATIVO = " + datos[i].intCor;
                                    }
                                    //querys += "\nUPDATE PT_TMAE_DETCOTIZACION SET IND_SISCO = 0 WHERE NUM_ARCHIVO = " + numArch + " AND NUM_OPERACION = " + datos[i].intNumOpe + " AND NUM_CORRELATIVO = " + datos[i].intCor;
                                }
                            }
                            else
                            {
                                querys += "\nUPDATE PT_TMAE_DETCOTIZACION SET IND_SISCO = 0 WHERE NUM_ARCHIVO = " + numArch + " AND NUM_OPERACION = " + datos[i].intNumOpe + " AND NUM_CORRELATIVO = " + datos[i].intCor;
                            }
                        }
                        else
                        {
                            querys += "\nUPDATE PT_TMAE_DETCOTIZACION SET IND_SISCO = 0 WHERE NUM_ARCHIVO = " + numArch + " AND NUM_OPERACION = " + datos[i].intNumOpe + " AND NUM_CORRELATIVO = " + datos[i].intCor;
                        }
                    }
                }
                _log.Info("Query SISCO " + querys);
                return querys;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string validacionesSiNoCotiza(int numArch)
        {
            string querys = "";
            try
            {
                List<SolicitudesCotizacion> datos = new List<SolicitudesCotizacion>();
                XmlConfigurator.Configure();
                _log.Info("VALIDACIONES SI COTIZA NO COTIZA");

                string query = "SELECT C.NUM_OPERACION, C.COD_CUSPP, DC.NUM_CORRELATIVO, MTO_PRIUNIFON, M.COD_SCOMP, C.COD_TIPPENSION, DC.COD_TIPREN, DC.COD_MODALIDAD " +
                              "FROM PT_TMAE_COTIZACION C " +
                              "INNER JOIN PT_TMAE_DETCOTIZACION DC ON C.NUM_ARCHIVO = DC.NUM_ARCHIVO AND C.NUM_OPERACION = DC.NUM_OPERACION " +
                              "INNER JOIN MA_TPAR_MONEDATIPOREAJU M ON DC.COD_TIPREAJUSTE = M.COD_TIPREAJUSTE and DC.COD_MONEDA = M.COD_MONEDA " +
                              "where C.NUM_ARCHIVO = '" + numArch + "' and IND_SISCO = 0";

                datos = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                    intNumOpe = Convert.ToInt32(x.GetDecimal(0)),
                    strCussp = x.GetString(1),
                    intCor = x.GetInt32(2),
                    strMtoCIC = Convert.ToString(x.GetDecimal(3)),
                    strMon = x.GetString(4),
                    strTipPen = x.GetString(5),
                    strTipRen = x.GetString(6),
                    strCodMod = x.GetString(7)
                }).ToList();
                
                _log.Info("Datos consulta " + datos);
                if (datos.Count != 0)
                {
                    List<MantenedorFiltros> info = _mantenedorFiltros.ConsultaDatosFiltros();
                    List<MantenedorFiltros> monedas = _mantenedorFiltros.ConsultaMonedasFiltros();
                    List<MantenedorFiltros> montos = _mantenedorFiltros.ConsultaMontosFiltros();
                    
                    _log.Info("Consultas realizadas correctamente");

                    for (int i = 0; i < datos.Count; i++)
                    {

                        List<MantenedorFiltros> listaInfoRenta = new List<MantenedorFiltros>();
                        List<MantenedorFiltros> listaInfoMod = new List<MantenedorFiltros>();
                        List<MantenedorFiltros> listaInfoSexo = new List<MantenedorFiltros>();
                        List<MantenedorFiltros> listaInfoEstadoC = new List<MantenedorFiltros>();
                        List<MantenedorFiltros> listaInfoCliente = new List<MantenedorFiltros>();

                        List<MantenedorFiltros> listaMonedas = new List<MantenedorFiltros>();
                        List<MantenedorFiltros> listaMontos = new List<MantenedorFiltros>();

                        listaInfoRenta = (from lisInf in info where (lisInf.codigoTabla == "TR" && lisInf.codigoPension == datos[i].strTipPen) select lisInf).ToList();
                        listaInfoMod = (from lisInf in info where (lisInf.codigoTabla == "AL" && lisInf.codigoPension == datos[i].strTipPen) select lisInf).ToList();
                        //listaInfoSexo = (from lisInf in info where (lisInf.codigoTabla == "SE" && lisInf.codigoPension == datos[i].strTipPen) select lisInf).ToList();
                        //listaInfoEstadoC = (from lisInf in info where (lisInf.codigoTabla == "EC" && lisInf.codigoPension == datos[i].strTipPen) select lisInf).ToList();
                        //listaInfoCliente = (from lisInf in info where (lisInf.codigoTabla == "CL" && lisInf.codigoPension == datos[i].strTipPen) select lisInf).ToList();

                        listaMonedas = (from lisMonedas in monedas where (lisMonedas.codigoPension == datos[i].strTipPen) select lisMonedas).ToList();
                        listaMontos = (from lisMontos in montos where (lisMontos.codigoPension == datos[i].strTipPen) select lisMontos).ToList();

                        int InfoRentErr = (from l in listaInfoRenta where l.codigoElemento == datos[i].strTipRen select l).Count();
                        int InfoModErr = (from l in listaInfoMod where l.codigoElemento == datos[i].strCodMod select l).Count();
                        //int InfoSexErr = (from l in listaInfoRenta where l.codigoElemento == datos[i].strSexo select l).Count();
                        //int InfoEstadoCErr = (from l in listaInfoRenta where l.codigoElemento == datos[i]. select l).Count();
                        //int InfoClienteErr = (from l in listaInfoRenta where l.codigoElemento == datos[i].strTipRen select l).Count();
                        int InfoMonedaErr = (from l in listaMonedas where l.codigoElemento == datos[i].strMon select l).Count();
                        int InfoMontosErr = 0;
                        //RRR 13.12.2024
                        if (datos[i].strMon == "S/.")
                        {
                            InfoMontosErr = (from l in listaMontos where 100000 <= Convert.ToDouble(datos[i].strMtoCIC) && l.mtoPriHasta >= Convert.ToDouble(datos[i].strMtoCIC) select l).Count();
                        }
                        else
                        {
                            InfoMontosErr = (from l in listaMontos where l.mtoPriDesde <= Convert.ToDouble(datos[i].strMtoCIC) && l.mtoPriHasta >= Convert.ToDouble(datos[i].strMtoCIC) select l).Count();

                        }
                        //fin RRR 13.12.2024
                        _log.Info("Restricciones realizadas correctamente");


                        string mensaj = "";
                        if (InfoRentErr == 0) {
                             mensaj = "No fue cotizada, el tipo de renta no es valido";
                        }
                        else if (InfoModErr == 0)
                        {
                            mensaj = "No fue cotizada, el tipo de modalidad no es valido";
                        }
                        else if (InfoMonedaErr == 0)
                        {
                            mensaj = "No fue cotizada, el tipo de moneda no es valida";
                        }
                        else if (InfoMontosErr == 0)
                        {
                            mensaj = "No fue cotizada, el monto del Capital no es valido";
                        }


                        if (InfoRentErr != 0 && InfoModErr != 0 && InfoMonedaErr != 0 && InfoMontosErr != 0)
                        {
                            querys += "\nUPDATE PT_TMAE_DETCOTIZACION SET " +
                                      "IND_FILTROCOTIZA = 'S' WHERE NUM_ARCHIVO = " + numArch + " AND NUM_OPERACION = " + datos[i].intNumOpe + " AND NUM_CORRELATIVO = " + datos[i].intCor;
                        }
                        else
                        {
                            querys += "\nUPDATE PT_TMAE_DETCOTIZACION SET " +
                                      "IND_FILTROCOTIZA = 'N', COD_RECHAZO = '902', ERR_DESCRIP = '" + mensaj + "' WHERE NUM_ARCHIVO = " + numArch + " AND NUM_OPERACION = " + datos[i].intNumOpe + " AND NUM_CORRELATIVO = " + datos[i].intCor;
                        }

                        
                    }
                }

                _log.Info("Query que se ejecutara" + querys);
                return querys;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EjecutarScript(string script)
        {
            try
            {
                _log.Info("Se ejecutara el Script" + script);
                SRVDBContext<DataTable>.CallSelectStatementDt(script, x => new DataTable());
                _log.Info("Se ejecuto correctamente el Script" + script);
            }
            catch (Exception ex)
            {
                _log.Info("Error al ejecutar el Script" + ex);
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
