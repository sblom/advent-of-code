#!/bin/env dotnet
#:package RegExtract@*

using static System.Console;

using RegExtract;

var lines = File.ReadAllLines("02.txt").ToList();

var line = lines.First();

var nums = line.Extract<List<(long,long)>>(@"((\d+)-(\d+),?)+") ?? new List<(long, long)>();

long c = 0, c2 = 0;

foreach (var (min, max) in nums)
{
    for (long i = min; i <= max; i++)
    {
        var s = i.ToString();
        
        for (int j = 1; j <= s.Length / 2; j++)
        {
            if (s.Length % j == 0)
            {
                var chunks = s.Chunk(j).Select(x => new string(x)).ToList();

                if (chunks.Distinct().Count() == 1)
                {
                    c2 += i;
                    break;
                }
            }
        }

        if (s[0..(s.Length / 2)] == s[(s.Length / 2)..])
        {
            c += i;
        }
    }
}

WriteLine(c);
WriteLine(c2);
