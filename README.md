# C# Tetris Game

A classic Tetris implementation in C# with the following features:
- Classic tetromino blocks with different colors
- Accelerated drop when down arrow is held
- Multiple difficulty levels (up to level 9)

## How to Run

### Option 1: Using Visual Studio
1. Open the solution in Visual Studio
2. Build the solution
3. Run the game

### Option 2: Using .NET CLI
```bash
cd /path/to/tetris
dotnet build
dotnet run
```

### Option 3: Using Docker
```bash
# Build the Docker image
docker build -t tetris-game .

# Run the container with interactive terminal
docker run -it tetris-game
```

### Option 4: Using Docker Compose
```bash
docker-compose up --build
```

## Running Tests
```bash
# Run tests using .NET CLI
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

## Controls
- Left/Right Arrow: Move piece horizontally
- Down Arrow: Accelerate drop
- Up Arrow: Rotate piece
- Space: Hard drop
- P: Pause game

## Features
- Classic Tetris gameplay
- Different colored tetromino blocks
- Increasing difficulty levels (1-9)
- Score tracking

## License

[MIT License](LICENSE)

## Author

&copy; Anthony Lim

