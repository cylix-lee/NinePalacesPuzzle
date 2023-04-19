using NinePalacesPuzzle.Algorithm;
using NinePalacesPuzzle.Algorithm.Search;
using NinePalacesPuzzle.Util;

namespace NinePalacesPuzzle;

class Program
{
    public const string Usage = @"Welcome to Nine Palaces Puzzle Solver!
Please input a 3x3 matrix as the initial arrangement, and leave a '#' where the space lays.
For example:
    1 2 3
    4 # 5
    6 7 8
And input the targeted arrangement in the same format of the former one.";

    public const string Algorithms = @"Please choose one of those supported algorithms below:
    1. Breadth-First Search [Default]
    2. Limited Depth-First Search
And input the number of one algorithm.";

    public static readonly Dictionary<int, IAlgorithm> algorithmDictionary = new()
    {
        {1, new BreadthFirstSearch()},
        {2, new LimitedDepthFirstSearch()}
    };

    static void Main()
    {
        Log.Message(Usage);
        Console.WriteLine();

        var initialStatus = ReadStatusFromConsole();
        Log.Success($"Initial status {initialStatus} confirmed.\n");

        var finalStatus = ReadStatusFromConsole();
        Log.Success($"Final status {finalStatus} confirmed.\n");

        if (!initialStatus.IsConvertibleTo(finalStatus))
        {
            Log.Failure("IsConvertible: false. This puzzle is unsolvable.");
            return;
        }

        if (initialStatus == finalStatus)
        {
            Log.Message("Initial status is identical to the final status. There's no puzzle.");
            return;
        }

        Log.Message(Algorithms);
        Console.WriteLine();
        var algorithm = ReadAlgorithmFromConsole();
        foreach (var step in algorithm.Solve(initialStatus, finalStatus))
        {
            Console.WriteLine(step);
        }
        Log.Success("Solution finished.");
    }

    static PuzzleStatus ReadStatusFromConsole()
    {
        var numberList = new List<int>();
        while (numberList.Count < PuzzleStatus.TwoDimensionalScale)
        {
            Console.Write("input> ");
            var line = Console.ReadLine()!;
            foreach (var token in line.Split())
            {
                if (token == "#")
                {
                    numberList.Add(PuzzleStatus.SpaceTileValue);
                    continue;
                }
                numberList.Add(int.Parse(token));
            }
        }

        return new(numberList);
    }

    static IAlgorithm ReadAlgorithmFromConsole()
    {
        Console.Write("input> ");
        var index = int.Parse(Console.ReadLine()!.Trim());
        while (!algorithmDictionary.ContainsKey(index))
        {
            Log.Failure("Unacceptable index. Please re-input.");
            Console.Write("input> ");
            index = int.Parse(Console.ReadLine()!.Trim());
        }

        return algorithmDictionary[index];
    }
}
