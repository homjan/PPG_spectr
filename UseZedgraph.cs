using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace Спектры_версия_2._0
{
    class UseZedgraph
    {
        const int potok2 = 13; //Общее Число потоков (работае только 4 + время)
        long shift_grafh = 200;
        long shift_grafh_ekg = 200;
        long maximum = 0;
        long minimum = 1024;

        private GraphPane pane;
        ZedGraphControl zedgraph;

        Initial_data initdata;

        Gistogramma.Gistogramma_Numeric gisto;

        Spectr.Spectr_numeric spect;


        public UseZedgraph(ZedGraphControl zedd)
        {            
            this.zedgraph = zedd;
            pane = zedd.GraphPane;

        }
        /// <summary>
        /// Конструктор для построения обычноого сигнала
        /// </summary>
        /// <param name="zedd"></param>
        /// <param name="data"></param>
        public UseZedgraph(ZedGraphControl zedd, Initial_data data)
        {
            this.zedgraph = zedd;
            pane = zedd.GraphPane;

            this.initdata = data;

        }
        /// <summary>
        /// Конструктор для построения гистограмм
        /// </summary>
        /// <param name="zedd"></param>
        /// <param name="gist"></param>
        public UseZedgraph(ZedGraphControl zedd,Gistogramma.Gistogramma_Numeric gist) {

            this.zedgraph = zedd;
            pane = zedd.GraphPane;
            this.gisto = gist;
        }

        /// <summary>
        /// Конструктор для построения спектров
        /// </summary>
        /// <param name="zedd"></param>
        /// <param name="spec"></param>
        public UseZedgraph(ZedGraphControl zedd, Spectr.Spectr_numeric spec)
        {

            this.zedgraph = zedd;
            pane = zedd.GraphPane;
            this.spect = spec;
        }
        /// <summary>
        /// Очистить кривые
        /// </summary>
        public void clearGraph()
        {
            pane.CurveList.Clear();
        }
        /// <summary>
        /// Построить график со всех 4 каналов
        /// </summary>
        /// <param name="xxx">Массив с сигналом</param>
        /// <param name="b">Число точек</param>
        public void MakeGraph_4_Canal(long[,] xxx, int b)
        { 
            PointPairList f1_list = new PointPairList();
            PointPairList f2_list = new PointPairList();
            PointPairList f3_list = new PointPairList();
            PointPairList f4_list = new PointPairList();

            for (int i = 3; i < b; i++)
            {
                f1_list.Add(xxx[i, 0] / 1000, xxx[i, 1]);
                f2_list.Add(xxx[i, 0] / 1000, xxx[i, 2]);
                f3_list.Add(xxx[i, 0] / 1000, xxx[i, 3]);
                f4_list.Add(xxx[i, 0] / 1000, xxx[i, 4]);
            }

            LineItem myCurve1 = pane.AddCurve("Канал1", f1_list, Color.Blue, SymbolType.None);
            LineItem myCurve2 = pane.AddCurve("Канал2", f2_list, Color.Red, SymbolType.None);
            LineItem myCurve3 = pane.AddCurve("Канал3", f3_list, Color.Green, SymbolType.None);
            LineItem myCurve4 = pane.AddCurve("Канал4", f4_list, Color.Black, SymbolType.None);

        }
        public void Install_Pane(String Xaxis, String Yaxis, String Title_text)
        {
            pane.Title.Text = Title_text;
            pane.XAxis.Title.Text = Xaxis;
            pane.YAxis.Title.Text = Yaxis;
        }

       
        /// <summary>
        /// Построить график ФПГ и е производной
        /// </summary>
        public void MakeGraph_On_Chosen_Canal() {

            long[,] row_1 = initdata.get_row1();
            long[] row_3 = initdata.get_row3();
            long[] row_4 = initdata.get_row4();
            int b = initdata.get_b();

         
            for (int y = 100; y < b - 10; y++)
            {
                if (maximum < row_1[y, initdata.REG])
                {
                    maximum = row_1[y, initdata.REG];
                }

                if (minimum > row_1[y, initdata.REG])
                {
                    minimum = row_1[y, initdata.REG];
                }

            }

            if ((maximum - minimum) < 200)
            {
                shift_grafh = 200;
                shift_grafh_ekg = 200;
            }
            else if ((maximum - minimum) > 500) {
                shift_grafh = -500;
                shift_grafh_ekg = 400;
            }
            else if ((maximum - minimum) > 1000)
            {
                shift_grafh = -5500;
                shift_grafh_ekg = 5500;
            }
            else
            {
                shift_grafh_ekg = 200;
                shift_grafh = -300;
            }


                // Создадим список точек для кривой f1(x)
                PointPairList f1_list = new PointPairList();
            PointPairList f2_list = new PointPairList();
            PointPairList f3_list = new PointPairList();
            PointPairList f4_list = new PointPairList();
            PointPairList f5_list_diff = new PointPairList();

            // Заполним массив точек для кривой f1-3(x)
            for (int y = 3; y < b - 10; y++)
            {
                f1_list.Add(row_1[y, 0] / 1000, row_4[y] + (shift_grafh_ekg));
                f2_list.Add(row_1[y, 0] / 1000, 570);
                f3_list.Add(row_1[y, 0] / 1000, shift_grafh);

                f4_list.Add(row_1[y, 0] / 1000, row_1[y, initdata.REG]);
                f5_list_diff.Add(row_1[y, 0] / 1000, row_3[y] / 10 + shift_grafh);
            }

            pane.XAxis.Title.Text = "t, мc";
            pane.YAxis.Title.Text = "R, Ом";
            pane.Title.Text = "Данные";

            LineItem f1_curve = pane.AddCurve("ЭКГ", f1_list, Color.Blue, SymbolType.None);
            LineItem f2_curve = pane.AddCurve("", f2_list, Color.Black, SymbolType.None);
            LineItem f3_curve = pane.AddCurve("", f3_list, Color.Black, SymbolType.None);
            LineItem f4_curve = pane.AddCurve(" РЭГ", f4_list, Color.Red, SymbolType.None);
            LineItem f5_curve_diff = pane.AddCurve("Производная РЭГ", f5_list_diff, Color.Green, SymbolType.None);


        }
        /// <summary>
        /// Построить график особых точек
        /// </summary>
        /// <param name="osob_x">Координаты х</param>
        /// <param name="osob_y">Координаты y</param>
        /// <param name="ew">Число наборов точек</param>
        public void MakeGraph_Special_Point(long[,] osob_x, long[,] osob_y, int ew) {

            // Выводим точки на экран
            PointPairList list5 = new PointPairList();
            PointPairList list6 = new PointPairList();
            PointPairList list7 = new PointPairList();
            PointPairList list8 = new PointPairList();
            PointPairList list9 = new PointPairList();

            for (int w = 2; w < ew - 2; w++)
            {
                /*   for (int i = 0; i < 14; i++)
                   {
                       list4.Add(osob_x[i, w] / 1000, osob_y[i, w]);
                   }*/
                list5.Add(osob_x[1, w] / 1000, osob_y[1, w]);
                list6.Add(osob_x[2, w] / 1000, osob_y[2, w]);
                list7.Add(osob_x[3, w] / 1000, osob_y[3, w]);
                list8.Add(osob_x[4, w] / 1000, osob_y[4, w]);

                list9.Add(osob_x[0, w] / 1000, osob_y[0, w]+ (shift_grafh_ekg));


            }
            LineItem myCurve5 = pane.AddCurve("B1", list5, Color.Blue, SymbolType.Diamond);
            LineItem myCurve6 = pane.AddCurve("B2", list6, Color.Black, SymbolType.Diamond);
            LineItem myCurve7 = pane.AddCurve("B3", list7, Color.DarkRed, SymbolType.Diamond);
            LineItem myCurve8 = pane.AddCurve("B4", list8, Color.Green, SymbolType.Diamond);
            LineItem myCurve9 = pane.AddCurve("ЭКГ", list9, Color.Brown, SymbolType.Diamond);


            // !!!
            // У кривой линия будет невидимой
            myCurve5.Line.IsVisible = false;
            myCurve6.Line.IsVisible = false;
            myCurve7.Line.IsVisible = false;
            myCurve8.Line.IsVisible = false;
            myCurve9.Line.IsVisible = false;

            // !!!
            // Цвет заполнения отметок (ромбиков) - голубой
            myCurve5.Symbol.Fill.Color = Color.Blue;
            myCurve6.Symbol.Fill.Color = Color.Black;
            myCurve7.Symbol.Fill.Color = Color.DarkRed;
            myCurve8.Symbol.Fill.Color = Color.Green;
            myCurve9.Symbol.Fill.Color = Color.Brown;

            // !!!
            // Тип заполнения - сплошная заливка
            myCurve5.Symbol.Fill.Type = FillType.Solid;
            myCurve6.Symbol.Fill.Type = FillType.Solid;
            myCurve7.Symbol.Fill.Type = FillType.Solid;
            myCurve8.Symbol.Fill.Type = FillType.Solid;
            myCurve9.Symbol.Fill.Type = FillType.Solid;

            // !!!
            // Размер ромбиков
            myCurve5.Symbol.Size = 8;
            myCurve6.Symbol.Size = 8;
            myCurve7.Symbol.Size = 8;
            myCurve8.Symbol.Size = 8;
            myCurve9.Symbol.Size = 8;

            pane.YAxis.MajorGrid.IsZeroLine = false;

        }
        /// <summary>
        /// Построение гистограммы
        /// </summary>
        /// <param name="text">Подпись гистограммы</param>
        public void MakeGraph_Gistogramma(String text)
        {  
            ///////////////////////////////////////////////
            // Версия с нулевыми столбиками
            int Gist_min_number =gisto.Gistogramma_number[0];
            int Gist_max_number = gisto.Gistogramma_number[0];

            for (int i = 0; i < gisto.turr; i++)
            {
                if (Gist_min_number > gisto.Gistogramma_number[i])
                    Gist_min_number = gisto.Gistogramma_number[i];

                if (Gist_max_number < gisto.Gistogramma_number[i])
                    Gist_max_number = gisto.Gistogramma_number[i];

            }

            string[] names = new string[Gist_max_number - Gist_min_number + 1];

            // Высота столбиков
            double[] values = new double[Gist_max_number - Gist_min_number + 1];

            // Заполним данные
            for (int i = Gist_min_number; i < Gist_max_number + 1; i++)
            {
                names[i - Gist_min_number] = string.Format("{0}-{1}", i * gisto.Shag_mod - gisto.Shag_mod, i * gisto.Shag_mod);

                values[i - Gist_min_number] = gisto.Gistogramma[i] * 100 / gisto.GIST_SUM;
            }


            StreamWriter writer = new StreamWriter("Гистограммы данные.txt");
            for (int i = Gist_min_number; i < Gist_max_number + 1; i++)
            {
                writer.WriteLine(names[i - Gist_min_number]+"\t"+ Math.Round(Convert.ToDouble(gisto.Gistogramma[i])*100.0/Convert.ToDouble(gisto.GIST_SUM), 3));
                   
            }
            writer.Close();

            BarItem curve = pane.AddBar(text, null, values, Color.Blue);
            // Настроим ось X так, чтобы она отображала текстовые данные
            pane.XAxis.Type = AxisType.Text;

            // Уставим для оси наши подписи
            pane.XAxis.Scale.TextLabels = names;
                       

        }

        /// <summary>
        /// Построение гистограммы с процентами
        /// </summary>
        /// <param name="text">Подпись гистограммы</param>
        public void MakeGraph_Gistogramma_Proz(String text)
        {
            ///////////////////////////////////////////////
            // Версия с нулевыми столбиками
            int Gist_min_number = gisto.Gistogramma_number[0];
            int Gist_max_number = gisto.Gistogramma_number[0];

            for (int i = 0; i < gisto.turr; i++)
            {
                if (Gist_min_number > gisto.Gistogramma_number[i])
                    Gist_min_number = gisto.Gistogramma_number[i];

                if (Gist_max_number < gisto.Gistogramma_number[i])
                    Gist_max_number = gisto.Gistogramma_number[i];

            }

            string[] names = new string[Gist_max_number - Gist_min_number + 1];

            // Высота столбиков
            double[] values = new double[Gist_max_number - Gist_min_number + 1];

            // Заполним данные
            for (int i = Gist_min_number; i < Gist_max_number + 1; i++)
            {
                names[i - Gist_min_number] = string.Format("{0}-{1}", i * gisto.Shag_mod_d - gisto.Shag_mod_d, i * gisto.Shag_mod_d);

                values[i - Gist_min_number] = gisto.Gistogramma[i] * 100 / gisto.GIST_SUM;
            }

            BarItem curve = pane.AddBar(text, null, values, Color.Blue);
            // Настроим ось X так, чтобы она отображала текстовые данные
            pane.XAxis.Type = AxisType.Text;

            // Уставим для оси наши подписи
            pane.XAxis.Scale.TextLabels = names;


        }//Построение гистограммы


        /// <summary>
        /// Построить график спектра
        /// </summary>
        /// <param name="name_curve">Имя кривой</param>
        /// <param name="N_point">Число точек</param>
        public void MakeGraph_Spectr(String name_curve, int N_point) {

            int n = N_point;
            double[] y = spect.Get_Amplitude_Spectr_Pow();

            if (y.Length < N_point) {
                n = y.Length;
            }
            // Создадим список точек для кривой f1(x)
            PointPairList f1_list = new PointPairList();

            // Заполним массив точек для кривой f1-3(x)
            for (int i = 0; i < n; i++)
            {
                f1_list.Add(Math.Round(spect.Get_DW() * (i) / (2 * 3.14), 3), Math.Round(y[i], 3));
            }

            LineItem f1_curve = pane.AddCurve(name_curve, f1_list, Color.Blue, SymbolType.None);

        }
        /// <summary>
        /// Построить график спектра для быстрого Фурье
        /// </summary>
        /// <param name="name_curve">Имя кривой</param>
        /// <param name="N_point">Число точек</param>
        /// <param name="dw"></param>
        /// <param name="y1"></param>
        public void MakeGraph_Spectr_Fast(String name_curve, int N_point, double dw, double[] y1)
        {

            int n = N_point;
            double[] y = y1;

            if (y.Length < N_point)
            {
                n = y.Length;
            }
            // Создадим список точек для кривой f1(x)
            PointPairList f1_list = new PointPairList();

            // Заполним массив точек для кривой f1-3(x)
            for (int i = 0; i < n; i++)
            {
                f1_list.Add(Math.Round((dw * (i + 1))/(2 * 3.14), 3), Math.Round(y[i], 3));

            }

            LineItem f1_curve = pane.AddCurve(name_curve, f1_list, Color.Blue, SymbolType.None);

        }
        /// <summary>
        /// Обновить график
        /// </summary>
        public void ResetGraph()
        {
            zedgraph.AxisChange();            
            zedgraph.Invalidate();
        }

        /// <summary>
        /// Сохранить график
        /// </summary>
        public void SaveGraph()
        {
            zedgraph.SaveAsBitmap();
        }

        /// <summary>
        /// Очистить график
        /// </summary>
        public void ClearAll()
        {

            pane.CurveList.Clear();

            pane.XAxis.Scale.MinAuto = true;
            pane.XAxis.Scale.MaxAuto = true;

            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;

            zedgraph.AxisChange();
            zedgraph.Invalidate();

        }

        /// <summary>
        /// Устанавливаем интересующий нас интервал в диапазоне value_min-value_mах
        /// </summary>
        /// <param name="value_min"></param>
        /// <param name="value_max"></param>
        public void Shift_Axis(double value_min, double value_max ) {
            // Устанавливаем интересующий нас интервал по оси Y
            pane.XAxis.Scale.Min = value_min;
            pane.XAxis.Scale.Max = value_max;

            pane.YAxis.Scale.Min = 0;
            pane.YAxis.Scale.Max = 1250;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            // В противном случае на рисунке будет показана только часть графика,
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedgraph.AxisChange();
            zedgraph.Invalidate();

        }


    }
}
