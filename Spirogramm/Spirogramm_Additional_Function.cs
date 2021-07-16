using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spirogramm
{
    static class Spirogramm_Additional_Function
    {
        /// <summary>
        /// Ищем положение максимальной амплитуды
        /// </summary>
        /// <param name="periods"></param>
        /// <returns></returns>
        public static int[] Calculate_Max_Amplitude_Position(long[][] periods)
        {
            int full_length = periods.Length;
            long max=0;
            int[] data = new int[full_length];

            for (int i = 0; i < periods.Length; i++)
            {
                max = 0;
                for (int j = 0; j < periods[i].Length; j++)
                {
                    if (max<periods[i][j])
                    {
                        max = periods[i][j];
                        data[i] = j;
                    }
                }                
            }

            return data;
        }

        /// <summary>
        /// Ищем положение 1/5 максимальной амплитуды
        /// </summary>
        /// <param name="periods"></param>
        /// <param name="position_max"></param>
        /// <returns></returns>
        public static int[] Calculate_End_Amplitude_Position(long[][] periods, int[] position_max)
        {
            int full_length = periods.Length;
            long max = 0;
            int[] data = new int[full_length];

            for (int i = 0; i < periods.Length-1; i++)
            {
                max = periods[i][position_max[i]]-periods[i][0];
                for (int j = position_max[i]; j < periods[i].Length; j++)
                {
                    if ((periods[i][j]-periods[i][0])<(max/5))
                    {                        
                        data[i] = j;
                        break;
                    }
                }
            }

            return data;
        }
        /// <summary>
        /// Ищем площадь спирограммы
        /// </summary>
        /// <param name="periods"></param>
        /// <param name="right_bound">положение 1/5 максимальной амплитуды</param>
        /// <returns></returns>
        public static long[] Calculate_Square(long[][] periods, int[] right_bound)
        {
            int full_length = periods.Length;
            long[] data = new long[full_length];
            long ground = 0;

            for (int i = 0; i < periods.Length-1; i++)
            {
                ground = periods[i][0];
                for (int j = 0; j < right_bound[i]; j++)
                {
                    if (j < periods[i].Length)
                    {
                        data[i] = data[i] + (periods[i][j] - ground);
                    }
                    else {
                        data[i] = data[i];
                    }
                   
                }
            }

            return data;
        }

        /// <summary>
        /// Рассчитать определенный интеграл от 0 до part и вывести положение точки part
        /// </summary>
        /// <param name="periods"></param>
        /// <param name="square"></param>
        /// <param name="part">доля площади</param>
        /// <returns></returns>
        public static int[] Calculate_Chosen_Part_Square(long[][] periods, long[] square, double part)
        {
            if (part>1.0)
            {
                part = 1.0;
            }
            if (part<0.0)
            {
                part = 0.0;
            } 
                 
            int full_length = square.Length;
            int[] data = new int[full_length];
            long ground = 0;
            long part_square = 0;
            long sum = 0;
            int j = 0;

            for (int i = 0; i < periods.Length-1; i++)
            {
                ground = periods[i][0];
                part_square =Convert.ToInt64( Convert.ToDouble(square[i]) * part);
                sum = 0;
                j = 0;

               while(sum<part_square)
                {
                    sum = sum + (periods[i][j] - ground);
                    data[i] = j;
                    j++;
                }
            }

            return data;
        }

        /// <summary>
        /// Рассчитать Амплитуду по константе
        /// </summary>
        /// <param name="periods"></param>
        /// <param name="time">время в мс</param>
        /// <returns></returns>
        public static int[] Calculate_Chosen_Part_Time_Amplitude(long[][] periods, double time) {

            int full_length = periods.Length;
            int[] data = new int[full_length];
            int position = Convert.ToInt32( time/1.66);

            for (int i = 0; i < periods.Length; i++)
            {
               
                data[i] = position;
               
            }

            return data;

        }

        public static int[] Calculate_Max_Derivative_Position(long[][] periods)
        {
            int full_length = periods.Length;
            long max = 0;
            int[] data = new int[full_length];
            long[][] periods_derivative = new long[periods.Length][];
            for (int i = 0; i < periods.Length; i++)
            {
                periods_derivative[i] = new long[periods[i].Length];
            }
           
            //Считаем производную
            for (int i = 0; i < periods.Length-1; i++)
            {                
                for (int j = 25; j < periods[i].Length-1; j++)
                {
                    periods_derivative[i][j] = periods[i][j]- periods[i][j-25];
                }
                periods_derivative[i][periods[i].Length - 1] = 0;
            }
          

                //Ищем максимум
                for (int i = 0; i < periods_derivative.Length-1; i++)
            {
                max = -1000;
                for (int j = 0; j < periods_derivative[i].Length; j++)
                {
                    if (max < periods_derivative[i][j])
                    {
                        max = periods_derivative[i][j];
                        data[i] = j;
                    }
                }
            }

            return data;


        }

        public static double[] Calculate_sred_SOSV(long[] m1, long[] m2, long[] m3, long[] m4) {

            double[] data = new double[m1.Length];
           
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToDouble(m1[i]+ m2[i] + m3[i] + m4[i]) / 4;
            }

            return data;

        }

      /*  public static double[] Calculate_Tiffno_Vatchap_Index() {

            double[] data = new double[5];

            for (int i = 0; i < data.Length; i++)
            {
             
            }

            return data;

        }*/

    }
}
