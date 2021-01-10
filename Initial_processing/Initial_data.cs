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

       
        /// <summary>
        /// Конструктор открывает файл c именем ss и заполняет содержимым 2-мерный массив 
        /// </summary>
        /// <param name="ss">Имя файла</param>
        /// <param name="reg">номер каналов с REG</param>
        /// <param name="ekg">номер каналов с EKG</param>
        public Initial_data(String ss, int reg, int ekg) //
        {
            this.name_file = ss;
            this.REG = reg;
            this.EKG = ekg;

            re_data = new Initial_processing.Reader_data(name_file);

            row1 = re_data.Return_Read_Array();
            b = re_data.Return_Read_String();

            row2 = new long[b + 200, potok2];
            row3 = new long[b + 200];
            row4 = new long[b + 200];

           Row3_Average_Canal_Reg();          

        }

        /// <summary>
        /// Конструктор открывает файл c именем ss, заполняет содержимым 2-мерный массив и разрезает данные с выбранный канал РЭГ на периоды
        /// </summary>
        /// <param name="ss">Имя файла</param>
        /// <param name="reg">номер каналов с REG</param>
        /// <param name="ekg">номер каналов с EKG</param>
        /// <param name="div"></param>
        public Initial_data(String ss, int reg, int ekg, bool div) //
        {
            this.name_file = ss;
            this.REG = reg;
            this.EKG = ekg;

            re_data = new Initial_processing.Reader_data(name_file);

            row_divided = re_data.Return_Read_Array_Divided_Data();
        }


        /// <summary>
        /// сдвигаем к 0 1 столбец c временем
        /// </summary>
        public void Row1_Shift_Time_To_0() // 
        { 
            for ( int j = 3; j < b; j++)
            {
                row1[j, 0] = row1[j, 0] - row1[2, 0];
             }
            row1[2, 0] = 0;
            row1[1, 0] = 0;
            row1[0, 0] = 0;

        }

        /// <summary>
        /// Сглаживаем по 7 точкам все кроме времени
        /// </summary>
        public void Row1_Smothing()  
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

        /// <summary>
        /// Считываем производную и усиливаем ее
        /// </summary>
        public void Row2_Calculate() 
        {
            for (int d = 1; d <= potok2; d++)
            {
                for (int q = 3; q < b-3; q++)
                {
                    row2[q, d - 1] = 1000000 * (row1[q + 1, d] - row1[q - 1, d]) / (row1[q + 1, 0] - row1[q - 1, 0]);
                }
            }
        }

        /// <summary>
        /// Усредняем производную 
        /// </summary>
        public void Row3_Average_Canal_Reg()// 
        {
            for (int q = 4; q < b - 4; q++)
            {
                row3[q] = (row2[q + 3, REG - 1] + row2[q + 2, REG - 1] + row2[q + 1, REG - 1] + row2[q, REG - 1] + row2[q - 1, REG - 1] + row2[q - 2, REG - 1] + row2[q - 3, REG - 1]) / 7;

            }
        }

        /// <summary>
        /// Сглаживаем экг
        /// </summary>
        public void Row4_Smoothing_Ekg()
        {
             for (int q = 3; q < b - 8; q++)
                {
                    row4[q] = (row1[q, EKG] + row1[q + 7, EKG]) / 2;
                }
        }

        /// <summary>
        /// Записываем массив в файл
        /// </summary>
        /// <param name="name_file">Имя файла</param>
        public void Row1_Write_In_File(String name_file)
        {
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

        /// <summary>
        /// Записать сигнал канала РЭГ и его производную в файл "сигнал-производная.txt"
        /// </summary>
        public void Row1_2_Write_In_File()
        {
            StreamWriter rw2 = new StreamWriter("сигнал-производная.txt");
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

        /// <summary>
        /// Найти положение элемента массива с выбранным временем
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int Find_Position_in_Time(long time)
        {
            int position = 0;

            long min_time = 0;
            long max_time = row1[b - 201, 0];

            int min_position = 0;
            int max_position = b;
            int current_position = (max_position + min_position) / 2;

            long current_time = row1[current_position, 0];

            if (time == 0)
            {
                return 0;
            }

            if (time > max_time)
            {
                return 0;
            }

            while (true)
            {
                if (time < current_time)
                {
                    max_time = current_time;
                    max_position = current_position;
                    current_position = (max_position + min_position) / 2;
                    current_time = row1[current_position, 0];
                }
                else if (time > current_time)
                {
                    min_time = current_time;
                    min_position = current_position;
                    current_position = (max_position + min_position) / 2;
                    current_time = row1[current_position, 0];
                }
                else if (time == current_time)
                {
                    position = current_position;
                    break;
                }
            }

            return position;
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

        /// <summary>
        ///  Возвращает выбранный элемент с выбранного канала
        /// </summary>
        /// <param name="b">Номер элемента</param>
        /// <param name="kanal"> номер канала</param>
        /// <returns></returns>
        public long Get_Row1_X_Y(int b, int kanal) {

            return row1[b, kanal];
        }

        public void set_row1(long[,] row_1_new) {

            row1 = row_1_new;
        }

        public void set_b(int bx) {
            b = bx;
        }

        public void Set_Row_In_Data_row1(long[] row, int canal) {

            for (int i = 0; i < b; i++)
            {
                row1[i, canal] = row[i];
            }

        }







    }
}
