using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Points
{
    class Statistic_Point
    {
        Initial_processing.Divided_by_periods_data Periods_Data;

        long[,] row1;
        int reg;

        const int N_nejron_in = 1000;
        const int shift_B1 = 2;


        long[][] periods;
        long periods_full_length;
        long[,] periods_1000;
        int ew;
        long[,] osob_x;
        long[,] osob_y;

        long[,] schet;// список особых точек для расчета (должны отличаться!!!!!)
        long[] schet_sum;// список особых точек

        double[,] row0001;
        double[,] row01;


        int B2;
        int B3;
        int B4;

        long max_B2;
        int coor_B2;
        int coor_B2_shift;

        long max_B4;
        int coor_B4;
        int coor_B4_shift;
                          
        double[] diff_B4;
        double diff_b4max;

        long min_B3;
        int coor_B3;

        int coor_B3_shift;

        public Statistic_Point(Initial_processing.Divided_by_periods_data period, long[,] data, int canal_REG)
        {
            Periods_Data = period;
            row1 = data;
            reg = canal_REG;
        }

        public void Set_External_Data()
        {
            periods = Periods_Data.get_period();//Конвертируем выбранную поток с рег в массив периодов
            periods_full_length = Periods_Data.Find_Length_Period_In_Data();
           
            ew = periods.Length;//счетчик найденных максимумов

            osob_x = new long[14, ew];// список особых точек для вывода на график
            osob_y = new long[14, ew];

            schet = new long[15, ew];// список особых точек для расчета (должны отличаться!!!!!)
            schet_sum = new long[15];// список особых точек

        }

        public void Calculate_Base_Statistic() {


            for (int i = 0; i < ew; i++)
            {
                if (periods[i].Length < 200)
                {
                    schet[2, i] = 0;// минимум В1
                    schet[3, i] = 0;// положение минимума В1 - начала отсчета
                    schet[4, i] = 0;// максимум В2
                    schet[5, i] = 0;// положение максимума В2 - начала отсчета - EKG_max_x[w]
                    schet[6, i] = 0;// минимум В3
                    schet[7, i] = 0;// положение минимума В3 - начала отсчета- EKG_max_x[w]
                    schet[8, i] = 0;// максимум В4
                    schet[9, i] = 0;// положение максимума В4 - начала отсчета - EKG_max_x[w]
                    schet[10, i] = 0;

                }
                else
                {

                    schet[2, i] = periods[i][0 + shift_B1]; // минимум В1
                    schet[3, i] = row1[Periods_Data.Return_Length_X_Zero(i, 0 + shift_B1), 0]; // положение минимума В1 - начала отсчета
                    schet[10, i] = periods[i][0 + shift_B1];

                    B2 = Methods_Statistics.Statistic_Point_B2(periods[i].Length);
                    B3 = Methods_Statistics.Statistic_Point_B3(periods[i].Length);
                    B4 = Methods_Statistics.Statistic_Point_B4(periods[i].Length);

                    schet[4, i] = periods[i][B2] - periods[i][0 + shift_B1]; // максимум В2
                    schet[5, i] = row1[Periods_Data.Return_Length_X_Zero(i, B2), 0]; // положение максимума В2 - начала отсчета - EKG_max_x[w]

                    schet[6, i] = periods[i][B3] - periods[i][0 + shift_B1]; // минимум В3
                    schet[7, i] = row1[Periods_Data.Return_Length_X_Zero(i, B3), 0]; // положение минимума В3 - начала отсчета- EKG_max_x[w]

                    schet[8, i] = periods[i][B4] - periods[i][0 + shift_B1]; // максимум В4
                    schet[9, i] = row1[Periods_Data.Return_Length_X_Zero(i, B4), 0]; // положение максимума В4 - начала отсчета - EKG_max_x[w]

                }

            }

        }


        public void Calculate_Points_B2_B3_B4() {

            for (int i = 0; i < ew; i++)
            {

                if (periods[i].Length > 150)
                {
                    Calculate_point_B2(i);
                    Calculate_point_B4(i);
                    Calculate_point_B3(i);
                
                }
            }

        }

        private void Calculate_point_B2(int number_period) {

            B2 = Methods_Statistics.Statistic_Point_B2(periods[number_period].Length);
            B3 = Methods_Statistics.Statistic_Point_B3(periods[number_period].Length);
            B4 = Methods_Statistics.Statistic_Point_B4(periods[number_period].Length);
            //В2
            max_B2 = periods[number_period][B2];
            coor_B2 = B2;
            coor_B2_shift =0;
            if (B2 >= 35)
            {
                coor_B2_shift = 35;
            }
            else
            {
                coor_B2_shift = B2;
            }
            for (int j = B2 - coor_B2_shift; j < B2 + coor_B2_shift; j++)
            {
                if (max_B2 < periods[number_period][j])
                {
                    max_B2 = periods[number_period][j];
                    coor_B2 = j;
                }
            }

            B2 = coor_B2;
            schet[4, number_period] = periods[number_period][B2] - periods[number_period][0 + shift_B1]; // максимум В2
            schet[5, number_period] = row1[Periods_Data.Return_Length_X_Zero(number_period, B2), 0]; // положение максимума В2 - начала отсчета - EKG_max_x[w]

        }

        private void Calculate_point_B4(int number_period)
        {
            max_B4 = periods[number_period][B4];
            coor_B4 = B4;
            coor_B4_shift = 0;
            if (B4 >= 45)
            {
                coor_B4_shift = 45;
            }
            else
            {
                coor_B4_shift = B4;
            }

            if ((B4 + coor_B4_shift) > periods[number_period].Length)
            {

                coor_B4_shift = periods[number_period].Length - B4 - 2;
            }

            diff_B4 = new double[coor_B4_shift + 1];

            for (int d = 0; d < diff_B4.Length - 1; d++)
            {
                diff_B4[d] = periods[number_period][B4 - coor_B4_shift + 1 + d] - periods[number_period][B4 - coor_B4_shift + d];

            }

            diff_b4max = diff_B4[0];
            int d1 = 0;
            for (int j = B4; j < B4 + coor_B4_shift; j++)
            {
                d1++;
                if (diff_b4max < diff_B4[d1])
                {
                    diff_b4max = diff_B4[d1];
                    max_B4 = periods[number_period][j];
                    coor_B4 = j;
                }
            }

            B4 = coor_B4;
            schet[8, number_period] = periods[number_period][B4] - periods[number_period][0 + shift_B1]; // максимум В2
            schet[9, number_period] = row1[Periods_Data.Return_Length_X_Zero(number_period, B4), 0]; // положение максимума В2 - начала отсчета - EKG_max_x[w]

        }

        private void Calculate_point_B3(int number_period)
        {
            B3 = B4 - 32;
            if (B3 < 0)
            {
                B3 = 0;
            }
            min_B3 = periods[number_period][B3];
            coor_B3 = B3;

            coor_B3_shift=0;
            if (B3 >= 14)
            {
                coor_B3_shift = 14;
            }
            else
            {
                coor_B3_shift = B3;
            }

            for (int j = B3 - coor_B3_shift; j < B3 + coor_B3_shift; j++)
            {

                if (min_B3 > periods[number_period][j])
                {
                    min_B3 = periods[number_period][j];
                    coor_B3 = j;
                }
            }

            B3 = coor_B3;
            schet[6, number_period] = periods[number_period][B3] - periods[number_period][0 + shift_B1]; // максимум В2
            schet[7, number_period] = row1[Periods_Data.Return_Length_X_Zero(number_period, B3), 0]; // положение максимума В2 - начала отсчета - EKG_max_x[w]



        }

        public long[,] Get_Special_Point()
        {
            return schet;
        }


    }
}
