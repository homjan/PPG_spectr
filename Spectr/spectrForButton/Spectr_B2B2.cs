using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spectr
{
    class Spectr_B2B2 : Spectr_numeric
    {
        public Spectr_B2B2(long[,] osob, int b1, int nudft) : base(osob, b1, nudft) {

        }

        public override void set_diffrence()//Считаем разницу используемую для построения гистогорамм
        {

            for (int i = 1; i < N_line - 1; i++)
            {
                if ((osob_s[5, i + 1] - osob_s[5, i]) > 0)
                {
                    diffrence[i - 1] = osob_s[5, i + 1] - osob_s[5, i];
                    diffrence_2[i - 1] = osob_s[5, i + 1] - osob_s[5, i];
                }
            }
        }

        public override void calculate_amp_spectr_pow()
        {
            for (int i = 0; i < N_line_new - 1; i++)
            {
                Amp_spectr[i] = Amp_spectr[i] / 1000000;

                //  Amp_spectr_pow[i] = Amp_spectr[i] * Amp_spectr[i] * N_line_new / 2;

                Amp_spectr_pow[i] = Amp_spectr[i] * Amp_spectr[i];
            }

        }
    
    }
}
