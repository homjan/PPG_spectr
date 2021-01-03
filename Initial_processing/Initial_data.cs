using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace Спектры_версия_2._0
{
    class Initial_data
    {
        const int numerical = 60;

        private long[,] row1;
        private long[,] row2;
        private long[] row3;
        private long[] row4;

        private long[][] row_divided;

        private int b;

        public int REG;
        public int EKG;

        const int potok2 = 12;

        String name_file;

        Initial_processing.Reader_data re_data;

        //Конструктур открывает файл c именем ss и заполняет содержимым 2-мерный массив 
        //reg ekg - номера каналов с рег и экг
        public Initial_data(String ss, int reg, int ekg) //
        {
            this.name_file = ss;
            this.REG = reg;
            this.EKG = ekg;

            re_data = new Initial_processing.Reader_data(name_file);

            row1 = re_data.return_read_massiv();
            b = re_data.return_read_stroki();

            row2 = new long[b + 200, potok2];
            row3 = new long[b + 200];
            row4 = new long[b + 200];

           row3_average_kanal_reg();          

        }

        public Initial_data(String ss, int reg, int ekg, bool div) //
        {
            this.name_file = ss;
            this.REG = reg;
            this.EKG = ekg;

            re_data = new Initial_processing.Reader_data(name_file);

            row_divided = re_data.return_read_massiv_divided_data();
            
           // b = re_data.return_read_stroki();

           

        }



        public void row1_shift_time_0() // сдвигаем к 0 1 столбик c временем
        {
           

            for ( int j = 3; j < b; j++)
            {
                row1[j, 0] = row1[j, 0] - row1[2, 0];
             }
            row1[2, 0] = 0;
            row1[1, 0] = 0;
            row1[0, 0] = 0;

        }

        public void row1_smothing() // сглаживаем все кроме времени
        {
            long[,] rw11 = row1;

            for (int d = 1; d < potok2; d++)
            {
                for (int q = 4; q < b - 4; q++)
                {
                    row1[q,d] = (rw11[q + 3, d] + rw11[q + 2, d] + rw11[q + 1, d] + rw11[q, d] + rw11[q - 1, d] + rw11[q - 2, d] + rw11[q - 3, d]) / 7;

                }
            }

        }

        public void row2_calculate()// считаем производную и усиливаем ее
        {
            for (int d = 1; d <= potok2; d++)
            {
                for (int q = 3; q < b-3; q++)
                {
                    row2[q, d - 1] = 1000000 * (row1[q + 1, d] - row1[q - 1, d]) / (row1[q + 1, 0] - row1[q - 1, 0]);
                }
            }
        }

        public void row3_average_kanal_reg()// усредняем производную 
        {

            for (int q = 4; q < b - 4; q++)
            {
                row3[q] = (row2[q + 3, REG - 1] + row2[q + 2, REG - 1] + row2[q + 1, REG - 1] + row2[q, REG - 1] + row2[q - 1, REG - 1] + row2[q - 2, REG - 1] + row2[q - 3, REG - 1]) / 7;

            }
        }

        public void row4_smoothing_ekg()//Сглаживаем экг
        {

             for (int q = 3; q < b - 8; q++)
                {
                    row4[q] = (row1[q, EKG] + row1[q + 7, EKG]) / 2;
                }
        }

        public void row1_write_in_file(String name_file) {
            StreamWriter rw2 = new StreamWriter(name_file);
            for (int j = 3; j < b; j++)
            {
               rw2.Write(System.Convert.ToString(row1[j, 0]));

                for (int z = 0; z < potok2; z++)
                {
                    rw2.Write(System.Convert.ToString("\t"));
                    rw2.Write(System.Convert.ToString(row1[j, z + 1]));
                }

                rw2.WriteLine();
             }

            rw2.Close();
            }

        public void row1_2_write_in_file()
        {
            StreamWriter rw2 = new StreamWriter("сигнал-производная");
            for (int j = 3; j < b; j++)
            {
                rw2.Write(System.Convert.ToString(row1[j, 0]));

                
                    rw2.Write(System.Convert.ToString("\t"));
                    rw2.Write(System.Convert.ToString(row1[j, REG]));
                rw2.Write(System.Convert.ToString("\t"));
                rw2.Write(System.Convert.ToString(row2[j, REG-1]));


                rw2.WriteLine();
            }

            rw2.Close();
        }


        //Геттеры
        public long[,] get_row1() {

            return row1;
        }
        public long[,] get_row2()
        {

            return row2;
        }
        public long[] get_row3()
        {

            return row3;
        }
        public long[] get_row4()
        {
            return row4;
        }
        public int get_b() {
            return b;
        }

        public long[][] get_row_divided()
        {

            return row_divided;
        }

        public long get_row1_x_y(int b, int kanal) {

            return row1[b, kanal];
        }

        public void set_row1(long[,] row_1_new) {

            row1 = row_1_new;
        }

        public void set_b(int bx) {
            b = bx;
        }







    }
}
