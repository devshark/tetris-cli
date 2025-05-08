using Xunit;
using System.Linq;

namespace Tetris.Tests
{
    public class TetrominoTests
    {
        [Fact]
        public void TetrominoFactory_CreateTetromino_CreatesCorrectPiece()
        {
            // Arrange & Act
            var iPiece = TetrominoFactory.CreateTetromino(1);
            var jPiece = TetrominoFactory.CreateTetromino(2);
            var lPiece = TetrominoFactory.CreateTetromino(3);
            var oPiece = TetrominoFactory.CreateTetromino(4);
            
            // Assert
            Assert.Equal(1, iPiece.Type);
            Assert.Equal(4, iPiece.Blocks.Count);
            
            Assert.Equal(2, jPiece.Type);
            Assert.Equal(4, jPiece.Blocks.Count);
            
            Assert.Equal(3, lPiece.Type);
            Assert.Equal(4, lPiece.Blocks.Count);
            
            Assert.Equal(4, oPiece.Type);
            Assert.Equal(4, oPiece.Blocks.Count);
        }
        
        [Fact]
        public void Tetromino_MoveLeft_DecreasesXCoordinate()
        {
            // Arrange
            var tetromino = TetrominoFactory.CreateTetromino(1);
            int initialX = tetromino.X;
            
            // Act
            tetromino.MoveLeft();
            
            // Assert
            Assert.Equal(initialX - 1, tetromino.X);
        }
        
        [Fact]
        public void Tetromino_MoveRight_IncreasesXCoordinate()
        {
            // Arrange
            var tetromino = TetrominoFactory.CreateTetromino(1);
            int initialX = tetromino.X;
            
            // Act
            tetromino.MoveRight();
            
            // Assert
            Assert.Equal(initialX + 1, tetromino.X);
        }
        
        [Fact]
        public void Tetromino_MoveDown_IncreasesYCoordinate()
        {
            // Arrange
            var tetromino = TetrominoFactory.CreateTetromino(1);
            int initialY = tetromino.Y;
            
            // Act
            tetromino.MoveDown();
            
            // Assert
            Assert.Equal(initialY + 1, tetromino.Y);
        }
        
        [Fact]
        public void Tetromino_MoveUp_DecreasesYCoordinate()
        {
            // Arrange
            var tetromino = TetrominoFactory.CreateTetromino(1);
            int initialY = tetromino.Y;
            
            // Act
            tetromino.MoveUp();
            
            // Assert
            Assert.Equal(initialY - 1, tetromino.Y);
        }
        
        [Fact]
        public void Tetromino_Rotate_ChangesBlockPositions()
        {
            // Arrange
            var tetromino = TetrominoFactory.CreateTetromino(1); // I-piece
            var originalPositions = tetromino.Blocks.Select(b => new { b.X, b.Y }).ToList();
            
            // Act
            tetromino.Rotate();
            var newPositions = tetromino.Blocks.Select(b => new { b.X, b.Y }).ToList();
            
            // Assert
            // Check that at least some positions have changed (rotation occurred)
            Assert.NotEqual(originalPositions, newPositions);
        }
        
        [Fact]
        public void Tetromino_RotateBack_ReversesRotation()
        {
            // Arrange
            var tetromino = TetrominoFactory.CreateTetromino(1); // I-piece
            var originalPositions = tetromino.Blocks.Select(b => new { b.X, b.Y }).ToList();
            
            // Act
            tetromino.Rotate();
            tetromino.RotateBack();
            var finalPositions = tetromino.Blocks.Select(b => new { b.X, b.Y }).ToList();
            
            // Assert
            // Check that positions are back to original (rotation was reversed)
            for (int i = 0; i < originalPositions.Count; i++)
            {
                Assert.Equal(originalPositions[i].X, finalPositions[i].X);
                Assert.Equal(originalPositions[i].Y, finalPositions[i].Y);
            }
        }
        
        [Fact]
        public void TetrominoFactory_CreateRandomTetromino_ReturnsValidTetromino()
        {
            // Arrange & Act
            var tetromino = TetrominoFactory.CreateRandomTetromino();
            
            // Assert
            Assert.InRange(tetromino.Type, 1, 7);
            Assert.Equal(4, tetromino.Blocks.Count);
        }
    }
}
