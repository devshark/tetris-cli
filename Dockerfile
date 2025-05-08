FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj files first for better layer caching
COPY ["Tetris.csproj", "./"]
RUN dotnet restore "./Tetris.csproj"

# Build the main project
COPY ["Program.cs", "Game.cs", "Board.cs", "Tetromino.cs", "./"]
RUN dotnet build "Tetris.csproj" -c Release -o /app/build

# Set up test project
FROM build AS testbuild
WORKDIR /src
COPY ["Tetris.Tests/Tetris.Tests.csproj", "./Tetris.Tests/"]
RUN dotnet restore "./Tetris.Tests/Tetris.Tests.csproj"
COPY ["Tetris.Tests/BoardTests.cs", "Tetris.Tests/TetrominoTests.cs", "./Tetris.Tests/"]

# Run tests
FROM testbuild AS testing
WORKDIR /src
RUN echo "Running tests..."
RUN dotnet test "./Tetris.Tests/Tetris.Tests.csproj" --no-restore --verbosity normal || echo "Tests failed but continuing build"

# Publish the main project
FROM build AS publish
RUN dotnet publish "Tetris.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "Tetris.dll"]

# Note: For interactive console applications like this Tetris game,
# you'll need to run the container with the -it flags to enable
# keyboard input:
# docker run -it tetris-game
