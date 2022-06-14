using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class GenArMelerRepository
    {
        public int Num_Arch;
        string queryLlenarTbl = "";
        CalculoCotizacionRepository _calculoCotizacionRepository = new CalculoCotizacionRepository();

        public List<String[]> Busca_Informacion(string Fec, string numArchivo, string caso)
        {
            try
            {
                List<GenArMeler> DatosCon = new List<GenArMeler>();
                List<string[]> datos = new List<string[]>();

                Response solicitudes = new Response();

                var parameters = new List<SqlParameter>();
                if (caso == "tabla")
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCAR", ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCART", ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCalculo", SqlDbType.VarChar, Fec, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, int.Parse(numArchivo), ParameterDirection.Input));
                DatosCon = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    numOperacion = Convert.ToInt32(x.GetDecimal(0)),
                    Correlativo = x.GetInt32(1),
                    fec = x.GetString(2) == "" ? "" : (x.GetString(2).Substring(6, 2) + "/" + x.GetString(2).Substring(4, 2) + "/" + x.GetString(2).Substring(0, 4)),
                    asegurado = x.GetString(3),
                    moneda = x.GetString(4),
                    modalidad = x.GetString(5),
                    periodoDiferido = Convert.ToString(x.GetInt32(6)),
                    rentaTMP = Convert.ToDouble(x.GetDecimal(7)),
                    perGarantizado = Convert.ToString(x.GetInt32(8)),
                    rentaEsc = Convert.ToDouble(x.GetDecimal(9)),
                    primaUnica = Convert.ToDouble(x.GetDecimal(10)),
                    renTmp1T = Convert.ToDouble(x.GetInt32(11)),
                    mtoPension = Convert.ToDouble(x.GetDecimal(12)),
                    tasaVenta = Convert.ToDouble(x.GetDecimal(13)),
                    tir = Convert.ToDouble(x.GetDecimal(14)),
                    perdida = Convert.ToString(x.GetDecimal(15)),
                    comision = Convert.ToDouble(x.GetDecimal(16)),
                    nomArchivo = x.GetString(17),
                    tipoArchivo = x.GetString(18),
                    fechaCrea = x.GetString(19),
                    horaCrea = x.GetString(20),
                    usuarioCrea = x.GetString(21),
                    numArch = x.GetInt32(22),
                    numCot = x.GetString(23),
                    codTipPension = x.GetString(24),
                    indEstado = x.GetString(25),
                    codTipRen = x.GetString(26),
                    mtoCic = Convert.ToDouble(x.GetDecimal(27)),
                    codTipCambio = Convert.ToDouble(x.GetDecimal(28)),
                    codCoberCon = x.GetString(29),
                    indFiltroCotiza = x.GetString(30)
                }).ToList();


                if (DatosCon.Count != 0)
                {
                    string[] numsArchs = getNumArchivosB(Fec);
                    for (int j = 0; j < numsArchs.Length; j++) //Por si se cargan diferentes numeros de archivo
                    {
                        int numArch = Convert.ToInt32(numsArchs[j].Split('-')[0]);
                        var listBeneficiarios = _calculoCotizacionRepository.getBeneficiarios(numArch);
                        for (int i = 0; i < DatosCon.Count; i++) //para realizar el calculo con todas las modalidades
                        {

                            if (numArch == DatosCon[i].numArch) //Para filtrar las modalidades por numero de archivo
                            {
                                string sumapension = "0";
                                string PrimerTramo = "0";
                                string SegundoTramo = "0";
                                string PrimaUnica = "0";
                                decimal suma = 0;
                               
                                    switch (DatosCon[i].codTipPension)
                                    {
                                        case "04":
                                            sumapension = DatosCon[i].mtoPension.ToString();
                                            break;
                                        case "05":
                                            sumapension = DatosCon[i].mtoPension.ToString();
                                            break;
                                        case "06":
                                            if (DatosCon[i].codCoberCon == "S")
                                            {
                                                sumapension = (DatosCon[i].mtoPension * 0.7).ToString("N2");
                                            }
                                            else
                                            {
                                                sumapension = (DatosCon[i].mtoPension).ToString("N2");
                                            }
                                            break;
                                        case "07":
                                            if (DatosCon[i].codCoberCon == "S")
                                            {
                                                sumapension = (DatosCon[i].mtoPension * 0.5).ToString("N2");
                                            }
                                            else
                                            {
                                                sumapension = (DatosCon[i].mtoPension).ToString("N2");
                                            }
                                            break;
                                        case "08":
                                        for (int b = 0; b < listBeneficiarios.Count; b++)
                                        {
                                            if (listBeneficiarios[b].Num_Corr == DatosCon[i].Correlativo && listBeneficiarios[b].Num_Operacion == DatosCon[i].numOperacion)
                                            {
                                                suma = suma + listBeneficiarios[b].Mto_Pension * (listBeneficiarios[b].prc_Pension / 100);
                                                sumapension = suma.ToString();
                                            }
                                        }
                                            break;
                                    }
                                    switch (DatosCon[i].codTipRen)
                                    {
                                        case "1":
                                            {
                                                if (DatosCon[i].moneda != "S/.Aj." && DatosCon[i].moneda != "S/.")
                                                {
                                                    PrimaUnica = ((DatosCon[i].primaUnica) * DatosCon[i].codTipCambio).ToString("N2");
                                                }


                                                else
                                                {
                                                    PrimaUnica = DatosCon[i].primaUnica.ToString("N2");
                                                }
                                                PrimerTramo = "0" /*(Convert.ToDouble(sumaPension) * 2)*/;
                                                SegundoTramo = sumapension;
                                                break;
                                            }
                                        case "2":
                                            {
                                                if (DatosCon[i].moneda != "S/.Aj." && DatosCon[i].moneda != "S/.")
                                                {
                                                    PrimaUnica = ((DatosCon[i].primaUnica) * DatosCon[i].codTipCambio).ToString("N2");
                                                    PrimerTramo = (((Convert.ToDouble(sumapension) * 2)) * DatosCon[i].codTipCambio).ToString("N2");
                                                }
                                                else
                                                {
                                                    PrimaUnica = DatosCon[i].primaUnica.ToString("N2");
                                                    PrimerTramo = (Convert.ToDouble(sumapension) * 2).ToString("N2");
                                                }
                                                SegundoTramo = sumapension;
                                                break;
                                            }

                                        case "6":
                                            {
                                                PrimaUnica = DatosCon[i].mtoCic.ToString("N2");
                                                PrimerTramo = sumapension;
                                                SegundoTramo = (Convert.ToDouble(sumapension) * (Convert.ToInt32(DatosCon[i].rentaEsc) / 100)).ToString("N2");
                                                if (DatosCon[i].moneda != "S/.Aj." && DatosCon[i].moneda != "S/.")
                                                {
                                                    PrimerTramo = ((Convert.ToDouble(sumapension)) * DatosCon[i].codTipCambio).ToString("N2");
                                                    SegundoTramo = (((Convert.ToDouble(sumapension) * (Convert.ToInt32(DatosCon[i].rentaEsc))) / 100) * DatosCon[i].codTipCambio).ToString("N2");
                                                }
                                                else
                                                {
                                                    PrimerTramo = sumapension;
                                                    // modalidades = _excepcionesRepository.ConsultarModalidadesModificar(idSolicitud.ToString(), _ExcepcionesRepository.numCorrelativo);
                                                    SegundoTramo = ((Convert.ToDouble(sumapension) * (Convert.ToInt32(DatosCon[i].rentaEsc))) / 100).ToString("N2");
                                                }
                                                PrimaUnica = DatosCon[i].mtoCic.ToString("N2");
                                                break;
                                            }
                                    }
                                
                                string[] dTabla = {  DatosCon[i].numOperacion.ToString(), DatosCon[i].Correlativo.ToString(),DatosCon[i].fec, DatosCon[i].asegurado,DatosCon[i].moneda, DatosCon[i].modalidad.ToString(), DatosCon[i].periodoDiferido, DatosCon[i].rentaTMP.ToString(),
                                DatosCon[i].perGarantizado, DatosCon[i].rentaEsc.ToString(), PrimaUnica, PrimerTramo, SegundoTramo, DatosCon[i].tasaVenta.ToString(), DatosCon[i].tir.ToString(),DatosCon[i].perdida, DatosCon[i].comision.ToString(),

                                /*SegundoTramo,*/DatosCon[i].nomArchivo,DatosCon[i].tipoArchivo,DatosCon[i].fechaCrea,DatosCon[i].horaCrea,DatosCon[i].usuarioCrea,DatosCon[i].numArch.ToString(), DatosCon[i].numCot.ToString(), DatosCon[i].indEstado, DatosCon[i].indFiltroCotiza};
                               // actualizarSumPension(DatosCon[i].numArch, DatosCon[i].numOperacion, DatosCon[i].Correlativo, sumapension);
                                datos.Add(dTabla);
                            }
                        }
                    }

                }
                return datos;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //public void actualizarSumPension(int numArch, int numOperacion, int Correlativo, string Mtopension)
        //{
        //    try
        //    {
        //        DateTime fecha = DateTime.Now;
        //        var parameters = new List<SqlParameter>();
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_ACTUALIZASUMAS", ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@sumPension", SqlDbType.Decimal, Convert.ToDecimal(Mtopension), ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(numOperacion), ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, Correlativo, ParameterDirection.Input));
        //        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, numArch, ParameterDirection.Input));
        //        VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
        //        {
        //        }).ToList();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        public List<GenArMeler> getNumArchivos()
        {

            try
            {
                int count = 0;
                var parameters = new List<SqlParameter>();
                List<GenArMeler> Dato = new List<GenArMeler>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETNUMARCHSREG", ParameterDirection.Input));
                Dato = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    regTablaMeler = count = count + 1,
                    numArch = x.GetInt32(0),
                    nomArchivo = x.GetString(1),
                    fecEnvio = x.GetString(2) == "" ? "" : (x.GetString(2).Substring(6, 2) + "/" + x.GetString(2).Substring(4, 2) + "/" + x.GetString(2).Substring(0, 4)),
                    fecCierre = x.GetString(3) == "" ? "" : (x.GetString(3).Substring(6, 2) + "/" + x.GetString(3).Substring(4, 2) + "/" + x.GetString(3).Substring(0, 4)),
                    fecSuscripcion = x.GetString(2),
                    nrosOperacion = x.GetString(4)
                }).ToList();
                DateTime fecha = DateTime.Now;
                for (int i = 0; i < Dato.Count; i++)
                {
                    var fechaFin = Convert.ToDateTime(Dato[i].fecCierre);
                    var diff = fechaFin - fecha;
                    var dia = (int)diff.TotalDays;
                    if (dia <= 0) { Dato[i].color = "red"; }
                    if (dia > 0 && dia <= 2) { Dato[i].color = "yellow"; }
                    if (dia > 2) { Dato[i].color = "green"; }
                }
                return Dato;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string[]> actualizarGrilla()
        {

            try
            {
                List<string[]> datos = new List<string[]>();
                int count = 0;
                var parameters = new List<SqlParameter>();
                List<GenArMeler> Dato = new List<GenArMeler>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETNUMARCHSREG", ParameterDirection.Input));
                Dato = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    regTablaMeler = count = count + 1,
                    numArch = x.GetInt32(0),
                    nomArchivo = x.GetString(1),
                    fecEnvio = x.GetString(2) == "" ? "" : (x.GetString(2).Substring(6, 2) + "/" + x.GetString(2).Substring(4, 2) + "/" + x.GetString(2).Substring(0, 4)),
                    fecCierre = x.GetString(3) == "" ? "" : (x.GetString(3).Substring(6, 2) + "/" + x.GetString(3).Substring(4, 2) + "/" + x.GetString(3).Substring(0, 4)),
                    fecSuscripcion = x.GetString(2),
                    nrosOperacion = x.GetString(4)
                }).ToList();
                if (Dato.Count != 0)
                {
                    DateTime fecha = DateTime.Now;
                    for (int i = 0; i < Dato.Count; i++)
                    {
                        var fechaFin = Convert.ToDateTime(Dato[i].fecCierre);
                        var diff = fechaFin - fecha;
                        var dia = (int)diff.TotalDays;
                        if (dia <= 0) { Dato[i].color = "red"; }
                        if (dia > 0 && dia <= 2) { Dato[i].color = "yellow"; }
                        if (dia > 2) { Dato[i].color = "green"; }
                        string[] dTabla = { Dato[i].numArch.ToString(), Dato[i].nomArchivo, Dato[i].fecEnvio, Dato[i].fecCierre, Dato[i].nrosOperacion, Dato[i].regTablaMeler.ToString(), Dato[i].fecSuscripcion, Dato[i].color };
                        datos.Add(dTabla);
                    }
                }
                return datos;
            }
            catch (Exception)
            {
                throw;
            }
        } //getNumCotizaciones
        public string[] getNumCotizaciones(int numArch)
        {

            try
            {

                var parameters = new List<SqlParameter>();
                List<GenArMeler> Dato = new List<GenArMeler>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETNUMCOTIZACIONES", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, numArch, ParameterDirection.Input));
                Dato = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    numCot = x.GetString(0)
                }).ToList();

                string[] datosR = new string[Dato.Count];

                for (int i = 0; i < Dato.Count; i++)
                {
                    datosR[i] = Dato[i].numCot;
                }

                return datosR;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string[] getNumArchivosB(string Fec)
        {

            try
            {
                List<GenArMeler> _genArchMeler = new List<GenArMeler>();
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETNUMARCHS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCalculo", SqlDbType.VarChar, Fec, ParameterDirection.Input));
                _genArchMeler = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    numArch = x.GetInt32(0),
                    nomArchivo = x.GetString(1)
                }).ToList();
                if (_genArchMeler.Count != 0)
                {
                    string[] res = new string[_genArchMeler.Count];
                    for (int i = 0; i < _genArchMeler.Count; i++)
                    {
                        res[i] = _genArchMeler[i].numArch + " - " + _genArchMeler[i].nomArchivo;
                    }
                    return res;
                }
                else
                {
                    string[] res = new string[1];
                    res[0] = "-1";
                    return res;
                }
            }
            catch (Exception)
            {
                string[] res = new string[1];
                res[0] = "-1"; ;
                return res;
            }
        }
        public void ActualizaNoVig()
        {
            try
            {
                List<GenArMeler> _genArchMeler = new List<GenArMeler>();
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_ACTUALIZANOVIG", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCierre", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCierre", SqlDbType.VarChar, "I", ParameterDirection.Input));

                _genArchMeler = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    numCot = x.GetString(0)
                }).ToList();
                if (_genArchMeler.Count != 0)
                {
                    for (int i = 0; i < _genArchMeler.Count; i++)
                    {
                        parameters = new List<SqlParameter>();
                        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_ACTUALIZANOVIG", ParameterDirection.Input));
                        parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, _genArchMeler[i].numCot, ParameterDirection.Input));
                        VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                        {
                        }).FirstOrDefault();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //MODIFICADO PARA OPTIMIZAR - José Hernández Alvarado
        public string llenarTablas(GenArMeler datos, int numArchSal)
        {
            queryLlenarTbl = "";
            try
            {
                double MtoPension = 0, SumMtoPension = 0;
                int NumSelec = 0, NumTotSol = 0, SumMtoPrima = 0;
                //Envia todas las cotizaciones (modificada para que envie la seleccionada)
                enviarCotizacion(datos, numArchSal, MtoPension, NumTotSol, NumSelec, SumMtoPension);
                //Debe crear un registro cuando en la cotización no existe ninguna mod. Seleccionada
                queryLlenarTbl += "\n" + registroNoModalidad(datos, numArchSal, "S");
                //Obtiene la suma de Mto Primas Cotizadas
                obtieneSumaMtoPrimaC("S", numArchSal, datos.numCot, SumMtoPension);
                //Actualiza el registro de salida con los mtos calculados
                queryLlenarTbl += "\n" + regSalMtoCalculos(NumSelec, SumMtoPrima, SumMtoPension, NumTotSol, numArchSal, datos.numArch);

                return queryLlenarTbl;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int ultimoNarchivoSal()
        {
            try
            {
                int val = 1;
                GenArMeler _genArchMeler = new GenArMeler();
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "NUMARCHSAL", ParameterDirection.Input));

                _genArchMeler = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    numArch = x.GetInt32(0)
                }).FirstOrDefault();
                if (_genArchMeler != null)
                {
                    val = _genArchMeler.numArch + 1;
                    Num_Arch = val;
                }
                return val;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void creaRegistroSalida(GenArMeler datos, int numArchSal)
        {
            try
            {
                GenArMeler _genArchMeler = new GenArMeler();
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "REGSALIDA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArchSal", SqlDbType.Int, numArchSal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTipArchivo", SqlDbType.VarChar, "040", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@nomArchivo", SqlDbType.VarChar, datos.nomArchivo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCarArchivo", SqlDbType.VarChar, datos.fechaCrea, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCarArchio", SqlDbType.VarChar, datos.horaCrea, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@usuario", SqlDbType.VarChar, datos.usuarioCrea, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, datos.numArch, ParameterDirection.Input));

                _genArchMeler = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void enviarCotizacion(GenArMeler datos, int numArchSal, double MtoPension, int NumTotSol, int NumSelec, double SumMtoPension)
        {
            try
            {
                GenArMeler _genArchMeler = new GenArMeler();
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "TODASLASCOTIZACIONES", ParameterDirection.Input)); // aqui solo se verefica que exista el dato con ind_estado = "I"
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@indEstado", SqlDbType.VarChar, "I", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, datos.numArch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datos.numCot, ParameterDirection.Input));

                _genArchMeler = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    numCot = x.GetString(0),
                    numArch = x.GetInt32(1)
                }).FirstOrDefault();

                if (_genArchMeler != null)
                {
                    MtoPension = 0;
                    List<GenArMeler> _genArchMeler2 = new List<GenArMeler>();
                    parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_GUARDARMODALIDADES", ParameterDirection.Input)); //obtiene los valores de la modalidad de esa cotizacion
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, _genArchMeler.numCot, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, _genArchMeler.numArch, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, datos.Correlativo, ParameterDirection.Input));
                    _genArchMeler2 = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                    {
                        numCot = x.GetString(0),
                        Correlativo = x.GetInt32(1),
                        numArch = x.GetInt32(2),
                        codRechazo = x.GetString(3),
                        codEstCot = x.GetString(4),
                        mtoPension = Convert.ToDouble(x.GetDecimal(5))
                    }).ToList();
                    if (_genArchMeler2.Count != 0)
                    {
                        for (int i = 0; i < _genArchMeler2.Count; i++)
                        {
                            parameters = new List<SqlParameter>();
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_GUARDARMODALIDADES", ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArchSal", SqlDbType.Int, numArchSal, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, _genArchMeler2[i].numArch, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, _genArchMeler2[i].numCot, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, _genArchMeler2[i].Correlativo, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@codRechazo", SqlDbType.VarChar, _genArchMeler2[i].codRechazo, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstCot", SqlDbType.VarChar, _genArchMeler2[i].codEstCot, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@usuario", SqlDbType.VarChar, datos.usuarioCrea, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                            VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                            {
                            }).FirstOrDefault();

                            //Graba en la tabla de etapas de la cotización
                            ProCarArchivoRepository _pcar = new ProCarArchivoRepository();
                            string cod_Etapa = "40";
                            string num_Cot = _genArchMeler.numCot;
                            int num_Correlativo = _genArchMeler2[i].Correlativo;
                            string fec_ini = _pcar.busca_FechaServidor();
                            string hor_ini = _pcar.busca_HoraServidor();
                            string cod_EtapaAnt = _pcar.etapaAnterior(num_Cot, num_Correlativo);
                            //validar si se ha registrado anteriormente la misma etapa
                            bool validarEtapa = _pcar.validar(num_Cot, num_Correlativo, cod_Etapa, fec_ini, hor_ini);
                            if (validarEtapa == false)
                            {
                                //no existe etapa
                                _pcar.insert_Ingreso_Envio(num_Cot, num_Correlativo, cod_Etapa, fec_ini, hor_ini, cod_EtapaAnt, datos.usuarioCrea);
                            }
                            else
                            {
                                //actualizar
                                _pcar.actualizarEtapa(num_Cot, num_Correlativo, cod_Etapa, fec_ini, hor_ini, cod_EtapaAnt, datos.usuarioCrea);
                            }
                            //Contador de Solicitudes (incluye todas
                            NumTotSol = NumTotSol + 1;
                            //Contador de seleccionadas
                            if (_genArchMeler2[i].codEstCot == "S" && _genArchMeler2[i].codRechazo == "0")
                            {
                                NumSelec = NumSelec + 1;
                            }
                            //Guarda el mayor Mto de Pensión de pt_tmae_detcotizacion
                            if (_genArchMeler2[i].mtoPension > MtoPension)
                            {
                                MtoPension = _genArchMeler2[i].mtoPension;
                            }
                            SumMtoPension = SumMtoPension + MtoPension;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //MODIFICADO PARA OPTIMIZAR - José Hernández Alvarado
        public string registroNoModalidad(GenArMeler datos, int numArchSal, string vgEstSel)
        {
            string queryScriptMod = "";
            try
            {
                GenArMeler _GenArMeler = new GenArMeler();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_NOEXISTEMOD1", ParameterDirection.Input)); //Modificada para que solo retorne la seleccionada
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArchSal", SqlDbType.Int, numArchSal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstCot", SqlDbType.VarChar, vgEstSel, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datos.numCot, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.VarChar, datos.Correlativo, ParameterDirection.Input));
                _GenArMeler = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    numCot = x.GetString(0),
                    numArch = x.GetInt32(1),
                    Correlativo = x.GetInt32(2)
                }).FirstOrDefault();
                if (_GenArMeler != null)
                {
                    GenArMeler _GenArMeler2 = new GenArMeler();
                    parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_NOEXISTEMOD2", ParameterDirection.Input)); //Modificada para que solo retorne la seleccionada
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codPar", SqlDbType.VarChar, "99", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.VarChar, _GenArMeler.numArch, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, _GenArMeler.numCot, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.VarChar, _GenArMeler.Correlativo, ParameterDirection.Input));
                    _GenArMeler2 = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                    {
                        numOperacion = Convert.ToInt32(x.GetDecimal(0)),
                        fecSuscripcion = x.GetString(1),
                        codCUSPP = x.GetString(2),
                        codTipPension = x.GetString(3),
                        codAFP = x.GetString(4),
                        nombre = x.GetString(5),
                        nombreSeg = x.IsDBNull(6) ? "" : x.GetString(6),
                        paterno = x.GetString(7),
                        materno = x.GetString(8),
                        usuarioCrea = x.GetString(9)
                    }).FirstOrDefault();
                    if (_GenArMeler2 != null)
                    {
                        try
                        {
                            DateTime fecha = DateTime.Now;

                            queryScriptMod = "\nINSERT INTO PT_THIS_DETSALCOT " +
                                                 "(NUM_ARCHSAL, NUM_COT, NUM_OPERACION, FEC_SUSCRIPCION, COD_CUSPP, COD_TIPPENSION, COD_AFP, " +
                                                 " GLS_NOMBEN, GLS_NOMSEGBEN, GLS_PATBEN, GLS_MATBEN, COD_USUARIOCREA, FEC_CREA, HOR_CREA) " +
                            " VALUES( " + numArchSal + ", '" + _GenArMeler.numCot + "', " + Convert.ToDecimal(_GenArMeler2.numOperacion) + ", '" + _GenArMeler2.fecSuscripcion +
                            "', '" + _GenArMeler2.codCUSPP + "', '" + _GenArMeler2.codTipPension + "', '" + _GenArMeler2.codAFP + "', '" + _GenArMeler2.nombre +
                            "', " + (_GenArMeler2.nombreSeg == "" ? "NULL" : "'" + _GenArMeler2.nombreSeg + "'") + ", '" + _GenArMeler2.paterno + "', '" + _GenArMeler2.materno +
                            "', '" + _GenArMeler2.usuarioCrea + "', '" + fecha.ToString("yyyyMMdd") + "', '" + fecha.ToString("hhmmss") + "')";

                            /*parameters = new List<SqlParameter>();
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "I_NOEXISTEMOD", ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, numArchSal, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, _GenArMeler.numCot, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Decimal, Convert.ToDecimal(_GenArMeler2.numOperacion), ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecSubs", SqlDbType.VarChar, _GenArMeler2.fecSuscripcion, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCUSPP", SqlDbType.VarChar, _GenArMeler2.codCUSPP, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTipPen", SqlDbType.VarChar, _GenArMeler2.codTipPension, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@codAFP", SqlDbType.VarChar, _GenArMeler2.codAFP, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@nombre", SqlDbType.VarChar, _GenArMeler2.nombre, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@nombreSeg", SqlDbType.VarChar, _GenArMeler2.nombreSeg == "" ? null : _GenArMeler2.nombreSeg, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@paterno", SqlDbType.VarChar, _GenArMeler2.paterno, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@materno", SqlDbType.VarChar, _GenArMeler2.materno, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@usuario", SqlDbType.VarChar, _GenArMeler2.usuarioCrea, ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@fecCrea", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                            parameters.Add(VCEDBContext<RowAffected>.AddParams("@horCrea", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                            VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                            {
                            }).FirstOrDefault();*/
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                return queryScriptMod;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void obtieneSumaMtoPrimaC(string vgEstSel, int numArchSal, string numCot, double SumMtoPrima)
        {
            try
            {
                GenArMeler _genArchMeler = new GenArMeler();
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "SUMMTOPRIMAS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstCot", SqlDbType.VarChar, vgEstSel, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, numArchSal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, numCot, ParameterDirection.Input));

                _genArchMeler = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    primaUnica = x.IsDBNull(0) ? 0 : Convert.ToDouble(x.GetDecimal(0))
                }).FirstOrDefault();
                if (_genArchMeler != null)
                {
                    SumMtoPrima = _genArchMeler.primaUnica;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //MODIFICADO PARA OPTIMIZAR - José Hernández Alvarado
        public string regSalMtoCalculos(int NumSelec, double SumMtoPrima, double SumMtoPension, int NumTotSol, int numArchSal, int numArch)
        {
            string queryScriptMtos = "";

            try
            {
                GenArMeler _genArchMeler = new GenArMeler();
                DateTime fecha = DateTime.Now;

                queryScriptMtos = "\nUPDATE PT_THIS_SALIDA SET " +
                                  "NUM_TOTSOLOFE = " + NumSelec + ", MTO_TOTPRIMA = " + Convert.ToDecimal(SumMtoPrima) + ", MTO_TOTPENSION = " + Convert.ToDecimal(SumMtoPension) +
                                  ", NUM_TOTSOL = " + NumTotSol + " WHERE NUM_ARCHSAL = " + numArchSal + " AND NUM_ARCHIVO = " + numArch + "";

                /*var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_REGSALIDA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numTotSolOfe", SqlDbType.Int, NumSelec, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoTotPrima", SqlDbType.Decimal, Convert.ToDecimal(SumMtoPrima), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@mtoTotPension", SqlDbType.Decimal, Convert.ToDecimal(SumMtoPension), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numTotSol", SqlDbType.Int, NumTotSol, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArchSal", SqlDbType.Int, numArchSal, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumArch", SqlDbType.Int, numArch, ParameterDirection.Input));

                _genArchMeler = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    primaUnica = x.IsDBNull(0) ? 0 : Convert.ToDouble(x.GetDecimal(0))
                }).FirstOrDefault();
                if (_genArchMeler != null)
                {
                    SumMtoPrima = _genArchMeler.primaUnica;
                }*/

                return queryScriptMtos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //MODIFICADO PARA OPTIMIZAR - José Hernández Alvarado
        public string Actualiza_EstEnv(GenArMeler datos)
        {
            string queryScriptEstEnv = "";

            try
            {
                //Actualiza el indicador de estado de ingresadas a enviadas
                DateTime fecha = DateTime.Now;

                queryScriptEstEnv += "\nUPDATE PT_TMAE_COTIZACION SET IND_ESTADO = 'E' WHERE NUM_COT " +
                                    "IN(SELECT DISTINCT NUM_COT FROM PT_THIS_DETSALIDA WHERE NUM_ARCHSAL = " + Num_Arch + ") AND NUM_COT = '" + datos.numCot + "'";

                var parameters = new List<SqlParameter>();
                /*parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_ACTUALIZAESTENV_COT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@indEstado", SqlDbType.VarChar, "E", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArchSal", SqlDbType.Int, Num_Arch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datos.numCot, ParameterDirection.Input));

                VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                }).FirstOrDefault();*/

                //Actualiza el estado de las cotizaciones seleccionadas a enviadas
                GenArMeler _genArchMeler2 = new GenArMeler();
                parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_ACTUALIZAESTENV", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstCot", SqlDbType.VarChar, "S", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArchSal", SqlDbType.Int, Num_Arch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, datos.numCot, ParameterDirection.Input));

                _genArchMeler2 = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    numCot = x.GetString(0),
                    Correlativo = x.GetInt32(1)
                }).FirstOrDefault();
                if (_genArchMeler2 != null)
                {

                    queryScriptEstEnv += "\nUPDATE PT_TMAE_DETCOTIZACION SET COD_ESTCOT = 'E' WHERE NUM_COT = '" + _genArchMeler2.numCot +"'"/*+ "' and NUM_CORRELATIVO = " + _genArchMeler2.Correlativo + ""*/;

                    /*parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_ACTUALIZAESTENV_DETCOT", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codEstCot", SqlDbType.VarChar, "E", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, _genArchMeler2.Correlativo, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, _genArchMeler2.numCot, ParameterDirection.Input));

                    VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                    {
                    }).FirstOrDefault();*/
                }
                return queryScriptEstEnv;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public GenArMeler getValoresRMB()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_RENTAMYB", ParameterDirection.Input));
                return VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    mtoPension = Convert.ToDouble(x.GetDecimal(0)),
                    prcTasaRT = Convert.ToDouble(x.GetDecimal(1)),
                    mtoPensionRVD = Convert.ToDouble(x.GetDecimal(2)),
                    prcTasaRVD = Convert.ToDouble(x.GetDecimal(3))
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public GenArMeler datosThisSalida(string numCot)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_DATOSXML1", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArchSal", SqlDbType.Int, Num_Arch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, numCot, ParameterDirection.Input));
                return VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    numCot = x.GetString(0),
                    numOperacion = Convert.ToInt32(x.GetDecimal(1)),
                    codCUSPP = x.GetString(2)
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GenArMeler> getProductosCotizados(string strNumCot, string vgCodTabla_TipRen)
        {
            try
            {

                List<GenArMeler> _listCorrelativos = new List<GenArMeler>();
                GenArMeler prodCot = new GenArMeler();
                List<GenArMeler> productosCotisadosS = new List<GenArMeler>();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETNUMSCORRELATIVOS", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numArchSal", SqlDbType.Int, Num_Arch, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, strNumCot, ParameterDirection.Input));
                _listCorrelativos = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                    Correlativo = x.GetInt32(0)
                }).ToList();
                for (int i = 0; i < _listCorrelativos.Count; i++)
                {
                    parameters = new List<SqlParameter>();
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "S_DATOSXML2", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@codTabla", SqlDbType.VarChar, vgCodTabla_TipRen, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCot", SqlDbType.VarChar, strNumCot, ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@numCorrelativo", SqlDbType.Int, _listCorrelativos[i].Correlativo, ParameterDirection.Input));
                    prodCot = VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                    {
                        numCot = x.GetString(0),
                        codTipRen = x.IsDBNull(1) ? "" : x.GetString(1),
                        codMoneda = x.GetString(2),
                        numMesDif = x.GetInt32(3),
                        prcRentaTmp = Convert.ToDouble(x.GetDecimal(4)),
                        numMesGar = x.GetInt32(5),
                        codCoberCon = x.GetString(6),
                        codDerCre = x.GetString(7),
                        codDerGra = x.GetString(8),
                        codEstCot = x.GetString(9),
                        mtoPriUni = Convert.ToDouble(x.GetDecimal(10)),
                        mtoPriuniDif = Convert.ToDouble(x.GetDecimal(11)),
                        mtoRentaTmpAfp = Convert.ToDouble(x.GetDecimal(12)),
                        prcRentaAFP = Convert.ToDouble(x.GetDecimal(13)),
                        //mtoPension = Convert.ToDouble(x.GetDecimal(14)),
                        mtoPension = Convert.ToDouble(x.GetDecimal(19)),
                        prcTasaVTA = Convert.ToDouble(x.GetDecimal(15)),
                        mtoValMoneda = Convert.ToDouble(x.GetDecimal(16)),
                        moneda = x.IsDBNull(17) ? "" : x.GetString(17),
                        codTipPension = x.GetString(18),
                        mtoSumPension = Convert.ToDouble(x.GetDecimal(19)),
                        codParCap = x.IsDBNull(20) ? "" : x.GetString(20),
                        prcParCapCia = x.IsDBNull(21) ? 0 : Convert.ToDouble(x.GetDecimal(21)),
                        codTipReajuste = x.IsDBNull(22) ? "" : x.GetString(22),
                        numMesesC = x.IsDBNull(23) ? 0 : x.GetInt32(23),
                        prcRentaEsc = Convert.ToDouble(x.GetDecimal(24)),
                        numArch = x.GetInt32(25),
                        Correlativo = x.GetInt32(26),
                        numOperacion = Convert.ToInt32(x.GetDecimal(27)),
                        codTipCambio = Convert.ToDouble(x.GetDecimal(28)),
                        Ind_SISCO = x.GetInt32(29),
                        indFiltroCotiza = x.GetString(30),
                        numCalculados = x.GetInt32(31)
                    }).FirstOrDefault();
                    productosCotisadosS.Add(prodCot);
                }
                return productosCotisadosS;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ActualizaIndCotiza(int numArch, string numCot, int numCorrelativo, string siNo)
        {
            try
            {
                return "UPDATE PT_TMAE_DETCOTIZACION SET IND_FILTROCOTIZA = '" + siNo + "'  WHERE NUM_ARCHIVO = " + numArch + " AND NUM_COT = '" + numCot + "' AND NUM_CORRELATIVO = " + numCorrelativo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ActualizaIndCotizaEjecutar(string query)
        {
            try
            {
                SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(query, x => new SolicitudesCotizacion
                {
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDetCotizacion(int numArch)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_DETCOT", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));
                VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void UpdateDetCotizacion1(int numArch)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "U_DETCOTP", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vNumArchivo", SqlDbType.Int, numArch, ParameterDirection.Input));
                VCEDBContext<GenArMeler>.CallStoreProcedure(StoredProcedures.CO_ConsultasProEnvioCotizaciones, parameters, x => new GenArMeler
                {
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void EjecutarScript(string script)
        {
            try
            {
                SRVDBContext<DataTable>.CallSelectStatementDt(script, x => new DataTable());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
