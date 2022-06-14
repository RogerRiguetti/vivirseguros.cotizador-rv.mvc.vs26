using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Process
{
    class RutinaMortalidadDin
    {
        public List<beMortalidadDinVal> TablaMortalidad(List<beMortalidadDinDet> ModelMorDEt,
          int Mortal_M_S, int Mortal_F_S, int Mortal_M_I, int Mortal_F_I)
        {

            beMortalidadDinDet TablaMortalDet = new beMortalidadDinDet();
            List<beMortalidadDinVal> ListadoResult = new List<beMortalidadDinVal>();
            int h, i, j, k;
            decimal vlMto, vlMtoax;

            // MONTOS MUJERES SANOS 

            if (Mortal_F_S != 0)
            {
                h = 1;
                i = 2; //sexo
                j = 2; //invalidez

                try
                {
                    var filNroTabMor = ModelMorDEt.Where(x => x.numCor == Mortal_F_S).ToList();
                    if (filNroTabMor != null)
                    {
                        //vgSw = true;
                        foreach (var Fil in filNroTabMor)
                        {
                            beMortalidadDinVal TablaMortal = new beMortalidadDinVal();
                            k = Fil.edad;
                            vlMto = Fil.mto_lx;
                            vlMtoax = Fil.mto_ax;

                            TablaMortal.Cor = Mortal_F_S;
                            TablaMortal.i = i;
                            TablaMortal.j = j;
                            TablaMortal.k = k;
                            TablaMortal.MtoLx = vlMto;
                            TablaMortal.MtoAx = vlMtoax;
                            ListadoResult.Add(TablaMortal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ListadoResult;
                }
            }


            //MONTOS MUJERES INVALIDOS

            if (Mortal_F_I != 0)
            {
                h = 1;
                i = 2;
                j = 1;

                try
                {
                    var filNroTabMor = ModelMorDEt.Where(x => x.numCor == Mortal_F_I).ToList();
                    if (filNroTabMor != null)
                    {
                        //vgSw = true;
                        foreach (var Fil in filNroTabMor)
                        {
                            beMortalidadDinVal TablaMortal = new beMortalidadDinVal();
                            k = Fil.edad;
                            vlMto = Fil.mto_lx;
                            vlMtoax = Fil.mto_ax;

                            TablaMortal.Cor = Mortal_F_S;
                            TablaMortal.i = i;
                            TablaMortal.j = j;
                            TablaMortal.k = k;
                            TablaMortal.MtoLx = vlMto;
                            TablaMortal.MtoAx = vlMtoax;
                            ListadoResult.Add(TablaMortal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ListadoResult;
                }

            }

            /*******************************************************************************************************************************************************/
            /*******************************************************************************************************************************************************/
            /*******************************************************************************************************************************************************/

            //MONTO HOMBRES SANOS

            if (Mortal_M_S != 0)
            {
                h = 1;
                i = 1; //sexo
                j = 2; //invalidez

                try
                {
                    var filNroTabMor = ModelMorDEt.Where(x => x.numCor == Mortal_M_S).ToList();
                    if (filNroTabMor != null)
                    {
                        //vgSw = true;
                        foreach (var Fil in filNroTabMor)
                        {
                            beMortalidadDinVal TablaMortal = new beMortalidadDinVal();
                            k = Fil.edad;
                            vlMto = Fil.mto_lx;
                            vlMtoax = Fil.mto_ax;

                            TablaMortal.Cor = Mortal_F_S;
                            TablaMortal.i = i;
                            TablaMortal.j = j;
                            TablaMortal.k = k;
                            TablaMortal.MtoLx = vlMto;
                            TablaMortal.MtoAx = vlMtoax;
                            ListadoResult.Add(TablaMortal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ListadoResult;
                }
            }

            //MONTOS HOMBRES INVALIDOS

            if (Mortal_M_I != 0)
            {
                h = 1;
                i = 1;
                j = 1;

                try
                {
                    var filNroTabMor = ModelMorDEt.Where(x => x.numCor == Mortal_M_I).ToList();
                    if (filNroTabMor != null)
                    {
                        //vgSw = true;
                        foreach (var Fil in filNroTabMor)
                        {
                            beMortalidadDinVal TablaMortal = new beMortalidadDinVal();
                            k = Fil.edad;
                            vlMto = Fil.mto_lx;
                            vlMtoax = Fil.mto_ax;

                            TablaMortal.Cor = Mortal_F_S;
                            TablaMortal.i = i;
                            TablaMortal.j = j;
                            TablaMortal.k = k;
                            TablaMortal.MtoLx = vlMto;
                            TablaMortal.MtoAx = vlMtoax;
                            ListadoResult.Add(TablaMortal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ListadoResult;
                }
            }

            return ListadoResult;
        }
    }
}
