<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen".GetLines();
#endif

int t = 0;

foreach (var line in lines)
{
    var nums = line.Where(ch => char.IsNumber(ch));
    t += (nums.First() - '0') * 10 + (nums.Last() - '0');
}
t.Dump("Part 1");

t = 0;

foreach (var line in lines)
{
    t += GetDigits(line);
}
t.Dump("Part 2");

int GetDigits(string str)
{
    int first = -1, last = -1;
    
    var span = str.AsSpan();
    int i = 0;
    
    for (i = 0; i < str.Length; i++)
    {
        var digit = GetDigit(span[i..]);
        if (digit is not null)
        {
            first = last = digit.Value;
            i++;
            break;
        }
    }

    for (; i < str.Length; i++)
    {
        var digit = GetDigit(span[i..]);
        if (digit is not null)
        {
            last = digit.Value;
        }
    }


    return first * 10 + last;
}

int? GetDigit(ReadOnlySpan<char> span)
{
    return span switch {
        [var ch,..] when ch is >= '0' and <= '9' => ch - '0',
        ['o', 'n', 'e', ..] => 1,
        ['t', 'w', 'o', ..] => 2,
        ['t', 'h', 'r', 'e', 'e', ..] => 3,
        ['f', 'o', 'u', 'r', ..] => 4,
        ['f', 'i', 'v', 'e', ..] => 5,
        ['s', 'i', 'x', ..] => 6,
        ['s', 'e', 'v', 'e', 'n', ..] => 7,
        ['e', 'i', 'g', 'h', 't', ..] => 8,
        ['n', 'i', 'n', 'e', ..] => 9,
        _ => null        
    };
}
