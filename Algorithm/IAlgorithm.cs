using NinePalacesPuzzle.Util;

namespace NinePalacesPuzzle.Algorithm;

interface IAlgorithm
{
    IEnumerable<Step> Solve(PuzzleStatus start, PuzzleStatus end);
}
