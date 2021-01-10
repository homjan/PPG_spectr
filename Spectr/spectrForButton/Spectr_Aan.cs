using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spectr
{
    class Spectr_Aan : Spectr_numeric
    {
        public Spectr_Aan(long[,] osob, int b1, int nudft) : base(osob, b1, nudft) {

        }
        /// <summary>
        /// Рассчитать разницу используемую для построения гистогорамм
        /// </summary>
        public override void Set_Diffrence()//
        {
            for (int i = 1; i < N_line - 1; i++)
            {
                if ((osob_s[3, i + 1] - osob_s[3, i]) > 300000 && osob_s[4, i] > 0)
                {
                    diffrence[i - 1] = osob_s[3, i + 1] - osob_s[3, i];
                    diffrence_2[i - 1] = osob_s[4, i];
                }
            }
        }

    }
}
