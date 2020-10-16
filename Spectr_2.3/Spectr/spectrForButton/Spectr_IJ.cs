using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spectr
{
    class Spectr_IJ : Spectr_numeric
    {
        public Spectr_IJ(long[,] osob, int b1, int nudft) : base(osob, b1, nudft) {

        }

        public void set_diffrence(String textbox )//Считаем разницу используемую для построения гистогорамм
        {
            double rost = System.Convert.ToDouble(textbox) / 100;

            for (int i = 2; i < N_line - 1; i++)
            {
                if ((osob_s[3, i + 1] - osob_s[3, i]) > 300000)
                {

                    diffrence[i - 2] = osob_s[3, i + 1] - osob_s[3, i];
                    diffrence_2[i - 2] = rost * 1000000 / (System.Convert.ToDouble(osob_s[9, i]) - System.Convert.ToDouble(osob_s[5, i]));
                }
            }
        }

        public override void delete_probel_diffrence()
        {
            int ze = 0;
            for (int i = 0; i < N_line - 1; i++)
            {
                if (diffrence[i] < 300000)
                {
                    for (int r = i; r < N_line - 1; r++)
                    {
                        diffrence_2[r] = diffrence_2[r + 1];
                        diffrence[r] = diffrence[r + 1];
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

    }
}
