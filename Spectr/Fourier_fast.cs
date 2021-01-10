using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spectr
{
    class Fourier_Fast
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

        /// <summary>
        /// Конструктор для быстрого преобразования фурье
        /// </summary>
        /// <param name="diffrence1">Временные промежутки</param>
        /// <param name="diffrence2">Непрерывная шкала</param>
        /// <param name="N_new1"></param>
        public Fourier_Fast(long[] diffrence1, double[] diffrence2, int N_new1) {
            this.diffrence_1 = diffrence1;
            this.diffrence_2 = diffrence2;
            this.N_new = N_new1;

            Amp_spectr = new double[N_new];
             Amp_dt = new double[N_new];

             time_diff_1 = new double[N_new];// Временные промежутки
             time_diff_2 = new double[N_new];//непрерывная шкала     

             Amplituda = new double[N_new];
        }

        public double[] Get_Amplituda_Spectr() {

            return Amplituda;
        }

        public double Get_DW() {
            return DW;
        }
        public double Get_DT()
        {
            return DT;
        }
        /// <summary>
        /// Рассщитать ВСЕ
        /// </summary>
        public void Calculate_All() {
            Set_Amplituda_Spectr();
            Calculate_Dt();
            Calculate_T_big();
            Calculate_DW();
            Calculate_Time_Diffrence();

        }

        /// <summary>
        /// Установить амплитуды спектра
        /// </summary>
        public void Set_Amplituda_Spectr() {

            for (int i = 0; i < N_new - 1; i++)
            {              
                Amp_dt[i] = System.Convert.ToDouble(diffrence_2[i]);
            }
        }

        /// <summary>
        /// Рассщитать минимальный шаг
        /// </summary>
        public void Calculate_Dt() {
            DT = diffrence_1[5];
            for (int i = 5; i < N_new - 1; i++)
            {            
                    if (DT > diffrence_1[i])
                    {
                      DT = diffrence_1[i];
                    }
             
            }           
            DT = DT / 1000000;
        }

        /// <summary>
        /// Рассщитать длительность сигнала
        /// </summary>
        public void Calculate_T_big() {
            for (int i = 0; i < N_new - 1; i++)
            {
                T_BIG = T_BIG + diffrence_1[i];
            }
            T_BIG = T_BIG / 1000000;
        }

        /// <summary>
        /// Рассщитать минимальную частоту
        /// </summary>
        public void Calculate_DW() {
            DW = (PI_ * 2.00) / T_BIG;
        }
        /// <summary>
        /// Рассщитать временные метки
        /// </summary>
        public void Calculate_Time_Diffrence() {
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

        /// <summary>
        /// Расчет спектра
        /// </summary>
        public void Spectr_09()
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
                      
            for (int i = 0; i < N_new - 1; i++)
            {
                POWER.WriteLine((i + 1) + "\t" + (RE_W[i]) + "\t" + IN_W[i]);
            }
            POWER.WriteLine();           

            POWER.Close();

        }


    }
}
