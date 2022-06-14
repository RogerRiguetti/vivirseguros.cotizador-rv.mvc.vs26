using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class TasaVentaProRepository
    {
        public List<TasaVentaPro> TiposMoneda()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TIPOSMONEDA", ParameterDirection.Input));
                return VCEDBContext<TasaVentaPro>.CallStoreProcedure(StoredProcedures.CO_CatalogosSeguros, parameters, x => new TasaVentaPro
                {
                    ClaveMoneda = x.GetString(0) + "#" + x.GetString(1),//cod_moneda+cod_tipreajuste
                    Elemento = x.GetString(2) + " - " + x.GetString(3)//cod_scomp+gls_descripcion
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public List<TasaVentaPro> RangosTasa()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CMBRANGOTASA", ParameterDirection.Input));

                return VCEDBContext<TasaVentaPro>.CallStoreProcedure(StoredProcedures.CO_CatalogosLimiteCotizacionMejorada, parameters, x => new TasaVentaPro
                {
                    IdRangoTasa = x.GetString(0),
                    Elemento = x.GetString(0) + " - " + x.GetString(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public List<TasaVentaPro> CargarTabla(string vlMoneda, string tipReajuste, string cod_Pres)
        {
            try
            {
                //string query = "";
                //var moneda = vlMoneda.Split('#');
                //query = "SELECT FEC_VTAPROM, MTO_VTAPROM FROM SeguroRV.dbo.PT_TVAL_VTAPROM WHERE COD_MONEDA ='" + moneda[0] + "' AND COD_TIPPENSION = '" + cod_Pres + "'  AND COD_TIPPREAJUSTE = "+moneda[1];

                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTAVTAS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPension", SqlDbType.VarChar, cod_Pres, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipReajuste", SqlDbType.VarChar, tipReajuste, ParameterDirection.Input));

                return VCEDBContext<TasaVentaPro>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaVentaProm, parameters, x => new TasaVentaPro
                {
                    //Mes = x.GetString(0) == "" ? "" : (x.GetString(0).Substring(6, 2) + "/" + x.GetString(0).Substring(4, 2) + "/" + x.GetString(0).Substring(0, 4)),
                    Mes = x.GetString(0).Substring(0, 4) + "/" + x.GetString(0).Substring(4, 2),
                    Prom = x.GetDecimal(1)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public TasaVentaPro Guardar(string vlMoneda, string tipReajuste, string cod_Pres, decimal prom, string fecha)
        {
            try
            {
                //string query = "";
                //var moneda = vlMoneda.Split('#');
                //query = "UPDATE SeguroRV.dbo.PT_TVAL_VTAPROM SET MTO_VTAPROM = " + prom + " WHERE COD_MONEDA ='" + moneda[0] + "' AND COD_TIPPENSION = '" + cod_Pres + "'  AND COD_TIPPREAJUSTE = " + moneda[1] + " AND FEC_VTAPROM = '" + fecha + "'";
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "MODIFICARVTA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPension", SqlDbType.VarChar, cod_Pres, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipReajuste", SqlDbType.VarChar, tipReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecVta", SqlDbType.VarChar, fecha, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMtoProm", SqlDbType.Decimal, prom, ParameterDirection.Input));

                return VCEDBContext<TasaVentaPro>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaVentaProm, parameters, x => new TasaVentaPro
                { }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 02-01-2018
        /// </summary>
        /// <param name="vlMoneda"></param>
        /// <param name="tipReajuste"></param>
        /// <param name="cod_Pres"></param>
        /// <param name="fechaTasa"></param>
        /// <returns></returns>
        public List<TasaVentaPro> BuscarTasa(string vlMoneda, string tipReajuste, string cod_Pres, string fechaTasa)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARVTA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPension", SqlDbType.VarChar, cod_Pres, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipReajuste", SqlDbType.VarChar, tipReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecVta", SqlDbType.VarChar, fechaTasa, ParameterDirection.Input));

                return VCEDBContext<TasaVentaPro>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaVentaProm, parameters, x => new TasaVentaPro
                {
                    Mes = x.GetString(3).Substring(4, 2) + "/" + x.GetString(3).Substring(0, 4),
                    Prom = x.GetDecimal(4)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 02-01-2018
        /// </summary>
        /// <param name="vlMoneda"></param>
        /// <param name="tipReajuste"></param>
        /// <param name="cod_Pres"></param>
        /// <param name="fecha"></param>
        /// <param name="prom"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public TasaVentaPro NuevaTasaVta(string vlMoneda, string tipReajuste, string cod_Pres, string fecha, decimal prom, string usuario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "INSERTARVTA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMoneda", SqlDbType.VarChar, vlMoneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPension", SqlDbType.VarChar, cod_Pres, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pTipReajuste", SqlDbType.VarChar, tipReajuste, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pFecVta", SqlDbType.VarChar, fecha, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMtoProm", SqlDbType.Decimal, prom, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));

                return VCEDBContext<TasaVentaPro>.CallStoreProcedure(StoredProcedures.CO_CatalogosTasaVentaProm, parameters, x => new TasaVentaPro
                {
                    Mes = x.GetString(3).Substring(4, 2) + "/" + x.GetString(3).Substring(0, 4),
                    Prom = x.GetDecimal(4)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
