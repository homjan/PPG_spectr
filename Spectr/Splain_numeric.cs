using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spectr
{
    class Splain_numeric
    {
        private long time_mas = 100000;

        private long time_diskret = 1000;

        private int Number_button;


        private long[] Diffrence;
        private long[] Diffrence_sum;

        private double[] Diffrence_2;
        private double[] Diffrence_2_diff;

        private int N_line_1;

        private long[] Diffrence_big;
        private double[] Diffrence_2_big;
        private int N_line_1_big;

        private long[] Diffrence_final;
        private double[] Diffrence_2_final;
        private int N_line_1_final;

        public Splain_numeric(long[] difff, double[] difff2, int N_line, int Numberbutton) {

            this.Diffrence = difff;
            this.Diffrence_2 = difff2;
            this.N_line_1 = N_line;
            Diffrence_sum = new long[N_line_1];
            Diffrence_2_diff = new double[N_line_1];

            this.Number_button = Numberbutton;

            for (int i = 0; i < N_line_1 - 1; i++)
            {

                for (int l = 0; l < i; l++)
                {
                    Diffrence_sum[i] = Diffrence_sum[i] + Diffrence[l];
                }
            }
        }
        /// <summary>
        /// Рассчитать производные
        /// </summary>
        private void Calculate_Diff() {
            Diffrence_2_diff[0] = (4 * Diffrence_2[1] - Diffrence_2[2] -3*Diffrence_2[0])/ (Convert.ToDouble(2 * Diffrence[1]));
            Diffrence_2_diff[N_line_1 - 1] = (3 * Diffrence_2[N_line_1 - 1] - Diffrence_2[N_line_1 - 3] - 3 * Diffrence_2[N_line_1 - 2]) / (Convert.ToDouble(2 * Diffrence[1]));

            for (int i = 1; i < N_line_1-1; i++)
            {
                Diffrence_2_diff[i] = (Diffrence_2[i + 1] - Diffrence_2[i-1]) / (Convert.ToDouble(2*Diffrence[i]));
            }

        }

        public long[] Get_Diffrence_Final() {
            return Diffrence_final;
        }

        public double[] Get_Diffrence_2_Final()
        {
            return Diffrence_2_final;
        }

        public int Get_N_Line_1_Final()
        {
            return N_line_1_final;
        }
        /// <summary>
        /// Рассчитать все
        /// </summary>
        public void Calculate_All() {
            Calculate_Diff();
            Calculate_Time_Full();
            Expand_Data();
            Calculate_Final_Data();
        }
        /// <summary>
        /// Рассчичитать время
        /// </summary>
        public void Calculate_Time_Full() {
            long full_time = 0;

            for (int i = 0; i < N_line_1; i++)
            {
                full_time = full_time + Diffrence[i];
            }

            N_line_1_big = Convert.ToInt32(Convert.ToDouble(full_time) / time_diskret);

            N_line_1_final = Convert.ToInt32( Convert.ToDouble( full_time) / Convert.ToDouble(time_mas));


        }
        /// <summary>
        /// Заполнить данными равномерно
        /// </summary>
        public void Expand_Data() {

            Diffrence_big = new long [N_line_1_big];
            Diffrence_2_big = new double[N_line_1_big];
            long time = 0;
            for (int i = 0; i < N_line_1_big; i++) {
                Diffrence_big[i] = time;
                time = time + time_diskret;
                
            }

            Diffrence_2_big[0] = Diffrence_2[0];
            Diffrence_2_big[N_line_1_big - 1] = Diffrence_2[N_line_1-1];

            long[] diff_N = new long[N_line_1]; 

            int s = 1;
         
            for (long i = 1; i < N_line_1_big-1; i++)
            {
                if (Diffrence_sum[s] > Diffrence_big[i] && Diffrence_sum[s] <= Diffrence_big[i + 1])
                {
                    Diffrence_2_big[i] = Diffrence_2[s];
                    s++;
                    
                }
                else {

                    if (Number_button == 1)
                    {
                        Diffrence_2_big[i] = Diffrence_2[s - 1] + Shift_X(Diffrence_sum[s - 1], Diffrence_sum[s], Diffrence_big[i], Diffrence_2[s - 1], Diffrence_2[s]);
                    }

                    if (Number_button == 2)
                    {
                        Diffrence_2_big[i] = Shift_Splain(Diffrence_sum[s - 1], Diffrence_sum[s], Diffrence_big[i], Diffrence_2[s - 1], Diffrence_2[s], Diffrence_2_diff[s - 1], Diffrence_2_diff[s]); 
                    }
                }

            }
            //////////////
            for (int i = 0; i < N_line_1_big; i++)
            {
                if (Diffrence_2_big[i] < 0)
                {
                    Diffrence_2_big[i] = Math.Abs(Diffrence_2_big[i]);
                }
            }
        }
        /// <summary>
        /// Рассчитать финальные данные
        /// </summary>
        public void Calculate_Final_Data() {

            Diffrence_final = new long[N_line_1_final+1];
            Diffrence_2_final = new double[N_line_1_final+1];

            long time = 0;
            for (int i = 0; i < N_line_1_final; i++)
            {
                Diffrence_final[i] = time;
                time = time + time_mas;

            }

            Diffrence_2_final[0] = Diffrence_2[0];
            Diffrence_2_final[N_line_1_final - 1] = Diffrence_2[N_line_1 - 1];


            int s = 1;
            for (int i = 0; i < N_line_1_big - 1; i++)
            {
                if (Diffrence_final[s] > Diffrence_big[i] && Diffrence_final[s] <= Diffrence_big[i + 1])
                {
                   Diffrence_2_final[s] = Diffrence_2_big[i];
                    s++;
                }               

            }

            for (int i = 0; i < N_line_1_final; i++)
            {
                Diffrence_final[i] = time_mas;  
            }

            for (int i = 0; i < N_line_1_final; i++)
            {
                if (Diffrence_final[i] < 0)
                {
                    Diffrence_final[i] = Math.Abs(Diffrence_final[i]);
                }
            }

            StreamWriter POWER = new StreamWriter("Проверка 2.txt");


            for (int i = 0; i < N_line_1_final - 1; i++)
            {
                POWER.WriteLine((i + 1) + "\t" + (Diffrence_final[i]) + "\t" + Diffrence_2_final[i]);
            }
            POWER.WriteLine();
            POWER.WriteLine();
  
            POWER.Close();
        }

        /// <summary>
        /// Расчет сплайна 1 порядка
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="x3_b"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public double Shift_X(long x1, long x2, long x3_b, double y1, double y2)
        {
            double shift_y = 0;

           if (x2 == x1)
            {
                shift_y = 0;
            }
            else
            {
                shift_y = (System.Convert.ToDouble(x3_b - x1) / System.Convert.ToDouble(x2 - x1)) * (y2 - y1);
            }
           
            return shift_y;
        }

        /// <summary>
        /// Рассчет сплайна 3 порядка
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="x3_b"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <param name="y1_diff"></param>
        /// <param name="y2_diff"></param>
        /// <returns></returns>
        public double Shift_Splain(long x1, long x2, long x3_b, double y1, double y2, double y1_diff, double y2_diff) {

            long h = x2 - x1;

            double a = Convert.ToDouble((x2-x3_b)* (x2 - x3_b)*(2*(x3_b-x1)+h))/(Convert.ToDouble(h*h*h));
            double b = Convert.ToDouble((x3_b-x1) * (x3_b-x1) * (2 * (x2 - x3_b) + h)) / (Convert.ToDouble(h * h * h)); 
            double c = Convert.ToDouble((x2 - x3_b) * (x2 - x3_b) * (x3_b-x1)) / (Convert.ToDouble(h * h)); 
            double d = Convert.ToDouble((x3_b-x1) * (x3_b-x1) * (x3_b - x2)) / (Convert.ToDouble(h * h)); 

            double shift_y = a * y1 + b * y2 + c * y1_diff + d * y2_diff;


            return shift_y;
        }


    }
}
