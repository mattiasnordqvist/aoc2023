using System.Text;

namespace AdventOfCode;

public static class Extensions
{
    public static int[] ia(this string[] input) => input.Select(int.Parse).ToArray();
    public static int i(this string[] input, int index) => int.Parse(input[index]);
    public static int i(this string input) => int.Parse(input);

    public static long[] la(this string[] input) => input.Select(long.Parse).ToArray();
    public static long l(this string[] input, int index) => long.Parse(input[index]);
    public static long l(this string input) => long.Parse(input);
    public static string[] Zplit(this string text, string by = "\n")
    {
        if (by == "\n")
        {
            return text.Split(by, StringSplitOptions.TrimEntries);
        }
        else
        {
            return text.Split(by, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
