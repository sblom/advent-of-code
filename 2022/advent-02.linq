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
var lines = @"".GetLines();
#endif

var score = 0;

foreach (var round in lines)
{
    var them = round[0];
    var me = round[2];
    
    if (them == me) score += 3;
    else
    score += them switch {
        'A' => me switch {
            'X' => 3,
            'Y' => 6,
            'Z' => 0
        },
        'B' => me switch {
            'X' => 0,
            'Y' => 3,
            'Z' => 6
        },
        'C' => me switch {
            'X' => 6,
            'Y' => 0,
            'Z' => 3
        }        
    };
    
    score += me switch {
        'X' => 1,
        'Y' => 2,
        'Z' => 3
    };
}

score.Dump();

score = 0;

foreach (var round in lines)
{
    var them = round[0];
    var me = round[2];

    score += them switch
    {
        'A' => me switch
        {
            'X' => 0 + 3,
            'Y' => 3 + 1,
            'Z' => 6 + 2
        },
        'B' => me switch
        {
            'X' => 0 + 1,
            'Y' => 3 + 2,
            'Z' => 6 + 3
        },
        'C' => me switch
        {
            'X' => 0 + 2,
            'Y' => 3 + 3,
            'Z' => 6 + 1
        }
    };
}

score.Dump();