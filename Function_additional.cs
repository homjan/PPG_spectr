using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0
{
    static class Function_additional
    {
        /// <summary>
        /// Сконвертировать двумерный массив long в double
        /// </summary>
        /// <param name="sloj"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double[,] Convert_Long_To_Double(long[,] sloj, int x, int y)
        {
            double[,] rowx = new double[x, y];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    rowx[i, j] = System.Convert.ToDouble(sloj[i, j]);
                }
            }

            return rowx;

        }
        /// <summary>
        /// Рассчитать производную массива
        /// </summary>
        /// <param name="sloj"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double[,] Calculate_Derivative_Array(double[,] sloj, int x, int y)
        {
            double[,] rowx = new double[x, y];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y - 1; j++)
                {
                    rowx[i, j] = sloj[i, j + 1] - sloj[i, j];
                }
            }
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (rowx[i, j] < -100)
                    {
                        rowx[i, j] = 0;
                    }
                }
            }

            return rowx;

        }
        /// <summary>
        /// Вернуть одну строку в выбранном диапазоне двумерного массива и обезразмерить
        /// </summary>
        /// <param name="sloj">массив</param>
        /// <param name="x">номер строки</param>
        /// <param name="N_nejron">Правый предел</param>
        /// <returns></returns>
        public static double[] Get_One_Line_1024(double[,] sloj, int x, int N_nejron)
        {
            double[] y = new double[N_nejron];

            for (int j = 0; j < N_nejron; j++)
            {
                y[j] = sloj[x, j] / 1024;
            }

            return y;

        }
        /// <summary>
        /// Вернуть одну строку двумерного массива
        /// </summary>
        /// <param name="sloj">Массив</param>
        /// <param name="x">Номер строки</param>
        /// <returns></returns>
        public static double[] Get_One_Line(double[,] sloj, int x)
        {
            double[] y = new double[1000];

            for (int j = 0; j < 1000; j++)
            {
                y[j] = sloj[x, j];
            }

            return y;

        }
        /// <summary>
        /// Найти максимум массива в диапазоне х - х+10
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Found_Max(double[] layer, int x)
        {
            double max_y = layer[1];
            int max_x = 1;

            for (int i = x; i < x + 10; i++)
            {
                if (max_y < layer[i])
                {
                    max_y = layer[i];
                    max_x = i;
                }

            }
            return max_x;
        }

        /// <summary>
        /// Найти минимум массива в диапазоне х - х+10
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Found_Min(double[] layer, int x)
        {
            double min_y = layer[1];
            int min_x = 1;

            for (int i = x; i < x + 10; i++)
            {
                if (min_y > layer[i])
                {
                    min_y = layer[i];
                    min_x = i;
                }

            }
            return min_x;
        }

        /// <summary>
        /// Вернуть максимальный элемент в данных, возвращаемых нейронной сетью
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static int Return_Max_Element_Neural_Network(double[] layer)
        {
            double max = 0;
            int a = 0;

            if (layer.Length < 500)
            {
                for (int i = 0; i < layer.Length; i++)
                {
                    if (max < layer[i])
                    {
                        max = layer[i];
                        a = i;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 500; i++)
                {
                    if (max < layer[i])
                    {
                        max = layer[i];
                        a = i;
                    }
                }
            }
            return a;

        }
        /// <summary>
        /// Присоеденить результат работы сети к пульсовому циклу
        /// </summary>
        /// <param name="result_NS"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static double[] layer_1000(double[] result_NS, int number)
        {
            double[] result = new double[1000];

            for (int i = 0; i < 1000; i++)
            {
                result[i] = 0;
            }

            for (int i = number; i < (number + result_NS.Length); i++)
            {
                result[i] = result_NS[i - number];
            }

            return result;

        }
        /// <summary>
        /// Найти максимум В2 В4 по приближенным данным
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="row"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double[] multiple_ten_B2_B4(double[] layer, double[] row, int x)
        {
            double[] y = new double[layer.Length * 10];
            int z = 0;
            double layer_max = layer[0];


            for (int i = 0; i < layer.Length; i++)
            {
                if (layer_max < layer[i])
                {
                    layer_max = layer[i];
                    z = i;
                }
            }
            int coor_0 = z * 10;
            int coor_1 = Found_Max(row, coor_0);

            for (int i = 0; i < y.Length; i++)
            {
                if (i == coor_1)
                {
                    y[i] = 1;
                }
            }

            return y;

        }
        /// <summary>
        /// Найти минимум В3 по приближенным данным
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="row"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double[] multiple_ten_B3(double[] layer, double[] row, int x)
        {

            double[] y = new double[layer.Length * 10];
            int z = 0;
            double layer_max = layer[0];


            for (int i = 0; i < layer.Length; i++)
            {
                if (layer_max > layer[i])
                {
                    layer_max = layer[i];
                    z = i;
                }
            }
            int coor_0 = z * 10;
            int coor_1 = Found_Min(row, coor_0);

            for (int i = 0; i < y.Length; i++)
            {
                if (i == coor_1)
                {
                    y[i] = 1;
                }
            }

            return y;

        }

        public static long Made_test(long time2)
        {
            double time = Convert.ToDouble(time2) / 1000000;
            Random ran = new Random();
            int value = ran.Next(-1000, 1000);          
            double y = 2000 * Math.Sin(2 * 3.14 * time) + 2000 + value;
            long y1 = Convert.ToInt64(y);
            return y1;
        }

    }
}
