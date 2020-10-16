﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Спектры_версия_2._0.Gistogramma
{
    class Data_gistogramm
    {
        Gistogramma_numeric gisto;

       private double dX = 0;//Вариационный размах
        private double IVR = 0;// Индекс вегетативного равновесия
        private double VPR = 0;// Вегетативный показатель ритма сердца
        private double PAPR = 0;//Показатель адекватности процессов регуляции ритма сердца
        private double IN = 0;// Индекс напряжения регуляторных систем
        private double CSHSS = 0;//ЧСС
        private double VKmax = 0;//Максимальная величина кардиоинтервала
        private double VKmin = 0;//Минимальная величина кардиоинтервала

        private double AMo_sum = 0; //Амплитуда моды 
        private double Mo_sum = 0;// Мода

        private double M_sred = 0; // Среднее арифметическое
        private double M_sigma = 0;// Среднее квадратичное

        private double V = 0;//Коэффициент вариации

        private double Y = 0;//Ошибка выборки

        private double A_s = 0;// "Показатель асимметрии"
        private double E_k = 0;//"Показатель эксцесса"

        bool x;

        public Data_gistogramm(Gistogramma_numeric gist, bool xxx) {

            this.gisto = gist;

            this.x = xxx;
        }

        public void calculate_all_parameters() {

            Amplitude_mod();
            Mod_();
            DX_razmax();
            IVR_();
            VPR_();
            PAPR_();
            IN_();
            max_min();
            M_SREDNEE();
            M_SREDNEKVADRA();
            CHSS();
            M_num();
            variacia();
            viborka();
            reset_diffrence();
            M_SREDNEKUB();
            M_SREDNEFOUR();
        }

        public void write_result_EKG_in_file(String name_file, bool x) {
            StreamWriter rust = new StreamWriter(name_file, x);
            rust.WriteLine();
            rust.WriteLine("Результаты");
            rust.WriteLine("RR-интервал");

            rust.WriteLine("Амплитуда моды" + "\t" + Convert.ToString((Math.Round(AMo_sum * 100, 3))) + "%");
            rust.WriteLine("Мода" + "\t" + Convert.ToString(Math.Round(Mo_sum, 3)) + " с");
            rust.WriteLine("Вариационный размах" + "\t" + Convert.ToString(Math.Round(dX, 3)) + " с");
            rust.WriteLine("Индекс вегетативного равновесия" + "\t" + Convert.ToString(Math.Round(IVR * 100, 3)));
            rust.WriteLine("Вегетативный показатель ритма сердца" + "\t" + Convert.ToString(Math.Round(VPR, 3)));
            rust.WriteLine("Показатель адекватности процессов регуляции ритма сердца" + "\t" + Convert.ToString((Math.Round(PAPR * 100, 3)) + "%"));
            rust.WriteLine("Индекс напряжения регуляторных систем" + "\t" + Convert.ToString((Math.Round(IN * 100, 3)) + "%"));
            rust.WriteLine("ЧССn" + "\t" + Convert.ToString(Math.Round(CSHSS, 3)) + " уд/м");
            rust.WriteLine("Максимальная величина кардиоинтервала" + "\t" + Convert.ToString(Math.Round(VKmax / 1000000, 3)) + " с");
            rust.WriteLine("Минимальная величина кардиоинтервала" + "\t" + Convert.ToString(Math.Round(VKmin / 1000000, 3)) + " с");
            rust.WriteLine("Среднее арифм." + "\t" + Convert.ToString(Math.Round(M_sred, 4)) + " с" + "\n");
            rust.WriteLine("Среднее квадр." + "\t" + Convert.ToString(Math.Round(M_sigma, 4)) + " c" + "\n");
            rust.WriteLine("Коэффициент вариации" + "\t" + Convert.ToString((Math.Round(V * 100, 3))) + "%" + "\n");
            rust.WriteLine("Ошибка выборки" + "\t" + Convert.ToString(Math.Round(Y, 3)) + " " + "\n");
            rust.WriteLine("Показатель асимметрии" + "\t" + Convert.ToString(Math.Round(A_s, 2)) + " " + "\n");
            rust.WriteLine("Показатель эксцесса" + "\t" + Convert.ToString(Math.Round(E_k, 2)) + " с" + "\n");

            rust.Close();
        }


        public void write_result_ALL_in_file(String name_file, bool x, String text) {
            StreamWriter rust = new StreamWriter(name_file,x);
            rust.WriteLine();
            rust.WriteLine("Результаты");
            rust.WriteLine(text);
            rust.WriteLine();
            rust.WriteLine("Амплитуда моды" + "\t" + Convert.ToString((Math.Round(AMo_sum * 100, 3))) + "%");
            rust.WriteLine("Мода" + "\t" + Convert.ToString(Math.Round(Mo_sum, 3)) + "%" + " с");
            rust.WriteLine("Вариационный размах" + "\t" + Convert.ToString(Math.Round(dX, 3)) + " с");
            rust.WriteLine("Максимальная величина кардиоинтервала" + "\t" + Convert.ToString(Math.Round(VKmax, 3)) + " с");
            rust.WriteLine("Минимальная величина кардиоинтервала" + "\t" + Convert.ToString(Math.Round(VKmin, 3)) + " с");
            rust.WriteLine("Среднее арифм." + "\t" + Convert.ToString(Math.Round(M_sred, 4)) + " с" + "\n");
            rust.WriteLine("Среднее квадр." + "\t" + Convert.ToString(Math.Round(M_sigma, 4)) + " " + "\n");
            rust.WriteLine("Коэффициент вариации" + "\t" + Convert.ToString((Math.Round(V * 100, 3))) + "%" + "\n");
            rust.WriteLine("Ошибка выборки" + "\t" + Convert.ToString(Math.Round(Y, 3)) + " " + "\n");
            rust.WriteLine("Показатель асимметрии" + "\t" + Convert.ToString(Math.Round(A_s, 2)) + " " + "\n");
            rust.WriteLine("Показатель эксцесса" + "\t" + Convert.ToString(Math.Round(E_k, 2)) + " с" + "\n");
            rust.WriteLine();
            rust.Close();

        }



        public void write_result_EKG_in_richtextbox(System.Windows.Forms.RichTextBox richtext) {
            richtext.AppendText("Результаты" + "\n");

            richtext.AppendText("Амплитуда моды" + "\t" + Convert.ToString((Math.Round(AMo_sum * 100, 3))) + "%" + "\n");
            richtext.AppendText("Мода" + "\t" + Convert.ToString(Math.Round(Mo_sum, 3)) + "\n");
            richtext.AppendText("ВР" + "\t" + Convert.ToString(Math.Round(dX, 3)) + "\n");
            richtext.AppendText("ИВР" + "\t" + Convert.ToString(Math.Round(IVR * 100, 3)) + "\n");
            richtext.AppendText("ВПР" + "\t" + Convert.ToString(Math.Round(VPR, 3)) + "\n");
            richtext.AppendText("ПАПР" + "\t" + Convert.ToString((Math.Round(PAPR * 100, 3)) + "%") + "\n");
            richtext.AppendText("ИНРС" + "\t" + Convert.ToString((Math.Round(IN * 100, 3)) + "%") + "\n");
            richtext.AppendText("ЧССn" + "\t" + Convert.ToString(Math.Round(CSHSS, 3)) + " уд/м" + "\n");
            richtext.AppendText("Xмакс" + "\t" + Convert.ToString(Math.Round(VKmax / 1000000, 3)) + "\n");
            richtext.AppendText("Xмин" + "\t" + Convert.ToString(Math.Round(VKmin / 1000000, 3)) + "\n");
            richtext.AppendText("Среднее арифм." + "\t" + Convert.ToString(Math.Round(M_sred, 4)) + "\n");
            richtext.AppendText("Среднее квадр." + "\t" + Convert.ToString(Math.Round(M_sigma, 4)) + "\n");
            richtext.AppendText("Коэффициент вариации" + "\t" + Convert.ToString((Math.Round(V * 100, 3))) + "%" + "\n");
            richtext.AppendText("Ошибка выборки" + "\t" + Convert.ToString(Math.Round(Y, 3)) + " " + "\n");
            richtext.AppendText("Показатель асимметрии" + "\t" + Convert.ToString(Math.Round(A_s, 2)) + " " + "\n");
            richtext.AppendText("Показатель эксцесса" + "\t" + Convert.ToString(Math.Round(E_k, 2)) + "\n");


        }

        public void write_result_ALL_in_richtextbox(System.Windows.Forms.RichTextBox richtext)
        {
            richtext.AppendText("Результаты" + "\n");

            richtext.AppendText("Амплитуда моды" + "\t" + Convert.ToString((Math.Round(AMo_sum * 100, 3))) + "%" + "\n");
            richtext.AppendText("Мода" + "\t" + Convert.ToString(Math.Round(Mo_sum, 3)) + "\n");
            richtext.AppendText("ВР" + "\t" + Convert.ToString(Math.Round(dX, 3)) + "\n");
            richtext.AppendText("Xмакс" + "\t" + Convert.ToString(Math.Round(VKmax, 3)) + "\n");
            richtext.AppendText("Xмин" + "\t" + Convert.ToString(Math.Round(VKmin, 3)) + "\n");
            richtext.AppendText("Среднее арифм." + "\t" + Convert.ToString(Math.Round(M_sred, 4)) + "\n");
            richtext.AppendText("Среднее квадр." + "\t" + Convert.ToString(Math.Round(M_sigma, 4)) + "\n");
            richtext.AppendText("Коэффициент вариации" + "\t" + Convert.ToString((Math.Round(V * 100, 3))) + "%" + "\n");
            richtext.AppendText("Ошибка выборки" + "\t" + Convert.ToString(Math.Round(Y, 3)) + " " + "\n");
            richtext.AppendText("Показатель асимметрии" + "\t" + Convert.ToString(Math.Round(A_s, 2)) + " " + "\n");
            richtext.AppendText("Показатель эксцесса" + "\t" + Convert.ToString(Math.Round(E_k, 2)) + "\n");

        }

        public void Amplitude_mod()//Расчет амплитуды моды
        {
            double AMo = 0;//Амплитуда моды 
            double AMo_sum_2 = 0;
            double AMo_sumx = 0;

            for (int h = 0; h < gisto.turr; h++)
            {
                if (AMo < gisto.Gistogramma[gisto.Gistogramma_number[h]])
                {
                    AMo = gisto.Gistogramma[gisto.Gistogramma_number[h]];

                }
                AMo_sum_2 = AMo_sum_2 + gisto.Gistogramma[gisto.Gistogramma_number[h]];

            }
            AMo_sumx = AMo / AMo_sum_2;

            AMo_sum = AMo_sumx;


        }
        public void Mod_()//Расчет моды
        {
            double AMo = 0;//Амплитуда моды 
            double Mo = 0;// Мода
            double Mo_sumx = 0;

            for (int h = 0; h < gisto.turr; h++)
            {
                if (AMo < gisto.Gistogramma[gisto.Gistogramma_number[h]])
                {
                    AMo = gisto.Gistogramma[gisto.Gistogramma_number[h]];
                    Mo = gisto.Gistogramma_number[h];

                }
                if (x == true)
                {
                    Mo_sumx = Mo * gisto.Shag_mod - gisto.Shag_mod * 0.5;
                    Mo_sum = Mo_sumx / 1000;
                }
                else {
                    Mo_sumx = Mo * gisto.Shag_mod_d - gisto.Shag_mod_d * 0.5;
                    Mo_sum = Mo_sumx;
                }
            }
         
           
           }
        public void DX_razmax()//Вариационный размах
        {
            if (gisto.diffrence_max > 10000)
            {
                dX = (gisto.diffrence_max - gisto.diffrence_min) / 1000000;
            }
            else {
                dX = gisto.diffrence_max - gisto.diffrence_min;

            }
            
        }
        public void IVR_()// Индекс вегетативного равновесия
        {
            IVR = AMo_sum / dX;

            
        }
        public void VPR_()// Вегетативный показатель ритма сердца
        {
            double vpr_ = 0;//Вариационный размах
            vpr_ = 1 / (Mo_sum * dX);

            VPR = vpr_;            
        }
        public void PAPR_()//Показатель адекватности процессов регуляции ритма сердца
        {
            double papr_ = 0;//Вариационный размах
            papr_ = AMo_sum / Mo_sum;

            PAPR = papr_;
        }
        public void IN_()// Индекс напряжения регуляторных систем
        {
            double inik_ = 0;//Вариационный размах

            inik_ = AMo_sum / (2 * Mo_sum * dX);
            IN = inik_;
        }
        public void CHSS() {

            CSHSS = 60 * 1000000 / M_sred;
        }

        public void max_min() {
            VKmax = gisto.diffrence_max; //Максимальная величина кардиоинтервала
            VKmin = gisto.diffrence_min; //Минимальная величина кардиоинтервала

        }

        public void M_SREDNEE()// Среднее
        {
            double M_sred_ = 0;//Вариационный размах
            double[] dif = gisto.get_diffrence_3();


            for (int i = 0; i < gisto.N_line - 1; i++)// считаем среднее
            {

                M_sred_ = M_sred_ + dif[i];

            }

            M_sred_ = M_sred_ / (gisto.N_line - 1);

            M_sred = M_sred_;

        }
        public void M_SREDNEKVADRA()// Среднеквадратичное
        {
            double M_sredkvad_ = 0;
            double M_sredkvad_sum = 0;
            double[] dif = gisto.get_diffrence_3();

            for (int i = 0; i < gisto.N_line - 1; i++)// считаем среднее
            {
                M_sredkvad_sum = M_sredkvad_sum + (M_sred - dif[i]) * (M_sred - dif[i]);
            }

            M_sredkvad_ = Math.Sqrt(M_sredkvad_sum / (gisto.N_line - 1));

            M_sigma = M_sredkvad_;

           }

        public void M_num() {
            if (M_sred > 10000)
            {
                M_sred = M_sred / 1000000;
                M_sigma = M_sigma / 1000000;
            }
            

        }

        public void variacia() {

            V = M_sigma / M_sred;//Коэффициент вариации
        }
        public void viborka() {

            Y = V / Math.Sqrt(gisto.N_line - 1);// Ошибка выборки
        }

        public void reset_diffrence() {

            double[] diff3 = gisto.get_diffrence_3();
            for (int i = 0; i < gisto.N_line; i++)
            {
                if (diff3[i] > 10000)
                {
                    diff3[i] = diff3[i] / 1000000;
                }

            }

            gisto.set_diffrence_3(diff3);
        }

        public void M_SREDNEKUB()// асимметрия
        {           
           
            double M_sredkub_sum = 0;
            double[] dif3 = gisto.get_diffrence_3();

            for (int i = 0; i < gisto.N_line - 1; i++)// считаем среднее
            {
                M_sredkub_sum = M_sredkub_sum + Math.Pow((M_sred - dif3[i]), 3);
            }

            M_sredkub_sum = M_sredkub_sum * AMo_sum;
            A_s = M_sredkub_sum / Math.Pow(M_sigma, 3);

        }
        public void M_SREDNEFOUR()// эксцесс
        {           
           
            double M_sredfour_sum = 0;
            double[] dif3 = gisto.get_diffrence_3();

            for (int i = 0; i < gisto.N_line - 1; i++)// считаем среднее
            {
                M_sredfour_sum = M_sredfour_sum + Math.Pow((M_sred - dif3[i]), 4);
            }

            M_sredfour_sum = M_sredfour_sum * AMo_sum;
            E_k = M_sredfour_sum / Math.Pow(M_sigma, 4) - 3;

        }



    }
}
