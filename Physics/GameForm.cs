using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.NetworkInformation;

namespace Physics
{
    public partial class GameForm : Form
    {

        public int[,] map = new int[4, 4];
        public Label[,] labels = new Label[4, 4];
        public PictureBox[,] pics = new PictureBox[4, 4];
        private int score = 0;

        public GameForm()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnKeyboardPressed);

            map[0, 0] = 1;
            map[0, 1] = 1;
            CreateMap();
            CreatePics();
            GenerateNewPic();
        }  

        private void CreateMap()
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(12 + 56 * j, 95 + 56 * i);
                    //pic.Location = new Point(12 + 56 * j, 73 + 56 * i);
                    pic.Size = new Size(50, 50);
                    pic.BackColor = Color.Gray;
                    this.Controls.Add(pic);
                }
            }
        }

        private void GenerateNewPic()
        {
            Random rnd = new Random();
            int a = rnd.Next(0, 4);
            int b = rnd.Next(0, 4);
            while (pics[a, b] != null)
            {
                a = rnd.Next(0, 4);
                b = rnd.Next(0, 4);
            }
            map[a, b] = 1;
            pics[a, b] = new PictureBox();
            labels[a, b] = new Label();
            labels[a, b].Text = "2";
            labels[a, b].Size = new Size(50, 50);
            labels[a, b].TextAlign = ContentAlignment.MiddleCenter;
            labels[a, b].Font = new Font(new FontFamily("Microsoft Sans Serif"), 15);
            pics[a, b].Controls.Add(labels[a, b]);
            pics[a, b].Location = new Point(12 + b * 56, 95 + 56 * a);
            pics[a, b].Size = new Size(50, 50);
            pics[a, b].BackColor = Color.Yellow;
            this.Controls.Add(pics[a, b]);
            pics[a, b].BringToFront();

            if (labels[a, b].Text == "2048")
            {
                MessageBox.Show("Congratulations! You've reached 2048 and won the game!", "Victory!");
                ResetGame();
            }
        }

        private void CreatePics()
        {
            pics[0, 0] = new PictureBox();
            labels[0, 0] = new Label();
            labels[0, 0].Text = "2";
            labels[0, 0].Size = new Size(50, 50);
            labels[0, 0].TextAlign = ContentAlignment.MiddleCenter;
            labels[0, 0].Font = new Font(new FontFamily("Microsoft Sans Serif"), 15);
            pics[0, 0].Controls.Add(labels[0, 0]);
            pics[0, 0].Location = new Point(12 , 95);
            pics[0, 0].Size = new Size(50, 50);
            pics[0, 0].BackColor = Color.Yellow;
            this.Controls.Add(pics[0, 0]);
            pics[0, 0].BringToFront();

            pics[0, 1] = new PictureBox();
            labels[0, 1] = new Label();
            labels[0, 1].Text = "2";
            labels[0, 1].Size = new Size(50, 50);
            labels[0, 1].TextAlign = ContentAlignment.MiddleCenter;
            labels[0, 1].Font = new Font(new FontFamily("Microsoft Sans Serif"), 15);
            pics[0, 1].Controls.Add(labels[0, 1]);
            pics[0, 1].Location = new Point(68, 95);
            pics[0, 1].Size = new Size(50, 50);
            pics[0, 1].BackColor = Color.Yellow;
            this.Controls.Add(pics[0, 1]);
            pics[0, 1].BringToFront();
        }

        private void ChangeColor(int sum, int k, int j)
        {
            // Calculate hue value based on the sum
            float hue = (float)(sum % 360) / 360.0f;

            // Create a color with a gradient based on the hue
            Color tileColor = ColorFromAhsb(255, hue, 0.8f, 0.8f);

            pics[k, j].BackColor = tileColor;
        }

        // Helper method to create a color from AHSB values
        private Color ColorFromAhsb(int alpha, float hue, float saturation, float brightness)
        {
            int hi = Convert.ToInt32(Math.Floor(hue * 6));
            float f = hue * 6 - hi;
            int p = Convert.ToInt32(brightness * (1 - saturation) * 255);
            int q = Convert.ToInt32(brightness * (1 - f * saturation) * 255);
            int t = Convert.ToInt32(brightness * (1 - (1 - f) * saturation) * 255);

            int red, green, blue;

            switch (hi % 6)
            {
                case 0:
                    red = Convert.ToInt32(brightness * 255);
                    green = t;
                    blue = p;
                    break;
                case 1:
                    red = q;
                    green = Convert.ToInt32(brightness * 255);
                    blue = p;
                    break;
                case 2:
                    red = p;
                    green = Convert.ToInt32(brightness * 255);
                    blue = t;
                    break;
                case 3:
                    red = p;
                    green = q;
                    blue = Convert.ToInt32(brightness * 255);
                    break;
                case 4:
                    red = t;
                    green = p;
                    blue = Convert.ToInt32(brightness * 255);
                    break;
                default:
                    red = Convert.ToInt32(brightness * 255);
                    green = p;
                    blue = q;
                    break;
            }

            return Color.FromArgb(alpha, red, green, blue);
        }

        private void OnKeyboardPressed(object sender,KeyEventArgs e)
        {
            bool ifPicWasMoved = false;

            switch (e.KeyCode.ToString())
            {
                case "Right":
                    for(int k = 0; k < 4; k++)
                    {
                        for(int l = 2; l >= 0; l--)
                        {
                            if(map[k,l] == 1)
                            {
                                for(int j = l + 1; j < 4; j++)
                                {
                                    if(map[k,j] == 0)
                                    {
                                        ifPicWasMoved = true;
                                        map[k, j - 1] = 0;
                                        map[k, j] = 1;
                                        pics[k, j] = pics[k, j - 1];
                                        pics[k, j - 1] = null;
                                        labels[k, j] = labels[k, j - 1];
                                        labels[k, j - 1] = null;
                                        pics[k, j].Location = new Point(pics[k, j].Location.X + 56, pics[k, j].Location.Y);
                                    }else
                                    {
                                        int a = int.Parse(labels[k, j].Text);
                                        int b = int.Parse(labels[k, j-1].Text);
                                        if (a == b)
                                        {
                                            ifPicWasMoved = true;
                                            labels[k, j].Text = (a + b).ToString();
                                            score += (a + b);
                                            ChangeColor(a + b, k, j);
                                            label1.Text = "Счет: " + score;
                                            map[k, j - 1] = 0;
                                            this.Controls.Remove(pics[k, j - 1]);
                                            this.Controls.Remove(labels[k, j - 1]);
                                            pics[k, j - 1] = null;
                                            labels[k, j - 1] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "Left":
                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 1; l < 4; l++)
                        {
                            if (map[k, l] == 1)
                            {
                                for (int j = l - 1; j >= 0; j--)
                                {
                                    if (map[k, j] == 0)
                                    {
                                        ifPicWasMoved = true;
                                        map[k, j + 1] = 0;
                                        map[k, j] = 1;
                                        pics[k, j] = pics[k, j + 1];
                                        pics[k, j + 1] = null;
                                        labels[k, j] = labels[k, j + 1];
                                        labels[k, j + 1] = null;
                                        pics[k, j].Location = new Point(pics[k, j].Location.X - 56, pics[k, j].Location.Y);
                                    }
                                    else
                                    {
                                        int a = int.Parse(labels[k, j].Text);
                                        int b = int.Parse(labels[k, j + 1].Text);
                                        if (a == b)
                                        {
                                            ifPicWasMoved = true;
                                            labels[k, j].Text = (a + b).ToString();
                                            score += (a + b);
                                            ChangeColor(a + b, k, j);
                                            label1.Text = "Счет: " + score;
                                            map[k, j + 1] = 0;
                                            this.Controls.Remove(pics[k, j + 1]);
                                            this.Controls.Remove(labels[k, j + 1]);
                                            pics[k, j + 1] = null;
                                            labels[k, j + 1] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "Down":
                    for (int k = 2; k >= 0; k--)
                    {
                        for (int l = 0; l <4; l++)
                        {
                            if (map[k, l] == 1)
                            {
                                for (int j = k + 1; j < 4; j++)
                                {
                                    if (map[j, l] == 0)
                                    {
                                        ifPicWasMoved = true;
                                        map[j - 1,l] = 0;
                                        map[j,l] = 1;
                                        pics[j,l] = pics[j - 1,l];
                                        pics[j - 1,l] = null;
                                        labels[j,l] = labels[j - 1,l];
                                        labels[j - 1,l] = null;
                                        pics[j,l].Location = new Point(pics[j,l].Location.X, pics[j,l].Location.Y+56);
                                    }
                                    else
                                    {
                                        int a = int.Parse(labels[j,l].Text);
                                        int b = int.Parse(labels[j - 1,l].Text);
                                        if (a == b)
                                        {
                                            ifPicWasMoved = true;
                                            labels[j,l].Text = (a + b).ToString();
                                            score += (a + b);
                                            ChangeColor(a + b, j,l);
                                            label1.Text = "Счет: " + score;
                                            map[j - 1,l] = 0;
                                            this.Controls.Remove(pics[j - 1,l]);
                                            this.Controls.Remove(labels[j - 1,l]);
                                            pics[j - 1,l] = null;
                                            labels[j - 1,l] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "Up":
                    for (int k = 1; k < 4; k++)
                    {
                        for (int l = 0; l < 4; l++)
                        {
                            if (map[k, l] == 1)
                            {
                                for (int j = k - 1; j >=0; j--)
                                {
                                    if (map[j, l] == 0)
                                    {
                                        ifPicWasMoved = true;
                                        map[j + 1, l] = 0;
                                        map[j, l] = 1;
                                        pics[j, l] = pics[j + 1, l];
                                        pics[j + 1, l] = null;
                                        labels[j, l] = labels[j + 1, l];
                                        labels[j + 1, l] = null;
                                        pics[j, l].Location = new Point(pics[j, l].Location.X, pics[j, l].Location.Y - 56);
                                    }
                                    else
                                    {
                                        int a = int.Parse(labels[j, l].Text);
                                        int b = int.Parse(labels[j + 1, l].Text);
                                        if (a == b)
                                        {
                                            ifPicWasMoved = true;
                                            labels[j, l].Text = (a + b).ToString();
                                            score += (a + b);
                                            ChangeColor(a + b, j, l);
                                            label1.Text = "Счет: " + score;
                                            map[j + 1, l] = 0;
                                            this.Controls.Remove(pics[j + 1, l]);
                                            this.Controls.Remove(labels[j + 1, l]);
                                            pics[j + 1, l] = null;
                                            labels[j + 1, l] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
            if (ifPicWasMoved)
            {
                GenerateNewPic();
                if (CheckGameOver())
                {
                    MessageBox.Show("На поле не осталось места! Ваш счет: " + score, "Игра закончина");
                    ResetGame();
                }
            }
        }
        private bool CheckGameOver()
        {
            int freeCellsCount = map.Cast<int>().Count(value => value == 0);

            // Если свободных ячеек меньше двух, игра завершена
            if (freeCellsCount < 2)
            {
                return true;
            }

            // Если не найдено свободных ячеек или соседних одинаковых значений, игра завершена
            return false;
        }

        private void ResetGame()
        {
            // Сбросить все необходимые переменные и структуры данных
            score = 0;
            label1.Text = "Счет: " + score;

            // Очистить карту, картинки и ярлыки
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    map[i, j] = 0;

                    if (pics[i, j] != null)
                    {
                        this.Controls.Remove(pics[i, j]);
                        pics[i, j] = null;
                    }

                    if (labels[i, j] != null)
                    {
                        this.Controls.Remove(labels[i, j]);
                        labels[i, j] = null;
                    }
                }
            }

            // Создать новую карту и начальные элементы
            CreateMap();
            CreatePics();
            

            // Дополнительная проверка и удаление фантомных плиток
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (pics[i, j] != null)
                    {
                        this.Controls.Remove(pics[i, j]);
                        pics[i, j] = null;
                    }

                    if (labels[i, j] != null)
                    {
                        this.Controls.Remove(labels[i, j]);
                        labels[i, j] = null;
                    }
                }
            }
            GenerateNewPic();
        }


        Point LastPoint;
        private void Form1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - LastPoint.X;
                this.Top += e.Y - LastPoint.Y;
            }
        }

        private void Form1_MouseDown_1(object sender, MouseEventArgs e)
        {
            LastPoint = new Point(e.X, e.Y);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            ResetGame();
        }
    }
}
