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

    class RutinaAct
    {
        #region RutinaActuarial
        //'*************************************************************************************************************************************************************'
        //'*****************************************RUTINA MIGRADA POR EL DEPATARMENTE DE SISTEMAS Y DESARROLLO DE SOFTWARE*********************************************'
        //'*****************************************PROGRAMADOR : ROGER RAMIREZ *****************************RRR********************************************************'
        //'*****************************************FECHA DE MIGRACION Y ASJUTES DE LINEAS DE CODIGO 20/03/2018
        //'*************************************************************************************************************************************************************'

        public string msj { get; set; }

        public beResultados RutinaPension(
            List<beDatosModalidad> ModelCot, List<beDatosBen> ModelBen,
            List<beDatosTasasPar> ModelTas, List<beTasaMercado> ModelTM,
            List<beTasaAnclaje> ModelTA, List<beMortalidad> ModelMor,
            List<beCPK> ModelCPK, List<beRentabilidad> ModelRen,
            List<beTasaFacVac> ModelFacVac, List<bePorcenLegales> ModelPorcenLeg)
        {

            beResultados Resultados = new beResultados();
            beResultados ListaResultador = new beResultados();
            RutinaPorcentaje RP = new RutinaPorcentaje();
            List<beDatosBen> ModelBenTmp = new List<beDatosBen>();
            //ListaResultador = null;

            #region Variables
            string vlFechaNacCausante, vlSexoCausante, Npolca, Mone, Depto, Cober, Alt, Indi, cplan, Sql, Numero, vlNumCot, cob, alt1, tip;
            string MarcaSob;
            //string[] Coinb = new string[20], Codcbe = new string[20], Sexob = new string[20], ;
            double tvmax, salcta_eva, vppen = 0, vpcm, vppfactor, Vpptem, Add_porc_be, Totpofr = 0, Rete_sim = 0, pensim = 0;
            double tasac_t = 0, gastos = 0, rdeuda = 0, timp = 0, penmax = 0, gasemi = 0, facdec = 0, PERMAX = 0;
            bool vlExisteDepto = false;
            double MontoFin = 0;
            long mesdiftmp, mesconTmp;
            int Fintab = 0, cuenta = 0;
            int Nben, Nap, Nmp, Ndp, Nad, Nmd, Ndd, NumCor, TipRea;
            string TipPen, TipRen, TipMod, ind_cob, FecCot, FecDev, FecSol, DerCre, DerGra, RegEst, FecRvt;
            double Valmon, MtoCic, MtoPri, GtoFun, PrcAfp, PrcTaf, PrcCom, PrcTri, PrcMen, PrcAnu, mtoMinPri, prcRenCom, MinRC, RepRC, sumaporcsob = 0, sumaporcsobrv = 0, sumaporcsobdV = 0;
            long EdaLim;
            long iRes;
            int vlVecesCot = 1;
            double Totpor;
            string fecha;
            int l = 0;
            long r = 0;
            int i = 0;
            int j;
            int t;
            int k;
            long nmax;
            double mto, px = 0, py = 0, qx = 0, qxt = 0, tpx = 0;
            double tasac;
            long Mesdif;
            long Mesgar;
            long mescosto;
            double tmm;
            double tm3, gto_supervivencia = 0, tirvta = 0, tinc = 0, sumaex = 0, sumaex1 = 0, tirmax = 0;
            double MesCostoTmp;
            long mesdif1, pergar, pergar1, mesdifc, perdif;
            long Fechap, Fechrv, FechaDv, mescon;
            int Tipajus = 0;
            double sumCostosN, sumaCostTotalMes, mescotoAlt;
            double vgFactorAjusteIPC = 0;
            double vl_FactorMensual = 0;
            double vl_FactorTrimestral = 0;
            double factorPorPen = 0;
            int ni = 0;
            int ns = 0;
            long edaca = 0, edalca = 0, edacai = 0, edacas = 0, edabe = 0, edalbe = 0, edaberv = 0, edadedv = 0, nibe = 0, nsbe = 0, edbedi = 0, nmdiga = 0, Ebedif = 0, edacax = 0;
            double rmpol, relres, pension;
            long limite, limite1, limite2, limite3, limite4, imas1, nt = 0, kdif, numbep = 0, nmdifi;
            double sumapenben, penanuAFP, facfam, FxVac, PenBase, vppenAfp, ival, SalCtaAfp, sumapx, sumaqx, penanu, actual, actua1 = 0, tce;
            double tci, vpte, vpte2, vppenres, vpcmres, tasatirc, difres, difre1, tir, TINC1, vlSumaPension, penanuFinal, vlSumPension = 0;
            string DerCrecer = "", swg = "", DerGratificacion = "";
            long nmdif = 0, valpx, ax, MesHijoDif_EdaLim;
            double new_prc = 0, vld_saldo = 0, Tasa;
            long mdif = 0;
            double reserva, PERDI, vld_comision, vld_impuesto, vld_puesta, flupag, gto_inicial, Comision, margen, vlMargenDespuesImpuesto, rend, vld_gtosbs;
            double resfin = 0, varrm, resant, tastceM, gto, tirmax_ori, TTirMax, RM, NewPen, perdis, tassim, tasa_tir, tasa_vta, tasa_tce, tprc_per, vlPenGar = 0;
            double vld_PensionAnual = 0, vld_ReservaSepelio = 0, vld_ReservaPensiones = 0;
            double tasanc = 0.00001;
            int icont10 = 0, icont20, icont11, icont21, icont30, icont35, icont40, icont77, icont30Inv;
            long vlContarMaximo;
            long Fechan;
            int at;
            int IM_a = 0, IM_m = 0, IM_d = 0;
            long edhm = 0;
            double porfam = 0;
            int[] Orden = new int[20];
            int[] Ncorbe = new int[20];
            int[] Nanbe = new int[20];
            int[] Nmnbe = new int[20];
            int[] Ndnbe = new int[20];
            int[] Ijam = new int[20];
            int[] Ijmn = new int[20];
            int[] Ijdn = new int[20];
            int[] isuc = new int[20];
            double[] porcbe_ori = new double[20];
            string[] Sexob = new string[20];
            string[] Coinb = new string[20];
            string[] Codcb = new string[20];
            double[,,] Lx = new double[3, 4, 1333];
            double[,,] Ly = new double[3, 4, 1333];
            double[] Penben = new double[20];
            double[] Porcbe = new double[20];
            double[] PorcbeSob = new double[20];
            double[] Porcbe_tram = new double[20];
            double[] fpxAfp = new double[1333];
            double[] fpx = new double[1333];
            double[] valotemp = new double[1333];
            double[] fpy = new double[1333];
            double[] valotempRC = new double[1333];
            double[] fqx = new double[1333];
            double[] PensionBenef = new double[1333];
            double[] vl_FactorReajuste = new double[1333];
            double[] facgratif = new double[1333];
            double[] Cp = new double[1333];
            double[] Prodin = new double[1333];
            double[] Flupen = new double[1333];
            double[] Flucm = new double[1333];
            double[] Exced = new double[1333];
            double[] FlupenVal = new double[1333];
            double[] valPY = new double[1333];
            double[] FlupenRC = new double[1333];
            double[] Arr_Tasas = new double[1333];
            double[] factual = new double[1333];
            double[] factualqx = new double[1333];
            double[] fqxt = new double[1333];
            double[] fqxy = new double[1333];
            double[] ftpx = new double[1333];
            double[] fcru = new double[1333];
            double[] fcruGS = new double[1333];
            double[] fRestci = new double[1333];
            double[] fTramos = new double[1333];
            double[] vl_FactorReaPaso = new double[1333];
            double vl_sumacosto = 0;

            long ltot = 0;
            bool vlExisteTM = false;
            bool vlExisteTA = false;
            double tm = 0;
            double ta = 0;
            double prcPen = 0;
            decimal Exp = (decimal)1 / 12;
            string msj = "";
            int cr = 0;
            double dUtilImp = 0, dCapit = 0, dvarCap = 0, dComis = 0, dGasMan = 0, dPagos = 0, dVarRes = 0, dProdInv = 0, dImp = 0, dMarSol = (6.75 / 100), dResTCI = 0;
            double dvarCapAnt = 0, resfinAnt = 0, dflupag = 0;
            int an = 0;

            #endregion

            foreach (var item in ModelCot)
            {
                #region CargaVariable
                Fintab = item.FinTab;
                Nben = ModelBen.Count - 1;
                Cober = item.Tippen;  //cober
                TipPen = item.Tippen;  //TipoPension
                TipRen = item.TipRen; //Indi
                TipMod = item.TipMod; //alt
                Mesgar = item.MesGar;
                Mone = item.CodMon;
                ind_cob = item.IndCob;
                Depto = item.CodReg;
                Valmon = item.ValCam; //MtoMoneda
                FecCot = item.FecCot;
                Nap = int.Parse(item.FecCot.Substring(0, 4));
                Nmp = int.Parse(item.FecCot.Substring(4, 2));
                Ndp = int.Parse(item.FecCot.Substring(6, 2));
                if (Mone != "NS") { MtoPri = item.MtoPri / Valmon; } else { MtoPri = item.MtoPri; };
                MtoCic = MtoPri; //SalCta
                Mesdif = item.MesDif;
                FecDev = item.FecDev;
                FecSol = item.DevSol; //FecDevSol
                Nad = int.Parse(item.FecDev.Substring(0, 4)); //Nad
                Nmd = int.Parse(item.FecDev.Substring(4, 2)); //Nmd
                Ndd = int.Parse(item.FecDev.Substring(6, 2)); //Ndd
                GtoFun = item.MtoGS;
                PrcAfp = (double)item.RenAfp / 100; //Prc_Tasa_Afp
                PrcTaf = (double)item.RenTmp / 100; //PrcTaf
                NumCor = item.NumCor; //vlCorrCot
                if (item.NumCot == null)
                {
                    vlNumCot = "0";
                }
                else
                {
                    vlNumCot = item.NumCot;
                }
                //FecRvt = FecDev + Mesdif
                PrcCom = item.PrcCom; //comisi
                DerCre = item.DerCre; //DerCrecer
                DerGra = item.DerGra; //DerGratificacion
                EdaLim = item.EdaLim; //EdaLim
                RegEst = item.RegEst; //Cod_DeptoEstandar
                TipRea = item.TipRea; //Tipajus
                PrcTri = item.PrcTri; //vl_FactorTrimestral
                PrcMen = item.PrcMen; //vl_FactorMensual
                PrcAnu = item.PrcAnu; //vl_FactorAnual
                FecRvt = DateTime.Parse(Ndd + "/" + Nmd + "/" + Nad).AddMonths(item.MesDif).ToString("yyyyMMdd");  //Format(Cdatetime(Ndd & "/" & Nmd & "/" & Nad).AddMonths(Mesdif), "yyyyMMdd")
                MinRC = item.MinRC; //mtoMinPri
                RepRC = item.RepRC / 100; //prcRenCom

                //     If TipoCot = "S" Then
                //    PenBase = CDbl((dr("Pension").Tostring))
                //End If

                //GRATIFICACION
                DateTime fecha1 = new DateTime(Nap, Nmp, 1);

                for (int g = 1; g <= Fintab - 1; g++)
                {
                    facgratif[g] = 1;
                    if ((fecha1.Month == 7 || fecha1.Month == 12) && DerGra == "S")
                    {
                        facgratif[g] = 2;
                    }
                    fecha1 = fecha1.AddMonths(1);
                    //fecha1 = new DateTime(Nap, Nmp + 1, 1);
                    if (g == 1332)
                    {
                        break;
                    }
                };
                switch (TipRen)
                {
                    case "1":
                        TipRen = "I";
                        break;
                    case "2":
                        TipRen = "D";
                        break;
                    case "3":
                        TipRen = "M";
                        break;
                    case "5":
                        TipRen = "C";
                        break;
                    case "6":
                        TipRen = "E";
                        break;

                };

                switch (TipMod)
                {
                    case "1":
                        TipMod = "S";
                        break;
                    case "3":
                        TipMod = "G";
                        break;
                    case "4":
                        TipMod = "F";
                        break;
                }

                switch (TipPen)
                {
                    case "08":
                        TipPen = "S";
                        break;
                    case "07":
                        TipPen = "P";
                        break;
                    case "06":
                        TipPen = "I";
                        break;
                    case "05":
                        TipPen = "V";
                        break;
                    case "04":
                        TipPen = "V";
                        break;
                }

                if (TipRen == "C" || TipRen == "M")
                {
                    MtoCic = MtoCic * RepRC;
                    if (DerGra == "S")
                    {
                        MinRC = (MinRC * 12) / 14;
                    }
                }

                //CARGA LOS DATOS DE LOS BENEFICIARIOS EN LAS VARIABLES
                if (vlVecesCot == 1)
                {
                    vlVecesCot = 2;
                    Totpor = 0;

                    foreach (var itemben2 in ModelBen)
                    {
                        Orden[l] = itemben2.NumOrd;
                        Ncorbe[l] = int.Parse(itemben2.CodPar);
                        if (TipPen == "S")
                        {
                            if (Ncorbe[l] == 99)
                            {
                                goto Next;
                                //sale de aca
                            }
                        }
                        if ((Ncorbe[l] == 99) || (Ncorbe[l] == 0))
                        {
                            vlFechaNacCausante = itemben2.FecNac;
                            vlSexoCausante = itemben2.TipSex;
                        }
                        porcbe_ori[l] = (itemben2.PrcLeg / 100);
                        Porcbe[l] = (itemben2.PrcPen / 100);
                        fecha = itemben2.FecNac;
                        Nanbe[l] = int.Parse(fecha.Substring(0, 4)); //'aa_nac
                        Nmnbe[l] = int.Parse(fecha.Substring(4, 2)); //'mm_nac
                        Ndnbe[l] = int.Parse(fecha.Substring(6, 2)); //'mm_nac

                        Sexob[l] = itemben2.TipSex;
                        Coinb[l] = itemben2.TipInv;
                        Codcb[l] = itemben2.DerPen;

                        if (itemben2.NacHM != "")
                        {
                            fecha = "";
                            fecha = itemben2.NacHM;
                            Ijam[l] = int.Parse(fecha.Substring(0, 4));    //'aa_hijom
                            Ijmn[l] = int.Parse(fecha.Substring(4, 2));    //'mm_hijom
                            Ijdn[l] = int.Parse(fecha.Substring(6, 2));    //'mm_hijom
                        }
                        else
                        {
                            Ijam[l] = 0;    //'aa_hijom
                            Ijmn[l] = 0;    //'mm_hijom
                            Ijdn[l] = 0;    //'mm_hijom
                        }
                        isuc[l] = 0;
                    Next:
                        l = l + 1;
                    }
                }

                //ARMA LOS PARAMETROS EN VARIABLE DE LAS TASAS
                //CARGA LSO PARAMETROS DE FECHA

                //var TGfil = ModelTas.Where(x => x.CodMon == Mone && x.TipPen == Cober && x.CodReg == Depto).ToList();
                var TGfil = ModelTas.Where(x => x.CodMon == Mone && x.TipPen == Cober && x.CodReg == Depto && x.TipRea == TipRea).ToList();
                foreach (var itemGas in TGfil)
                {
                    tasac_t = itemGas.PrcTir;             //'Tasa de Costo Capital
                    gastos = itemGas.MtoGad;              //'Gastos de Administración
                    //gto_supervivencia = Format(CDbl((drpg("PRC_GASCTRSUP").ToString)), "#0.00000")  //'Gastos de Administración
                    rdeuda = itemGas.PrcDeu;              //'Indice de Endeudamiento
                    timp = itemGas.MtoImp;                //'Impuesto
                    penmax = itemGas.PrcTas;              //'tasa_min
                    gasemi = itemGas.MtoGem;              //'Gastos de Emisión
                    facdec = 0; //'                       //'gastos_eva
                    PERMAX = itemGas.PrcPer;              //'Porc. Pérdida Máxima
                    if (Mone != "NS")
                    {
                        gastos = gastos / Valmon;
                        gasemi = gasemi / Valmon;
                        GtoFun = GtoFun / Valmon;
                    }
                    vlExisteDepto = true; //'Si encuentra los parametros del departamento
                    break;
                }
                if (vlExisteDepto == false)
                {
                    //msj = "No se hallaron tasas para EL fondo y comisión selecionados. Comunicarse con el area Tecnica para regularizar.";
                    //salir de funcion
                    msj = "Problemas al hallar tasas.";
                    ListaResultador.Mensaje = msj;
                    return ListaResultador;
                }
                tasac = tasac_t;

                //TASAS DE MERCADO
                var TMfil = ModelTM.Where(x => x.CodMon == Mone && x.TipRea == TipRea).ToList();
                foreach (var itemTM in TMfil)
                {
                    tm = itemTM.PrcVal;
                    vlExisteTM = true; //'Si encuentra los parametros de la TM
                    break;
                }
                if (vlExisteTM == false)
                {
                    msj = "Problemas al hallar tasas de Mercado.";
                    ListaResultador.Mensaje = msj;
                    return ListaResultador;
                }

                //TASA DE ANCLAJE
                var TAfil = ModelTA.Where(x => x.CodMon == Mone && x.TipRea == TipRea).ToList();
                foreach (var itemTA in TAfil)
                {
                    ta = itemTA.PrcVal;
                    vlExisteTA = true; //'Si encuentra los parametros de la TM
                    break;
                }
                if (vlExisteTA == false)
                {
                    msj = "Problemas al hallar tasas de Anclaje.";
                    ListaResultador.Mensaje = msj;
                    return ListaResultador;
                }
                //carga los datos de CPK
                int intFila = 1;

                var CPKfil = ModelCPK.Where(x => x.COD_MONEDA == Mone && x.COD_TIPREAJUSTE == TipRea).ToList();
                if (CPKfil == null)
                {
                    msj = "Problemas al hallar tasas de CPK";
                    ListaResultador.Mensaje = msj;
                    return ListaResultador;
                }
                foreach (var ItemCpk in CPKfil)
                {
                    intFila = ItemCpk.NUM_ANNO;
                    switch (intFila)
                    {
                        case 1:
                            Cp[1] = ItemCpk.PRC_CPK;
                            break;
                        case 2:
                            Cp[2] = ItemCpk.PRC_CPK;
                            break;
                        case 3:
                            Cp[3] = ItemCpk.PRC_CPK;
                            break;
                        case 4:
                            Cp[4] = ItemCpk.PRC_CPK;
                            break;
                        case 5:
                            Cp[5] = ItemCpk.PRC_CPK;
                            break;
                        case 6:
                            Cp[6] = ItemCpk.PRC_CPK;
                            break;
                        case 7:
                            Cp[7] = ItemCpk.PRC_CPK;
                            break;
                        case 8:
                            Cp[8] = ItemCpk.PRC_CPK;
                            break;
                        case 9:
                            Cp[9] = ItemCpk.PRC_CPK;
                            break;
                        case 10:
                            Cp[10] = ItemCpk.PRC_CPK;
                            break;
                    }
                }
                intFila = 1;


                //carga los datos de rentabilidad
                var RentabFil = ModelRen.Where(x => x.COD_MONEDA == Mone && x.COD_TIPREAJUSTE == TipRea).ToList();
                if (RentabFil == null)
                {
                    msj = "Problemas al hallar tasas de Rentabilidad";
                    ListaResultador.Mensaje = msj;
                    return ListaResultador;
                }
                foreach (var itemTA in RentabFil)
                {
                    intFila = itemTA.NUM_ANNO;
                    Arr_Tasas[intFila] = itemTA.PRC_TASAREN;
                }

                int algo = 1;

                for (int m = 1; m <= Fintab; m = m + 12)
                {
                    for (l = m; l <= (m + 12) - 1; l++)
                    {
                        Prodin[l] = (Math.Pow((1 + (Arr_Tasas[algo] / 100)), (double)Exp)) - 1;
                    }
                    algo = algo + 1;
                };

                //Array.Resize(ref Lx[0,0,0], double[2, 3, 1332]);
                //CARGA TABLAS DE MORTALIDAD
                foreach (var itemMor in ModelMor)
                {
                    i = itemMor.i;
                    j = itemMor.j;
                    t = itemMor.h;
                    k = itemMor.k;
                    mto = (double)itemMor.MtoLx;

                    if (t == 1)
                    {
                        Lx[i, j, k] = mto;
                    }
                    else
                    {
                        Ly[i, j, k] = mto;
                    }
                }
                #endregion
                #region Cambios en pension segun las fechas
                if (TipPen != "S")
                {

                    try
                    {
                        //'***************************************************************************************************************************
                        //'1 . -OBTENGO LOS PORCENTAJES A LA FECHA DE PROCESO  ***********************************************************************
                        //'***************************************************************************************************************************

                        List<beDatosBen> CopiaBen = new List<beDatosBen>();
                        foreach (var itembencop in ModelBen)
                        {
                            CopiaBen.Add(new beDatosBen { CodPar = itembencop.CodPar, NacHM = itembencop.NacHM });
                        }
                        ModelBenTmp = RP.PorcentajeBen(ModelBen, FecCot, ind_cob, Cober, EdaLim, ModelPorcenLeg, CopiaBen);
                        foreach (var itemBen in ModelBenTmp)
                        {
                            prcPen = itemBen.PrcPen;
                            sumaporcsob = sumaporcsob + prcPen / 100;
                        }

                        if (TipRen == "D")
                        {
                            //'*********************************************************************************************************
                            //   '2 . -OBTENGO LOS PORCENTAJES A LA FECHA DE LA RENTA VITALICIA ****************************************
                            //'*********************************************************************************************************
                            sumaporcsob = 0;
                            ModelBenTmp = RP.PorcentajeBen(ModelBen, FecRvt, ind_cob, Cober, EdaLim, ModelPorcenLeg, CopiaBen);
                            foreach (var itemBen in ModelBenTmp)
                            {
                                prcPen = itemBen.PrcPen;
                                sumaporcsob = sumaporcsob + prcPen / 100;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //salir de la funcion
                    }
                }
                else
                {
                    try
                    {
                        //'***************************************************************************************************************************
                        //'1 . -OBTENGO LOS PORCENTAJES A LA FECHA DE PROCESO  ***********************************************************************
                        //'***************************************************************************************************************************
                        List<beDatosBen> CopiaBen = new List<beDatosBen>();
                        foreach (var itembencop in ModelBen)
                        {
                            CopiaBen.Add(new beDatosBen { CodPar = itembencop.CodPar, NacHM = itembencop.NacHM });
                        }
                        ModelBenTmp = RP.PorcentajeBen(ModelBen, FecCot, ind_cob, Cober, EdaLim, ModelPorcenLeg, CopiaBen);
                        foreach (var itemBen in ModelBenTmp)
                        {
                            prcPen = itemBen.PrcPen;
                            sumaporcsob = sumaporcsob + prcPen / 100;
                        }

                        //'*********************************************************************************************************
                        //'2 . -OBTENGO LOS PORCENTAJES A LA FECHA DE LA RENTA VITALICIA *******************************************
                        //'*********************************************************************************************************
                        ModelBenTmp = RP.PorcentajeBen(ModelBen, FecRvt, ind_cob, Cober, EdaLim, ModelPorcenLeg, CopiaBen);
                        foreach (var itemBen in ModelBenTmp)
                        {
                            prcPen = itemBen.PrcPen;
                            sumaporcsobrv = sumaporcsobrv + prcPen / 100;
                        }
                        //'*********************************************************************************************************
                        //'OBTENGO LOS PORCENTAJES A LA FECHA DE DEVENGE ***********************************************************
                        //'*********************************************************************************************************
                        ModelBenTmp = RP.PorcentajeBen(ModelBen, FecDev, ind_cob, Cober, EdaLim, ModelPorcenLeg, CopiaBen);
                        foreach (var itemBen in ModelBen)
                        {
                            prcPen = itemBen.PrcPen;
                            sumaporcsobdV = sumaporcsobdV + prcPen / 100;
                        }

                        if (TipRen == "I")
                        {
                            sumaporcsobdV = 0;
                            foreach (var itemBen in ModelBenTmp)
                            {
                                prcPen = itemBen.PrcPen;
                                sumaporcsobdV = sumaporcsobdV + prcPen / 100;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //salir de la funcion
                    }
                }



                //'RRR OBTIENE LOS MESES COSTOS SOLO EN CASO DE SOBREVIVENCIA 31/08/2012 **************************************
                //'************************************************************************************************************
                if (TipPen == "S")
                {
                    if (TipRen == "D")
                    {
                        if (sumaporcsobrv != sumaporcsobdV)
                        {
                            factorPorPen = sumaporcsobdV / sumaporcsobrv;
                        }
                        else
                        {
                            factorPorPen = 1;
                        }
                        PrcTaf = PrcTaf / factorPorPen;
                        sumaporcsob = sumaporcsobrv;
                    }
                }
                //'***********************************************************************************************************
                #endregion
                #region Inicializa y setea variables
                //'inicialización de variables
                PrcCom = PrcCom / 100;
                tasac = tasac / 100;
                timp = timp / 100;
                tmm = (1 + (tm / 100));
                tm3 = (1 + (ta / 100));
                facdec = facdec / 100;
                gto_supervivencia = gto_supervivencia / 100;
                tirvta = 0;
                tinc = 0;
                sumaex = 0;
                sumaex1 = 0;
                tirmax = 0;
                nmax = 0;
                cplan = TipPen;
                for (int ir = 1; ir <= Fintab; ir++)
                {
                    Exced[ir] = 0;
                    Flupen[ir] = 0;
                    Flucm[ir] = 0;
                }

                //Fechap = Nap * 12 + Nmp;
                Fechap = Nad * 12 + Nmd;
                Fechrv = Nad * 12 + Nmd + Mesdif;
                FechaDv = Nad * 12 + Nmd;

                perdif = 0;
                mesdif1 = Mesdif; //'* 12 'debe venir en meses
                pergar = Mesgar;
                pergar1 = Mesgar; // '* 12
                mesdifc = mesdif1;
                mescon = ((Nap * 12) + Nmp) - ((Nad * 12) + Nmd);

                //OBTIENE LOS MESESCOSTOS
                if (mescon < (mesdif1 + pergar1))
                {
                    if (mescon > mesdif1)
                    {
                        Mesdif = 0;
                        mescosto = mescon;
                    }
                    else
                    {
                        if (TipRen == "E")
                        {
                            mescosto = mescon;
                        }
                        else
                        {
                            mescosto = mescon;
                        }
                    }
                }
                else
                {
                    if (mescon > mesdif1)
                    {
                        Mesdif = 0;
                    }
                    mescosto = mescon - (Mesdif + Mesgar);
                }
                MesCostoTmp = mescosto;

                //'Periodo Diferido
                if (mescon > mesdif1)
                {
                    Mesdif = 0;
                }
                else
                {
                    if (mesdif1 > mescon)
                    {
                        Mesdif = mesdif1;
                    }
                    else
                    {
                        Mesdif = (mescon - mesdif1);
                    }
                }

                //'Periodo Garantizado
                if (mescon > (pergar1 + mesdif1))
                {
                    pergar = 0;
                }
                else
                {
                    if (mescon < mesdif1)
                    {
                        pergar = pergar1;
                    }
                    else
                    {
                        //pergar = (pergar1 + mesdif1) - mescon;
                        pergar = (pergar1 + mesdif1);// -mescon;
                    }
                }
                Mesgar = pergar;   // '/ 12



                //mescosto = mescosto;  //'/ 12
                //Mesdif = Mesdif;   // '/ 12

                if (TipRen == "E")
                {
                    ltot = Mesgar;
                    mesdif1 = 0;
                }
                else
                {
                    ltot = Mesgar + Mesdif;
                }
                perdif = Mesdif;
                MarcaSob = "N";

                if (TipRen == "D" && TipPen == "S")
                {
                    icont10 = 0;
                    icont20 = 0;
                    icont11 = 0;
                    icont21 = 0;
                    icont30 = 0;
                    icont35 = 0;
                    icont30Inv = 0;
                    icont40 = 0;
                    icont77 = 0;
                    for (int b = 1; b <= Nben; b++)
                    {
                        nibe = 0;

                        if (Coinb[b] == "S" || Coinb[b] == "T" || Coinb[b] == "I")
                        {
                            nibe = 1;
                        }
                        if (Coinb[b] == "N")
                        {
                            nibe = 2;
                        }
                        if (Coinb[b] == "P")
                        {
                            nibe = 2;
                        }
                        if (nibe == 0)
                        {
                            msj = "Error en variable nibe==0. ";
                            ListaResultador.Mensaje = msj;
                            return ListaResultador;
                        }
                        nsbe = 0;

                        if (Sexob[b] == "M")
                        {
                            nsbe = 1;
                        }
                        if (Sexob[b] == "F")
                        {
                            nsbe = 2;
                        }
                        if (nsbe == 0)
                        {
                            msj = "Error en variable nsbe==0. ";
                            ListaResultador.Mensaje = msj;
                            return ListaResultador;
                        }
                        Fechan = Nanbe[b] * 12 + Nmnbe[b];
                        edabe = Fechap - Fechan;
                        if (edabe < 1)
                        {
                            edabe = 1;
                        }
                        if (edabe > Fintab)
                        {
                            msj = "Error Edad mayor que fin de tabla Mortalidad";
                            ListaResultador.Mensaje = msj;
                            return ListaResultador;
                        }

                        if (Ncorbe[b] == 10 || Ncorbe[b] == 11)
                        {
                            icont10 = icont10 + 1;
                        }
                        if (Ncorbe[b] == 20 || Ncorbe[b] == 21)
                        {
                            icont20 = icont20 + 1;
                        }
                        if (Ncorbe[b] == 30)
                        {
                            icont30 = icont30 + 1;
                        }
                        if (Ncorbe[b] == 30 && Coinb[b] != "N")
                        {
                            icont30Inv = icont30Inv + 1;
                        }
                        if (Ncorbe[b] == 35)
                        {
                            icont35 = icont35 + 1;
                        }
                        if (Ncorbe[b] > 40 && Ncorbe[b] < 50)
                        {
                            icont40 = icont40 + 1;
                        }
                        if (Ncorbe[b] == 77)
                        {
                            icont77 = icont77 + 1;
                        }
                    }


                    if ((icont10 > 0 || icont20 > 0) && icont30 > 0 && icont30Inv == 0)
                    {
                        for (int jp = 0; jp <= Nben; jp++)
                        {
                            if (Ncorbe[jp] == 10 || Ncorbe[jp] == 11 || Ncorbe[jp] == 20 || Ncorbe[jp] == 21)
                            {
                                edhm = (Fechap - (Ijam[jp] * 12 + Ijmn[jp])) + perdif;
                                if (edhm >= EdaLim)
                                {
                                    Porcbe[jp] = 0.42;   //'Valor_Porcentaje("10", "N", "F", iFechaIniVig)
                                    IM_a = Ijam[jp];
                                    IM_m = Ijmn[jp];
                                    IM_d = Ijdn[jp];
                                }
                            }
                            if (edhm >= EdaLim && Ncorbe[jp] == 30 && Nanbe[jp] == IM_a && Nmnbe[jp] == IM_m)
                            {
                                Porcbe[jp] = 0;
                            }
                        }
                    }
                }
                #endregion
                #region Tipo de Ajuste
                if (TipRea == 0)
                {
                    for (int li = 1; li <= Fintab; li++)
                    {
                        vl_FactorReajuste[i] = 1;
                    }
                }

                if (TipRea == 1)
                {

                }
                else
                {
                    vgFactorAjusteIPC = 1;
                }
                k = 0;

                int ki = 0, ki2 = 0, paso1 = 0;
                DateTime FecCot1, FecCot2;
                double TEM = 0;
                double TET = 0;
                double vl_FacRea = 1;

                ax = 0;
                if (TipRea == 2)
                {
                    TEM = 1 + PrcMen / 100;
                    TET = 1 + PrcTri / 100;

                    FecCot1 = new DateTime(Nad, Nmd, Ndd);

                    switch (FecCot1.Month)
                    {
                        case 1:
                        case 4:
                        case 7:
                        case 10:
                            ki2 = 3;
                            paso1 = 3;
                            break;
                        case 2:
                        case 5:
                        case 8:
                        case 11:
                            ki2 = 2;
                            paso1 = 3;
                            break;
                        case 3:
                        case 6:
                        case 9:
                        case 12:
                            ki2 = 1;
                            paso1 = 2;
                            break;
                    }
                    for (ki = 1; ki <= Fintab; ki++)
                    {
                        if (ki == 1)
                        {
                            vl_FactorReajuste[ki] = 1;
                        }
                        else
                        {
                            if (ki <= ki2)
                            {
                                vl_FactorReajuste[ki] = vl_FactorReajuste[ki - 1];// * TEM;
                            }
                            else
                            {
                                if (ki <= paso1)
                                {
                                    vl_FactorReajuste[ki] = vl_FactorReajuste[ki - 1] * Math.Pow(TEM, ki2);
                                }
                                else
                                {
                                    if (FecCot1.Month == 4 || FecCot1.Month == 7 || FecCot1.Month == 10 || FecCot1.Month == 1)
                                    {
                                        vl_FactorReajuste[ki] = vl_FactorReajuste[ki - 1] * TET;
                                    }
                                    else
                                    {
                                        vl_FactorReajuste[ki] = vl_FactorReajuste[ki - 1];
                                    }
                                }
                            }
                        }
                        FecCot1 = FecCot1.AddMonths(1);
                        //FecCot1 = new DateTime(Nad, Nmd + 1, 1);
                    }


                    for (ki = 1; ki <= Fintab; ki++)
                    {
                        vl_FactorReaPaso[ki] = vl_FactorReajuste[ki];
                    }
                    vl_sumacosto = 0;
                    ki2 = 0;

                    //for (ki = 1; ki <= Fintab; ki++)
                    //{
                    //    if (ki > mescon)
                    //    {
                    //        ki2 = ki2 + 1;
                    //        //vl_FactorReajuste[ki2] = vl_FactorReaPaso[ki];
                    //    }
                    //    else
                    //    {
                    //        if (ki > mesdif1)
                    //        {
                    //            vl_sumacosto = vl_sumacosto + vl_FactorReaPaso[ki];
                    //        }
                    //    }
                    //}
                    //mescosto = vl_sumacosto;
                    factorPorPen = 1;
                    sumCostosN = 0;
                    sumaCostTotalMes = 0;
                    mescotoAlt = 0;
                    //'RRR OBTIENE LOS MESES COSTOS SOLO EN CASO DE SOBREVIVENCIA 31/08/2012 **************************************

                    if (TipPen == "S")
                    {
                        valpx = 1;
                        if (MesCostoTmp > 0)
                        {
                            for (ax = 0; ax <= MesCostoTmp - 1; ax++)
                            {
                                sumCostosN = 0;
                                sumaCostTotalMes = 0;
                                for (j = 0; j <= Nben; j++)
                                {
                                    if (Ncorbe[j] == 30)
                                    {
                                        Fechan = Nanbe[j] * 12 + Nmnbe[j];
                                        edabe = ((Nad * 12 + Nmd) - Fechan) + ax;
                                        edaberv = Fechrv - Fechan; //'((Nad * 12 + Nmd + Mesdif) - Fechan)
                                        if (edabe > Fintab)
                                        {
                                            edabe = Fintab;
                                        }
                                        else
                                        {
                                            if (edabe < 1) { edabe = 1; };
                                        }
                                        if (edaberv >= EdaLim && Coinb[j] == "N")
                                        {

                                        }
                                        else
                                        {
                                            if (Coinb[j] != "N")
                                            {
                                                sumCostosN = 1 * Porcbe[j] * vl_FactorReaPaso[ax + 1] * facgratif[ax + 1];
                                            }
                                            else
                                            {
                                                MesHijoDif_EdaLim = EdaLim - edaberv;
                                                if (MesHijoDif_EdaLim < 0) { MesHijoDif_EdaLim = 0; };
                                                if (MesHijoDif_EdaLim == 0) { valpx = 0; };
                                                if (mescon > MesHijoDif_EdaLim)
                                                {
                                                    sumCostosN = valpx * Porcbe_tram[j] * vl_FactorReaPaso[ax + 1] * facgratif[ax + 1];
                                                }
                                                else
                                                {
                                                    sumCostosN = 1 * Porcbe[j] * vl_FactorReaPaso[ax + 1] * facgratif[ax + 1];
                                                }
                                            }
                                        }


                                    }
                                    else
                                    {
                                        sumCostosN = 1 * Porcbe[j] * vl_FactorReaPaso[ax + 1] * facgratif[ax + 1];
                                    }
                                    sumaCostTotalMes = sumaCostTotalMes + sumCostosN;
                                }
                                mescotoAlt = mescotoAlt + sumaCostTotalMes;
                            }
                            //mescosto = mescotoAlt;
                        }
                    }



                    //'************************************************************************************************************


                }
                else
                {
                    FecCot1 = new DateTime(Nad, Nmd, Ndd);
                    for (ki = 1; ki <= Fintab; ki++)
                    {
                        vl_FactorReajuste[ki] = 1;
                    }
                    sumCostosN = 0;
                    sumaCostTotalMes = 0;
                    mescotoAlt = 0;


                    if (TipPen == "S")
                    {
                        if (Mone == "NS")
                        {
                            valpx = 1;
                            if (mescon > 0)
                            {
                                for (ax = 0; ax <= mescon - 1; ax++)
                                {
                                    FecCot2 = new DateTime(Nap, Nmp, Ndp);
                                    FxVac = ModelFacVac.Where(x => x.FEC_INICUOMOR <= FecCot2 && x.FEC_TERCUOMOR >= FecCot2).Select(x => x.MTO_FACTOR).SingleOrDefault();
                                    sumCostosN = 0;
                                    sumaCostTotalMes = 0;
                                    for (j = 0; j <= Nben; j++)
                                    {
                                        if (Ncorbe[j] == 30)
                                        {
                                            Fechan = Nanbe[j] * 12 + Nmnbe[j];
                                            edabe = ((Nad * 12 + Nmd) - Fechan) + ax;
                                            edaberv = Fechrv - Fechan;
                                            if (edabe > Fintab)
                                            {
                                                edabe = Fintab;
                                            }
                                            else
                                            {
                                                if (edabe < 1) { edabe = 1; };
                                            }
                                            if (edaberv >= EdaLim && Coinb[j] == "N")
                                            {

                                            }
                                            else
                                            {
                                                if (Coinb[j] != "N")
                                                {
                                                    sumCostosN = 1 * Porcbe[j] * FxVac * facgratif[ax + 1];
                                                }
                                                else
                                                {
                                                    MesHijoDif_EdaLim = EdaLim - edaberv;
                                                    if (MesHijoDif_EdaLim < 0) { MesHijoDif_EdaLim = 0; };
                                                    if (MesHijoDif_EdaLim == 0) { valpx = 0; };
                                                    if (mescon > MesHijoDif_EdaLim)
                                                    {
                                                        sumCostosN = valpx * Porcbe_tram[j] * FxVac * facgratif[ax + 1];
                                                    }
                                                    else
                                                    {
                                                        sumCostosN = 1 * Porcbe[j] * FxVac * facgratif[ax + 1];
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            sumCostosN = 1 * Porcbe[j] * FxVac * facgratif[ax + 1];
                                        }
                                        sumaCostTotalMes = sumaCostTotalMes + sumCostosN;
                                    }
                                    mescotoAlt = mescotoAlt + sumaCostTotalMes;
                                }
                                //mescosto = mescotoAlt;
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        sumCostosN = 0;
                        sumaCostTotalMes = 0;
                        mescotoAlt = 0;
                        if (Mone == "NS")
                        {
                            sumCostosN = 0;
                            mescotoAlt = 0;
                            valpx = 1;
                            FecCot1 = new DateTime(Nad, Nmd, Ndd);
                            if (mescon > 0)
                            {
                                for (ax = 0; ax <= mescon - 1; ax++)
                                {
                                    FecCot2 = new DateTime(Nap, Nmp, Ndp);
                                    FxVac = ModelFacVac.Where(x => x.FEC_INICUOMOR <= FecCot2 && x.FEC_TERCUOMOR >= FecCot2).Select(x => x.MTO_FACTOR).SingleOrDefault();
                                    sumCostosN = 1 * Porcbe[0] * FxVac * facgratif[ax + 1];
                                    mescotoAlt = mescotoAlt + sumCostosN;
                                }
                                //mescosto = mescotoAlt;
                            }
                        }
                        else
                        {
                            //
                        }
                    }
                }
                #endregion

                #region Calcula Flujos de Pension
                new_prc = 1;
                //flujos para tramos
                for (long g = 0; g <= 1332; g++)
                {
                    fTramos[g] = 1;
                }

                if (TipRen == "E")
                {
                    for (long g = 1; g <= 1332; g++)
                    {
                        if (g > Mesdif)
                        {
                            fTramos[g] = PrcTaf;
                        }
                    }
                }
                //flujos para tramos


                if (TipPen != "S")
                {
                    facfam = 1;
                    for (j = 0; j <= Nben; j++)
                    {
                        for (long g = 0; g <= 1332; g++)
                        {
                            ftpx[g] = 0;
                            fqxt[g] = 0;
                            fpy[g] = 0;
                            fqxy[g] = 0;
                            valotemp[g] = 0;
                        }

                        Penben[j] = Porcbe[j];
                        numbep = numbep + 1;

                        if (Ncorbe[j] == 99 && j == 0)
                        {
                            if (ind_cob == "S")
                            {
                                if (Coinb[j] == "T")
                                {
                                    new_prc = 0.7;
                                }
                                if (Coinb[j] == "P")
                                {
                                    new_prc = 0.5;
                                }
                                Penben[j] = Penben[j] * new_prc;
                            }
                            ni = 0;
                            if (Coinb[j] == "S" || Coinb[j] == "T" || Coinb[j] == "I") { ni = 1; };
                            if (Coinb[j] == "N") { ni = 2; };
                            if (Coinb[j] == "P") { ni = 3; };
                            if (ni == 0)
                            {
                                msj = "Error en variable ni==0. ";
                                ListaResultador.Mensaje = msj;
                                return ListaResultador;
                                //salir de funcion
                            }
                            ns = 0;
                            if (Sexob[j] == "M") { ns = 1; };
                            if (Sexob[j] == "F") { ns = 2; };
                            if (ns == 0)
                            {
                                msj = "Error en variable ns==0. ";
                                ListaResultador.Mensaje = msj;
                                return ListaResultador;
                                //salir de funcion
                            }
                            Fechan = Nanbe[j] * 12 + Nmnbe[j];
                            edaca = FechaDv - Fechan;
                            edacax = edaca;

                            if (edaca < 780 && ns == 1 && ni == 2) { cplan = "A"; };
                            if (edaca < 720 && ns == 2 && ni == 2) { cplan = "A"; };

                            if (edaca <= 0 || edaca > Fintab)
                            {
                                msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                                ListaResultador.Mensaje = msj;
                                return ListaResultador;
                                //salir de funcion
                            }
                            limite1 = Fintab - edaca - 1;
                            nmax = limite1;
                            try
                            {
                                for (i = 0; i <= limite1; i++)
                                {
                                    imas1 = i + 1;
                                    edacax = edaca + i;
                                    edacai = edacax + 1;
                                    edacai = (int)amin0(edacai, Fintab);

                                    if (i < mescosto)
                                    {
                                        tpx = 1;
                                        qxt = 0;
                                        px = 1;
                                        fpx[i] = 1;
                                        fqxt[i] = 1;
                                    }
                                    else
                                    {

                                        qxt = 1 - (Lx[ns, ni, edacai] / Lx[ns, ni, edacax]);

                                        if (i == 0)
                                        {
                                            tpx = 1;
                                            px = tpx;
                                        }
                                        else
                                        {
                                            tpx = ftpx[i - 1] * (1 - fqxt[i - 1]);

                                            if (i < ltot)
                                            {
                                                px = 1;
                                            }
                                            else
                                            {
                                                px = tpx;
                                            }
                                        }

                                    }
                                    Flupen[imas1] = Flupen[imas1] + px * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1] * fTramos[imas1];

                                    //saca el gasto de Sepelio
                                    if (i < mescosto)
                                    {
                                        qx = 0;
                                    }
                                    else
                                    {
                                        if (i < mesdif1)
                                        {
                                            qx = 0;
                                        }
                                        else
                                        {
                                            if (i == 0)
                                            {
                                                qx = 0;
                                            }
                                            else
                                            {
                                                qx = ftpx[i - 1] - tpx;
                                            }

                                        }
                                    }
                                    Flucm[imas1] = Flucm[imas1] + GtoFun * qx;

                                    ftpx[i] = tpx;
                                    fqxt[i] = qxt;
                                    fpx[imas1] = tpx;
                                    fqx[imas1] = qx;
                                    edacas = edacai + 1;
                                    if (edacas == 1332) { break; };

                                }
                            }
                            catch (Exception ex)
                            {
                                msj = "Problemas en los Flujos de Titular de la Rutina. ";
                                ListaResultador.Mensaje = msj;
                                return ListaResultador;
                            }
                        }
                        if (Ncorbe[j] != 99)
                        {
                            Penben[j] = porcbe_ori[j];
                            //Penben[j] = porcbe_ori[j] / new_prc;
                            //actualiza la tabla con la nueva pension en caso de tener cobertura de invalidez
                            //ActualizaXMLDET(pathD, j + 1, "PRC_PENSIONREP", CStr(Penben(j) * 100))
                            nibe = 0;
                            if (Coinb[j] == "S" || Coinb[j] == "T" || Coinb[j] == "I") { nibe = 1; };
                            if (Coinb[j] == "N") { nibe = 2; };
                            if (Coinb[j] == "P") { nibe = 3; };
                            if (nibe == 0)
                            {
                                msj = "Error en variable nibe==0. ";
                                ListaResultador.Mensaje = msj;
                                return ListaResultador;
                                //salir de funcion
                            }
                            nsbe = 0;
                            if (Sexob[j] == "M") { nsbe = 1; };
                            if (Sexob[j] == "F") { nsbe = 2; };
                            if (nsbe == 0)
                            {
                                msj = "Error en variable nibe==0. ";
                                ListaResultador.Mensaje = msj;
                                return ListaResultador;
                                //salir de funcion
                            }
                            edabe = FechaDv - (Nanbe[j] * 12 + Nmnbe[j]);
                            if (edabe < 1) { edabe = 1; };
                            if (edabe > Fintab)
                            {
                                msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                                ListaResultador.Mensaje = msj;
                                return ListaResultador;
                            }
                            if (Ncorbe[j] == 10 || Ncorbe[j] == 11 || Ncorbe[j] == 20 || Ncorbe[j] == 21 || Ncorbe[j] == 41 || Ncorbe[j] == 42 || ((Ncorbe[j] >= 30 && Ncorbe[j] < 40) && (Coinb[j] != "N")))
                            {
                                edacai = 0;
                                limite1 = Fintab - edabe - 1;
                                nmax = (long)amax0(nmax, limite1);
                                int a = 0;
                                try
                                {
                                    for (i = 0; i <= limite1 - 1; i++)
                                    {
                                        imas1 = i + 1;
                                        edalbe = edabe + i;
                                        edacai = edalbe + 1;
                                        edacai = (int)amin0(edacai, Fintab);

                                        if (i < mescosto)
                                        {
                                            tpx = 1;
                                            qxt = 0;
                                            py = 0;
                                            fpy[i] = 1;
                                            fqxy[i] = 1;
                                        }
                                        else
                                        {
                                            qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);
                                            if (i == 0)
                                            {
                                                tpx = 1;
                                                py = tpx;
                                            }
                                            else
                                            {
                                                tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                                if (i < ltot)
                                                {
                                                    py = 0;
                                                }
                                                else
                                                {
                                                    py = tpx;
                                                }
                                            }

                                        }
                                        fpy[i] = tpx;
                                        fqxy[i] = qxt;
                                        valotemp[imas1] = py * (1 - fpx[imas1]) * facgratif[imas1] * Penben[j] * vl_FactorReajuste[imas1] * fTramos[imas1];
                                        Flupen[imas1] = Flupen[imas1] + valotemp[imas1];
                                    }
                                }
                                catch (Exception ex)
                                {
                                    msj = "Problemas en los Flujos de Conyugue, Padres o hijos Invalidos de la Rutina. ";
                                    ListaResultador.Mensaje = msj;
                                    return ListaResultador;
                                }
                            }
                            else
                            {
                                if (Ncorbe[j] >= 30 && Ncorbe[j] < 40)
                                {
                                    //ActualizaXMLDET(pathD, j + 1, "PRC_PENSIONREP", CStr(Penben(j) * 100))
                                    if (edabe > EdaLim)
                                    {
                                    }
                                    else
                                    {
                                        mdif = EdaLim - edabe;
                                        nmdif = mdif;
                                        limite2 = Fintab - edaca;
                                        limite = (long)amin0(nmdif, limite2) - 1;
                                        nmax = (long)amax0(nmax, limite);
                                        try
                                        {
                                            for (i = 0; i <= mdif - 1; i++)
                                            {
                                                imas1 = i + 1;
                                                edalbe = edabe + i;
                                                edacai = edalbe + 1;
                                                edacai = (int)amin0(edacai, Fintab);
                                                if (i < mescosto)
                                                {
                                                    tpx = 1;
                                                    qxt = 0;
                                                    py = 0;
                                                    fpy[i] = 1;
                                                    fqxy[i] = 1;
                                                }
                                                else
                                                {
                                                    qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);

                                                    if (i == 0)
                                                    {
                                                        tpx = 1;
                                                    }
                                                    else
                                                    {
                                                        tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                                        if (i < ltot)
                                                        {
                                                            py = 0;
                                                        }
                                                        else
                                                        {
                                                            py = tpx;
                                                        }
                                                    }

                                                }
                                                fpy[i] = tpx;
                                                fqxy[i] = qxt;
                                                valotemp[imas1] = py * (1 - fpx[imas1]) * facgratif[imas1] * Penben[j] * vl_FactorReajuste[imas1] * fTramos[imas1];
                                                Flupen[imas1] = Flupen[imas1] + valotemp[imas1];
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            msj = "Problemas en los Flujos de hijos Sanos de la Rutina. ";
                                            return ListaResultador;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else //'Calculo de flujos de Sobrevivencia
                {
                    //SOBREVIVENCIA+
                    long limiteA = 0;
                    for (j = 0; j <= Nben; j++)
                    {
                        for (long g = 0; g <= 1332; g++)
                        {
                            fpy[g] = 0;
                            fqxy[g] = 0;
                            valotemp[g] = 0;
                        }

                        if (Ncorbe[j] == 99)
                        {
                            goto Next;
                        }

                        Mesgar = pergar;
                        if (TipMod == "S") { Mesgar = 0; };
                        nmdiga = perdif + Mesgar;

                        Penben[j] = Porcbe[j];
                        numbep = numbep + 1;
                        nibe = 0;

                        if (Coinb[j] == "S" || Coinb[j] == "T" || Coinb[j] == "I") { nibe = 1; };
                        if (Coinb[j] == "N") { nibe = 2; };
                        if (nibe == 0)
                        {
                            msj = "Error en variable nibe==0. ";
                            ListaResultador.Mensaje = msj;
                            return ListaResultador;
                        }
                        nsbe = 0;
                        if (Sexob[j] == "M") { nsbe = 1; };
                        if (Sexob[j] == "F") { nsbe = 2; };
                        if (nsbe == 0)
                        {
                            msj = "Error en variable nsbe==0. ";
                            ListaResultador.Mensaje = msj;
                            return ListaResultador;
                        }
                        Fechan = Nanbe[j] * 12 + Nmnbe[j];
                        edabe = Fechap - Fechan;
                        edaberv = Fechrv - Fechan;
                        edadedv = FechaDv - Fechan;

                        if (edabe > Fintab)
                        {
                            //'Mensaje = "Error en Edad del beneficiario es mayor al limite de la tabla de mortalidad"
                            //Exit Function
                            msj = "Error Edad es mayor a final de tabla Mortal y menor a 0. ";
                            ListaResultador.Mensaje = msj;
                            return ListaResultador;
                        }
                        if (edabe < 1) { edabe = 1; };
                        //'calculo de renta vitalicias
                        if (Ncorbe[j] == 10 || Ncorbe[j] == 11 || Ncorbe[j] == 20 || Ncorbe[j] == 21 || Ncorbe[j] == 41 || Ncorbe[j] == 42 || ((Ncorbe[j] >= 30 && Ncorbe[j] < 40) && (Coinb[j]) != "N"))
                        {
                            limite1 = Fintab - edabe - 1;
                            nmax = (long)amax0(nmax, limite1);
                            tpx = 1;
                            for (i = 0; i <= limite1; i++)
                            {
                                imas1 = i + 1;
                                edalbe = edabe + i;
                                edacai = edalbe + 1;
                                edacai = (int)amin0(edacai, Fintab);

                                if (i == 179)
                                {
                                    an = 1;
                                }

                                if (i < mescosto)
                                {
                                    tpx = 1;
                                    qxt = 0;
                                    py = 1;
                                    fpy[i] = 1;
                                    fqxy[i] = 1;
                                }
                                else
                                {
                                    qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);

                                    if (i == 0)
                                    {
                                        tpx = 1;
                                        py = tpx;
                                    }
                                    else
                                    {
                                        tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                        if (i < ltot)
                                        {
                                            py = 1;
                                        }
                                        else
                                        {
                                            py = tpx;
                                            if (i >= Mesdif && i < nmdiga) { py = 1; };
                                        }
                                    }
                                }

                                fpy[i] = tpx;
                                fqxy[i] = qxt;
                                valPY[imas1] = py;
                                valotemp[imas1] = py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                                Flupen[imas1] = Flupen[imas1] + py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                            }
                        }
                        else
                        {
                            if (Ncorbe[j] >= 30 && Ncorbe[j] < 40)
                            {
                                if ((edaberv > EdaLim && Coinb[j] == "N"))
                                {
                                    Penben[j] = 0;

                                    //ActualizaXMLDET(pathB, j + 1, "PRC_PENSIONSOBDIF", "0")
                                }
                                else
                                {
                                    mdif = EdaLim - edabe;
                                    nmdif = mdif;// - 1;
                                    limiteA = nmdif;
                                    nmax = (long)amax0(nmax, limiteA);
                                    if (TipMod == "G" && TipPen == "S") { limiteA = nmdiga; };
                                    limiteA = (long)amax0(nmdiga, nmdif);

                                    for (i = 0; i <= limiteA - 1; i++)
                                    {
                                        imas1 = i + 1;
                                        edalbe = edabe + i;
                                        edacai = edalbe + 1;
                                        edacai = (int)amin0(edacai, Fintab);

                                        if (i < mescosto)
                                        {
                                            tpx = 1;
                                            qxt = 0;
                                            py = 1;
                                            fpy[i] = 1;
                                            fqxy[i] = 1;
                                        }
                                        else
                                        {
                                            qxt = 1 - (Ly[nsbe, nibe, edacai] / Ly[nsbe, nibe, edalbe]);

                                            if (i == 0)
                                            {
                                                tpx = 1;
                                                py = tpx;
                                            }
                                            else
                                            {
                                                tpx = fpy[i - 1] * (1 - fqxy[i - 1]);

                                                if (i < ltot)
                                                {
                                                    py = 1;
                                                }
                                                else
                                                {
                                                    py = tpx;
                                                    if (swg == "S" && i < nmdiga) { py = 1; }
                                                    if (i < nmdiga) { py = 1; }
                                                    if (i < perdif) { py = 0; }


                                                }
                                            }
                                        }
                                        valPY[i] = py;
                                        fpy[i] = tpx;
                                        fqxy[i] = qxt;
                                        valotemp[imas1] = py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                                        Flupen[imas1] = Flupen[imas1] + py * Penben[j] * facgratif[imas1] * vl_FactorReajuste[imas1];
                                    }
                                }
                            }

                        }

                    Next:
                        Alt = "";
                    }
                }
                //Limpia todas las variables
                for (long g = 0; g <= 1332; g++)
                {
                    FlupenVal[g] = 0;
                    valPY[g] = 0;
                    fpy[g] = 0;
                    fqxy[g] = 0;
                    ftpx[g] = 0;
                    fqxt[g] = 0;
                    //fpx[imas1] = tpx;
                }
                #endregion

                #region Limpia flujos excedentes
                if (TipRen != "E")
                {
                    if (TipPen == "S" || TipPen == "I" || TipPen == "V" || TipPen == "A" || TipPen == "P")
                    {
                        for (i = 1; i <= nmax; i++)
                        {
                            if (i <= (ltot + 1))
                            {
                                if (i <= (mesdif1))
                                {
                                    Flupen[i] = 0;
                                    Flucm[i] = 0;
                                }
                            }
                            else
                            {
                                if (i <= (mesdif1))
                                {
                                    Flupen[i] = 0;
                                    Flucm[i] = 0;
                                }
                            }
                        }
                    }
                    if (TipPen == "S" && mesdif1 > 0)
                    {
                        for (i = 1; i <= nmax; i++)
                        {
                            if (i <= mesdif1)
                            {
                                Flupen[i] = 0;
                                Flucm[i] = 0;
                            }
                        }
                    }
                }
                for (i = 1; i <= nmax; i++)
                {
                    MontoFin = MontoFin + Flupen[i];
                }
                #endregion

                #region Calculos de Inversion
                rmpol = 0;
                nmax = nmax + 1;
                sumapx = 0;
                sumaqx = 0;
                ival = 0;
                penanu = 0;
                cr = 1;
                for (i = 1; i <= nmax; i++)
                {
                    if (i <= 24) { nt = 1; }
                    if (i >= 25 && i <= 48) { nt = 2; }
                    if (i >= 49 && i <= 72) { nt = 3; }
                    if (i >= 73 && i <= 96) { nt = 4; }
                    if (i >= 97 && i <= 120) { nt = 5; }
                    if (i >= 121 && i <= 156) { nt = 6; }
                    if (i >= 157 && i <= 192) { nt = 7; }
                    if (i >= 193 && i <= 252) { nt = 8; }
                    if (i >= 253 && i <= 312) { nt = 9; }
                    if (i > 313) { nt = 10; }

                    if (i <= mescosto + 1)
                    {
                        actual = (0.8 * Cp[nt] / Math.Pow(Math.Pow(tmm, (double)Exp), (0))) + ((1 - 0.8 * Cp[nt]) / Math.Pow(Math.Pow(tm3, (double)Exp), (0)));
                        actua1 = (0.8 * Cp[nt] / Math.Pow(Math.Pow(tmm, (double)Exp), (0))) + ((1 - 0.8 * Cp[nt]) / Math.Pow(Math.Pow(tm3, (double)Exp), (0)));
                    }
                    else
                    {
                        actual = (0.8 * Cp[nt] / Math.Pow(Math.Pow(tmm, (double)Exp), (cr))) + ((1 - 0.8 * Cp[nt]) / Math.Pow(Math.Pow(tm3, (double)Exp), (cr)));
                        actua1 = (0.8 * Cp[nt] / Math.Pow(Math.Pow(tmm, (double)Exp), (cr - 0.5))) + ((1 - 0.8 * Cp[nt]) / Math.Pow(Math.Pow(tm3, (double)Exp), (cr - 0.5)));
                        cr = cr + 1;
                    }
                    sumapx = sumapx + Flupen[i] * actual;
                    sumaqx = sumaqx + Flucm[i] * actua1;

                    factual[i] = actual;
                    factualqx[i] = actua1;
                }

                if (sumapx <= 0)
                {
                    PenBase = 0;
                }
                else
                {
                    PenBase = (MtoCic - sumaqx) / sumapx;
                }
                if (sumapx <= 0)
                {
                    rmpol = 0;
                }
                else
                {
                    rmpol = sumapx * PenBase + sumaqx;
                }
                #endregion

                #region Calcula TCE
                tci = 0;
                tce = 0;
                vpte = 0;
                vpte2 = 0;
                vppenres = 0;
                vpcmres = 0;
                tasatirc = 0;
                difres = 0;
                difre1 = 0;
                tir = 1;
                tinc = 0.01;
                TINC1 = 0.00001;
            CalTce:
                //EMPIEZA LA RUTINA DEL CALCULO DE TCE
                Tasa = (Math.Pow((1 + tir / 100), (double)Exp) - 1);
                i = 1;
                cr = 1;
                for (i = 1; i <= nmax; i++)
                {
                    if (i <= mescosto + 1)
                    {
                        vpte = vpte + (Flupen[i] * PenBase / Math.Pow((1 + Tasa), (0)));
                    }
                    else
                    {
                        vpte = vpte + (Flupen[i] * PenBase / Math.Pow((1 + Tasa), (cr)));
                        cr = cr + 1;
                    }
                    vpte2 = vpte2 + ((Flupen[i] * PenBase * factual[i]));
                }
                //vpte = vpte;
                difres = vpte - vpte2;
                if (difres >= 0)
                {
                    tir = tir + tinc;
                    if (tir > 100)
                    {
                        msj = "Tasa TIR mayor o igual a 100%. ";
                        ListaResultador.Mensaje = msj;
                        return ListaResultador;
                    }
                    difre1 = difres;
                    vpte = 0;
                    vpte2 = 0;
                    goto CalTce;
                }
                tci = tir + tinc * (difres / (difre1 - difres));
                tci = (Math.Pow((1 + (tci / 100)), (double)Exp)) - 1;
                #endregion
                #region Calcula Tasa de Venta
                //'"EMPIEZA LA RUTINA DEL CALCULO DE TASA "
                tirvta = ((Math.Pow((1 + (penmax / 100)), (double)Exp)) - 1) + 0.00001;
            CalTva:
                tasatirc = 0;
                tirvta = tirvta - 0.00001;
                tvmax = tirvta;
                tce = amin0(tci, tvmax);
                salcta_eva = MtoCic;
                vppen = 0;
                vpcm = 0;
                vppenres = 0;
                vpcmres = 0;
                cr = 1;
                dflupag = 0;
                for (int ir = 0; ir <= Fintab; ir++)
                {
                    fcru[ir] = 0;
                }
                for (i = 1; i <= nmax; i++)
                {
                    if (i <= mescosto + 1)
                    {
                        fcru[i] = Flupen[i] * Math.Pow((1 / (1 + tvmax)), (0));
                        vppen = vppen + fcru[i];
                        //vppenres = vppenres + Flupen[i] / Math.Pow((1 + tce), (0));
                    }
                    else
                    {
                        fcru[i] = Flupen[i] * Math.Pow((1 / (1 + tvmax)), (cr));
                        vppen = vppen + fcru[i];
                        vppenres = vppenres + Flupen[i] / Math.Pow((1 + tce), (cr));
                        //vppenres = vppenres + Flupen[i] * Math.Pow((1 / (1 + tce)), (cr));

                        // CRU GS
                        fcruGS[i] = Flucm[i] * Math.Pow((1 / (1 + tvmax)), (cr - 0.5));
                        vpcm = vpcm + fcruGS[i];
                        vpcmres = vpcmres + Flucm[i] / Math.Pow((1 + tce), (cr - 0.5));
                        //vpcmres = vpcmres + Flucm[i] * Math.Pow((1 / (1 + tce)), (cr));

                        cr = cr + 1;
                    }

                }
                penanu = (salcta_eva - vpcm) / vppen;
                mesdiftmp = Mesdif;
                mesdif1 = mesdifc;

                if (TipPen == "S")
                {
                    if (TipRen == "D")
                    {
                        //''SOBREVIVENCIA DIFERIDA
                        if (DerGratificacion == "S")
                        {
                            mesdif1 = mesdif1 + ((mesdif1 / 12) * 2);
                        }
                        mesdiftmp = mesdif1 - mescon;
                        if (mesdiftmp < 0)
                        {
                            mesdiftmp = 0;
                        }
                        ival = ((Math.Pow((1 + PrcAfp), (double)Exp)) - 1);
                        if (mescon > mesdif1)
                        {
                            mesconTmp = mesdif1;
                        }
                        else
                        {
                            mesconTmp = mescon;
                        }
                        vppfactor = ival / ((ival * mesconTmp) + (1 - Math.Pow((1 + ival), -(mesdiftmp))) * (1 + ival));
                        Vpptem = (1 / vppfactor) * sumaporcsob;
                        penanu = (salcta_eva - vpcm) / (vppen + (Vpptem / PrcTaf));
                        Add_porc_be = Totpofr;

                        if (salcta_eva > 0)
                        {
                            if (Mone == "NS")
                            {
                                if (Vpptem == 0 || PrcTaf == 0)
                                {
                                    Rete_sim = 0;
                                }
                                else
                                {
                                    Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
                                }
                            }
                            else
                            {
                                if (Vpptem == 0 || PrcTaf == 0)
                                {
                                    Rete_sim = 0;
                                }
                                else
                                {
                                    Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
                                }
                            }
                        }
                        sumapenben = 0;
                        for (i = 0; i <= Nben; i++)
                        {
                            PensionBenef[i] = Penben[i] * (penanu / PrcTaf);
                            sumapenben = sumapenben + PensionBenef[i];
                        }
                        vld_saldo = Rete_sim;
                        if (vppen > 0)
                        {
                            salcta_eva = (salcta_eva - vld_saldo);
                        }
                        pensim = 0;
                        if (vppen > 0)
                        {
                            penanu = (salcta_eva - vpcm) / vppen;
                        }

                    }
                }
                else
                {
                    if (TipRen == "D")
                    {
                        if (DerGratificacion == "S")
                        {
                            mesdif1 = mesdif1 + ((mesdif1 / 12) * 2);
                        }
                        mesdiftmp = mesdif1 - mescon;
                        if (mesdiftmp < 0) { mesdiftmp = 0; }
                        ival = ((Math.Pow((1 + (PrcAfp)), (double)Exp)) - 1);
                        if (mescon > mesdif1)
                        {
                            mesconTmp = mesdif1;
                        }
                        else
                        {
                            mesconTmp = mescon;
                        }
                        vppfactor = ival / ((ival * mesconTmp) + (1 - Math.Pow((1 + ival), -(mesdiftmp))) * (1 + ival));
                        Vpptem = (1 / vppfactor) * new_prc;
                        penanu = (salcta_eva - vpcm) / (vppen + (Vpptem / PrcTaf));
                        Add_porc_be = Totpofr;

                        if (salcta_eva > 0)
                        {
                            if (Mone == "NS")
                            {
                                if (Vpptem == 0 || PrcTaf == 0)
                                {
                                    Rete_sim = 0;
                                }
                                else
                                {
                                    Rete_sim = (penanu / PrcTaf) * Vpptem;
                                    //Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
                                }
                            }
                            else
                            {
                                if (Vpptem == 0 || PrcTaf == 0)
                                {
                                    Rete_sim = 0;
                                }
                                else
                                {
                                    Rete_sim = (penanu / PrcTaf) * Vpptem;
                                    //Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
                                }
                            }
                        }
                        sumapenben = 0;
                        PensionBenef[1] = (penanu / PrcTaf);
                        sumapenben = sumapenben + PensionBenef[1];

                        vld_saldo = Rete_sim;
                        if (vppen > 0)
                        {
                            salcta_eva = (salcta_eva - vld_saldo);
                        }
                        pensim = 0;
                        if (vppen > 0)
                        {
                            penanu = (salcta_eva - vpcm) / vppen;
                        }
                    }
                }

                for (int ir = 0; ir <= Fintab; ir++)
                {
                    Exced[ir] = 0;
                }
                if (TipPen != "S")
                {
                    sumaporcsob = 1;
                }
                reserva = 0;
                PERDI = 0;
                dflupag = 0;
                flupag = 0;
                for (int a = 0; a <= mescosto + 1; a++)
                {
                    dflupag = dflupag + (penanu * Flupen[a] + Flucm[a]);/// (Math.Pow((1 + tce), a)));
                }
                reserva = penanu * (vppenres) + vpcmres + dflupag;
                PERDI = ((reserva / salcta_eva) - 1) * 100;

                //if (TipRen == "D")
                //{
                //    reserva = salcta_eva;
                //}

                //dComis = (salcta_eva * PrcCom);
                dComis = (salcta_eva * (2.6 / 100) * 1.42);
                dGasMan = (gastos / 12);
                dVarRes = reserva;
                dUtilImp = salcta_eva - (dComis + gasemi + dGasMan + dVarRes);
                dvarCap = dVarRes * dMarSol;
                //PRIMER FLUJO EXDENTES
                Exced[1] = dUtilImp - dImp - dvarCap;

                dvarCapAnt = dvarCap;
                resfinAnt = dVarRes;
                vlContarMaximo = nmax;

                for (i = 2; i <= nmax; i++)
                {

                    relres = 1;
                    resfin = 0;
                    at = 0;
                    iRes = i + mescosto - 1;
                    flupag = penanu * Flupen[iRes] + Flucm[iRes];

                    for (long a = iRes; a <= nmax; a++)
                    {
                        resfin = resfin + ((penanu * Flupen[a + 1] + Flucm[a + 1]) / (Math.Pow((1 + tce), at)));
                        at = at + 1;
                    }
                    resfin = flupag + resfin;
                    dGasMan = (gastos / 12) * (resfin / reserva);
                    dPagos = flupag;

                    if (i == 2)
                    {
                        dPagos = (dPagos + dflupag);
                    }
                    dVarRes = (resfin - resfinAnt);
                    dProdInv = (resfinAnt + dvarCapAnt) * Prodin[i];
                    dUtilImp = -(dGasMan + dPagos + dVarRes) + dProdInv;

                    dCapit = resfin * dMarSol;
                    dvarCap = dCapit - dvarCapAnt;

                    Exced[i] = Math.Round(dUtilImp - dImp - dvarCap, 4);

                    dvarCapAnt = dCapit;
                    resfinAnt = resfin;

                }

            CalExd:
                sumaex = 0;
                for (i = 1; i <= vlContarMaximo; i++)
                {
                    sumaex = sumaex + Exced[i] / Math.Pow((1 + tasatirc), i);
                }
                if (sumaex >= 0)
                {
                    tasatirc = tasatirc + tasanc;
                    goto CalExd;
                }
                tasatirc = (Math.Pow((1 + tasatirc), 12)) - 1;
                if (Math.Round(tasatirc, 2) <= tasac)
                {
                    goto CalTva;
                }
                if (sumaex >= 0)
                {
                    tirvta = tirvta + TINC1;
                    if (tirvta > 100)
                    {
                        break;
                        //sale funcion
                    }
                    sumaex1 = sumaex;
                    sumaex = 0;
                    goto CalTva;
                }
                tirmax = tirvta;
                tirmax_ori = tirmax;

            CalPer:
                if (PERDI > PERMAX)
                {
                    //goto CalTva;
                    TTirMax = tirmax;
                    tirmax = tirmax - TINC1;

                    vppen = 0;
                    vpcm = 0;
                    RM = 0;
                    for (int ir = 0; ir <= Fintab; ir++)
                    {
                        fcru[ir] = 0;
                    }
                    cr = 1;
                    for (i = 1; i <= nmax; i++)
                    {
                        if (i <= mescosto + 1)
                        {
                            fcru[i] = Flupen[i] * Math.Pow((1 / (1 + TTirMax)), (0));
                            vppen = vppen + fcru[i];
                        }
                        else
                        {
                            fcru[i] = Flupen[i] * Math.Pow((1 / (1 + TTirMax)), (cr));
                            vppen = vppen + fcru[i];
                            vpcm = vpcm + Flucm[i] / Math.Pow((1 + TTirMax), (cr - 0.5));
                            cr = cr + 1;
                        }
                    }
                    NewPen = (salcta_eva - vpcm) / vppen;
                    RM = (NewPen * vppenres) + vpcmres;
                    PERDI = ((RM / salcta_eva) - 1) * 100;
                    goto CalPer;
                }

                if (PERDI < 0)
                {
                    PERDI = 0;
                }


                tirmax = ((Math.Pow((1 + tirmax), 12)) - 1) * 100;
                tirmax = Math.Round(tirmax, 2);
                if (tirmax > penmax) { tirmax = penmax; }


                //OBTIENE LOS VALORES FINALES CON LA TASA CALCULADA//
                salcta_eva = MtoCic;
                tirmax = ((Math.Pow((1 + (tirmax / 100)), (double)Exp)) - 1);

                vppen = 0;
                vpcm = 0;
                for (int ir = 0; ir <= Fintab; ir++)
                {
                    fcru[ir] = 0;
                }
                cr = 1;
                for (i = 1; i <= nmax; i++)
                {
                    if (i <= mescosto + 1)
                    {
                        fcru[i] = Flupen[i] * Math.Pow((1 / (1 + tirmax)), (0));
                        vppen = vppen + fcru[i];
                    }
                    else
                    {
                        // CRU PENSION
                        fcru[i] = Flupen[i] * Math.Pow((1 / (1 + tirmax)), (cr));
                        vppen = vppen + fcru[i];

                        // CRU GS
                        fcruGS[i] = Flucm[i] * Math.Pow((1 / (1 + tirmax)), (cr - 0.5));
                        vpcm = vpcm + fcruGS[i];

                        cr = cr + 1;
                    }
                }
                penanu = (salcta_eva - vpcm) / vppen;
                //mesdiftmp = Mesdif;
                mesdif1 = mesdifc;
                sumapenben = 0;
                if (TipPen == "S")
                {
                    if (TipRen == "D")
                    {
                        //''SOBREVIVENCIA DIFERIDA
                        if (DerGratificacion == "S")
                        {
                            mesdif1 = mesdif1 + ((mesdif1 / 12) * 2);
                        }
                        mesdiftmp = mesdif1 - mescon;
                        if (mesdiftmp < 0)
                        {
                            mesdiftmp = 0;
                        }
                        ival = ((Math.Pow((1 + PrcAfp), (double)Exp)) - 1);
                        if (mescon > mesdif1)
                        {
                            mesconTmp = mesdif1;
                        }
                        else
                        {
                            mesconTmp = mescon;
                        }
                        vppfactor = ival / ((ival * mesconTmp) + (1 - Math.Pow((1 + ival), -(mesdiftmp))) * (1 + ival));
                        Vpptem = (1 / vppfactor) * sumaporcsob;
                        penanu = (salcta_eva - vpcm) / (vppen + (Vpptem / PrcTaf));
                        Add_porc_be = Totpofr;

                        if (salcta_eva > 0)
                        {
                            if (Mone == "NS")
                            {
                                if (Vpptem == 0 || PrcTaf == 0)
                                {
                                    Rete_sim = 0;
                                }
                                else
                                {
                                    Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
                                }
                            }
                            else
                            {
                                if (Vpptem == 0 || PrcTaf == 0)
                                {
                                    Rete_sim = 0;
                                }
                                else
                                {
                                    Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
                                }
                            }
                        }
                        sumapenben = 0;
                        for (i = 0; i <= Nben; i++)
                        {
                            PensionBenef[i] = Penben[i] * (penanu / PrcTaf);
                            sumapenben = sumapenben + PensionBenef[i];
                        }
                        vld_saldo = Rete_sim;
                        if (vppen > 0)
                        {
                            salcta_eva = (salcta_eva - vld_saldo);
                        }
                        pensim = 0;
                        if (vppen > 0)
                        {
                            penanu = (salcta_eva - vpcm) / vppen;
                        }

                    }
                }
                else
                {
                    if (TipRen == "D")
                    {
                        if (DerGratificacion == "S")
                        {
                            mesdif1 = mesdif1 + ((mesdif1 / 12) * 2);
                        }
                        mesdiftmp = mesdif1 - mescon;
                        if (mesdiftmp < 0) { mesdiftmp = 0; }
                        ival = ((Math.Pow((1 + (PrcAfp)), (double)Exp)) - 1);
                        if (mescon > mesdif1)
                        {
                            mesconTmp = mesdif1;
                        }
                        else
                        {
                            mesconTmp = mescon;
                        }
                        vppfactor = ival / ((ival * mesconTmp) + (1 - Math.Pow((1 + ival), -(mesdiftmp))) * (1 + ival));
                        Vpptem = (1 / vppfactor) * new_prc;
                        penanu = (salcta_eva - vpcm) / (vppen + (Vpptem / PrcTaf));
                        Add_porc_be = Totpofr;

                        if (salcta_eva > 0)
                        {
                            if (Mone == "NS")
                            {
                                if (Vpptem == 0 || PrcTaf == 0)
                                {
                                    Rete_sim = 0;
                                }
                                else
                                {
                                    Rete_sim = (penanu / PrcTaf) * Vpptem;
                                    //Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
                                }
                            }
                            else
                            {
                                if (Vpptem == 0 || PrcTaf == 0)
                                {
                                    Rete_sim = 0;
                                }
                                else
                                {
                                    Rete_sim = (penanu / PrcTaf) * Vpptem;
                                    //Rete_sim = ((penanu / PrcTaf) / vppfactor) * sumaporcsob;
                                }
                            }
                        }
                        sumapenben = 0;
                        PensionBenef[1] = (penanu / PrcTaf);
                        sumapenben = sumapenben + PensionBenef[1];

                        vld_saldo = Rete_sim;
                        if (vppen > 0)
                        {
                            salcta_eva = (salcta_eva - vld_saldo);
                        }
                        pensim = 0;
                        if (vppen > 0)
                        {
                            penanu = (salcta_eva - vpcm) / vppen;
                        }
                    }
                }

                for (int ir = 0; ir <= Fintab; ir++)
                {
                    Exced[ir] = 0;
                }
                if (TipPen != "S")
                {
                    sumaporcsob = 1;
                }
                dflupag = 0;

                for (int a = 0; a <= mescosto + 1; a++)
                {
                    dflupag = dflupag + (penanu * Flupen[a] + Flucm[a]);// / (Math.Pow((1 + tce), a)));
                }
                reserva = penanu * (vppenres) + vpcmres + dflupag;
                PERDI = ((reserva / salcta_eva) - 1) * 100;

                dUtilImp = 0;
                dCapit = 0;
                dvarCap = 0;
                dComis = 0;
                dGasMan = 0;
                dPagos = 0;
                dVarRes = 0;
                dProdInv = 0;
                dImp = 0;
                dMarSol = (6.75 / 100);
                dvarCapAnt = 0;
                resfinAnt = 0;

                //dComis = (salcta_eva * PrcCom);
                dComis = (salcta_eva * (2.6 / 100) * 1.42);
                dGasMan = (gastos / 12);
                dVarRes = reserva;
                dUtilImp = salcta_eva - (dComis + gasemi + dGasMan + dVarRes);
                dvarCap = dVarRes * dMarSol;

                Exced[1] = dUtilImp - dImp - dvarCap;

                dvarCapAnt = dvarCap;
                resfinAnt = reserva;
                vlContarMaximo = nmax;

                for (i = 2; i <= nmax; i++)
                {

                    relres = 1;
                    resfin = 0;
                    at = 1;
                    iRes = i + mescosto - 1;
                    flupag = penanu * Flupen[iRes] + Flucm[iRes];
                    for (long a = iRes; a <= nmax; a++)
                    {
                        resfin = resfin + ((penanu * Flupen[a + 1] + Flucm[a + 1]) / (Math.Pow((1 + tce), at)));
                        at = at + 1;
                    }
                    resfin = flupag + resfin;
                    if (resfin <= 0) { goto CalSalExd; };
                    //mescon = mescon - 1;
                    //if (mescon < 0) { mescon = 0; };
                    dGasMan = (gastos / 12) * (resfin / reserva);
                    dPagos = flupag;


                    if (i == 2)
                    {
                        dPagos = (dPagos + dflupag);
                    }
                    dVarRes = (resfin - resfinAnt);
                    dProdInv = (resfinAnt + dvarCapAnt) * Prodin[i];
                    dUtilImp = -(dGasMan + dPagos + dVarRes) + dProdInv;

                    dCapit = resfin * dMarSol;
                    dvarCap = dCapit - dvarCapAnt;

                    Exced[i] = Math.Round(dUtilImp - dImp - dvarCap, 4);

                    dvarCapAnt = dCapit;
                    resfinAnt = resfin;

                }

            CalSalExd:
                tasatirc = 0;
            CalExd2:
                sumaex = 0;
                for (i = 1; i <= vlContarMaximo; i++)
                {
                    sumaex = sumaex + Exced[i] / Math.Pow((1 + tasatirc), i);
                }
                if (sumaex >= 0)
                {
                    tasatirc = tasatirc + tasanc;
                    goto CalExd2;
                }

                PERDI = ((reserva / salcta_eva) - 1) * 100;

                if (PERDI < 0)
                {
                    PERDI = 0;
                }
                perdis = PERDI;
                tce = ((Math.Pow((1 + tce), 12)) - 1) * 100;
                tasatirc = (Math.Pow((1 + tasatirc), 12)) - 1;
                tasatirc = tasatirc * 100;
                tirmax = (Math.Pow((1 + tirmax), 12)) - 1;
                tirmax = tirmax * 100;
                tasa_tir = Math.Round((tasatirc), 2);
                tasa_vta = Math.Round(tirmax, 2);
                tasa_tce = Math.Round(tce, 2);
                tprc_per = Math.Round(perdis, 2);

                //FIN////



                #endregion


                #region Suma Pensiones Conjunto Familiar
                if (TipRen == "I")
                {
                    sumapenben = 0;
                }
                int e = 1;
                vlSumaPension = 0;

                for (int vlI = 0; vlI <= Nben; vlI++)
                {
                    if (TipRen != "I")
                    {
                        if (Ncorbe[vlI] != 0)
                        {
                            if (Penben[vlI] != 0)
                            {
                                vlSumaPension = vlSumaPension + (penanu * (Penben[vlI]));
                            }
                        }
                    }
                    else
                    {
                        if (Ncorbe[vlI] != 0)
                        {
                            if (Penben[vlI] != 0)
                            {
                                vlSumaPension = vlSumaPension + (penanu * (Porcbe_tram[vlI]));
                            }
                        }
                    }
                }
                #endregion
                #region Carga Tabla de Resultados
                Resultados = new beResultados();

                //vlSumPension = vlSumaPension; //formatea
                if (TipRen == "M") { MinRC = MinRC / Valmon; }
                if (TipPen != "S") { penanuFinal = penanu; } else { penanuFinal = vlSumPension; }
                //NoCotiza:
                if (TipPen == "C" || TipPen == "M")
                {
                    if (penanuFinal < MinRC)
                    {
                        Resultados.PRC_TASATCE = 0;
                        Resultados.PRC_TASAVTA = 0;
                        Resultados.PRC_TASATIR = 0;
                        Resultados.MTO_RESMAT = 0;
                        Resultados.NUM_CORRELATIVO = NumCor;
                        Resultados.NUM_COTESTUDIO = vlNumCot;
                        Resultados.MTO_PENANUAL = 0;
                        Resultados.MTO_RMGTOSEP = 0;
                        Resultados.MTO_RMPENSION = 0;
                        Resultados.PRC_PERCON = 0;
                        Resultados.MTO_VALREAJUSTETRI = 0;
                        Resultados.MTO_VALREAJUSTEMEN = 0;
                        Resultados.MARCASOB = "N";
                        if (MarcaSob == "S")
                        {
                            Resultados.MARCASOB = "S";
                        }

                        Resultados.MTO_VALPREPENTMP = 0;
                        Resultados.MTO_PENSION = 0;
                        Resultados.MTO_PRIUNIDIF = 0;
                        Resultados.MTO_CTAINDAFP = 0;
                        Resultados.MTO_RENTATMPAFP = 0;
                        Resultados.MTO_PRIUNISIM = 0;
                        Resultados.MTO_PENSIONGAR = 0;
                        Resultados.MTO_RMGTOSEPRV = 0;
                        Resultados.MTO_SUMPENSION = 0;
                        if (Mone == "N" && TipRen == "D")
                        {
                            Resultados.MTO_AJUSTEIPC = vgFactorAjusteIPC;
                        }
                        else
                        {
                            Resultados.MTO_AJUSTEIPC = 1;
                        }
                        Resultados.PRIMA_UNICA = 0;
                    }
                    else
                    {
                        Resultados.PRC_TASATCE = Math.Round(tasa_tce, 2);
                        Resultados.PRC_TASAVTA = Math.Round(tasa_vta, 2);
                        Resultados.PRC_TASATIR = Math.Round(tasa_tir, 2);
                        Resultados.MTO_RESMAT = Math.Round(reserva, 2);
                        Resultados.NUM_CORRELATIVO = NumCor;
                        Resultados.NUM_COTESTUDIO = vlNumCot;
                        Resultados.MTO_PENANUAL = Math.Round(vld_PensionAnual, 2); //falta calcular
                        Resultados.MTO_RMGTOSEP = Math.Round(vld_ReservaSepelio, 2); //falta Reserva Gastos de Sepelio
                        Resultados.MTO_RMPENSION = Math.Round(vld_ReservaPensiones, 2);
                        Resultados.PRC_PERCON = Math.Round(tprc_per, 2);
                        Resultados.MTO_VALREAJUSTETRI = PrcTri;
                        Resultados.MTO_VALREAJUSTEMEN = PrcMen;
                        Resultados.MARCASOB = "N";
                        if (MarcaSob == "S")
                        {
                            Resultados.MARCASOB = "S";
                        }

                        Resultados.MTO_VALPREPENTMP = 0;
                        Resultados.MTO_PENSION = Math.Round(penanu, 2);
                        Resultados.MTO_PRIUNIDIF = Math.Round(salcta_eva, 2);
                        Resultados.MTO_CTAINDAFP = Math.Round(vld_saldo, 2);
                        Resultados.MTO_RENTATMPAFP = Math.Round(sumapenben, 2);
                        Resultados.MTO_PRIUNISIM = 0;
                        Resultados.MTO_PENSIONGAR = Math.Round(vlPenGar, 2);
                        Resultados.MTO_RMGTOSEPRV = 0;
                        Resultados.MTO_SUMPENSION = Math.Round(vlSumPension, 2);
                        if (Mone == "N" && TipRen == "D")
                        {
                            Resultados.MTO_AJUSTEIPC = vgFactorAjusteIPC;
                        }
                        else
                        {
                            Resultados.MTO_AJUSTEIPC = 1;
                        }
                        Resultados.PRIMA_UNICA = rmpol;
                    }
                }
                else
                {
                    if (TipPen == "E")
                    {
                        Resultados.PRC_TASATCE = Math.Round(tasa_tce, 2);
                        Resultados.PRC_TASAVTA = Math.Round(tasa_vta, 2);
                        Resultados.PRC_TASATIR = Math.Round(tasa_tir, 2);
                        Resultados.MTO_RESMAT = Math.Round(reserva, 2);
                        Resultados.NUM_CORRELATIVO = NumCor;
                        Resultados.NUM_COTESTUDIO = vlNumCot;
                        Resultados.MTO_PENANUAL = vld_PensionAnual; //falta calcular
                        Resultados.MTO_RMGTOSEP = vld_ReservaSepelio; //falta Reserva Gastos de Sepelio
                        Resultados.MTO_RMPENSION = vld_ReservaPensiones;
                        Resultados.PRC_PERCON = tprc_per;
                        Resultados.MTO_VALREAJUSTETRI = vl_FactorTrimestral;
                        Resultados.MTO_VALREAJUSTEMEN = vl_FactorMensual;
                        Resultados.MARCASOB = "N";
                        if (MarcaSob == "S")
                        {
                            Resultados.MARCASOB = "S";
                        }

                        Resultados.MTO_VALPREPENTMP = 0;
                        Resultados.MTO_PENSION = penanu;
                        Resultados.MTO_PRIUNIDIF = salcta_eva;
                        Resultados.MTO_CTAINDAFP = vld_saldo;
                        Resultados.MTO_RENTATMPAFP = sumapenben;
                        Resultados.MTO_PRIUNISIM = 0;
                        Resultados.MTO_PENSIONGAR = vlPenGar;
                        Resultados.MTO_RMGTOSEPRV = 0;
                        Resultados.MTO_SUMPENSION = vlSumPension;
                        if (Mone == "N" && TipRen == "D")
                        {
                            Resultados.MTO_AJUSTEIPC = vgFactorAjusteIPC;
                        }
                        else
                        {
                            Resultados.MTO_AJUSTEIPC = 1;
                        }
                        Resultados.PRIMA_UNICA = rmpol;
                    }
                    else
                    {
                        Resultados.PRC_TASATCE = Math.Round(tasa_tce, 2);
                        Resultados.PRC_TASAVTA = Math.Round(tasa_vta, 2);
                        Resultados.PRC_TASATIR = Math.Round(tasa_tir, 2);
                        Resultados.MTO_RESMAT = Math.Round(reserva, 2);
                        Resultados.NUM_CORRELATIVO = NumCor;
                        Resultados.NUM_COTESTUDIO = vlNumCot;
                        Resultados.MTO_PENANUAL = vld_PensionAnual; //falta calcular
                        Resultados.MTO_RMGTOSEP = vld_ReservaSepelio; //falta Reserva Gastos de Sepelio
                        Resultados.MTO_RMPENSION = vld_ReservaPensiones;
                        Resultados.PRC_PERCON = tprc_per;
                        Resultados.MTO_VALREAJUSTETRI = vl_FactorTrimestral;
                        Resultados.MTO_VALREAJUSTEMEN = vl_FactorMensual;
                        Resultados.MARCASOB = "N";
                        if (MarcaSob == "S")
                        {
                            Resultados.MARCASOB = "S";
                        }

                        Resultados.MTO_VALPREPENTMP = 0;
                        Resultados.MTO_PENSION = penanu;
                        Resultados.MTO_PRIUNIDIF = salcta_eva;
                        Resultados.MTO_CTAINDAFP = vld_saldo;
                        Resultados.MTO_RENTATMPAFP = sumapenben;
                        Resultados.MTO_PRIUNISIM = 0;
                        Resultados.MTO_PENSIONGAR = vlPenGar;
                        Resultados.MTO_RMGTOSEPRV = 0;
                        Resultados.MTO_SUMPENSION = vlSumPension;
                        if (Mone == "N" && TipRen == "D")
                        {
                            Resultados.MTO_AJUSTEIPC = vgFactorAjusteIPC;
                        }
                        else
                        {
                            Resultados.MTO_AJUSTEIPC = 1;
                        }
                        Resultados.PRIMA_UNICA = rmpol;
                    }
                }
                ListaResultador = Resultados;
                cuenta = cuenta + 1;
                #endregion


            }

            return ListaResultador;
        }
        #endregion

        #region FuncionesPropias
        public double amax0(double arg1, double arg2)
        {
            double xx = 0;
            if (arg1 >= arg2)
            {
                xx = arg1;
            }
            else
            {
                xx = arg2;
            }
            return xx;
        }

        public double amin0(double arg1, double arg2)
        {
            double xx = 0;
            if (arg1 <= arg2)
            {
                xx = arg1;
            }
            else
            {
                xx = arg2;
            }
            return xx;
        }

        #endregion
    }
}
