using System;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "C# Tetris";
            
            // Create and start the game
            Game game = new Game();
            game.Start();
        }
    }
}
