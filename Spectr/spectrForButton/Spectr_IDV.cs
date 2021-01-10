using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spectr
{
    class Spectr_IDV : Spectr_numeric
    {
        public Spectr_IDV(long[,] osob, int b1, int nudft) : base(osob, b1, nudft) {

        }

        /// <summary>
        /// Рассчитать разницу используемую для построения гистогорамм
        /// </summary>
        public override void Set_Diffrence()//
        {
            for (int i = 1; i < N_line - 1; i++)
            {
                if ((osob_s[3, i + 1] - osob_s[3, i]) > 300000 && osob_s[4, i] > 1.1 * osob_s[6, i])
                {
                    diffrence[i - 1] = osob_s[3, i + 1] - osob_s[3, i];
                    diffrence_2[i - 1] = System.Convert.ToDouble(osob_s[6, i]) / System.Convert.ToDouble(osob_s[4, i]);
                }
            }
        }

        /// <summary>
        /// Удалить промежутки нулевой длительности
        /// </summary>
        public override void Delete_Zero_Diffrence()
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
