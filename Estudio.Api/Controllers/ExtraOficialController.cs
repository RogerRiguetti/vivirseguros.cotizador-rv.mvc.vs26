using Estudio.Api.Request;
using Estudio.Api.Static;
using Estudio.Api.Validator;
using Estudio.Logic;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Estudio.Controllers.Controllers;
using System.Globalization;

namespace Estudio.Api.Controllers
{
    public class ExtraOficialController : ApiController
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        CalculoExtraOficialValidator _calculoEOValidator = new CalculoExtraOficialValidator();
        CotizacionLogic _cotizacionLogic = new CotizacionLogic();
        ModalidadesLogic _modalidadesLogic = new ModalidadesLogic();
        BeneficiariosLogic _beneficiariosLogic = new BeneficiariosLogic();
        CotizacionController _CotizacionController = new CotizacionController();


        // POST api/values
        public object Post([FromBody] CalculoExtraOficialRequest request)
        {
            var response = new Response();

            var idsBeneficiarios = new List<string>();
            var idsModalidades = new List<string>();
            string bandera = "C";

            //Modalidades
            int idModalidad = 0;

            try
            {
                var validationRules = _calculoEOValidator.validator(request);

                if (validationRules.Count() > 0)
                {
                    response.IsOk = false;
                    response.Message = "Errores de validación.";
                    response.Errors = validationRules;
                    return response;
                }

                var IdAsesor = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdAsesor", request.Asesor);
                var IdSexo = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdSexo", request.Asegurado.Genero);
                var IdTipoDocumento = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdTipoDocumento", request.Asegurado.NombreDocumento);
                var IdDepartamento = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdDepartamento", request.Asegurado.Departamento);
                var IdProvincia = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdProvincia", request.Asegurado.Provincia);
                var IdDistrito = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdDistrito", request.Asegurado.Distrito);
                var IdAfp = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdAfp", request.Asegurado.TipoAFP);
                var IdPension = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdPension", request.Asegurado.TipoPension);
                var CodigoPension = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("CodigoPension", request.Asegurado.TipoPension);
                var PorAfp = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("PorAfp", request.Asegurado.TipoAFP);

                // Declarar una lista para almacenar los ObjetoModalidad
                List<string> ModalidadIDs = new List<string>();

                foreach (var mod in request.Modalidad)
                {
                    var IdMoneda = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdMoneda", mod.Moneda);
                    var AniosDiferidos = mod.PeriodoDiferido.ToString();
                    var PeriodoGarantizado = mod.PeriodoGarantizado.ToString();
                    var Gratificacion = mod.Gratificacion;
                    var IdModalidadCat = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdModalidadCat", mod.TipoModalidad);
                    var IdTipoRenta = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdTipoRenta", mod.TipoRenta);
                    var PorcentajeRentaTemporal = 50;//mod.RentaTemp;
                    var PorcentajeRentabilidadAfp = decimal.Parse(PorAfp);//mod.TasaRentaAFP;
                    var SegundoTramo = mod.SegundoTramo;
                    var PrimerTramo = mod.PrimerTramo;

                    if (IdMoneda == "00" || IdModalidadCat == "00" || IdTipoRenta == "00")
                    {
                        response.IsOk = false;
                        response.Message = CatalogoErrores.Cotizacion00;
                        return response;
                    }

                    var modalidad = new Repository.Core.Domain.Modalidad
                    {
                        AniosDiferidos = int.Parse(AniosDiferidos),
                        AniosGarantizados = int.Parse(PeriodoGarantizado),
                        Gratificacion = Gratificacion,
                        IdComision = 8,
                        IdModalidadCat = int.Parse(IdModalidadCat),
                        IdMoneda = int.Parse(IdMoneda),
                        IdTipoRenta = int.Parse(IdTipoRenta),
                        PorcentajeRentaTemporal = PorcentajeRentaTemporal,
                        PorcentajeRentabilidadAfp = PorcentajeRentabilidadAfp,
                        PrimerTramo = PrimerTramo,
                        SegundoTramo = SegundoTramo,
                    };

                    var ObjetoModalidad = _modalidadesLogic.RegistrarModificarModalidad(bandera, idModalidad, modalidad);

                    if (ObjetoModalidad.IsOk)
                    {
                        // Suponiendo que IdModalidad es una propiedad del objeto devuelto
                        int idModalidadDevuelto = (int)ObjetoModalidad.Object.GetType().GetProperty("IdModalidad").GetValue(ObjetoModalidad.Object);
                        ModalidadIDs.Add(idModalidadDevuelto.ToString());
                    }

                }


                //Beneficiarios
                //necesito guardar primero el asegurado

                // PASO 1  - GUARDAR ASEGURADO EN LA TABLA DE BENEFICIARIO

                // Crear una lista para almacenar los IdBeneficiario
                List<string> BenficiariosIDs = new List<string>();

                var Aseg = request.Asegurado;

                var NombresAseg = Aseg.Nombres;
                var ApellidoPaternoAseg = Aseg.ApellidoPaterno;
                var ApellidoMaternoAseg = Aseg.ApellidoMaterno;
                var idParentescoAseg = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdParentescoAsegurado", "99");
                var IdPensionAsegurado = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdPension", Aseg.TipoPension); //lo quiero para la funcion de abajo !!
                var FechaNacimientoAseg = Aseg.FechaNacimiento;
                var IdTipoDocumentoBeneficiarioAseg = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdTipoDocumento", Aseg.NombreDocumento);
                var DocumentoAseg = Aseg.NumeroDocumento;
                var IdSexoBeneficiarioAseg = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdSexo", Aseg.Genero);
                var IdSituacionInvalidezBeneficiarioAseg = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdSituacionInvalidez", Aseg.TipoInvalidez);
                string fechaFallecimiento = null;
                var FechaDevengueAsegurado = request.Asegurado.FechaDevengue;


                if (request.Asegurado.TipoPension == "SOBREVIVENCIA")
                {
                    fechaFallecimiento = FechaDevengueAsegurado.ToString("dd/MM/yyyy");
                }


                // Crear un objeto que contenga todos los datos del asegurado
                var asegurado = new Repository.Core.Domain.Beneficiario
                {
                    Nombres = Aseg.Nombres,
                    Apellidos = $"{ApellidoPaternoAseg} {ApellidoMaternoAseg}",
                    IdParentesco = int.Parse(idParentescoAseg),
                    FechaNacimiento = FechaNacimientoAseg,
                    IdTipoDocumento = int.Parse(IdTipoDocumentoBeneficiarioAseg),
                    Documento = DocumentoAseg,
                    IdSexo = int.Parse(IdSexoBeneficiarioAseg),
                    IdSituacionInvalidez = int.Parse(IdSituacionInvalidezBeneficiarioAseg),
                    FechaFallecimientoStr = fechaFallecimiento
                };


               var ObjetoAsegurado = _beneficiariosLogic.RegistrarInsertBeneficiario(bandera, asegurado);

                if (ObjetoAsegurado is Repository.Core.Domain.Beneficiario beneficiarioResult)
                {

                   var AseguradoID = beneficiarioResult.IdBeneficiario;

                   BenficiariosIDs.Add(Convert.ToString(AseguradoID));
                }

                // PASO 2 - OBTENER LOS BENEFICIARIOS 
                var beneficiarioList = new List<Repository.Core.Domain.Beneficiario>();

                foreach (var benf in request.Beneficiario)
                {
                    // var beneficiario que es la estructura
                    var Nombres = benf.Nombres;
                    var ApellidoPaterno = benf.ApellidoPaterno;
                    var ApellidoMaterno = benf.ApellidoMaterno;
                    var idParentesco = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdParentescoBeneficiario", benf.Parentesco);
                    var FechaNacimiento = benf.FechaNacimiento;
                    var IdTipoDocumentoBeneficiario = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdTipoDocumento", benf.NombreDocumento);
                    var Documento = benf.NumeroDocumento;
                    var IdSexoBeneficiario = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdSexo", benf.Genero);
                    var IdSituacionInvalidezBeneficiario = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdSituacionInvalidez", benf.TipoInvalidez);

                    var beneficiario = new Repository.Core.Domain.Beneficiario
                    {
                        Nombres = Nombres,
                        Apellidos = $"{ApellidoPaterno} {ApellidoMaterno}",
                        IdParentesco = int.Parse(idParentesco),
                        FechaNacimiento = FechaNacimiento,
                        IdTipoDocumento = int.Parse(IdTipoDocumentoBeneficiario),
                        Documento = Documento,
                        IdSexo = int.Parse(IdSexoBeneficiario),
                        IdSituacionInvalidez = int.Parse(IdSituacionInvalidezBeneficiario),
                        FechaFallecimientoStr = "",
                        PorcentajeBen = "0",
                    };

                    ObjetoAsegurado = _beneficiariosLogic.RegistrarInsertBeneficiario(bandera, beneficiario);

                    if (ObjetoAsegurado is Repository.Core.Domain.Beneficiario beneficiarioResulta)
                    {

                        var AseguradoID = beneficiarioResulta.IdBeneficiario;

                        BenficiariosIDs.Add(Convert.ToString(AseguradoID));
                    }
                
                }

                var BenfeDatos = _CotizacionController.PorcentajesBeneficiarios(BenficiariosIDs, int.Parse(IdPensionAsegurado), fechaFallecimiento, FechaDevengueAsegurado.ToString("dd/MM/yyyy"));
                
       
                List<string> Benf_prc_pension = new List<string>();

                if (BenfeDatos is JsonResult jsonResult && jsonResult.Data != null)
                {
                    dynamic datos = jsonResult.Data;
                    var ObjectData = datos.Object;

                    if (ObjectData != null && ObjectData is IEnumerable<object>)
                    {
                        foreach (var elemento in ObjectData)
                        {
                            if (elemento.PorcentajeBenDbl != null && elemento.PorcentajeBenDbl is double)
                            {
                                Benf_prc_pension.Add(elemento.PorcentajeBenDbl.ToString());
                            }
                        }
                    }
                }


                if (IdAsesor == "00" || IdSexo == "00" || IdTipoDocumento == "00" || IdDepartamento == "00" || IdProvincia == "00" ||
                    IdDistrito == "00" || IdAfp == "00" || IdPension == "00" || CodigoPension == "00" || PorAfp == "00")
                {
                    response.IsOk = false;
                    response.Message = CatalogoErrores.Cotizacion00;
                    return response;
                }


                // Crear una lista para guardar todos los IdBeneficiarioJubilare
                List<int> idBeneficiariosJubilare = new List<int>();

                // Agregar el IdBeneficiarioJubilare del asegurado
                idBeneficiariosJubilare.Add(request.Asegurado.IdBeneficiarioJubilare);

                // Agregar los IdBeneficiarioJubilare de los beneficiarios
                idBeneficiariosJubilare.AddRange(request.Beneficiario.Select(b => b.IdBeneficiarioJubilare));


                var cotizacion = new Cotizacion
                {

                    IdCotizacion = request.IdCotizacionJubilare,
                    Documento = request.Asegurado.NumeroDocumento,
                    CUSPP = request.Asegurado.CUSPP,
                    Nombres = request.Asegurado.Nombres,
                    ApellidoPaterno = request.Asegurado.ApellidoPaterno,
                    ApellidoMaterno = request.Asegurado.ApellidoMaterno,
                    FechaNacimiento = request.Asegurado.FechaNacimiento,
                    FechaNacimientoStr = null,
                    Cic = request.MontoCIC,
                    FechaDevengue = request.Asegurado.FechaDevengue,
                    FechaDevengueStr = null,
                    FechaEstudio = DateTime.Now,
                    FechaEstudioStr = null,
                    GastoSepelio = decimal.Parse(_cotizacionLogic.ConsultaGastoSepelio()),//request.GastoSepelio,
                    TipoCambio = _cotizacionLogic.ConsultaTipoCambio(),//request.TipoCambio,
                    FechaCotizacion = DateTime.Now,
                    FechaCotizacionStr = null,
                    IdAsesor = int.Parse(IdAsesor),
                    Asesor = request.Asesor,
                    IdSexo = int.Parse(IdSexo),
                    IdTipoDocumento = int.Parse(IdTipoDocumento),
                    TipoDocumento = request.Asegurado.NombreDocumento,
                    IdDepartamento = int.Parse(IdDepartamento),
                    IdProvincia = int.Parse(IdProvincia),
                    IdDistrito = int.Parse(IdDistrito),
                    IdAfp = int.Parse(IdAfp),
                    IdPension = int.Parse(IdPension),
                    Afp = request.Asegurado.TipoAFP,
                    CodigoPension = CodigoPension,
                    ClaveSexo = request.Asegurado.Genero,
                    PorAfp = PorAfp,
                    Estado = 0,
                    IdCotizacionjubilare = new int[] { request.IdCotizacionJubilare },
                    IdModalidadjubilare = request.Modalidad.Select(m => m.IdModalidadJubilare).ToArray(),
                    IdBeneficiariojubilare = idBeneficiariosJubilare.ToArray(),
                    Benf_prc_pension = Benf_prc_pension.ToArray(),
                    Tipo_Pension = request.Asegurado.TipoPension,
                };

                response = _cotizacionLogic.RegistrarModificarCotizacionDetalle(bandera, cotizacion, BenficiariosIDs, ModalidadIDs);

                return Json(response);
            }
            catch (Exception ex)
            {
                _log.Info("Error en el cálculo extra oficial" + ex.Message);
                return null;
            }
        }

    }

}

