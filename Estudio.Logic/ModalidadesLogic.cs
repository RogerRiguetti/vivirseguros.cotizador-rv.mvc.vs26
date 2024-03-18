using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Persistence.Repositories;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using System.Data;

namespace Estudio.Logic
{
    public class ModalidadesLogic
    {
        ModalidadesRepository _modalidadesRepository = new ModalidadesRepository();

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-15
        /// Registra o modifica modalidades dependiendo de la bandera
        /// </summary>
        /// <param name="bandera"> Indica si se va a registrar o modificar un beneficiario </param>
        /// <param name="idModalidad"> Id de la modalidad a modificar </param>
        /// <param name="aniosDiferidos"> Años diferidos </param>
        /// <param name="porcentajeAfp"> Porcentaje AFP (Administrador de Fondo de Pensión) </param>
        /// <param name="aniosGarantizados"> Años garantizados </param>
        /// <param name="gratificacion"> Gratificación </param>
        /// <param name="porcentajeTemporal"> Porcentaje temporal </param>
        /// <param name="idMoneda"> Id del tipo de moneda </param>
        /// <param name="idTipoRenta"> Id del tipo de renta </param>
        /// <param name="idModalidadCat"> Id del catálogo de modalidades </param>
        /// <param name="primerTramo"> Procentaje del primer tramo </param>
        /// <param name="segundoTramo"> Porcentaje del primer tramo </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public Response RegistrarModificarModalidad(string bandera, int idModalidad, Modalidad modalidad)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _modalidadesRepository.RegistrarModificarModalidad(bandera, idModalidad, modalidad);
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-15
        /// Consulta la modalidad a modificar
        /// </summary>
        /// <param name="idModalidad"> Id de la modalidad a consultar </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public Response ConsultarModalidad(int idModalidad)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _modalidadesRepository.ConsultarModalidad(idModalidad);
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-15
        /// Elimina modalidad de base de datos
        /// </summary>
        /// <param name="idModalidad"> Id de la modalidad a eliminar </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public Response EliminarModalidad(int idModalidad)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _modalidadesRepository.EliminarModalidad(idModalidad);
                res.Message = "Modalidad eliminada con éxito";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }
        }

        public Response EliminarModalidadC(int idModalidad)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _modalidadesRepository.EliminarModalidadC(idModalidad);
                res.Message = "Modalidad eliminada con éxito";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-15
        /// Registra las modalidades del paquete
        /// 
        /// Antonio Quezada 2018-07-23
        /// Se agrega el tipo pensión para agregarlo a los paquetes
        /// </summary>
        /// <param name="idPaquete"> Id del paquete a registrar </param>
        /// <param name="TipoAfp"> Tipo de AFP seleccionada </param>
        /// <returns> Regresa una respuesta que contiene mensaje y el objeto recuperado de la operación </returns>

        public Response RegistrarPaquete(int idPaquete, string TipoAfp)
        {
            try
            {
                Response res = new Response();
                DataTable afpDT = new DataTable();
                Modalidad modalidad = new Modalidad();
                string codafp = "";

                afpDT = _modalidadesRepository.CodigoAFP(TipoAfp);
                if (afpDT.Rows.Count != 0)
                {
                    codafp = (afpDT.Rows[0][0]).ToString();
                    res.IsOk = true;
                    res.Object = _modalidadesRepository.RegistrarPaquete(idPaquete, Convert.ToDecimal(codafp));
                    res.Message = "Operación exitosa";
                }
                else
                {
                    codafp = "0";
                    res.IsOk = false;
                    res.Message = "Ingrese AFP";
                }
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }
        }

        /// <summary>
        /// Lizbeth Morales 
        /// 10/07/2018
        /// Consulta el porcetaje Afp dependiendo del tipo de Afp seleccionado
        /// 
        /// Antonio Quezada 2018-07-23
        /// Modificación de modalidades previamente registradas
        /// </summary>
        /// <param name="afp">Parametro que contiene la informacion del afp seleccionado</param>
        /// <param name="idsModalidades"> Lista de ids de modalidades registradas hasta el momento </param>
        /// <returns>regresa el % afp </returns>

        public Response ConsultarAfp(string afp, List<string> idsModalidades)
        {
            try
            {
                DataTable afpDT = new DataTable();
                Modalidad modalidad = new Modalidad();
                string codafp;

                afpDT = _modalidadesRepository.CodigoAFP(afp);
                codafp = (afpDT.Rows[0][0]).ToString();

                if (idsModalidades != null)
                {
                    foreach (var item in idsModalidades)
                    {
                        Int32 idModalidad = Convert.ToInt32(item);

                        modalidad = new Modalidad();
                        modalidad = _modalidadesRepository.ConsultarModalidad(idModalidad);

                        modalidad.IdModalidad = idModalidad;
                        modalidad.PorcentajeRentabilidadAfp = Convert.ToDecimal(codafp);

                        _modalidadesRepository.RegistrarModificarModalidad("U", idModalidad, modalidad);
                    }
                }

                Response res = new Response();
                res.IsOk = true;
                res.Object = codafp;
                res.Message = "Información cargada con éxito";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-08-08
        /// Elimina todos las Modalidades que fueron registradas
        /// </summary>
        /// <param name="idsModalidades"> Lista de ids de Beneficiarios registrados </param>
        /// <returns> Regresa un objeto que contiene una lista de los ids eliminados </returns>

        public Response EliminarModalidades(List<string> idsModalidades)
        {
            try
            {
                Response res = new Response();

                if (idsModalidades != null)
                {
                    foreach (var item in idsModalidades)
                    {
                        int idModalidad = Convert.ToInt32(item);
                        //_modalidadesRepository.EliminarModalidad(idModalidad);
                        _modalidadesRepository.BajaTemporal(idModalidad);
                    }
                }

                res.IsOk = true;
                res.Object = idsModalidades;
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }
        }
        

        public Response EliminarModalidadesC(List<string> idsModalidades)
        {
            try
            {
                Response res = new Response();

                if (idsModalidades != null)
                {
                    foreach (var item in idsModalidades)
                    {
                        int idModalidad = Convert.ToInt32(item);
                        _modalidadesRepository.EliminarModalidadC(idModalidad);
                    }
                }

                res.IsOk = true;
                res.Object = idsModalidades;
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }
        }

        public Response ValidacionRegresarMod(int idCotizacion)
        {
            try
            {
                Response res = new Response();
                _modalidadesRepository.ValidacionRegresarMod(idCotizacion);
                res.IsOk = true;
                res.Object = idCotizacion;
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        public Response BajaTemporal(int idBeneficiario)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _modalidadesRepository.BajaTemporal(idBeneficiario);
                // _beneficiariosRepository.EjecutarScript(query);
                //res.Object = idBeneficiario;
                //PorcentajesBeneficiarios(ListidsBeneficiarios, int idPension, string fechaFallecimiento, string FecDev)
                res.Message = "Modalidad eliminada con éxito";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

        public Response ConsultarValidacionesModalidades(int idPension, int idRenta, int idMoneda)
        {
            try
            {
                List<Modalidad> validaciones = new List<Modalidad>();

                List<int> AniosDiferidos = new List<int>();
                List<int> PorcentajeRentTmp = new List<int>();
                List<int> AniosGarantizados = new List<int>();
                List<decimal> PrimerTramo = new List<decimal>();
                List<decimal> SegundoTramo = new List<decimal>();

                validaciones = _modalidadesRepository.ConsultaValidacionesModalidades(idPension, idRenta, idMoneda);

                AniosDiferidos = (from ad in validaciones select ad.AniosDiferidos).Distinct().ToList();
                PorcentajeRentTmp = (from prt in validaciones select prt.PorcentajeRentaTemporal).Distinct().ToList();
                AniosGarantizados = (from ag in validaciones select ag.AniosGarantizados).Distinct().ToList();
                PrimerTramo = (from pt in validaciones select pt.PrimerTramo).Distinct().ToList();
                SegundoTramo = (from st in validaciones select st.SegundoTramo).Distinct().ToList();

                Response res = new Response();
                res.IsOk = true;
                res.Object = new { AniosDiferidos = AniosDiferidos, PorcentajeRentTmp = PorcentajeRentTmp, AniosGarantizados = AniosGarantizados,
                                   PrimerTramo = PrimerTramo, SegundoTramo = SegundoTramo};
                res.Message = "Operación exitosa";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas";
                return res;
            }
        }

    }
}
