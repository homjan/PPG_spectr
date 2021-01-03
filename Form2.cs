using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Спектры_версия_2._0
{
    public partial class Form2 : Form
    {
        const int potok2 = 12;
        private String combobox_3;
        private int reg1;
        private int ekg1;        

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(String max_X, String max_point_x, String combo, int reg, int ekg)
        {
            InitializeComponent();
            textBox2.Text = max_X;
            textBox7.Text = max_point_x;
            this.combobox_3 = combo;
            this.reg1 = reg;
            this.ekg1 = ekg;

        }

        private void button1_Click(object sender, EventArgs e)//Редактировать. Временные промежутки
        {
            long min = Convert.ToInt64(Convert.ToDouble(this.textBox1.Text)) * 1000;
            long max = Convert.ToInt64(Convert.ToDouble(this.textBox2.Text)) * 1000;


            long minimum_delete_time = Convert.ToInt64(Convert.ToDouble(this.textBox3.Text))*1000;
            long maximum_delete_time = Convert.ToInt64(Convert.ToDouble(this.textBox4.Text))*1000;

            if (minimum_delete_time >= maximum_delete_time)
            {
                minimum_delete_time = 0;
                maximum_delete_time = 0;
            }

            if (minimum_delete_time <min) {
                minimum_delete_time = 0;
            }

            if (minimum_delete_time > max){
                minimum_delete_time = max;
            }

            if (maximum_delete_time < min)
            {
                maximum_delete_time = 0;
            }

            if (maximum_delete_time >= max)
            {
                maximum_delete_time = max;
            }

            int minimum_i = 0;
            int maximum_i = 0;

            Initial_data init_data = new Initial_data("test3.txt", reg1, ekg1);
            init_data.row1_shift_time_0();//Сдвигаем время к 0
            init_data.row1_smothing();// Сглаживаем полученные данные  

            long[,] row_1new = init_data.get_row1();
            int b_new = init_data.get_b();

            for (int i = 0; i < b_new; i++) {
                if (minimum_delete_time > row_1new[i, 0]) {
                    minimum_i = i;
                }
            }

            for (int i = 0; i < b_new; i++)
            {
                if (maximum_delete_time > row_1new[i, 0])
                {
                    maximum_i = i;
                }
            }
            int diff_i = maximum_i - minimum_i;
            int b_new_del = b_new - diff_i;
            

            ///////////////////////////////////
            

            if (radioButton1.Checked) {

            long[,] row_11new = new long[b_new, 1 + potok2];

                for (int i = 0; i < b_new; i++)
                {
                    row_11new[i, 0] = row_1new[i, 0];
                }

                for (int i = 0; i < b_new; i++)
                {
                    if (i < maximum_i && i > minimum_i)
                    {
                        for (int z = 1; z <= potok2; z++)
                        {
                            row_11new[i, z] = 0;
                        }
                    }
                    else
                    {
                        for (int z = 1; z <= potok2; z++)
                        {
                            row_11new[i, z] = row_1new[i, z];
                        }
                    }
                }
                              
                init_data.set_row1(row_11new);
                init_data.set_b(b_new);
                init_data.row1_write_in_file("test3.txt");
            }

            if (radioButton2.Checked) {
                long[,] row_11new = new long[b_new_del, 1 + potok2];

                for (int i = 0; i < b_new_del; i++) {                  
                                         
                  row_11new[i, 0] = row_1new[i, 0];  

                }

                for (int i = 0; i < b_new_del; i++)
                {
                    if (i < minimum_i)
                    {
                        for (int z = 1; z <= potok2; z++)
                        {
                            row_11new[i, z] = row_1new[i, z];
                        }

                    }
                    else {

                        for (int z = 1; z <= potok2; z++)
                        {
                            row_11new[i, z] = row_1new[i+diff_i, z];
                        }
                    } 
                }

                init_data.set_row1(row_11new);
                init_data.set_b(b_new_del);
                init_data.row1_write_in_file("test3.txt");
            }            
        }

        private void button2_Click(object sender, EventArgs e)//Редактировать. Особые точки
        {
            label11.Text = " ";

            int min = Convert.ToInt32(Convert.ToDouble(this.textBox8.Text));
            int max = Convert.ToInt32(Convert.ToDouble(this.textBox7.Text));


            int minimum_delete_period = Convert.ToInt32(Convert.ToDouble(this.textBox6.Text));
            int maximum_delete_period = Convert.ToInt32(Convert.ToDouble(this.textBox5.Text));

            if (minimum_delete_period >= maximum_delete_period)
            {
                minimum_delete_period = 0;
                maximum_delete_period = 0;
            }

            if (minimum_delete_period < min)
            {
                minimum_delete_period = 0;
            }

            if (minimum_delete_period > max)
            {
                minimum_delete_period = max;
            }

            if (maximum_delete_period < min)
            {
                maximum_delete_period = 0;
            }

            if (maximum_delete_period >= max)
            {
                maximum_delete_period = max;
            }

            int minimum_i = 0;
            int maximum_i = 0;

            Initial_data init_data = new Initial_data("test3.txt", reg1, ekg1);
            init_data.row1_shift_time_0();//Сдвигаем время к 0
            init_data.row1_smothing();// Сглаживаем полученные данные
            init_data.row2_calculate();
            init_data.row3_average_kanal_reg();

            long[,] row_1new = init_data.get_row1();
            int b_new = init_data.get_b();

            Initial_processing.Divided_by_periods_data divided_row = new Initial_processing.Divided_by_periods_data(init_data, combobox_3);
            divided_row.return_data_in_period();
            //  divided_row.delete_zero_in_period();
           
            minimum_i = Convert.ToInt32(divided_row.return_length_x_zero(minimum_delete_period, 0));
            maximum_i = Convert.ToInt32(divided_row.return_length_x_zero(maximum_delete_period, 0));

            int diff_i = maximum_i - minimum_i;
            int b_new_del = b_new - diff_i;
       
            if (radioButton4.Checked)
            {

                long[,] row_11new = new long[b_new, 1 + potok2];

                for (int i = 0; i < b_new; i++)
                {
                    row_11new[i, 0] = row_1new[i, 0];
                }

                for (int i = 0; i < b_new; i++)
                {
                    if (i < maximum_i && i > minimum_i)
                    {
                        for (int z = 1; z <= potok2; z++)
                        {
                            row_11new[i, z] = 0;
                        }
                    }
                    else
                    {
                        for (int z = 1; z <= potok2; z++)
                        {
                            row_11new[i, z] = row_1new[i, z];
                        }
                    }

                }
            
                init_data.set_row1(row_11new);
                init_data.set_b(b_new);
                init_data.row1_write_in_file("test3.txt");
            }

            if (radioButton3.Checked)
            {
                long[,] row_11new = new long[b_new_del, 1 + potok2];

                for (int i = 0; i < b_new_del; i++)
                {

                    row_11new[i, 0] = row_1new[i, 0];

                }

                for (int i = 0; i < b_new_del; i++)
                {
                    if (i < minimum_i)
                    {
                        for (int z = 1; z <= potok2; z++)
                        {
                            row_11new[i, z] = row_1new[i, z];
                        }
                    }
                    else
                    {
                        for (int z = 1; z <= potok2; z++)
                        {
                            row_11new[i, z] = row_1new[i + diff_i, z];
                        }
                    }
                }

                init_data.set_row1(row_11new);
                init_data.set_b(b_new_del);
                init_data.row1_write_in_file("test3.txt");
            }

            label11.Text = "Готово";

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            int total_minimum = Convert.ToInt32(Convert.ToDouble(this.textBox7.Text));
            int total_maximum = Convert.ToInt32(Convert.ToDouble(this.textBox8.Text));

            int minimum_delete_period = Convert.ToInt32(Convert.ToDouble(this.textBox6.Text));
            int maximum_delete_period = Convert.ToInt32(Convert.ToDouble(this.textBox5.Text));

            if (minimum_delete_period<total_minimum)
            {
                minimum_delete_period = total_minimum;
            }

            if (minimum_delete_period> total_maximum)
            {
                minimum_delete_period = total_maximum;
            }

            if (maximum_delete_period<total_minimum)
            {
                maximum_delete_period = total_minimum;
            }

            if (maximum_delete_period> total_maximum)
            {
                maximum_delete_period = total_maximum;
            }


            if (maximum_delete_period<minimum_delete_period)
            {
                maximum_delete_period = minimum_delete_period;
            }

            int minimum_i = 0;
            int maximum_i = 0;

            Initial_data init_data = new Initial_data("test3.txt", reg1, ekg1);
            init_data.row1_shift_time_0();//Сдвигаем время к 0
            init_data.row1_smothing();// Сглаживаем полученные данные
            init_data.row2_calculate();
            init_data.row3_average_kanal_reg();

            long[,] row_1new = init_data.get_row1();
            int b_new = init_data.get_b();

            Initial_processing.Divided_by_periods_data divided_row = new Initial_processing.Divided_by_periods_data(init_data, combobox_3);
            divided_row.return_data_in_period();
         
            minimum_i = Convert.ToInt32(divided_row.return_length_x_zero(minimum_delete_period, 0));
            maximum_i = Convert.ToInt32(divided_row.return_length_x_zero(maximum_delete_period, 0));
                     
            textBox9.Text = Convert.ToString(row_1new[minimum_i, 0]/1000);
            textBox10.Text = Convert.ToString(row_1new[maximum_i, 0] / 1000);
        }
    }
}
