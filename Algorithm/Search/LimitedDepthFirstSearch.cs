using NinePalacesPuzzle.Util;

namespace NinePalacesPuzzle.Algorithm.Search;

sealed class LimitedDepthFirstSearch : IAlgorithm
{
    int depthLimit;

    public IEnumerable<Step> Solve(PuzzleStatus start, PuzzleStatus end)
    {
        Log.Message("Limited DFS needs a depth limit. Please input.");
        Console.Write("input> ");
        depthLimit = int.Parse(Console.ReadLine()!.Trim());

        return Dfs(1, start, start, end)!;
    }

    IEnumerable<Step>? Dfs(int depth, PuzzleStatus target, PuzzleStatus start, PuzzleStatus end)
    {
        if (depth > depthLimit) return null;

        foreach (var child in target.MoveAround())
        {
            if (target.Parents.Contains(child)) continue;

            if (child == end)
            {
                var steps = new List<Step>();
                var pointer = child;
                while (pointer.Parent is not null)
                {
                    steps.Add(new Step(pointer.Parent, pointer));
                    pointer = pointer.Parent;
                }

                steps.Reverse();
                return steps;
            }

            var childDfs = Dfs(depth + 1, child, start, end);
            if (childDfs is not null) return childDfs;
        }
        return null;
    }
}
