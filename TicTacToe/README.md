# Tic Tac Toe with SignalR

This sample is a simple Tic Tac Toe game that uses SignalR to communicate between the clients
that are playing or observing the game.

In addition to the game board, each client displays the client role (player X, player O, or observer)
and game state (which player's turn, game ended with winner or tie) -- all updated dynamically

The app only enables entry for the active player -- to stop cheaters!

### Prerequisites

- [.NET Core SDK 6.0 or later](https://dotnet.microsoft.com/download/dotnet-core/)

### Clone, build, and run

1. Clone the repo.
1. Navigate to the TicTacToe.Web folder.
1. Build the client app.

```sh
npm install
npm run build
```

1. Run the app.

```sh
dotnet run
```

## How I built this

1. Copy the [Scaffold](../Scaffold) project to a new folder.

```sh
cp -r Scaffold/ TicTacToe
```

1. Rename the Scaffold.Web solution and project to TicTacToe.Web.

```sh
cd TicTacToe
mv Scaffold.Web TicTacToe.Web
mv Scaffold.sln TicTacToe.sln
mv TicTacToe.Web/Scaffold.Web.csproj TicTacToe.Web/TicTacToe.Web.csproj
```

1. Fixup the solution file to point to the new project.

```sh
sed -i '' 's/Scaffold/TicTacToe/g' TicTacToe.sln
```

1. Update the index.html file to use the new project name.

```sh
cd TicTacToe.Web
sed -i '' 's/Scaffold/TicTacToe/g' client/index.html
```

1. Build the project.

```sh
npm install
npm run build
```

1. Run the project and open the main page to verify it works.

```sh
dotnet run
```sh

1. Create a Hubs folder and the TicTacToeHub class.

1. Add the TicTacToeHub to the services in Program.cs.

1. Modified the index.html file to show the game board.

1. Added logic in the client index.ts to capture game moves and send them to the server,
and process game updates from the server.
