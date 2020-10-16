using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spectr
{
    class Fourier_fast
    {
        const double PI_ = 3.1415926;

        long[] diffrence_1;
        double[] diffrence_2;
        int N_new;

        double[] Amp_spectr;
        double[] Amp_dt;

        double[] time_diff_1;// Временные промежутки
        double[] time_diff_2;//непрерывная шкала

        double[] Amplituda;

       

        double DT;

        double T_BIG = 0;//Длительность отсчета
        double DW = 0;

        public Fourier_fast(long[] diffrence1, double[] diffrence2, int N_new1) {
            this.diffrence_1 = diffrence1;
            this.diffrence_2 = diffrence2;
            this.N_new = N_new1;

            Amp_spectr = new double[N_new];
             Amp_dt = new double[N_new];

             time_diff_1 = new double[N_new];// Временные промежутки
             time_diff_2 = new double[N_new];//непрерывная шкала     

             Amplituda = new double[N_new];
        }

        public double[] get_amplituda_spectr() {

            return Amplituda;
        }

        public double get_DW() {
            return DW;
        }
        public double get_DT()
        {
            return DT;
        }

        public void calculate_all() {
            set_Amp_spectr();
            calculate_Dt();
            calculate_T_big();
            calculate_DW();
            calculate_time_diff();

        }

        public void set_Amp_spectr() {

            for (int i = 0; i < N_new - 1; i++)
            {              
                Amp_dt[i] = System.Convert.ToDouble(diffrence_2[i]);
            }
        }

        public void calculate_Dt() {
            DT = diffrence_1[5];
            for (int i = 5; i < N_new - 1; i++)
            {
             //   if (diffrence_1[i] > 300000)
              //  {
                    if (DT > diffrence_1[i])
                    {
                      DT = diffrence_1[i];
                    }
              //  }
            }
           // DT = diffrence_1[5];
            DT = DT / 1000000;
        }

        public void calculate_T_big() {
            for (int i = 0; i < N_new - 1; i++)
            {
                T_BIG = T_BIG + diffrence_1[i];
            }
            T_BIG = T_BIG / 1000000;
        }

        public void calculate_DW() {
            DW = (PI_ * 2.00) / T_BIG;
        }

        public void calculate_time_diff() {
            for (int i = 0; i < N_new - 1; i++)
            {
                time_diff_1[i] = System.Convert.ToDouble(diffrence_1[i]) / 1000000;

            }
            for (int i = 0; i < N_new - 1; i++)
            {

                for (int l = 0; l < i; l++)
                {
                    time_diff_2[i] = time_diff_2[i] + time_diff_1[l];
                }
            }
        }

        public void Spectr_09()//Расчет спектра
        {
            double Dt2 = DT;// промежуток времени
            double Dw2 = DW;// промежуток пространства

           
            double[] RE_W = new double[N_new];
            double[] IN_W = new double[N_new];
            double scet_k = 1;
            //Расчет спектра
            for (int i = 0; i < N_new - 1; i++)
            {

                for (int j = 0; j < N_new - 1; j++)
                {
                    RE_W[i] = RE_W[i] + Amp_dt[j]* DT * Math.Cos(time_diff_2[j] * Dw2 * scet_k);
                    IN_W[i] = IN_W[i] + Amp_dt[j]*DT * Math.Sin(time_diff_2[j] * Dw2 * scet_k);
                }

                scet_k = scet_k + 1;
            }
            
            for (int i = 0; i < N_new - 1; i++)
            {
                Amplituda[i] = Math.Sqrt(RE_W[i] * RE_W[i] + IN_W[i] * IN_W[i]);
            }

            StreamWriter POWER = new StreamWriter("Проверка 3.txt");

            /*  for (int i = 0; i < N_line_1_big - 1; i++)
              {
                  POWER.WriteLine((i + 1) + "\t" + (Diffrence_big[i]) + "\t" + Diffrence_2_big[i]);
              }*/

            for (int i = 0; i < N_new - 1; i++)
            {
                POWER.WriteLine((i + 1) + "\t" + (RE_W[i]) + "\t" + IN_W[i]);
            }
            POWER.WriteLine();
           

            POWER.Close();

        }


    }
}
