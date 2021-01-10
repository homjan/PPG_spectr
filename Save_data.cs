using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0
{
    static class Save_data
    {
        
        /// <summary>
        /// Удалить нули из массива особых точек
        /// </summary>
        /// <param name="osob">массив особых точек</param>
        /// <param name="ew1">Число элементов</param>
        /// <returns></returns>
        public static long Compression(long[,] osob, long ew1) {

            int a = 5;
            long ew = ew1;

            for (int i = 0; i < ew - 1; i++)
            {
                if (osob[1, i]==0 && osob[0, i] == 0 && osob[3, i] == 0 && osob[2, i] == 0 && osob[5, i] == 0 && osob[4, i] == 0 && osob[7, i] == 0 &&
                    osob[6, i] == 0 && osob[9, i] == 0 && osob[8, i] == 0 && osob[10, i] == 0)
                {
                    for (int r = i; r < ew - 1; r++)
                    {
                        osob[0, r] = osob[0, r + 1];
                        osob[1, r] = osob[1, r + 1];
                        osob[2, r] = osob[2, r + 1];
                        osob[3, r] = osob[3, r + 1];
                        osob[4, r] = osob[4, r + 1];
                        osob[5, r] = osob[5, r + 1];
                        osob[6, r] = osob[6, r + 1];
                        osob[7, r] = osob[7, r + 1];
                        osob[8, r] = osob[8, r + 1];
                        osob[9, r] = osob[9, r + 1];
                        osob[10, r] = osob[10, r + 1];
                        osob[11, r] = osob[11, r + 1];
                        osob[12, r] = osob[12, r + 1];
                        osob[13, r] = osob[13, r + 1];
                        osob[14, r] = osob[14, r + 1];

                    }
                    i--;
                    ew--;
                    
                }
            }
            return ew;
        }
        /// <summary>
        /// Сохранить данные для обработки данных графика
        /// </summary>
        /// <param name="osob">массив особых точек</param>
        /// <param name="ew">Число элементов</param>
        public static void Save_Special_Point_For_File(long[,] osob, long ew) {
            ew = Compression(osob, ew);
            StreamWriter rw = new StreamWriter("Особые точки чистые.txt");

            for (int i = 0; i < ew - 1; i++)
            {

                rw.WriteLine(osob[1, i] + "\t" + osob[0, i] + "\t" + osob[3, i] + "\t" +
                    osob[2, i] + "\t" + osob[5, i] + "\t" + osob[4, i] + "\t" + osob[7, i] + "\t" +
                    osob[6, i] + "\t" + osob[9, i] + "\t" + osob[8, i] + "\t" + osob[10, i]);
            }
            rw.Close();         

        }

        /// <summary>
        /// Сохранить данные для построения графика
        /// </summary>
        /// <param name="osob">массив особых точек</param>
        /// <param name="ew">Число элементов</param>
        public static void Save_Special_Point_For_Plotting(long[,] osob, long ew)
        {
            ew = Compression(osob, ew);
           
            StreamWriter rw2 = new StreamWriter("Особые точки - построение.txt");

            for (int i = 0; i < ew - 1; i++)
            {
                rw2.WriteLine(osob[1, i] + "\t" + osob[0, i] + "\t" + osob[3, i] + "\t" +
                    osob[2, i] + "\t" + osob[5, i] + "\t" + (osob[4, i] + osob[10, i]) + "\t" + osob[7, i]
                    + "\t" + (osob[6, i] + osob[10, i]) + "\t" + osob[9, i] + "\t" + (osob[8, i] + osob[10, i]));
            }
            rw2.Close();

        }


        /// <summary>
        /// Очистить файл
        /// </summary>
        /// <param name="adres">адрес файла</param>
        public static void Clear_Data_In_File(String adres)
        {
            StreamWriter file = new StreamWriter(adres, false);       //ofstream
            file.Close();
        }
    }
}
