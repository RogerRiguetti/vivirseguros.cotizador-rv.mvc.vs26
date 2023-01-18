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

        // POST api/values
        public object Post([FromBody] CalculoExtraOficialRequest request)
        {
            var response = new Response();

            var idsBeneficiarios = new List<string>();
            var idsModalidades = new List<string>();
            char bandera = 'C';

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
                    Estado = 1
                };

                foreach (var ids in request.Beneficiario)
                {
                    idsBeneficiarios.Add(ids.IdBeneficiarioJubilare.ToString());
                }

                foreach (var ids in request.Modalidad)
                {
                    idsModalidades.Add(ids.IdModalidadJubilare.ToString());
                }

                response = _cotizacionLogic.RegistrarModificarCotizacion(bandera, cotizacion, idsBeneficiarios, idsModalidades);

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