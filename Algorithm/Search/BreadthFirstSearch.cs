using NinePalacesPuzzle.Util;

namespace NinePalacesPuzzle.Algorithm.Search;

sealed class BreadthFirstSearch : IAlgorithm
{
    public IEnumerable<Step> Solve(PuzzleStatus start, PuzzleStatus end)
    {
        var stepList = new List<Step>();
        if (!start.IsConvertibleTo(end)) return stepList;

        var statusQueue = new Queue<PuzzleStatus>();
        statusQueue.Enqueue(start);
        while (statusQueue.Count > 0)
        {
            var status = statusQueue.Dequeue();
            if (status == end)
            {
                while (status.Parent is not null)
                {
                    stepList.Add(new(status.Parent, status));
                    status = status.Parent;
                }
                stepList.Reverse();
                break;
            }

            foreach (var candidate in status.MoveAround())
            {
                if (!status.Parents.Contains(candidate)) statusQueue.Enqueue(candidate);
            }
        }

        return stepList;
    }
}
