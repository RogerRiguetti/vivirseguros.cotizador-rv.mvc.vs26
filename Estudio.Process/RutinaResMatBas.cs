using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using log4net;
using System.Reflection;
using log4net.Config;

namespace Estudio.Process
{
    public class RutinaResMatBas
    {
        #region Rutina Reserva Matematica
        //Globales
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string msj { get; set; }
        public string querysTot = "";

        public List<DataTable> RutinaActResMat(
            List<beResultadosFlujos> ModelFlu,
            List<beDatosPol> ModelPol,
            List<beDatosBen> Modelben,
            string FecCal, double ValTc, double valGS, string tipTbl, string rutina)
        {
            querysTot = "";
            List<DataTable> lstTablasReservas = new List<DataTable>();
            XmlConfigurator.Configure();

            #region DataTables
            DataTable PR_TMAE_CALPOL1 = new DataTable();
            DataTable PR_TMAE_CALPOL2 = new DataTable();
            DataTable PR_TMAE_CALBEN1 = new DataTable();
            DataTable PR_TMAE_CALBEN2 = new DataTable();

            PR_TMAE_CALPOL1.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESBAS", typeof(decimal));
            PR_TMAE_CALPOL1.Columns.Add("MTO_RESBASRET", typeof(decimal));

            PR_TMAE_CALPOL2.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESBAS", typeof(decimal));
            PR_TMAE_CALPOL2.Columns.Add("MTO_RESBASRET", typeof(decimal));

            PR_TMAE_CALBEN1.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALBEN1.Columns.Add("NUM_ORDEN", typeof(int));
            PR_TMAE_CALBEN1.Columns.Add("NUM_EDAD", typeof(int));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNUBAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNABAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNGBAS", typeof(decimal));
            PR_TMAE_CALBEN1.Columns.Add("MTO_CNTBAS", typeof(decimal));

            PR_TMAE_CALBEN2.Columns.Add("NUM_POLIZA", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("COD_BASE", typeof(string));
            PR_TMAE_CALBEN2.Columns.Add("NUM_ORDEN", typeof(int));
            PR_TMAE_CALBEN2.Columns.Add("NUM_EDAD", typeof(int));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNUBAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNABAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNGBAS", typeof(decimal));
            PR_TMAE_CALBEN2.Columns.Add("MTO_CNTBAS", typeof(decimal));
            #endregion

            #region Variables
            bool Resultado = true;
            List<beResultadosResBas> ListaResultados = new List<beResultadosResBas>();
            List<beResultadosResBasBen> ListaResultadosBen = new List<beResultadosResBasBen>();
            int orden = 0;
            long mes = 0;
            double tce = 0;
            double mtoPen = 0;
            double mtoGS = 0;
            decimal Exp = (decimal)1 / 12;
            double sumaPX = 0, sumaPXben = 0, sumaQx = 0, sumaPXbenLeg = 0, sumaPXbenGar = 0;
            double[] Flupen = new double[1333];
            double[] FluGS = new double[1333];
            double[] SumResBen = new double[20];
            double[] SumResBenLeg = new double[20];
            double[] SumResBenGar = new double[20];
            long[] EdadBen = new long[20];
            string numPol = "";
            int MesDif = 0, MeGar = 0, MesCon = 0;
            int ben = 0;
            int Nben, Nap, Nmp, Ndp, Nad, Nmd, Ndd, NumCor, TipRea;
            long mesFx = 0;
            long mesQx = 0;
            long mesI = 1;
            #endregion

            try
            {
                #region Calculo Reserva Base

                Nap = int.Parse(FecCal.Substring(0, 4));
                Nmp = int.Parse(FecCal.Substring(4, 2));
                Ndp = int.Parse(FecCal.Substring(6, 2));

                foreach (var item in ModelPol)
                {
                    numPol = item.NumPol;
                    MesDif = item.NumDif;
                    MeGar = item.NumGar;
                    Nad = int.Parse(item.FecDev.Substring(0, 4)); //Nad
                    Nmd = int.Parse(item.FecDev.Substring(4, 2)); //Nmd
                    Ndd = int.Parse(item.FecDev.Substring(6, 2)); //Ndd
                    MesCon = ((Nap * 12) + Nmp) - ((Nad * 12) + Nmd);

                }

                foreach (var itemBen in Modelben)
                {
                    orden = itemBen.NumOrd;
                    sumaPXben = 0;
                    sumaPXbenGar = 0;
                    sumaPXbenLeg = 0;
                    //mes = MesCon;
                    foreach (var item in ModelFlu.Where(x => x.numOrd == orden).ToList())
                    {
                        //if (item.GtoSep != 0)
                        //{
                        //    mes = mes + 1;
                        //    //mes = MesCon + 1;
                        //}

                        mes = item.numMes;
                        if (item.fluPen != 0)
                        {
                            //if (mes - 1 == MesCon)
                            //{
                            //    mtoPen = item.fluPen - item.GtoSep;
                            //}
                            //else
                            //{
                            //    mtoPen = item.fluPen; //- item.GtoSep;
                            //}
                            mesFx = (mes - 1) - MesCon;
                            mtoPen = item.fluPen;
                            mtoGS = item.GtoSep;
                            if (mesI == 1)
                            {
                                mtoPen = mtoPen - mtoGS;
                            }
                            //item.tasTce = 5;
                            tce = item.tasTce;//26052021
                            //tce = Math.Round(item.tasTce, 2);
                            tce = ((Math.Pow((1 + (tce / 100)), (double)Exp)) - 1);
                            EdadBen[ben] = item.numEdad;
                            Flupen[item.numMes] = mtoPen * Math.Pow(1 / (1 + tce), mesFx);
                            sumaPX = sumaPX + Flupen[item.numMes];

                            if ((mes - 1) <= MeGar)
                            {
                                sumaPXbenGar = sumaPXbenGar + Flupen[item.numMes];
                            }
                            else
                            {
                                sumaPXbenLeg = sumaPXbenLeg + Flupen[item.numMes];
                            }
                            sumaPXben = sumaPXben + Flupen[item.numMes];
                            if (orden == 1)
                            {
                                if (mes > MesCon)
                                {
                                    mesQx = mesQx + 1;
                                }
                                sumaQx = sumaQx + (mtoGS * Math.Pow(1 / (1 + tce), mesQx + 1));
                            }
                            mesI = mesI + 1;
                        }
                        //Mes = Mes + 1;
                    }
                    SumResBen[ben] = sumaPXben;
                    SumResBenGar[ben] = sumaPXbenGar;
                    SumResBenLeg[ben] = sumaPXbenLeg;
                    ben = ben + 1;
                }
                //sumaQx = valGS * sumaQx;
                #endregion

                #region CargaTablaResultados

                if (tipTbl == "1")
                {
                    #region Tablas CALPOL1 y CALBEN1
                    DataRow rowCALPOL1 = PR_TMAE_CALPOL1.NewRow();
                    if (rutina == "N")
                    {
                        rowCALPOL1["NUM_POLIZA"] = numPol;
                        rowCALPOL1["COD_BASE"] = "S";
                        rowCALPOL1["MTO_RESBAS"] = sumaPX;
                        rowCALPOL1["MTO_RESBASRET"] = sumaPX;

                        PR_TMAE_CALPOL1.Rows.Add(rowCALPOL1);

                        //querysTot += "UPDATE SeguroRV.dbo.PR_TMAE_CALPOL1 SET COD_BASE = 'S', MTO_RESBAS = " + sumaPX + ", MTO_RESBASRET = " + sumaPX + " " +
                        //         "WHERE NUM_POLIZA = " + numPol + "\n";
                    }
                    else
                    {
                        rowCALPOL1["NUM_POLIZA"] = numPol;
                        rowCALPOL1["COD_BASE"] = "S";
                        rowCALPOL1["MTO_RESBAS"] = sumaPX;
                        rowCALPOL1["MTO_RESBASRET"] = sumaPX;

                        PR_TMAE_CALPOL1.Rows.Add(rowCALPOL1);

                        //querysTot += "UPDATE SeguroRV.dbo.PR_TMAE_CALPOL1 SET MTO_RESBAS_METAN = " + sumaPX + ", MTO_RESBASRET_METAN = " + sumaPX + " " +
                        //         "WHERE NUM_POLIZA = " + numPol + "\n";
                    }
                    lstTablasReservas.Add(PR_TMAE_CALPOL1);

                    //SACA LOS FLUJOS POR BENEFICIARIOS
                    for (int j = 0; j <= ben - 1; j++)
                    {
                        DataRow rowCALBEN1 = PR_TMAE_CALBEN1.NewRow();
                        if (rutina == "N")
                        {
                            rowCALBEN1["NUM_POLIZA"] = numPol;
                            rowCALBEN1["COD_BASE"] = "S";
                            rowCALBEN1["NUM_ORDEN"] = (j + 1);
                            rowCALBEN1["NUM_EDAD"] = EdadBen[j];
                            rowCALBEN1["MTO_CNUBAS"] = SumResBen[j];
                            rowCALBEN1["MTO_CNABAS"] = sumaQx;
                            rowCALBEN1["MTO_CNGBAS"] = SumResBenGar[j];
                            rowCALBEN1["MTO_CNTBAS"] = SumResBenLeg[j];

                            PR_TMAE_CALBEN1.Rows.Add(rowCALBEN1);

                            //querysTot += "UPDATE SeguroRV.dbo.PR_TMAE_CALBEN1 SET COD_BASE = 'S', NUM_EDAD = " + EdadBen[j] + ", " +
                            //         "MTO_CNUBAS = " + SumResBen[j] + ", MTO_CNABAS = " + sumaQx + ", MTO_CNGBAS = " + SumResBenGar[j] + ", MTO_CNTBAS = " + SumResBenLeg[j] + " " +
                            //         "WHERE NUM_POLIZA = " + numPol + " AND NUM_ORDEN = " + (j + 1) + "\n";
                        }
                        else
                        {
                            rowCALBEN1["NUM_POLIZA"] = numPol;
                            rowCALBEN1["COD_BASE"] = "S";
                            rowCALBEN1["NUM_ORDEN"] = (j + 1);
                            rowCALBEN1["NUM_EDAD"] = EdadBen[j];
                            rowCALBEN1["MTO_CNUBAS"] = SumResBen[j];
                            rowCALBEN1["MTO_CNABAS"] = sumaQx;
                            rowCALBEN1["MTO_CNGBAS"] = SumResBenGar[j];
                            rowCALBEN1["MTO_CNTBAS"] = SumResBenLeg[j];

                            PR_TMAE_CALBEN1.Rows.Add(rowCALBEN1);

                            //querysTot += "UPDATE SeguroRV.dbo.PR_TMAE_CALBEN1 SET " +
                            //         "MTO_CNUBAS_METAN = " + SumResBen[j] + ", MTO_CNABAS_METAN = " + sumaQx + ", MTO_CNGBAS_METAN = " + SumResBenGar[j] + ", MTO_CNTBAS_METAN = " + SumResBenLeg[j] + " " +
                            //         "WHERE NUM_POLIZA = " + numPol + " AND NUM_ORDEN = " + (j + 1) + "\n";
                        }

                    }
                    lstTablasReservas.Add(PR_TMAE_CALBEN1);
                    #endregion
                }
                else
                {
                    #region Tablas CALPOL2 y CALBEN2
                    DataRow rowCALPOL2 = PR_TMAE_CALPOL2.NewRow();
                    if (rutina == "N")
                    {
                        rowCALPOL2["NUM_POLIZA"] = numPol;
                        rowCALPOL2["COD_BASE"] = "S";
                        rowCALPOL2["MTO_RESBAS"] = sumaPX;
                        rowCALPOL2["MTO_RESBASRET"] = sumaPX;

                        PR_TMAE_CALPOL2.Rows.Add(rowCALPOL2);

                        //querysTot += "UPDATE SeguroRV.dbo.PR_TMAE_CALPOL2 SET COD_BASE = 'S', MTO_RESBAS = " + sumaPX + ", MTO_RESBASRET = " + 0 + " " +
                        //         "WHERE NUM_POLIZA = " + numPol + "\n";
                    }
                    else
                    {
                        rowCALPOL2["NUM_POLIZA"] = numPol;
                        rowCALPOL2["COD_BASE"] = "S";
                        rowCALPOL2["MTO_RESBAS"] = sumaPX;
                        rowCALPOL2["MTO_RESBASRET"] = sumaPX;

                        PR_TMAE_CALPOL2.Rows.Add(rowCALPOL2);

                        //querysTot += "UPDATE SeguroRV.dbo.PR_TMAE_CALPOL2 SET MTO_RESBAS_METAN = " + sumaPX + ", MTO_RESBASRET_METAN = " + 0 + " " +
                        //         "WHERE NUM_POLIZA = " + numPol + "\n";
                    }
                    lstTablasReservas.Add(PR_TMAE_CALPOL2);

                    //SACA LOS FLUJOS POR BENEFICIARIOS
                    for (int j = 0; j <= ben - 1; j++)
                    {
                        DataRow rowCALBEN2 = PR_TMAE_CALBEN2.NewRow();
                        if (rutina == "N")
                        {
                            rowCALBEN2["NUM_POLIZA"] = numPol;
                            rowCALBEN2["COD_BASE"] = "S";
                            rowCALBEN2["NUM_ORDEN"] = (j + 1);
                            rowCALBEN2["NUM_EDAD"] = EdadBen[j];
                            rowCALBEN2["MTO_CNUBAS"] = SumResBen[j];
                            rowCALBEN2["MTO_CNABAS"] = sumaQx;
                            rowCALBEN2["MTO_CNGBAS"] = SumResBenGar[j];
                            rowCALBEN2["MTO_CNTBAS"] = SumResBenLeg[j];

                            PR_TMAE_CALBEN2.Rows.Add(rowCALBEN2);

                            //querysTot += "UPDATE SeguroRV.dbo.PR_TMAE_CALBEN2 SET COD_BASE = 'S', NUM_EDAD = " + EdadBen[j] + ", " +
                            //         "MTO_CNUBAS = " + SumResBen[j] + ", MTO_CNABAS = " + sumaQx + ", MTO_CNGBAS = " + SumResBenGar[j] + ", MTO_CNTBAS = " + SumResBenLeg[j] + " " +
                            //         "WHERE NUM_POLIZA = " + numPol + " AND NUM_ORDEN = " + (j + 1) + "\n";
                        }
                        else
                        {
                            rowCALBEN2["NUM_POLIZA"] = numPol;
                            rowCALBEN2["COD_BASE"] = "S";
                            rowCALBEN2["NUM_ORDEN"] = (j + 1);
                            rowCALBEN2["NUM_EDAD"] = EdadBen[j];
                            rowCALBEN2["MTO_CNUBAS"] = SumResBen[j];
                            rowCALBEN2["MTO_CNABAS"] = sumaQx;
                            rowCALBEN2["MTO_CNGBAS"] = SumResBenGar[j];
                            rowCALBEN2["MTO_CNTBAS"] = SumResBenLeg[j];

                            PR_TMAE_CALBEN2.Rows.Add(rowCALBEN2);

                            //querysTot += "UPDATE SeguroRV.dbo.PR_TMAE_CALBEN2 SET " +
                            //         "MTO_CNUBAS_METAN = " + SumResBen[j] + ", MTO_CNABAS_METAN = " + sumaQx + ", MTO_CNGBAS_METAN = " + SumResBenGar[j] + ", MTO_CNTBAS_METAN = " + SumResBenLeg[j] + " " +
                            //         "WHERE NUM_POLIZA = " + numPol + " AND NUM_ORDEN = " + (j + 1) + "\n";
                        }
                    }
                    lstTablasReservas.Add(PR_TMAE_CALBEN2);
                    #endregion
                }

                #endregion

                return lstTablasReservas;
            }
            catch (Exception ex)
            {
                _log.Info("Error en tarea de Póliza " + numPol + ": " + ex.Message);
                //return msj = "Error en el flujo de Pensiones en Reserva Base. ";
                return lstTablasReservas;
            }

        }
        #endregion

    }
}
