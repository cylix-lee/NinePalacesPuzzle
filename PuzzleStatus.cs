using System.Diagnostics;
using System.Text;
using NinePalacesPuzzle.Util;

namespace NinePalacesPuzzle;

sealed class PuzzleStatus : IEquatable<PuzzleStatus>
{
    public const int OneDimensionalScale = Coordinate.OneDimensionalScale;
    public const int TwoDimensionalScale = Coordinate.TwoDimensionalScale;
    public const int SpaceTileValue = -1;

    public static bool operator ==(PuzzleStatus? left, PuzzleStatus? right) => (left is null && right is null) || (left is not null && right is not null && left.Equals(right));

    public static bool operator !=(PuzzleStatus? left, PuzzleStatus? right) => !(left == right);

    static int[,] ToAcceptableArrayForm(IEnumerable<int> sequence)
    {
        Debug.Assert(sequence.Count() == TwoDimensionalScale);
        var array = new int[OneDimensionalScale, OneDimensionalScale];
        var index = 0;
        for (var i = 0; i < OneDimensionalScale; i++)
        {
            for (var j = 0; j < OneDimensionalScale; j++)
            {
                array[i, j] = sequence.ElementAt(index++);
            }
        }

        return array;
    }

    public Coordinate SpacePosition { get; set; }
    public PuzzleStatus? Parent { get; init; }
    public IEnumerable<PuzzleStatus> Parents
    {
        get
        {
            if (this.parents is not null) return this.parents;

            var parents = new List<PuzzleStatus>();
            var pointer = this;
            while (pointer.Parent is not null)
            {
                parents.Add(pointer.Parent);
                pointer = pointer.Parent;
            }

            this.parents = parents;
            return parents;
        }
    }
    public int ReverseOrderNumber
    {
        get
        {
            if (reverseOrderNumber is int value) return value;

            var appearedElements = new List<int>();
            var number = 0;
            foreach (var element in matrix)
            {
                if (element == SpaceTileValue) continue;

                foreach (var appeared in appearedElements)
                {
                    if (appeared == SpaceTileValue) continue;
                    if (appeared < element) number++;
                }
                appearedElements.Add(element);
            }

            reverseOrderNumber = number;
            return number;
        }
    }
    public int this[int x, int y] => matrix[x, y];

    readonly int[,] matrix;
    IEnumerable<PuzzleStatus>? parents;
    int? reverseOrderNumber;

    public PuzzleStatus(int[,] matrix)
    {
        Debug.Assert(matrix.GetLength(0) == OneDimensionalScale && matrix.GetLength(1) == OneDimensionalScale);
        this.matrix = (int[,])matrix.Clone();

        Coordinate? spacePosition = null;
        for (var i = 0; i < OneDimensionalScale; i++)
        {
            for (var j = 0; j < OneDimensionalScale; j++)
            {
                if (matrix[i, j] == SpaceTileValue)
                {
                    spacePosition = (i, j);
                    break;
                }
            }
        }
        SpacePosition = spacePosition!;
    }

    public PuzzleStatus(IEnumerable<int> sequence) : this(ToAcceptableArrayForm(sequence)) { }

    PuzzleStatus(PuzzleStatus formerStatus, Coordinate neoSpacePosition)
    {
        matrix = (int[,])formerStatus.matrix.Clone();
        SpacePosition = neoSpacePosition;

        var (x0, y0) = formerStatus.SpacePosition;
        var (x1, y1) = neoSpacePosition;

        (matrix[x0, y0], matrix[x1, y1]) = (matrix[x1, y1], matrix[x0, y0]);
    }

    public bool Equals(PuzzleStatus? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;

        for (var i = 0; i < OneDimensionalScale; i++)
        {
            for (var j = 0; j < OneDimensionalScale; j++)
            {
                if (matrix[i, j] != other.matrix[i, j]) return false;
            }
        }
        return true;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || (obj is not null && obj is PuzzleStatus other && Equals(other));
    public override int GetHashCode() => HashCode.Combine(matrix.GetHashCode(), SpacePosition.GetHashCode());

    public override string ToString()
    {
        var sequenceStringBuilder = new StringBuilder($"{matrix[0, 0]}");
        for (var i = 0; i < OneDimensionalScale; i++)
        {
            for (var j = 0; j < OneDimensionalScale; j++)
            {
                if (i == 0 && j == 0) continue;

                sequenceStringBuilder.Append($", {matrix[i, j]}");
            }
        }

        return $"PuzzleStatus {{{sequenceStringBuilder}}}";
    }

    public bool IsConvertibleTo(PuzzleStatus another) => ReverseOrderNumber % 2 == another.ReverseOrderNumber % 2;

    public PuzzleStatus? MoveTowards(Direction direction)
    {
        var neoSpacePosition = SpacePosition + direction.Delta();
        return neoSpacePosition.IsOutOfBoundary() ? null : new(this, neoSpacePosition) { Parent = this };
    }

    public IEnumerable<PuzzleStatus> MoveAround()
    {
        var neoStatusList = new List<PuzzleStatus>();
        foreach (var direction in Enum.GetValues<Direction>())
        {
            var neoSpacePosition = SpacePosition + direction.Delta();
            if (neoSpacePosition.IsOutOfBoundary()) continue;

            neoStatusList.Add(new(this, neoSpacePosition) { Parent = this });
        }

        return neoStatusList;
    }
}
