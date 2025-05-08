using Xunit;

namespace Tetris.Tests
{
    public class BoardTests
    {
        [Fact]
        public void Board_Constructor_InitializesCorrectDimensions()
        {
            // Arrange & Act
            var board = new Board(10, 20);
            
            // Assert
            Assert.Equal(10, board.Width);
            Assert.Equal(20, board.Height);
            Assert.NotNull(board.Grid);
            Assert.Equal(20, board.Grid.GetLength(0)); // Height
            Assert.Equal(10, board.Grid.GetLength(1)); // Width
        }
        
        [Fact]
        public void Board_IsValidPosition_ReturnsTrueForValidPosition()
        {
            // Arrange
            var board = new Board(10, 20);
            var tetromino = TetrominoFactory.CreateTetromino(1); // I-piece
            tetromino.X = 5;
            tetromino.Y = 5;
            
            // Act
            bool result = board.IsValidPosition(tetromino);
            
            // Assert
            Assert.True(result);
        }
        
        [Fact]
        public void Board_IsValidPosition_ReturnsFalseForOutOfBoundsX()
        {
            // Arrange
            var board = new Board(10, 20);
            var tetromino = TetrominoFactory.CreateTetromino(1); // I-piece
            tetromino.X = 9; // This will put part of the I-piece out of bounds (right side)
            tetromino.Y = 5;
            
            // Act
            bool result = board.IsValidPosition(tetromino);
            
            // Assert
            Assert.False(result);
        }
        
        [Fact]
        public void Board_IsValidPosition_ReturnsFalseForOutOfBoundsY()
        {
            // Arrange
            var board = new Board(10, 20);
            var tetromino = TetrominoFactory.CreateTetromino(1); // I-piece
            tetromino.X = 5;
            tetromino.Y = 20; // This will put the I-piece out of bounds (bottom)
            
            // Act
            bool result = board.IsValidPosition(tetromino);
            
            // Assert
            Assert.False(result);
        }
        
        [Fact]
        public void Board_AddPiece_AddsBlocksToGrid()
        {
            // Arrange
            var board = new Board(10, 20);
            var tetromino = TetrominoFactory.CreateTetromino(1); // I-piece
            tetromino.X = 5;
            tetromino.Y = 5;
            
            // Act
            board.AddPiece(tetromino);
            
            // Assert
            // Check that the blocks are added to the grid
            foreach (var block in tetromino.Blocks)
            {
                int x = tetromino.X + block.X;
                int y = tetromino.Y + block.Y;
                Assert.Equal(tetromino.Type, board.Grid[y, x]);
            }
        }
        
        [Fact]
        public void Board_ClearCompletedLines_ClearsFullLine()
        {
            // Arrange
            var board = new Board(10, 20);
            
            // Fill a line (line 18)
            for (int x = 0; x < board.Width; x++)
            {
                board.Grid[18, x] = 1;
            }
            
            // Act
            int linesCleared = board.ClearCompletedLines();
            
            // Assert
            Assert.Equal(1, linesCleared);
            
            // Check that the line is cleared
            for (int x = 0; x < board.Width; x++)
            {
                Assert.Equal(0, board.Grid[18, x]);
            }
        }
        
        [Fact]
        public void Board_ClearCompletedLines_ShiftsLinesDown()
        {
            // Arrange
            var board = new Board(10, 20);
            
            // Add a block in line 17
            board.Grid[17, 5] = 1;
            
            // Fill line 18
            for (int x = 0; x < board.Width; x++)
            {
                board.Grid[18, x] = 1;
            }
            
            // Act
            int linesCleared = board.ClearCompletedLines();
            
            // Assert
            Assert.Equal(1, linesCleared);
            
            // Check that the block from line 17 has moved down to line 18
            Assert.Equal(1, board.Grid[18, 5]);
        }
    }
}
