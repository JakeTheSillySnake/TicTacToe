﻿using TicTacToe.Domain.Model;

namespace TicTacToe.Web.Model;

public class GameWebDTO
{
    public Guid Id { get; init; }
    public GameBoardWebDTO GameBoard { get; init; } = new();
    public GameOutcome GameOutcome { get; init; }
}
