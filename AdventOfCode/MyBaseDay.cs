namespace AdventOfCode;

public abstract class MyBaseDay : BaseDay
{
    protected readonly string _input;

    public MyBaseDay()
    {
        _input = File.ReadAllText(InputFilePath);
    }
    public MyBaseDay(string input)
    {
        _input = input;
    }
}

