using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Gistogramma
{
    class Gistogramma_EKG : Gistogramma_numeric
    {
        public Gistogramma_EKG(long[,] osob, int b1) : base(osob, b1) {

        }

        public override void set_diffrence()//Считаем разницу используемую для построения гистогорамм
        {

            for (int i = 1; i < N_line - 1; i++)
            {
                if ((osob_p[1, i + 1] - osob_p[1, i]) > 0)
                {
                    diffrence_2[i - 1] = osob_p[1, i + 1] - osob_p[1, i];
                }
            }
        }

    }
}
