using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Process
{
    class RutinaMortalidadOficiales
    {
        public List<beMortalidad> TablaMortalidad(List<beMortalidadDet> ModelMorDEt,
            int MortalVit_F, int MortalTot_F, int MortalPar_F, int MortalBen_F,
            int MortalVit_M, int MortalTot_M, int MortalPar_M, int MortalBen_M)
        {

            beMortalidadDet TablaMortalDet = new beMortalidadDet();
            List<beMortalidad> ListadoResult = new List<beMortalidad>();
            //ListadoResult = null;

            //string vgIndicadorTipoMovimiento_F, vgDinamicaAñoBase_F;
            int h, i, j, k, intI, vgs_Nro;
            decimal vgMto;
            bool vgSw = false;



            //PENSION VITALICIA MUJER
            if (MortalVit_F != 0)
            {
                h = 1;
                i = 2;
                j = 2;
                vgs_Nro = 0;

                try
                {
                    var filNroTabMor = ModelMorDEt.Where(x => x.numCor == MortalVit_F).ToList();
                    if (filNroTabMor != null)
                    {
                        vgSw = true;
                        foreach (var Fil in filNroTabMor)
                        {
                            beMortalidad TablaMortal = new beMortalidad();
                            k = Fil.edad;
                            vgMto = Fil.mto_lx;
                            TablaMortal.Cor = MortalVit_F;
                            TablaMortal.i = i;
                            TablaMortal.j = j;
                            TablaMortal.h = h;
                            TablaMortal.k = k;
                            TablaMortal.MtoLx = vgMto;
                            ListadoResult.Add(TablaMortal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ListadoResult;
                }
            }
            if (vgSw == false)
            {
                //no se cargo la tabla correctamente
                return ListadoResult;
            }

            //INVALIDEZ TOTAL MUJER
            if (MortalTot_F != 0)
            {
                for (h = 1; h <= 2; h++)
                {
                    vgs_Nro = 0;
                    i = 2;
                    j = 1;
                    vgSw = false;
                    var filNroTabMorT = ModelMorDEt.Where(x => x.numCor == MortalTot_F).ToList();
                    if (filNroTabMorT != null)
                    {
                        vgSw = true;
                        foreach (var Fil in filNroTabMorT)
                        {
                            beMortalidad TablaMortal = new beMortalidad();
                            k = Fil.edad;
                            vgMto = Fil.mto_lx;
                            TablaMortal.Cor = MortalTot_F;
                            TablaMortal.i = i;
                            TablaMortal.j = j;
                            TablaMortal.h = h;
                            TablaMortal.k = k;
                            TablaMortal.MtoLx = vgMto;
                            ListadoResult.Add(TablaMortal);
                        }
                    }
                }
            }
            if (vgSw == false)
            {
                //no se cargo la tabla correctamente
                return ListadoResult;
            }

            //INVALIDEZ PARCIAL MUJER
            if (MortalPar_F != 0)
            {
                for (h = 1; h <= 2; h++)
                {
                    vgs_Nro = 0;
                    i = 2;
                    j = 3;
                    vgSw = false;
                    var filNroTabMorP = ModelMorDEt.Where(x => x.numCor == MortalPar_F).ToList();
                    if (filNroTabMorP != null)
                    {
                        vgSw = true;
                        foreach (var Fil in filNroTabMorP)
                        {
                            beMortalidad TablaMortal = new beMortalidad();
                            k = Fil.edad;
                            vgMto = Fil.mto_lx;
                            TablaMortal.Cor = MortalPar_F;
                            TablaMortal.i = i;
                            TablaMortal.j = j;
                            TablaMortal.h = h;
                            TablaMortal.k = k;
                            TablaMortal.MtoLx = vgMto;
                            ListadoResult.Add(TablaMortal);
                        }
                    }
                }
            }
            if (vgSw == false)
            {
                //no se cargo la tabla correctamente
                return ListadoResult;
            }

            //BENEFICIARIOS MUJER
            if (MortalBen_F != 0)
            {
                vgs_Nro = 0;
                h = 2;
                i = 2;
                j = 2;
                vgSw = false;

                var filNroTabMorB = ModelMorDEt.Where(x => x.numCor == MortalBen_F).ToList();
                if (filNroTabMorB != null)
                {
                    vgSw = true;
                    foreach (var Fil in filNroTabMorB)
                    {
                        beMortalidad TablaMortal = new beMortalidad();
                        k = Fil.edad;
                        vgMto = Fil.mto_lx;
                        TablaMortal.Cor = MortalBen_F;
                        TablaMortal.i = i;
                        TablaMortal.j = j;
                        TablaMortal.h = h;
                        TablaMortal.k = k;
                        TablaMortal.MtoLx = vgMto;
                        ListadoResult.Add(TablaMortal);
                    }
                }
            }
            if (vgSw == false)
            {
                //no se cargo la tabla correctamente
                return ListadoResult;
            }


            /*******************************************************************************************************************************************************/
            /*******************************************************************************************************************************************************/
            /*******************************************************************************************************************************************************/

            //PENSION VITALICIA HOMBRE
            if (MortalVit_M != 0)
            {
                h = 1;
                i = 1;
                j = 2;
                vgs_Nro = 0;

                var filNroTabMorH = ModelMorDEt.Where(x => x.numCor == MortalVit_M).ToList();
                if (filNroTabMorH != null)
                {
                    vgSw = true;
                    foreach (var Fil in filNroTabMorH)
                    {
                        beMortalidad TablaMortal = new beMortalidad();
                        k = Fil.edad;
                        vgMto = Fil.mto_lx;
                        TablaMortal.Cor = MortalVit_M;
                        TablaMortal.i = i;
                        TablaMortal.j = j;
                        TablaMortal.h = h;
                        TablaMortal.k = k;
                        TablaMortal.MtoLx = vgMto;
                        ListadoResult.Add(TablaMortal);
                    }
                }
            }
            if (vgSw == false)
            {
                //no se cargo la tabla correctamente
                return ListadoResult;
            }

            //INVALIDEZ TOTAL HOMBRE
            if (MortalTot_M != 0)
            {
                for (h = 1; h <= 2; h++)
                {
                    vgs_Nro = 0;
                    i = 1;
                    j = 1;
                    vgSw = false;
                    var filNroTabMorTH = ModelMorDEt.Where(x => x.numCor == MortalTot_M).ToList();
                    if (filNroTabMorTH != null)
                    {
                        vgSw = true;
                        foreach (var Fil in filNroTabMorTH)
                        {
                            beMortalidad TablaMortal = new beMortalidad();
                            k = Fil.edad;
                            vgMto = Fil.mto_lx;
                            TablaMortal.Cor = MortalTot_M;
                            TablaMortal.i = i;
                            TablaMortal.j = j;
                            TablaMortal.h = h;
                            TablaMortal.k = k;
                            TablaMortal.MtoLx = vgMto;
                            ListadoResult.Add(TablaMortal);
                        }
                    }
                }
            }
            if (vgSw == false)
            {
                //no se cargo la tabla correctamente
                return ListadoResult;
            }

            //INVALIDEZ PARCIAL HOMBRE
            if (MortalPar_M != 0)
            {
                for (h = 1; h <= 2; h++)
                {
                    vgs_Nro = 0;
                    i = 1;
                    j = 3;
                    vgSw = false;
                    var filNroTabMorPH = ModelMorDEt.Where(x => x.numCor == MortalPar_M).ToList();
                    if (filNroTabMorPH != null)
                    {
                        vgSw = true;
                        foreach (var Fil in filNroTabMorPH)
                        {
                            beMortalidad TablaMortal = new beMortalidad();
                            k = Fil.edad;
                            vgMto = Fil.mto_lx;
                            TablaMortal.Cor = MortalPar_M;
                            TablaMortal.i = i;
                            TablaMortal.j = j;
                            TablaMortal.h = h;
                            TablaMortal.k = k;
                            TablaMortal.MtoLx = vgMto;
                            ListadoResult.Add(TablaMortal);
                        }
                    }
                }
            }
            if (vgSw == false)
            {
                //no se cargo la tabla correctamente
                return ListadoResult;
            }

            //BENEFICIARIOS HOMBRE
            if (MortalBen_M != 0)
            {
                vgs_Nro = 0;
                h = 2;
                i = 1;
                j = 2;
                vgSw = false;

                var filNroTabMorBH = ModelMorDEt.Where(x => x.numCor == MortalBen_M).ToList();
                if (filNroTabMorBH != null)
                {
                    vgSw = true;
                    foreach (var Fil in filNroTabMorBH)
                    {
                        beMortalidad TablaMortal = new beMortalidad();
                        k = Fil.edad;
                        vgMto = Fil.mto_lx;
                        TablaMortal.Cor = MortalBen_M;
                        TablaMortal.i = i;
                        TablaMortal.j = j;
                        TablaMortal.h = h;
                        TablaMortal.k = k;
                        TablaMortal.MtoLx = vgMto;
                        ListadoResult.Add(TablaMortal);
                    }
                }
            }
            if (vgSw == false)
            {
                //no se cargo la tabla correctamente
                return ListadoResult;
            }

            return ListadoResult;
        }
    }
}
