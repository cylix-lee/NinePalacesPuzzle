namespace NinePalacesPuzzle.Util;

static class Log
{
    public record Tag(string Text, ConsoleColor Color, TextWriter? Writer = null);

    public static readonly Tag SuccessTag = new(nameof(Success), ConsoleColor.Green);
    public static readonly Tag MessageTag = new(nameof(Message), ConsoleColor.Blue);
    public static readonly Tag WarningTag = new(nameof(Warning), ConsoleColor.Yellow);
    public static readonly Tag FailureTag = new(nameof(Failure), ConsoleColor.Red, Console.Error);

    static void LogWithTag(Tag tag, string content)
    {
        var writer = tag.Writer ?? Console.Out;
        var originalColor = Console.ForegroundColor;
        writer.Write("[");
        {
            Console.ForegroundColor = tag.Color;
            writer.Write($"{tag.Text}");
        }
        Console.ForegroundColor = originalColor;
        writer.WriteLine($"] {content}");
    }

    public static void Success(string content) => LogWithTag(SuccessTag, content);
    public static void Message(string content) => LogWithTag(MessageTag, content);
    public static void Warning(string content) => LogWithTag(WarningTag, content);
    public static void Failure(string content) => LogWithTag(FailureTag, content);
}
