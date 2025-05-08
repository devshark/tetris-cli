using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Tetris
{
    public class Game
    {
        private Board _board;
        private Tetromino _currentPiece;
        private Tetromino _nextPiece;
        private Timer _timer;
        private bool _gameOver;
        private int _score;
        private int _level;
        private int _linesCleared;
        private bool _isPaused;

        public Game()
        {
            _board = new Board(10, 20);
            _score = 0;
            _level = 1;
            _linesCleared = 0;
            _gameOver = false;
            _isPaused = false;
            
            // Initialize the timer
            _timer = new Timer();
            _timer.Elapsed += OnTimerElapsed;
            UpdateTimerInterval();
            
            // Create the first pieces
            _currentPiece = TetrominoFactory.CreateRandomTetromino();
            _nextPiece = TetrominoFactory.CreateRandomTetromino();
        }

        public void Start()
        {
            Console.CursorVisible = false;
            DrawBorders();
            DrawUI();
            
            _timer.Start();
            
            // Main game loop for handling input
            while (!_gameOver)
            {
                if (Console.KeyAvailable)
                {
                    HandleInput(Console.ReadKey(true).Key);
                }
                
                Thread.Sleep(50); // Small delay to prevent high CPU usage
            }
            
            GameOver();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_isPaused || _gameOver) return;
            
            // Move the current piece down
            if (!MovePieceDown())
            {
                // If the piece can't move down, lock it in place
                _board.AddPiece(_currentPiece);
                
                // Check for completed lines
                int linesCleared = _board.ClearCompletedLines();
                if (linesCleared > 0)
                {
                    UpdateScore(linesCleared);
                }
                
                // Get the next piece
                _currentPiece = _nextPiece;
                _nextPiece = TetrominoFactory.CreateRandomTetromino();
                
                // Check if the new piece can be placed
                if (!_board.IsValidPosition(_currentPiece))
                {
                    _gameOver = true;
                    _timer.Stop();
                    return;
                }
                
                DrawNextPiece();
            }
            
            DrawBoard();
            DrawPiece();
        }

        private void HandleInput(ConsoleKey key)
        {
            if (_gameOver) return;
            
            if (key == ConsoleKey.P)
            {
                TogglePause();
                return;
            }
            
            if (_isPaused) return;
            
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    MovePieceLeft();
                    break;
                case ConsoleKey.RightArrow:
                    MovePieceRight();
                    break;
                case ConsoleKey.DownArrow:
                    // Accelerate drop
                    MovePieceDown();
                    // Add points for manually dropping
                    _score += 1;
                    DrawScore();
                    break;
                case ConsoleKey.UpArrow:
                    RotatePiece();
                    break;
                case ConsoleKey.Spacebar:
                    HardDrop();
                    break;
            }
            
            DrawBoard();
            DrawPiece();
        }

        private bool MovePieceDown()
        {
            _currentPiece.MoveDown();
            
            if (!_board.IsValidPosition(_currentPiece))
            {
                _currentPiece.MoveUp(); // Move back up
                return false;
            }
            
            return true;
        }

        private bool MovePieceLeft()
        {
            _currentPiece.MoveLeft();
            
            if (!_board.IsValidPosition(_currentPiece))
            {
                _currentPiece.MoveRight(); // Move back right
                return false;
            }
            
            return true;
        }

        private bool MovePieceRight()
        {
            _currentPiece.MoveRight();
            
            if (!_board.IsValidPosition(_currentPiece))
            {
                _currentPiece.MoveLeft(); // Move back left
                return false;
            }
            
            return true;
        }

        private bool RotatePiece()
        {
            _currentPiece.Rotate();
            
            if (!_board.IsValidPosition(_currentPiece))
            {
                _currentPiece.RotateBack(); // Rotate back
                return false;
            }
            
            return true;
        }

        private void HardDrop()
        {
            int dropDistance = 0;
            
            // Keep moving down until we hit something
            while (MovePieceDown())
            {
                dropDistance++;
            }
            
            // Add points for hard drop (2 points per cell)
            _score += dropDistance * 2;
            DrawScore();
        }

        private void UpdateScore(int linesCleared)
        {
            // Classic Tetris scoring system
            int[] linePoints = { 0, 40, 100, 300, 1200 };
            _score += linePoints[linesCleared] * _level;
            
            _linesCleared += linesCleared;
            
            // Level up every 10 lines
            int newLevel = (_linesCleared / 10) + 1;
            if (newLevel > _level && newLevel <= 9)
            {
                _level = newLevel;
                UpdateTimerInterval();
            }
            
            DrawScore();
            DrawLevel();
        }

        private void UpdateTimerInterval()
        {
            // Speed increases with level
            // Level 1: 1000ms, Level 9: 100ms
            _timer.Interval = Math.Max(1000 - ((_level - 1) * 100), 100);
        }

        private void TogglePause()
        {
            _isPaused = !_isPaused;
            
            if (_isPaused)
            {
                Console.SetCursorPosition(15, 10);
                Console.Write("PAUSED");
            }
            else
            {
                Console.SetCursorPosition(15, 10);
                Console.Write("      ");
            }
        }

        private void GameOver()
        {
            Console.SetCursorPosition(15, 10);
            Console.Write("GAME OVER");
            
            Console.SetCursorPosition(0, 22);
            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }

        private void DrawBorders()
        {
            // Draw the game area border
            for (int y = 0; y < _board.Height; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("|");
                Console.SetCursorPosition(_board.Width + 1, y);
                Console.Write("|");
            }
            
            for (int x = 0; x <= _board.Width + 1; x++)
            {
                Console.SetCursorPosition(x, _board.Height);
                Console.Write("-");
            }
        }

        private void DrawUI()
        {
            Console.SetCursorPosition(_board.Width + 5, 1);
            Console.Write("NEXT PIECE:");
            
            Console.SetCursorPosition(_board.Width + 5, 7);
            Console.Write("SCORE:");
            DrawScore();
            
            Console.SetCursorPosition(_board.Width + 5, 9);
            Console.Write("LEVEL:");
            DrawLevel();
            
            DrawNextPiece();
        }

        private void DrawScore()
        {
            Console.SetCursorPosition(_board.Width + 5, 8);
            Console.Write($"{_score,8}");
        }

        private void DrawLevel()
        {
            Console.SetCursorPosition(_board.Width + 5, 10);
            Console.Write($"{_level,8}");
        }

        private void DrawBoard()
        {
            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    Console.SetCursorPosition(x + 1, y);
                    
                    if (_board.Grid[y, x] != 0)
                    {
                        Console.ForegroundColor = GetColorForBlock(_board.Grid[y, x]);
                        Console.Write("█");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
            }
        }

        private void DrawPiece()
        {
            foreach (var block in _currentPiece.Blocks)
            {
                int x = _currentPiece.X + block.X;
                int y = _currentPiece.Y + block.Y;
                
                if (y >= 0) // Only draw if the block is visible
                {
                    Console.SetCursorPosition(x + 1, y);
                    Console.ForegroundColor = GetColorForBlock(_currentPiece.Type);
                    Console.Write("█");
                    Console.ResetColor();
                }
            }
        }

        private void DrawNextPiece()
        {
            // Clear the next piece area
            for (int y = 2; y < 6; y++)
            {
                Console.SetCursorPosition(_board.Width + 5, y);
                Console.Write("        ");
            }
            
            // Draw the next piece
            foreach (var block in _nextPiece.Blocks)
            {
                int x = block.X;
                int y = block.Y;
                
                Console.SetCursorPosition(_board.Width + 8 + x, 3 + y);
                Console.ForegroundColor = GetColorForBlock(_nextPiece.Type);
                Console.Write("█");
                Console.ResetColor();
            }
        }

        private ConsoleColor GetColorForBlock(int blockType)
        {
            switch (blockType)
            {
                case 1: return ConsoleColor.Cyan;      // I
                case 2: return ConsoleColor.Blue;      // J
                case 3: return ConsoleColor.DarkYellow; // L
                case 4: return ConsoleColor.Yellow;    // O
                case 5: return ConsoleColor.Green;     // S
                case 6: return ConsoleColor.Magenta;   // T
                case 7: return ConsoleColor.Red;       // Z
                default: return ConsoleColor.Gray;
            }
        }
    }
}
