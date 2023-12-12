using AdventOfCode;

using FluentAssertions;

using Xunit;

namespace Tests;

public class Day12Tests
{

    [Fact]
    public void Example1Match()
    {
        Line.Matches("#.#.###", [1,1,3]).Should().BeTrue();
    }
    [Fact]
    public void Example1NoMatch()
    {
        Line.Matches("....###", [1, 1, 3]).Should().BeFalse();
    }

    [Fact]
    public void Example2Match()
    {
        Line.Matches(".#...#....###.", [1, 1, 3]).Should().BeTrue();
    }
    [Fact]
    public void Example1()
    {
        var a = new Line("???.### 1,1,3");
        a.Arrangements().Should().Be(1);
    }
    [Fact]
    public void Example2()
    {
        var a = new Line(".??..??...?##. 1,1,3");
        a.Arrangements().Should().Be(4);
    }

    [Fact]
    public void Example3()
    {
        var a = new Line("?#?#?#?#?#?#?#? 1,3,1,6");
        a.Arrangements().Should().Be(1);
    }

    [Fact]
    public void Example4()
    {
        var a = new Line("????.#...#... 4,1,1");
        a.Arrangements().Should().Be(1);
    }
    [Fact]
    public void Example5()
    {
        var a = new Line("????.######..#####. 1,6,5");
        a.Arrangements().Should().Be(4);
    }
    [Fact]
    public void Example6()
    {
        var a = new Line("?###???????? 3,2,1");
        a.Arrangements().Should().Be(10);
    }
}
