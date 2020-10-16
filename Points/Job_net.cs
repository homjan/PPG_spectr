﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Спектры_версия_2._0
{
    class Job_net
    {
        private double[,] weight_1;
        private double[,] weight_2;

        private double[] bias0;
        private double[] bias1;

        int razmer_data_in;
        int razmer_layer_1_in;
        int razmer_layer_2_in;

        public Job_net(int razmer1, int razmer2, int razmer3)
        {

            this.razmer_data_in = razmer1;
            this.razmer_layer_1_in = razmer2;
            this.razmer_layer_2_in = razmer3;

            weight_1 = new double[razmer_data_in, razmer_layer_1_in];
            weight_2 = new double[razmer_layer_1_in, razmer_layer_2_in];

            bias0 = new double[razmer_layer_1_in];
            bias1 = new double[razmer_layer_2_in];


        }

        public void read_in_file_bias_1(String name_file)
        {

            StringBuilder buffer = new StringBuilder();
            int a = 0;
            int b = 0;//счетчик строк

            string n1;

            int l1;
            int j = 0;// счетчик строк 10         

            int m = 0;//смещение буффера


            //  try
            //  {
            StreamReader sw = new StreamReader(name_file);

            while (sw.Peek() != -1)
            {
                l1 = sw.Read();


                if (l1 == 13)
                {
                    buffer.Replace('.', ',');
                    n1 = buffer.ToString(); // пищем цифру в строку

                    buffer.Remove(0, n1.Length); //очищаем буффер

                    //   n1.Replace('.', ',');                             // rw2.WriteLine(n1);
                    //    double rtes = Convert.ToDouble("123,5");
                    //    double gg=1;
                    if (n1 != "")
                    {
                        bias0[j] = System.Convert.ToDouble(n1);// пишем в массив
                    }
                    j++; // переходим на следующую строку

                    m = 0;
                    b++;
                }

                if (l1 == 48 || l1 == 49 || l1 == 50 || l1 == 51 || l1 == 52 || l1 == 53 || l1 == 54 || l1 == 55 || l1 == 56 || l1 == 57 || l1 == 46)
                {
                    buffer.Insert(m, System.Convert.ToChar(l1)); // пишем символ
                    m++;
                }
                else
                {
                    a++;
                    continue;
                }

            }

            sw.Close();

        }

        public void read_in_file_bias_2(String name_file)
        {

            StringBuilder buffer = new StringBuilder();
            int a = 0;
            int b = 0;//счетчик строк

            string n1;

            int l1;
            int j = 0;// счетчик строк 10         

            int m = 0;//смещение буффера

            //  try
            //  {
            StreamReader sw = new StreamReader(name_file);

            while (sw.Peek() != -1)
            {
                l1 = sw.Read();


                if (l1 == 13)
                {
                    buffer.Replace('.', ',');
                    n1 = buffer.ToString(); // пищем цифру в строку
                    buffer.Remove(0, n1.Length); //очищаем буффер

                    if (n1 != "")
                    {
                        bias1[j] = System.Convert.ToDouble(n1);// пишем в массив
                    }
                    j++; // переходим на следующую строку

                    m = 0;
                    b++;
                }

                if (l1 == 48 || l1 == 49 || l1 == 50 || l1 == 51 || l1 == 52 || l1 == 53 || l1 == 54 || l1 == 55 || l1 == 56 || l1 == 57 || l1 == 46)
                {
                    buffer.Insert(m, System.Convert.ToChar(l1)); // пишем символ
                    m++;
                }
                else
                {
                    a++;
                    continue;
                }

            }

            sw.Close();


        }


        public void read_in_file_weight_1(String name_file)
        {
            StringBuilder buffer = new StringBuilder();
            int a = 0;
            int b = 0;//счетчик строк

            string n1;

            int l1;
            int j = 0;// счетчик строк 10

            int k = 0;//счетчик столбцов 2


            int m = 0;//смещение буффера


            long[,] rowx = new long[razmer_layer_1_in, razmer_data_in];

            //  try
            //  {
            StreamReader sw = new StreamReader(name_file);

            while (sw.Peek() != -1)
            {
                l1 = sw.Read();


                if (l1 == 13)
                {
                    buffer.Replace('.', ',');
                    n1 = buffer.ToString(); // пищем цифру в строку
                    buffer.Remove(0, n1.Length); //очищаем буффер
                                                 // rw2.WriteLine(n1);
                    if (n1 != "")
                    {
                        weight_1[j, k] = System.Convert.ToDouble(n1);// пишем в массив
                    }
                    j++; // переходим на следующую строку
                    k = 0; // переходим на первый столбец
                    m = 0;
                    b++;
                }
                if (l1 == 9)
                {
                    buffer.Replace('.', ',');
                    n1 = buffer.ToString(); // пищем цифру в строку
                    buffer.Remove(0, n1.Length); //очищаем буффер
                    if (n1 != "")
                    {
                        weight_1[j, k] = System.Convert.ToDouble(n1);// пишем в массив
                    }
                    k++; // переходим на следующий столбец
                    m = 0;
                }
                if (l1 == 48 || l1 == 49 || l1 == 50 || l1 == 51 || l1 == 52 || l1 == 53 || l1 == 54 || l1 == 55 || l1 == 56 || l1 == 57 || l1 == 46)
                {
                    buffer.Insert(m, System.Convert.ToChar(l1)); // пишем символ
                    m++;
                }
                else
                {
                    a++;
                    continue;
                }
            }
            sw.Close();
        }


        public void read_in_file_weight_2(String name_file)
        {
            StringBuilder buffer = new StringBuilder();
            int a = 0;
            int b = 0;//счетчик строк

            string n1;

            int l1;
            int j = 0;// счетчик строк 10
            int k = 0;//счетчик столбцов 2
            int m = 0;//смещение буфера

            long[,] rowx = new long[razmer_layer_2_in, razmer_layer_1_in];

            //  try
            //  {
            StreamReader sw = new StreamReader(name_file);

            while (sw.Peek() != -1)
            {
                l1 = sw.Read();


                if (l1 == 13)
                {
                    buffer.Replace('.', ',');
                    n1 = buffer.ToString(); // пищем цифру в строку
                    buffer.Remove(0, n1.Length); //очищаем буффер
                                                 // rw2.WriteLine(n1);
                    if (n1 != "")
                    {
                        weight_2[j, k] = System.Convert.ToDouble(n1);// пишем в массив
                    }
                    j++; // переходим на следующую строку
                    k = 0; // переходим на первый столбец
                    m = 0;
                    b++;
                }
                if (l1 == 9)
                {
                    buffer.Replace('.', ',');
                    n1 = buffer.ToString(); // пищем цифру в строку
                    buffer.Remove(0, n1.Length); //очищаем буффер
                    if (n1 != "")
                    {
                        weight_2[j, k] = System.Convert.ToDouble(n1);// пишем в массив
                    }
                    k++; // переходим на следующий столбец
                    m = 0;
                }
                if (l1 == 48 || l1 == 49 || l1 == 50 || l1 == 51 || l1 == 52 || l1 == 53 || l1 == 54 || l1 == 55 || l1 == 56 || l1 == 57 || l1 == 46)
                {
                    buffer.Insert(m, System.Convert.ToChar(l1)); // пишем символ
                    m++;
                }
                else
                {
                    a++;
                    continue;
                }

            }

            sw.Close();

        }

        public double[] Perzertron_forward(double[] x, int data_in, int layer_1_in)
        {
            double[] y = new double[layer_1_in];

            for (int i = 0; i < layer_1_in; i++)
            {
                for (int j = 0; j < data_in; j++)
                {
                    y[i] = y[i] + (weight_1[j, i] * x[j]);
                }
            }

            for (int i = 0; i < layer_1_in; i++)
            {
                y[i] = y[i] + bias0[i];
            }

            for (int i = 0; i < layer_1_in; i++)
            {

                y[i] = sigmoid(y[i]);
            }

            return y;

        }

        public double[] Perzertron_forward_softmax(double[] x, int layer_1_in, int layer_2_in)
        {
            double[] y = new double[layer_2_in];
            double[] z = new double[layer_2_in];


            for (int i = 0; i < layer_2_in; i++)
            {
                for (int j = 0; j < layer_1_in; j++)
                {
                    y[i] = y[i] + (weight_2[j, i] * x[j]);
                }
            }
            for (int i = 0; i < layer_2_in; i++)
            {
                y[i] = y[i] + bias1[i];
            }

            z = softmax(y);
            /*     for (int i = 0; i < x.Length; i++)
                 {

                     y[i] = sigmoid(y[i]);
                 }*/

            return z;

        }

        public double sigmoid(double x)
        {

            double y = 1 / (1 + Math.Exp((-1) * x));
            return y;
        }


        public double tanh(double x)
        {

            double y = (Math.Exp(x) - Math.Exp((-1) * x)) / (Math.Exp(x) + Math.Exp((-1) * x));
            return y;
        }

        public double RELU(double x)
        {
            double y;
            if (x < 0) { y = 0; }
            else { y = x; }

            return y;
        }

        public double[] softmax(double[] x)
        {
            double[] y = new double[x.Length];
            double maxi = x.Max();

            for (int i = 0; i < x.Length; i++)
            {
                y[i] = Math.Exp(x[i] - maxi);
            }
            double sum = y.Sum();

            for (int i = 0; i < x.Length; i++)
            {
                y[i] = y[i] / sum;

            }
            return y;
        }
    }
}
