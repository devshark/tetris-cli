using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Block
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Block(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Tetromino
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Type { get; }
        public List<Block> Blocks { get; private set; }
        private int[,] _rotationMatrix;
        private int _rotationState;

        public Tetromino(int type, List<Block> blocks, int[,] rotationMatrix)
        {
            Type = type;
            Blocks = blocks;
            _rotationMatrix = rotationMatrix;
            _rotationState = 0;
            
            // Start position (center top)
            X = 3;
            Y = -1;
        }

        public void MoveLeft()
        {
            X--;
        }

        public void MoveRight()
        {
            X++;
        }

        public void MoveDown()
        {
            Y++;
        }

        public void MoveUp()
        {
            Y--;
        }

        public void Rotate()
        {
            List<Block> newBlocks = new List<Block>();
            
            foreach (var block in Blocks)
            {
                // Apply rotation matrix
                int newX = block.Y;
                int newY = -block.X;
                
                newBlocks.Add(new Block(newX, newY));
            }
            
            Blocks = newBlocks;
            _rotationState = (_rotationState + 1) % 4;
        }

        public void RotateBack()
        {
            List<Block> newBlocks = new List<Block>();
            
            foreach (var block in Blocks)
            {
                // Apply inverse rotation matrix
                int newX = -block.Y;
                int newY = block.X;
                
                newBlocks.Add(new Block(newX, newY));
            }
            
            Blocks = newBlocks;
            _rotationState = (_rotationState + 3) % 4; // +3 is equivalent to -1 in modulo 4
        }
    }

    public static class TetrominoFactory
    {
        private static Random _random = new Random();

        public static Tetromino CreateRandomTetromino()
        {
            int type = _random.Next(1, 8); // 1-7 for the 7 tetromino types
            return CreateTetromino(type);
        }

        public static Tetromino CreateTetromino(int type)
        {
            List<Block> blocks = new List<Block>();
            int[,] rotationMatrix = new int[4, 4];
            
            switch (type)
            {
                case 1: // I-piece (Cyan)
                    blocks.Add(new Block(-1, 0));
                    blocks.Add(new Block(0, 0));
                    blocks.Add(new Block(1, 0));
                    blocks.Add(new Block(2, 0));
                    break;
                
                case 2: // J-piece (Blue)
                    blocks.Add(new Block(-1, -1));
                    blocks.Add(new Block(-1, 0));
                    blocks.Add(new Block(0, 0));
                    blocks.Add(new Block(1, 0));
                    break;
                
                case 3: // L-piece (Orange)
                    blocks.Add(new Block(-1, 0));
                    blocks.Add(new Block(0, 0));
                    blocks.Add(new Block(1, 0));
                    blocks.Add(new Block(1, -1));
                    break;
                
                case 4: // O-piece (Yellow)
                    blocks.Add(new Block(0, 0));
                    blocks.Add(new Block(1, 0));
                    blocks.Add(new Block(0, -1));
                    blocks.Add(new Block(1, -1));
                    break;
                
                case 5: // S-piece (Green)
                    blocks.Add(new Block(-1, 0));
                    blocks.Add(new Block(0, 0));
                    blocks.Add(new Block(0, -1));
                    blocks.Add(new Block(1, -1));
                    break;
                
                case 6: // T-piece (Purple)
                    blocks.Add(new Block(-1, 0));
                    blocks.Add(new Block(0, 0));
                    blocks.Add(new Block(1, 0));
                    blocks.Add(new Block(0, -1));
                    break;
                
                case 7: // Z-piece (Red)
                    blocks.Add(new Block(-1, -1));
                    blocks.Add(new Block(0, -1));
                    blocks.Add(new Block(0, 0));
                    blocks.Add(new Block(1, 0));
                    break;
            }
            
            return new Tetromino(type, blocks, rotationMatrix);
        }
    }
}
