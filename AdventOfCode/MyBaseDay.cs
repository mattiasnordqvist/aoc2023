namespace AdventOfCode;

public abstract class MyBaseDay : BaseDay
{
    private string? _input;
    protected string Input => _input ??= File.ReadAllText(InputFilePath);

    public void SetInput(string input)
    {
        _input = input;
    }
}

