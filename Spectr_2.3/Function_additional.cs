using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0
{
    class Function_additional
    {


        public double[,] convert_long_double(long[,] sloj, int x, int y)
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

        public double[,] proizvodnaja_massiv(double[,] sloj, int x, int y)
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

        public double[] Get_one_line_1024(double[,] sloj, int x, int N_nejron)
        {
            double[] y = new double[N_nejron];

            for (int j = 0; j < N_nejron; j++)
            {
                y[j] = sloj[x, j] / 1024;
            }

            return y;

        }

        public double[] Get_one_line(double[,] sloj, int x)
        {
            double[] y = new double[1000];

            for (int j = 0; j < 1000; j++)
            {
                y[j] = sloj[x, j];
            }

            return y;

        }

        public int found_max(double[] layer, int x)
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

        public int found_min(double[] layer, int x)
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

        public int return_max_element_neural_network(double[] layer)
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

        public double[] layer_1000(double[] result_NS, int number)
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

        public double[] multiple_ten_B2_B4(double[] layer, double[] row, int x)
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
            int coor_1 = found_max(row, coor_0);

            for (int i = 0; i < y.Length; i++)
            {
                if (i == coor_1)
                {
                    y[i] = 1;
                }
            }

            return y;

        }

        public double[] multiple_ten_B3(double[] layer, double[] row, int x)
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
            int coor_1 = found_min(row, coor_0);

            for (int i = 0; i < y.Length; i++)
            {
                if (i == coor_1)
                {
                    y[i] = 1;
                }
            }

            return y;

        }

    }
}
