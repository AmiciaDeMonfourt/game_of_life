using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game_of_life
{
    public partial class Form1 : Form
    {
        private Graphics graphics;

        private GameEngine game_engine;

        private int resolution;

        public Form1()
        {
            InitializeComponent();

            resolution = (int)resolution_button.Value;

            game_engine = new GameEngine
            (
                rows: pictureBox1.Height / resolution,
                cols: pictureBox1.Width / resolution
            );


            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            Text = $"Generation {"Бархатные тяги"}";

            DrawCurrentGeneration();
        }

        private void StartGame()
        {
            if (game_timer.Enabled) {
                return;
            }

            resolution_button.Enabled = false;
            density_button.Enabled = false;

            Text = $"Generation {game_engine.current_generation}";

            game_timer.Start();
        }


        private void StopGame()
        {
            if (!game_timer.Enabled)
            {
                return;
            }

            game_timer.Stop();

            resolution_button.Enabled = true;
            density_button.Enabled = true;
            mix_up_button.Enabled = true;
        }

        private void DrawCurrentGeneration()
        {
            graphics.Clear(Color.BurlyWood);

            var field = game_engine.GetCurrentGeneration();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(Brushes.White, x * resolution, y * resolution, resolution, resolution);
                }
            }


            pictureBox1.Refresh();
            Text = $"Generation {game_engine.current_generation}";
        }

        private void DrawNextGeneration()
        {
            graphics.Clear(Color.BurlyWood);

            var field = game_engine.GetCurrentGeneration();

            for(int x =  0; x < field.GetLength(0); x++)
            {
                for(int y = 0; y < field.GetLength(1); y++)
                {
                    if(field[x,y])
                        graphics.FillRectangle(Brushes.White, x * resolution, y * resolution, resolution, resolution);
                }
            }
 

            pictureBox1.Refresh();
            Text = $"Generation {game_engine.current_generation}";
            game_engine.NextGeneration();
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!game_timer.Enabled)
                return;


            if (e.Button == MouseButtons.Left)
            {
                int x = e.Location.X / resolution;
                int y = e.Location.Y / resolution;
                game_engine.AddCell(x, y);
            }


            if (e.Button == MouseButtons.Right)
            {
                int x = e.Location.X / resolution;
                int y = e.Location.Y / resolution;
                game_engine.RemoveCell(x, y);
            }
        }


        private void StartButton_Click(object sender, EventArgs e)
        {
            StartGame();            
        }


        private void StopButton_Click(object sender, EventArgs e)
        {
            StopGame();
        }


        private void game_timer_tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menu_timer_tick(object sender, EventArgs e)
        {
            //DrawCurrentField();
        }

        private void MixUpButton_Click(object sender, EventArgs e)
        {
            int density = (int)(density_button.Minimum) + (int)(density_button.Maximum) - (int)(density_button.Value);
            game_engine.MixUpField(density);

            DrawCurrentGeneration();
        }

        private void density_button_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
