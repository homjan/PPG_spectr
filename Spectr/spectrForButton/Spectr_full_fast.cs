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

        /// <summary>
        /// Рассчитать разницу используемую для построения гистогорамм
        /// </summary>
        public override void Set_Diffrence()//
        {
            for (int i = 1; i < N_line - 1; i++)
            {
               
                    diffrence[i - 1] = osob_s[i, reg1];
                    diffrence_2[i - 1] = osob_s[i, reg1];
                
            }
        }
        /// <summary>
        /// Удалить промежутки нулевой длительности
        /// </summary>
        public override void Delete_Zero_Diffrence()
        {
           
        }

    }
}
