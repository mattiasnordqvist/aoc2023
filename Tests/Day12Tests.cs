using AdventOfCode;

using FluentAssertions;

using Xunit;

namespace Tests;

public class Day12Tests
{

    [Fact]
    public void Example1Match()
    {
        Line.IsMatch("#.#.###", [1,1,3]).Should().BeTrue();
    }
    [Fact]
    public void Example1NoMatch()
    {
        Line.IsMatch("....###", [1, 1, 3]).Should().BeFalse();
    }

    [Fact]
    public void Example2Match()
    {
        Line.IsMatch(".#...#....###.", [1, 1, 3]).Should().BeTrue();
    }
    [Fact]
    public void Example1()
    {
        var a = new Line("???.### 1,1,3");
        a.CountArrangements().Should().Be(1);
    }
    [Fact]
    public void Example2()
    {
        var a = new Line(".??..??...?##. 1,1,3");
        a.CountArrangements().Should().Be(4);
    }

    [Fact]
    public void Example3()
    {
        var a = new Line("?#?#?#?#?#?#?#? 1,3,1,6");
        a.CountArrangements().Should().Be(1);
    }

    [Fact]
    public void Example4()
    {
        var a = new Line("????.#...#... 4,1,1");
        a.CountArrangements().Should().Be(1);
    }
    [Fact]
    public void Example5()
    {
        var a = new Line("????.######..#####. 1,6,5");
        a.CountArrangements().Should().Be(4);
    }
    [Fact]
    public void Example6()
    {
        var a = new Line("?###???????? 3,2,1");
        a.CountArrangements().Should().Be(10);
    }
}
