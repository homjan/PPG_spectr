using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Jenyay.Mathematics;

using ZedGraph;

namespace Спектры_версия_2._0
{
    public partial class Form1 : Form
    {
        UseZedgraph usergraph;

        public Form1()
        {
            InitializeComponent();
        }

        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        delegate void SetTextCallback(int nomer);

        const double PI_ = 3.1415926;
        const int time_numerical = 60;
        double N_shift_axis = 0;
        double max_time = 0;

        public String texti = "50";

        int radbutton = 0;

        int radbutton_spectr = 0;

      
        private void progrBar(int nomer)//Безопасный доступ к прогресбару
        {
            if (this.progressBar1.InvokeRequired)
            {

                SetTextCallback d = new SetTextCallback(progrBar);
                this.Invoke(d, new object[] { nomer });
            }
            else
            {
                this.progressBar1.Value = nomer;
            }

        }

        public void ProgBarSafe(int value_progBar)
        {
            this.progrBar(value_progBar);
        }

        private void Inizial()
        {
            StreamWriter sw = new StreamWriter("Информация о пациенте.txt");

            sw.WriteLine("Фамилия: " + textBox8.Text + "\n");
            sw.WriteLine("Имя: " + textBox12.Text + "\n");
            sw.WriteLine("Отчество: " + textBox9.Text + "\n");
            sw.WriteLine("Пол: " + comboBox2.Text + "\n");
            sw.WriteLine("Рост (см): " + System.Convert.ToString(this.textBox7.Text) + "\n");
            sw.WriteLine("Вес (кг): " + System.Convert.ToString(this.textBox6.Text) + "\n");
            sw.WriteLine("Возраст: " + System.Convert.ToString(this.textBox10.Text) + "\n");
            sw.WriteLine("Состояние (покой/нагрузка): " + textBox4.Text + "\n");

            sw.WriteLine(System.Convert.ToString(this.richTextBox4.Text) + "\n");


            string s = DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss");
            sw.WriteLine(s);
            sw.Close();
        }

        private void Read_Info_pazient()
        {
            StreamReader rex = new StreamReader("Информация о пациенте.txt");
            string[] dar = new string[34];

            for (int i = 0; i < 34; i++)
            {
                dar[i] = rex.ReadLine();
            }


            textBox8.Text = dar[0].Substring(9);
            textBox12.Text = dar[2].Substring(5);
            textBox9.Text = dar[4].Substring(10);
            comboBox2.Text = dar[6].Substring(5);
            textBox7.Text = dar[8].Substring(11);
            textBox6.Text = dar[10].Substring(10);
            textBox10.Text = dar[12].Substring(9);
            textBox4.Text = dar[14].Substring(28);

            richTextBox4.Text = dar[16];



            rex.Close();
        }

        void Clear_list_zed()
        {
            ZedGraph.MasterPane masterPane = zedGraph1.MasterPane;

            masterPane.PaneList.Clear();
                     
            // Создаем экземпляр класса GraphPane, представляющий собой один график
            GraphPane pane = new GraphPane();

            pane.CurveList.Clear();

            // Добавим новый график в MasterPane
            masterPane.Add(pane);

            // Установим масштаб по умолчанию для оси X
            pane.XAxis.Scale.MinAuto = true;
            pane.XAxis.Scale.MaxAuto = true;

            // Установим масштаб по умолчанию для оси Y
            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;

            using (Graphics g = CreateGraphics())
            {
                // Закомментарены разные варианты (не все) размещения графиков.

                // Графики будут размещены в один столбец друг под другом
                masterPane.SetLayout(g, PaneLayout.SingleColumn);

             
            }
                      
            zedGraph1.AxisChange();
            zedGraph1.Invalidate();

        }//Очистить график - функция


        /////////////////////////////////
        //Разное
        ///////////////////

        private void button1_Click(object sender, EventArgs e)//Получить список СОМ портов
        {

            string[] portnames = SerialPort.GetPortNames();
            StreamWriter swx = new StreamWriter("порт.txt");
            foreach (string port in portnames)
            {
                swx.WriteLine(port);
            }
            swx.Close();
            StreamReader a = new StreamReader("порт.txt");
            richTextBox1.Text = a.ReadToEnd();
            a.Close();
        }

        private void button7_Click(object sender, EventArgs e)//Восстановить данные
        {
            Read_Info_pazient();
        }

        /////////////////////////////////
        //Работа с финальными данными
        ///////////////////
       

        private void button3_Click(object sender, EventArgs e)//Очистить график
        {
            try
            {
                usergraph.ClearAll();
                Inizial();
                Clear_list_zed();
                richTextBox2.Clear();
                richTextBox3.Clear();
            }
            catch { }
            
        }

        private void button6_Click(object sender, EventArgs e)//Соханить график
        {
            usergraph.SaveGraph();
        }

        private void button5_Click(object sender, EventArgs e)//Сохранить данные
        {
            string adres = "q";
            string datapath = "w";

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                adres = fbd.SelectedPath;
                datapath = Path.Combine(Application.StartupPath);
               
               FileInfo f1 = new FileInfo(Path.Combine(Application.StartupPath, "test3.txt"));
               if (f1.Exists)
                {
                    f1.CopyTo(Path.Combine(adres, "test.txt"), true);
                }
                   
                FileInfo f2 = new FileInfo(Path.Combine(Application.StartupPath, "Расчетные данные гистограмм.txt"));
                if (f2.Exists)
                {
                    f2.CopyTo(Path.Combine(adres, "Расчетные данные гистограмм.txt"), true);
                }                

                FileInfo f3 = new FileInfo(Path.Combine(Application.StartupPath, "Расчетные данные спектра.txt"));
                if (f3.Exists)
                {
                    f3.CopyTo(Path.Combine(adres, "Расчетные данные спектра.txt"), true);
                }                

                FileInfo f4 = new FileInfo(Path.Combine(Application.StartupPath, "Информация о пациенте.txt"));
                if (f4.Exists)
                {
                    f4.CopyTo(Path.Combine(adres, "Информация о пациенте.txt"), true);
                }               

                FileInfo f5 = new FileInfo(Path.Combine(Application.StartupPath, "Точки гистограмм.txt"));
                if (f5.Exists)
                {
                    f5.CopyTo(Path.Combine(adres, "Точки гистограмм.txt"), true);
                }                

                FileInfo f6 = new FileInfo(Path.Combine(Application.StartupPath, "Точки спектра.txt"));
                if (f6.Exists)
                {
                    f6.CopyTo(Path.Combine(adres, "Точки спектра.txt"), true);
                }                

                FileInfo f7 = new FileInfo(Path.Combine(Application.StartupPath, "Особые точки - построение.txt"));

                if (f7.Exists)
                {
                    f7.CopyTo(Path.Combine(adres, "Особые точки - построение.txt"), true);
                }               

                FileInfo f8 = new FileInfo(Path.Combine(Application.StartupPath, "Особые точки чистые.txt"));
                if (f8.Exists)
                {
                    f8.CopyTo(Path.Combine(adres, "Особые точки чистые.txt"), true);
                }

            }
        }


        /////////////////////////////////
        //Снятие и обработка
        ///////////////////
        private void button4_Click(object sender, EventArgs e)// Открыть файл
        {
            Inizial();
            N_shift_axis = 0;
            progressBar1.Value = 0;

            string adres = "q";
            string adres2 = "q";
            string datapath = "w";

            int da5 = 0;

            StringBuilder buffer2 = new StringBuilder();

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            OpenFileDialog qqq = new OpenFileDialog();
            qqq.Filter = "Файлы txt|*.txt";

            if (qqq.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //adres = qqq.SelectedPath;
                adres = qqq.FileName;
                //       textBox4.Text = adres;
                buffer2.Insert(0, qqq.FileName);

                da5 = buffer2.Length;
                buffer2.Remove(da5 - 8, 8);
                adres2 = buffer2.ToString();
                datapath = Path.Combine(Application.StartupPath);
         
                System.IO.File.Copy(Path.Combine(qqq.InitialDirectory, qqq.FileName), Path.Combine(datapath, "test.txt"), true);
                try
                {
                    System.IO.File.Copy(adres2 + "Информация о пациенте.txt", Path.Combine(datapath, "Информация о пациенте.txt"), true);
                }
                catch
                {
                }
            }

            ///////////////////////////////////////////////////////////
            

            int reg = System.Convert.ToInt32(this.textBox13.Text);
            int ekg = System.Convert.ToInt32(this.textBox2.Text);

            try
            {
                Initial_data init_data = new Initial_data("test.txt", reg, ekg);
                init_data.Row1_Shift_Time_To_0();//Сдвигаем время к 0
                init_data.Row1_Smothing();// Сглаживаем полученные данные
                init_data.Row1_Write_In_File("test3.txt");


           //     Initial_processing.Divided_by_periods_data didi = new Initial_processing.Divided_by_periods_data(init_data);
           //     didi.return_data_in_period(this.comboBox3.Text);
             

                usergraph = new UseZedgraph(zedGraph1);
                usergraph.ClearAll();//Очищаем полотно
                usergraph.MakeGraph_4_Canal(init_data.get_row1(), init_data.get_b());//Строим график
                usergraph.Install_Pane("t, мc", "R, Ом", "Каналы");//Устанавливаем оси и загавие
                usergraph.ResetGraph();//Обновляем
                max_time = Convert.ToDouble(init_data.Get_Row1_X_Y(init_data.get_b() - 1, 0));

            }
            catch (Exception ex)
            {
                MessageBox.Show("Выбран неправильный файл");
            }

            Read_Info_pazient();


        }

        /// <summary>
        /// Обновить график
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button19_Click(object sender, EventArgs e)
        {
            Inizial();
            N_shift_axis = 0;
            progressBar1.Value = 0;

            int reg = System.Convert.ToInt32(this.textBox13.Text);
            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            try
            {
                Initial_data init_data = new Initial_data("test3.txt", reg, ekg);
                //      init_data.row1_shift_time_0();
                //       init_data.row1_smothing();
                if (checkBox2.Checked)
                {init_data.Set_Row_In_Data_row1(
                    Calculate_Fast_Fourier_Signal_Filtration(init_data.get_row1(), init_data.get_b()), reg);
                }
              
                usergraph = new UseZedgraph(zedGraph1);
                usergraph.ClearAll();//Очищаем полотно
                usergraph.MakeGraph_4_Canal(init_data.get_row1(), init_data.get_b());//Строим график
                usergraph.Install_Pane("t, мc", "R, Ом", "Каналы");//Устанавливаем оси и загавие
                usergraph.ResetGraph();//Обновляем
                max_time = Convert.ToDouble(init_data.Get_Row1_X_Y(init_data.get_b()-1, 0));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Выбран неправильный файл");
            }

          
            Read_Info_pazient();
        }

        private async void button26_Click(object sender, EventArgs e)//Снимать данные(сек)
        {
            CancellationToken token = cancelTokenSource.Token;

            N_shift_axis = 0;
            progressBar1.Value = 0;

            Inizial();

            byte[] data = new byte[40];
            long ly1;
            long potok = 5;
            long potok2 = 12;
            long ckor = System.Convert.ToInt64(this.textBox3.Text) * (230400 / 400) * (10 + potok2 * potok / 3);
            //    progressBar1.Minimum = 0;
            int ckor2 = System.Convert.ToInt32(ckor / 1000);
            //  progressBar1.Maximum = ckor2;
            int Razmer1 = System.Convert.ToInt32(this.textBox11.Text) * 16750;
            int Axis_shift = 0;
            int[] base_sum = new int[Razmer1];
            int uhi = 0;
            progressBar1.Minimum = 0;
            //  progressBar1.Maximum = 10;
            progressBar1.Maximum = Convert.ToInt32(Convert.ToDouble(ckor2 * 1000 / Razmer1));
            progressBar1.Value = 0;

            SerialPort port = new SerialPort(textBox1.Text, 230400, Parity.None, 8, StopBits.One);
            StreamWriter sw = new StreamWriter("Результаты.txt");
            StreamWriter test = new StreamWriter("test.txt");
            port.ReadTimeout = 2000;
            port.Open();
            int scetchik_sbrosa = 0;

            if (token.IsCancellationRequested)
            {
                // richTextBox1.Text ="Операция прервана";

                return;
            }


            while (uhi < ckor)
            {
                uhi++;
                ly1 = port.ReadChar();

                test.Write(System.Convert.ToChar(ly1));

                base_sum[scetchik_sbrosa] = Convert.ToChar(ly1);
                scetchik_sbrosa++;
                if (scetchik_sbrosa % Razmer1 == 0)
                {
                    try
                    {
                        scetchik_sbrosa = 0;

                        await Task.Run(() => nepr_snjatie(ckor, potok2, base_sum, Axis_shift, Razmer1));
                      
                        uhi++;
                        Axis_shift++;
                        BeginInvoke(new ThreadStart(delegate { progressBar1.Value = Axis_shift; }));

                    }
                    catch
                    {
                        scetchik_sbrosa = 0;
                        uhi++;
                        Axis_shift++;
                        BeginInvoke(new ThreadStart(delegate { progressBar1.Value = Axis_shift; }));
                    }

                }

            }
            test.Close();
            port.Close();
            sw.Close();

          
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            int ekg = System.Convert.ToInt32(this.textBox2.Text);

            Initial_data init_data = new Initial_data("test.txt", reg, ekg);
            init_data.Row1_Shift_Time_To_0();//Сдвигаем время к 0
            init_data.Row1_Smothing();// Сглаживаем полученные данные
            init_data.Row1_Write_In_File("test3.txt");


            usergraph = new UseZedgraph(zedGraph1);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_4_Canal(init_data.get_row1(), init_data.get_b());//Строим график
            usergraph.Install_Pane("t, мc", "R, Ом", "Каналы");//Устанавливаем оси и загавие
            usergraph.ResetGraph();//Обновляем
            max_time = Convert.ToDouble(init_data.Get_Row1_X_Y(init_data.get_b() - 1, 0));


            Read_Info_pazient();

        }

        private void button22_Click(object sender, EventArgs e)//Вывод промежуточных данных
        {
            Inizial();
            N_shift_axis = 0;
            progressBar1.Value = 0;

            int reg = System.Convert.ToInt32(this.textBox13.Text);
            int ekg = System.Convert.ToInt32(this.textBox2.Text);

           
                Initial_data init_data = new Initial_data("test3.txt", reg, ekg);
                init_data.Row1_Shift_Time_To_0();//Сдвигаем время к 0
                init_data.Row1_Smothing();// Сглаживаем полученные данные
                init_data.Row2_Calculate();
                init_data.Row3_Average_Canal_Reg();
                init_data.Row4_Smoothing_Ekg();

            if (checkBox2.Checked)
            {
                init_data.Set_Row_In_Data_row1(
                   Calculate_Fast_Fourier_Signal_Filtration(init_data.get_row1(), init_data.get_b()), reg);
            }

            usergraph = new UseZedgraph(zedGraph1, init_data);
                usergraph.ClearAll();//Очищаем полотно
                usergraph.MakeGraph_On_Chosen_Canal();
                usergraph.Install_Pane("t, мc", "R, Ом", " ");//Устанавливаем оси и заглавие
                usergraph.ResetGraph();//Обновляем
                max_time = Convert.ToDouble(init_data.Get_Row1_X_Y(init_data.get_b() - 1, 0));



        }

        private void button21_Click(object sender, EventArgs e)//Рассчитать особые точки
        {
            Inizial();
            N_shift_axis = 0;
            progressBar1.Value = 0;

            int reg = System.Convert.ToInt32(this.textBox13.Text);
            int ekg = System.Convert.ToInt32(this.textBox2.Text);


            Initial_data init_data = new Initial_data("test3.txt", reg, ekg);
            init_data.Row1_Shift_Time_To_0();//Сдвигаем время к 0
            init_data.Row1_Smothing();// Сглаживаем полученные данные
            init_data.Row2_Calculate();
            init_data.Row3_Average_Canal_Reg();
            init_data.Row4_Smoothing_Ekg();

            if (checkBox2.Checked)
            {
                init_data.Set_Row_In_Data_row1(
                   Calculate_Fast_Fourier_Signal_Filtration(init_data.get_row1(), init_data.get_b()), reg);
            }

            init_data.Row1_2_Write_In_File();

            usergraph = new UseZedgraph(zedGraph1, init_data);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_On_Chosen_Canal();
           
            //Разделяем 
            Initial_processing.Divided_by_periods_data divided_row = new Initial_processing.Divided_by_periods_data(init_data, this.comboBox3.Text);
            divided_row.Calculate_Data_In_Period();         

            Special_point osob_point = new Special_point(divided_row, init_data);

            long[,] osob = null;
            if (radioButton8.Checked)
            {
                osob_point.return_osob_point(this.comboBox3.Text);
            }
            if (radioButton7.Checked)
            {
                osob_point.Return_Special_Point_Neural_Network();
            }
            if (radioButton4.Checked)
            {
                osob_point.Return_Special_Point_Statistic_Num(); 
            }

            if (checkBox1.Checked)
            {
                osob_point.Return_Point_EKG(this.comboBox3.Text);

                osob_point.Delete_Zero_From_Data();
            }
            osob = osob_point.get_spec_point();

            int arre = osob.Length;
            int ew = arre / 15;//счетчик найденных максимумов
         

            /////////////////////////
            /////////////////////////
            // новое
            //ЭКГ мах -     0
            //ЭКГ мах -х -  1
            // В1, В5 -     2
            // В1x, В5x -   3
            // В2 -         4
            // В2x -        5
            // В3 -         6
            // В3x -        7
            // В4 -         8  
            // В4x -        9
            //osob_10  -    Изначальная высота

            ////////////////////////

            long[,] osob_x = new long[5, ew];// список особых точек для вывода на график
            long[,] osob_y = new long[5, ew];

            for (int i = 0; i < ew - 1; i++)
            {
                osob_x[0, i] = osob[1, i];
                osob_y[0, i] = osob[0, i];

                osob_x[1, i] = osob[3, i];
                osob_y[1, i] = osob[2, i];

                osob_x[2, i] = osob[5, i];
                osob_y[2, i] = osob[4, i] + osob[10, i];

                osob_x[3, i] = osob[7, i];
                osob_y[3, i] = osob[6, i] + osob[10, i];

                osob_x[4, i] = osob[9, i];
                osob_y[4, i] = osob[8, i] + osob[10, i];

            }
            usergraph.MakeGraph_Special_Point(osob_x, osob_y, ew);
            usergraph.Install_Pane("t, мc", "R, Ом", " ");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
            max_time = Convert.ToDouble(init_data.Get_Row1_X_Y(init_data.get_b() - 1, 0));

                     
           
            Save_data.Save_Special_Point_For_Plotting(osob, ew);
            /////////////////////////////////////////

            osob = osob_point.Calculate_Shift_Special_Point(osob, ew);

            Save_data.Save_Special_Point_For_File(osob, ew);

        }       
        /////////////////////////////////
        //Гистограммы
        ///////////////////
        private void button8_Click(object sender, EventArgs e)//Гистограммы RR-интервала
        {
            Inizial();
           
            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b+1);
 
            int ew = b;
            //Считаем данные для гистограммы
            Gistogramma.Gistogramma_EKG gisto = new Gistogramma.Gistogramma_EKG(osob, ew);
            gisto.Set_Diffrence();
            gisto.Delete_Space_Diffrence(300000);
            gisto.Convert_Diffrence_2_3_To_Double();
            gisto.Find_Diffrence_Max_Min();
            gisto.Pilliars_Gisto(this.textBox5.Text);

            // Строим гистограмму
            usergraph = new UseZedgraph(zedGraph1, gisto);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Gistogramma("RR-интервалы, %");
            usergraph.Install_Pane("t, мc", "%", "");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
            //Считаем дополнительные данные
            Gistogramma.Data_gistogramm data_gisto = new Gistogramma.Data_gistogramm(gisto, true);
            data_gisto.Calculate_All_Parameters();
            data_gisto.Write_Result_EKG_In_File("Расчетные данные.txt", false);
            data_gisto.Write_Result_EKG_in_Richtextbox(richTextBox2);


        }

        private void button2_Click(object sender, EventArgs e)// Рассчитать гистограммы В1В1
        {
            Inizial();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b + 1);

            int ew = b;
            //Считаем данные для гистограммы
            Gistogramma.Gistogramma_B1B1 gisto = new Gistogramma.Gistogramma_B1B1(osob, ew);
            gisto.Set_Diffrence();
            gisto.Delete_Space_Diffrence(300000);
            gisto.Convert_Diffrence_2_3_To_Double();
            gisto.Find_Diffrence_Max_Min();
            gisto.Pilliars_Gisto(this.textBox5.Text);

            // Строим гистограмму
            usergraph = new UseZedgraph(zedGraph1, gisto);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Gistogramma("B1, %");
            usergraph.Install_Pane("t, мc", "%", " ");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
            //Считаем дополнительные данные
            Gistogramma.Data_gistogramm data_gisto = new Gistogramma.Data_gistogramm(gisto, true);
            data_gisto.Calculate_All_Parameters();
            data_gisto.Write_Result_EKG_In_File("Расчетные данные.txt", false);
            data_gisto.Write_Result_EKG_in_Richtextbox(richTextBox2);
        }

        private void button28_Click(object sender, EventArgs e)// Рассчитать гистограммы В2В2
        {
            Inizial();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b + 1);

            int ew = b;
            //Считаем данные для гистограммы
            Gistogramma.Gistogramma_B2B2 gisto = new Gistogramma.Gistogramma_B2B2(osob, ew);
            gisto.Set_Diffrence();
            gisto.Delete_Space_Diffrence(300000);
            gisto.Convert_Diffrence_2_3_To_Double();
            gisto.Find_Diffrence_Max_Min();
            gisto.Pilliars_Gisto(this.textBox5.Text);

            // Строим гистограмму
            usergraph = new UseZedgraph(zedGraph1, gisto);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Gistogramma("B2, %");
            usergraph.Install_Pane("%", "%", "В2");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
            //Считаем дополнительные данные
            Gistogramma.Data_gistogramm data_gisto = new Gistogramma.Data_gistogramm(gisto, true);
            data_gisto.Calculate_All_Parameters();
            data_gisto.Write_Result_EKG_In_File("Расчетные данные.txt", false);
            data_gisto.Write_Result_EKG_in_Richtextbox(richTextBox2);
        }

        private void button23_Click(object sender, EventArgs e)//Гистограммы ВРПР
        {
            Inizial();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;
            //Считаем данные для гистограммы
            Gistogramma.Gistogramma_VRPR gisto = new Gistogramma.Gistogramma_VRPR(osob, ew);
            gisto.Set_Diffrence();
            gisto.Delete_Space_Diffrence(10000);
            gisto.Convert_Diffrence_2_3_To_Double();
            gisto.Find_Diffrence_Max_Min();
            gisto.Pilliars_Gisto(this.textBox5.Text);

            // Строим гистограмму
            usergraph = new UseZedgraph(zedGraph1, gisto);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Gistogramma("ВРПР, %");
            usergraph.Install_Pane("%", "%", " ");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
            //Считаем дополнительные данные
            Gistogramma.Data_gistogramm data_gisto = new Gistogramma.Data_gistogramm(gisto, true);
            data_gisto.Calculate_All_Parameters();
            data_gisto.Write_Result_ALL_In_File("Расчетные данные.txt", false, "ВРПР");
            data_gisto.Write_Result_ALL_in_Richtextbox(richTextBox2);
        }

        private void button13_Click(object sender, EventArgs e)//Гистограммы анакроты
        {
            Inizial();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;
            //Считаем данные для гистограммы
            Gistogramma.Gistogramma_Ana gisto = new Gistogramma.Gistogramma_Ana(osob, ew);
            gisto.Set_Diffrence();
            gisto.Delete_Space_Diffrence(1);
            gisto.Convert_Diffrence_2_3_To_Double();
            gisto.Find_Diffrence_Max_Min();
            gisto.shift_diffrence();
            gisto.Pilliars_Gisto(this.textBox14.Text);

            // Строим гистограмму
            usergraph = new UseZedgraph(zedGraph1, gisto);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Gistogramma_Proz("Анакрота, %");
            usergraph.Install_Pane("%", "%", "");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
            //Считаем дополнительные данные
            Gistogramma.Data_gistogramm data_gisto = new Gistogramma.Data_gistogramm(gisto, false);
            data_gisto.Calculate_All_Parameters();
            data_gisto.Write_Result_ALL_In_File("Расчетные данные.txt", false, " ");
            data_gisto.Write_Result_ALL_in_Richtextbox(richTextBox2);
        }

        private void button14_Click(object sender, EventArgs e)//Гистограммы Дикротического индекса
        {
            Inizial();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;
            //Считаем данные для гистограммы
            Gistogramma.Gistogramma_IDV gisto = new Gistogramma.Gistogramma_IDV(osob, ew);
            gisto.Set_Diffrence();
            gisto.Delete_Space_Diffrence(1);
            gisto.Convert_Diffrence_2_3_To_Double();
            gisto.Find_Diffrence_Max_Min();
            gisto.shift_diffrence();
            gisto.Pilliars_Gisto(this.textBox14.Text);

            // Строим гистограмму
            usergraph = new UseZedgraph(zedGraph1, gisto);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Gistogramma_Proz("Дикротический индекс, %");
            usergraph.Install_Pane("%", "%", " ");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
            //Считаем дополнительные данные
            Gistogramma.Data_gistogramm data_gisto = new Gistogramma.Data_gistogramm(gisto, false);
            data_gisto.Calculate_All_Parameters();
            data_gisto.Write_Result_ALL_In_File("Расчетные данные.txt", false, "Дикротический индекс");
            data_gisto.Write_Result_ALL_in_Richtextbox(richTextBox2);
        }

        private void button15_Click(object sender, EventArgs e)//Гистограммы Индекса отражения
        {
            Inizial();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;
            //Считаем данные для гистограммы
            Gistogramma.Gistogramma_IO gisto = new Gistogramma.Gistogramma_IO(osob, ew);
            gisto.Set_Diffrence();
            gisto.Delete_Space_Diffrence(1);
            gisto.Convert_Diffrence_2_3_To_Double();
            gisto.Find_Diffrence_Max_Min();
            gisto.shift_diffrence();
            gisto.Pilliars_Gisto(this.textBox14.Text);

            // Строим гистограмму
            usergraph = new UseZedgraph(zedGraph1, gisto);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Gistogramma_Proz("Индекс отражения, %");
            usergraph.Install_Pane("%", "%", " ");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
            //Считаем дополнительные данные
            Gistogramma.Data_gistogramm data_gisto = new Gistogramma.Data_gistogramm(gisto, false);
            data_gisto.Calculate_All_Parameters();
            data_gisto.Write_Result_ALL_In_File("Расчетные данные.txt", false, " ");
            data_gisto.Write_Result_ALL_in_Richtextbox(richTextBox2);
        }

        private void button16_Click(object sender, EventArgs e)////Гистограммы индекса жесткости
        {
            Inizial();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;
            //Считаем данные для гистограммы
            Gistogramma.Gistogramma_IJ gisto = new Gistogramma.Gistogramma_IJ(osob, ew);
            gisto.set_diffrence(this.textBox7.Text);
            gisto.Delete_Space_Diffrence(1);
            gisto.Convert_Diffrence_2_3_To_Double();
            gisto.Find_Diffrence_Max_Min();
            gisto.Pilliars_Gisto(this.textBox16.Text);

            // Строим гистограмму
            usergraph = new UseZedgraph(zedGraph1, gisto);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Gistogramma_Proz("Индекс жесткости, %");
            usergraph.Install_Pane("%", "%", " ");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
            //Считаем дополнительные данные
            Gistogramma.Data_gistogramm data_gisto = new Gistogramma.Data_gistogramm(gisto, false);
            data_gisto.Calculate_All_Parameters();
            data_gisto.Write_Result_ALL_In_File("Расчетные данные.txt", false, " ");
            data_gisto.Write_Result_ALL_in_Richtextbox(richTextBox2);

        }

        private void button17_Click(object sender, EventArgs e)//Гистограммы индекса восходящей волны
        {

            Inizial();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;
            //Считаем данные для гистограммы
            Gistogramma.Gistogramma_IVV gisto = new Gistogramma.Gistogramma_IVV(osob, ew);
            gisto.Set_Diffrence();
            gisto.Delete_Space_Diffrence(1);
            gisto.Convert_Diffrence_2_3_To_Double();
            gisto.Find_Diffrence_Max_Min();
            gisto.shift_diffrence();
            gisto.Pilliars_Gisto(this.textBox15.Text);

            // Строим гистограмму
            usergraph = new UseZedgraph(zedGraph1, gisto);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Gistogramma_Proz("Индекс восходящей волны, %");
            usergraph.Install_Pane("%", "%", " ");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
            //Считаем дополнительные данные
            Gistogramma.Data_gistogramm data_gisto = new Gistogramma.Data_gistogramm(gisto, false);
            data_gisto.Calculate_All_Parameters();
            data_gisto.Write_Result_ALL_In_File("Расчетные данные.txt", false, " ");
            data_gisto.Write_Result_ALL_in_Richtextbox(richTextBox2);
        }
        /////////////////////////////////
        //Спектры
        ///////////////////

        private void button9_Click(object sender, EventArgs e)//Рассчитать спектр ЭКГ
        {
            Inizial();
            inizialize_radbutton_spectr();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;

            Spectr.Spectr_EKG spectr = new Spectr.Spectr_EKG(osob, b, radbutton_spectr);

            spectr.Set_Diffrence();
            spectr.Delete_Zero_Diffrence();
            spectr.Delete_Zero_Value();
            spectr.Set_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr_Pow();
            spectr.Get_Data_From_Richtextbox(richTextBox3, radbutton);
            spectr.Write_In_File();
            spectr.Spectr_Out_Text("спектр ЭКГ.txt", richTextBox2, false, "Спектр ЭКГ");


            usergraph = new UseZedgraph(zedGraph1, spectr);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Spectr("спектр ЭКГ", Convert.ToInt32(this.textBox17.Text));
           
            usergraph.Install_Pane("W, Гц", "P, Вт", "Данные");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем


        }

        private void button27_Click(object sender, EventArgs e)//Рассчитать Спектр В1В1
        {
            Inizial();
            inizialize_radbutton_spectr();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;

            Spectr.Spectr_B1B1 spectr = new Spectr.Spectr_B1B1(osob, b, radbutton_spectr);

            spectr.Set_Diffrence();
            spectr.Delete_Zero_Diffrence();
            spectr.Delete_Zero_Value();
            spectr.Set_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr_Pow();
            spectr.Get_Data_From_Richtextbox(richTextBox3, radbutton);
            spectr.Write_In_File();
            spectr.Spectr_Out_Text("спектр B1B1.txt", richTextBox2, false, "Спектр В1В1");


            usergraph = new UseZedgraph(zedGraph1, spectr);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Spectr("спектр B1B1", Convert.ToInt32(this.textBox17.Text));

            usergraph.Install_Pane("W, Гц", "P, Вт", "Данные");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
        }

        private void button29_Click(object sender, EventArgs e)//Рассчитать Спектр В2В2
        {
            Inizial();
            inizialize_radbutton_spectr();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;

            Spectr.Spectr_B2B2 spectr = new Spectr.Spectr_B2B2(osob, b, radbutton_spectr);

            spectr.Set_Diffrence();
            spectr.Delete_Zero_Diffrence();
            spectr.Delete_Zero_Value();
            spectr.Set_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr_Pow();
            spectr.Get_Data_From_Richtextbox(richTextBox3, radbutton);
            spectr.Write_In_File();
            spectr.Spectr_Out_Text("спектр B2B2.txt", richTextBox2, false, "Спектр В2В2");


            usergraph = new UseZedgraph(zedGraph1, spectr);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Spectr("спектр B2B2", Convert.ToInt32(this.textBox17.Text));

            usergraph.Install_Pane("W, Гц", "P, Вт", "Данные");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
        }


        private void button24_Click(object sender, EventArgs e)//Рассчитать спектр ВРПР
        {
            Inizial();
            inizialize_radbutton_spectr();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;

            Spectr.Spectr_VRPR spectr = new Spectr.Spectr_VRPR(osob, b, radbutton_spectr);

            spectr.Set_Diffrence();
            spectr.Delete_Zero_Diffrence();
           spectr.Set_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr_Pow();
            spectr.Get_Data_From_Richtextbox(richTextBox3, radbutton);
            spectr.Write_In_File();
            spectr.Spectr_Out_Text("спектр ВРПР.txt", richTextBox2, false, "Спектр ВРПР");


            usergraph = new UseZedgraph(zedGraph1, spectr);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Spectr("спектр ВРПР", Convert.ToInt32(this.textBox17.Text));

            usergraph.Install_Pane("W, Гц", "P, Вт", "Данные");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
        }

        private void button10_Click(object sender, EventArgs e)//Рассчитать спектр Аан
        {
            Inizial();
            inizialize_radbutton_spectr();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;

            Spectr.Spectr_Aan spectr = new Spectr.Spectr_Aan(osob, b, radbutton_spectr);

            spectr.Set_Diffrence();
            spectr.Delete_Zero_Diffrence();
            spectr.Set_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr_Pow();
            spectr.Get_Data_From_Richtextbox(richTextBox3, radbutton);
            spectr.Write_In_File();
            spectr.Spectr_Out_Text("спектр Аан.txt", richTextBox2, false, "Спектр Анакроты");


            usergraph = new UseZedgraph(zedGraph1, spectr);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Spectr("спектр Аан", Convert.ToInt32(this.textBox17.Text));

            usergraph.Install_Pane("W, Гц", "P, Вт", "Данные");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
        }

        private void button11_Click(object sender, EventArgs e)//Рассчитать спектр ИДВ
        {
            Inizial();
            inizialize_radbutton_spectr();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;

            Spectr.Spectr_IDV spectr = new Spectr.Spectr_IDV(osob, b, radbutton_spectr);

            spectr.Set_Diffrence();
            spectr.Delete_Zero_Diffrence();
            spectr.Set_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr_Pow();
            spectr.Get_Data_From_Richtextbox(richTextBox3, radbutton);
            spectr.Write_In_File();
            spectr.Spectr_Out_Text("спектр ИДВ.txt", richTextBox2, false, "Спектр Индекса дикротической волны");


            usergraph = new UseZedgraph(zedGraph1, spectr);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Spectr("спектр ИДВ", Convert.ToInt32(this.textBox17.Text));

            usergraph.Install_Pane("W, Гц", "P, Вт", "Данные");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
        }

        private void button12_Click(object sender, EventArgs e)//Рассчитать спектр ИО
        {
            Inizial();
            inizialize_radbutton_spectr();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;

            Spectr.Spectr_IO spectr = new Spectr.Spectr_IO(osob, b, radbutton_spectr);

            spectr.Set_Diffrence();
            spectr.Delete_Zero_Diffrence();
            spectr.Set_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr_Pow();
            spectr.Get_Data_From_Richtextbox(richTextBox3, radbutton);
            spectr.Write_In_File();
            spectr.Spectr_Out_Text("спектр ИО.txt", richTextBox2, false, "Спектр Индекса отражения");


            usergraph = new UseZedgraph(zedGraph1, spectr);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Spectr("спектр ИО", Convert.ToInt32(this.textBox17.Text));

            usergraph.Install_Pane("W, Гц", "P, Вт", "Данные");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
        }

        private void button18_Click(object sender, EventArgs e)//Рассчитать спектр ИЖ
        {
            Inizial();
            inizialize_radbutton_spectr();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;

            Spectr.Spectr_IJ spectr = new Spectr.Spectr_IJ(osob, b, radbutton_spectr);

            spectr.set_diffrence(this.textBox7.Text);
            spectr.Delete_Zero_Diffrence();
            spectr.Set_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr_Pow();
            spectr.Get_Data_From_Richtextbox(richTextBox3, radbutton);
            spectr.Write_In_File();
            spectr.Spectr_Out_Text("спектр ИЖ.txt", richTextBox2, false, "Спектр Индекса жесткости");


            usergraph = new UseZedgraph(zedGraph1, spectr);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Spectr("спектр ИЖ", Convert.ToInt32(this.textBox17.Text));

            usergraph.Install_Pane("W, Гц", "P, Вт", "Данные");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
        }

        private void button20_Click(object sender, EventArgs e)//Рассчитать спектр ИВВ
        {
            Inizial();
            inizialize_radbutton_spectr();

            richTextBox2.Clear();
            richTextBox3.Clear();

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;

            Spectr.Spectr_IVV spectr = new Spectr.Spectr_IVV(osob, b, radbutton_spectr);

            spectr.Set_Diffrence();
            spectr.Delete_Zero_Diffrence();
            spectr.Set_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr_Pow();
            spectr.Get_Data_From_Richtextbox(richTextBox3, radbutton);
            spectr.Write_In_File();
            spectr.Spectr_Out_Text("спектр ИВВ.txt", richTextBox2, false, "Спектр Индекса восходящей волны");


            usergraph = new UseZedgraph(zedGraph1, spectr);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Spectr("спектр ИВВ", Convert.ToInt32(this.textBox17.Text));

            usergraph.Install_Pane("W, Гц", "P, Вт", "Данные");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем
        }
       
        /// ///////////////////////////////////////
        
        private void button25_Click(object sender, EventArgs e)//Рассчитать и сбросить в файл все
        {
            Inizial();
            inizialize_radbutton_spectr();

            richTextBox2.Clear();
            richTextBox3.Clear();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 5;
            progressBar1.Value = 0;

            int ekg = System.Convert.ToInt32(this.textBox2.Text);
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            // считываем особые точки из файла
            Initial_processing.Reader_data reader_data = new Initial_processing.Reader_data("Особые точки чистые.txt");
            int b = reader_data.Return_Read_String();
            long[,] osob = reader_data.Return_Read_Array_Special_Points(b);

            int ew = b;

            double[] r0;
            double[] r1;

            Gistogramma.Gistogramma_B2B2 gisto8 = new Gistogramma.Gistogramma_B2B2(osob, ew);
            gisto8.Set_Diffrence();
            gisto8.Delete_Space_Diffrence(300000);
            gisto8.Convert_Diffrence_2_3_To_Double();
            gisto8.Find_Diffrence_Max_Min();
            gisto8.Pilliars_Gisto(this.textBox5.Text);
                      
            double[] r8 = gisto8.Get_Diffrence_3();

            
                r0 = new double[r8.Length];
                r1 = new double[r8.Length];
           
           Gistogramma.Gistogramma_EKG gisto = new Gistogramma.Gistogramma_EKG(osob, ew);
            if (checkBox1.Checked)
            {

            
            gisto.Set_Diffrence();
            gisto.Delete_Space_Diffrence(300000);
            gisto.Convert_Diffrence_2_3_To_Double();
            gisto.Find_Diffrence_Max_Min();
            gisto.Pilliars_Gisto(this.textBox5.Text);

            r0 = gisto.Get_Diffrence();
            r1 = gisto.Get_Diffrence_3();
            }

            Gistogramma.Gistogramma_VRPR gisto2 = new Gistogramma.Gistogramma_VRPR(osob, ew);
            gisto2.Set_Diffrence();
            gisto2.Delete_Space_Diffrence(300000);
            gisto2.Convert_Diffrence_2_3_To_Double();
            gisto2.Find_Diffrence_Max_Min();
            gisto2.Pilliars_Gisto(this.textBox5.Text);

            double[] r2 = gisto2.Get_Diffrence_3();

            //Считаем данные для гистограммы
            Gistogramma.Gistogramma_Ana gisto3 = new Gistogramma.Gistogramma_Ana(osob, ew);
            gisto3.Set_Diffrence();
            gisto3.Delete_Space_Diffrence(1);
            gisto3.Convert_Diffrence_2_3_To_Double();
            gisto3.Find_Diffrence_Max_Min();
            gisto3.shift_diffrence();
            gisto3.Pilliars_Gisto(this.textBox14.Text);

            double[] r3 = gisto3.Get_Diffrence_3();

            Gistogramma.Gistogramma_IDV gisto4 = new Gistogramma.Gistogramma_IDV(osob, ew);
            gisto4.Set_Diffrence();
            gisto4.Delete_Space_Diffrence(1);
            gisto4.Convert_Diffrence_2_3_To_Double();
            gisto4.Find_Diffrence_Max_Min();
            gisto4.shift_diffrence();
            gisto4.Pilliars_Gisto(this.textBox14.Text);

            double[] r4 = gisto4.Get_Diffrence_3();

            Gistogramma.Gistogramma_IO gisto5 = new Gistogramma.Gistogramma_IO(osob, ew);
            gisto5.Set_Diffrence();
            gisto5.Delete_Space_Diffrence(1);
            gisto5.Convert_Diffrence_2_3_To_Double();
            gisto5.Find_Diffrence_Max_Min();
            gisto5.shift_diffrence();
            gisto5.Pilliars_Gisto(this.textBox14.Text);

            double[] r5 = gisto5.Get_Diffrence_3();

            Gistogramma.Gistogramma_IJ gisto6 = new Gistogramma.Gistogramma_IJ(osob, ew);
            gisto6.set_diffrence(this.textBox7.Text);
            gisto6.Delete_Space_Diffrence(1);
            gisto6.Convert_Diffrence_2_3_To_Double();
            gisto6.Find_Diffrence_Max_Min();
            gisto6.Pilliars_Gisto(this.textBox16.Text);

            double[] r6 = gisto6.Get_Diffrence_3();

            Gistogramma.Gistogramma_IVV gisto7 = new Gistogramma.Gistogramma_IVV(osob, ew);
            gisto7.Set_Diffrence();
            gisto7.Delete_Space_Diffrence(1);
            gisto7.Convert_Diffrence_2_3_To_Double();
            gisto7.Find_Diffrence_Max_Min();
            gisto7.shift_diffrence();
            gisto7.Pilliars_Gisto(this.textBox15.Text);
            progressBar1.Value = 1;

            double[] r7 = gisto7.Get_Diffrence_3();

            string[] Gistogramma_text = new string[8]{ "RR-интервал", "Время распространения пульсовой волны",
                "Анакрота","Дикротический индекс","Индекс отражения", "Индекс жесткости", "Индекс восходящей волны", "B2B2-интервал"};

        
            StreamWriter GIST_point = new StreamWriter("Точки гистограмм.txt");
        
            //Выводим в файл точки гистограмм
            GIST_point.WriteLine("i" + "\t" + "ВРПР" + "\t" + "ЭКГ" + "\t" + "ВРПВ" + "\t" + "Аан" + "\t" + "ДИ" +
                "\t" + "ИО" + "\t" + "ИЖ" + "\t" + "ИВВ" + "\t" + "В2В2" + "\n");

            for (int i = 0; i < r2.Length; i++)
            {
                GIST_point.WriteLine(i + "\t" + Math.Round(r0[i], 4) + "\t" + Math.Round(r1[i], 4) + "\t" +
                      Math.Round(r2[i], 4) + "\t" + Math.Round(r3[i], 4) + "\t" + Math.Round(r4[i], 4) + "\t" +
                       Math.Round(r5[i], 4) + "\t" + Math.Round(r6[i], 4) + "\t" + Math.Round(r7[i], 4)
                       + "\t" + Math.Round(r8[i], 4));
            }

            GIST_point.Close();

            progressBar1.Value = 2;

            if (checkBox1.Checked)
            {
            Gistogramma.Data_gistogramm data_gisto = new Gistogramma.Data_gistogramm(gisto, true);
            data_gisto.Calculate_All_Parameters();
            data_gisto.Write_Result_EKG_In_File("Расчетные данные гистограмм.txt", false);
            }           

            Gistogramma.Data_gistogramm data_gisto2 = new Gistogramma.Data_gistogramm(gisto2, true);
            data_gisto2.Calculate_All_Parameters();
            data_gisto2.Write_Result_ALL_In_File("Расчетные данные гистограмм.txt", true, Gistogramma_text[1]);

            Gistogramma.Data_gistogramm data_gisto3 = new Gistogramma.Data_gistogramm(gisto3, false);
            data_gisto3.Calculate_All_Parameters();
            data_gisto3.Write_Result_ALL_In_File("Расчетные данные гистограмм.txt", true, Gistogramma_text[2]);

            Gistogramma.Data_gistogramm data_gisto4 = new Gistogramma.Data_gistogramm(gisto4, false);
            data_gisto4.Calculate_All_Parameters();
            data_gisto4.Write_Result_ALL_In_File("Расчетные данные гистограмм.txt", true, Gistogramma_text[3]);

            Gistogramma.Data_gistogramm data_gisto5 = new Gistogramma.Data_gistogramm(gisto5, false);
            data_gisto5.Calculate_All_Parameters();
            data_gisto5.Write_Result_ALL_In_File("Расчетные данные гистограмм.txt", true, Gistogramma_text[4]);

            Gistogramma.Data_gistogramm data_gisto6 = new Gistogramma.Data_gistogramm(gisto6, false);
            data_gisto6.Calculate_All_Parameters();
            data_gisto6.Write_Result_ALL_In_File("Расчетные данные гистограмм.txt", true, Gistogramma_text[5]);

            Gistogramma.Data_gistogramm data_gisto7 = new Gistogramma.Data_gistogramm(gisto7, false);
            data_gisto7.Calculate_All_Parameters();
            data_gisto7.Write_Result_ALL_In_File("Расчетные данные гистограмм.txt", true, Gistogramma_text[6]);

            Gistogramma.Data_gistogramm data_gisto8 = new Gistogramma.Data_gistogramm(gisto8, false);
            data_gisto8.Calculate_All_Parameters();
            data_gisto8.Write_Result_ALL_In_File("Расчетные данные гистограмм.txt", true, Gistogramma_text[7]);


            progressBar1.Value = 3;

          

            Spectr.Spectr_B2B2 spectr8 = new Spectr.Spectr_B2B2(osob, b, radbutton_spectr);

            spectr8.Set_Diffrence();
            spectr8.Delete_Zero_Diffrence();
            spectr8.Delete_Zero_Value();
            spectr8.Set_Amplitude_Spectr();
            spectr8.Calculate_Amplitude_Spectr();
            spectr8.Calculate_Amplitude_Spectr_Pow();
           // spectr8.Spectr_out_text("Расчетные данные спектра.txt", richTextBox2, true, "Спектр B2B2");

            double[] s8 = spectr8.Get_Amplitude_Spectr_Pow();
            double[] s08 = new double[5000];
            if (s8.Length == 0)
            {
                s8 = s08;
            }

            double[] s1 = new double[5000];
            Spectr.Spectr_EKG spectr = new Spectr.Spectr_EKG(osob, b, radbutton_spectr);

            if (checkBox1.Checked)
            {

            spectr.Set_Diffrence();
            spectr.Delete_Zero_Diffrence();
            spectr.Delete_Zero_Value();
            spectr.Set_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr();
            spectr.Calculate_Amplitude_Spectr_Pow();
         //   spectr.Spectr_out_text("Расчетные данные спектра.txt", richTextBox2, false, "Спектр ЭКГ");
            s1 = spectr.Get_Amplitude_Spectr_Pow();
            }  
           
            
            if (s1.Length==0 || s1 == null) {
                double[] s01 = new double[s8.Length];
                s1 = s01;
            }
            Spectr.Spectr_VRPR spectr2 = new Spectr.Spectr_VRPR(osob, b, radbutton_spectr);

            spectr2.Set_Diffrence();
            spectr2.Delete_Zero_Diffrence();
            spectr2.Set_Amplitude_Spectr();
            spectr2.Calculate_Amplitude_Spectr();
            spectr2.Calculate_Amplitude_Spectr_Pow();
          //  spectr2.Spectr_out_text("Расчетные данные спектра.txt", richTextBox2, true, "Спектр ВРПР");

            double[] s2 = spectr2.Get_Amplitude_Spectr_Pow();

            Spectr.Spectr_Aan spectr3 = new Spectr.Spectr_Aan(osob, b, radbutton_spectr);

            spectr3.Set_Diffrence();
            spectr3.Delete_Zero_Diffrence();
            spectr3.Set_Amplitude_Spectr();
            spectr3.Calculate_Amplitude_Spectr();
            spectr3.Calculate_Amplitude_Spectr_Pow();
         
           // spectr3.Spectr_out_text("Расчетные данные спектра.txt", richTextBox2, true, "Спектр Анакроты");

            double[] s3 = spectr3.Get_Amplitude_Spectr_Pow();


            Spectr.Spectr_IDV spectr4 = new Spectr.Spectr_IDV(osob, b, radbutton_spectr);

            spectr4.Set_Diffrence();
            spectr4.Delete_Zero_Diffrence();
            spectr4.Set_Amplitude_Spectr();
            spectr4.Calculate_Amplitude_Spectr();
            spectr4.Calculate_Amplitude_Spectr_Pow();
           // spectr4.Spectr_out_text("Расчетные данные спектра.txt", richTextBox2, true, "Спектр Дикротического индекса");

            double[] s4 = spectr4.Get_Amplitude_Spectr_Pow();

            Spectr.Spectr_IO spectr5 = new Spectr.Spectr_IO(osob, b, radbutton_spectr);

            spectr5.Set_Diffrence();
            spectr5.Delete_Zero_Diffrence();
            spectr5.Set_Amplitude_Spectr();
            spectr5.Calculate_Amplitude_Spectr();
            spectr5.Calculate_Amplitude_Spectr_Pow();
          
         //   spectr5.Spectr_out_text("Расчетные данные спектра.txt", richTextBox2, true, "Спектр Индекса отражения");

            double[] s5 = spectr5.Get_Amplitude_Spectr_Pow();


            Spectr.Spectr_IJ spectr6 = new Spectr.Spectr_IJ(osob, b, radbutton_spectr);

            spectr6.set_diffrence(this.textBox7.Text);
            spectr6.Delete_Zero_Diffrence();
            spectr6.Set_Amplitude_Spectr();
            spectr6.Calculate_Amplitude_Spectr();
            spectr6.Calculate_Amplitude_Spectr_Pow();
         //   spectr6.Spectr_out_text("Расчетные данные спектра.txt", richTextBox2, true, "Спектр индекса жесткости");

            double[] s6 = spectr6.Get_Amplitude_Spectr_Pow();

            Spectr.Spectr_IVV spectr7 = new Spectr.Spectr_IVV(osob, b, radbutton_spectr);

            spectr7.Set_Diffrence();
            spectr7.Delete_Zero_Diffrence();
            spectr7.Set_Amplitude_Spectr();
            spectr7.Calculate_Amplitude_Spectr();
            spectr7.Calculate_Amplitude_Spectr_Pow();
          //  spectr7.Spectr_out_text("Расчетные данные спектра.txt", richTextBox2, true, "Спектр Индекса восходящей волны");

            double[] s7 = spectr7.Get_Amplitude_Spectr_Pow();

            double w = spectr7.Get_DW();

            progressBar1.Value = 4;

            richTextBox2.Clear();

            // double N_point_spectr = System.Convert.ToDouble(this.textBox17.Text);
            int min_length_spectr = s8.Length;

            if (min_length_spectr>s1.Length)
            {
                min_length_spectr = s1.Length;
            }
            if (min_length_spectr > s2.Length)
            {
                min_length_spectr = s2.Length;
            }
            if (min_length_spectr > s3.Length)
            {
                min_length_spectr = s3.Length;
            }
            if (min_length_spectr > s4.Length)
            {
                min_length_spectr = s4.Length;
            }
            if (min_length_spectr > s5.Length)
            {
                min_length_spectr = s5.Length;
            }
            if (min_length_spectr > s6.Length)
            {
                min_length_spectr = s6.Length;
            }
            if (min_length_spectr > s7.Length)
            {
                min_length_spectr = s7.Length;
            }


            int N_point_spectr = min_length_spectr;

            StreamWriter SPECTR_point = new StreamWriter("Точки спектра.txt");

            SPECTR_point.WriteLine("Частота, Гц" + "\t" + "Спектр ЭКГ"
                  + "\t" + "Спектр ВРПВ" + "\t" + "Спектр Аан"
                  + "\t" + "Спектр ИДВ" + "\t" + "Спектр ИО"
                  + "\t" + "Спектр ИЖ" + "\t" + "Спектр ИВВ" + "\t" + "Спектр B2B2" + "\n");

            for (int i = 0; i < N_point_spectr; i++)
            {
                SPECTR_point.WriteLine(Math.Round(w * (i + 1)/(2*PI_), 3) + "\t" + Math.Round(s1[i], 3)
                    + "\t" + Math.Round(s2[i], 3) + "\t" + Math.Round(s3[i], 3)
                    + "\t" + Math.Round(s4[i], 3) + "\t" + Math.Round(s5[i], 3)
                    + "\t" + Math.Round(s6[i], 3) + "\t" + Math.Round(s7[i], 3)
                    + "\t" + Math.Round(s8[i], 3));
            }

            SPECTR_point.Close();

            progressBar1.Value = 5;


        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                radbutton = 0;

            if (radioButton2.Checked)
                radbutton = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        ////////////////////////////////////////Разное
        public void nepr_snjatie(long ck, long pot, int[] basa, int Axis, int Razmer)
        {
            StringBuilder buffer = new StringBuilder();
            int a1 = 0;
            int b1 = 0;//счетчик строк
            string n11;

            int l11;
            int j1 = 0;// счетчик строк 10

            int k1 = 0;//счетчик столбцов 2

            int m1 = 0;//смещение буффера

            long[,] row11 = new long[ck, 1 + pot];

            for (int s = 0; s <= pot; s++)// s потоков+время
            {
                for (long r = 0; r < ck; r++)
                {
                    row11[r, s] = 0;
                }
            }

            int[] base_sum = new int[Razmer + 1]; // надеюсь хватит

            for (int r = 0; r < Razmer; r++)
            {
                base_sum[r] = basa[r];
            }

            int bas_s = Razmer - 10;
            int Axis_shift = Axis;

            for (int yu = 1; yu < bas_s; yu++)
            {
                l11 = base_sum[yu];


                if (l11 == 13)
                {
                    try
                    {
                        n11 = buffer.ToString(); // пищем цифру в строку
                        buffer.Remove(0, n11.Length); //очищаем буффер
                                                      // rw2.WriteLine(n1);
                        row11[j1, k1] = System.Convert.ToInt64(n11);// пишем в массив
                        j1++; // переходим на следующую строку
                        k1 = 0; // переходим на первый столбец
                        m1 = 0;
                        b1++;
                    }
                    catch
                    {
                        j1++; // переходим на следующую строку
                        k1 = 0; // переходим на первый столбец
                        m1 = 0;
                        b1++;
                    }
                }
                if (l11 == 9)
                {
                    try
                    {
                        n11 = buffer.ToString(); // пищем цифру в строку
                        buffer.Remove(0, n11.Length); //очищаем буффер
                        row11[j1, k1] = System.Convert.ToInt64(n11);// пишем в массив
                        k1++; // переходим на следующий столбец
                        m1 = 0;
                    }
                    catch (FormatException iskl)
                    {

                        k1 = 0; // переходим на первый столбец
                        m1 = 0;
                        j1++;
                        b1++;
                    }
                }
                if (l11 == 48 || l11 == 49 || l11 == 50 || l11 == 51 || l11 == 52 || l11 == 53 || l11 == 54 || l11 == 55 || l11 == 56 || l11 == 57)
                {
                    buffer.Insert(m1, System.Convert.ToChar(l11)); // пишем символ
                    m1++;
                }
                else
                {
                    a1++;
                }
            }
            /////////////////////
            for (j1 = 3; j1 < b1; j1++)
            {
                row11[j1, 0] = row11[j1, 0] - row11[2, 0];
            }


            // строим график
            // Получим панель для рисования
            GraphPane pane2 = zedGraph1.GraphPane;
            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane2.CurveList.Clear();

            // Создадим список точек для кривой f1(x)
            PointPairList f1_list2 = new PointPairList();

            // Создадим список точек для кривой f2(x)
            PointPairList f2_list2 = new PointPairList();
            // Создадим список точек для кривой f3(x)
            PointPairList f3_list2 = new PointPairList();
            // Создадим список точек для кривой f3(x)
            PointPairList f4_list2 = new PointPairList();

            // Заполним массив точек для кривой f1-3(x)
            for (int y = 3; y < b1; y++)
            {
                f1_list2.Add(row11[y, 0] / 1000 + Axis_shift * row11[b1 - 1, 0] / 1000, row11[y, 1]);
                f2_list2.Add(row11[y, 0] / 1000 + Axis_shift * row11[b1 - 1, 0] / 1000, row11[y, 2]);
                f3_list2.Add(row11[y, 0] / 1000 + Axis_shift * row11[b1 - 1, 0] / 1000, row11[y, 3]);
                f4_list2.Add(row11[y, 0] / 1000 + Axis_shift * row11[b1 - 1, 0] / 1000, row11[y, 4]);

            }

            pane2.XAxis.Title.Text = "t, мc";
            pane2.YAxis.Title.Text = "R, Ом";
            pane2.Title.Text = "Данные";
            // !!!
            // Создадим кривую с названием "Sinc", 
            // которая будет рисоваться голубым цветом (Color.Blue),
            // Опорные точки выделяться не будут (SymbolType.None)
            LineItem f1_curve2 = pane2.AddCurve("Канал 1", f1_list2, Color.Blue, SymbolType.None);

            LineItem f2_curve2 = pane2.AddCurve(" Канал 2", f2_list2, Color.Red, SymbolType.None);
            LineItem f3_curve2 = pane2.AddCurve("Канал 3", f3_list2, Color.Green, SymbolType.None);
            LineItem f4_curve2 = pane2.AddCurve("Канал 4", f4_list2, Color.Black, SymbolType.None);


            pane2.XAxis.Scale.Min = row11[3, 0] / 1000 + Axis_shift * row11[b1 - 1, 0] / 1000;
            pane2.XAxis.Scale.Max = (1 + Axis_shift) * row11[b1 - 1, 0] / 1000;

         
            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph1.AxisChange();

            // Обновляем график
            zedGraph1.Invalidate();

            // обнуляем счетчики
            j1 = 0; // переходим на следующую строку
            k1 = 0; // переходим на первый столбец
            m1 = 0;
            b1 = 0;

        }// Блок снятия каждые неск секунд
               

        private void button30_Click(object sender, EventArgs e)//Быстрое фурье
        {
            Clear_list_zed();
            Inizial();

            int reg = System.Convert.ToInt32(this.textBox13.Text);
            int ekg = System.Convert.ToInt32(this.textBox2.Text);


            Initial_data init_data = new Initial_data("test3.txt", reg, ekg);
            init_data.Row1_Shift_Time_To_0();//Сдвигаем время к 0
            init_data.Row1_Smothing();// Сглаживаем полученные данные
            init_data.Row2_Calculate();
            init_data.Row3_Average_Canal_Reg();
            init_data.Row4_Smoothing_Ekg();
            long[,] row1 = init_data.get_row1();

            long[] row3 = init_data.get_row3();


           int b = init_data.get_b();// Считаем число строк
            int b2 = init_data.get_b();// Считаем число строк

            int stepen = 2;

            for (int qs = 0; qs < 30; qs++)
            {
                if (stepen < b)
                {
                    stepen = stepen * 2;
                }
            }
            b = stepen / 2;
                      
            double[] row_x = new double[b];
            double[] row_y = new double[b];

            for (int r = 0; r < b; r++)
            {
                row_x[r] = System.Convert.ToDouble(row1[r, 0]) / 1000000;
                row_y[r] = System.Convert.ToDouble(row1[r, reg]);
            }
            Jenyay.Mathematics.Complex[] sp_dft = Fourier.FFT(row_y);
            

            double T_BIG = 0;//Длительность отсчета
           
            for (int i = 1; i < b / 2; i++)
            {
                T_BIG = T_BIG + (row_x[i] - row_x[i - 1]);

            }
            T_BIG = T_BIG + 0.001;
            double DW = (PI_ * 2.00) / T_BIG;
            double DT = (row_x[20] - row_x[19]);


            // Получим панель для рисования
            GraphPane pane = zedGraph1.GraphPane;
            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            PointPairList f1_list = new PointPairList();
            StreamWriter writer = new StreamWriter("Фурье_канал.txt");
 
            for (int j = 1; j < b / 2; j++)
            {
                f1_list.Add(Math.Round((DW * (j))/(4*3.14), 3), DT*sp_dft[j].Abs);
                writer.WriteLine(Math.Round((DW * (j)) / (4 * 3.14), 3) +"\t"+ DT* sp_dft[j].Abs);
            }
            LineItem f1_curve = pane.AddCurve("Канал 1", f1_list, Color.Blue, SymbolType.None);
            writer.Close();
            zedGraph1.AxisChange();

            // Обновляем график
            zedGraph1.Invalidate();
            richTextBox2.Text = "Готово";
            Read_Info_pazient();
        }

        private void button31_Click(object sender, EventArgs e)//Фурье
        {
            Clear_list_zed();
            Inizial();

            int reg = System.Convert.ToInt32(this.textBox13.Text);
            int ekg = System.Convert.ToInt32(this.textBox2.Text);


            Initial_data init_data = new Initial_data("test3.txt", reg, ekg);
            init_data.Row1_Shift_Time_To_0();//Сдвигаем время к 0
            init_data.Row1_Smothing();// Сглаживаем полученные данные
            init_data.Row2_Calculate();
            init_data.Row3_Average_Canal_Reg();
            init_data.Row4_Smoothing_Ekg();
            long[,] row1 = init_data.get_row1();

            long[] row3 = init_data.get_row3();


            int b = init_data.get_b();// Считаем число строк

       

            long[] row_x = new long[b];
            double[] row_y = new double[b];

            for (int r = 1; r < b; r++)
            {
                row_x[r] = row1[r, 0]-row1[r-1, 0];
                row_y[r] = System.Convert.ToDouble(row1[r, reg]);
            }

            Spectr.Fourier_Fast fourier = new Spectr.Fourier_Fast(row_x, row_y, b);
            fourier.Calculate_All();
            fourier.Spectr_09();

            double dw = fourier.Get_DW();
            double[] row_yy = fourier.Get_Amplituda_Spectr();

            usergraph = new UseZedgraph(zedGraph1);
            usergraph.ClearAll();//Очищаем полотно
            usergraph.MakeGraph_Spectr_Fast("Спектр Фурье", Convert.ToInt32(this.textBox17.Text), dw, row_yy);
            usergraph.Install_Pane("W, Гц", "P, Вт", "Данные");//Устанавливаем оси и заглавие
            usergraph.ResetGraph();//Обновляем


        }
              
       

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void inizialize_radbutton_spectr() {
            if (radioButton9.Checked) {
                radbutton_spectr = 0;
            }

            if (radioButton11.Checked)
            {
                radbutton_spectr = 1;
            }

            if (radioButton10.Checked)
            {
                radbutton_spectr = 2;
            }

        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button33_Click(object sender, EventArgs e)// Редактировать
        {
           

            int reg = System.Convert.ToInt32(this.textBox13.Text);
            int ekg = System.Convert.ToInt32(this.textBox2.Text);

            int bx= 0;
            long max_x = 0;

            int osob_x = 0;
            String max_x1 = null;
            String osob_X = null;

     
                Initial_data init_data = new Initial_data("test3.txt", reg, ekg);
            init_data.Row1_Shift_Time_To_0();//Сдвигаем время к 0
            init_data.Row1_Smothing();// Сглаживаем полученные данные
            init_data.Row2_Calculate();
            init_data.Row3_Average_Canal_Reg();
            init_data.Row4_Smoothing_Ekg();

                bx = init_data.get_b();
                max_x = init_data.Get_Row1_X_Y(bx - 1, 0);
                max_x1 = Convert.ToString(max_x/1000);

                Initial_processing.Divided_by_periods_data divided_row = new Initial_processing.Divided_by_periods_data(init_data, this.comboBox3.Text);
                divided_row.Calculate_Data_In_Period();
            //    divided_row.delete_zero_in_period();
                osob_x = divided_row.get_period_number_element();
                osob_X = Convert.ToString(osob_x);

           Form2 newForm = new Form2(max_x1, osob_X, this.comboBox3.Text, reg, ekg);
            newForm.Show();

        }

        private void button34_Click(object sender, EventArgs e)//+10 секунд
        { if (N_shift_axis < (max_time / 1000.0))
            {
                N_shift_axis = N_shift_axis + 10000.0;
            }
            double value_Min = N_shift_axis;
            double value_Max = N_shift_axis + 10000.0;
           
            usergraph = new UseZedgraph(zedGraph1);
            usergraph.Shift_Axis(value_Min, value_Max);
           
            progressBar1.Maximum = Convert.ToInt32(max_time / 1000000.0);
            progressBar1.Minimum = 0;
            double value_2 = N_shift_axis / 1000.0;
            if (value_2 < 0)
            {
                value_2 = progressBar1.Minimum;
            }
            if (value_2 > (max_time/1000000.0)) {
                value_2 = progressBar1.Maximum;
            
            }

            progressBar1.Value = Convert.ToInt32(value_2);
           

        }

        private void button35_Click(object sender, EventArgs e)//-10 секунд
        {
 if (N_shift_axis > 0)
            {
                N_shift_axis = N_shift_axis - 10000.0;
            }

            double value_Min = N_shift_axis;
            double value_Max = N_shift_axis + 10000.0;

            usergraph = new UseZedgraph(zedGraph1);
            usergraph.Shift_Axis(value_Min, value_Max);

            progressBar1.Maximum = Convert.ToInt32(max_time / 1000000.0);
            progressBar1.Minimum = 0;
            double value_2 = N_shift_axis / 1000.0;
            if (value_2 < 0)
            {
                value_2 = progressBar1.Minimum;
            }
            if (value_2 > (max_time / 1000000.0))
            {
                value_2 = progressBar1.Maximum;              

            }

            progressBar1.Value = Convert.ToInt32(value_2);
           
          

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox18.Enabled = true;
                textBox19.Enabled = true;
                label23.Enabled = true;
                label24.Enabled = true;
                label25.Enabled = true;
            }

            if (!checkBox2.Checked)
            {
                textBox18.Enabled = false;
                textBox19.Enabled = false;
                label23.Enabled = false;
                label24.Enabled = false;
                label25.Enabled = false;
            }
        }

        /// <summary>
        /// Отфильтровать сигнал в выбранном диапазоне используя БПФ
        /// </summary>
        /// <param name="r1">массив с данными</param>
        /// <param name="b1">Число строк массива</param>
        /// <returns></returns>
        private long[] Calculate_Fast_Fourier_Signal_Filtration(long[,] r1, int b1) {

            long[] result = new long[b1];
            int reg = System.Convert.ToInt32(this.textBox13.Text);
            int ekg = System.Convert.ToInt32(this.textBox2.Text);

            int b = b1;
            int stepen = 2;
            long[,] row1 = r1;

            for (int qs = 0; qs < 30; qs++)
            {
                if (stepen < b)
                {
                    stepen = stepen * 2;
                }
            }
            b = stepen;

            double[] row_x = new double[b];
            double[] row_y = new double[b];

            for (int r = 0; r < b - 10; r++)
            {
                row_x[r] = System.Convert.ToDouble(row1[r, 0]) / 1000000;
                row_y[r] = System.Convert.ToDouble(row1[r, reg]);
            }
            Jenyay.Mathematics.Complex[] sp_dft = Fourier.FFT(row_y);
            //////////////////////////////////////////////
            double T_BIG = 0;//Длительность отсчета

            for (int i = 1; i < b / 2; i++)
            {
                T_BIG = T_BIG + (row_x[i] - row_x[i - 1]);
            }
            T_BIG = T_BIG + 0.001;
            double DW = (PI_ * 2.00) / T_BIG;
            double DT = (row_x[20] - row_x[19]);


            //////////////////////////////////////////////////////////
            //Фильтрация

            if (checkBox2.Checked)
            {
                double left_border = Convert.ToDouble(textBox18.Text.Replace('.', ','));
                double right_border = Convert.ToDouble(textBox19.Text.Replace('.', ','));

                for (int j = 1; j < b1 - 1; j++)
                {
                    if (Math.Round((DW * (j)) / (4 * 3.14), 3) > left_border && Math.Round((DW * (j)) / (4 * 3.14), 3) < right_border)
                    {
                    }
                    else
                    {
                        sp_dft[j].Re = 0.0;
                        sp_dft[j].Im = 0.0;
                    }
                }

            }
            ////////////////////////////////////////////////////////////////
            Jenyay.Mathematics.Complex[] signal = Fourier.IFFT(sp_dft);

            for (int j = 0; j < b1; j++)
            {
                result[j] = Convert.ToInt64(signal[j].Abs);
            }
            
            return result;

        }

    }
}
