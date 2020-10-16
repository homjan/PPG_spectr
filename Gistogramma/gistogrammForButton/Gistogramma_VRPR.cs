using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Gistogramma
{
    class Gistogramma_VRPR : Gistogramma_numeric
    {
        public Gistogramma_VRPR(long[,] osob, int b1) : base(osob, b1) {

        }

        public override void set_diffrence()
        {
            for (int i = 1; i < N_line - 1; i++)
            {
                if ((osob_p[3, i + 1] - osob_p[3, i]) > 30000)
                {
                    diffrence_2[i - 1] = Math.Abs(osob_p[3, i] - osob_p[1, i]);
                }
            }
        }

        public override void pilliars_gisto(string textbox5)
        {
            int gist = 0;
            GIST_SUM = 1;
            int stolb = 0;// счетчик пустых столбов

            Shag_mod = System.Convert.ToInt64(textbox5);

            try
            {
                for (int i = 0; i < N_line_new - 1; i++)
                {
                    while (diffrence[i] >= 0)
                    {
                        diffrence[i] = diffrence[i] - Shag_mod * 1000;
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
