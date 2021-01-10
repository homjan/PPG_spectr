using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Спектры_версия_2._0.Spectr
{
    class Spectr_numeric
    {
        protected long[,] osob_s;
        public int N_line;

        protected int N_line_new;

        private static int N_peak = 1000000;

        public void set_N_peak(int x) {
            N_peak = x;
        }

        protected long[] diffrence = new long[N_peak]; //Расстояние между пиками
        protected double[] diffrence_3 = new double[N_peak];
        protected double[] diffrence_2 = new double[N_peak];
        protected double[] diffrence_4 = new double[N_peak]; //Расстояние между пиками
        private int nf;

        protected long[] diffrence_x;
        protected double[] diffrence_2_x;

        private int N_line_splain;

            
       protected double DW = 0;
       protected double DT = 0;
       protected double[] Amp_spectr;
       protected double[] Amp_spectr_pow;

        public long[] Get_Diffrence()
        {
            return diffrence;
        }

        public double[] Get_Diffrence_2()
        {
            return diffrence_2;
        }

        public double[] Get_Diffrence_3()
        {
            return diffrence_3;
        }

        public double Get_DW() {
            return DW;
        }

        public double Get_DT() {
            return DT;
        }

        public double[] Get_Amplitude_Spectr_Pow() {
            return Amp_spectr_pow;
        }

        public Spectr_numeric(long[,] osob, int b1, int nudft)
        {
            this.osob_s = osob;
            this.N_line = b1;
            this.nf = nudft;


        }
        /// <summary>
        /// Считать разницу используемую для построения гистогорамм
        /// </summary>
        public virtual void Set_Diffrence()
        {

            for (int i = 1; i < N_line - 1; i++)
            {
                if ((osob_s[1, i + 1] - osob_s[1, i]) > 0)
                {
                    diffrence[i - 1] = osob_s[1, i + 1] - osob_s[1, i];
                    diffrence_2[i - 1] = osob_s[1, i + 1] - osob_s[1, i];
                }
            }
        }
        /// <summary>
        /// Удалить возможные нули
        /// </summary>
        public virtual void Delete_Zero_Diffrence()//
        {
            int ze = 0;
            for (int i = 0; i < N_line - 1; i++)
            {
                if (diffrence_2[i] == 0 || diffrence[i] < 300000)
                {
                    for (int r = i; r < N_line - 1; r++)
                    {
                        diffrence_2[r] = diffrence_2[r + 1];
                        diffrence[r] = diffrence[r + 1];
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
            N_line_new = N_line - ze + 1;
        }
        /// <summary>
        /// Удалить нулевые значения
        /// </summary>
        public virtual void Delete_Zero_Value()
        {
            for (int r = 0; r < N_line_new - 1; r++)
            {
                diffrence_2[r] = diffrence_2[r + 1];
                diffrence[r] = diffrence[r + 1];
            }
            if (N_line_new > 0)
            {
                N_line_new = N_line_new - 1;
            }
        }


        /// <summary>
        /// Получить Амплитуду спектра из Splain_numeric
        /// </summary>
        public void Set_Amplitude_Spectr() {
                     
            if (nf == 0)
            {
                Amp_spectr = new double[N_line_new];
                Amp_spectr_pow = new double[N_line_new];
            }

            if(nf==1 || nf==2) {
                 Spectr.Splain_numeric splain = new Spectr.Splain_numeric(diffrence, diffrence_2, N_line_new, nf);
                 splain.Calculate_All();

                 diffrence_x = splain.Get_Diffrence_Final();
                 diffrence_2_x = splain.Get_Diffrence_2_Final();
                 N_line_splain = splain.Get_N_Line_1_Final();
                 N_line_new = N_line_splain;
                 Amp_spectr = new double[N_line_splain];
                 Amp_spectr_pow = new double[N_line_splain];

            }
        }

        /// <summary>
        /// Рассщитать Амплитуду спектра
        /// </summary>
        public virtual void Calculate_Amplitude_Spectr() {
            Spectr.Fourier_Fast fo;
            if (nf == 0)
            {
                fo = new Spectr.Fourier_Fast(diffrence, diffrence_2, N_line_new);
            }
            else
            {
                 fo = new Spectr.Fourier_Fast(diffrence_x, diffrence_2_x, N_line_splain);
                N_line_new = N_line_splain;
            }
            fo.Calculate_All();
            fo.Spectr_09();

            Amp_spectr = fo.Get_Amplituda_Spectr();
            DW = fo.Get_DW();
            DT = fo.Get_DT();

        }
        /// <summary>
        /// Рассщитать мощность спектра
        /// </summary>
        public virtual void Calculate_Amplitude_Spectr_Pow() {
            for (int i = 0; i < N_line_new - 1; i++)
            {
                Amp_spectr_pow[i] = Amp_spectr[i] * Amp_spectr[i];
            }

        }
        /// <summary>
        /// Получить данные для вывода в RichTextBox
        /// </summary>
        /// <param name="richtext"></param>
        /// <param name="radbutton"></param>
        public void Get_Data_From_Richtextbox(System.Windows.Forms.RichTextBox richtext, int radbutton) {

            if (radbutton == 0)
            {
                for (int i = 0; i < N_line_new; i++)
                {
                    if (nf == 0)
                    {
                        richtext.AppendText(System.Convert.ToString(i + 1) + "\t" + System.Convert.ToString(diffrence[i]) + "\t" + System.Convert.ToString(Math.Round(diffrence_2[i], 3)) + "\n");
                    }
                    else
                    {
                        richtext.AppendText(System.Convert.ToString(i + 1) + "\t" + System.Convert.ToString(diffrence_x[i]) + "\t" + System.Convert.ToString(Math.Round(diffrence_2_x[i], 3)) + "\n");
                    }
                }

            }
            if (radbutton == 1)
            {
                for (int i = 0; i < N_line_new; i++)
                {
                    richtext.AppendText(System.Convert.ToString(Math.Round(DW * (i + 1) / (2 * 3.14), 3)) + "\t" + System.Convert.ToString(Math.Round(Amp_spectr_pow[i], 3)) + "\n");
                }
            }
        }

        /// <summary>
        /// Записать данные в файл "Проверка 1"
        /// </summary>
        public void Write_In_File() {
            StreamWriter POWER = new StreamWriter("Проверка 1.txt");
            for (int i = 0; i < N_line_new - 1; i++)
            {
                POWER.WriteLine((i + 1) + "\t"  + (Amp_spectr[i]) + "\t" + Amp_spectr_pow[i]);
            }
            POWER.WriteLine("Dt" + "\t" + DT);
            POWER.WriteLine("DW" + "\t" + DW);

            POWER.Close();

        }
        /// <summary>
        /// Рассщитать мощности компонент спектра и записать их в RichTextBox и в файл fileName
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="richtext">Имя RichTextBox</param>
        /// <param name="x"></param>
        /// <param name="zaglav">Заглавие</param>
        public void Spectr_Out_Text(string fileName, System.Windows.Forms.RichTextBox richtext, bool x, string zaglav)
        {
            StreamWriter sw = new StreamWriter(fileName, x);
            double ULF_border = 0.015;// Верхняя граница сверхнизкой частоты
            double VLF_border = 0.04;// Верхняя граница очень низкой частоты
            double LF_border = 0.15;// Верхняя граница низкой частоты
            double HF_border = 0.4;// Верхняя граница высокой частоты

            double P_ULF = 0;//мощности компонент
            double P_VLF = 0;
            double P_LF = 0;
            double P_HF = 0;
            double P_SUM = 0;

            double P_ULF_sred = 0;// средние мощности компонент
            double P_VLF_sred = 0;
            double P_LF_sred = 0;
            double P_HF_sred = 0;
            double P_SUM_sred = 0;

            double P_ULF_sred_proc = 0;// средние мощности компонент в процентах
            double P_VLF_sred_proc = 0;
            double P_LF_sred_proc = 0;
            double P_HF_sred_proc = 0;


            double P_LF_HF = 0;
            double UHF = 0;//Индекс централизации

            double MMMM = 1;
            int a1 = 0, a2 = 0, a3 = 0, a4 = 0;

            for (int i = 0; i < N_line_new - 1; i++)
            {
                if ((DW * MMMM / (2 * 3.14)) < ULF_border)
                {
                    P_ULF = P_ULF + Amp_spectr_pow[i];
                    a1++;
                }
                if ((DW * MMMM / (2 * 3.14)) < VLF_border && (DW * MMMM / (2 * 3.14)) >= ULF_border)
                {
                    P_VLF = P_VLF + Amp_spectr_pow[i];
                    a2++;
                }
                if ((DW * MMMM / (2 * 3.14)) < LF_border && (DW * MMMM / (2 * 3.14)) >= VLF_border)
                {
                    P_LF = P_LF + Amp_spectr_pow[i];
                    a3++;
                }
                if ((DW * MMMM / (2 * 3.14)) < HF_border && (DW * MMMM / (2 * 3.14)) >= LF_border)
                {
                    P_HF = P_HF + Amp_spectr_pow[i];
                    a4++;
                }

                MMMM = MMMM + 1;
            }
            MMMM = 0;

            P_SUM = P_HF + P_LF + P_VLF + P_ULF;//Суммарная мощность спектра
            P_SUM_sred = P_SUM / HF_border;
            P_ULF_sred = P_ULF / (ULF_border);// средние мощности компонент
            P_VLF_sred = P_VLF / (VLF_border - ULF_border);
            P_LF_sred = P_LF / (LF_border - VLF_border);
            P_HF_sred = P_HF / (HF_border - LF_border);

            P_ULF_sred_proc = 100 * P_ULF / P_SUM;// средние мощности компонент в процентах
            P_VLF_sred_proc = 100 * P_VLF / P_SUM;
            P_LF_sred_proc = 100 * P_LF / P_SUM;
            P_HF_sred_proc = 100 * P_HF / P_SUM;

            P_LF_HF = P_LF_sred / P_HF_sred;//Отношение высокочастотного и низкочастотного
            UHF = 100 * P_ULF / P_SUM_sred;// Индекс централизации

            richtext.Text = "HF " + System.Convert.ToString(Math.Round(P_HF_sred_proc, 3)) + "%" + "\n"
            + "LF " + System.Convert.ToString(Math.Round(P_LF_sred_proc, 3)) + "%" + "\n"
            + "VLF " + System.Convert.ToString(Math.Round(P_VLF_sred_proc, 3)) + "%" + "\n"
            + "UVLF " + System.Convert.ToString(Math.Round(P_ULF_sred_proc, 3)) + "%" + "\n"
            + "HFav " + System.Convert.ToString(Math.Round(P_HF_sred, 3)) + "%" + "\n"
            + "LFav " + System.Convert.ToString(Math.Round(P_LF_sred, 3)) + "%" + "\n"
            + "VLFav " + System.Convert.ToString(Math.Round(P_VLF_sred, 3)) + "%" + "\n"
            + "UVLFav " + System.Convert.ToString(Math.Round(P_ULF_sred, 3)) + "%" + "\n"
            + "(LF/HF)av " + System.Convert.ToString(Math.Round(P_LF_HF, 3)) + "\n"
            + "ТР " + System.Convert.ToString(Math.Round(P_SUM_sred, 3)) + "мс2" + "\n"
            + "UHF " + System.Convert.ToString(Math.Round(UHF, 3)) + "%" + "\n";
            sw.WriteLine();
            sw.WriteLine(zaglav);
            sw.WriteLine("HF " + System.Convert.ToString(Math.Round(P_HF_sred_proc, 3)) + "%");
            sw.WriteLine("LF " + System.Convert.ToString(Math.Round(P_LF_sred_proc, 3)) + "%" + "\n");
            sw.WriteLine("VLF " + System.Convert.ToString(Math.Round(P_VLF_sred_proc, 3)) + "%" + "\n");
            sw.WriteLine("UVLF " + System.Convert.ToString(Math.Round(P_ULF_sred_proc, 3)) + "%" + "\n");

            sw.WriteLine("HFav " + System.Convert.ToString(Math.Round(P_HF_sred, 3)) + "%");
            sw.WriteLine("LFav " + System.Convert.ToString(Math.Round(P_LF_sred, 3)) + "%" + "\n");
            sw.WriteLine("VLFav " + System.Convert.ToString(Math.Round(P_VLF_sred, 3)) + "%" + "\n");
            sw.WriteLine("UVLFav " + System.Convert.ToString(Math.Round(P_ULF_sred, 3)) + "%" + "\n");

            sw.WriteLine("(LF/HF)av " + System.Convert.ToString(Math.Round(P_LF_HF, 3)) + "\n");
            sw.WriteLine("ТР " + System.Convert.ToString(Math.Round(P_SUM_sred, 3)) + "мс2" + "\n");
            sw.WriteLine("UHF " + System.Convert.ToString(Math.Round(UHF, 3)) + "%" + "\n");
            sw.WriteLine();
            

            sw.Close();

        }




    }
}
