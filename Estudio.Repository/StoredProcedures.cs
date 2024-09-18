using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository
{
    public class StoredProcedures
    {
        // VCEstudioOficiales
        public const string VCE_CatalogoPerfiles = "VCE_CatalogoPerfiles";
        public const string VCE_CatalogoUsuarios = "VCE_CatalogoUsuarios";
        public const string VCE_ValidaLogin = "VCE_ValidaLogin";
        public const string VCE_Catalogos = "VCE_Catalogos";
        public const string VCE_ObtenerPermisos = "VCE_ObtenerPermisos";
        public const string VCE_ConsultasCotizacion = "VCE_ConsultasCotizacion";
        public const string VCE_CatalogoBeneficiarios = "VCE_CatalogoBeneficiarios";
        public const string VCE_CatalogoModalidades = "VCE_CatalogoModalidades";
        public const string VCE_CatalogoCotizaciones = "VCE_CatalogoCotizaciones";
        public const string VCE_ClonacionCotizaciones = "VCE_ClonacionCotizaciones";
        public const string VCE_CatalogoParametros = "VCE_CatalogoParametros";
        public const string VCE_CatalogoRutina = "VCE_CatalogoRutina";
        public const string VCE_ConsultasDirecciones = "VCE_ConsultasDirecciones";
        public const string VCE_Consulta_TipoPension = "VCE_Consulta_TipoPension";
        public const string VCE_ConsultaSituacionesInvalidez = "VCE_ConsultaSituacionesInvalidez";


        // SeguroRV
        public const string VCE_ConsultasRutinas = "VCE_ConsultasRutinas";
        public const string VCE_CatalogoClave = "VCE_CatalogoClave";
        public const string VCE_ConsultaCUSPP = "VCE_ConsultaCUSPP";
        public const string VCE_ConsultaValidacionesModalidades = "VCE_ConsultaValidacionesModalidades";

        //Cotizador Oficiales
        public const string CO_CatalogosSeguros = "CO_CatalogosSeguros";
        public const string CO_CatalogosTasaAnclaje = "CO_CatalogosTasaAnclaje";
        public const string CO_CatalogosTasaMercadoHistoricas = "CO_CatalogosTasaMercadoHistoricas";
        public const string CO_CatalogosGastosSepelio = "CO_CatalogosGastosSepelio";
        public const string CO_CatalogosValoresMonedaMensual = "CO_CatalogosValoresMonedaMensual";
        public const string CO_CatalogosMantenedorIPC = "CO_CatalogosMantenedorIPC";
        public const string CO_CatalogosLimiteCotizacionMejorada = "CO_CatalogosLimiteCotizacionMejorada";
        public const string CO_CatalogosLimiteCotizacionInicial = "CO_CatalogosLimiteCotizacionInicial";
        public const string CO_ValoresMoneda = "CO_CatalogosValoresMoneda";
        public const string CO_ConsultasTasaCalce = "CO_ConsultasTasaCalce";
        public const string CO_CatalogoTasaCalce = "CO_CatalogoTasaCalce";
        public const string CO_ConsultasTasaRentabilidad = "CO_ConsultasTasaRentabilidad";
        public const string CO_CatalogoTasaRentabilidad = "CO_CatalogoTasaRentabilidad";
        public const string CO_CatalogoParametrosGasto = "CO_CatalogosParametrosGastos";
        public const string CO_ActualizacionDetCotizacion = "CO_ActualizacionDetCotizacion";
        public const string CO_CatalogosTasaVentaProm = "CO_CatalogosTasaVentaProm";

        //ExtraOficial
        public const string CO_ConsultasCotizacionesExtraOficial = "CO_ConsultasCotizacionesExtraOficial";

        //carga de archivo
        public const string CO_CatalogosCalcularAsignacionIntermediario = "CO_CatalogosCalcularAsignacionIntermediario";
        public const string CO_CatalogoCargaSolicitud = "CO_CatalogosCargaSolicitudes";

        //ProCarArchivo
        public const string CO_CatalogosProCarArchivo = "CO_CatalogosProCarArchivo";
        public const string CO_CatalogosReportesProCarArchivo = "CO_CatalogosReportesProCarArchivo";
        
        //Rutina
        public const string CO_ConsultasCotizacionesOficiales = "CO_ConsultasCotizacionesOficiales";
        public const string CO_ConsultasRutinaOficiales = "CO_ConsultasRutinaOficiales";
        //WebService
        public const string CO_WebService = "CO_WebService";
        //Excepciones
        public const string CO_CatalogosExcepciones = "CO_CatalogosExcepciones";
        //GenArchMeller
        public const string CO_ConsultasProEnvioCotizaciones = "CO_ConsultasProEnvioCotizaciones";
        //Mantenedor de fechas
        public const string CO_CatalogoManFecAceptacion = "CO_CatalogoManFecAceptacion";
        public const string CO_CatalogosLimiteCotizacionExtraOficial = "CO_CatalogosLimiteCotizacionExtraOficial";

        //SISCO
        public const string CO_ConsultasSISCO = "CO_ConsultasSISCO";

        //REPORTE GANADOS
        public const string CO_ConsultasReporteGanados = "CO_ConsultasReporteGanados";

        public const string CO_ConsultasMantenedorFiltros = "CO_ConsultasMantenedorFiltros";
        public const string CO_ConsultasFiltrosSNCotiza = "CO_ConsultasFiltrosSNCotiza";

        //Eliminar cargas
        public const string CO_ConsultasEliminaCargas = "CO_ConsultasEliminaCargas";

        //Modifica informacion de la rutina
        public const string CO_CatalogoActualizaInfo = "CO_CatalogoActualizaInfo";

        //RESERVAS
        public const string CO_ConsultasReservas = "CO_ConsultasReservas";
        public const string CO_ConsultasRutinaReservas = "CO_ConsultasRutinaReservas";
        public const string CR_ConsultasReportesReservas = "CR_ConsultasReportesReservas";

        public const string CR_ConsultasReservas = "CR_ConsultasReservas";
        public const string CR_ConsultasRutinaReservas = "CR_ConsultasRutinaReservas";


        //MANTENEDOR PERFILES
        public const string VCE_CatalogoMantenedorPerfiles = "VCE_CatalogoMantenedorPerfiles";
        public const string ReporteGanados = "ReporteGanados";


        // JUBILARE
        public const string usp_Jub_Sel_CarteraCompleta = "usp_Jub_Sel_CarteraCompleta";
        //public const string usp_Jub_Sel_CarteraCompleta_Paginado = "usp_Jub_Sel_CarteraCompleta_Paginado";
        public const string usp_Jub_Sel_CarteraCompleta_Paginado = "usp_Jub_Sel_CarteraCompleta_Paginado2";
        public const string usp_Sel_JubilarePlanilla = "usp_Sel_JubilarePlanilla";
    }
}
