using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spectr
{
    class Spectr_full_fast : Spectr_numeric
    {
        private int reg1;

        public Spectr_full_fast(long[,] osob, int b1, int Reg, int nudft) : base(osob, b1, nudft) {
            this.reg1 = Reg;
        }

        public override void set_diffrence()//Считаем разницу используемую для построения гистогорамм
        {

            for (int i = 1; i < N_line - 1; i++)
            {
               
                    diffrence[i - 1] = osob_s[i, reg1];
                    diffrence_2[i - 1] = osob_s[i, reg1];
                
            }
        }

        public override void delete_probel_diffrence()
        {
           
        }

    }
}
