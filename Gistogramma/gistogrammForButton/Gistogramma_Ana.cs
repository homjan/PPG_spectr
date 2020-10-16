using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Gistogramma
{
    class Gistogramma_Ana : Gistogramma_numeric
    {
        public Gistogramma_Ana (long[,] osob, int b1) : base(osob, b1) {

        }

        public override void set_diffrence()
        {
            for (int i = 1; i < N_line - 1; i++)
            {
                if ((osob_p[3, i + 1] - osob_p[3, i]) > 200000 && osob_p[4, i] > 0)
                {
                    diffrence_2[i - 1] = osob_p[4, i];
                }
            }
        }

        public override void convert_diffrence_2_3()
        {
            double diff_pro_100 = diffrence_2[1];

            for (int i = 0; i < N_line_new; i++)
            {
                diffrence[i] = System.Convert.ToDouble(diffrence_2[i]) / diff_pro_100;
                diffrence_3[i] = System.Convert.ToDouble(diffrence_2[i]) / diff_pro_100;
            }

        }

        public void shift_diffrence() {
            for (int i = 0; i < N_line_new; i++)
            {
                //    diffrence[i] = System.Convert.ToDouble(diffrence_2[i]) / diff_pro_100;
                diffrence_3[i] = diffrence[i];

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
                        diffrence[i] = diffrence[i] - Shag_mod_d/100;
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
