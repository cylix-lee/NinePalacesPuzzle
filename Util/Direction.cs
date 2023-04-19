using System.Diagnostics;

namespace NinePalacesPuzzle.Util;

enum Direction
{
    Left, Up, Right, Down
}

static class DirectionExtension
{
    public static readonly Dictionary<Direction, Coordinate> Deltas = new()
    {
        {Direction.Left, (0, -1)},
        {Direction.Up, (-1, 0)},
        {Direction.Right, (0, 1)},
        {Direction.Down, (1, 0)}
    };

    public static Coordinate Delta(this Direction direction) => Deltas[direction];

    public static Direction DistinguishDirection(this Coordinate coordinate)
    {
        Direction? result = null;
        foreach (var (key, value) in Deltas)
        {
            if (value == coordinate)
            {
                result = key;
                break;
            }
        }

        Debug.Assert(result is not null);
        return (Direction)result;
    }
}