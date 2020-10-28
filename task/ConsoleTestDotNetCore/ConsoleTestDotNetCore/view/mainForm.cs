using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


/// <summary>
/// Вынести всю логику, при нажатие на кнопки должны вызываться только методы класса
/// </summary>

namespace Cellular_automaton
{
    /// <summary>
    /// Главная форма
    /// </summary>
    public class mainForm : Form
    {
        private Graphics graphics;
        /// <summary>
        /// Переменная отвечающая за разрещение вывода.
        /// </summary>
        private int resolution = 10;
        private int rows;
        private int cols;

        /// <summary>
        /// Делегат для доступа к форме из другого потока
        /// </summary>
        private delegate void safeRefresh();

        private Field field;

        /// <summary>
        /// Преоброзования которые проводяться над клеткой
        /// </summary>
        private Transformations transform;

        /// <summary>
        /// Правило показа
        /// </summary>
        private Rules rule;

        /// <summary>
        /// Поток для вычислений и перерисовки поля
        /// </summary>
        private Thread threadReDraw;

        /// <summary>
        /// Временный костыль для кнопки стоп
        /// </summary>
        private bool loop = true;

        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            Map.Image = new Bitmap(Map.Width, Map.Height);
            graphics = Graphics.FromImage(Map.Image);
            transform = new Transformations();
            rule = new Rules();

            rows = Map.Height / resolution;
            cols = Map.Width / resolution;

            field = new Field(cols, rows);
            CompleteList();
        }
        
        /// <summary>
        /// Обертка для RefreshSely
        /// </summary>
        private void refresh()
        {
            while(loop)
                RefreshSafely();
        }

        /// <summary>
        /// Потоко безопасное обновление поля
        /// </summary>
        private void RefreshSafely()
        {
            if (Map.InvokeRequired)
            {
                field.NextGeneration();
                graphics.Clear(Color.Black);
                ReDraw(rule.Rule_1, Brushes.Red);
                var s = new safeRefresh(RefreshSafely);
                Map.Invoke(s);
            }
            else
            {
                Map.Refresh();
            }
        }

        /// <summary>
        /// Перерисовка изображения
        /// </summary>
        /// <param name="p">Предикат <Cell> по правилу которого рисуется поле</param>
        /// <param name="b">Кисть которой рисуется</param>
        private void ReDraw(Predicate<Cell> p, Brush b)
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (p(field.data[i][j]))
                    {
                        graphics.FillRectangle(b, i * resolution, j * resolution, resolution, resolution);
                    }
                }
            }
        }

        public void CompleteList()
        {
            transformList.Items.Add("Dicriment");
            transformList.Items.Add("Incriment");
            transformList.Items.Add("Life");
            transformList.Items.Add("Nothing");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        /// <summary>
        /// Запуск автомата
        /// </summary>
        private void Start()
        {
            //field.transform = transform.Life;
            field.Generate();

            graphics.Clear(Color.Black);
            ReDraw(rule.Rule_1, Brushes.Red);
            Map.Refresh();

            threadReDraw = new Thread(refresh);
            threadReDraw.Name = "threadReDraw";
            threadReDraw.Start();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            loop = false;
        }

        private void transformList_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch( transformList.SelectedItem.ToString())
            {
                case "Life" : field.transform = transform.Life; break;
                case "Incriment" : field.transform = transform.Incriment; break;
                case "Dicriment" : field.transform = transform.Dicriment; break;
                case "Nothing" : field.transform = transform.Nothing; break;
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            field.Generate();
        }
    }
}
