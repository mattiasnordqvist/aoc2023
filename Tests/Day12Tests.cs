using AdventOfCode;

using FluentAssertions;

using Xunit;

namespace Tests;

public class Day12Tests
{
    [Theory]
    [InlineData("1,1,3", "#.#.###", "", "")]
    [InlineData("1,1,3,2", "#.#.###", "2", "")]
    [InlineData("2", "", "2", "")]
    [InlineData("1,1", "#.#", "", "")]
    [InlineData("1", ".", "1", "")]
    [InlineData("1,1,3", "#.#.###.", "", "")]
    [InlineData("2,1,3", "#.#.###", "2,1,3", "#.#.###")]
    [InlineData("1,2,3", "#.#.###", "2,3", "#.###")]
    [InlineData("1,1,3", "....###", "1,1,3", "###")]
    [InlineData("1,1,3", ".#...#....###.", "", "")]
    [InlineData("1,1,3", "???.###", "1,1,3", "???.###")]
    [InlineData("1,1,3", ".??..??...?##.", "1,1,3", "??..??...?##.")]
    [InlineData("1,1,3", ".#..??...?##.", "1,3", "??...?##.")]
    [InlineData("2,1,3", ".#..??...?##.", "2,1,3", "#..??...?##.")]
    [InlineData("2,1,3", "##..#...###.", "", "")]
    [InlineData("2,1,3", "##..#...?##.", "3", "?##.")]
    [InlineData("3", ".##.", "3", "##.")]
    [InlineData("3", "###.", "", "")]
    [InlineData("3", "#", "3", "#")]
    [InlineData("", ".", "", "")]
    public void EatTests(string groups, string text, string remainingGroups, string remainingText)
    {
        var (remG, remT) = Day12.EatWithCache(groups.Zplit(",").ia(), text);
        string.Join(",", remG).Should().Be(remainingGroups);
        remT.Should().Be(remainingText);
    }

    [Theory]
    [InlineData("#. 1",1)]
    [InlineData(".# 1",1)]
    [InlineData("# 1",1)]
    [InlineData(".# 2",0)]
    [InlineData("#. 2",0)]
    [InlineData("## 2",1)]

    [InlineData("#? 1", 1)]
    [InlineData("?# 1", 1)]
    [InlineData("??? 1", 3)]

    public void CountArrangementsTests(string input, long count)
    {
        new Line(input).CountArrangements().Should().Be(count);
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
