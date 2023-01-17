using Estudio.WebService.Requests;
using Estudio.WebService.Static;
using System.Collections.Generic;

namespace Estudio.WebService.Validator
{
    public class CalculoExtraOficialValidator
    {

        public List<string> validator(CalculoExtraOficialRequest request)
        {
            var errores = new List<string>();

            #region Asegurados

            if (string.IsNullOrEmpty(request.Asegurado.NumeroDocumento))
            {
                errores.Add(CatalogoErrores.Cotizacion03);
            }

            if (string.IsNullOrEmpty(request.Asegurado.FechaNacimiento.ToString()))
            {
                errores.Add(CatalogoErrores.Cotizacion08);
            }

            if (request.MontoCIC.Equals(""))
            {
                errores.Add(CatalogoErrores.Cotizacion16);
            }

            if (string.IsNullOrEmpty(request.Asegurado.Departamento))
            {
                errores.Add(CatalogoErrores.Cotizacion54);
            }

            if (string.IsNullOrEmpty(request.Asegurado.Provincia))
            {
                errores.Add(CatalogoErrores.Cotizacion55);
            }

            if (string.IsNullOrEmpty(request.Asegurado.Distrito))
            {
                errores.Add(CatalogoErrores.Cotizacion56);
            }

            if (string.IsNullOrEmpty(request.Asegurado.Genero))
            {
                errores.Add(CatalogoErrores.Cotizacion07);
            }

            if (string.IsNullOrEmpty(request.Asegurado.Genero))
            {
                errores.Add(CatalogoErrores.Cotizacion07);
            }

            if (string.IsNullOrEmpty(request.Asegurado.TipoAFP))
            {
                errores.Add(CatalogoErrores.Cotizacion77);
            }

            if (string.IsNullOrEmpty(request.Asegurado.TipoPension))
            {
                errores.Add(CatalogoErrores.Cotizacion78);
            }

            if (string.IsNullOrEmpty(request.Asegurado.NombreDocumento))
            {
                errores.Add(CatalogoErrores.Cotizacion02);
            }

            #endregion

            #region Cotizaciones

            if (string.IsNullOrEmpty(request.Asesor))
            {
                errores.Add(CatalogoErrores.Cotizacion79);
            }

            #endregion

            #region Beneficiarios

            foreach (var bene in request.Beneficiario)
            {
                if (string.IsNullOrEmpty(bene.FechaNacimiento.ToString()))
                {
                    errores.Add(CatalogoErrores.Cotizacion11);
                }

                if (string.IsNullOrEmpty(bene.Genero))
                {
                    errores.Add(CatalogoErrores.Cotizacion12);
                }

                if (string.IsNullOrEmpty(bene.Parentesco))
                {
                    errores.Add(CatalogoErrores.Cotizacion13);
                }

                if (string.IsNullOrEmpty(bene.TipoInvalidez))
                {
                    errores.Add(CatalogoErrores.Cotizacion10);
                }
            }

            if (request.Asegurado.PorcentajeBeneficiario.Equals(""))
            {
                errores.Add(CatalogoErrores.Cotizacion80);
            }

            #endregion

            #region Modalidad

            foreach (var modal in request.Modalidad)
            {
                if (modal.TasaRentaAFP.Equals(""))
                {
                    errores.Add(CatalogoErrores.Cotizacion24);
                }

                if (modal.PrimerTramo.Equals(""))
                {
                    errores.Add(CatalogoErrores.Cotizacion18);
                }

                if (modal.SegundoTramo.Equals(""))
                {
                    errores.Add(CatalogoErrores.Cotizacion19);
                }

                if (modal.PeriodoDiferido.Equals(""))
                {
                    errores.Add(CatalogoErrores.Cotizacion22);
                }

                if (modal.PeriodoGarantizado.Equals(""))
                {
                    errores.Add(CatalogoErrores.Cotizacion23);
                }

                if (modal.RentaTemp.Equals(""))
                {
                    errores.Add(CatalogoErrores.Cotizacion82);
                }

                if (string.IsNullOrEmpty(modal.Moneda))
                {
                    errores.Add(CatalogoErrores.Cotizacion20);
                }

                if (string.IsNullOrEmpty(modal.TipoRenta))
                {
                    errores.Add(CatalogoErrores.Cotizacion28);
                }

                if (string.IsNullOrEmpty(modal.TipoModalidad))
                {
                    errores.Add(CatalogoErrores.Cotizacion81);
                }
            }

            #endregion

            return errores;
        }
    }
}