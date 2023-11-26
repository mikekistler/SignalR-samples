using Microsoft.AspNetCore.SignalR;

public class TicTacToeHub : Hub
{
    public static int PlayerCount = 0;

    public static List<int> state = new List<int>();

    // When a client connects, they send this request to start the game.
    // We only let 2 clients start the game. We keep track of the number of clients
    // in the group and if it's 2, we start the game.
    public async Task JoinGame()
    {
        if (PlayerCount == 0)
        {
            state = new List<int>();
            PlayerCount++;
            await Clients.Caller.SendAsync("startGame", 0, state); // Player X
            await Clients.Others.SendAsync("newGame");
        }
        else if (PlayerCount == 1)
        {
            PlayerCount++;
            await Clients.Caller.SendAsync("startGame", 1, state); // Player O
        }
        else
        {
            // All other clients will simply be spectators.
            // Send the current game state since observers may join in the middle of a game.
            await Clients.Caller.SendAsync("watchGame", state);
        }
    }

    // When a player makes a move, we update the game state and send to all clients.
    public async Task Move(int cellId)
    {
        // Update the game state
        state.Add(cellId);
        if (state.Count == 9)
        {
            // If there are no more moves, it's a draw.
            await Clients.All.SendAsync("gameOver", -1);
            // Set PlayerCount back to zero so that the next two clients can start a new game.
            PlayerCount = 0;
        }
        else
        {
            // Otherwise, we check if there's a winner.
            int winner = CheckWinner();
            if (winner != -1)
            {
                // If there's a winner, we send the winner to all clients.
                await Clients.All.SendAsync("gameOver", state, winner);
                // Set PlayerCount back to zero so that the next two clients can start a new game.
                PlayerCount = 0;
            }
            else
            {
                // Otherwise, we send the updated game state to all clients.
                await Clients.All.SendAsync("updateState", state);
            }
        }
    }

    // There are 8 possible winning combinations.
    private static readonly int[][] winners = new int[][]
    {
        new int[] {0, 1, 2},
        new int[] {3, 4, 5},
        new int[] {6, 7, 8},
        new int[] {0, 3, 6},
        new int[] {1, 4, 7},
        new int[] {2, 5, 8},
        new int[] {0, 4, 8},
        new int[] {2, 4, 6}
    };

    // This method checks if there's a winner.
    private int CheckWinner()
    {
        if (state.Count < 5)
        {
            // If there are less than 5 moves, there's no winner yet.
            return -1;
        }
        var Xs = state.Where((x, i) => i % 2 == 0).ToList();
        var Os = state.Where((x, i) => i % 2 != 0).ToList();

        if (Xs.Count >= 3 && winners.Any(winner => winner.All(cell => Xs.Contains(cell))))
        {
            return 0;
        }
        if (Os.Count >= 3 && winners.Any(winner => winner.All(cell => Os.Contains(cell))))
        {
            return 1;
        }
        return -1;
    }
}
