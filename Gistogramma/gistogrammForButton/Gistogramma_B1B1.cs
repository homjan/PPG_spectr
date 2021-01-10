using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Gistogramma
{
    class Gistogramma_B1B1 : Gistogramma_Numeric
    {
        public Gistogramma_B1B1(long[,] osob, int b1) : base(osob, b1) {

        }

        public override void Set_Diffrence()//Считаем разницу используемую для построения гистогорамм
        {

            for (int i = 1; i < N_line - 1; i++)
            {
                if ((osob_p[3, i + 1] - osob_p[3, i]) > 0)
                {
                    diffrence_2[i - 1] = osob_p[3, i + 1] - osob_p[3, i];
                }
            }
        }

    }
}
