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

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"199
200
208
210
200
207
240
269
260
263".GetLines();
#endif

var nums = lines.Select(int.Parse).ToList();

nums.Zip(nums.Skip(1)).Where(x => x.First < x.Second).Count().Dump();

var windows = nums.Zip(nums.Skip(1), nums.Skip(2)).Select(x => x.First + x.Second + x.Third).ToList();

windows.Zip(windows.Skip(1)).Where(x => x.First < x.Second).Count().Dump();

