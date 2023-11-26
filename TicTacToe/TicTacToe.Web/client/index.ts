import * as signalR from "@microsoft/signalr";

let roleSpan = document.getElementById("role")!;
let stateSpan = document.getElementById("state")!;

var player: number = -1;

let cells: HTMLInputElement[] = [];
for (let i = 0; i < 9; i++) {
    cells[i] = <HTMLInputElement> document.getElementById(`cell${i}`)!;
    cells[i].addEventListener("click", function (evt) {
        // Player has made a move, disable all cells
        disableAll();
        // send to hub
        connection.invoke("Move", i);
    });
}

function disableAll() {
    for (let i = 0; i < 9; i++) {
        cells[i].disabled = true;
    }
}
function enableAll() {
    for (let i = 0; i < 9; i++) {
        cells[i].disabled = false;
    }
}

// create connection
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/tictactoe")
    .build();
    
// connection events
function startSuccess() {
    console.log("Connected.");
    connection.invoke("JoinGame");
}
function startFail() {
    console.log("Connection failed.");
}

// start the connection
connection.start().then(startSuccess, startFail);

// client events
connection.on("startGame", (val: number, state: number[]) => {
    player = val;
    roleSpan.innerText = `You are player ${player == 0 ? "X" : "O"}`;
    update(state);
});
connection.on("watchGame", (state: number[]) => {
    player = -1;
    roleSpan.innerText = `You are an observer`;
    update(state);
});
connection.on("updateState", (state: number[]) => {
    update(state);
});
connection.on("newGame", () => {
    clear();

});
connection.on("gameOver", (state: number[], winner: number) => {
    update(state);
    disableAll();
    if (winner == -1) {
        stateSpan.innerText = "Game over, it was a tie";
    } else {
        stateSpan.innerText = `Game over, player ${winner == 0 ? "X" : "O"} won`;
    }
});

// The state of the game is just an array of up to 9 numbers that
// represent the moves of the players, starting with X and alternating.
// The value of an entry is the number of the cell that was played, 1-9.
function update(state: number[]) {
    if (state.length % 2 == 0) {
        stateSpan.innerText = "It is X's turn";
    } else {
        stateSpan.innerText = "It is O's turn";
    }
    if (player == state.length % 2) {
        enableAll();
    }
    for (let i = 0; i < state.length; i++) {
        cells[state[i]].innerText =  (i % 2 == 0) ? "X" : "O";
        cells[state[i]].disabled = true;
    }
}

function clear() {
    for (let i = 0; i < 9; i++) {
        cells[i].innerText = "";
    }
    disableAll();
    roleSpan.innerText = "";
    stateSpan.innerText = "New game started";
}
