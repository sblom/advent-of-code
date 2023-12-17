<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010".GetLines();
#endif

var nums = lines.Select(line => line.Select(ch => (double)(ch - '0')).ToArray()).Dump();
var c = nums.Count();


Enumerable.Range(0,nums.First().Length).Select(i => nums.Sum(line => line[i]) / c).Dump();

var commonest = "101000101001";
var rarest = "010111010110";

Convert.ToInt32("101000101001",2).Dump();
Convert.ToInt32("010111010110",2).Dump();

(2601 * 1494).Dump1();

var list = lines.ToList();

var listnums = lines.Select(line => line.Select(ch => (double)(ch - '0')).ToArray());

for (int i = 0; i < nums.First().Length; i++)
{
	var listcount = (double)listnums.Count();
	
	var common = Math.Round(listnums.Sum(line => line[i]).Dump() / listcount, MidpointRounding.AwayFromZero).Dump();
	
	listnums = listnums.Where(num => num[i] == common).ToList().Dump();
}

var oxygen = string.Join("",listnums.Single().Select(d => (char)(d + '0'))).Dump();

listnums = lines.Select(line => line.Select(ch => (double)(ch - '0')).ToArray());

for (int i = 0; i < nums.First().Length; i++)
{
	var listcount = listnums.Count();

	var common = 1.0 - Math.Round(listnums.Sum(line => line[i]) / listcount, MidpointRounding.AwayFromZero);

	listnums = listnums.Where(num => num[i] == common).ToList();
	
	if (listnums.Count() == 1) break;
}

var co2 = string.Join("", listnums.Single().Select(d => (char)(d + '0'))).Dump();

(Convert.ToInt32("111010111111",2) * Convert.ToInt32("010010000111",2)).Dump2();