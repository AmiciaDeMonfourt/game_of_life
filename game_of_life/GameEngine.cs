using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace game_of_life
{
    internal class GameEngine
    {
        public uint current_generation {  get; private set; }

        private bool[,] field;

        private readonly int rows;

        private readonly int cols;

        private readonly Random random = new Random();

        public GameEngine(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            field = new bool[cols, rows];

            for (int x = 0; x < cols; x++) {
                for (int y = 0; y < rows; y++) { 
                    field[x, y] = false;
                }
            }
        }

        public void MixUpField(int density)
        {
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    //получаем рандомное число от 0 до density
                    field[x, y] = random.Next(density) == 0;
                }
            }
        }

        private int CountNeighbors(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + cols) % cols;
                    int row = (y + j + rows) % rows;

                    bool is_self_checking = (col == x) && (row == y);
                    bool has_life = field[col, row];

                    if (has_life && !is_self_checking)
                    {
                        count++;
                    }
                }
            }
            return count;
        }


        public void NextGeneration()
        {
            bool[,] new_field = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighbors_count = CountNeighbors(x, y);
                    bool has_life = field[x, y];

                    if (!has_life && (neighbors_count == 3))
                    {
                        new_field[x, y] = true;
                    }
                    else if (has_life && ((neighbors_count < 2) || (neighbors_count > 3)))
                    {
                        new_field[x, y] = false;
                    }
                    else
                    {
                        new_field[x, y] = field[x, y];
                    }

                }
            }
            field = new_field;
            current_generation++;
        }


        public bool[,] GetCurrentGeneration()
        {
            var result = new bool[cols, rows];
            for(int x = 0; x < cols; x++)
            {
                for(int y = 0; y < rows; y++)
                {
                    result[x,y] = field[x,y];
                }
            }
            return result;
        }


        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }


        private void UpdateCell(int x, int y, bool state)
        {
            if(ValidateCellPosition(x,y))
                field[x,y] = state;  
        }


        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, true); 
        }

        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, false); 
        }
    }
}
