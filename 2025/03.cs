#!/bin/env dotnet
#:package RegExtract@*

using static System.Console;

using RegExtract;

var lines = File.ReadAllLines("03.txt").ToList();

var c = 0;

foreach (var line in lines)
{
    var m = 0;
    for (int i = 0; i < line.Length - 1; i++)
    {
        for (int j = i + 1; j < line.Length; j++)
        {
            var v = (line[i] - '0') * 10 + (line[j] - '0');
            if (v > m)
            {
                m = v;
            }
        }
    }
    WriteLine(m);
    c += m;
}

WriteLine(c);

long t = 0;

foreach (var line in lines)
{
    var dp = new Dictionary<int, long>
    {
        [0] = 0,
    };
    for (int i = line.Length - 1; i >= 0; i--)
    {
        for (int j = 12; j >= 0; j--)
        {
            if (dp.ContainsKey(j))
            {
                var v = dp[j];
                var nv = (long)(Math.Pow(10, j)) * (line[i] - '0') + v;
                if (dp.ContainsKey(j + 1))
                {
                    dp[j + 1] = Math.Max(dp[j + 1], nv);
                }
                else
                {
                    dp[j + 1] = nv;
                }
            }
        }
    }

    Console.WriteLine(dp[12]);

    t += dp[12];
}

WriteLine(t);
