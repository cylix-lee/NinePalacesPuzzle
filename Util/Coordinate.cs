namespace NinePalacesPuzzle.Util;

record Coordinate(int X, int Y)
{
    public const int OneDimensionalScale = 3;
    public const int TwoDimensionalScale = OneDimensionalScale * OneDimensionalScale;

    public static implicit operator Coordinate((int x, int y) tuple) => new(tuple.x, tuple.y);
    public static implicit operator (int, int)(Coordinate coordinate) => (coordinate.X, coordinate.Y);

    public static Coordinate operator +(Coordinate left, Coordinate right) => new(left.X + right.X, left.Y + right.Y);
    public static Coordinate operator -(Coordinate left, Coordinate right) => new(left.X - right.X, left.Y - right.Y);

    public bool IsOutOfBoundary() => X < 0 || X >= OneDimensionalScale || Y < 0 || Y >= OneDimensionalScale;
}
