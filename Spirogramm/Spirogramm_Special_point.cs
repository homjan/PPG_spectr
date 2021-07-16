using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spirogramm
{
    class Spirogramm_Special_point
    {
        Divided_by_periods_data_spiro Period_job;
        Initial_data initial_data;

        const int shift_B1 = 2;
        private int mass = 10;
        private long[,] spec_point;

        public int[] max_Amplitude_position;
        public int[] end_Amplitude_position;
        public int[] max_Derivative_Amplitude;
        public long[] full_Square;
        public int[] B1_4_Square_position;
        public int[] B2_4_Square_position;
        public int[] B3_4_Square_position;
        public int[] B4_4_Square_position;

        public long[] B1_4_Square;
        public long[] B2_4_Square;
        public long[] B3_4_Square;
        public long[] B4_4_Square;

        private void set_spec_point(long[,] value)
        {
            spec_point = value;
        }

        

        public long[,] get_spec_point()
        {
            return spec_point;
        }

        public Spirogramm_Special_point(Divided_by_periods_data_spiro per, Initial_data init)
        {
            this.Period_job = per;
            this.initial_data = init;

        }

        /// <summary>
        /// Удалить нули из данных об особых точках
        /// </summary>
        public void Delete_Zero_From_Data()
        {
            int arre = spec_point.Length;
            int ew = arre / 15;

            for (int i = 1; i < ew; i++)
            {

                if (spec_point[2, i] == 0 && spec_point[3, i] == 0 && spec_point[4, i] == 0 && spec_point[5, i] == 0
                   )
                {
                    for (int j = i; j < ew - 1; j++)
                    {
                        for (int k = 0; k < 15; k++)
                        {
                            spec_point[k, j] = spec_point[k, j + 1];
                        }
                    }
                }
            }
            int s = 1;

            for (int i = 1; i < ew; i++)
            {
                s++;
                if (spec_point[2, i] == 0 && spec_point[3, i] == 0 && spec_point[4, i] == 0 && spec_point[5, i] == 0
                    )
                {
                    break;
                }
            }

            long[,] period_new = new long[15, s];

            for (int i = 0; i < s; i++)
            {
                for (int k = 0; k < 15; k++)
                {
                    period_new[k, i] = spec_point[k, i];
                }
            }

            set_spec_point(period_new);

        }


        public void Calculate_Time_Special_Point()
        {
            long[][] periods = Period_job.get_period();

            long periods_full_length = Period_job.Find_Length_Period_In_Data();

            int ew = periods.Length;//счетчик найденных максимумов

            long[,] osob_x = new long[14, ew];// список особых точек для вывода на график
            long[,] osob_y = new long[14, ew];

            long[,] schet = new long[15, ew];// список особых точек для расчета (должны отличаться!!!!!)
            long[] schet_sum = new long[15];// список особых точек

            long[,] row1 = initial_data.get_row1();
            long[] row3 = initial_data.get_row3();
            int reg = initial_data.REG;


            max_Amplitude_position = Spirogramm_Additional_Function.Calculate_Max_Amplitude_Position(periods);
            end_Amplitude_position = Spirogramm_Additional_Function.Calculate_End_Amplitude_Position(periods, max_Amplitude_position);

            max_Derivative_Amplitude = Spirogramm_Additional_Function.Calculate_Max_Derivative_Position(periods);

            full_Square = Spirogramm_Additional_Function.Calculate_Square(periods, end_Amplitude_position);
            B1_4_Square_position = Spirogramm_Additional_Function.Calculate_Chosen_Part_Time_Amplitude(periods, 250);
            B2_4_Square_position = Spirogramm_Additional_Function.Calculate_Chosen_Part_Time_Amplitude(periods, 500);
            B3_4_Square_position = Spirogramm_Additional_Function.Calculate_Chosen_Part_Time_Amplitude(periods, 750);
            B4_4_Square_position = Spirogramm_Additional_Function.Calculate_Chosen_Part_Time_Amplitude(periods, 1000);

            B1_4_Square = Spirogramm_Additional_Function.Calculate_Square(periods, B1_4_Square_position);
            B2_4_Square = Spirogramm_Additional_Function.Calculate_Square(periods, B2_4_Square_position);
            B3_4_Square = Spirogramm_Additional_Function.Calculate_Square(periods, B3_4_Square_position);
            B4_4_Square = Spirogramm_Additional_Function.Calculate_Square(periods, B4_4_Square_position);


            for (int i = 0; i < ew; i++)
            {
                if (periods[i].Length < 700)
                {
                    schet[1, i] = 0;//Положение максимума производной
                    schet[2, i] = 0;// минимум В1
                    schet[3, i] = 0;// положение минимума В1 - начала отсчета
                    schet[4, i] = 0;// максимум В2 - max
                    schet[5, i] = 0;// положение максимума В2 
                    schet[6, i] = 0;// В1_4 - первая четверть
                    schet[7, i] = 0;// положение первой четверти В1_4 
                    schet[8, i] = 0;// В2_4 - вторая четверть
                    schet[9, i] = 0;// положение второй четверти В2_4 
                    schet[10, i] = 0;//В3_4 - третья четверть
                    schet[11, i] = 0;// положение третьей четверти В3_4 
                    schet[12, i] = 0;//В4_4 - четвертая четверть - или конец отсчета
                    schet[13, i] = 0;//положение четвертой четверти В4_4 
                    schet[14, i] = 0;// основание - положение В1

                }
                else
                {
                    schet[0, i] = row3[Period_job.Return_Length_X_Zero(i, max_Derivative_Amplitude[i])];
                    schet[1, i] = row1[Period_job.Return_Length_X_Zero(i, max_Derivative_Amplitude[i]), 0]; // положение максимума В2 - начала отсчета - EKG_max_x[w]

                    schet[2, i] = periods[i][0 + shift_B1]; // минимум В1
                    schet[3, i] = row1[Period_job.Return_Length_X_Zero(i, 0 + shift_B1), 0]; // положение минимума В1 - начала отсчета
                    schet[14, i] = periods[i][0 + shift_B1];

                    schet[4, i] = periods[i][max_Amplitude_position[i]] - periods[i][0 + shift_B1]; // максимум В2
                    schet[5, i] = row1[Period_job.Return_Length_X_Zero(i, max_Amplitude_position[i]), 0]; // положение максимума В2 

                    schet[6, i] = periods[i][B1_4_Square_position[i]] - periods[i][0 + shift_B1]; // первоя четверть В1-4
                    schet[7, i] = row1[Period_job.Return_Length_X_Zero(i, B1_4_Square_position[i]), 0]; // положение первой четверти В1-4 

                    schet[8, i] = periods[i][B2_4_Square_position[i]] - periods[i][0 + shift_B1]; // максимум В2-4
                    schet[9, i] = row1[Period_job.Return_Length_X_Zero(i, B2_4_Square_position[i]), 0]; // положение второй четверти В2-4

                    schet[10, i] = periods[i][B3_4_Square_position[i]] - periods[i][0 + shift_B1]; // максимум В3-4
                    schet[11, i] = row1[Period_job.Return_Length_X_Zero(i, B3_4_Square_position[i]), 0]; // положение третьей четверти В3-4

                    schet[12, i] = periods[i][end_Amplitude_position[i]] - periods[i][0 + shift_B1]; // максимум В4-4
                    schet[13, i] = row1[Period_job.Return_Length_X_Zero(i, end_Amplitude_position[i]), 0]; // положение четвертой четверти В4-4

                }
            }

            spec_point = schet;

        }

        public void Calculate_Square_Special_Point()
        {
            long[][] periods = Period_job.get_period();

            long periods_full_length = Period_job.Find_Length_Period_In_Data();

            int ew = periods.Length;//счетчик найденных максимумов

            long[,] osob_x = new long[14, ew];// список особых точек для вывода на график
            long[,] osob_y = new long[14, ew];

            long[,] schet = new long[15, ew];// список особых точек для расчета (должны отличаться!!!!!)
            long[] schet_sum = new long[15];// список особых точек

            long[,] row1 = initial_data.get_row1();
            long[] row3 = initial_data.get_row3();
            int reg = initial_data.REG;


            max_Amplitude_position = Spirogramm_Additional_Function.Calculate_Max_Amplitude_Position(periods);
            end_Amplitude_position = Spirogramm_Additional_Function.Calculate_End_Amplitude_Position(periods, max_Amplitude_position);

            max_Derivative_Amplitude = Spirogramm_Additional_Function.Calculate_Max_Derivative_Position(periods);

            full_Square = Spirogramm_Additional_Function.Calculate_Square(periods, end_Amplitude_position);
            B1_4_Square_position = Spirogramm_Additional_Function.Calculate_Chosen_Part_Square(periods, full_Square, 0.25);
            B2_4_Square_position = Spirogramm_Additional_Function.Calculate_Chosen_Part_Square(periods, full_Square, 0.5);
            B3_4_Square_position = Spirogramm_Additional_Function.Calculate_Chosen_Part_Square(periods, full_Square, 0.75);
            B4_4_Square_position = Spirogramm_Additional_Function.Calculate_Chosen_Part_Time_Amplitude(periods, 1000);

            B1_4_Square = Spirogramm_Additional_Function.Calculate_Square(periods, B1_4_Square_position);
            B2_4_Square = Spirogramm_Additional_Function.Calculate_Square(periods, B2_4_Square_position);
            B3_4_Square = Spirogramm_Additional_Function.Calculate_Square(periods, B3_4_Square_position);
            B4_4_Square = Spirogramm_Additional_Function.Calculate_Square(periods, B4_4_Square_position);


            for (int i = 0; i < ew; i++)
            {
                if (periods[i].Length < 700)
                {
                    schet[1, i] = 0;//Положение максимума производной
                    schet[2, i] = 0;// минимум В1
                    schet[3, i] = 0;// положение минимума В1 - начала отсчета
                    schet[4, i] = 0;// максимум В2 - max
                    schet[5, i] = 0;// положение максимума В2 
                    schet[6, i] = 0;// В1_4 - первая четверть
                    schet[7, i] = 0;// положение первой четверти В1_4 
                    schet[8, i] = 0;// В2_4 - вторая четверть
                    schet[9, i] = 0;// положение второй четверти В2_4 
                    schet[10, i] = 0;//В3_4 - третья четверть
                    schet[11, i] = 0;// положение третьей четверти В3_4 
                    schet[12, i] = 0;//В4_4 - четвертая четверть - или конец отсчета
                    schet[13, i] = 0;//положение четвертой четверти В4_4 
                    schet[14, i] = 0;// основание - положение В1

                }
                else
                {
                    schet[0, i] = row3[Period_job.Return_Length_X_Zero(i, max_Derivative_Amplitude[i])];
                    schet[1, i] = row1[Period_job.Return_Length_X_Zero(i, max_Derivative_Amplitude[i]), 0]; // положение максимума В2 - начала отсчета - EKG_max_x[w]

                    schet[2, i] = periods[i][0 + shift_B1]; // минимум В1
                    schet[3, i] = row1[Period_job.Return_Length_X_Zero(i, 0 + shift_B1), 0]; // положение минимума В1 - начала отсчета
                    schet[14, i] = periods[i][0 + shift_B1];
                                      
                    schet[4, i] = periods[i][max_Amplitude_position[i]] - periods[i][0 + shift_B1]; // максимум В2
                    schet[5, i] = row1[Period_job.Return_Length_X_Zero(i, max_Amplitude_position[i]), 0]; // положение максимума В2 

                    schet[6, i] = periods[i][B1_4_Square_position[i]] - periods[i][0 + shift_B1]; // первоя четверть В1-4
                    schet[7, i] = row1[Period_job.Return_Length_X_Zero(i, B1_4_Square_position[i]), 0]; // положение первой четверти В1-4 

                    schet[8, i] = periods[i][B2_4_Square_position[i]] - periods[i][0 + shift_B1]; // максимум В2-4
                    schet[9, i] = row1[Period_job.Return_Length_X_Zero(i, B2_4_Square_position[i]), 0]; // положение второй четверти В2-4

                    schet[10, i] = periods[i][B3_4_Square_position[i]] - periods[i][0 + shift_B1]; // максимум В3-4
                    schet[11, i] = row1[Period_job.Return_Length_X_Zero(i, B3_4_Square_position[i]), 0]; // положение третьей четверти В3-4

                    schet[12, i] = periods[i][end_Amplitude_position[i]] - periods[i][0 + shift_B1]; // максимум В4-4
                    schet[13, i] = row1[Period_job.Return_Length_X_Zero(i, end_Amplitude_position[i]), 0]; // положение четвертой четверти В4-4

                }
            }

            spec_point = schet;
        } 
    }
}
