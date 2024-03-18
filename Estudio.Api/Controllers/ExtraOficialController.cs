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

namespace Estudio.Api.Controllers
{
    public class ExtraOficialController : ApiController
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        CalculoExtraOficialValidator _calculoEOValidator = new CalculoExtraOficialValidator();
        CotizacionLogic _cotizacionLogic = new CotizacionLogic();
        ModalidadesLogic _modalidadesLogic = new ModalidadesLogic();

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

                foreach (var mod in request.Modalidad)
                {
                    var IdMoneda = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdMoneda", mod.Moneda);
                    var AniosDiferidos = mod.PeriodoDiferido.ToString();
                    var PeriodoGarantizado = mod.PeriodoGarantizado.ToString();
                    var Gratificacion = mod.Gratificacion;
                    var IdModalidadCat = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdModalidadCat", mod.TipoModalidad);
                    var IdTipoRenta = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdTipoRenta", mod.TipoRenta);
                    var PorcentajeRentaTemporal = mod.RentaTemp;
                    var PorcentajeRentabilidadAfp = mod.TasaRentaAFP;

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
                        PorcentajeRentabilidadAfp = PorcentajeRentabilidadAfp
                    };

                    _modalidadesLogic.RegistrarModificarModalidad(bandera, idModalidad, modalidad);

                    idsModalidades.Add(idModalidad.ToString());
                }

                // aqui hay que guardar los benefciarios 
                //bandera="C" creada , "U" actuliza
                // var beneficiario que es la estructura
                //traer ID de PARENTESCO
                //var idParentesco=traer el ID del parentescto que llega en el request.beneficiario.Parentesco Crear la bandera Parentesto
                //var IdTipoDocumento = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdTipoDocumento", request.beneficiario.NombreDocumento);
                //var IdSexo = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdSexo", request.beneficiario.Genero);

                //crear la bandera en el SP "CO_ConsultasCotizacionesExtraOficial"  IdInvalidez usar la tabla SituacionesInvalidez
                //var idInvalidez = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdInvalidez", request.beneficiario.TipoInvalidez);

                //el array
                //beneficiario:

                /*  
                  public string Nombres { get; set; }
                  public string Apellidos { get; set; }
                  public string Documento { get; set; }
                  public DateTime FechaNacimiento { get; set; }
                  public string Sexo { get; set; }
                  public int IdParentesco { get; set; }
                  public int IdTipoDocumento { get; set; }
                  public int IdSituacionInvalidez { get; set; }
                  public DateTime FechaFallecimiento { get; set; }
                  public string PorcentajeBen { get; set; }*/
                //IdBeneficiarioJubilare

                //_modalidadesLogic.RegistrarModificarBeneficiarios(bandera, beneficiario); el procediminto Almacenado es VCE_CatalogoBeneficiarios


                if (IdAsesor == "00" || IdSexo == "00" || IdTipoDocumento == "00" || IdDepartamento == "00" || IdProvincia == "00" ||
                    IdDistrito == "00" || IdAfp == "00" || IdPension == "00" || CodigoPension == "00" || PorAfp == "00")
                {
                    response.IsOk = false;
                    response.Message = CatalogoErrores.Cotizacion00;
                    return response;
                }

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
                    GastoSepelio = request.GastoSepelio,
                    TipoCambio = request.TipoCambio,
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
                    Estado = 0
                };

                //hacer la consulta para traer todos los ID de beneficiario y guardarlo en el array  idsBeneficiarios SP VCE_CatalogoBeneficiarios buscare en la bandera @pBandera = 'B' campo idBeneficiario
                //foreach (var ids in request.Beneficiario)
                //{
                //    idsBeneficiarios.Add(ids.IdBeneficiarioJubilare.ToString());
                //}

                //hacer una consulta de los ID de modalidades y guardarlo en el Array idsModalidades SP VCE_CatalogoModalidades la bandera @pBandera = 'M' campo IdModalidad
                //foreach (var ids in request.Modalidad)
                //{
                //    idsModalidades.Add(ids.IdModalidadJubilare.ToString());
                //}
                // esto debe cambiar 



                response = _cotizacionLogic.RegistrarModificarCotizacionDetalle(bandera, cotizacion, idsBeneficiarios, idsModalidades);

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