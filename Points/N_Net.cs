using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Points
{
    class N_Net
    {
        Initial_processing.Divided_by_periods_data Periods_Data;

        long[,] row1;
        int reg;

        const int N_nejron_in = 1000;
        const int shift_B1 = 2;


        long[][] periods;
        long periods_full_length;
        long[,] periods_1000;
        int ew;
        long[,] osob_x;
        long[,] osob_y;

        long[,] schet;// список особых точек для расчета (должны отличаться!!!!!)
        long[] schet_sum;// список особых точек

        double[,] row0001;
        double[,] row01;

        //Размерности слоев сети
        private int layer_1_B2;
        private int layer_2_B2;

        private int layer_1_B3;
        private int layer_2_B3;

        private int layer_1_B4;
        private int layer_2_B4;

        private int position_search_B2;
        private int position_search_B3;
        private int position_search_B4;


        //Слои сети
        double[] sloj2B2;
        double[] sloj3B2;

        double[] sloj2B3;
        double[] sloj3B3;

        double[] sloj2B4;
        double[] sloj3B4;

        double[] sloj3B2_final;
        double[] sloj3B3_final;
        double[] sloj3B4_final;


        Job_net job_net;
        Job_net job_net2;
        Job_net job_net3;

        public N_Net(Initial_processing.Divided_by_periods_data period, long[,] data, int canal_REG)
        {
            Periods_Data = period;
            row1 = data;
            reg = canal_REG;
        }

      

        public void Set_Dimension_Layer_B2(int l1, int l2) {
           layer_1_B2 = l1;//100
           layer_2_B2 = l2;//35
        }

        public void Set_Dimension_Layer_B3(int l1, int l2)
        {
            layer_1_B3 = l1;//100
            layer_2_B3 = l2;//35
        }

        public void Set_Dimension_Layer_B4(int l1, int l2)
        {
            layer_1_B4 = l1;//100
            layer_2_B4 = l2;//35
        }

        public void Set_Position_Search(int l1, int l2, int l3)
        {
            position_search_B2 = l1;
            position_search_B3 = l2;
            position_search_B4 = l3;
    }

        private void Inicialize_Layers() {

            sloj2B2 = new double[layer_1_B2];
            sloj3B2 = new double[layer_2_B2];

            sloj2B3 = new double[layer_1_B3];
            sloj3B3 = new double[layer_2_B3];

            sloj2B4 = new double[layer_1_B4];
            sloj3B4 = new double[layer_2_B4];

            sloj3B2_final = new double[N_nejron_in];
            sloj3B3_final = new double[N_nejron_in];
            sloj3B4_final = new double[N_nejron_in];

        }

        public void Set_All_Data() {
            Set_External_Data();
            Set_Signal_Data();

            Inicialize_Layers();
            


        }

        private void Set_External_Data() {

            periods = Periods_Data.get_period();//Конвертируем выбранную поток с рег в массив периодов
            periods_full_length = Periods_Data.Find_Length_Period_In_Data();
            periods_1000 = Periods_Data.Return_Periods_1000();//Добавляем 0 в массиве до одинаковой длины=1000

            ew = periods.Length;//счетчик найденных максимумов

            osob_x = new long[14, ew];// список особых точек для вывода на график
            osob_y = new long[14, ew];

            schet = new long[15, ew];// список особых точек для расчета (должны отличаться!!!!!)
            schet_sum = new long[15];// список особых точек

        }

        private void Set_Signal_Data() 
        {

            row0001 = Function_additional.Convert_Long_To_Double(periods_1000, ew, N_nejron_in);
            row01 = Function_additional.Calculate_Derivative_Array(row0001, ew, N_nejron_in);


        }

        public void Inicialize_Net_Parameters() {

            job_net = new Job_net(N_nejron_in, layer_1_B2, layer_2_B2);
            job_net.Read_From_File_Bias_1("Сеть1/bias0B2.txt");
            job_net.Read_From_File_Bias_2("Сеть1/bias1B2.txt");

            job_net.Read_From_File_Weight_1("Сеть1/kernel0B2.txt");
            job_net.Read_From_File_Weight_2("Сеть1/kernel1B2.txt");

            /////////////////////////////////////////////
            job_net2 = new Job_net(N_nejron_in, layer_1_B3, layer_2_B3);
            job_net2.Read_From_File_Bias_1("Сеть1/bias0B3.txt");
            job_net2.Read_From_File_Bias_2("Сеть1/bias1B3.txt");

            job_net2.Read_From_File_Weight_1("Сеть1/kernel0B3.txt");
            job_net2.Read_From_File_Weight_2("Сеть1/kernel1B3.txt");

            /////////////////////////////////////
            job_net3 = new Job_net(N_nejron_in, layer_1_B4, layer_2_B4);
            job_net3.Read_From_File_Bias_1("Сеть1/bias0B4.txt");
            job_net3.Read_From_File_Bias_2("Сеть1/bias1B4.txt");

            job_net3.Read_From_File_Weight_1("Сеть1/kernel0B4.txt");
            job_net3.Read_From_File_Weight_2("Сеть1/kernel1B4.txt");

        }

        public void Inicialize_Net_Parameters_100()
        {

            job_net = new Job_net(N_nejron_in, layer_1_B2, layer_2_B2);
            job_net.Read_From_File_Bias_1("Сеть100/bias0B2.txt");
            job_net.Read_From_File_Bias_2("Сеть100/bias1B2.txt");

            job_net.Read_From_File_Weight_1("Сеть100/kernel0B2.txt");
            job_net.Read_From_File_Weight_2("Сеть100/kernel1B2.txt");

            /////////////////////////////////////////////
            job_net2 = new Job_net(N_nejron_in, layer_1_B3, layer_2_B3);
            job_net2.Read_From_File_Bias_1("Сеть100/bias0B3.txt");
            job_net2.Read_From_File_Bias_2("Сеть100/bias1B3.txt");

            job_net2.Read_From_File_Weight_1("Сеть100/kernel0B3.txt");
            job_net2.Read_From_File_Weight_2("Сеть100/kernel1B3.txt");

            /////////////////////////////////////
            job_net3 = new Job_net(N_nejron_in, layer_1_B4, layer_2_B4);
            job_net3.Read_From_File_Bias_1("Сеть100/bias0B4.txt");
            job_net3.Read_From_File_Bias_2("Сеть100/bias1B4.txt");

            job_net3.Read_From_File_Weight_1("Сеть100/kernel0B4.txt");
            job_net3.Read_From_File_Weight_2("Сеть100/kernel1B4.txt");

        }

        public void Calculate_Points() {
          
            for (int i = 0; i < ew; i++)
            {
                if (periods[i].Length < 240)
                {
                    schet[2, i] = 0;// минимум В1
                    schet[3, i] = 0;// положение минимума В1 - начала отсчета
                    schet[4, i] = 0;// максимум В2
                    schet[5, i] = 0;// положение максимума В2 - начала отсчета - EKG_max_x[w]
                    schet[6, i] = 0;// минимум В3
                    schet[7, i] = 0;// положение минимума В3 - начала отсчета- EKG_max_x[w]
                    schet[8, i] = 0;// максимум В4
                    schet[9, i] = 0;// положение максимума В4 - начала отсчета - EKG_max_x[w]
                    schet[10, i] = 0;

                }

                else
                {
                    schet[2, i] = periods[i][0 + shift_B1]; // минимум В1
                    schet[3, i] = row1[Periods_Data.Return_Length_X_Zero(i, 0 + shift_B1), 0]; // положение минимума В1 - начала отсчета
                    schet[10, i] = periods[i][0 + shift_B1];

                    double[] r1 = Function_additional.Get_One_Line(row01, i);

                    //////////////////////////////////////////////////////
                    //Нейронная сеть
                    ////////////////////////////////////////////////////////                   

                    sloj2B2 = job_net.Perzertron_forward(r1, N_nejron_in, layer_1_B2);
                    sloj3B2 = job_net.Perzertron_forward_Softmax(sloj2B2, layer_1_B2, layer_2_B2);
                    sloj3B2_final = Function_additional.layer_1000(sloj3B2, position_search_B2);

                    int B2 = Function_additional.Return_Max_Element_Neural_Network(sloj3B2_final);

                    schet[5, i] = row1[Periods_Data.Return_Length_X_Zero(i, B2), 0]; // положение максимума В2 - начала отсчета - EKG_max_x[w]
                    schet[4, i] = row1[Periods_Data.Return_Length_X_Zero(i, B2), reg] - periods[i][0 + shift_B1]; // максимум В2

                    /////////////////////////////////////////////////////////
                    sloj2B3 = job_net2.Perzertron_forward(r1, N_nejron_in, layer_1_B3);
                    sloj3B3 = job_net2.Perzertron_forward_Softmax(sloj2B3, layer_1_B3, layer_2_B3);
                    sloj3B3_final = Function_additional.layer_1000(sloj3B3, position_search_B3);

                    int B3 = Function_additional.Return_Max_Element_Neural_Network(sloj3B3_final);

                    schet[7, i] = row1[Periods_Data.Return_Length_X_Zero(i, B3), 0]; // положение максимума В2 - начала отсчета - EKG_max_x[w]
                    schet[6, i] = row1[Periods_Data.Return_Length_X_Zero(i, B3), reg] - periods[i][0 + shift_B1]; // максимум В2

                    /////////////////////////////////////////////////////////
                    sloj2B4 = job_net3.Perzertron_forward(r1, N_nejron_in, layer_1_B4);
                    sloj3B4 = job_net3.Perzertron_forward_Softmax(sloj2B4, layer_1_B4, layer_2_B4);
                    sloj3B4_final = Function_additional.layer_1000(sloj3B4, position_search_B4);

                    int B4 = Function_additional.Return_Max_Element_Neural_Network(sloj3B4_final);

                    schet[9, i] = row1[Periods_Data.Return_Length_X_Zero(i, B4), 0]; // положение максимума В2 - начала отсчета - EKG_max_x[w]
                    schet[8, i] = row1[Periods_Data.Return_Length_X_Zero(i, B4), reg] - periods[i][0 + shift_B1]; // максимум В2


                    /////////////////////////////////////////////////////////
                }

            }


        }

        public long[,] Get_Special_Point() {

            return schet;
        }


       
    }
}
