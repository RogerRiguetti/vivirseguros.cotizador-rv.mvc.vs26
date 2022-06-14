using System;
using System.Collections.Generic;
using System.Linq;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using System.Data.SqlClient;
using System.Data;
using log4net;
using System.Reflection;
using log4net.Config;

namespace Estudio.Repository.Persistence.Repositories
{
    public class ReservasRepository
    {
        //Globales
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        DataTable PR_TMAE_POLIZA1 = new DataTable();
        DataTable PR_TMAE_POLIZA2 = new DataTable();
        DataTable PR_TMAE_CALPOL1 = new DataTable();
        DataTable PR_TMAE_CALPOL2 = new DataTable();
        DataTable PR_TMAE_BENEFICIARIO1 = new DataTable();
        DataTable PR_TMAE_BENEFICIARIO2 = new DataTable();
        DataTable PR_TMAE_CALBEN1 = new DataTable();
        DataTable PR_TMAE_CALBEN2 = new DataTable();
        DataTable PR_TTMP_TASASRES = new DataTable();

        /// <summary>
        /// Buscar y validar periodo de reservas a procesar para comprobar que se enceuentra abierto.
        /// José Hernández Alvarado.
        /// 11-12-2019
        /// </summary>
        /// <param name="pFecha">Fecha de periodo en pantalla de Reservas a procesar.</param>
        /// <returns>Mensaje de confirmación de periodo Abierto o mensaje erroneo.</returns>
        public Response PeriodoRepository(string pFecha)
        {
            Response respuesta = new Response();
            try
            {
                var Dato = new Reservas();
                string query = "SELECT FEC_CALCULO, COD_ESTPERIODO FROM PR_TMAE_PROCALDEF";
                query = query + " WHERE COD_CLIENTE = 1 AND FEC_CALCULO = '" + pFecha + "'";

                Dato = SRVDBContext<Reservas>.CallSelectStatement(query, x => new Reservas
                {
                    Cod_ESTPERIODO = x.GetString(1)
                }).FirstOrDefault();

                if (Dato != null)
                {
                    if (Dato.Cod_ESTPERIODO == "A")
                    {
                        respuesta.IsOk = true;
                        respuesta.Message = "Abierto";
                    }
                    else
                    {
                        respuesta.IsOk = false;
                        respuesta.Message = "El Periodo Con la Fecha Ingresada Esta Cerrado.\n Operacion Cancelada.";
                    }
                }
                else
                {
                    respuesta.IsOk = false;
                    respuesta.Message = "No Se a Encontrado Ningun Periodo Con la Fecha Ingresada.\n Operacion Cancelada.";
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.IsOk = false;
                respuesta.Message = "Error al ejecutar query para búsqueda de periodo abierto: " + ex.Message;
                return respuesta;
            }
        }

        /// <summary>
        /// Cosultar periodo abierto en Base de Datos.
        /// José Hernández Alvarado.
        /// 11-12-2019
        /// </summary>
        /// <param name="pQuery">Query de consulta para el periodo abierto.</param>
        /// <returns>Fecha de periodo abierto.</returns>
        public Reservas ConsultarPeriodoAbierto(string pQuery)
        {
            return SRVDBContext<Reservas>.CallSelectStatement(pQuery, x => new Reservas
            {
                Cod_ESTPERIODO = x.GetString(0)
            }).FirstOrDefault();
        }

        /// <summary>
        /// Generar tablas de información para el Proceso de Migración de Reservas e inserción de las mismas.
        /// José Hernández Alvarado / Jesús Solis / Omar Figueroa
        /// 28-10-2019
        /// </summary>
        /// <param name="fecha">Fecha de periodo abierto.</param>
        /// <param name="usuario">Nombre de usuario en sistema.</param>
        /// <param name="TasasPolPar">Lsita de pólizas en tabla de Tasas de Reservas.</param>
        /// <param name="TasasPolNum">Total de pólizas existentes en tabla de Tasas de Reservas.</param>
        /// <returns>Objeto de respuesta erronea o exitosa.</returns>
        public Response IniCal(string fecha, string usuario, int TasasPolNum, List<Reservas> pLstPolizasTasasRes)
        {
            XmlConfigurator.Configure();
            Response res2 = new Response();

            #region DataTables para Proceso de Migración
            #region PR_TMAE_POLIZA1
            PR_TMAE_POLIZA1.Columns.Add("COD_CLIENTE", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_RESERVA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_PLAN", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_COBERTURA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_ESTADO", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_TIPREN", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_MODALIDAD", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("NUM_CARGAS", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("FEC_VIGENCIA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("MTO_PRIMA", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("MTO_PENSION", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("NUM_MESDIF", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("NUM_MESGAR", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASACE", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASAVTA", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("COD_CIAREA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_OPEREA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_MODREA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("FEC_REA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("FEC_INIREA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("FEC_FINREA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASARET", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASACRE", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("COD_USUARIOCREA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("FEC_CREA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("HOR_CREA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_USUARIOMODI", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("FEC_MODI", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("HOR_MODI", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_BAUTIZO", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_BAUTIZOREA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASACEDEF", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASAVTADEF", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASABASEDEF", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("MTO_RMPOL", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("MTO_RMBASE", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("NUM_BENPROBAU", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASACREDEF", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASABASEREADEF", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("MTO_RMPOLREA", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("MTO_RMBASEREA", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("NUM_MESNOC", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("NUM_BENPROBAUREA", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("NUM_ENDOSO", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("COD_BAUFIN", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASACEF", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("MTO_RESCEF", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("NUM_BENPROBAUFIN", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("FEC_DEV", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("FEC_PAGPRI", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("FEC_COT", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_TEM", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_CIAORIGEN", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_CUSPP", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_COBCIA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_INDCALCE", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_INDRAMO", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("NUM_POLIZAREF", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("PRC_FACTOR", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("MTO_GTOSEP", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("COD_MONEDA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("IND_COB", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_COBERCON", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("MTO_FACPENELLA", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("PRC_FACPENELLA", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("COD_DERCRE", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_DERGRA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_TIPREAJUSTE", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("MTO_VALREAJUSTETRI", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("MTO_VALREAJUSTEMEN", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("NUM_MESESC", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("PRC_RENTAESC", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("FEC_FINPERESC", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("MTO_PENSIONINI", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("TOPE_18_AÑOS", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("COD_ESTUDIANTE", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("NUM_CASO_ESPECIAL", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("FEC_DEVSOL", typeof(string));
            
            
            #endregion

            #region PR_TMAE_POLIZA2
            PR_TMAE_POLIZA2.Columns.Add("COD_CLIENTE", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_RESERVA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_PLAN", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_COBERTURA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_ESTADO", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_TIPREN", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_MODALIDAD", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("NUM_CARGAS", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("FEC_VIGENCIA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("MTO_PRIMA", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("MTO_PENSION", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("NUM_MESDIF", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("NUM_MESGAR", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASACE", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASAVTA", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("COD_CIAREA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_OPEREA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_MODREA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("FEC_REA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("FEC_INIREA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("FEC_FINREA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASARET", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASACRE", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("COD_USUARIOCREA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("FEC_CREA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("HOR_CREA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_USUARIOMODI", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("FEC_MODI", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("HOR_MODI", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_BAUTIZO", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_BAUTIZOREA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASACEDEF", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASAVTADEF", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASABASEDEF", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("MTO_RMPOL", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("MTO_RMBASE", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("NUM_BENPROBAU", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASACREDEF", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASABASEREADEF", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("MTO_RMPOLREA", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("MTO_RMBASEREA", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("NUM_MESNOC", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("NUM_BENPROBAUREA", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("NUM_ENDOSO", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("COD_BAUFIN", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASACEF", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("MTO_RESCEF", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("NUM_BENPROBAUFIN", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("FEC_DEV", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("FEC_PAGPRI", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("FEC_COT", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_TEM", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_CIAORIGEN", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_CUSPP", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_COBCIA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_INDCALCE", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_INDRAMO", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("NUM_POLIZAREF", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("PRC_FACTOR", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("MTO_GTOSEP", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("COD_MONEDA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("IND_COB", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_COBERCON", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("MTO_FACPENELLA", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("PRC_FACPENELLA", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("COD_DERCRE", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_DERGRA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_TIPREAJUSTE", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("MTO_VALREAJUSTETRI", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("MTO_VALREAJUSTEMEN", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("NUM_MESESC", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("PRC_RENTAESC", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("FEC_FINPERESC", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("MTO_PENSIONINI", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("TOPE_18_AÑOS", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("COD_ESTUDIANTE", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("NUM_CASO_ESPECIAL", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("FEC_DEVSOL", typeof(string));
            #endregion

            #region PR_TMAE_CALPOL1
            PR_TMAE_CALPOL1.Columns.Add("COD_CLIENTE", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("NUM_CARGAS", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("COD_FIJA", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("COD_FIN", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("COD_TASAVTA", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESBAS", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESBASRET", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESBASAJU", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("NUM_POLBASCED", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_POLBASRET", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENPROBAS", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENBASCED", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENBASRET", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESFIJ", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESFIJRET", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESFIJAJU", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("NUM_POLFIJCED", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_POLFIJRET", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENPROFIJ", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENFIJCED", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENFIJRET", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESFIN", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESFINRET", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESFINAJU", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("NUM_POLFINCED", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_POLFINRET", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENPROFIN", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENFINCED", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENFINRET", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESTASA", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESTASARET", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESTASAAJU", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("NUM_POLTASACED", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_POLTASARET", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENPROTASA", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENTASACED", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("NUM_BENTASARET", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("PRC_TASAMER", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("COD_USUARIOCREA", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("FEC_CREA", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("HOR_CREA", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("COD_USUARIOMODI", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("FEC_MODI", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("HOR_MODI", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("NUM_ENDOSO", typeof(int));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESBAS_METAN", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESBASRET_METAN", typeof(decimal));
            #endregion

            #region PR_TMAE_CALPOL2
            PR_TMAE_CALPOL2.Columns.Add("COD_CLIENTE", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("NUM_CARGAS", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("COD_FIJA", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("COD_FIN", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("COD_TASAVTA", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESBAS", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESBASRET", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESBASAJU", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("NUM_POLBASCED", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_POLBASRET", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENPROBAS", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENBASCED", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENBASRET", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESFIJ", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESFIJRET", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESFIJAJU", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("NUM_POLFIJCED", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_POLFIJRET", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENPROFIJ", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENFIJCED", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENFIJRET", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESFIN", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESFINRET", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESFINAJU", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("NUM_POLFINCED", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_POLFINRET", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENPROFIN", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENFINCED", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENFINRET", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESTASA", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESTASARET", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESTASAAJU", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("NUM_POLTASACED", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_POLTASARET", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENPROTASA", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENTASACED", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("NUM_BENTASARET", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("PRC_TASAMER", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("COD_USUARIOCREA", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("FEC_CREA", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("HOR_CREA", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("COD_USUARIOMODI", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("FEC_MODI", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("HOR_MODI", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("NUM_ENDOSO", typeof(int));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESBAS_METAN", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESBASRET_METAN", typeof(decimal));
            #endregion

            #region PR_TMAE_BENEFICIARIO1
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_CLIENTE", typeof(int));
            PR_TMAE_BENEFICIARIO1.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("NUM_ORDEN", typeof(int));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_SEXO", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_PAR", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_SITINV", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_DERPEN", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_GRUFAM", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_DERCRE", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("FEC_NACBEN", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("FEC_FALBEN", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("FEC_NACHM", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_MOTREQPEN", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("FEC_INVBEN", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_CAUINV", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("MTO_PENSION", typeof(decimal));
            PR_TMAE_BENEFICIARIO1.Columns.Add("MTO_PENSIONGAR", typeof(decimal));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_USUARIOCREA", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("FEC_CREA", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("HOR_CREA", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_USUARIOMODI", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("FEC_MODI", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("HOR_MODI", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("NUM_ENDOSO", typeof(int));
            PR_TMAE_BENEFICIARIO1.Columns.Add("PRC_PENSION", typeof(decimal));
            PR_TMAE_BENEFICIARIO1.Columns.Add("PRC_PENSIONLEG", typeof(decimal));
            PR_TMAE_BENEFICIARIO1.Columns.Add("PRC_PENSIONGAR", typeof(decimal));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_TOPE_18", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_ESTUDIANTE", typeof(string));
            #endregion

            #region PR_TMAE_BENEFICIARIO2
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_CLIENTE", typeof(int));
            PR_TMAE_BENEFICIARIO2.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("NUM_ORDEN", typeof(int));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_SEXO", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_PAR", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_SITINV", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_DERPEN", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_GRUFAM", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_DERCRE", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("FEC_NACBEN", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("FEC_FALBEN", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("FEC_NACHM", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_MOTREQPEN", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("FEC_INVBEN", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_CAUINV", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("MTO_PENSION", typeof(decimal));
            PR_TMAE_BENEFICIARIO2.Columns.Add("MTO_PENSIONGAR", typeof(decimal));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_USUARIOCREA", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("FEC_CREA", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("HOR_CREA", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_USUARIOMODI", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("FEC_MODI", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("HOR_MODI", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("NUM_ENDOSO", typeof(int));
            PR_TMAE_BENEFICIARIO2.Columns.Add("PRC_PENSION", typeof(decimal));
            PR_TMAE_BENEFICIARIO2.Columns.Add("PRC_PENSIONLEG", typeof(decimal));
            PR_TMAE_BENEFICIARIO2.Columns.Add("PRC_PENSIONGAR", typeof(decimal));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_TOPE_18", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_ESTUDIANTE", typeof(string));
            #endregion

            #region PR_TMAE_CALBEN1
            PR_TMAE_CALBEN1.Columns.Add("COD_CLIENTE", typeof(int));
            PR_TMAE_CALBEN1.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("NUM_ORDEN", typeof(int));
            PR_TMAE_CALBEN1.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("COD_FIJA", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("COD_FIN", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("COD_TASAVTA", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("NUM_EDAD", typeof(int));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNUBAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNABAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNRBAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNTBAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNGBAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("TMO_CNUFIJ", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNAFIJ", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNRFIJ", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNTFIJ", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNGFIJ", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNUFIN", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNAFIN", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNRFIN", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNTFIN", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNGFIN", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNUTASA", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNATASA", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNRTASA", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNTTASA", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNGTASA", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("COD_USUARIOCREA", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("FEC_CREA", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("HOR_CREA", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("COD_USUARIOMODI", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("FEC_MODI", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("HOR_MODI", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("NUM_ENDOSO", typeof(int));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNUBAS_METAN", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNABAS_METAN", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNGBAS_METAN", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNTBAS_METAN", typeof(decimal));
            #endregion

            #region PR_TMAE_CALBEN2
            PR_TMAE_CALBEN2.Columns.Add("COD_CLIENTE", typeof(int));
            PR_TMAE_CALBEN2.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("NUM_ORDEN", typeof(int));
            PR_TMAE_CALBEN2.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("COD_FIJA", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("COD_FIN", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("COD_TASAVTA", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("NUM_EDAD", typeof(int));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNUBAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNABAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNRBAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNTBAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNGBAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("TMO_CNUFIJ", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNAFIJ", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNRFIJ", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNTFIJ", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNGFIJ", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNUFIN", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNAFIN", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNRFIN", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNTFIN", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNGFIN", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNUTASA", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNATASA", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNRTASA", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNTTASA", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNGTASA", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("COD_USUARIOCREA", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("FEC_CREA", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("HOR_CREA", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("COD_USUARIOMODI", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("FEC_MODI", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("HOR_MODI", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("NUM_ENDOSO", typeof(int));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNUBAS_METAN", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNABAS_METAN", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNGBAS_METAN", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNTBAS_METAN", typeof(decimal));
            #endregion

            #region PR_TTMP_TASASRES
            PR_TTMP_TASASRES.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TTMP_TASASRES.Columns.Add("PRC_TASLIBRERI", typeof(decimal));
            PR_TTMP_TASASRES.Columns.Add("PRC_TASVTAPROM", typeof(decimal));
            #endregion
            #endregion

            try
            {
                List<Reservas> Datos = new List<Reservas>();
                List<Reservas> DatosBen = new List<Reservas>();
                var dat = new Reservas();
                var dat7 = new Reservas();
                var dat8 = new Reservas();
                string query = "";
                string strConexionSeguroRV = cadena_conexion();

                #region Proceso e inserción de Pólizas(PR_TMAE_POLIZA, PR_TMAE_CALPOL, PR_TTMP_TASASRES)
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_MTOS", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFAnioMes", SqlDbType.VarChar, fecha.Substring(0, 6), ParameterDirection.Input));

                dat = SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    PrcCastigo = x.GetDecimal(0),
                    TopeMax = x.GetDecimal(1),
                }).FirstOrDefault();

                bool RealizarProcesoCalQuiebra = false;

                if (dat != null)
                {
                    if (dat.PrcCastigo != 0 && dat.TopeMax != 0)
                    {
                        RealizarProcesoCalQuiebra = true;
                    }
                }

                bool CargarPolizasBeneficiarios = false;

                int NumeroPolizas = 0;
                int NumeroBeneficiarios = 0;

                parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_DATOSPB", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, fecha, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFAnioMes", SqlDbType.VarChar, fecha.Substring(0, 6), ParameterDirection.Input));

                Datos = SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    Num_Poliza = x.GetString(0),
                    Num_Endoso = x.GetInt32(1),
                    Cod_Tippension = x.GetString(2),
                    Cod_Estado = x.GetString(3),
                    Cod_Tipren = x.GetString(4),
                    Cod_Modalidad = x.GetString(5),
                    Fec_Vigencia = x.GetString(6),
                    Mto_Prima = x.GetDecimal(7),
                    Mto_Pension = x.GetDecimal(8),
                    Num_Mesdif = x.GetInt32(9),
                    Num_Mesgar = x.GetInt32(10),
                    Prc_tasace = x.GetDecimal(11),
                    Prc_tasvta = x.GetDecimal(12),
                    Num_Cargas = x.GetInt32(13),
                    FecDev = x.GetString(14),
                    Cod_Cuspp = x.GetString(15),
                    Cod_Moneda = x.GetString(16),
                    Ind_Cob = x.GetString(17),
                    Cod_Cobercon = x.GetString(18),
                    Mto_Facpenella = x.GetDecimal(19),
                    Prc_Facpenella = x.GetDecimal(20),
                    Cod_Dercre = x.GetString(21),
                    Cod_Dergra = x.GetString(22),
                    Cod_Tipreajuste = x.GetString(23),
                    Mto_Valreajustetri = x.GetDecimal(24),
                    Mto_Valreajustemen = x.GetDecimal(25),
                    Num_Mesesc = x.GetInt32(26),
                    Prc_Rentaesc = x.GetDecimal(27),
                    mto_pensionact = x.GetDecimal(28),
                    FecPagPri = x.GetString(29),
                    FecCot = x.GetString(30),
                    MtoGtoSep = x.GetDecimal(31),
                    CobCia = x.GetString(32),
                    SwFecDev = x.GetDecimal(33) != 0 ? true : false,
                    IpcDevengue = x.GetDecimal(33),
                    Prc_TasBaseDef = (double)x.GetDecimal(34),
                    Num_Caso_Especial = x.GetInt32(35)
                }).ToList();


                parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_IPCCOTIZACION", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFAnioMes", SqlDbType.VarChar, fecha.Substring(0, 6), ParameterDirection.Input));

                dat7 = SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    IpcCotizacion = x.GetDecimal(0)
                }).FirstOrDefault();

                bool SwFecCot = false;

                if (dat7 != null && dat7.IpcCotizacion != 0)
                {
                    SwFecCot = true;
                }

                parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_CODCLIENTE", ParameterDirection.Input));

                dat8 = SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    Cod_cliente = x.GetInt32(0)
                }).FirstOrDefault();
                GC.Collect();
                Console.WriteLine("Memory used after full collection:   {0:N0}",
                GC.GetTotalMemory(true));

                parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_BACKTABLAS", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@vCod_Cliente", SqlDbType.VarChar, dat8.Cod_cliente, ParameterDirection.Input));
                dat = SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                }).FirstOrDefault();

                GC.Collect();
                Console.WriteLine("Memory used after full collection:   {0:N0}",
                GC.GetTotalMemory(true));
                for (var i = 0; i < Datos.Count; i++)
                {
                    DataRow rowPoliza1 = PR_TMAE_POLIZA1.NewRow();
                    DataRow rowPoliza2 = PR_TMAE_POLIZA2.NewRow();
                    DataRow rowCalPol1 = PR_TMAE_CALPOL1.NewRow();
                    DataRow rowCalPol2 = PR_TMAE_CALPOL2.NewRow();
                    DataRow rowTasasRes = PR_TTMP_TASASRES.NewRow();

                    decimal Prc_Factor = 0;

                    if (RealizarProcesoCalQuiebra == true)
                    {
                        int QuiebraPensionRef = 0;
                        bool QuiebraExistePenRef = true;
                        //dat.mto_pensionact = 0;
                    }

                    bool ObtenerFactorAjusteIPC = false;
                    decimal FactorAjusteIPC = 0;

                    if (Datos[i].SwFecDev == true && SwFecCot == true)
                    {
                        if (Datos[i].IpcDevengue != 0)
                        {
                            var AuxFactorAjusteIPC = String.Format("{0:0.00000000}", (dat7.IpcCotizacion / Datos[i].IpcDevengue));
                            FactorAjusteIPC = Convert.ToDecimal(AuxFactorAjusteIPC);
                        }
                        else
                        {
                            FactorAjusteIPC = 0;
                        }
                        ObtenerFactorAjusteIPC = true;
                    }

                    if (ObtenerFactorAjusteIPC == true)
                    {
                        Prc_Factor = FactorAjusteIPC;
                    }

                    if (Datos[i].Cod_Moneda == "NS")
                    {
                        #region Llenado de tabla PR_TMAE_POLIZA1
                        rowPoliza1["COD_CLIENTE"] = dat8.Cod_cliente;
                        rowPoliza1["NUM_POLIZA"] = Datos[i].Num_Poliza;
                        rowPoliza1["COD_RESERVA"] = "N";
                        rowPoliza1["COD_PLAN"] = Datos[i].Cod_Tippension;
                        rowPoliza1["COD_COBERTURA"] = DBNull.Value;
                        rowPoliza1["COD_ESTADO"] = Datos[i].Cod_Estado;
                        rowPoliza1["COD_TIPREN"] = Datos[i].Cod_Tipren;
                        rowPoliza1["COD_MODALIDAD"] = Datos[i].Cod_Modalidad;
                        rowPoliza1["NUM_CARGAS"] = Datos[i].Num_Cargas;
                        rowPoliza1["FEC_VIGENCIA"] = Datos[i].Fec_Vigencia;
                        rowPoliza1["MTO_PRIMA"] = Datos[i].Mto_Prima;
                        rowPoliza1["MTO_PENSION"] = Datos[i].mto_pensionact;
                        rowPoliza1["NUM_MESDIF"] = Datos[i].Num_Mesdif;
                        rowPoliza1["NUM_MESGAR"] = Datos[i].Num_Mesgar;
                        rowPoliza1["PRC_TASACE"] = Datos[i].Prc_tasace;
                        rowPoliza1["PRC_TASAVTA"] = Datos[i].Prc_tasvta;
                        rowPoliza1["COD_CIAREA"] = "00";
                        rowPoliza1["COD_OPEREA"] = DBNull.Value;
                        rowPoliza1["COD_MODREA"] = DBNull.Value;
                        rowPoliza1["FEC_REA"] = DBNull.Value;
                        rowPoliza1["FEC_INIREA"] = DBNull.Value;
                        rowPoliza1["FEC_FINREA"] = DBNull.Value;
                        rowPoliza1["PRC_TASARET"] = 0;
                        rowPoliza1["PRC_TASACRE"] = 0;
                        rowPoliza1["COD_USUARIOCREA"] = usuario;
                        rowPoliza1["FEC_CREA"] = DateTime.Today.ToString("yyyyMMdd");
                        rowPoliza1["HOR_CREA"] = DateTime.Now.ToString("HHmmss");
                        rowPoliza1["COD_USUARIOMODI"] = DBNull.Value;
                        rowPoliza1["FEC_MODI"] = DBNull.Value;
                        rowPoliza1["HOR_MODI"] = DBNull.Value;
                        rowPoliza1["COD_BAUTIZO"] = DBNull.Value;
                        rowPoliza1["COD_BAUTIZOREA"] = DBNull.Value;
                        rowPoliza1["PRC_TASACEDEF"] = DBNull.Value;
                        rowPoliza1["PRC_TASAVTADEF"] = DBNull.Value;
                        rowPoliza1["PRC_TASABASEDEF"] = Datos[i].Prc_TasBaseDef;
                        rowPoliza1["MTO_RMPOL"] = DBNull.Value;
                        rowPoliza1["MTO_RMBASE"] = DBNull.Value;
                        rowPoliza1["NUM_BENPROBAU"] = DBNull.Value;
                        rowPoliza1["PRC_TASACREDEF"] = DBNull.Value;
                        rowPoliza1["PRC_TASABASEREADEF"] = DBNull.Value;
                        rowPoliza1["MTO_RMPOLREA"] = DBNull.Value;
                        rowPoliza1["MTO_RMBASEREA"] = DBNull.Value;
                        rowPoliza1["NUM_MESNOC"] = DBNull.Value;
                        rowPoliza1["NUM_BENPROBAUREA"] = DBNull.Value;
                        rowPoliza1["NUM_ENDOSO"] = Datos[i].Num_Endoso;
                        rowPoliza1["COD_BAUFIN"] = DBNull.Value;
                        rowPoliza1["PRC_TASACEF"] = DBNull.Value;
                        rowPoliza1["MTO_RESCEF"] = DBNull.Value;
                        rowPoliza1["NUM_BENPROBAUFIN"] = DBNull.Value;
                        rowPoliza1["FEC_DEV"] = Datos[i].FecDev;
                        rowPoliza1["FEC_PAGPRI"] = Datos[i].FecPagPri;
                        rowPoliza1["FEC_COT"] = Datos[i].FecCot;
                        rowPoliza1["COD_TEM"] = "N";
                        rowPoliza1["COD_CIAORIGEN"] = "01";
                        rowPoliza1["COD_CUSPP"] = Datos[i].Cod_Cuspp;
                        rowPoliza1["COD_COBCIA"] = Datos[i].CobCia;
                        rowPoliza1["COD_INDCALCE"] = "SI";
                        rowPoliza1["COD_INDRAMO"] = "09";
                        rowPoliza1["NUM_POLIZAREF"] = Datos[i].Num_Poliza;
                        rowPoliza1["PRC_FACTOR"] = Prc_Factor;
                        rowPoliza1["MTO_GTOSEP"] = Datos[i].MtoGtoSep;
                        rowPoliza1["COD_MONEDA"] = Datos[i].Cod_Moneda;
                        rowPoliza1["IND_COB"] = Datos[i].Ind_Cob;
                        rowPoliza1["COD_COBERCON"] = Datos[i].Cod_Cobercon;
                        rowPoliza1["MTO_FACPENELLA"] = Datos[i].Mto_Facpenella;
                        rowPoliza1["PRC_FACPENELLA"] = Datos[i].Prc_Facpenella;
                        rowPoliza1["COD_DERCRE"] = Datos[i].Cod_Dercre;
                        rowPoliza1["COD_DERGRA"] = Datos[i].Cod_Dergra;
                        rowPoliza1["COD_TIPREAJUSTE"] = Datos[i].Cod_Tipreajuste;
                        rowPoliza1["MTO_VALREAJUSTETRI"] = Datos[i].Mto_Valreajustetri;
                        rowPoliza1["MTO_VALREAJUSTEMEN"] = Datos[i].Mto_Valreajustemen;
                        rowPoliza1["NUM_MESESC"] = Datos[i].Num_Mesesc;
                        rowPoliza1["PRC_RENTAESC"] = Datos[i].Prc_Rentaesc;
                        rowPoliza1["FEC_FINPERESC"] = DBNull.Value;
                        rowPoliza1["MTO_PENSIONINI"] = Datos[i].Mto_Pension;
                        rowPoliza1["TOPE_18_AÑOS"] = DBNull.Value;
                        rowPoliza1["COD_ESTUDIANTE"] = DBNull.Value;
                        rowPoliza1["NUM_CASO_ESPECIAL"] = Datos[i].Num_Caso_Especial;
                        rowPoliza1["FEC_DEVSOL"] = DBNull.Value;

                        PR_TMAE_POLIZA1.Rows.Add(rowPoliza1);
                        #endregion

                        #region Llenado de tabla PR_TMAE_CALPOL1
                        rowCalPol1["COD_CLIENTE"] = dat8.Cod_cliente;
                        rowCalPol1["NUM_POLIZA"] = Datos[i].Num_Poliza;
                        rowCalPol1["NUM_CARGAS"] = Datos[i].Num_Cargas;
                        rowCalPol1["COD_BASE"] = "N";
                        rowCalPol1["COD_FIJA"] = "N";
                        rowCalPol1["COD_FIN"] = "N";
                        rowCalPol1["COD_TASAVTA"] = "N";
                        rowCalPol1["MTO_RESBAS"] = 0;
                        rowCalPol1["MTO_RESBASRET"] = 0;
                        rowCalPol1["MTO_RESBASAJU"] = 0;
                        rowCalPol1["NUM_POLBASCED"] = 0;
                        rowCalPol1["NUM_POLBASRET"] = 0;
                        rowCalPol1["NUM_BENPROBAS"] = 0;
                        rowCalPol1["NUM_BENBASCED"] = 0;
                        rowCalPol1["NUM_BENBASRET"] = 0;
                        rowCalPol1["MTO_RESFIJ"] = 0;
                        rowCalPol1["MTO_RESFIJRET"] = 0;
                        rowCalPol1["MTO_RESFIJAJU"] = 0;
                        rowCalPol1["NUM_POLFIJCED"] = 0;
                        rowCalPol1["NUM_POLFIJRET"] = 0;
                        rowCalPol1["NUM_BENPROFIJ"] = 0;
                        rowCalPol1["NUM_BENFIJCED"] = 0;
                        rowCalPol1["NUM_BENFIJRET"] = 0;
                        rowCalPol1["MTO_RESFIN"] = 0;
                        rowCalPol1["MTO_RESFINRET"] = 0;
                        rowCalPol1["MTO_RESFINAJU"] = 0;
                        rowCalPol1["NUM_POLFINCED"] = 0;
                        rowCalPol1["NUM_POLFINRET"] = 0;
                        rowCalPol1["NUM_BENPROFIN"] = 0;
                        rowCalPol1["NUM_BENFINCED"] = 0;
                        rowCalPol1["NUM_BENFINRET"] = 0;
                        rowCalPol1["MTO_RESTASA"] = 0;
                        rowCalPol1["MTO_RESTASARET"] = 0;
                        rowCalPol1["MTO_RESTASAAJU"] = 0;
                        rowCalPol1["NUM_POLTASACED"] = 0;
                        rowCalPol1["NUM_POLTASARET"] = 0;
                        rowCalPol1["NUM_BENPROTASA"] = 0;
                        rowCalPol1["NUM_BENTASACED"] = 0;
                        rowCalPol1["NUM_BENTASARET"] = 0;
                        rowCalPol1["PRC_TASAMER"] = 0;
                        rowCalPol1["COD_USUARIOCREA"] = usuario;
                        rowCalPol1["FEC_CREA"] = DateTime.Today.ToString("yyyyMMdd");
                        rowCalPol1["HOR_CREA"] = DateTime.Now.ToString("HHmmss");
                        rowCalPol1["COD_USUARIOMODI"] = DBNull.Value;
                        rowCalPol1["FEC_MODI"] = DBNull.Value;
                        rowCalPol1["HOR_MODI"] = DBNull.Value;
                        rowCalPol1["NUM_ENDOSO"] = Datos[i].Num_Endoso;
                        rowCalPol1["MTO_RESBAS_METAN"] = 0;
                        rowCalPol1["MTO_RESBASRET_METAN"] = 0;

                        PR_TMAE_CALPOL1.Rows.Add(rowCalPol1);
                        #endregion
                    }
                    if (Datos[i].Cod_Moneda == "US")
                    {
                        #region Llenado de tabla PR_TMAE_POLIZA2
                        rowPoliza2["COD_CLIENTE"] = dat8.Cod_cliente;
                        rowPoliza2["NUM_POLIZA"] = Datos[i].Num_Poliza;
                        rowPoliza2["COD_RESERVA"] = "N";
                        rowPoliza2["COD_PLAN"] = Datos[i].Cod_Tippension;
                        rowPoliza2["COD_COBERTURA"] = DBNull.Value;
                        rowPoliza2["COD_ESTADO"] = Datos[i].Cod_Estado;
                        rowPoliza2["COD_TIPREN"] = Datos[i].Cod_Tipren;
                        rowPoliza2["COD_MODALIDAD"] = Datos[i].Cod_Modalidad;
                        rowPoliza2["NUM_CARGAS"] = Datos[i].Num_Cargas;
                        rowPoliza2["FEC_VIGENCIA"] = Datos[i].Fec_Vigencia;
                        rowPoliza2["MTO_PRIMA"] = Datos[i].Mto_Prima;
                        rowPoliza2["MTO_PENSION"] = Datos[i].mto_pensionact;
                        rowPoliza2["NUM_MESDIF"] = Datos[i].Num_Mesdif;
                        rowPoliza2["NUM_MESGAR"] = Datos[i].Num_Mesgar;
                        rowPoliza2["PRC_TASACE"] = Datos[i].Prc_tasace;
                        rowPoliza2["PRC_TASAVTA"] = Datos[i].Prc_tasvta;
                        rowPoliza2["COD_CIAREA"] = "00";
                        rowPoliza2["COD_OPEREA"] = DBNull.Value;
                        rowPoliza2["COD_MODREA"] = DBNull.Value;
                        rowPoliza2["FEC_REA"] = DBNull.Value;
                        rowPoliza2["FEC_INIREA"] = DBNull.Value;
                        rowPoliza2["FEC_FINREA"] = DBNull.Value;
                        rowPoliza2["PRC_TASARET"] = 0;
                        rowPoliza2["PRC_TASACRE"] = 0;
                        rowPoliza2["COD_USUARIOCREA"] = usuario;
                        rowPoliza2["FEC_CREA"] = DateTime.Today.ToString("yyyyMMdd");
                        rowPoliza2["HOR_CREA"] = DateTime.Now.ToString("HHmmss");
                        rowPoliza2["COD_USUARIOMODI"] = DBNull.Value;
                        rowPoliza2["FEC_MODI"] = DBNull.Value;
                        rowPoliza2["HOR_MODI"] = DBNull.Value;
                        rowPoliza2["COD_BAUTIZO"] = DBNull.Value;
                        rowPoliza2["COD_BAUTIZOREA"] = DBNull.Value;
                        rowPoliza2["PRC_TASACEDEF"] = DBNull.Value;
                        rowPoliza2["PRC_TASAVTADEF"] = DBNull.Value;
                        rowPoliza2["PRC_TASABASEDEF"] = Datos[i].Prc_TasBaseDef;
                        rowPoliza2["MTO_RMPOL"] = DBNull.Value;
                        rowPoliza2["MTO_RMBASE"] = DBNull.Value;
                        rowPoliza2["NUM_BENPROBAU"] = DBNull.Value;
                        rowPoliza2["PRC_TASACREDEF"] = DBNull.Value;
                        rowPoliza2["PRC_TASABASEREADEF"] = DBNull.Value;
                        rowPoliza2["MTO_RMPOLREA"] = DBNull.Value;
                        rowPoliza2["MTO_RMBASEREA"] = DBNull.Value;
                        rowPoliza2["NUM_MESNOC"] = DBNull.Value;
                        rowPoliza2["NUM_BENPROBAUREA"] = DBNull.Value;
                        rowPoliza2["NUM_ENDOSO"] = Datos[i].Num_Endoso;
                        rowPoliza2["COD_BAUFIN"] = DBNull.Value;
                        rowPoliza2["PRC_TASACEF"] = DBNull.Value;
                        rowPoliza2["MTO_RESCEF"] = DBNull.Value;
                        rowPoliza2["NUM_BENPROBAUFIN"] = DBNull.Value;
                        rowPoliza2["FEC_DEV"] = Datos[i].FecDev;
                        rowPoliza2["FEC_PAGPRI"] = Datos[i].FecPagPri;
                        rowPoliza2["FEC_COT"] = Datos[i].FecCot;
                        rowPoliza2["COD_TEM"] = "N";
                        rowPoliza2["COD_CIAORIGEN"] = "01";
                        rowPoliza2["COD_CUSPP"] = Datos[i].Cod_Cuspp;
                        rowPoliza2["COD_COBCIA"] = Datos[i].CobCia;
                        rowPoliza2["COD_INDCALCE"] = "SI";
                        rowPoliza2["COD_INDRAMO"] = "09";
                        rowPoliza2["NUM_POLIZAREF"] = Datos[i].Num_Poliza;
                        rowPoliza2["PRC_FACTOR"] = Prc_Factor;
                        rowPoliza2["MTO_GTOSEP"] = Datos[i].MtoGtoSep;
                        rowPoliza2["COD_MONEDA"] = Datos[i].Cod_Moneda;
                        rowPoliza2["IND_COB"] = Datos[i].Ind_Cob;
                        rowPoliza2["COD_COBERCON"] = Datos[i].Cod_Cobercon;
                        rowPoliza2["MTO_FACPENELLA"] = Datos[i].Mto_Facpenella;
                        rowPoliza2["PRC_FACPENELLA"] = Datos[i].Prc_Facpenella;
                        rowPoliza2["COD_DERCRE"] = Datos[i].Cod_Dercre;
                        rowPoliza2["COD_DERGRA"] = Datos[i].Cod_Dergra;
                        rowPoliza2["COD_TIPREAJUSTE"] = Datos[i].Cod_Tipreajuste;
                        rowPoliza2["MTO_VALREAJUSTETRI"] = Datos[i].Mto_Valreajustetri;
                        rowPoliza2["MTO_VALREAJUSTEMEN"] = Datos[i].Mto_Valreajustemen;
                        rowPoliza2["NUM_MESESC"] = Datos[i].Num_Mesesc;
                        rowPoliza2["PRC_RENTAESC"] = Datos[i].Prc_Rentaesc;
                        rowPoliza2["FEC_FINPERESC"] = DBNull.Value;
                        rowPoliza2["MTO_PENSIONINI"] = Datos[i].Mto_Pension;
                        rowPoliza2["TOPE_18_AÑOS"] = DBNull.Value;
                        rowPoliza2["COD_ESTUDIANTE"] = DBNull.Value;
                        rowPoliza2["NUM_CASO_ESPECIAL"] = Datos[i].Num_Caso_Especial;//0;
                        rowPoliza2["FEC_DEVSOL"] = DBNull.Value;

                        PR_TMAE_POLIZA2.Rows.Add(rowPoliza2);
                        #endregion

                        #region Llenado de tabla PR_TMAE_CALPOL2
                        rowCalPol2["COD_CLIENTE"] = dat8.Cod_cliente;
                        rowCalPol2["NUM_POLIZA"] = Datos[i].Num_Poliza;
                        rowCalPol2["NUM_CARGAS"] = Datos[i].Num_Cargas;
                        rowCalPol2["COD_BASE"] = "N";
                        rowCalPol2["COD_FIJA"] = "N";
                        rowCalPol2["COD_FIN"] = "N";
                        rowCalPol2["COD_TASAVTA"] = "N";
                        rowCalPol2["MTO_RESBAS"] = 0;
                        rowCalPol2["MTO_RESBASRET"] = 0;
                        rowCalPol2["MTO_RESBASAJU"] = 0;
                        rowCalPol2["NUM_POLBASCED"] = 0;
                        rowCalPol2["NUM_POLBASRET"] = 0;
                        rowCalPol2["NUM_BENPROBAS"] = 0;
                        rowCalPol2["NUM_BENBASCED"] = 0;
                        rowCalPol2["NUM_BENBASRET"] = 0;
                        rowCalPol2["MTO_RESFIJ"] = 0;
                        rowCalPol2["MTO_RESFIJRET"] = 0;
                        rowCalPol2["MTO_RESFIJAJU"] = 0;
                        rowCalPol2["NUM_POLFIJCED"] = 0;
                        rowCalPol2["NUM_POLFIJRET"] = 0;
                        rowCalPol2["NUM_BENPROFIJ"] = 0;
                        rowCalPol2["NUM_BENFIJCED"] = 0;
                        rowCalPol2["NUM_BENFIJRET"] = 0;
                        rowCalPol2["MTO_RESFIN"] = 0;
                        rowCalPol2["MTO_RESFINRET"] = 0;
                        rowCalPol2["MTO_RESFINAJU"] = 0;
                        rowCalPol2["NUM_POLFINCED"] = 0;
                        rowCalPol2["NUM_POLFINRET"] = 0;
                        rowCalPol2["NUM_BENPROFIN"] = 0;
                        rowCalPol2["NUM_BENFINCED"] = 0;
                        rowCalPol2["NUM_BENFINRET"] = 0;
                        rowCalPol2["MTO_RESTASA"] = 0;
                        rowCalPol2["MTO_RESTASARET"] = 0;
                        rowCalPol2["MTO_RESTASAAJU"] = 0;
                        rowCalPol2["NUM_POLTASACED"] = 0;
                        rowCalPol2["NUM_POLTASARET"] = 0;
                        rowCalPol2["NUM_BENPROTASA"] = 0;
                        rowCalPol2["NUM_BENTASACED"] = 0;
                        rowCalPol2["NUM_BENTASARET"] = 0;
                        rowCalPol2["PRC_TASAMER"] = 0;
                        rowCalPol2["COD_USUARIOCREA"] = usuario;
                        rowCalPol2["FEC_CREA"] = DateTime.Today.ToString("yyyyMMdd");
                        rowCalPol2["HOR_CREA"] = DateTime.Now.ToString("HHmmss");
                        rowCalPol2["COD_USUARIOMODI"] = DBNull.Value;
                        rowCalPol2["FEC_MODI"] = DBNull.Value;
                        rowCalPol2["HOR_MODI"] = DBNull.Value;
                        rowCalPol2["NUM_ENDOSO"] = Datos[i].Num_Endoso;
                        rowCalPol2["MTO_RESBAS_METAN"] = 0;
                        rowCalPol2["MTO_RESBASRET_METAN"] = 0;

                        PR_TMAE_CALPOL2.Rows.Add(rowCalPol2);
                        #endregion
                    }

                    //Condición para insertar todas las pólizas en la tabla de tasas en caso de que se encuentre sola.
                    if (TasasPolNum == 0)
                    {
                        #region Llenado de tabla PR_TTMP_TASASRES
                        rowTasasRes["NUM_POLIZA"] = Datos[i].Num_Poliza;
                        rowTasasRes["PRC_TASLIBRERI"] = 0;
                        rowTasasRes["PRC_TASVTAPROM"] = 0;

                        PR_TTMP_TASASRES.Rows.Add(rowTasasRes);
                        #endregion
                    }
                    else
                    {
                        //Condición para insertar las nuevas pólizas en la tabla de tasas.
                        if (!pLstPolizasTasasRes.Any(pol => pol.Pol_NumPol == Datos[i].Num_Poliza))
                        {
                            #region Llenado de tabla PR_TTMP_TASASRES
                            rowTasasRes["NUM_POLIZA"] = Datos[i].Num_Poliza;
                            rowTasasRes["PRC_TASLIBRERI"] = 0;
                            rowTasasRes["PRC_TASVTAPROM"] = 0;

                            PR_TTMP_TASASRES.Rows.Add(rowTasasRes);
                            #endregion
                        }
                    }

                    //Línea para actualizar Tasa de Reserva en tabla maestra PP_TMAE_POLIZA
                    query += "UPDATE PP_TMAE_POLIZA SET PRC_TASARES = " + Datos[i].Prc_TasBaseDef + " WHERE NUM_POLIZA = '" + Datos[i].Num_Poliza + "' \n";
                }
                query += "UPDATE PR_TMAE_PROCALDEF SET NUM_TOTALPOLCAR = " + Datos.Count + " WHERE FEC_CALCULO = '" + fecha + "' \n";
                
                #endregion

                #region Proceso e inserción de Pólizas(PR_TMAE_BENEFICIARIO, PR_TMAE_CALBEN)
                parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_BEN", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, fecha, ParameterDirection.Input));

                DatosBen = SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    Num_Poliza = x.GetString(0),
                    Cod_Moneda = x.GetString(1),
                    Num_Orden = x.GetInt32(2),
                    Cod_Sexo = x.GetString(3),
                    Cod_Par = x.GetString(4),
                    Cod_SitInv = x.GetString(5),
                    Cod_Grufam = x.GetString(6),
                    Cod_Derpen = x.GetString(7),
                    Cod_Motreqpen = x.GetString(8),
                    Cod_Dercre = x.GetString(9),
                    Fec_Nacben = x.IsDBNull(10) ? "" : x.GetString(10),
                    Fec_Falben = x.IsDBNull(11) ? "" : x.GetString(11),
                    Fec_Nachm = x.IsDBNull(12) ? "" : x.GetString(12),
                    Fec_Invben = x.IsDBNull(13) ? "" : x.GetString(13),
                    Cod_Cauinv = x.GetString(14),
                    Mto_Pension = x.GetDecimal(15),
                    Mto_Pensiongar = x.IsDBNull(16) ? 0 : x.GetDecimal(16),
                    Num_Endoso = x.GetInt32(17),
                    Prc_Pension = x.GetDecimal(18),
                    Prc_Pensionleg = x.GetDecimal(19),
                    Prc_Pensiongar = x.GetDecimal(20),
                    Cod_Estudiante = x.GetString(21),
                    CodigoTope18 = x.GetString(22)
                }).ToList();

                //query = "";

                for (var i = 0; i < DatosBen.Count; i++)
                {
                    DataRow rowBeneficiario1 = PR_TMAE_BENEFICIARIO1.NewRow();
                    DataRow rowBeneficiario2 = PR_TMAE_BENEFICIARIO2.NewRow();
                    DataRow rowCalBen1 = PR_TMAE_CALBEN1.NewRow();
                    DataRow rowCalBen2 = PR_TMAE_CALBEN2.NewRow();

                    if (DatosBen[i].Cod_Moneda == "NS")
                    {
                        #region Llenado de tabla PR_TMAE_BENEFICIARIO1
                        rowBeneficiario1["COD_CLIENTE"] = dat8.Cod_cliente;
                        rowBeneficiario1["NUM_POLIZA"] = DatosBen[i].Num_Poliza;
                        rowBeneficiario1["NUM_ORDEN"] = DatosBen[i].Num_Orden;
                        rowBeneficiario1["COD_SEXO"] = DatosBen[i].Cod_Sexo;
                        rowBeneficiario1["COD_PAR"] = DatosBen[i].Cod_Par;
                        rowBeneficiario1["COD_SITINV"] = DatosBen[i].Cod_SitInv;
                        rowBeneficiario1["COD_DERPEN"] = DatosBen[i].Cod_Derpen;
                        rowBeneficiario1["COD_GRUFAM"] = DatosBen[i].Cod_Grufam;
                        rowBeneficiario1["COD_DERCRE"] = DatosBen[i].Cod_Dercre;
                        rowBeneficiario1["FEC_NACBEN"] = DatosBen[i].Fec_Nacben;
                        rowBeneficiario1["FEC_FALBEN"] = DatosBen[i].Fec_Falben;
                        rowBeneficiario1["FEC_NACHM"] = DatosBen[i].Fec_Nachm;
                        rowBeneficiario1["COD_MOTREQPEN"] = DatosBen[i].Cod_Motreqpen;
                        rowBeneficiario1["FEC_INVBEN"] = DatosBen[i].Fec_Invben;
                        rowBeneficiario1["COD_CAUINV"] = DatosBen[i].Cod_Cauinv;
                        rowBeneficiario1["MTO_PENSION"] = DatosBen[i].Mto_Pension;
                        rowBeneficiario1["MTO_PENSIONGAR"] = DatosBen[i].Mto_Pensiongar;
                        rowBeneficiario1["COD_USUARIOCREA"] = usuario;
                        rowBeneficiario1["FEC_CREA"] = DateTime.Today.ToString("yyyyMMdd");
                        rowBeneficiario1["HOR_CREA"] = DateTime.Now.ToString("HHmmss");
                        rowBeneficiario1["COD_USUARIOMODI"] = DBNull.Value;
                        rowBeneficiario1["FEC_MODI"] = DBNull.Value;
                        rowBeneficiario1["HOR_MODI"] = DBNull.Value;
                        rowBeneficiario1["NUM_ENDOSO"] = DatosBen[i].Num_Endoso;
                        rowBeneficiario1["PRC_PENSION"] = DatosBen[i].Prc_Pension;
                        rowBeneficiario1["PRC_PENSIONLEG"] = DatosBen[i].Prc_Pensionleg;
                        rowBeneficiario1["PRC_PENSIONGAR"] = DatosBen[i].Prc_Pensiongar;
                        rowBeneficiario1["COD_TOPE_18"] = DatosBen[i].CodigoTope18;
                        rowBeneficiario1["COD_ESTUDIANTE"] = DatosBen[i].Cod_Estudiante;

                        PR_TMAE_BENEFICIARIO1.Rows.Add(rowBeneficiario1);
                        #endregion

                        #region Llenado de tabla PR_TMAE_CALBEN1
                        rowCalBen1["COD_CLIENTE"] = dat8.Cod_cliente;
                        rowCalBen1["NUM_POLIZA"] = DatosBen[i].Num_Poliza;
                        rowCalBen1["NUM_ORDEN"] = DatosBen[i].Num_Orden;
                        rowCalBen1["COD_BASE"] = "N";
                        rowCalBen1["COD_FIJA"] = "N";
                        rowCalBen1["COD_FIN"] = "N";
                        rowCalBen1["COD_TASAVTA"] = "N";
                        rowCalBen1["NUM_EDAD"] = 0;
                        rowCalBen1["MTO_CNUBAS"] = 0;
                        rowCalBen1["MTO_CNABAS"] = 0;
                        rowCalBen1["MTO_CNRBAS"] = 0;
                        rowCalBen1["MTO_CNTBAS"] = 0;
                        rowCalBen1["MTO_CNGBAS"] = 0;
                        rowCalBen1["TMO_CNUFIJ"] = 0;
                        rowCalBen1["MTO_CNAFIJ"] = 0;
                        rowCalBen1["MTO_CNRFIJ"] = 0;
                        rowCalBen1["MTO_CNTFIJ"] = 0;
                        rowCalBen1["MTO_CNGFIJ"] = 0;
                        rowCalBen1["MTO_CNUFIN"] = 0;
                        rowCalBen1["MTO_CNAFIN"] = 0;
                        rowCalBen1["MTO_CNRFIN"] = 0;
                        rowCalBen1["MTO_CNTFIN"] = 0;
                        rowCalBen1["MTO_CNGFIN"] = 0;
                        rowCalBen1["MTO_CNUTASA"] = 0;
                        rowCalBen1["MTO_CNATASA"] = 0;
                        rowCalBen1["MTO_CNRTASA"] = 0;
                        rowCalBen1["MTO_CNTTASA"] = 0;
                        rowCalBen1["MTO_CNGTASA"] = 0;
                        rowCalBen1["COD_USUARIOCREA"] = usuario;
                        rowCalBen1["FEC_CREA"] = DateTime.Today.ToString("yyyyMMdd");
                        rowCalBen1["HOR_CREA"] = DateTime.Now.ToString("HHmmss");
                        rowCalBen1["COD_USUARIOMODI"] = DBNull.Value;
                        rowCalBen1["FEC_MODI"] = DBNull.Value;
                        rowCalBen1["HOR_MODI"] = DBNull.Value;
                        rowCalBen1["NUM_ENDOSO"] = DatosBen[i].Num_Endoso;
                        rowCalBen1["MTO_CNUBAS_METAN"] = 0;
                        rowCalBen1["MTO_CNABAS_METAN"] = 0;
                        rowCalBen1["MTO_CNGBAS_METAN"] = 0;
                        rowCalBen1["MTO_CNTBAS_METAN"] = 0;

                        PR_TMAE_CALBEN1.Rows.Add(rowCalBen1);
                        #endregion

                        //Actualiza FEC_DEVSOL (Nueva columna que se agregó a tablas PR_TMAE_POLIZA)
                        query += "UPDATE PR SET FEC_DEVSOL = CS.FEC_DEVSOL FROM PT_THIS_CARGASOL CS JOIN PD_TMAE_POLIZA P ON CS.NUM_OPERACION = P.NUM_OPERACION JOIN PR_TMAE_POLIZA1 PR ON PR.NUM_POLIZA = P.NUM_POLIZA\n";
                    }
                    if (DatosBen[i].Cod_Moneda == "US")
                    {
                        #region Llenado de tabla PR_TMAE_BENEFICIARIO2
                        rowBeneficiario2["COD_CLIENTE"] = dat8.Cod_cliente;
                        rowBeneficiario2["NUM_POLIZA"] = DatosBen[i].Num_Poliza;
                        rowBeneficiario2["NUM_ORDEN"] = DatosBen[i].Num_Orden;
                        rowBeneficiario2["COD_SEXO"] = DatosBen[i].Cod_Sexo;
                        rowBeneficiario2["COD_PAR"] = DatosBen[i].Cod_Par;
                        rowBeneficiario2["COD_SITINV"] = DatosBen[i].Cod_SitInv;
                        rowBeneficiario2["COD_DERPEN"] = DatosBen[i].Cod_Derpen;
                        rowBeneficiario2["COD_GRUFAM"] = DatosBen[i].Cod_Grufam;
                        rowBeneficiario2["COD_DERCRE"] = DatosBen[i].Cod_Dercre;
                        rowBeneficiario2["FEC_NACBEN"] = DatosBen[i].Fec_Nacben;
                        rowBeneficiario2["FEC_FALBEN"] = DatosBen[i].Fec_Falben;
                        rowBeneficiario2["FEC_NACHM"] = DatosBen[i].Fec_Nachm;
                        rowBeneficiario2["COD_MOTREQPEN"] = DatosBen[i].Cod_Motreqpen;
                        rowBeneficiario2["FEC_INVBEN"] = DatosBen[i].Fec_Invben;
                        rowBeneficiario2["COD_CAUINV"] = DatosBen[i].Cod_Cauinv;
                        rowBeneficiario2["MTO_PENSION"] = DatosBen[i].Mto_Pension;
                        rowBeneficiario2["MTO_PENSIONGAR"] = DatosBen[i].Mto_Pensiongar;
                        rowBeneficiario2["COD_USUARIOCREA"] = usuario;
                        rowBeneficiario2["FEC_CREA"] = DateTime.Today.ToString("yyyyMMdd");
                        rowBeneficiario2["HOR_CREA"] = DateTime.Now.ToString("HHmmss");
                        rowBeneficiario2["COD_USUARIOMODI"] = DBNull.Value;
                        rowBeneficiario2["FEC_MODI"] = DBNull.Value;
                        rowBeneficiario2["HOR_MODI"] = DBNull.Value;
                        rowBeneficiario2["NUM_ENDOSO"] = DatosBen[i].Num_Endoso;
                        rowBeneficiario2["PRC_PENSION"] = DatosBen[i].Prc_Pension;
                        rowBeneficiario2["PRC_PENSIONLEG"] = DatosBen[i].Prc_Pensionleg;
                        rowBeneficiario2["PRC_PENSIONGAR"] = DatosBen[i].Prc_Pensiongar;
                        rowBeneficiario2["COD_TOPE_18"] = DatosBen[i].CodigoTope18;
                        rowBeneficiario2["COD_ESTUDIANTE"] = DatosBen[i].Cod_Estudiante;

                        PR_TMAE_BENEFICIARIO2.Rows.Add(rowBeneficiario2);
                        #endregion

                        #region Llenado de tabla PR_TMAE_CALBEN2
                        rowCalBen2["COD_CLIENTE"] = dat8.Cod_cliente;
                        rowCalBen2["NUM_POLIZA"] = DatosBen[i].Num_Poliza;
                        rowCalBen2["NUM_ORDEN"] = DatosBen[i].Num_Orden;
                        rowCalBen2["COD_BASE"] = "N";
                        rowCalBen2["COD_FIJA"] = "N";
                        rowCalBen2["COD_FIN"] = "N";
                        rowCalBen2["COD_TASAVTA"] = "N";
                        rowCalBen2["NUM_EDAD"] = 0;
                        rowCalBen2["MTO_CNUBAS"] = 0;
                        rowCalBen2["MTO_CNABAS"] = 0;
                        rowCalBen2["MTO_CNRBAS"] = 0;
                        rowCalBen2["MTO_CNTBAS"] = 0;
                        rowCalBen2["MTO_CNGBAS"] = 0;
                        rowCalBen2["TMO_CNUFIJ"] = 0;
                        rowCalBen2["MTO_CNAFIJ"] = 0;
                        rowCalBen2["MTO_CNRFIJ"] = 0;
                        rowCalBen2["MTO_CNTFIJ"] = 0;
                        rowCalBen2["MTO_CNGFIJ"] = 0;
                        rowCalBen2["MTO_CNUFIN"] = 0;
                        rowCalBen2["MTO_CNAFIN"] = 0;
                        rowCalBen2["MTO_CNRFIN"] = 0;
                        rowCalBen2["MTO_CNTFIN"] = 0;
                        rowCalBen2["MTO_CNGFIN"] = 0;
                        rowCalBen2["MTO_CNUTASA"] = 0;
                        rowCalBen2["MTO_CNATASA"] = 0;
                        rowCalBen2["MTO_CNRTASA"] = 0;
                        rowCalBen2["MTO_CNTTASA"] = 0;
                        rowCalBen2["MTO_CNGTASA"] = 0;
                        rowCalBen2["COD_USUARIOCREA"] = usuario;
                        rowCalBen2["FEC_CREA"] = DateTime.Today.ToString("yyyyMMdd");
                        rowCalBen2["HOR_CREA"] = DateTime.Now.ToString("HHmmss");
                        rowCalBen2["COD_USUARIOMODI"] = DBNull.Value;
                        rowCalBen2["FEC_MODI"] = DBNull.Value;
                        rowCalBen2["HOR_MODI"] = DBNull.Value;
                        rowCalBen2["NUM_ENDOSO"] = DatosBen[i].Num_Endoso;
                        rowCalBen2["MTO_CNUBAS_METAN"] = 0;
                        rowCalBen2["MTO_CNABAS_METAN"] = 0;
                        rowCalBen2["MTO_CNGBAS_METAN"] = 0;
                        rowCalBen2["MTO_CNTBAS_METAN"] = 0;

                        PR_TMAE_CALBEN2.Rows.Add(rowCalBen2);
                        #endregion

                        //Actualiza FEC_DEVSOL (Nueva columna que se agregó a tablas PR_TMAE_POLIZA)
                        query += "UPDATE PR SET FEC_DEVSOL = CS.FEC_DEVSOL FROM PT_THIS_CARGASOL CS JOIN PD_TMAE_POLIZA P ON CS.NUM_OPERACION = P.NUM_OPERACION JOIN PR_TMAE_POLIZA2 PR ON PR.NUM_POLIZA = P.NUM_POLIZA\n";
                    }
                }

                query += "UPDATE PR_TMAE_PROCALDEF SET NUM_TOTALBENCAR = " + DatosBen.Count + " WHERE FEC_CALCULO = '" + fecha + "' \n";
                #endregion

                //Lineas para mandar a ejecutar los querys en Base de Datos(directamente en SeguroRV).
                _log.Info("Comenzara a realizar los bulks de inserción");
                BulkInsertFlujos(PR_TMAE_POLIZA1, "PR_TMAE_POLIZA1", strConexionSeguroRV);
                //_log.Info("Termino el primer Bulk");
                BulkInsertFlujos(PR_TMAE_POLIZA2, "PR_TMAE_POLIZA2", strConexionSeguroRV);
                //_log.Info("Termino el Segundo Bulk");
                BulkInsertFlujos(PR_TMAE_BENEFICIARIO1, "PR_TMAE_BENEFICIARIO1", strConexionSeguroRV);
                //_log.Info("Termino el tercer Bulk");
                BulkInsertFlujos(PR_TMAE_BENEFICIARIO2, "PR_TMAE_BENEFICIARIO2", strConexionSeguroRV);
                //_log.Info("Termino el cuarto Bulk");
                BulkInsertFlujos(PR_TMAE_CALPOL1, "PR_TMAE_CALPOL1", strConexionSeguroRV);
                //_log.Info("Termino el quinto Bulk");
                BulkInsertFlujos(PR_TMAE_CALPOL2, "PR_TMAE_CALPOL2", strConexionSeguroRV);
                //_log.Info("Termino el sexto Bulk");
                BulkInsertFlujos(PR_TMAE_CALBEN1, "PR_TMAE_CALBEN1", strConexionSeguroRV);
                //_log.Info("Termino el septimo Bulk");
                BulkInsertFlujos(PR_TMAE_CALBEN2, "PR_TMAE_CALBEN2", strConexionSeguroRV);
                //_log.Info("Termino el octavo Bulk");
                BulkInsertFlujos(PR_TTMP_TASASRES, "PR_TTMP_TASASRES", strConexionSeguroRV);
                _log.Info("Termino la ejecucion de Bulks");

                SRVDBContext<Reservas>.CallSelectStatement(query, x => new Reservas
                { }).FirstOrDefault();

                res2.IsOk = true;
                res2.Message = "Carga Terminada con Éxito";
                _log.Info("El Proceso de Migración de Reservas ha terminado correctamente.");
                return res2;
            }
            catch (Exception ex)
            {
                _log.Info("Error en el procesamiento y/o inserción del Proceso de Migración: " + ex.Message);
                res2.IsOk = false;
                res2.Message = "La carga no ha terminado correctamente: " + ex.Message;
                return res2;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 26-02-2019
        /// Método para generar variable de texto con los querys a ejecutar en BD para la actualización de la información.
        /// </summary>
        /// <param name="listDatos">Lista con registros y sus datos correspondientes a actualizar en BD.</param>
        /// <returns>Retorna mensaje de confirmación de proceso.</returns>
        public string Carga_Excel(List<Reservas> listDatos, List<Reservas> listParentesco)
        {
            #region DataTables para Carga de Excel
            #region PR_TMAE_POLIZA1
            PR_TMAE_POLIZA1.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASAVTA", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("PRC_TASACE", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("MTO_PENSION", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("MTO_PENSIONINI", typeof(decimal));
            PR_TMAE_POLIZA1.Columns.Add("NUM_MESDIF", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("NUM_MESGAR", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("NUM_MESESC", typeof(int));
            PR_TMAE_POLIZA1.Columns.Add("PRC_RENTAESC", typeof(decimal));
            #endregion

            #region PR_TMAE_POLIZA2
            PR_TMAE_POLIZA2.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASAVTA", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("PRC_TASACE", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("MTO_PENSION", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("MTO_PENSIONINI", typeof(decimal));
            PR_TMAE_POLIZA2.Columns.Add("NUM_MESDIF", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("NUM_MESGAR", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("NUM_MESESC", typeof(int));
            PR_TMAE_POLIZA2.Columns.Add("PRC_RENTAESC", typeof(decimal));
            #endregion

            #region PR_TMAE_BENEFICIARIO1
            PR_TMAE_BENEFICIARIO1.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_PAR", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("FEC_FALBEN", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("FEC_NACBEN", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("PRC_PENSION", typeof(decimal));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_TOPE_18", typeof(string));
            PR_TMAE_BENEFICIARIO1.Columns.Add("COD_ESTUDIANTE", typeof(string));
            #endregion

            #region PR_TMAE_BENEFICIARIO2
            PR_TMAE_BENEFICIARIO2.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_PAR", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("FEC_FALBEN", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("FEC_NACBEN", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("PRC_PENSION", typeof(decimal));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_TOPE_18", typeof(string));
            PR_TMAE_BENEFICIARIO2.Columns.Add("COD_ESTUDIANTE", typeof(string));
            #endregion

            #region PR_TTMP_TASASRES
            PR_TTMP_TASASRES.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TTMP_TASASRES.Columns.Add("PRC_TASLIBRERI", typeof(decimal));
            PR_TTMP_TASASRES.Columns.Add("PRC_TASVTAPROM", typeof(decimal));
            #endregion
            #endregion

            #region Tablas temporales y querys de actualización
            #region Tablas Temporales Carga Excel
            string TablasTemporalesCargaExcel = "CREATE TABLE TBL_TEMP_POLIZA1 (NUM_POLIZA VARCHAR(10) NULL, " +
                                                                               "PRC_TASAVTA NUMERIC(5, 2) NULL, " +
                                                                               "PRC_TASACE NUMERIC(5, 2) NULL, " +
                                                                               "MTO_PENSION NUMERIC(18, 2) NULL, " +
                                                                               "MTO_PENSIONINI NUMERIC(18, 2) NULL, " +
                                                                               "NUM_MESDIF INT NULL, " +
                                                                               "NUM_MESGAR INT NULL, " +
                                                                               "NUM_MESESC INT NULL, " +
                                                                               "PRC_RENTAESC NUMERIC(18, 2) NULL) \n\n" +
                                                "CREATE TABLE TBL_TEMP_POLIZA2 (NUM_POLIZA VARCHAR(10) NULL, " +
                                                                               "PRC_TASAVTA NUMERIC(5, 2) NULL, " +
                                                                               "PRC_TASACE NUMERIC(5, 2) NULL, " +
                                                                               "MTO_PENSION NUMERIC(18, 2) NULL, " +
                                                                               "MTO_PENSIONINI NUMERIC(18, 2) NULL, " +
                                                                               "NUM_MESDIF INT NULL, " +
                                                                               "NUM_MESGAR INT NULL, " +
                                                                               "NUM_MESESC INT NULL, " +
                                                                               "PRC_RENTAESC NUMERIC(18, 2) NULL) \n\n" +
                                                "CREATE TABLE TBL_TEMP_BENEFICIARIO1 (NUM_POLIZA VARCHAR(10) NULL, " +
                                                                                     "COD_PAR VARCHAR(5) NULL, " +
                                                                                     "FEC_FALBEN VARCHAR(8) NULL, " +
                                                                                     "FEC_NACBEN VARCHAR(8) NULL, " +
                                                                                     "PRC_PENSION NUMERIC(5, 2) NULL, " +
                                                                                     "COD_TOPE_18 VARCHAR(2) NULL, " +
                                                                                     "COD_ESTUDIANTE VARCHAR(2) NULL) \n\n" +
                                                "CREATE TABLE TBL_TEMP_BENEFICIARIO2 (NUM_POLIZA VARCHAR(10) NULL, " +
                                                                                     "COD_PAR VARCHAR(5) NULL, " +
                                                                                     "FEC_FALBEN VARCHAR(8) NULL, " +
                                                                                     "FEC_NACBEN VARCHAR(8) NULL, " +
                                                                                     "PRC_PENSION NUMERIC(5, 2) NULL, " +
                                                                                     "COD_TOPE_18 VARCHAR(2) NULL, " +
                                                                                     "COD_ESTUDIANTE VARCHAR(2) NULL) \n\n" +
                                                "CREATE TABLE TBL_TEMP_TASASRES (NUM_POLIZA VARCHAR(10) NULL, " +
                                                                                "PRC_TASLIBRERI NUMERIC(6, 4) NULL, " +
                                                                                "PRC_TASVTAPROM NUMERIC(6, 4) NULL)";

            string strEliminarTablasTemporales = "DROP TABLE TBL_TEMP_POLIZA1 \n" +
                                                 "DROP TABLE TBL_TEMP_POLIZA2 \n" +
                                                 "DROP TABLE TBL_TEMP_BENEFICIARIO1 \n" +
                                                 "DROP TABLE TBL_TEMP_BENEFICIARIO2 \n" +
                                                 "DROP TABLE TBL_TEMP_TASASRES \n";
            #endregion

            #region Updates Carga Excel
            string strUpdatePoliza1 = "UPDATE T SET T.MTO_PENSION = Temp.MTO_PENSION, " +
                                                   "T.MTO_PENSIONINI = Temp.MTO_PENSIONINI, " +
                                                   "T.PRC_TASAVTA = Temp.PRC_TASAVTA, " +
                                                   "T.PRC_TASACE = Temp.PRC_TASACE, " +
                                                   "T.NUM_MESDIF = Temp.NUM_MESDIF, " +
                                                   "T.NUM_MESGAR = Temp.NUM_MESGAR, " +
                                                   "T.NUM_MESESC = Temp.NUM_MESESC, " +
                                                   "T.PRC_RENTAESC = Temp.PRC_RENTAESC " +
                                                   "FROM PR_TMAE_POLIZA1 T " +
                                                   "INNER JOIN TBL_TEMP_POLIZA1 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA";

            string strUpdatePoliza2 = "UPDATE T SET T.MTO_PENSION = Temp.MTO_PENSION, " +
                                                   "T.MTO_PENSIONINI = Temp.MTO_PENSIONINI, " +
                                                   "T.PRC_TASAVTA = Temp.PRC_TASAVTA, " +
                                                   "T.PRC_TASACE = Temp.PRC_TASACE, " +
                                                   "T.NUM_MESDIF = Temp.NUM_MESDIF, " +
                                                   "T.NUM_MESGAR = Temp.NUM_MESGAR, " +
                                                   "T.NUM_MESESC = Temp.NUM_MESESC, " +
                                                   "T.PRC_RENTAESC = Temp.PRC_RENTAESC " +
                                                   "FROM PR_TMAE_POLIZA2 T " +
                                                   "INNER JOIN TBL_TEMP_POLIZA2 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA";

            string strUpdateBeneficiario1 = "UPDATE T SET T.PRC_PENSION = Temp.PRC_PENSION, " +
                                                         "T.FEC_FALBEN = Temp.FEC_FALBEN, " +
                                                         "T.COD_ESTUDIANTE = Temp.COD_ESTUDIANTE, " +
                                                         "T.COD_TOPE_18 = Temp.COD_TOPE_18 " +
                                                         "FROM PR_TMAE_BENEFICIARIO1 T " +
                                                         "INNER JOIN TBL_TEMP_BENEFICIARIO1 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA AND T.FEC_NACBEN = Temp.FEC_NACBEN " +
                                                         //AND T.COD_PAR = Temp.COD_PAR";
                                                         "AND case when T.COD_PAR in (10,11,21,20) then 10 else T.COD_PAR end = case when Temp.COD_PAR in (10,11,21,20) then 10 else Temp.COD_PAR end";

            string strUpdateBeneficiario2 = "UPDATE T SET T.PRC_PENSION = Temp.PRC_PENSION, " +
                                                         "T.FEC_FALBEN = Temp.FEC_FALBEN, " +
                                                         "T.COD_ESTUDIANTE = Temp.COD_ESTUDIANTE, " +
                                                         "T.COD_TOPE_18 = Temp.COD_TOPE_18 " +
                                                         "FROM PR_TMAE_BENEFICIARIO2 T " +
                                                         "INNER JOIN TBL_TEMP_BENEFICIARIO2 Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA AND T.FEC_NACBEN = Temp.FEC_NACBEN "+
                                                         //AND T.COD_PAR = Temp.COD_PAR";
                                                         "AND case when T.COD_PAR in (10,11,21,20) then 10 else T.COD_PAR end = case when Temp.COD_PAR in (10,11,21,20) then 10 else Temp.COD_PAR end";

            string strUpdateTasasRes = "UPDATE T SET T.PRC_TASLIBRERI = Temp.PRC_TASLIBRERI, " +
                                                    "T.PRC_TASVTAPROM = Temp.PRC_TASVTAPROM " +
                                                    "FROM PR_TTMP_TASASRES T " +
                                                    "INNER JOIN TBL_TEMP_TASASRES Temp ON T.NUM_POLIZA = Temp.NUM_POLIZA ";
            #endregion
            #endregion

            XmlConfigurator.Configure();
            string strConexionSeguroRV = cadena_conexion();
            string strCodigoParentesco = "";

            try
            {
                if (listDatos.Count != 0)
                {
                    for (int i = 0; i < listDatos.Count; i++)
                    {
                        strCodigoParentesco = "";
                        //strCodigoParentesco = (from par in listParentesco where par.Gls_Elemento == listDatos[i].Ben_CodPar select par.Cod_Par).FirstOrDefault();
                        switch (listDatos[i].Ben_CodPar)
                        {
                            case "T":
                                strCodigoParentesco = "99";
                                break;

                            case "C":
                                int hijos = (from hijo in listDatos where hijo.Ben_CodPar == "H" && hijo.Pol_NumPol == listDatos[i].Pol_NumPol select hijo.Ben_CodPar).ToList().Count();
                                if(hijos > 0)
                                    strCodigoParentesco = "11";
                                else
                                    strCodigoParentesco = "10";
                                break;

                            case "P":
                                if(listDatos[i].Ben_CodSexo == "F")
                                    strCodigoParentesco = "42";
                                else
                                    strCodigoParentesco = "41";
                                break;

                            case "H":
                                strCodigoParentesco = "30";
                                break;
                        }


                        if (listDatos[i].Pol_CodMod.ToUpper() == "SOLES INDEXADOS" || listDatos[i].Pol_CodMod.ToUpper() == "SOLES AJUSTADOS")
                        {
                            #region Llenado de DataTables
                            DataRow rowPoliza1 = PR_TMAE_POLIZA1.NewRow();
                            DataRow rowBeneficiario1 = PR_TMAE_BENEFICIARIO1.NewRow();

                            #region Llenado de tabla PR_TMAE_POLIZA1
                            rowPoliza1["NUM_POLIZA"] = listDatos[i].Pol_NumPol;
                            rowPoliza1["PRC_TASAVTA"] = Convert.ToDecimal(String.Format("{0:0.00}", (listDatos[i].Pol_TasaVta * 100)));
                            rowPoliza1["PRC_TASACE"] = Convert.ToDecimal(String.Format("{0:0.00}", (listDatos[i].Tasa_Equiv * 100)));
                            rowPoliza1["MTO_PENSION"] = Convert.ToDecimal(String.Format("{0:0.00}", listDatos[i].Remuneracion_Ajus));
                            rowPoliza1["MTO_PENSIONINI"] = Convert.ToDecimal(String.Format("{0:0.00}", listDatos[i].Remuneracion_Ini));
                            rowPoliza1["NUM_MESDIF"] = listDatos[i].Pol_MesesDif;
                            rowPoliza1["NUM_MESGAR"] = listDatos[i].Pol_MesesGar;
                            rowPoliza1["NUM_MESESC"] = listDatos[i].Pol_MesesEsc;
                            rowPoliza1["PRC_RENTAESC"] = Convert.ToDecimal(String.Format("{0:0.00}", (listDatos[i].Pol_PrcRentaEsc * 100)));
                            PR_TMAE_POLIZA1.Rows.Add(rowPoliza1);
                            #endregion

                            #region Llenado de tabla PR_TMAE_BENEFICIARIO1
                            rowBeneficiario1["NUM_POLIZA"] = listDatos[i].Pol_NumPol;
                            rowBeneficiario1["COD_PAR"] = strCodigoParentesco;
                            rowBeneficiario1["FEC_FALBEN"] = listDatos[i].Ben_FecFal;
                            rowBeneficiario1["FEC_NACBEN"] = listDatos[i].Ben_FecNac;
                            rowBeneficiario1["PRC_PENSION"] = Convert.ToDecimal(String.Format("{0:0.00}", (listDatos[i].Ben_PrcPension * 100)));
                            rowBeneficiario1["COD_TOPE_18"] = listDatos[i].CodigoTope18;
                            rowBeneficiario1["COD_ESTUDIANTE"] = listDatos[i].Num_Estudiante;

                            PR_TMAE_BENEFICIARIO1.Rows.Add(rowBeneficiario1);
                            #endregion

                            //queryFinal += "\nUPDATE PR_TMAE_POLIZA1 " +
                            //              "SET MTO_PENSION = " + listDatos[i].Remuneracion_Ajus + ", MTO_PENSIONINI = " + listDatos[i].Remuneracion_Ini + ", PRC_TASAVTA = " + (listDatos[i].Pol_TasaVta * 100) + ", PRC_TASACE = " + (listDatos[i].Tasa_Equiv * 100) +
                            //              " WHERE NUM_POLIZA = '" + listDatos[i].Pol_NumPol + "'";
                            //queryFinal += "\nUPDATE PR_TMAE_BENEFICIARIO1 " +
                            //              "SET PRC_PENSION = " + (listDatos[i].Ben_PrcPension * 100) + ", FEC_FALBEN = '" + listDatos[i].Ben_FecFal + "' " + ", COD_ESTUDIANTE = '" + listDatos[i].Num_Estudiante + "', COD_TOPE_18 = '" + listDatos[i].CodigoTope18 + "' " +
                            //              "WHERE NUM_POLIZA = '" + listDatos[i].Pol_NumPol + "' AND FEC_NACBEN = '" + listDatos[i].Ben_FecNac + "' AND COD_PAR = '" + strCodigoParentesco + "' ";
                            //queryFinal += "\nUPDATE PR_TTMP_TASASRES SET PRC_TASLIBRERI = " + listDatos[i].Pol_TasaLR + ", PRC_TASVTAPROM = " + listDatos[i].Tasa_VtaProm + " WHERE NUM_POLIZA = '" + listDatos[i].Pol_NumPol + "'";
                            #endregion
                        }

                        if (listDatos[i].Pol_CodMod.ToUpper() == "DOLARES AMERICANOS" || listDatos[i].Pol_CodMod.ToUpper() == "DÓLARES AMERICANOS" || listDatos[i].Pol_CodMod.ToUpper() == "DOLARES AJUSTADOS" || listDatos[i].Pol_CodMod.ToUpper() == "DÓLARES AJUSTADOS")
                        {
                            #region Llenado de DataTables
                            DataRow rowPoliza2 = PR_TMAE_POLIZA2.NewRow();
                            DataRow rowBeneficiario2 = PR_TMAE_BENEFICIARIO2.NewRow();

                            #region Llenado de tabla PR_TMAE_POLIZA2
                            rowPoliza2["NUM_POLIZA"] = listDatos[i].Pol_NumPol;
                            rowPoliza2["PRC_TASAVTA"] = Convert.ToDecimal(String.Format("{0:0.00}", (listDatos[i].Pol_TasaVta * 100)));
                            rowPoliza2["PRC_TASACE"] = Convert.ToDecimal(String.Format("{0:0.00}", (listDatos[i].Tasa_Equiv * 100)));
                            rowPoliza2["MTO_PENSION"] = Convert.ToDecimal(String.Format("{0:0.00}", listDatos[i].Remuneracion_Ajus));
                            rowPoliza2["MTO_PENSIONINI"] = Convert.ToDecimal(String.Format("{0:0.00}", listDatos[i].Remuneracion_Ini));
                            rowPoliza2["NUM_MESDIF"] = listDatos[i].Pol_MesesDif;
                            rowPoliza2["NUM_MESGAR"] = listDatos[i].Pol_MesesGar;
                            rowPoliza2["NUM_MESESC"] = listDatos[i].Pol_MesesEsc;
                            rowPoliza2["PRC_RENTAESC"] = Convert.ToDecimal(String.Format("{0:0.00}", (listDatos[i].Pol_PrcRentaEsc * 100)));
                            PR_TMAE_POLIZA2.Rows.Add(rowPoliza2);
                            #endregion

                            #region Llenado de tabla PR_TMAE_BENEFICIARIO2
                            rowBeneficiario2["NUM_POLIZA"] = listDatos[i].Pol_NumPol;
                            rowBeneficiario2["COD_PAR"] = strCodigoParentesco;
                            rowBeneficiario2["FEC_FALBEN"] = listDatos[i].Ben_FecFal;
                            rowBeneficiario2["FEC_NACBEN"] = listDatos[i].Ben_FecNac;
                            rowBeneficiario2["PRC_PENSION"] = Convert.ToDecimal(String.Format("{0:0.00}", (listDatos[i].Ben_PrcPension * 100)));
                            rowBeneficiario2["COD_TOPE_18"] = listDatos[i].CodigoTope18;
                            rowBeneficiario2["COD_ESTUDIANTE"] = listDatos[i].Num_Estudiante;

                            PR_TMAE_BENEFICIARIO2.Rows.Add(rowBeneficiario2);
                            #endregion

                            //queryFinal += "\nUPDATE PR_TMAE_POLIZA2 " +
                            //              "SET MTO_PENSION = " + listDatos[i].Remuneracion_Ajus + ", MTO_PENSIONINI = " + listDatos[i].Remuneracion_Ini + ", PRC_TASAVTA = " + (listDatos[i].Pol_TasaVta * 100) + ", PRC_TASACE = " + (listDatos[i].Tasa_Equiv * 100) +
                            //              " WHERE NUM_POLIZA = '" + listDatos[i].Pol_NumPol + "'";
                            //queryFinal += "\nUPDATE PR_TMAE_BENEFICIARIO2 " +
                            //              "SET PRC_PENSION = " + (listDatos[i].Ben_PrcPension * 100) + ", FEC_FALBEN = '" + listDatos[i].Ben_FecFal + "' " + ", COD_ESTUDIANTE = '" + listDatos[i].Num_Estudiante + "', COD_TOPE_18 = '" + listDatos[i].CodigoTope18 + "' " +
                            //              "WHERE NUM_POLIZA = '" + listDatos[i].Pol_NumPol + "' AND FEC_NACBEN = '" + listDatos[i].Ben_FecNac + "' AND COD_PAR = '" + strCodigoParentesco + "' ";
                            //queryFinal += "\nUPDATE PR_TTMP_TASASRES SET PRC_TASLIBRERI = " + listDatos[i].Pol_TasaLR + ", PRC_TASVTAPROM = " + listDatos[i].Tasa_VtaProm + " WHERE NUM_POLIZA = '" + listDatos[i].Pol_NumPol + "'";
                            #endregion
                        }

                        if (listDatos[i].Pol_TasaLR != 0 || listDatos[i].Tasa_VtaProm != 0)
                        {
                            #region Llenado de tabla PR_TTMP_TASASRES
                            DataRow rowTasasRes = PR_TTMP_TASASRES.NewRow();
                            rowTasasRes["NUM_POLIZA"] = listDatos[i].Pol_NumPol;
                            rowTasasRes["PRC_TASLIBRERI"] = Convert.ToDecimal(String.Format("{0:0.0000}", listDatos[i].Pol_TasaLR));
                            rowTasasRes["PRC_TASVTAPROM"] = Convert.ToDecimal(String.Format("{0:0.0000}", listDatos[i].Tasa_VtaProm));

                            PR_TTMP_TASASRES.Rows.Add(rowTasasRes);
                            #endregion

                            //queryFinal += "\nUPDATE PR_TTMP_TASASRES SET PRC_TASLIBRERI = " + listDatos[i].Pol_TasaLR + ", PRC_TASVTAPROM = " + listDatos[i].Tasa_VtaProm + " WHERE NUM_POLIZA = '" + listDatos[i].Pol_NumPol + "'";
                        }
                    }
                }
                else
                {
                    return "ERROR: lista de datos a modificar vacía, verificar que el archivo contenga información.";
                }
            }
            catch (Exception ex)
            {
                _log.Info("ERROR en llenado de DataTables para Carga de Excel: " + ex.Message);
                return "Error en llenado de tablas internas para actulización en Base de Datos.";
            }

            try
            {
                _log.Info("Se procederá a crear tablas temporales e insertar la información.");
                #region Inserción en tablas temporales y actualización de información.
                //Creación de tablas temporales para Carga de Excel.
                SRVDBContext<Reservas>.CallSelectStatement(TablasTemporalesCargaExcel, x => new Reservas { }).FirstOrDefault();

                //Inserción en Tablas Temporales.
                BulkInsertFlujos(PR_TMAE_POLIZA1, "TBL_TEMP_POLIZA1", strConexionSeguroRV);
                BulkInsertFlujos(PR_TMAE_POLIZA2, "TBL_TEMP_POLIZA2", strConexionSeguroRV);
                BulkInsertFlujos(PR_TMAE_BENEFICIARIO1, "TBL_TEMP_BENEFICIARIO1", strConexionSeguroRV);
                BulkInsertFlujos(PR_TMAE_BENEFICIARIO2, "TBL_TEMP_BENEFICIARIO2", strConexionSeguroRV);
                BulkInsertFlujos(PR_TTMP_TASASRES, "TBL_TEMP_TASASRES", strConexionSeguroRV);
                _log.Info("Información insertada correctamente para actualizar.");

                _log.Info("Se procederá a actualizar la información correspondiente de Pólizas y Beneficiarios.");
                //Actualización de información de Excel.
                SRVDBContext<Reservas>.CallSelectStatement(strUpdatePoliza1, x => new Reservas { }).FirstOrDefault();
                SRVDBContext<Reservas>.CallSelectStatement(strUpdatePoliza2, x => new Reservas { }).FirstOrDefault();
                SRVDBContext<Reservas>.CallSelectStatement(strUpdateBeneficiario1, x => new Reservas { }).FirstOrDefault();
                SRVDBContext<Reservas>.CallSelectStatement(strUpdateBeneficiario2, x => new Reservas { }).FirstOrDefault();
                SRVDBContext<Reservas>.CallSelectStatement(strUpdateTasasRes, x => new Reservas { }).FirstOrDefault();
                
                //Eliminación de tablas temporales.
                SRVDBContext<Reservas>.CallSelectStatement(strEliminarTablasTemporales, x => new Reservas { }).FirstOrDefault();
                #endregion

                _log.Info("CARGA DE EXCEL EXITOSA: actualización de información en Base de Datos exitosa.");
                return "Actualización en Base de Datos exitosa.";
            }
            catch (Exception ex)
            {
                _log.Info("ERROR al insertar y actualizar la información en Base de Datos: " + ex.Message);
                return "Error al insertar y actualizar la información en Base de Datos.";
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 05-03-2019
        /// Método reutilizado para enviar variable string con querys y ejecutarlas en BD.
        /// </summary>
        /// <param name="query">Variable con querys a ejecutar en BD.</param>
        public void EjecutaScripts_CalculoFlujos(string query)
        {
            try
            {
                XmlConfigurator.Configure();
                //_log.Info("Se procedera a ejecutar Query" + query);
                SRVDBContext<Reservas>.CallSelectStatement(query, x => new Reservas { }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Info("Error al ejecutar Query" + query + " Error" + ex);
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 25-05-2019
        /// Método para consultar el número de pólizas que ya existen en la tabla de tasas de reservas.
        /// </summary>
        /// <returns>Lista con todas las pólizas y sus números correspondientes.</returns>
        public List<Reservas> Consulta_PolizasTasas()
        {
            try
            {
                string query = "SELECT * FROM PR_TTMP_TASASRES ORDER BY 1";
                return SRVDBContext<Reservas>.CallSelectStatement(query, x => new Reservas
                {
                    Pol_NumPol = x.GetString(0)
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado
        /// 18-07-2019
        /// Método para obtener el último no. de póliza de la Tabla de Tasas de Reservas.
        /// </summary>
        /// <returns>Número de la última póliza.</returns>
        public int Consulta_UltimaPol()
        {
            XmlConfigurator.Configure();
            try
            {
                string query = "SELECT CONVERT(INT,MAX(NUM_POLIZA)) FROM PR_TTMP_TASASRES ORDER BY 1";
                Reservas listPol = new Reservas();
                listPol = SRVDBContext<Reservas>.CallSelectStatement(query, x => new Reservas
                {
                    LastPol = x.GetInt32(0)
                }).FirstOrDefault();

                return listPol.LastPol;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar última póliza para actualización de la tabla PR_TTMP_TASASRES: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 11-03-2019
        /// </summary>
        /// <param name="FecPeriodo">Fecha de periodo ingresado en pantalla.</param>
        /// <returns>Fecha con información de la última tasa promedio de mercado.</returns>
        public string Fec_TasaProm(string FecPeriodo)
        {
            string FecTasaProm = "";
            int year = int.Parse(FecPeriodo.Substring(0, 4));
            string month = FecPeriodo.Substring(4, 2);
            Reservas ListTasaProm = new Reservas();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_TASAPROM", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecPeriodo", SqlDbType.VarChar, FecPeriodo + "01", ParameterDirection.Input));

                ListTasaProm = SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    FecActual = x.GetString(0)
                }).FirstOrDefault();

                if (ListTasaProm.FecActual == (FecPeriodo + "01"))
                {
                    FecTasaProm = FecPeriodo + "01";
                    //FecTasaProm = FecPeriodo;
                }
                else
                {
                    FecTasaProm = ListTasaProm.FecActual;
                    //FecTasaProm = (year - 1).ToString() + month + "01";
                }

                return FecTasaProm;
            }
            catch (Exception)
            {
                return "Error al obtener la Tasa Promedio de Mercado.";
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 14-03-2019
        /// Método para validar que exista el Tipo de Cambio para la fecha ingresada.
        /// </summary>
        /// <param name="FecPeriodo">Fecha de periodo ingresada en pantalla.</param>
        /// <returns>Lista con resultados de búsqueda en BD.</returns>
        public List<Reservas> Tipo_Cambio(string FecPeriodo)
        {
            XmlConfigurator.Configure();
            string FecPer = FecPeriodo.Substring(0, 6);
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_TIPCAMBIO", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecPeriodo", SqlDbType.VarChar, FecPer, ParameterDirection.Input));

                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    TipCambio = (double)x.GetDecimal(0)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("Error al obtener el tipo de cambio: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// Método para insertar en BD con conexión directa a la BD SeguroRV.
        /// </summary>
        /// <param name="dt">Tabla con todos los registros a insertar.</param>
        /// <param name="TableName">Nombre de la tabla de BD en la cual se insertará.</param>
        /// <param name="conexion">Cadena de conexión.</param>
        public void BulkInsertFlujos(DataTable dt, string TableName, string conexion)
        {
            XmlConfigurator.Configure();

            try
            {
                using (SqlConnection connection = new SqlConnection(conexion))
                {
                    SqlBulkCopy bulkCopy =
                        new SqlBulkCopy
                        (
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                        );

                    bulkCopy.BulkCopyTimeout = 7200;
                    bulkCopy.DestinationTableName = TableName;
                    connection.Open();

                    bulkCopy.WriteToServer(dt);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                _log.Info("ERROR al insertar información en la tabla " + TableName + ": " + ex.Message);
            }
        }

        //public void BulkInsertFlujos(DataTable dt, string TableName, string conexion)
        //{
        //    using (var localConnection = new SqlConnection(conexion))
        //    {
        //        SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(localConnection);
        //        sqlBulkCopy.BulkCopyTimeout = 0;
        //        sqlBulkCopy.DestinationTableName = TableName;
        //        localConnection.Open();
        //        sqlBulkCopy.WriteToServer(dt);
        //    }
        //}

        public void BulkUpdateFlujos(DataTable dt, string TableName, string conexion, string TblTemp, string strUpdate, string strWhere, string numTbl)
        {
            using (SqlConnection connection = new SqlConnection(conexion))
            {
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    try
                    {
                        connection.Open();

                        //Creating temp table on database
                        command.CommandText = TblTemp;
                        command.ExecuteNonQuery();

                        //Bulk insert into temp table
                        using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                        {
                            bulkcopy.BulkCopyTimeout = 960000;
                            bulkcopy.DestinationTableName = "#TmpTableFujo" + numTbl;
                            bulkcopy.WriteToServer(dt);
                            bulkcopy.Close();
                        }

                        // Updating destination table, and dropping temp table
                        command.CommandTimeout = 960000;
                        command.CommandText = strUpdate + TableName + " T INNER JOIN #TmpTableFujo" + numTbl + " Temp ON " + strWhere + " DROP TABLE #TmpTableFujo" + numTbl + "; ";
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 08-04-2019
        /// Método para obtener datos de homologación de Parentesco para Carga de Excel-
        /// </summary>
        /// <returns>Lista con datos de parentescos.</returns>
        public List<Reservas> Homologa_Parentesco()
        {
            XmlConfigurator.Configure();
            try
            {
                string queryCon = "SELECT COD_ELEMENTO, GLS_ELEMENTO FROM MA_TPAR_TABCOD WHERE COD_TABLA = 'PA'";
                return SRVDBContext<Reservas>.CallSelectStatement(queryCon, x => new Reservas
                {
                    Cod_Par = x.GetString(0),
                    Gls_Elemento = x.GetString(1)
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("ERROR EN PROCESO LÓGICO DE CARGA DE EXCEL.");
                _log.Info("ERROR al consultar información para homologación de parentesco: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 21-05-2019
        /// Método para ejecutar los querys directamente en BD SeguroRV.
        /// </summary>
        /// <param name="query">Cadena con instrucciones a ejecutar en BD.</param>
        /// <param name="conn">Cadena de conexión a SeguroRV.</param>
        public void Ejecuta_Query_Conn(string query, string conn)
        {
            XmlConfigurator.Configure();
            try
            {
                VCEDBContext<Reservas>.CallSelectStatementConection(conn, query, x => new Reservas
                { }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Info("Error al ejecutar query en Base de Datos: " + ex.Message);
            }
        }

        #region Reportes de Flujos de Cartera
        #region Rpt Cálculo Nuevo
        /// <summary>
        /// José Hernández Alvarado.
        /// 27-05-2019
        /// Método para consultar los Flujos Totales desde BD.
        /// </summary>
        /// <returns>Lista con Flujos Totales de Proceso de Flujos de Cartera.</returns>
        //public List<Reservas> Consulta_FluCartera(string FecFlu)
        //{
        //    XmlConfigurator.Configure();
        //    try
        //    {
        //        var parameters = new List<SqlParameter>();
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_FLUPOLTOT", ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, FecFlu, ParameterDirection.Input));

        //        _log.Info("Se consultará la información de los Flujos de Reservas Nuevos Totales.");
        //        return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
        //        {
        //            FC_Mes = Convert.ToInt32(x.GetDecimal(0)),
        //            FC_SIPensiones = (double)x.GetDecimal(1),
        //            FC_SISepelio = (double)x.GetDecimal(2),
        //            FC_SAPensiones = (double)x.GetDecimal(3),
        //            FC_SASepelio = (double)x.GetDecimal(4),
        //            FC_DPensiones = (double)x.GetDecimal(5),
        //            FC_DSepelio = (double)x.GetDecimal(6),
        //            FC_SITotales = (double)x.GetDecimal(7),
        //            FC_SATotales = (double)x.GetDecimal(8),
        //            FC_DTotales = (double)x.GetDecimal(9)
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info("Error al consultar información de hoja de flujos totales: " + ex.Message);
        //        return null;
        //    }

        //}
        public int Consulta_FluCartera(string FecFlu)
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_FLUPOLTOT", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, FecFlu, ParameterDirection.Input));

                _log.Info("Se consultará la información de los Flujos de Reservas Nuevos Totales.");
                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    NumeroRegistros = x.GetInt32(0)
                }).First().NumeroRegistros;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información de hoja de flujos totales: " + ex.Message);
                return 0;
            }
        }
        public bool CreacionArchivosRPT_FLUPOLTOT_hoja1(DataTable DatosArchivos, string Consulta, string NombreTabla)
        {
            XmlConfigurator.Configure();
            try
            {
                List<Reservas> lista = new List<Reservas>();
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CURSORGENEXCELHOJA1", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTypeGenExcel", SqlDbType.Structured, DatosArchivos, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pConsultasCursor", SqlDbType.VarChar, Consulta, ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pTablaCursor", SqlDbType.VarChar, NombreTabla, ParameterDirection.Input));

                _log.Info("Se consultará la información de los Flujos de Reservas Nuevos Totales.");
                lista = SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas {
                    Mensaje =x.IsDBNull(0) ? "" : x.GetString(0) 
                }).ToList();

                return true;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información de hoja de flujos totales: " + ex.Message);
                return false;
            }
        }

        public string RutaArchivo()
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RUTAARCHIVO", ParameterDirection.Input));
                
                _log.Info("Se consultará la ruta del archivo");
                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    Mensaje = x.GetString(0)
                }).First().Mensaje;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información de hoja de flujos totales: " + ex.Message);
                return null;
            }
        }

        public string RutaArchivoCalculoRes()
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RUTAARCHIVOCALRES", ParameterDirection.Input));

                _log.Info("Se consultará la ruta del archivo");
                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    Mensaje = x.GetString(0)
                }).First().Mensaje;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información de hoja de flujos totales: " + ex.Message);
                return null;
            }
        }
        public string RutaArchivoMigracion()
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RUTAARCHIVOMIGRACION", ParameterDirection.Input));

                _log.Info("Se consultará la ruta del archivo");
                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    Mensaje = x.GetString(0)
                }).First().Mensaje;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la ruta de excel Migración: " + ex.Message);
                return null;
            }
        }

        public int Consulta_FluDetCartera(string FecFlu)
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_FLUDETPOL", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, FecFlu, ParameterDirection.Input));

                _log.Info("Se consultará la información de los Flujos de Reservas Nuevos, flujos por beneficiarios.");
                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    NumeroRegistros = x.GetInt32(0)
                }).First().NumeroRegistros;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información de hoja de flujos por beneficiario: " + ex.Message);
                return 0;
            }

        }
        #endregion

        #region Rpt Cálculo Antigüo
        /// <summary>
        /// José Hernández Alvarado.
        /// 18-06-2019
        /// Método para consultar los Flujos Totales desde BD (cálculo antigüo).
        /// </summary>
        /// <returns>Lista con Flujos Totales de Proceso de Flujos de Cartera.</returns>
        //public List<Reservas> Consulta_FluCarteraAnt(string FecFlu)
        //{
        //    XmlConfigurator.Configure();
        //    try
        //    {
        //        var parameters = new List<SqlParameter>();
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_FLUPOLTOT_ANT", ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, FecFlu, ParameterDirection.Input));

        //        _log.Info("Se consultará la información de los Flujos de Reservas Antigüos Totales.");
        //        return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
        //        {
        //            FC_Mes = Convert.ToInt32(x.GetDecimal(0)),
        //            FC_SIPensiones = (double)x.GetDecimal(1),
        //            FC_SISepelio = (double)x.GetDecimal(2),
        //            FC_SAPensiones = (double)x.GetDecimal(3),
        //            FC_SASepelio = (double)x.GetDecimal(4),
        //            FC_DPensiones = (double)x.GetDecimal(5),
        //            FC_DSepelio = (double)x.GetDecimal(6),
        //            FC_SITotales = (double)x.GetDecimal(7),
        //            FC_SATotales = (double)x.GetDecimal(8),
        //            FC_DTotales = (double)x.GetDecimal(9)
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info("Error al consultar información de hoja de flujos totales: " + ex.Message);
        //        return null;
        //    }

        //}
        public int Consulta_FluCarteraAnt(string FecFlu)
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_FLUPOLTOT_ANT", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, FecFlu, ParameterDirection.Input));

                _log.Info("Se consultará la información de los Flujos de Reservas Antigüos Totales.");
                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    NumeroRegistros = x.GetInt32(0)
                }).First().NumeroRegistros;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información de hoja de flujos totales: " + ex.Message);
                return 0;
            }

        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 18-06-2019
        /// Método para consultar los Flujos por póliza desde BD (cálculo antigüo).
        /// </summary>
        /// <returns>Lista con Flujos por póliza de Proceso de Flujos de Cartera.</returns>
        //public List<Reservas> Consulta_FluDetCarteraAnt(string FecFlu)
        //{
        //    XmlConfigurator.Configure();
        //    try
        //    {
        //        var parameters = new List<SqlParameter>();
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_FLUDETPOL_ANT", ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, FecFlu, ParameterDirection.Input));

        //        _log.Info("Se consultará la información de los Flujos de Reservas Antigüos, flujos por beneficiarios.");
        //        return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
        //        {
        //            FC_NumPol = x.GetString(0),
        //            FC_Mes = Convert.ToInt32(x.GetInt64(1)),
        //            FC_GastoSep = (double)x.GetDecimal(2),
        //            FC_Titular = (double)x.GetDecimal(3),
        //            FC_Conyugue = (double)x.GetDecimal(4),
        //            FC_Padre = (double)x.GetDecimal(5),
        //            FC_Madre = (double)x.GetDecimal(6),
        //            FC_Hijo1 = (double)x.GetDecimal(7),
        //            FC_Hijo2 = (double)x.GetDecimal(8),
        //            FC_Hijo3 = (double)x.GetDecimal(9),
        //            FC_Hijo4 = (double)x.GetDecimal(10),
        //            FC_Hijo5 = (double)x.GetDecimal(11),
        //            FC_Hijo6 = (double)x.GetDecimal(12),
        //            FC_Hijo7 = (double)x.GetDecimal(13),
        //            FC_Hijo8 = (double)x.GetDecimal(14),
        //            FC_Hijo9 = (double)x.GetDecimal(15),
        //            FC_Hijo10 = (double)x.GetDecimal(16),
        //            FC_MtoTotal = (double)x.GetDecimal(17)
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info("Error al consultar información de hoja de flujos por beneficiario: " + ex.Message);
        //        return null;
        //    }

        //}
        public int Consulta_FluDetCarteraAnt(string FecFlu)
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_FLUDETPOL_ANT", ParameterDirection.Input));
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pFecha", SqlDbType.VarChar, FecFlu, ParameterDirection.Input));

                _log.Info("Se consultará la información de los Flujos de Reservas Antigüos, flujos por beneficiarios.");
                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    NumeroRegistros = x.GetInt32(0)
                }).First().NumeroRegistros;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar información de hoja de flujos por beneficiario: " + ex.Message);
                return 0;
            }

        }
        #endregion
        #endregion

        #region Reporte de Proceso de Migración
        /// <summary>
        /// José Hernández Alvarado.
        /// 27-06-2019
        /// Método para consultar en BD datos para Rpt de Proceso de Migración.
        /// </summary>
        /// <returns>Lista con registros para reporte.</returns>
        public List<Reservas> Consulta_ResMigracion()
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "REPORTE_CSV", ParameterDirection.Input));

                _log.Info("Se consultará la información del Excel de Migración.");
                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    Pol_NumPol = x.GetString(0),
                    Pol_CUSPP = x.GetString(1),
                    Pol_Prestacion = x.GetString(2),
                    Pol_CodMod = x.GetString(3),
                    Pol_FecVig = x.GetString(4),
                    Pol_FecDev = x.GetString(5),
                    Pol_MesesDif = x.GetInt32(6),
                    Pol_MesesGar = x.GetInt32(7),
                    Pol_MesesEsc = x.GetInt32(8),
                    Pol_PrcRentaEsc = x.GetDecimal(9),
                    Pol_CodGratif = x.GetString(10),
                    Ben_CodSitInv = x.GetString(11),
                    Ben_FecNac = x.GetString(12),
                    Ben_CodSexo = x.GetString(13),
                    Ben_CodPar = x.GetString(14),
                    Ben_PrcPension = x.GetDecimal(15),
                    Num_Estudiante = x.GetString(16),
                    Cod_Estudiante = x.GetString(17),
                    Ben_FecFal = x.GetString(18),
                    Remuneracion_Ini = x.GetDecimal(19),
                    Remuneracion_Ajus = x.GetDecimal(20),
                    Pension_Ajus = x.GetDecimal(21),
                    Pol_MtoPrima = x.GetDecimal(22),
                    Pol_TasaVta = x.GetDecimal(23),
                    Pol_TasaLR = x.GetDecimal(24),
                    Tasa_VtaProm = x.GetDecimal(25),
                    Tasa_Equiv = x.GetDecimal(26)
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("Error en obtención de datos para reporte de Proceso de Migración.");
                _log.Info("Error: " + ex.Message);
                return null;
            }

        }
        #endregion

        #region Reporte de Cálculo de Reservas
        /// <summary>
        /// José Hernández Alvarado.
        /// 28-06-2019
        /// Metódo para consultar en BD registros para Rpt de Reservas Nuevas.
        /// </summary>
        /// <returns>Lista con registros de consulta de BD.</returns>
        public List<Reservas> Consulta_CalculoRes()
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_CALRES", ParameterDirection.Input));

                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    Pol_NumPol = x.GetString(0),
                    Pol_CUSPP = x.GetString(1),
                    Pol_Prestacion = x.GetString(2),
                    Pol_CodMod = x.GetString(3),
                    Pol_FecVig = x.GetString(4),
                    Pol_FecDev = x.GetString(5),
                    Pol_MesesDif = x.GetInt32(6),
                    Pol_MesesGar = x.GetInt32(7),
                    Pol_MesesEsc = x.GetInt32(8),
                    Pol_PrcRentaEsc = x.GetDecimal(9),
                    Pol_CodGratif = x.GetString(10),
                    Ben_CodSitInv = x.GetString(11),
                    Ben_FecNac = x.GetString(12),
                    Ben_CodSexo = x.GetString(13),
                    Ben_CodPar = x.GetString(14),
                    Ben_PrcPension = x.GetDecimal(15),
                    Num_Estudiante = x.GetString(16),
                    Cod_Estudiante = x.GetString(17),
                    Ben_FecFal = x.GetString(18),
                    Remuneracion_Ini = x.GetDecimal(19),
                    Remuneracion_Ajus = x.GetDecimal(20),
                    Pension_Ajus = x.GetDecimal(21),
                    Pol_MtoPrima = x.GetDecimal(22),
                    Pol_TasaVta = x.GetDecimal(23),
                    Pol_TasaLR = x.GetDecimal(24),
                    Tasa_VtaProm = x.GetDecimal(25),
                    Tasa_Equiv = x.GetDecimal(26),
                    Reserva_Ben = x.GetDecimal(27),
                    Reserva_GastoSep = x.GetDecimal(28),
                    Reserva_TotPol = x.GetDecimal(29)
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("Error al obtener datos de reporte de Cálculo de Reservas(Nuevas): " + ex.Message);
                return null;
            }

        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 28-06-2019
        /// Metódo para consultar en BD registros para Rpt de Reservas Antigüas.
        /// </summary>
        /// <returns>Lista con registros de consulta de BD.</returns>
        public List<Reservas> Consulta_CalculoResAnt()
        {
            XmlConfigurator.Configure();
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(SRVDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "RPT_CALRES_ANT", ParameterDirection.Input));

                return SRVDBContext<Reservas>.CallStoreProcedure(StoredProcedures.CR_ConsultasReservas, parameters, x => new Reservas
                {
                    Pol_NumPol = x.GetString(0),
                    Pol_CUSPP = x.GetString(1),
                    Pol_Prestacion = x.GetString(2),
                    Pol_CodMod = x.GetString(3),
                    Pol_FecVig = x.GetString(4),
                    Pol_FecDev = x.GetString(5),
                    Pol_MesesDif = x.GetInt32(6),
                    Pol_MesesGar = x.GetInt32(7),
                    Pol_MesesEsc = x.GetInt32(8),
                    Pol_PrcRentaEsc = x.GetDecimal(9),
                    Pol_CodGratif = x.GetString(10),
                    Ben_CodSitInv = x.GetString(11),
                    Ben_FecNac = x.GetString(12),
                    Ben_CodSexo = x.GetString(13),
                    Ben_CodPar = x.GetString(14),
                    Ben_PrcPension = x.GetDecimal(15),
                    Num_Estudiante = x.GetString(16),
                    Cod_Estudiante = x.GetString(17),
                    Ben_FecFal = x.GetString(18),
                    Remuneracion_Ini = x.GetDecimal(19),
                    Remuneracion_Ajus = x.GetDecimal(20),
                    Pension_Ajus = x.GetDecimal(21),
                    Pol_MtoPrima = x.GetDecimal(22),
                    Pol_TasaVta = x.GetDecimal(23),
                    Pol_TasaLR = x.GetDecimal(24),
                    Tasa_VtaProm = x.GetDecimal(25),
                    Tasa_Equiv = x.GetDecimal(26),
                    Reserva_Ben = x.GetDecimal(27),
                    Reserva_GastoSep = x.GetDecimal(28),
                    Reserva_TotPol = x.GetDecimal(29)
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("Error al obtener datos de reporte de Cálculo de Reservas(Antigüas): " + ex.Message);
                return null;
            }

        }
        #endregion

        #region Obtener cadena de conexión a SegurosRV
        /// <summary>
        /// José Hernández Alvarado.
        /// Método reutilizado para obtener datos de conexión directos a BD SeguroRV.
        /// 28-10-2019
        /// </summary>
        /// <returns>Lista con datos de conexión.</returns>
        public List<Parametro> ParamsConexion()
        {
            string queryCon = "SELECT ClaveParametro, Parametro FROM Parametros WHERE DescripcionParametro = 'CONQA'";
            return VCEDBContext<Parametro>.CallSelectStatement(queryCon, x => new Parametro
            {
                ClaveParametro = x.GetString(0),
                Elemento = x.GetString(1),
            }).ToList();
        }

        /// <summary>
        /// Generar cadena string de conexión directa a Base de Datos SeguroRV para ejecución de scripts desde el back.
        /// José Hernández Alvarado.
        /// 28-10-2019
        /// </summary>
        /// <returns>Cadena de conexión directa.</returns>
        public string cadena_conexion()
        {
            List<Parametro> DatosCon = new List<Parametro>();
            DatosCon = ParamsConexion();

            string pass = (from passw in DatosCon where passw.ClaveParametro == "PASS" select passw.Elemento).FirstOrDefault();
            string ip = (from ips in DatosCon where ips.ClaveParametro == "IPSERVQA" select ips.Elemento).FirstOrDefault();
            string us = (from user in DatosCon where user.ClaveParametro == "USERSERQA" select user.Elemento).FirstOrDefault();
            string BD = (from bd in DatosCon where bd.ClaveParametro == "BD" select bd.Elemento).FirstOrDefault();

            return "Data Source=" + ip + "; Initial Catalog="+ BD +"; uid=" + us + ";pwd=" + pass + "";
        }
        #endregion
    }
}
    