<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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
    var d1 = string.Join("", nums.First(),nums.Last());
    t += int.Parse(d1);
}

t.Dump();

t = 0;

foreach (var line in lines)
{
    var tmp = line;
    
    string[] numerals = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
    string[] values = ["o1e","t2o","t3e","f4r","f5e","s6x","s7n","e8t","n9e"];
    
    for (int i = 0; i < numerals.Length; i++)
    {
        tmp = tmp.Replace(numerals[i],values[i]);
    }
    
    var nums = tmp.Where(ch => char.IsNumber(ch));
    var d1 = string.Join("", nums.First(), nums.Last());
    t += int.Parse(d1);
}
t.Dump();