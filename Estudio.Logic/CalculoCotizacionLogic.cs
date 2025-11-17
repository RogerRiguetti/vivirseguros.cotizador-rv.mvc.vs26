using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Estudio.Logic
{
    public class CalculoCotizacionLogic
    {
        CalculoCotizacionRepository _calculoCotizacionRepository = new CalculoCotizacionRepository();
        List<CalculoCotizaciones> ListaRep = new List<CalculoCotizaciones>();
        ExportarExcelRepository _exportarExcelRepository = new ExportarExcelRepository();
        CalcularAsignacionIntermediarioRepository _calcularAsignacionIntermediarioRepository = new CalcularAsignacionIntermediarioRepository();
        SISCORepository _SISCORepository = new SISCORepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// José Hernández Alvarado.
        /// 09-10-2018
        /// </summary>
        /// <param name="numArch">Número de archivo de XML leído.</param>
        /// <returns>Lista con registros de cotizaciones "Calculadas".</returns>
        public Response MostrarGridCalculadas(int numArch)
        {
            try
            {
                XmlConfigurator.Configure();

                Response res = new Response();
                List<SISCO> CotSISCO = new List<SISCO>();

                ListaRep = _calculoCotizacionRepository.RptCalculadas(numArch, "");
                var listBeneficiarios = _calculoCotizacionRepository.getBeneficiarios(numArch);
                string montoPenSISCO = "";
                string montoTasV = "";
                SISCO valSISCO = new SISCO();
                
                for (int b = 0; b < ListaRep.Count; b++)
                {
                    decimal suma = 0;
                    double monpen = 0;
                    ListaRep[b].Moneda = homologarMoneda(ListaRep[b].Moneda, ListaRep[b].codTipReajuste);

                    valSISCO = _SISCORepository.ConsultaSISCO(ListaRep[b].Num_Operacion.ToString(), ListaRep[b].CUSPP);
                    

                    int ind_sisco = 0;
                    
                    string SNCotiza = "S";


                    SISCO valSis = new SISCO();

                    _log.Info("Proceso de validacion despues de la carga de solicitud");
                    switch (ListaRep[b].Cod_TipPen)
                    {
                        case "08":
                            for (int i = 0; i < listBeneficiarios.Count; i++)
                            {
                                if (listBeneficiarios[i].Num_Corr == ListaRep[b].Num_Corr && listBeneficiarios[i].Num_Operacion == ListaRep[b].Num_Operacion)
                                {
                                    suma = suma + listBeneficiarios[i].Mto_Pension * (listBeneficiarios[i].prc_Pension / 100);
                                    ListaRep[b].Mto_Pension = suma;
                                }
                            }
                            break;
                        case "07":
                            if (ListaRep[b].Cob_Cony == "S")
                                ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension) * (0.5));
                            else
                                ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension));
                            break;
                        case "06":
                            if (ListaRep[b].Cob_Cony == "S")
                                ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension) * (0.7));
                            else
                                ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension));
                            break;
                        case "05":
                            ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension));
                            break;
                        case "04":
                            ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension));
                            break;
                    }
                    _log.Info("Pension (Pensiones para tipo de pension)" + ListaRep[b].Mto_Pension);
                    decimal mtosumpen = 0;
                    switch (ListaRep[b].Cod_TipRen)
                    {
                        case "1":
                            {
                                ListaRep[b].Mto_PensionRT = 0;
                                if (ListaRep[b].Moneda != "S/.Aj." && ListaRep[b].Moneda != "S/.")
                                {
                                    ListaRep[b].Prima_Unica = ListaRep[b].Prima_Unica * (float)(ListaRep[b].tipCambio);
                                }
                                else
                                {
                                    ListaRep[b].Prima_Unica = ListaRep[b].Prima_Unica;
                                }
                                if (valSISCO != null)
                                {
                                    if (valSISCO.SISCO_OK == 0)
                                    //if (ListaRep[b].Ind_SISCO == 1)
                                    {
                                        montoPenSISCO = valSISCO.PensionSISCO.ToString();
                                        montoTasV = valSISCO.TasaInteres.ToString();

                                        if (Convert.ToDouble(ListaRep[b].Mto_Pension) < Convert.ToDouble(montoPenSISCO))
                                        {
                                            ListaRep[b].Mto_Pension = Convert.ToDecimal(montoPenSISCO);
                                            mtosumpen = Convert.ToDecimal(montoPenSISCO);
                                            ind_sisco = 1;
                                        }
                                        else
                                        {
                                            ListaRep[b].Mto_Pension = ListaRep[b].Mto_Pension;
                                            ind_sisco = 0;
                                            SNCotiza = "S";
                                        }
                                    }
                                    else
                                        {
                                            if (valSISCO.SISCO_VS == 1)
                                            {
                                                ListaRep[b].Mto_Pension = ListaRep[b].Mto_Pension;
                                            }
                                        }
                                    }
                                    else {
                                        ListaRep[b].Mto_Pension = ListaRep[b].Mto_Pension;
                                    }
                                if (ind_sisco == 1)
                                {
                                    _log.Info("Pension (Pensiones para tipo de Renta vitalicia)" + mtosumpen);
                                    _log.Info("Número Correlativo " + ListaRep[b].Num_Corr);
                                     _log.Info("Número de Operacion " + ListaRep[b].Num_Operacion.ToString());
                                    _log.Info("Número de archivo " + Convert.ToInt32(ListaRep[b].Num_Archivo));
                                   // _calcularAsignacionIntermediarioRepository.PensionNueSisco(Convert.ToDecimal(montoPenSISCO), Convert.ToInt32(ListaRep[b].Num_Archivo), ListaRep[b].Num_Operacion.ToString(), ListaRep[b].Num_Corr, Convert.ToDecimal(montoTasV));
                                    _calcularAsignacionIntermediarioRepository.PensionValid(mtosumpen, Convert.ToInt32(ListaRep[b].Num_Archivo), ListaRep[b].Num_Operacion.ToString(), ListaRep[b].Num_Corr, 1);
                                    _log.Info("Se ejecuto correctamente el UPDATE a Suma pension" + mtosumpen);
                                }
                                else
                                {
                                    _log.Info("Pension (Pensiones para tipo de Renta vitalicia)" + ListaRep[b].Mto_Pension);
                                    _log.Info("Número Correlativo " + ListaRep[b].Num_Corr);
                                    _log.Info("Número de Operacion " + ListaRep[b].Num_Operacion.ToString());
                                    _log.Info("Número de archivo " + Convert.ToInt32(ListaRep[b].Num_Archivo));
                                    _calcularAsignacionIntermediarioRepository.PensionValid(ListaRep[b].Mto_Pension, Convert.ToInt32(ListaRep[b].Num_Archivo), ListaRep[b].Num_Operacion.ToString(), ListaRep[b].Num_Corr, 0);
                                    _log.Info("Se ejecuto correctamente el UPDATE a Suma pension" + ListaRep[b].Mto_Pension);
                                }
                                    ListaRep[b].Num_Orden = b + 1;
                                _calcularAsignacionIntermediarioRepository.ValSNCotza(Convert.ToInt32(ListaRep[b].Num_Archivo), ListaRep[b].Num_Operacion.ToString(), ListaRep[b].Num_Corr, SNCotiza);
                               
                                break;
                            }
                        case "2":
                            {
                                if (ListaRep[b].Moneda != "S/.Aj." && ListaRep[b].Moneda != "S/.")
                                {
                                    ListaRep[b].Mto_PensionRT = (ListaRep[b].Mto_Pension * 2) * (ListaRep[b].tipCambio);
                                    ListaRep[b].Prima_Unica = ListaRep[b].Prima_Unica * (float)(ListaRep[b].tipCambio);
                                }
                                else
                                {
                                    ListaRep[b].Mto_PensionRT = (ListaRep[b].Mto_Pension * 2);
                                    ListaRep[b].Prima_Unica = ListaRep[b].Prima_Unica;
                                }

                                _log.Info("Pension (Pensiones para tipo de Renta vitalicia con temporal)" + ListaRep[b].Mto_Pension);
                                _log.Info("Número Correlativo " + ListaRep[b].Num_Corr);
                                _log.Info("Número de Operacion " + ListaRep[b].Num_Operacion.ToString());
                                _log.Info("Número de archivo " + Convert.ToInt32(ListaRep[b].Num_Archivo));
                                _calcularAsignacionIntermediarioRepository.PensionValid(ListaRep[b].Mto_Pension, Convert.ToInt32(ListaRep[b].Num_Archivo), ListaRep[b].Num_Operacion.ToString(), ListaRep[b].Num_Corr, 0);
                                _log.Info("Se ejecuto correctamente el UPDATE a Suma pension" + ListaRep[b].Mto_Pension);
                                ListaRep[b].Num_Orden = b + 1;
                                break;
                            }

                        case "6":
                            {
                                if (ListaRep[b].Moneda != "S/.Aj." && ListaRep[b].Moneda != "S/.")
                                {
                                    //ListaRep[b].Mto_PensionRT = (ListaRep[b].Mto_Pension) * (ListaRep[b].tipCambio);
                                    ListaRep[b].Prima_Unica = ListaRep[b].Prima_Unica * (float)(ListaRep[b].tipCambio);
                                }
                                else
                                {
                                    
                                    ListaRep[b].Prima_Unica = ListaRep[b].Prima_Unica;
                                }
                                _log.Info("Pension (Pensiones para tipo de Renta Escalonada)" + ListaRep[b].Mto_Pension);
                                _log.Info("Número Correlativo " + ListaRep[b].Num_Corr);
                                _log.Info("Número de Operacion " + ListaRep[b].Num_Operacion.ToString());
                                _log.Info("Número de archivo " + Convert.ToInt32(ListaRep[b].Num_Archivo));
                                _calcularAsignacionIntermediarioRepository.PensionValid(ListaRep[b].Mto_Pension, Convert.ToInt32(ListaRep[b].Num_Archivo), ListaRep[b].Num_Operacion.ToString(), ListaRep[b].Num_Corr, 0);
                                _log.Info("Se ejecuto correctamente el UPDATE a Suma pension" + ListaRep[b].Mto_Pension);
                                ListaRep[b].Mto_PensionRT = (ListaRep[b].Mto_Pension);
                                ListaRep[b].Mto_Pension = (ListaRep[b].Mto_Pension * ListaRep[b].Renta_Esc)/100;
                                
                                ListaRep[b].Num_Orden = b + 1;
                                break;
                            }
                    }
                }

                res.Object = ListaRep;
                if (res.Object == null)
                {
                    List<CalculoCotizaciones> lista = new List<CalculoCotizaciones>();
                    CalculoCotizaciones InformacionIncial = new CalculoCotizaciones();
                    //lista.Add(InformacionIncial);
                    //res.Object = lista;
                    res.Message = "No se encontro información";
                }
                else
                {
                    res.Message = "Información cargada con éxito";
                }
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// José Hernández Alvarado.
        /// 09-10-2018
        /// </summary>
        /// <param name="numArch">Número de archivo de XML leído.</param>
        /// <returns>Lista con registros de cotizaciones "No Calculadas".</returns>
        public Response MostrarGridNoCalculadas(int numArch)
        {
            try
            {
                Response res = new Response();
                ListaRep = _calculoCotizacionRepository.RptNoCalculadas(numArch, "");

                for (int b = 0; b < ListaRep.Count; b++)
                {
                    ListaRep[b].Moneda = homologarMoneda(ListaRep[b].Moneda, ListaRep[b].codTipReajuste);
                    if (ListaRep[b].MensajeErr != "")
                    {
                        _calcularAsignacionIntermediarioRepository.ValSNCotza(Convert.ToInt32(ListaRep[b].Num_Archivo), ListaRep[b].Num_Operacion.ToString(), ListaRep[b].Num_Corr, "N");
                    }
                    ListaRep[b].Num_Orden = b + 1;
                }
                res.Object = ListaRep;
                if (res.Object == null)
                {
                    List<CalculoCotizaciones> lista = new List<CalculoCotizaciones>();
                    CalculoCotizaciones InformacionIncial = new CalculoCotizaciones();
                    //lista.Add(InformacionIncial);
                    //res.Object = lista;
                    res.Message = "No se encontro información";
                }
                else
                {
                    res.Message = "Información cargada con éxito";
                }
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }
        /// <summary>
        /// Omar Figureoa Flores
        /// 26-11-2018
        /// </summary>
        /// <param name="moneda">Codigo de la moneda ha homologar</param>
        /// <param name="reajuste">Codigo tipo reajuste para poder homologar la moneda</param>
        /// <returns>El valor de la moneda homologada.</returns>
        public string homologarMoneda(string moneda, string reajuste)
        {
            try
            {
                return _calculoCotizacionRepository.homologarMoneda(moneda, reajuste);
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        /// José Hernández Alvarado.
        /// 09-10-2018
        /// </summary>
        /// <param name="strNum">Número de archivo XML leído.</param>
        /// <param name="strNom">Nombre de archivo XML leído.</param>
        /// <returns>Lista con registros para generar reporte de cotizaciones "Calculadas".</returns>
        public List<CalculoCotizaciones> RptCalculadas(string strNum, string strNom)
        {
            try
            {
                ListaRep = _calculoCotizacionRepository.RptCalculadas(Convert.ToInt32(strNum), strNom);
                var listBeneficiarios = _calculoCotizacionRepository.getBeneficiarios(int.Parse(strNum));

                for (int b = 0; b < ListaRep.Count; b++)
                {
                    decimal suma = 0;
                    ListaRep[b].Moneda = homologarMoneda(ListaRep[b].Moneda, ListaRep[b].codTipReajuste);

                    switch (ListaRep[b].Cod_TipPen)
                    {
                        case "08":
                            for (int i = 0; i < listBeneficiarios.Count; i++)
                            {
                                if (listBeneficiarios[i].Num_Corr == ListaRep[b].Num_Corr && listBeneficiarios[i].Num_Operacion == ListaRep[b].Num_Operacion)
                                {
                                    suma = suma + listBeneficiarios[i].Mto_Pension * (listBeneficiarios[i].prc_Pension / 100);
                                    ListaRep[b].Mto_Pension = suma;
                                }
                            }
                            break;
                        case "07":
                            if (ListaRep[b].Cob_Cony == "S")
                                ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension) * (0.5));
                            else
                                ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension));
                            break;
                        case "06":
                            if (ListaRep[b].Cob_Cony == "S")
                                ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension) * (0.7));
                            else
                                ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension));
                            break;
                        case "05":
                            ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension));
                            break;
                        case "04":
                            ListaRep[b].Mto_Pension = Convert.ToDecimal(Convert.ToDouble(ListaRep[b].Mto_Pension));
                            break;
                    }
                    switch (ListaRep[b].Cod_TipRen)
                    {
                        case "1":
                            {
                                ListaRep[b].Mto_PensionRT = 0;
                                ListaRep[b].Prima_Unica = ListaRep[b].Prima_Unica;

                                ListaRep[b].Num_Orden = b + 1;
                                break;
                            }
                        case "2":
                            {
                                if (ListaRep[b].Moneda != "S/.Aj." && ListaRep[b].Moneda != "S/.")
                                {
                                    ListaRep[b].Mto_PensionRT = (ListaRep[b].Mto_Pension * 2) * (ListaRep[b].tipCambio);
                                    ListaRep[b].Prima_Unica = ListaRep[b].Prima_Unica * (float)(ListaRep[b].tipCambio);
                                }
                                else
                                {
                                    ListaRep[b].Mto_PensionRT = (ListaRep[b].Mto_Pension * 2);
                                    ListaRep[b].Prima_Unica = ListaRep[b].Prima_Unica;
                                }
                                ListaRep[b].Num_Orden = b + 1;
                                break;
                            }

                        case "6":
                            {
                                if (ListaRep[b].Moneda != "S/.Aj." && ListaRep[b].Moneda != "S/.")
                                {
                                    ListaRep[b].Mto_PensionRT = (ListaRep[b].Mto_Pension) * (ListaRep[b].tipCambio);
                                }
                                else
                                {
                                    ListaRep[b].Mto_PensionRT = (ListaRep[b].Mto_Pension);
                                }


                                ListaRep[b].Prima_Unica = ListaRep[b].Prima_Unica;

                                ListaRep[b].Num_Orden = b + 1;
                                break;
                            }

                            //ListaRep[b].Mto_PensionRT = ListaRep[b].Mto_Pension * 2;

                    }
                    //ListaRep[b].Mto_PensionRT = ListaRep[b].Mto_Pension * 2;
                    //ListaRep[b].Num_Orden = b + 1;
                }
                return ListaRep;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 09-10-2018
        /// </summary>
        /// <param name="strNum">Número de archivo XML leído.</param>
        /// <param name="strNom">Nombre de archivo XML leído.</param>
        /// <returns>Lista con registros para generar reporte de cotizaciones "No Calculadas".</returns>
        public List<CalculoCotizaciones> RptNoCalculadas(string strNum, string strNom)
        {
            try
            {
                ListaRep = _calculoCotizacionRepository.RptNoCalculadas(Convert.ToInt32(strNum), strNom);
                for (int b = 0; b < ListaRep.Count; b++)
                {
                    ListaRep[b].Moneda = homologarMoneda(ListaRep[b].Moneda, ListaRep[b].codTipReajuste);
                    ListaRep[b].Num_Orden = b + 1;
                }
                return ListaRep;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<List<Dictionary<string, object>>> ExportarExcelCalculadas(int num_Archivo)
        {
            return _exportarExcelRepository.ExportarExcelCalculadas(num_Archivo);
        }

        public List<List<Dictionary<string, object>>> ExportarExcelNoCalculadas(int num_Archivo)
        {
            return _exportarExcelRepository.ExportarExcelNoCalculadas(num_Archivo);
        }

        #region Consultas a BD para Excel de Solicitudes Calculadas y No Calculadas.
        /// <summary>
        /// José Hernández Alvarado.
        /// 05-08-2019
        /// Método para retornar lista con registros de consulta a BD para Excel de Solicitudes Calculadas con plantilla..
        /// </summary>
        /// <returns>Lista con registros de BD de Solicitudes Calculadas.</returns>
        public List<AsignacionIntermediario> ConsultaCalculadas(int num_Archivo)
        {
            XmlConfigurator.Configure();

            try
            {
                return _calculoCotizacionRepository.ExportarExcelCalculadas(num_Archivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la información de Solicitudes Calculadas en Repositorio: " + ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 05-08-2019
        /// Método para retornar lista con registros de consulta a BD para Excel de Solicitudes No Calculadas con plantilla..
        /// </summary>
        /// <returns>Lista con registros de BD de Solicitudes No Calculadas.</returns>
        public List<AsignacionIntermediario> ConsultaNolculadas(int num_Archivo)
        {
            XmlConfigurator.Configure();
            
            try
            {
                return _calculoCotizacionRepository.ExportarExcelNoCalculadas(num_Archivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la información de Solicitudes No Calculadas : " + ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion

    }
}
