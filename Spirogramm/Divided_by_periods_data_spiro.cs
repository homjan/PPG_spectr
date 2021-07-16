using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spirogramm
{
    class Divided_by_periods_data_spiro : Initial_processing.Divided_by_periods_data
    {
        public Divided_by_periods_data_spiro(Initial_data init_data2, string text) : base(init_data2, text)
        {
        }

        public override void Calculate_Data_In_Period()
        {
            int b = init_data.get_b();
            
            long[,] row1 = init_data.get_row1();
            long[] row3 = init_data.get_row3();

            int reg = init_data.REG;


            int Shift_03 = 500; //Как далеко ищем минимум влево от точки перегиба (для спиро -800мс)

            int b0 = 0; //второй счетчик строк
            int ew = 0;//счетчик найденных максимумов
            int est = 0;
            int maxim = 0;// счетчик массива
            long[] max1_y = new long[2000]; // счетчик максимума
            long[] max1_x = new long[2000];
            long[] max1_coor = new long[2000];

            for (int u = 0; u < 2000; u++)
            {
                max1_x[u] = 1;
                max1_y[u] = 1;
            }

            // while (ew<2)
            while (b0 < b)/////////////поиск опорных точек
            {
                for (int t = 0; t < 200; t++)
                {
                    b0++;

                    if ((row3[t + 1 + est]) > max1_y[maxim])
                    {
                        max1_y[maxim] = (row3[t + 1 + est]);
                        max1_x[maxim] = row1[t + 1 + est, 0];
                        max1_coor[maxim] = t + 1 + est;
                    }
                }

                if (max1_y[maxim] > System.Convert.ToInt64(combobox_3))////////////////////!!!!!!
                {
                    ew++;// счетчик пиков производной
                    maxim++;
                }
                est = est + 200;
            }



            long[] osob_x = new long[ew + 1];// список особых точек для вывода на график
            long[] osob_y = new long[ew + 1];
            long[] osob_coor = new long[ew + 1];




            long[] period_all = new long[ew];
            period = new long[ew][];

            if (ew == 0)
            {
                System.Windows.Forms.MessageBox.Show("Выбран не тот канал");
                period = new long[1][];
                period[0] = new long[0];

            }
            else
            {

                for (int u = 1; u < ew; u++)
                {
                    period_all[u - 1] = max1_coor[u] - max1_coor[u - 1];
                }

                osob_x[0] = 0;
                osob_y[0] = 512;// Данные с соответствующего канала (№4)
                osob_coor[0] = 0;
                period[0] = new long[0];


                for (int w = 1; w < ew; w++)//перебираем пики
                {
                    //////////////////////////////ищем начало подъема--2

                    osob_x[w] = row1[max1_coor[w], 0];
                    osob_y[w] = row1[max1_coor[w], reg];// Данные с соответствующего канала (№4)
                    osob_coor[w] = max1_coor[w];


                    for (long i = max1_coor[w]; i > max1_coor[w] - Shift_03; i--)//2
                    {
                        if (row1[i, reg] < osob_y[w])
                        {
                            osob_x[w] = row1[i, 0];
                            osob_y[w] = row1[i, reg];// Данные с соответствующего канала (№4)
                            osob_coor[w] = i;

                        }
                    }
                }


                for (int i = 0; i < ew; i++)
                {
                    try
                    {
                        period[i] = new long[osob_coor[i + 1] - osob_coor[i]];
                        for (int j = 0; j < (osob_coor[i + 1] - osob_coor[i]); j++)
                        {

                            period[i][j] = row1[j + osob_coor[i], reg];
                        }
                    }
                    catch (Exception e)
                    {
                        period[i] = new long[0];
                    }

                }


            }
        }
    }
}
