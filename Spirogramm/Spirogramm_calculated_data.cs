using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spirogramm
{
    class Spirogramm_calculated_data
    {
        Spirogramm_Special_point special_point;
        private long[,] spec_point;
        private int length_sp_point;

        private long[] EJEL;
        private long[] EJEL_1;
        private double[] Tiffno_Watchel;
        private long[] POS;
        private long[] MOS1_4;
        private long[] MOS2_4;
        private long[] MOS3_4;
        private long[] MOS4_4;
        private double[] SOSV;
        private long[] PAV;

        public Spirogramm_calculated_data(Spirogramm_Special_point Special_points)
        {
            special_point = Special_points;
            this.spec_point = Special_points.get_spec_point();
            int arre = spec_point.Length;
            length_sp_point = arre / 15;//счетчик найденных максимумов

            EJEL = new long[length_sp_point];
            EJEL_1 = new long[length_sp_point];
            Tiffno_Watchel = new double[length_sp_point];
            POS = new long[length_sp_point];
            MOS1_4 = new long[length_sp_point];
            MOS2_4 = new long[length_sp_point];
            MOS3_4 = new long[length_sp_point];
            MOS4_4 = new long[length_sp_point];
            SOSV = new double[length_sp_point];
            PAV = new long[length_sp_point];
        }

        private void Calculate_EJEL() {
            EJEL = special_point.full_Square;
        }
        private void Calculate_EJEL1() {
            EJEL_1 = special_point.B4_4_Square;
        }

        private void Calculate_Tiffno_Watchel() {
            for (int i = 0; i < length_sp_point; i++)
            {
                Tiffno_Watchel[i] = Convert.ToDouble(EJEL_1[i]) / Convert.ToDouble(EJEL[i]);
            }
        }
        private void Calculate_POS() {
            for (int i = 0; i < length_sp_point; i++)
            {
                //POS[i] = spec_point[4, i] - spec_point[2, i];
                POS[i] = spec_point[4, i];
            }
        }
        private void Calculate_MOS1_4() {
            for (int i = 0; i < length_sp_point; i++)
            {
                MOS1_4[i] = spec_point[6, i];
            }
        }
        private void Calculate_MOS2_4() {
            for (int i = 0; i < length_sp_point; i++)
            {
                MOS2_4[i] = spec_point[8, i];
            }
        }
        private void Calculate_MOS3_4() {
            for (int i = 0; i < length_sp_point; i++)
            {
                MOS3_4[i] = spec_point[10, i];
            }
        }
        private void Calculate_MOS4_4() {
            for (int i = 0; i < length_sp_point; i++)
            {
                MOS4_4[i] = spec_point[12, i];
            }
        }
        private void Calculate_SOSV() {
            for (int i = 0; i < length_sp_point; i++)
            {
                SOSV[i] = Convert.ToDouble(MOS1_4[i]+ MOS2_4[i]+ MOS3_4[i]+ MOS4_4[i]) /4;
            }
        }
        private void Calculate_PAV() {
            for (int i = 0; i < length_sp_point; i++)
            {
                MOS1_4[i] = spec_point[0, i];
            }

        }

        public void Calculate_All() {
            Calculate_EJEL();
            Calculate_EJEL1();
            Calculate_Tiffno_Watchel();
            Calculate_POS();
            Calculate_MOS1_4();
            Calculate_MOS2_4();
            Calculate_MOS3_4();
            Calculate_MOS4_4();
            Calculate_SOSV();
            Calculate_PAV();
        }


        public void Save_Special_Point_Spirogramma()
        {            
            StreamWriter rw2 = new StreamWriter("Параметры спирограммы.txt");
            rw2.WriteLine("ЭЖЕЛ, л" + "\t" + "ЭЖЕЛ1, л" + "\t" + "Тиффно-Вотчала" + "\t" +
                  "ПОС" + "\t" + "МОС1/4, л/с" + "\t" + "МОС2/4, л/с" + "\t" + "МОС3/4, л/с" + "\t" +
                    "МОС4/4, л/с" + "\t" + "СОСВ, л/с" + "\t" + "ПАВ, л/с2");
            rw2.WriteLine();

            for (int i = 0; i < length_sp_point; i++)
            {
                rw2.WriteLine(i + "\t" + EJEL[i] + "\t" + EJEL_1[i] + "\t" +
                    Tiffno_Watchel[i] + "\t" + POS[i] + "\t" + MOS1_4[i] + "\t" + MOS2_4[i]
                    + "\t" + MOS3_4[i] + "\t" + MOS4_4[i] + "\t" + SOSV[i]
                   + "\t" + PAV[i]);
            }
            rw2.Close();

        }

    }
}
