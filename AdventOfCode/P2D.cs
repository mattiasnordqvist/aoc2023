﻿
namespace AdventOfCode;

public record P2D(int x, int y)
{
    public bool IsDirectlyAbove(P2D p) => x == p.x && y == p.y - 1;
    public bool IsDirectlyBelow(P2D p) => x == p.x && y == p.y + 1;
    public bool IsDirectlyToTheLeftOf(P2D p) => x == p.x - 1 && y == p.y;
    public bool IsDirectlyToTheRightOf(P2D p) => x == p.x + 1 && y == p.y;

    public P2D PoVLeft(P2D p, (int x, int y) facing) => facing switch
    {
        (0, -1) => new P2D(p.x - 1, p.y),
        (0, 1) => new P2D(p.x + 1, p.y),
        (-1, 0) => new P2D(p.x, p.y + 1),
        (1, 0) => new P2D(p.x, p.y - 1),
    };

    public P2D[] S4() => [
       new P2D(x - 1, y),
        new P2D(x + 1, y),
        new P2D(x, y + 1),
        new P2D(x, y - 1)
   ];
    public P2D[] S8() => [
        new P2D(x - 1, y),
        new P2D(x + 1, y),
        new P2D(x, y + 1),
        new P2D(x, y - 1),
        new P2D(x - 1, y - 1),
        new P2D(x + 1, y + 1),
        new P2D(x - 1, y + 1),
        new P2D(x + 1, y - 1)
  ];

    internal P2D Up()
    {
        return new P2D(x, y-1);
    }
    internal P2D Down()
    {
        return new P2D(x, y + 1);
    }
    internal P2D Left()
    {
        return new P2D(x-1, y);
    }
    internal P2D Right()
    {
        return new P2D(x + 1, y);
    }

    public int ManhattanDistance(P2D p2)
    {
        return Math.Abs(x - p2.x) + Math.Abs(y - p2.y);
    }
}
