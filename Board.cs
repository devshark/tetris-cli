using System;

namespace Tetris
{
    public class Board
    {
        public int Width { get; }
        public int Height { get; }
        public int[,] Grid { get; }

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new int[height, width];
        }

        public bool IsValidPosition(Tetromino tetromino)
        {
            foreach (var block in tetromino.Blocks)
            {
                int x = tetromino.X + block.X;
                int y = tetromino.Y + block.Y;
                
                // Check if the block is out of bounds
                if (x < 0 || x >= Width || y >= Height)
                {
                    return false;
                }
                
                // Check if the block overlaps with an existing block
                // (but ignore if the block is above the board)
                if (y >= 0 && Grid[y, x] != 0)
                {
                    return false;
                }
            }
            
            return true;
        }

        public void AddPiece(Tetromino tetromino)
        {
            foreach (var block in tetromino.Blocks)
            {
                int x = tetromino.X + block.X;
                int y = tetromino.Y + block.Y;
                
                if (y >= 0 && y < Height && x >= 0 && x < Width)
                {
                    Grid[y, x] = tetromino.Type;
                }
            }
        }

        public int ClearCompletedLines()
        {
            int linesCleared = 0;
            
            for (int y = Height - 1; y >= 0; y--)
            {
                bool isLineComplete = true;
                
                for (int x = 0; x < Width; x++)
                {
                    if (Grid[y, x] == 0)
                    {
                        isLineComplete = false;
                        break;
                    }
                }
                
                if (isLineComplete)
                {
                    ClearLine(y);
                    ShiftLinesDown(y);
                    y++; // Check the same line again after shifting
                    linesCleared++;
                }
            }
            
            return linesCleared;
        }

        private void ClearLine(int y)
        {
            for (int x = 0; x < Width; x++)
            {
                Grid[y, x] = 0;
            }
        }

        private void ShiftLinesDown(int clearedLine)
        {
            for (int y = clearedLine; y > 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    Grid[y, x] = Grid[y - 1, x];
                }
            }
            
            // Clear the top line
            for (int x = 0; x < Width; x++)
            {
                Grid[0, x] = 0;
            }
        }
    }
}
