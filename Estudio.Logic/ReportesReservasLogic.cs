using Estudio.Repository.Core.Domain;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using System.Reflection;
using log4net.Config;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Globalization;

namespace Estudio.Logic
{
    public class ReportesReservasLogic
    {
        //Globales
        ReportesReservasRepository _reportesReservasRepository = new ReportesReservasRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Insertar valores de registros en archivo excel.
        /// José Hernández Alvarado
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha de periodo abierto o periodo a buscar</param>
        /// <param name="pPathFile">Ruta temporal del archivo creado para descargarlo</param>
        /// <param name="pHistorico">Parámetro para determinar si la consulta será de las tablas de histórico o no(S -> Si, N -> No)</param>
        /// <returns>Ruta temporal del archivo</returns>
        public string ExportarReporteFlujosPasivos(string pFechaPeriodo, string pPathFile, string pHistorico)
        {
            XmlConfigurator.Configure();
            SLDocument sl = new SLDocument(pPathFile);
            List<ReportesReservas> lstFlujosPasivos = new List<ReportesReservas>();
            List<ReportesReservas> lstFlujosPasivosAntiguos = new List<ReportesReservas>();
            List<ReportesReservas> lstFlujosNuevosAntiguos = new List<ReportesReservas>();

            try
            {
                _log.Info("Se consultará la información para reporte de Flujos de Pasivos, por favor espere...");
                if (pHistorico == "N")
                {
                    lstFlujosPasivos = _reportesReservasRepository.ConsultarFlujosPasivos(pFechaPeriodo, "SELECTFLUJOSPASIVOS"); //Flujos Nuevos
                    lstFlujosPasivosAntiguos = _reportesReservasRepository.ConsultarFlujosPasivos(pFechaPeriodo, "SELECTFLUJOSPASIVOSANTIGUOS"); //Flujos Antigüos
                    lstFlujosNuevosAntiguos = _reportesReservasRepository.ConsultarFlujosPasivos(pFechaPeriodo, "SELECTFLUJOSPASIVOSANTIGUOSNUEVOS"); //Flujos Nuevos y Antigüos
                }
                else
                {
                    lstFlujosPasivos = _reportesReservasRepository.ConsultarFlujosPasivos(pFechaPeriodo, "SELECTFLUJOSPASIVOSHIS"); //Flujos Nuevos
                    lstFlujosPasivosAntiguos = _reportesReservasRepository.ConsultarFlujosPasivos(pFechaPeriodo, "SELECTFLUJOSPASIVOSANTIGUOSHIS"); //Flujos Antigüos
                    lstFlujosNuevosAntiguos = _reportesReservasRepository.ConsultarFlujosPasivos(pFechaPeriodo, "SELECTFLUJOSPASIVOSANTIGUOSNUEVOSHIS"); //Flujos Nuevos y Antigüos
                }

                if (lstFlujosPasivos != null && lstFlujosPasivosAntiguos != null && lstFlujosNuevosAntiguos != null)
                {
                    _log.Info("Se ha consultado toda la información.");
                }
                else
                {
                    _log.Info("No se pudo consultar toda la información correctamente.");
                }

                #region Llenado de excel
                if (lstFlujosPasivos != null)
                {
                    sl.SelectWorksheet("Flujos de pasivos"); //Hoja de Flujos Nuevos
                    sl.SetCellValue(1, 8, pFechaPeriodo);
                    for (int i = 0; i < lstFlujosPasivos.Count; i++)
                    {
                        sl.SetCellValue(5 + i, 1, lstFlujosPasivos[i].AnoMes);
                        sl.SetCellValue(5 + i, 2, lstFlujosPasivos[i].Fila);
                        sl.SetCellValue(5 + i, 3, lstFlujosPasivos[i].Ano);
                        sl.SetCellValue(5 + i, 4, lstFlujosPasivos[i].Mes);
                        sl.SetCellValue(5 + i, 5, lstFlujosPasivos[i].Jubilacion_SolesIndexados);
                        sl.SetCellValue(5 + i, 6, lstFlujosPasivos[i].Jubilacion_Dolares);
                        sl.SetCellValue(5 + i, 7, lstFlujosPasivos[i].Jubilacion_SolesAjustados);
                        sl.SetCellValue(5 + i, 8, lstFlujosPasivos[i].Jubilacion_GastosSepelio);
                        sl.SetCellValue(5 + i, 9, lstFlujosPasivos[i].Sobrevivencia_SolesIndexados);
                        sl.SetCellValue(5 + i, 10, lstFlujosPasivos[i].Sobrevivencia_Dolares);
                        sl.SetCellValue(5 + i, 11, lstFlujosPasivos[i].Sobrevivencia_SolesAjustados);
                        sl.SetCellValue(5 + i, 12, lstFlujosPasivos[i].Invalidez_SolesIndexados);
                        sl.SetCellValue(5 + i, 13, lstFlujosPasivos[i].Invalidez_Dolares);
                        sl.SetCellValue(5 + i, 14, lstFlujosPasivos[i].Invalidez_SolesAjustados);
                        sl.SetCellValue(5 + i, 15, lstFlujosPasivos[i].Invalidez_GastosSepelio);
                        sl.SetCellValue(5 + i, 16, lstFlujosPasivos[i].GastosOperacion);
                    }
                }

                if (lstFlujosPasivosAntiguos != null)
                {
                    sl.SelectWorksheet("Flujos de pasivos antigüos"); //Hoja de Flujos Antigüos
                    sl.SetCellValue(1, 8, pFechaPeriodo);
                    for (int i = 0; i < lstFlujosPasivosAntiguos.Count; i++)
                    {
                        sl.SetCellValue(5 + i, 1, lstFlujosPasivosAntiguos[i].AnoMes);
                        sl.SetCellValue(5 + i, 2, lstFlujosPasivosAntiguos[i].Fila);
                        sl.SetCellValue(5 + i, 3, lstFlujosPasivosAntiguos[i].Ano);
                        sl.SetCellValue(5 + i, 4, lstFlujosPasivosAntiguos[i].Mes);
                        sl.SetCellValue(5 + i, 5, lstFlujosPasivosAntiguos[i].Jubilacion_SolesIndexados);
                        sl.SetCellValue(5 + i, 6, lstFlujosPasivosAntiguos[i].Jubilacion_Dolares);
                        sl.SetCellValue(5 + i, 7, lstFlujosPasivosAntiguos[i].Jubilacion_SolesAjustados);
                        sl.SetCellValue(5 + i, 8, lstFlujosPasivosAntiguos[i].Jubilacion_GastosSepelio);
                        sl.SetCellValue(5 + i, 9, lstFlujosPasivosAntiguos[i].Sobrevivencia_SolesIndexados);
                        sl.SetCellValue(5 + i, 10, lstFlujosPasivosAntiguos[i].Sobrevivencia_Dolares);
                        sl.SetCellValue(5 + i, 11, lstFlujosPasivosAntiguos[i].Sobrevivencia_SolesAjustados);
                        sl.SetCellValue(5 + i, 12, lstFlujosPasivosAntiguos[i].Invalidez_SolesIndexados);
                        sl.SetCellValue(5 + i, 13, lstFlujosPasivosAntiguos[i].Invalidez_Dolares);
                        sl.SetCellValue(5 + i, 14, lstFlujosPasivosAntiguos[i].Invalidez_SolesAjustados);
                        sl.SetCellValue(5 + i, 15, lstFlujosPasivosAntiguos[i].Invalidez_GastosSepelio);
                        sl.SetCellValue(5 + i, 16, lstFlujosPasivosAntiguos[i].GastosOperacion);
                    }
                }

                if (lstFlujosNuevosAntiguos != null)
                {
                    sl.SelectWorksheet("Flujos nuevos y antigüos"); //Hoja de Flujos Antigüos
                    sl.SetCellValue(1, 8, pFechaPeriodo);
                    for (int i = 0; i < lstFlujosNuevosAntiguos.Count; i++)
                    {
                        sl.SetCellValue(5 + i, 1, lstFlujosNuevosAntiguos[i].AnoMes);
                        sl.SetCellValue(5 + i, 2, lstFlujosNuevosAntiguos[i].Fila);
                        sl.SetCellValue(5 + i, 3, lstFlujosNuevosAntiguos[i].Ano);
                        sl.SetCellValue(5 + i, 4, lstFlujosNuevosAntiguos[i].Mes);
                        sl.SetCellValue(5 + i, 5, lstFlujosNuevosAntiguos[i].Jubilacion_SolesIndexados);
                        sl.SetCellValue(5 + i, 6, lstFlujosNuevosAntiguos[i].Jubilacion_Dolares);
                        sl.SetCellValue(5 + i, 7, lstFlujosNuevosAntiguos[i].Jubilacion_SolesAjustados);
                        sl.SetCellValue(5 + i, 8, lstFlujosNuevosAntiguos[i].Jubilacion_GastosSepelio);
                        sl.SetCellValue(5 + i, 9, lstFlujosNuevosAntiguos[i].Sobrevivencia_SolesIndexados);
                        sl.SetCellValue(5 + i, 10, lstFlujosNuevosAntiguos[i].Sobrevivencia_Dolares);
                        sl.SetCellValue(5 + i, 11, lstFlujosNuevosAntiguos[i].Sobrevivencia_SolesAjustados);
                        sl.SetCellValue(5 + i, 12, lstFlujosNuevosAntiguos[i].Invalidez_SolesIndexados);
                        sl.SetCellValue(5 + i, 13, lstFlujosNuevosAntiguos[i].Invalidez_Dolares);
                        sl.SetCellValue(5 + i, 14, lstFlujosNuevosAntiguos[i].Invalidez_SolesAjustados);
                        sl.SetCellValue(5 + i, 15, lstFlujosNuevosAntiguos[i].Invalidez_GastosSepelio);
                        sl.SetCellValue(5 + i, 16, lstFlujosNuevosAntiguos[i].GastosOperacion);
                    }
                }
                #endregion

                sl.SaveAs(pPathFile);

                return pPathFile;
            }
            catch (Exception ex)
            {
                _log.Info("Error en generación de Reporte de Flujos de Pasivos, al insertar información en excel: " + ex.Message);
                return pPathFile;
            }
        }

        /// <summary>
        /// Insertar en archivo de texto líneas con información consultada.
        /// José Hernández Alvarado
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha del periodo abierto o periodo a buscar</param>
        /// <param name="pPathFile">Ruta temporal del archivo creado</param>
        /// <param name="pTipoArchivo">Parámetro para determinar si viene de la pantalla de Reservas o Reportes de Reservas</param>
        /// <param name="pHistorico">Parámetro para determinar si la consulta será de las tablas de histórico o no(S -> Si, N -> No)</param>
        /// <returns>Ruta temporal del archivo</returns>
        public string GenerarArchivoTxt(string pFechaPeriodo, string pPathFile, string pTipoArchivo, string pHistorico)
        {
            XmlConfigurator.Configure();
            string strBandera = "";
            string strArchivo = "";
            List<ReportesReservas> lstLineasArchivo = new List<ReportesReservas>();

            try
            {
                _log.Info("Se generará el archivo de texto del anexo 17, por favor espere...");

                switch (pTipoArchivo)
                {
                    case "N":
                        strBandera = pHistorico == "S" ? "SELECTTXTNUEVOSHIS" : "SELECTTXTNUEVOS";
                        strArchivo = "Anexo 17 - Nuevas";
                        break;

                    case "A":
                        strBandera = pHistorico == "S" ? "SELECTTXTANTIGUOSHIS" : "SELECTTXTANTIGUOS";
                        strArchivo = "Anexo 17 - Antigüas";
                        break;

                    case "M":
                        strBandera = pHistorico == "S" ? "SELECTTXTNUEVOSANTIGUOSHIS" : "SELECTTXTNUEVOSANTIGUOS";
                        strArchivo = "Anexo 17 - Mixtas";
                        break;
                }

                
                lstLineasArchivo = _reportesReservasRepository.ConsultarDatosArchivoTxt(pFechaPeriodo, strBandera);
                _log.Info("Se ha consultado la información correctamente.");
                if (lstLineasArchivo != null)
                {
                    using (StreamWriter sw = File.CreateText(pPathFile))
                    {
                        foreach (var item in lstLineasArchivo)
                        {
                            sw.WriteLine(item.LineaArchivoTxt);
                        }
                    }
                    _log.Info("El archivo " + strArchivo + " se ha generado correctamente.");
                }
                
                return pPathFile;
            }
            catch (Exception ex)
            {
                _log.Info("Error al generar archivo " + strArchivo + ", insertando la información en el excel: " + ex.Message);
                return pPathFile;
            }
        }

        /// <summary>
        /// Insertar información consultada para Reporte de Resumen de Reservas en el excel.
        /// José Hernández Alvarado.
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha del periodo abierto o periodo a buscar</param>
        /// <param name="pPathFile">Ruta temporal del archivo creado</param>
        /// <param name="pHistorico">Parámetro para determinar si la consulta será de las tablas de histórico o no(S -> Si, N -> No)</param>
        /// <returns>Ruta temporal del archivo</returns>
        public string ExportarReporteResumenReservas(string pFechaPeriodo, string pPathFile, string pHistorico)
        {
            XmlConfigurator.Configure();
            DateTime dtmFechaPeriodo = new DateTime();
            string strFechaPeriodoAnterior = "";
            SLDocument sl = new SLDocument(pPathFile);
            List<ReportesReservas> lstNormaAntiguaActual = new List<ReportesReservas>();
            List<ReportesReservas> lstNormaAntiguaAnterior = new List<ReportesReservas>();
            List<ReportesReservas> lstNormaNuevaActual = new List<ReportesReservas>();
            List<ReportesReservas> lstNormaNuevaAnterior = new List<ReportesReservas>();
            ReportesReservas datosInsuficiencia = new ReportesReservas();

            try
            {
                dtmFechaPeriodo = DateTime.ParseExact(pFechaPeriodo, "yyyyMMdd", CultureInfo.InvariantCulture);
                strFechaPeriodoAnterior = dtmFechaPeriodo.AddDays(-(DateTime.DaysInMonth(dtmFechaPeriodo.Year, dtmFechaPeriodo.Month))).ToString("yyyyMMdd");

                _log.Info("Se consultará la información para reporte de Resumen de Reservas, por favor espere...");
                if (pHistorico == "N")
                {
                    lstNormaAntiguaActual = _reportesReservasRepository.ConsultarReporteResumenReservas(pFechaPeriodo, "SELECTNORMAANTIGUAACTUAL", strFechaPeriodoAnterior);
                    lstNormaAntiguaAnterior = _reportesReservasRepository.ConsultarReporteResumenReservas(pFechaPeriodo, "SELECTNORMAANTIGUAANTERIOR", strFechaPeriodoAnterior);
                    lstNormaNuevaActual = _reportesReservasRepository.ConsultarReporteResumenReservas(pFechaPeriodo, "SELECTNORMANUEVAACTUAL", strFechaPeriodoAnterior);
                    lstNormaNuevaAnterior = _reportesReservasRepository.ConsultarReporteResumenReservas(pFechaPeriodo, "SELECTNORMANUEVAANTERIOR", strFechaPeriodoAnterior);
                    datosInsuficiencia = _reportesReservasRepository.ConsultarInsuficiencia("ANEXO18INSUFICIENCIA");
                }
                else
                {
                    lstNormaAntiguaActual = _reportesReservasRepository.ConsultarReporteResumenReservas(pFechaPeriodo, "SELECTNORMAANTIGUAACTUALHIS", strFechaPeriodoAnterior);
                    lstNormaAntiguaAnterior = _reportesReservasRepository.ConsultarReporteResumenReservas(pFechaPeriodo, "SELECTNORMAANTIGUAANTERIORHIS", strFechaPeriodoAnterior);
                    lstNormaNuevaActual = _reportesReservasRepository.ConsultarReporteResumenReservas(pFechaPeriodo, "SELECTNORMANUEVAACTUALHIS", strFechaPeriodoAnterior);
                    lstNormaNuevaAnterior = _reportesReservasRepository.ConsultarReporteResumenReservas(pFechaPeriodo, "SELECTNORMANUEVAANTERIORHIS", strFechaPeriodoAnterior);
                    datosInsuficiencia = _reportesReservasRepository.ConsultarInsuficiencia("ANEXO18INSUFICIENCIA");
                }

                if (lstNormaAntiguaActual != null && lstNormaAntiguaAnterior != null && lstNormaNuevaActual != null && lstNormaNuevaAnterior != null && datosInsuficiencia != null)
                {
                    _log.Info("Se ha consultado toda la información.");
                }
                else
                {
                    _log.Info("No se pudo consultar toda la información correctamente.");
                }

                #region Llenado de Excel
                sl.SelectWorksheet("Estado Resultados");
                sl.SetCellValue(2, 6, dtmFechaPeriodo);
                sl.SetCellValue(2, 12, DateTime.ParseExact(strFechaPeriodoAnterior, "yyyyMMdd", CultureInfo.InvariantCulture));

                #region NORMA ANTIGÜA - MES ACTUAL
                if (lstNormaAntiguaActual != null)
                {
                    //SOLES INDEXADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(9, 4, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(10, 4, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(11, 4, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(9, 7, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(10, 7, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(11, 7, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());

                    //SOLES AJUSTADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(15, 4, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(16, 4, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(17, 4, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(15, 7, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(16, 7, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(17, 7, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());

                    //SOLES AJUSTADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(21, 4, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(22, 4, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(23, 4, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(21, 7, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(22, 7, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(23, 7, (from monto in lstNormaAntiguaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());
                }
                #endregion

                #region NORMA ANTIGÜA - MES ANTERIOR
                if (lstNormaAntiguaAnterior != null)
                {
                    //SOLES INDEXADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(9, 10, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(10, 10, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(11, 10, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(9, 13, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(10, 13, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(11, 13, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());

                    //SOLES AJUSTADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(15, 10, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(16, 10, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(17, 10, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(15, 13, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(16, 13, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(17, 13, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());

                    //SOLES AJUSTADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(21, 10, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(22, 10, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(23, 10, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(21, 13, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(22, 13, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(23, 13, (from monto in lstNormaAntiguaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());
                }
                #endregion

                #region NORMA NUEVA - MES ACTUAL
                if (lstNormaNuevaActual != null)
                {
                    sl.SetCellValue(39, 4, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(40, 4, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(41, 4, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(39, 7, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(40, 7, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(41, 7, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());

                    //SOLES AJUSTADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(45, 4, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(46, 4, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(47, 4, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(45, 7, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(46, 7, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(47, 7, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());

                    //SOLES AJUSTADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(51, 4, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(52, 4, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(53, 4, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(51, 7, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(52, 7, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(53, 7, (from monto in lstNormaNuevaActual where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());
                }
                #endregion

                #region NORMA NUEVA - MES ANTERIOR
                if (lstNormaNuevaAnterior != null)
                {
                    //SOLES INDEXADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(39, 10, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(40, 10, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(41, 10, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(39, 13, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(40, 13, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(41, 13, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "1" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());

                    //SOLES AJUSTADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(45, 10, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(46, 10, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(47, 10, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(45, 13, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(46, 13, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(47, 13, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "NS" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());

                    //SOLES AJUSTADOS - NORMA ANTIGUA/MES ACTUAL
                    sl.SetCellValue(51, 10, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(52, 10, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.MontoReservaBase).FirstOrDefault());
                    sl.SetCellValue(53, 10, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.MontoReservaBase).FirstOrDefault());

                    sl.SetCellValue(51, 13, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "1. Jubilación" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(52, 13, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "2. Invalidez" select monto.NumeroPolizas).FirstOrDefault());
                    sl.SetCellValue(53, 13, (from monto in lstNormaNuevaAnterior where monto.CodigoMoneda == "US" && monto.CodigoReajuste == "2" && monto.Prestacion == "3. Sobrevivencia" select monto.NumeroPolizas).FirstOrDefault());
                }
                #endregion

                #region INSUFICIENCIA
                if (datosInsuficiencia != null)
                {
                    sl.SetCellValue(91, 4, datosInsuficiencia.SI_Insuficiencia);
                    sl.SetCellValue(91, 5, datosInsuficiencia.SA_Insuficiencia);
                    sl.SetCellValue(91, 6, datosInsuficiencia.DolaresEnSoles_Insuficiencia);
                }
                #endregion
                #endregion

                sl.SaveAs(pPathFile);

                return pPathFile;
            }
            catch (Exception ex)
            {
                _log.Info("Error al generar Reporte de Resumen Reservas, al insetar información en excel: " + ex.Message);
                return pPathFile;
            }
        }

        /// <summary>
        /// Insertar información consultada para Reporte SBS en el excel.
        /// José Hernández Alvarado.
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha del periodo abierto o periodo a buscar</param>
        /// <param name="pPathFile">Ruta temporal del archivo creado</param>
        /// <returns>Ruta temporal del archivo</returns>
        public string ExportarReporteSbs(string pFechaPeriodo, string pPathFile)
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se consultará la información para Reporte SBS, por favor espere...");
                DataTable baseSBS = _reportesReservasRepository.ConsultarReporteSBS(pFechaPeriodo, "SELECTREPORTESBS");
                baseSBS.TableName = "Base SBS";

                SLDocument sl = new SLDocument(pPathFile);

                int renglon = 4;
                foreach (DataRow rowDataTable in baseSBS.Rows)
                {
                    //sl.DrawBorder(renglon, 2, renglon, 48, BorderStyleValues.Thin, System.Drawing.Color.Black); //para marcar todo el renglon
                    for (int j = 0; j < baseSBS.Columns.Count; j++)
                    {
                        sl.SetCellValue(renglon, j + 2, rowDataTable[j].ToString());
                    }
                    renglon++;
                }
                sl.SaveAs(pPathFile);

                return pPathFile;
            }
            catch (Exception ex)
            {
                _log.Info("Error en generación de Reporte SBS, al insertar información en excel: " + ex.Message);
                return pPathFile;
            }
        }

        /// <summary>
        /// Insertar información consultada para Reporte Resumen Adecuación en el excel.
        /// José Hernández Alvarado.
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha del periodo abierto o periodo a buscar</param>
        /// <param name="pPathFile">Ruta temporal del archivo creado</param>
        /// <param name="pHistorico">Parámetro para determinar si la consulta será de las tablas de histórico o no(S -> Si, N -> No)</param>
        /// <returns>Ruta temporal del archivo</returns>
        public string ExportarReporteResumenAdecuacion(string pFechaPeriodo, string pPathFile, string pHistorico)
        {
            XmlConfigurator.Configure();
            Reservas ResMontos = new Reservas();
            List<Reservas> MtosMonPen = new List<Reservas>();

            try
            {
                _log.Info("Se consultará la información para Reporte de Resumen Adecuación, por favor espere...");
                if (pHistorico == "N")
                {
                    MtosMonPen = _reportesReservasRepository.ConsultarReporteResumenAdecuacion(pFechaPeriodo, "SELECTRESUMENADECUACION");
                }
                else
                {
                    MtosMonPen = _reportesReservasRepository.ConsultarReporteResumenAdecuacion(pFechaPeriodo, "SELECTRESUMENADECUACIONHIS");
                }

                if (MtosMonPen != null)
                {
                    _log.Info("Se ha consultado toda la información.");

                    /*MORTALIDAD VIGENTES 2018*/
                    ResMontos.Jubilacion_SolIndex = (from mto in MtosMonPen where mto.Renta == "JUBILACION" && mto.Moneda == "SOLES INDEXADOS" && mto.Num_Poliza == "2018" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Jubilacion_SolReaj = (from mto in MtosMonPen where mto.Renta == "JUBILACION" && mto.Moneda == "SOLES AJUSTADOS" && mto.Num_Poliza == "2018" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Jubilacion_Dolar = (from mto in MtosMonPen where mto.Renta == "JUBILACION" && mto.Moneda == "DOLARES AJUSTADOS" && mto.Num_Poliza == "2018" select mto.Mtos_TotalesRes).FirstOrDefault();

                    ResMontos.Invalidez_SolIndex = (from mto in MtosMonPen where mto.Renta == "INVALIDEZ" && mto.Moneda == "SOLES INDEXADOS" && mto.Num_Poliza == "2018" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Invalidez_SolReaj = (from mto in MtosMonPen where mto.Renta == "INVALIDEZ" && mto.Moneda == "SOLES AJUSTADOS" && mto.Num_Poliza == "2018" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Invalidez_Dolar = (from mto in MtosMonPen where mto.Renta == "INVALIDEZ" && mto.Moneda == "DOLARES AJUSTADOS" && mto.Num_Poliza == "2018" select mto.Mtos_TotalesRes).FirstOrDefault();

                    ResMontos.Sobrevivencia_SolIndex = (from mto in MtosMonPen where mto.Renta == "SOBREVIVENCIA" && mto.Moneda == "SOLES INDEXADOS" && mto.Num_Poliza == "2018" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Sobrevivencia_SolReaj = (from mto in MtosMonPen where mto.Renta == "SOBREVIVENCIA" && mto.Moneda == "SOLES AJUSTADOS" && mto.Num_Poliza == "2018" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Sobrevivencia_Dolar = (from mto in MtosMonPen where mto.Renta == "SOBREVIVENCIA" && mto.Moneda == "DOLARES AJUSTADOS" && mto.Num_Poliza == "2018" select mto.Mtos_TotalesRes).FirstOrDefault();

                    /*MORTALIDAD VIGENTES 2019*/
                    ResMontos.Jubilacion_SolIndex19 = (from mto in MtosMonPen where mto.Renta == "JUBILACION" && mto.Moneda == "SOLES INDEXADOS" && mto.Num_Poliza == "2019" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Jubilacion_SolReaj19 = (from mto in MtosMonPen where mto.Renta == "JUBILACION" && mto.Moneda == "SOLES AJUSTADOS" && mto.Num_Poliza == "2019" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Jubilacion_Dolar19 = (from mto in MtosMonPen where mto.Renta == "JUBILACION" && mto.Moneda == "DOLARES AJUSTADOS" && mto.Num_Poliza == "2019" select mto.Mtos_TotalesRes).FirstOrDefault();

                    ResMontos.Invalidez_SolIndex19 = (from mto in MtosMonPen where mto.Renta == "INVALIDEZ" && mto.Moneda == "SOLES INDEXADOS" && mto.Num_Poliza == "2019" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Invalidez_SolReaj19 = (from mto in MtosMonPen where mto.Renta == "INVALIDEZ" && mto.Moneda == "SOLES AJUSTADOS" && mto.Num_Poliza == "2019" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Invalidez_Dolar19 = (from mto in MtosMonPen where mto.Renta == "INVALIDEZ" && mto.Moneda == "DOLARES AJUSTADOS" && mto.Num_Poliza == "2019" select mto.Mtos_TotalesRes).FirstOrDefault();

                    ResMontos.Sobrevivencia_SolIndex19 = (from mto in MtosMonPen where mto.Renta == "SOBREVIVENCIA" && mto.Moneda == "SOLES INDEXADOS" && mto.Num_Poliza == "2019" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Sobrevivencia_SolReaj19 = (from mto in MtosMonPen where mto.Renta == "SOBREVIVENCIA" && mto.Moneda == "SOLES AJUSTADOS" && mto.Num_Poliza == "2019" select mto.Mtos_TotalesRes).FirstOrDefault();
                    ResMontos.Sobrevivencia_Dolar19 = (from mto in MtosMonPen where mto.Renta == "SOBREVIVENCIA" && mto.Moneda == "DOLARES AJUSTADOS" && mto.Num_Poliza == "2019" select mto.Mtos_TotalesRes).FirstOrDefault();

                    SLDocument sl = new SLDocument(pPathFile);

                    #region Llenado de excel
                    /*JUBILACION*/
                    //--Soles Indexados
                    sl.SetCellValue(10, 5, ResMontos.Jubilacion_SolIndex); //TABLAS DE MORTALIDAD VIGENTES HASTA 31/12/2018
                    sl.SetCellValue(10, 6, ResMontos.Jubilacion_SolIndex19);  //TABLAS DE MORTALIDAD VIGENTES A PARTIR DEL 01/01/2019

                    //--Soles Reajustados
                    sl.SetCellValue(10, 8, ResMontos.Jubilacion_SolReaj); //TABLAS DE MORTALIDAD VIGENTES HASTA 31/12/2018
                    sl.SetCellValue(10, 9, ResMontos.Jubilacion_SolReaj19);  //TABLAS DE MORTALIDAD VIGENTES A PARTIR DEL 01/01/2019

                    //--Dolares
                    sl.SetCellValue(10, 11, ResMontos.Jubilacion_Dolar); //TABLAS DE MORTALIDAD VIGENTES HASTA 31/12/2018
                    sl.SetCellValue(10, 12, ResMontos.Jubilacion_Dolar19);  //TABLAS DE MORTALIDAD VIGENTES A PARTIR DEL 01/01/2019


                    /*INVALIDEZ*/
                    //--Soles Indexados
                    sl.SetCellValue(11, 5, ResMontos.Invalidez_SolIndex); //TABLAS DE MORTALIDAD VIGENTES HASTA 31/12/2018
                    sl.SetCellValue(11, 6, ResMontos.Invalidez_SolIndex19);//TABLAS DE MORTALIDAD VIGENTES A PARTIR DEL 01/01/2019

                    //--Soles Reajustados
                    sl.SetCellValue(11, 8, ResMontos.Invalidez_SolReaj); //TABLAS DE MORTALIDAD VIGENTES HASTA 31/12/2018
                    sl.SetCellValue(11, 9, ResMontos.Invalidez_SolReaj19);//TABLAS DE MORTALIDAD VIGENTES A PARTIR DEL 01/01/2019

                    //--Soles Dolares
                    sl.SetCellValue(11, 11, ResMontos.Invalidez_Dolar); //TABLAS DE MORTALIDAD VIGENTES HASTA 31/12/2018
                    sl.SetCellValue(11, 12, ResMontos.Invalidez_Dolar19);//TABLAS DE MORTALIDAD VIGENTES A PARTIR DEL 01/01/2019


                    /*SOBREVIVENCIA*/
                    //--Soles Indexados
                    sl.SetCellValue(12, 5, ResMontos.Sobrevivencia_SolIndex);//TABLAS DE MORTALIDAD VIGENTES HASTA 31/12/2018
                    sl.SetCellValue(12, 6, ResMontos.Sobrevivencia_SolIndex19);//TABLAS DE MORTALIDAD VIGENTES A PARTIR DEL 01/01/2019

                    //--Soles Reajustados
                    sl.SetCellValue(12, 8, ResMontos.Sobrevivencia_SolReaj);//TABLAS DE MORTALIDAD VIGENTES HASTA 31/12/2018
                    sl.SetCellValue(12, 9, ResMontos.Sobrevivencia_SolReaj19);//TABLAS DE MORTALIDAD VIGENTES A PARTIR DEL 01/01/2019

                    //--Dolares
                    sl.SetCellValue(12, 11, ResMontos.Sobrevivencia_Dolar);//TABLAS DE MORTALIDAD VIGENTES HASTA 31/12/2018
                    sl.SetCellValue(12, 12, ResMontos.Sobrevivencia_Dolar19);//TABLAS DE MORTALIDAD VIGENTES A PARTIR DEL 01/01/2019
                    #endregion

                    sl.SaveAs(pPathFile);
                }
                else
                {
                    _log.Info("No se pudo consultar toda la información correctamente.");
                }

                return pPathFile;
            }
            catch (Exception ex)
            {
                _log.Info("Error en generación de Reporte de Resumen Adecuación, al insertar información en excel: " + ex.Message);
                return pPathFile;
            }
        }

        /// <summary>
        /// Insertar información consultada para Reporte Contable en el excel.
        /// José Hernández Alvarado.
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha del periodo abierto o periodo a buscar</param>
        /// <param name="pPathFile">Ruta temporal del archivo creado</param>
        /// <returns>Ruta temporal del archivo</returns>
        public string ExportarReporteContable(string pFechaPeriodo, string pPathFile)
        {
            XmlConfigurator.Configure();
            SLDocument sl = new SLDocument(pPathFile);
            List<ReportesReservas> lstReporteContable = new List<ReportesReservas>();
            try
            {
                _log.Info("Se consultará la información para Reporte Contable, por favor espere...");
                lstReporteContable = _reportesReservasRepository.ConsultarReporteContable(pFechaPeriodo, "SELECTREPORTECONTABLE"); //Reporte Contable

                if (lstReporteContable != null)
                {
                    _log.Info("Se ha consultado toda la información.");

                    #region Llenado de Excel
                    sl.SelectWorksheet("Diario"); //Hoja de Reporte Contable
                    for (int i = 0; i < lstReporteContable.Count; i++)
                    {
                        sl.SetCellValue(2 + i, 1, lstReporteContable[i].Paquete);
                        sl.SetCellValue(2 + i, 2, lstReporteContable[i].Asiento);
                        sl.SetCellValue(2 + i, 3, lstReporteContable[i].FechaContable);
                        sl.SetCellValue(2 + i, 4, lstReporteContable[i].TipoAsiento);
                        sl.SetCellValue(2 + i, 5, lstReporteContable[i].TipoContabilidad);
                        sl.SetCellValue(2 + i, 6, lstReporteContable[i].ClaseAsiento);
                        sl.SetCellValue(2 + i, 7, lstReporteContable[i].Fuente);
                        sl.SetCellValue(2 + i, 8, lstReporteContable[i].Referencia);
                        sl.SetCellValue(2 + i, 9, lstReporteContable[i].Contribuyente);
                        sl.SetCellValue(2 + i, 10, lstReporteContable[i].CentroCosto);
                        sl.SetCellValue(2 + i, 11, lstReporteContable[i].CuentaContable);
                        sl.SetCellValue(2 + i, 12, lstReporteContable[i].DebitoLocal);
                        sl.SetCellValue(2 + i, 13, lstReporteContable[i].CreditoLocal);
                        sl.SetCellValue(2 + i, 14, lstReporteContable[i].DebitoDolar);
                        sl.SetCellValue(2 + i, 15, lstReporteContable[i].CreditoDolar);
                        sl.SetCellValue(2 + i, 16, lstReporteContable[i].MontoUnidades);
                    }
                    #endregion
                }
                else
                {
                    _log.Info("No se pudo consultar toda la información correctamente.");
                }

                sl.SaveAs(pPathFile);

                return pPathFile;
            }
            catch (Exception ex)
            {
                _log.Info("Error en generación de Reporte Contable, al insertar información en excel: " + ex.Message);
                return pPathFile;
            }
        }
    }
}
