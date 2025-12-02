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
