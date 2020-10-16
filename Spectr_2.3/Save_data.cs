using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0
{
    class Save_data
    {
        public Save_data() {
         }

        public long compression(long[,] osob, long ew1) {

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

              //  ew--;
            }

            return ew;

        }

        public void save_osob_point_clear(long[,] osob, long ew) {
            ew = compression(osob, ew);
            StreamWriter rw = new StreamWriter("Особые точки чистые.txt");

            for (int i = 0; i < ew - 1; i++)
            {

                rw.WriteLine(osob[1, i] + "\t" + osob[0, i] + "\t" + osob[3, i] + "\t" +
                    osob[2, i] + "\t" + osob[5, i] + "\t" + osob[4, i] + "\t" + osob[7, i] + "\t" +
                    osob[6, i] + "\t" + osob[9, i] + "\t" + osob[8, i] + "\t" + osob[10, i]);
            }
            rw.Close();         

        }


        public void save_osob_point_postr(long[,] osob, long ew)
        {
            ew = compression(osob, ew);
           
            StreamWriter rw2 = new StreamWriter("Особые точки - построение.txt");

            for (int i = 0; i < ew - 1; i++)
            {
                rw2.WriteLine(osob[1, i] + "\t" + osob[0, i] + "\t" + osob[3, i] + "\t" +
                    osob[2, i] + "\t" + osob[5, i] + "\t" + (osob[4, i] + osob[10, i]) + "\t" + osob[7, i]
                    + "\t" + (osob[6, i] + osob[10, i]) + "\t" + osob[9, i] + "\t" + (osob[8, i] + osob[10, i]));
            }
            rw2.Close();

        }



        public void clearDataINFile(String adres)
        {
            StreamWriter file = new StreamWriter(adres, false);       //ofstream
            file.Close();
        }
    }
}
