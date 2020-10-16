﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Gistogramma
{
    class Gistogramma_IJ : Gistogramma_numeric
    {
        public Gistogramma_IJ(long[,] osob, int b1) : base(osob, b1) {

        }

        public void set_diffrence( String textboxx)
        {
            double rost = System.Convert.ToDouble(textboxx) / 100;

            for (int i = 1; i < N_line - 1; i++)
            {  
                if ((osob_p[3, i + 1] - osob_p[3, i]) > 300000 && osob_p[9, i] > osob_p[5, i])
                {
                    diffrence_4[i - 1] = rost * 1000000 / (System.Convert.ToDouble(osob_p[9, i]) - System.Convert.ToDouble(osob_p[5, i]));
                }
            }
        }

        public override void delete_probel_diffrence(int limit)
        {
            int ze = 0;
            for (int i = 0; i < N_line - 1; i++)
            {
                if (diffrence_4[i] == 0)
                {
                    for (int r = i; r < N_line - 1; r++)
                    {
                        diffrence_4[r] = diffrence_4[r + 1];
                    }
                    i--;
                    ze++;
                }
                if (i > (N_line - 1 - ze))
                {
                    break;
                }

            }
            //Пересчитываем ew
            N_line_new = N_line - ze + 1;
        }

        public override void convert_diffrence_2_3()
        {          
            for (int i = 0; i < N_line_new; i++)
            {

                diffrence[i] = diffrence_4[i];// / diff_pro_100;
                diffrence_3[i] = diffrence_4[i];
            }
        }

      

        public override void pilliars_gisto(string textbox5)
        {
            int gist = 0;
            GIST_SUM = 0;
            int stolb = 0;// счетчик пустых столбов

            Shag_mod_d = System.Convert.ToDouble(textbox5.Replace('.', ','));

            try
            {
                for (int i = 0; i < N_line_new - 1; i++)
                {
                    while (diffrence[i] > 0)
                    {
                        diffrence[i] = diffrence[i] - Shag_mod_d;
                        gist++;
                    }

                    if (gist > 0)
                    {
                        Gistogramma[gist] = Gistogramma[gist] + 1;
                        GIST_SUM++;
                        gist = 0;
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Уменьшите регулировку");
            }

            for (int kjuh = 0; kjuh < 60; kjuh++)
            {

                if (Gistogramma[kjuh] == 0)
                {
                    stolb++;
                }
                else
                {
                    Gistogramma_number[turr] = kjuh;
                    turr++;
                }

            }
        }







    }
}
