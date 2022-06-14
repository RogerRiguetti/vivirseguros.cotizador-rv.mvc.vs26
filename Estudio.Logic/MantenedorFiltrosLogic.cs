using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace Estudio.Logic
{
    public class MantenedorFiltrosLogic
    {
        MantenedorFiltrosRepository _mantenedor = new MantenedorFiltrosRepository();
        public Response getInfo()
        {
            try
            {
                Response res = new Response();
                List<List<string[]>> listas = new List<List<string[]>>();

                listas.Add(_mantenedor.getInfo("TR")); //Tipo de renta
                listas.Add(_mantenedor.getInfo("AL")); //Modalidad
                listas.Add(_mantenedor.getInfo("SE")); //Sexo
                listas.Add(_mantenedor.getInfo("EC")); //Estado civil
                listas.Add(_mantenedor.getInfo("CL")); //Cliente
                listas.Add(_mantenedor.getTipoMoneda()); //Tipo moneda
                res.Object = listas;
                res.IsOk = true;
                return res;
            }
            catch (Exception)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Error al buscar en base de datos";
                return res;
            }
        }
        public Response buscar(string caso)
        {
            try
            {
                Response res = new Response();
                switch (caso)
                {
                    case "JubLegal": res.Object = _mantenedor.getCombos("04"); break; //Jubilación Legal - VEJEZ NORMAL
                    case "JubAnticipada": res.Object = _mantenedor.getCombos("05"); break; //Jubilación Anticipada - VEJEZ ANTICIPADA
                    case "Sobrevivencia": res.Object = _mantenedor.getCombos("08"); break; //Sobrevivencia - SOBREVIVENCIA
                    case "InParcial": res.Object = _mantenedor.getCombos("07"); break; //Invalidez Parcial - INVALIDEZ PARCIAL 
                    case "InTotal": res.Object = _mantenedor.getCombos("06"); break; //Invalidez Total - INVALIDEZ TOTAL
                }
                res.IsOk = true;
                return res;
            }
            catch (Exception)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Error al buscar en base de datos";
                return res;
            }
        }
        public Response aceptar()
        {
            Response res = new Response();
            try
            {
                bool bolNuevo;
                var aceptar = _mantenedor.aceptar();
                if (aceptar.Count == 0)
                {
                    bolNuevo = true;
                }
                else
                {
                    bolNuevo = false;
                }
                if (bolNuevo == false)
                {
                    string[] respuesta = { "Proceso de Actualización", "¿ Está seguro que desea Modificar los Datos ?" };
                    res.Object = respuesta;
                }
                else
                {
                    string[] respuesta = { "Proceso de Ingreso", "¿ Está seguro que desea Ingresar los Datos ?" };
                    res.Object = respuesta;
                }
                res.IsOk = true;
                return res;
            }
            catch (Exception)
            {
                res.IsOk = false;
                res.Message = "Error al buscar en base de datos";
                return res;
            }
        }
        public Response listasSeleccionadas(List<string[]> combos, string edadDesde, string edadHasta, string primaDesde, string primaHasta, string caso, string usuario)
        {
            Response res = new Response();
            try
            {
                //_mantenedor.eliminar(); //ELIMINA DETALLE FILTRO
                switch (caso)
                {
                    case "JubLegal":
                        _mantenedor.eliminar("04");
                        regActFiltro(combos, edadDesde, edadHasta, primaDesde, primaHasta, usuario, "04");
                        break; //Jubilación Legal - VEJEZ NORMAL
                    case "JubAnticipada":
                        _mantenedor.eliminar("05");
                        regActFiltro(combos, edadDesde, edadHasta, primaDesde, primaHasta, usuario, "05");
                        break; //Jubilación Anticipada - VEJEZ ANTICIPADA
                    case "Sobrevivencia":
                        _mantenedor.eliminar("08");
                        regActFiltro(combos, edadDesde, edadHasta, primaDesde, primaHasta, usuario, "08");
                        break; //Sobrevivencia - SOBREVIVENCIA
                    case "InParcial":
                        _mantenedor.eliminar("07");
                        regActFiltro(combos, edadDesde, edadHasta, primaDesde, primaHasta, usuario, "07");
                        break; //Invalidez Parcial - INVALIDEZ PARCIAL 
                    case "InTotal":
                        _mantenedor.eliminar("06");
                        regActFiltro(combos, edadDesde, edadHasta, primaDesde, primaHasta, usuario, "06");
                        break; //Invalidez Total - INVALIDEZ TOTAL
                }
                res.Message = "El Proceso término satisfactoriamente";
                res.IsOk = true;
                return res;
            }
            catch (Exception)
            {
                res.IsOk = false;
                res.Message = "Surgío un error durante el proceso";
                return res;
            }
        }
        public void regActFiltro(List<string[]> combos, string edadDesde, string edadHasta, string primaDesde, string primaHasta, string usuario, string codIndPlan)
        {
            try
            {
                MantenedorFiltros registrar = new MantenedorFiltros();

                registrar.codIndPlan = codIndPlan;

                registrar.codIndTipRenta = registroIndicador(combos, 0, codIndPlan, "TR"); //'REGISTRO DE INDICADOR DE SELECCION PARA EL TIPO DE RENTA'

                registrar.codIndModalidad = registroIndicador(combos, 1, codIndPlan, "AL"); //'REGISTRO DE INDICADOR DE SELECCION PARA EL TIPO DE MODALIDAD'

                //'REGISTRO DE INDICADOR DE SELECCION PARA LA PRIMA'
                if (primaDesde != "" && primaHasta != "")
                {
                    registrar.codIndPrima = "1";
                    registrar.mtoPriDesde = Convert.ToDouble(primaDesde);
                    registrar.mtoPriHasta = Convert.ToDouble(primaHasta);
                }
                else
                {
                    registrar.codIndPrima = "0";
                    registrar.mtoPriDesde = 0;
                    registrar.mtoPriHasta = 0;
                }

                registrar.codIndCliente = registroIndicador(combos, 2, codIndPlan, "CL"); //'REGISTRO DE INDICADOR DE SELECCION PARA EL TIPO DE CLIENTE'

                registrar.codIndSexo = registroIndicador(combos, 3, codIndPlan, "SE"); //'REGISTRO DE INDICADOR DE SELECCION PARA EL SEXO'

                registrar.codIndEstCivil = registroIndicador(combos, 4, codIndPlan, "EC"); //'REGISTRO DE INDICADOR DE SELECCION PARA EL ESTADO CIVIL'

                //'REGISTRO DE INDICADOR DE SELECCION DE LA EDAD'
                if (edadDesde != "" && edadHasta != "")
                {
                    registrar.codIndPrima = "1";
                    registrar.numEdadDesde = Convert.ToInt32(edadDesde);
                    registrar.numEdadHasta = Convert.ToInt32(edadHasta);
                }
                else
                {
                    registrar.numEdadDesde = 0;
                    registrar.numEdadHasta = 0;
                }

                registrar.codIndMoneda = registroIndicador(combos, 5, codIndPlan, "TM"); //'REGISTRO DE INDICADOR DE SELECCION PARA LA MONEDA'

                bool bolNuevoTP = _mantenedor.buscarFiltro(registrar.codIndPlan);

                if (bolNuevoTP == false)
                {
                    _mantenedor.agregarNuevo(registrar, usuario);

                }
                else
                {
                    _mantenedor.actualizar(registrar, usuario);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string registroIndicador(List<string[]> combos, int posicion, string codIndPlan, string codTabla)
        {
            int indicador;
            if (combos[posicion][0] == "-1")
            {
                indicador = 0;
            }
            else
            {
                indicador = 1;
                for (int j = 0; j < combos[posicion].Length; j++)
                {
                    MantenedorFiltros detalle = new MantenedorFiltros();
                    var valor = combos[posicion][j].ToString();
                    detalle.codIndPlan = codIndPlan;
                    detalle.codTabla = codTabla;
                    detalle.codElemento = valor;
                    detalle.codIndicador = indicador;
                    _mantenedor.agregarNuevoFiltro(detalle);
                }
            }
            return indicador.ToString();
        }
    }
}


