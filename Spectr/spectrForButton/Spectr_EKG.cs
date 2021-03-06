﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spectr
{
    class Spectr_EKG : Spectr_numeric
    {
        public Spectr_EKG(long[,] osob, int b1, int nudft) : base(osob, b1, nudft) {

        }
        /// <summary>
        /// Рассчитать разницу используемую для построения гистогорамм
        /// </summary>
        public override void Set_Diffrence()
        {

            for (int i = 1; i < N_line - 1; i++)
            {
                if ((osob_s[1, i + 1] - osob_s[1, i]) > 0)
                {
                    diffrence[i - 1] = osob_s[1, i + 1] - osob_s[1, i];                  
                    diffrence_2[i - 1] = osob_s[1, i + 1] - osob_s[1, i];
                }
            }
        }
        /// <summary>
        /// Расчитать мощность спектра
        /// </summary>
        public override void Calculate_Amplitude_Spectr_Pow()
        {
            for (int i = 0; i < N_line_new - 1; i++)
            {
                Amp_spectr[i] = Amp_spectr[i] / 1000000;

                Amp_spectr_pow[i] = Amp_spectr[i] * Amp_spectr[i];
            }

        }

    }
}
