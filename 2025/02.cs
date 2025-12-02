#!/bin/env dotnet
#:package RegExtract@*

using static System.Console;

using RegExtract;

var lines = File.ReadAllLines("02.txt").ToList();

var line = lines.First();

var nums = line.Extract<List<(long,long)>>(@"((\d+)-(\d+),?)+") ?? new List<(long, long)>();

long c = 0;

foreach (var (min, max) in nums)
{
    for (long i = min; i <= max; i++)
    {
        var s = i.ToString();
        if (s[0..(s.Length / 2)] == s[(s.Length / 2)..])
        {
            c += i;
        }
    }
}

WriteLine(c);
