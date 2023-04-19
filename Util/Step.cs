namespace NinePalacesPuzzle.Util;

sealed class Step
{
    readonly string description;

    public Step(PuzzleStatus former, PuzzleStatus latter)
    {
        var spaceMoveDirection = (latter.SpacePosition - former.SpacePosition).DistinguishDirection();
        var (x, y) = latter.SpacePosition;
        description = $"(clicking {former[x, y]}) Move space {Enum.GetName<Direction>(spaceMoveDirection)}, "
                    + $"letting {former} -> {latter}";
    }

    public override string ToString() => description;
}
