using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using System.Web;
using System.Data;
using System.IO;

namespace Estudio.Process
{
    public class RutinaPorcentaje
    {
        public string msj { get; set; }
        public List<beDatosBen> PorcentajeBen(List<beDatosBen> model, string fec, string cob, string pen, long eda, List<bePorcenLegales> ValPorcentajes, List<beDatosBen> CopiaBen)
        {
            double vlValor;
            int[] Numor = new int[20];
            int[] Ncorbe = new int[20];
            int[] Codrel = new int[20];
            int[] Cod_Grfam = new int[20];
            string[] Sexobe = new string[20];
            string[] Inv = new string[20];
            string[] Coinbe = new string[20];
            int[] Derpen = new int[20];
            int[] Nanbe = new int[20];
            int[] Nmnbe = new int[20];
            int[] Ndnbe = new int[20];
            int[] Nabe = new int[20];
            int[] Nmbe = new int[20];
            int[] Ndbe = new int[20];
            double[] Porben = new double[20];
            string[] Codcbe = new string[20];
            int Iaap;
            int Immp;
            int Iddp;
            int vlNum_Ben;
            int[] Hijos = new int[20];
            int[] Hijos_Inv = new int[20];
            int[] Hijos_SinDerechoPension = new int[20];
            DateTime[] Hijo_Menor = new DateTime[20];
            DateTime[] Hijo_Menor_Ant = new DateTime[20];
            DateTime[] Fec_NacHM = new DateTime[20];
            int[] Hijos_SinConyugeMadre = new int[20];
            string[] Fec_Fall = new string[20];
            string vlFechaFallCau;
            int cont_esp_Totales;
            int[] cont_esp_Tot_GF = new int[20];
            int[] cont_mhn_Totales = new int[20];
            int[] cont_mhn_Tot_GF = new int[20];
            double vlValorHijo;
            DateTime datFechaNac;
            string strFechaNac;
            string strFechaInv;
            int[] cont_mhn = new int[20];
            int cont_causante;
            int cont_esposa;
            int cont_mhn_tot;
            int cont_hijo;
            int cont_padres;
            long i, edad_mes_ben, fecha_sin, Q, x;
            int u;
            long L18 = 0;
            string sexo_cau = "";
            double v_hijo;
            string vlFechaFallecimiento;
            long vlContBen = 1;
            //string vlFechaMatrimonio;
            //double vlPorcBenef = 0;
            //double vlPenBenef = 0;
            //double vlPenGarBenef = 0;
            double vlSumarTotalPorcentajePension = 0;
            double vlSumaDef = 0;
            double vlDif = 0;
            double vlPorcentajeRecal = 0;
            double vlRemuneracionProm = 0;
            double vlRemuneracionBase = 0;
            double vlSumaPensionesHijosSC = 0;
            //double vlDifSumaPenHjosSC = 0;
            double vlSumaPjePenPadres = 0;

            beDatosBen TablaResul = new beDatosBen();
            beDatosBen Resultado = new beDatosBen();
            List<beDatosBen> TablaResulList = new List<beDatosBen>();

            Iaap = int.Parse(fec.Substring(0, 4));
            Immp = int.Parse(fec.Substring(4, 2));
            Iddp = int.Parse(fec.Substring(6, 2));

            fecha_sin = Iaap * 12 + Immp;
            vlNum_Ben = model.Count();

            //Array.Resize(ref Numor, vlNum_Ben);
            //Array.Resize(ref Ncorbe, vlNum_Ben);
            //Array.Resize(ref Codrel, vlNum_Ben);
            //Array.Resize(ref Cod_Grfam, vlNum_Ben);
            //Array.Resize(ref Sexobe, vlNum_Ben);
            //Array.Resize(ref Inv, vlNum_Ben);
            //Array.Resize(ref Coinbe, vlNum_Ben);
            //Array.Resize(ref Derpen, vlNum_Ben);
            //Array.Resize(ref Nanbe, vlNum_Ben);
            //Array.Resize(ref Nmnbe, vlNum_Ben);
            //Array.Resize(ref Ndnbe, vlNum_Ben);
            //Array.Resize(ref Hijos, vlNum_Ben);
            //Array.Resize(ref Hijos_Inv, vlNum_Ben);
            //Array.Resize(ref Hijos_SinDerechoPension, vlNum_Ben);
            //Array.Resize(ref Hijo_Menor, vlNum_Ben);
            //Array.Resize(ref Hijo_Menor_Ant, vlNum_Ben);
            //Array.Resize(ref Porben, vlNum_Ben);
            //Array.Resize(ref Codcbe, vlNum_Ben);
            //Array.Resize(ref Fec_NacHM, vlNum_Ben);
            //Array.Resize(ref cont_mhn, vlNum_Ben);
            //Array.Resize(ref Nabe, vlNum_Ben);
            //Array.Resize(ref Nmbe, vlNum_Ben);
            //Array.Resize(ref Ndbe, vlNum_Ben);

            //Array.Resize(ref Hijos_SinConyugeMadre, vlNum_Ben);
            //Array.Resize(ref Fec_Fall, vlNum_Ben);
            //Array.Resize(ref cont_mhn_Tot_GF, vlNum_Ben);
            //Array.Resize(ref cont_esp_Tot_GF, vlNum_Ben);

            vlFechaFallCau = "";
            vlValorHijo = 0;

            i = 0;
            foreach (var item in model)
            {
                if (item.NumOrd == 0)
                {
                    return TablaResulList;
                };
                Numor[i] = item.NumOrd;
                Ncorbe[i] = int.Parse(CopiaBen[(int)i].CodPar);
                Codrel[i] = int.Parse(CopiaBen[(int)i].CodPar);
                Cod_Grfam[i] = int.Parse(item.GruFam);

                Sexobe[i] = item.TipSex;
                if (Ncorbe[i] == 99)
                {
                    sexo_cau = Sexobe[i];
                };
                Inv[i] = item.TipInv;
                if (Ncorbe[i] == 99 && pen == "S")
                {
                    Derpen[i] = 10;
                }
                else
                {
                    Derpen[i] = 99;
                };
                strFechaNac = item.FecNac;
                Nanbe[i] = int.Parse(strFechaNac.Substring(0, 4));
                Nmnbe[i] = int.Parse(strFechaNac.Substring(4, 2));
                Ndnbe[i] = int.Parse(strFechaNac.Substring(6, 2));
                strFechaInv = item.FecInv;
                if (strFechaInv != "")
                {
                    Nabe[i] = int.Parse(strFechaInv.Substring(0, 4));
                    Nmbe[i] = int.Parse(strFechaInv.Substring(4, 2));
                    Ndbe[i] = int.Parse(strFechaInv.Substring(6, 2));
                }
                if (Inv[i] == "P") { Coinbe[i] = "P"; };
                if (Inv[i] == "T") { Coinbe[i] = "T"; };
                if (Inv[i] == "N") { Coinbe[i] = "N"; };

                edad_mes_ben = fecha_sin - (Nanbe[i] * 12 + Nmnbe[i]);

                vlFechaFallecimiento = item.FacFal;
                Fec_Fall[i] = vlFechaFallecimiento;
                if (Codrel[i] == 99) { vlFechaFallCau = vlFechaFallecimiento; };

                if (Codrel[i] >= 30 && Codrel[i] < 40)
                {
                    L18 = eda;
                    if (vlFechaFallecimiento != "")
                    {
                        Derpen[i] = 10;
                        if (item.DerPen != "10")
                        {
                            item.DerPen = "10";
                            Hijos_SinDerechoPension[int.Parse(item.GruFam)] = Hijos_SinDerechoPension[int.Parse(item.GruFam)] + 1;
                        }
                    }
                    else
                    {
                        if (edad_mes_ben > L18 && Coinbe[i] == "N")
                        {
                            Derpen[i] = 10;
                            if (item.DerPen != "10")
                            {
                                item.DerPen = "10";
                                Hijos_SinDerechoPension[int.Parse(item.GruFam)] = Hijos_SinDerechoPension[int.Parse(item.GruFam)] + 1;
                            }
                        }
                        else
                        {
                            Derpen[i] = 99;
                        }
                    }
                }
                else
                {
                    if (vlFechaFallecimiento != "")
                    {
                        Derpen[i] = 10;
                    }
                    else
                    {
                        Derpen[i] = 99;
                    }
                }
                TablaResul.DerPen = Derpen[i].ToString();
                i = i + 1;
            }

            cont_causante = 0;
            cont_esposa = 0;
            cont_mhn_tot = 0;
            cont_hijo = 0;
            cont_padres = 0;
            cont_esp_Totales = 0;
            //cont_mhn_Totales = 0;
            for (int g = 0; g <= vlNum_Ben - 1; g++)
            {
                if (Derpen[g] != 10)
                {
                    if (Ncorbe[g] == 99)
                    {
                        cont_causante = cont_causante + 1;
                    }
                    else
                    {
                        switch (Ncorbe[g])
                        {
                            case 10:
                                cont_esposa = cont_esposa + 1;
                                break;
                            case 11:
                                cont_esposa = cont_esposa + 1;
                                break;
                            case 20:
                                Q = Cod_Grfam[g];
                                cont_mhn[Q] = cont_mhn[Q] + 1;
                                cont_mhn_tot = cont_mhn_tot + 1;
                                break;
                            case 21:
                                Q = Cod_Grfam[g];
                                cont_mhn[Q] = cont_mhn[Q] + 1;
                                cont_mhn_tot = cont_mhn_tot + 1;
                                break;
                            case 30:
                                Q = Cod_Grfam[g];
                                Hijos[Q] = Hijos[Q] + 1;
                                if (Coinbe[g] != "N") { Hijos_Inv[Q] = Hijos_Inv[Q] + 1; };
                                Hijo_Menor[Q] = new DateTime(Nanbe[g], Nmnbe[g], Ndnbe[g]);
                                if (Hijos[Q] > 1)
                                {
                                    if (Hijo_Menor[Q] > Hijo_Menor_Ant[Q]) ;
                                    {
                                        Hijo_Menor_Ant[Q] = Hijo_Menor[Q];
                                    }
                                }
                                else
                                {
                                    Hijo_Menor_Ant[Q] = Hijo_Menor[Q];
                                }

                                edad_mes_ben = fecha_sin - (Nanbe[g] * 12 + Nmnbe[g]);

                                if (Coinbe[g] == "N" && edad_mes_ben <= L18)
                                {
                                    cont_hijo = cont_hijo + 1;
                                }
                                else
                                {
                                    if (Coinbe[g] == "T" || Coinbe[g] == "P")
                                    {
                                        cont_hijo = cont_hijo + 1;
                                    }
                                }
                                break;
                            case 41:
                                cont_padres = cont_padres + 1;
                                break;
                            case 42:
                                cont_padres = cont_padres + 1;
                                break;

                        }
                    }
                }
                else
                {
                    switch (Ncorbe[g])
                    {
                        case 11:

                            DateTime DatFalTit = DateTime.ParseExact(vlFechaFallCau, "d", null);
                            DateTime DatFalBen = DateTime.ParseExact(Fec_Fall[g], "d", null);

                            if (DatFalTit > DatFalBen)
                            {
                                cont_esposa = cont_esposa + 1;
                                Q = Cod_Grfam[g];
                                cont_esp_Tot_GF[Q] = cont_esp_Tot_GF[Q] + 1;
                                cont_esp_Totales = cont_esp_Totales + 1;
                            }
                            break;
                        case 21:
                            break;
                    }

                }

            };

            /////////////////////////////////////////////////////////////////////////////////////////////////////

            //j = 1;
            for (int j = 0; j <= vlNum_Ben - 1; j++)
            {
                if (Coinbe[j] == null)
                {
                    Coinbe[j] = null;
                }
                if (Derpen[j] != 10)
                {
                    edad_mes_ben = fecha_sin - (Nanbe[j] * 12 + Nmnbe[j]);
                    switch (Ncorbe[j])
                    {
                        case 99:

                            if (cont_causante > 1)
                            {
                                return TablaResulList;
                            }
                            vlValor = (double)ValPorcentajes.Where(be => be.COD_PAR == Ncorbe[j] && be.COD_SITINV == Coinbe[j] && be.COD_SEXO == Sexobe[j]).Select(be => be.PRC_PENSION).SingleOrDefault(); //ValorPrc(Ncorbe[j].ToString(), Coinbe[j], Sexobe[j], fec);
                            if (vlValor < 0)
                            {
                                return TablaResulList;
                            }
                            else
                            {
                                Porben[j] = vlValor;
                                Codcbe[j] = "N";
                            }
                            break;
                        case 10:
                        case 11:
                            if (sexo_cau == "M")
                            {
                                if (Sexobe[j] != "F")
                                {
                                    msj = "Error de código de sexo, el Sexo de la Cónyuge debe ser Femenino.";

                                    Resultado.Mensaje = msj;
                                    TablaResulList.Add(Resultado);
                                    return TablaResulList;
                                }
                            }
                            else
                            {
                                if (Sexobe[j] != "M")
                                {
                                    msj = "Error de codigo de sexo, el Sexo del Cónyuge debe ser Masculino.";

                                    Resultado.Mensaje = msj;
                                    TablaResulList.Add(Resultado);
                                    return TablaResulList;
                                }
                            }
                            u = Cod_Grfam[j];
                            if (Hijos[u] == 0 && Ncorbe[j] == 11)
                            {
                                if (Hijos_SinDerechoPension[u] > cont_hijo)
                                {
                                    Ncorbe[j] = 10;
                                    TablaResul.CodPar = Ncorbe[j].ToString();
                                }
                            }

                            vlValor = (double)ValPorcentajes.Where(be => be.COD_PAR == Ncorbe[j] && be.COD_SITINV == Coinbe[j] && be.COD_SEXO == Sexobe[j]).Select(be => be.PRC_PENSION).SingleOrDefault();

                            if (vlValor < 0)
                            {
                                msj = "Valor del Porcentaje registrado es <= a Cero para el parentesco ";

                                Resultado.Mensaje = msj;
                                TablaResulList.Add(Resultado);
                                return TablaResulList;
                            }
                            Codcbe[j] = "N";
                            if (sexo_cau == "M" || sexo_cau == "F")
                            {
                                Porben[j] = vlValor / cont_esposa; //CDbl(Format(vlValor / cont_esposa, "#0.00"))
                                if (Hijos[u] > 0)
                                {
                                    Codcbe[j] = "S";
                                    if (Hijos_Inv[u] > 0) { Codcbe[j] = "N"; };
                                }
                                if (Hijos[u] > 0 && Ncorbe[j] == 10)
                                {
                                    msj = "Error de código de relación, 'Cónyuge Sin Hijos con Dº Pensión', tiene Hijos.";

                                    Resultado.Mensaje = msj;
                                    TablaResulList.Add(Resultado);
                                    return TablaResulList;
                                }
                            }
                            break;
                        case 20:
                        case 21:
                            if (sexo_cau == "M")
                            {
                                if (Sexobe[j] != "F")
                                {
                                    msj = "Error de código de sexo, el Sexo de la Cónyuge debe ser Femenino.";

                                    Resultado.Mensaje = msj;
                                    TablaResulList.Add(Resultado);
                                    return TablaResulList;
                                }
                            }
                            else
                            {
                                if (Sexobe[j] != "M")
                                {
                                    msj = "Error de codigo de sexo, el Sexo del Cónyuge debe ser Masculino.";

                                    Resultado.Mensaje = msj;
                                    TablaResulList.Add(Resultado);
                                    return TablaResulList;
                                }
                            }
                            u = Cod_Grfam[j];
                            if (Hijos[u] == 0 && Ncorbe[j] == 21)
                            {
                                if (Hijos_SinDerechoPension[u] > cont_hijo)
                                {
                                    Ncorbe[j] = 20;
                                    TablaResul.CodPar = Ncorbe[j].ToString();
                                }
                            }

                            vlValor = (double)ValPorcentajes.Where(be => be.COD_PAR == Ncorbe[j] && be.COD_SITINV == Coinbe[j] && be.COD_SEXO == Sexobe[j]).Select(be => be.PRC_PENSION).SingleOrDefault();

                            if (vlValor < 0)
                            {
                                msj = "Valor del Porcentaje registrado es <= a Cero para el parentesco ";

                                Resultado.Mensaje = msj;
                                TablaResulList.Add(Resultado);
                                return TablaResulList;
                            }
                            else
                            {
                                Porben[j] = vlValor / cont_mhn_tot;
                            }

                            Codcbe[j] = "N";

                            break;
                        case 30:
                            Codcbe[j] = "N";
                            Q = Cod_Grfam[j];

                            if (cont_esposa > 0 || cont_mhn[Q] > 0)
                            {
                                if (Coinbe[j] == "N" && edad_mes_ben > L18)
                                {
                                    Porben[j] = 0;
                                }
                                else
                                {
                                    if ((Coinbe[j] == "P" || Coinbe[j] == "T") && edad_mes_ben > L18)
                                    {
                                        vlValor = (double)ValPorcentajes.Where(be => be.COD_PAR == Ncorbe[j] && be.COD_SITINV == Coinbe[j] && be.COD_SEXO == Sexobe[j]).Select(be => be.PRC_PENSION).SingleOrDefault();
                                        if (vlValor < 0)
                                        {
                                            msj = "Valor del Porcentaje registrado es <= a Cero para el parentesco " + Ncorbe[j] + ".";

                                            Resultado.Mensaje = msj;
                                            TablaResulList.Add(Resultado);
                                            return TablaResulList;
                                        }
                                        else
                                        {
                                            Porben[j] = vlValor;
                                        }
                                    }
                                    else
                                    {
                                        vlValor = (double)ValPorcentajes.Where(be => be.COD_PAR == Ncorbe[j] && be.COD_SITINV == Coinbe[j] && be.COD_SEXO == Sexobe[j]).Select(be => be.PRC_PENSION).SingleOrDefault();
                                        if (vlValor < 0)
                                        {
                                            msj = "Valor del Porcentaje registrado es <= a Cero para el parentesco " + Ncorbe[j] + ".";

                                            Resultado.Mensaje = msj;
                                            TablaResulList.Add(Resultado);
                                            return TablaResulList;
                                        }
                                        else
                                        {
                                            Porben[j] = vlValor;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Q = Cod_Grfam[j];
                                Codcbe[j] = "N";
                                if (cont_esposa == 0 && cont_mhn[Q] == 0)
                                {
                                    if ((Coinbe[j] == "P" || Coinbe[j] == "T") && edad_mes_ben > L18)
                                    {
                                        vlValor = (double)ValPorcentajes.Where(be => be.COD_PAR == Ncorbe[j] && be.COD_SITINV == Coinbe[j] && be.COD_SEXO == Sexobe[j]).Select(be => be.PRC_PENSION).SingleOrDefault();
                                        if (vlValor < 0)
                                        {
                                            msj = "Valor del Porcentaje registrado es <= a Cero para el parentesco " + Ncorbe[j] + ".";

                                            Resultado.Mensaje = msj;
                                            TablaResulList.Add(Resultado);
                                            return TablaResulList;
                                        }
                                        else
                                        {
                                            Porben[j] = vlValor;
                                            vlValorHijo = vlValor;
                                        }
                                    }
                                    else
                                    {
                                        vlValor = (double)ValPorcentajes.Where(be => be.COD_PAR == Ncorbe[j] && be.COD_SITINV == Coinbe[j] && be.COD_SEXO == Sexobe[j]).Select(be => be.PRC_PENSION).SingleOrDefault();
                                        if (vlValor < 0)
                                        {
                                            msj = "Valor del Porcentaje registrado es <= a Cero para el parentesco " + Ncorbe[j] + ".";

                                            Resultado.Mensaje = msj;
                                            TablaResulList.Add(Resultado);
                                            return TablaResulList;
                                        }
                                        else
                                        {
                                            vlValorHijo = vlValor;
                                            Porben[j] = vlValor;
                                        }
                                    }
                                    vlValor = (double)ValPorcentajes.Where(be => be.COD_PAR == 10 && be.COD_SITINV == "N" && be.COD_SEXO == "F").Select(be => be.PRC_PENSION).SingleOrDefault();  //ValorPrc("10", "N", "F", fec);//DEstudio.Valor_Porcentaje("10", "N", "F", ifechaIniVig)
                                    if (vlValor < 0)
                                    {
                                        msj = "Valor del Porcentaje registrado es <= a Cero para el parentesco 10.";

                                        Resultado.Mensaje = msj;
                                        TablaResulList.Add(Resultado);
                                        return TablaResulList;
                                    }
                                    else
                                    {
                                        v_hijo = vlValor;
                                        if (Coinbe[j] == "N" && edad_mes_ben <= L18)
                                        {
                                            if (cont_hijo == 1)
                                            {
                                                Porben[j] = v_hijo;
                                            }
                                            else
                                            {
                                                Porben[j] = v_hijo / cont_hijo + vlValorHijo;
                                            }
                                        }
                                        else
                                        {
                                            if (Coinbe[j] == "T" || Coinbe[j] == "N")
                                            {
                                                if (cont_hijo == 1)
                                                {
                                                    Porben[j] = v_hijo;
                                                }
                                                else
                                                {
                                                    Porben[j] = v_hijo / cont_hijo + vlValorHijo;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        //fin case 30
                        case 41:
                        case 42:
                            vlValor = (double)ValPorcentajes.Where(be => be.COD_PAR == Ncorbe[j] && be.COD_SITINV == Coinbe[j] && be.COD_SEXO == Sexobe[j]).Select(be => be.PRC_PENSION).SingleOrDefault();
                            if (vlValor < 0)
                            {
                                msj = "Valor del Porcentaje registrado es <= a Cero para el parentesco " + Ncorbe[j] + ".";

                                Resultado.Mensaje = msj;
                                TablaResulList.Add(Resultado);
                                return TablaResulList;
                            }
                            else
                            {
                                Codcbe[j] = "N";
                                Porben[j] = vlValor;
                            }
                            break;
                    }
                }
                vlSumaPensionesHijosSC = vlSumaPensionesHijosSC + Porben[j];
            }


            for (int k = 0; k <= vlNum_Ben - 1; k++)
            {
                if (Derpen[k] != 10)
                {
                    switch (Ncorbe[k])
                    {
                        case 11:
                            Q = Cod_Grfam[k];
                            Fec_NacHM[k] = Hijo_Menor_Ant[Q];
                            break;
                        case 10:
                            Q = Cod_Grfam[k];
                            Fec_NacHM[k] = Hijo_Menor_Ant[Q];
                            break;
                    }
                }
            }

            vlSumarTotalPorcentajePension = 0;
            vlSumaPjePenPadres = 0;
            //j = 0;

            int p = 0;
            DateTime fechaInicio = new DateTime(1899, 12, 30);
            foreach (var itemR in model)
            {
                if (Porben[p] != 0)
                {
                    itemR.PrcLeg = Porben[p];  //Format(Porben[j], "#0.00");
                    itemR.PrcPen = Porben[p]; //Format(Porben[j], "#0.00");
                }
                else
                {
                    itemR.PrcLeg = 0;
                    itemR.PrcPen = 0;
                }

                if (Codcbe[p] != "")
                {
                    //GUARDAR EL DERECHO A ACRECER DE LOS BENEFICIARIOS
                    itemR.derCre = Codcbe[p];
                }
                else
                {
                    //POR DEFECTO NEGAR EL DERECHO A ACRECER DE LOS BENEFICIARIOS
                    itemR.derCre = "N";
                }


                if (Fec_NacHM[p].ToString() != "")
                {
                    if (Fec_NacHM[p] > fechaInicio)
                    {
                        //GUARDAR LA FECHA DE NACIMIENTO DEL HIJO MENOR DE LA CÓNYUGE
                        datFechaNac = Fec_NacHM[p];
                        itemR.NacHM = datFechaNac.Year.ToString("0000") + datFechaNac.Month.ToString("00") + datFechaNac.Day.ToString("00");
                    }
                    else
                    {
                        itemR.NacHM = "";
                    }
                }
                else
                {
                    itemR.NacHM = "";
                }

                //debe convertir las fechas a formato dd/mm/yyyy

                if (itemR.DerPen != "10" && CopiaBen[p].CodPar != "99" && CopiaBen[p].CodPar != "0")
                {
                    vlSumarTotalPorcentajePension = vlSumarTotalPorcentajePension + itemR.PrcLeg;
                }

                if ((itemR.DerPen != "10") && (CopiaBen[p].CodPar == "41" || CopiaBen[p].CodPar == "42") && (CopiaBen[p].CodPar != "0"))
                {
                    vlSumaPjePenPadres = vlSumaPjePenPadres + itemR.PrcLeg;
                }

                itemR.CodPar = Ncorbe[p].ToString();

                p = p + 1;
            }

            //'Validar si los Porcentajes de Pensión Suman más del 100%

            //'Validacion de existencia de padres y eliminar o disminuir su % segun corresponda

            if (cont_padres > 0 && vlSumarTotalPorcentajePension > 100)
            {
                if (vlSumarTotalPorcentajePension - vlSumaPjePenPadres <= 100)
                {
                    int copia = 0;
                    foreach (var itemR in model)
                    {
                        if ((itemR.DerPen != "10") && (CopiaBen[copia].CodPar == "41" || CopiaBen[copia].CodPar == "42") && (CopiaBen[copia].CodPar != "0"))
                        {
                            vlPorcentajeRecal = (100 - (vlSumarTotalPorcentajePension - vlSumaPjePenPadres)) / cont_padres;
                            itemR.PrcPen = vlPorcentajeRecal;
                            itemR.PrcLeg = vlPorcentajeRecal;
                        }
                        copia = copia + 1;
                    }
                }
                else
                {
                    int copia = 0;
                    foreach (var itemR in model)
                    {
                        if ((itemR.DerPen != "10") && (CopiaBen[copia].CodPar == "41" || CopiaBen[copia].CodPar == "42") && (CopiaBen[copia].CodPar != "0"))
                        {
                            vlPorcentajeRecal = 0;
                            itemR.PrcPen = vlPorcentajeRecal;
                            itemR.PrcLeg = vlPorcentajeRecal;
                        }
                        copia = copia + 1;
                    }
                    vlSumaDef = 0;
                    int copia2 = 0;
                    foreach (var itemR in model)
                    {
                        if ((itemR.DerPen != "10") && (CopiaBen[copia2].CodPar != "99" && CopiaBen[copia2].CodPar != "0"))
                        {
                            vlPorcentajeRecal = (itemR.PrcLeg / (vlSumarTotalPorcentajePension - vlSumaPjePenPadres)) * 100;
                            itemR.PrcPen = vlPorcentajeRecal;
                            itemR.PrcLeg = vlPorcentajeRecal;
                        }
                        copia2 = copia2 + 1;
                    }
                    if ((vlDif < 0) || (vlDif > 0))
                    {
                        int copia3 = 0;
                        foreach (var itemR in model)
                        {
                            if ((itemR.DerPen != "10") && (CopiaBen[copia3].CodPar != "99" && CopiaBen[copia3].CodPar != "0"))
                            {
                                vlPorcentajeRecal = itemR.PrcPen + vlDif;
                                itemR.PrcPen = vlPorcentajeRecal;
                                itemR.PrcLeg = vlPorcentajeRecal;
                            }
                            copia3 = copia3 + 1;
                        }
                    }
                }
            }
            else
            {
                if (vlSumarTotalPorcentajePension > 100)
                {
                    vlSumaDef = 0;
                    int copia4 = 0;
                    foreach (var itemR in model)
                    {
                        if ((itemR.DerPen != "10") && (CopiaBen[copia4].CodPar != "99" && CopiaBen[copia4].CodPar != "0"))
                        {
                            vlPorcentajeRecal = (itemR.PrcLeg / vlSumarTotalPorcentajePension) * 100;
                            itemR.PrcPen = Math.Round(vlPorcentajeRecal, 2);
                            itemR.PrcLeg = Math.Round(vlPorcentajeRecal, 2);
                        }
                        vlSumaDef = vlSumaDef + vlPorcentajeRecal;
                        vlDif = 100 - vlSumaDef;
                        copia4 = copia4 + 1;
                    }
                    if ((vlDif < 0 || vlDif > 0) && (cont_esposa > 0 || cont_mhn_tot > 0))
                    {
                        int copia5 = 0;
                        foreach (var itemR in model)
                        {
                            if ((itemR.DerPen != "10") && (CopiaBen[copia5].CodPar != "99" && CopiaBen[copia5].CodPar != "0"))
                            {
                                vlPorcentajeRecal = itemR.PrcPen + vlDif;
                                itemR.PrcPen = Math.Round(vlPorcentajeRecal, 2);
                                itemR.PrcLeg = Math.Round(vlPorcentajeRecal, 2);
                                break;
                            }
                        }
                    }
                }
            }

            double vlRemuneracion = 0;
            double vlPrcCobertura = 0;
            //Recalcular Porcentajes si se trata de un Caso de Invalidez - Con Cobertura

            //switch (pen)
            //{
            //    case "T":
            //        pen = "06";
            //        break;
            //    case "P":
            //        pen = "07";
            //        break;
            //}


            if ((pen == "06" || pen == "07") && (cob == "S"))
            {
                //if (pen == "I") 
                //{ 
                //    vlPorcentajeRecal = 70; 
                //};
                //if (pen == "P") 
                //{
                //    vlPorcentajeRecal= 50;  
                //};

                //itemR.PrcPen = vlPorcentajeRecal;
                if (ObtienePorCob(pen, fec, ref vlRemuneracion, ref vlPrcCobertura) == true)
                {
                    vlRemuneracion = 1000;
                    if (pen == "06")
                    {
                        vlPrcCobertura = 70;
                    }
                    else
                    {
                        vlPrcCobertura = 50;
                    }

                    //Determinar la Remuneración Promedio para el Causante
                    vlRemuneracionBase = vlRemuneracion * (vlPrcCobertura / 100);
                    //Determinar para cada Beneficiario el Nuevo Porcentaje
                    int copia6 = 0;
                    foreach (var itemR in model)
                    {
                        if (itemR.DerPen != "10")
                        {
                            if (CopiaBen[copia6].CodPar == "99")
                            {
                                vlRemuneracionProm = vlRemuneracion * (vlPrcCobertura / 100);
                            }
                            else
                            {
                                vlRemuneracionProm = vlRemuneracion * (itemR.PrcPen / 100);
                            }
                        }
                        vlPorcentajeRecal = (vlRemuneracionProm / vlRemuneracionBase) * 100;
                        itemR.PrcPen = vlPorcentajeRecal;
                        copia6++;
                    }
                }
            }

            return model;
        }

        public double ValorPrc(string par, string inv, string sex, string fec)
        {
            try
            {
                //hacer el query


                return 70;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public bool ObtienePorCob(string pen, string fec, ref double mtorem, ref double prccob)
        {
            try
            {
                mtorem = 10;
                prccob = 50;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
