using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Gistogramma
{
    abstract class Gistogramma_numeric
    {
       protected long[,] osob_p;
       public int N_line;

       protected int N_line_new;

        protected double[] diffrence = new double[1000]; //Расстояние между пиками
        protected double[] diffrence_3 = new double[1000];
        protected long[] diffrence_2 = new long[1000];
        protected double[] diffrence_4 = new double[1000]; //Расстояние между пиками

        public double diffrence_min;
        public double diffrence_max;

        public int[] Gistogramma;
        public int turr;// счетчик заполенных столбцов
        public int[] Gistogramma_number;
        public long Shag_mod;
        public double Shag_mod_d;
        public int GIST_SUM;

        public double[] get_diffrence() {
            return diffrence;
        }

        public long[] get_diffrence_2()
        {
            return diffrence_2;
        }

        public double[] get_diffrence_3()
        {
            return diffrence_3;
        }

        public void set_diffrence_3(double[] diff) {

            diffrence_3 = diff;
        }

        public Gistogramma_numeric(long[,] osob, int b1)
        {
            this.osob_p = osob;
            this.N_line = b1;

            this.Gistogramma = new int[180];
            this.Gistogramma_number = new int[180];

            this.turr = 0;
        }

        public virtual void set_diffrence()//Считаем разницу используемую для построения гистогорамм
        {

            for (int i = 1; i < N_line - 1; i++)
            {
                if ((osob_p[1, i + 1] - osob_p[1, i]) > 0)
                {
                    diffrence_2[i - 1] = osob_p[1, i + 1] - osob_p[1, i];
                }
            }
        }

   
        public virtual void delete_probel_diffrence(int limit)//Удаляем возможные нули
        {
            int ze = 0;
            for (int i = 0; i < N_line - 1; i++)
            {
                if (diffrence_2[i] < limit)
                {
                    for (int r = i; r < N_line - 1; r++)
                    {
                        diffrence_2[r] = diffrence_2[r + 1];
                    }
                    i--;
                    ze++;
                }
                if (i > (N_line - 1 - ze))
                {
                    break;
                }

            }
            //Пересчитываем ew
            N_line_new = N_line - ze+1;
        }

     
        public virtual void convert_diffrence_2_3()//Переводим в double
        {

            for (int i = 0; i < N_line_new; i++)
            {
                diffrence[i] = System.Convert.ToDouble(diffrence_2[i]);
                diffrence_3[i] = System.Convert.ToDouble(diffrence_2[i]);
            }
        }

        public void find_diffrence_max_min()//Находим максимальное и минимальное значение
        {
            //Находим минимум и максимум
             diffrence_min = diffrence[0];
             diffrence_max = diffrence[0];

            for (int i = 0; i < N_line_new - 1; i++)
            {
                if (diffrence_min > diffrence[i] && diffrence[i] != 0)
                {
                    diffrence_min = diffrence[i];
                }

                if (diffrence_max < diffrence[i])
                {
                    diffrence_max = diffrence[i];
                }

            }
        }


        public virtual void pilliars_gisto(String textbox5)//Находим число и высоту столбцов гистограммы
        {          
            int gist = 0;
            GIST_SUM = 0;
            int stolb = 0;// счетчик пустых столбов

            Shag_mod = System.Convert.ToInt64(textbox5);

            try
            {
                for (int i = 0; i < N_line_new - 1; i++)
                {
                    while (diffrence[i] > 0)
                    {
                        diffrence[i] = diffrence[i] - Shag_mod * 1000;
                        gist++;
                    }

                    if (gist > 0)
                    {
                        Gistogramma[gist] = Gistogramma[gist] + 1;
                        GIST_SUM++;
                        gist = 0;
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Уменьшите регулировку");
            }

            for (int kjuh = 0; kjuh < 60; kjuh++)
            {

                if (Gistogramma[kjuh] == 0)
                {
                    stolb++;
                }
                else
                {
                    Gistogramma_number[turr] = kjuh;
                    turr++;
                }

            }
        }

    }
}
