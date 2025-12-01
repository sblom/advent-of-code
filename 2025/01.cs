#!/bin/env dotnet
#:package RegExtract@*

using static System.Console;

using RegExtract;

var lines = File.ReadAllLines("01.txt").ToList();

var d = 50;
var c = 0;

foreach (var line in lines)
{
    var (cmd, val) = line.Extract<(string, int)>(@"(.)(\d+)");
    
    d = cmd == "R" ? (d + val) % 100 : (100 + d - val) % 100;
    if (d == 0) c++;
}

WriteLine(c);

d = 50; c = 0;

foreach (var line in lines)
{
    var (cmd, val) = line.Extract<(string, int)>(@"^(.)(\d+)");

    if (cmd == "R")
    {
        for (int i = 0; i < val; i++)
        {
            d = (d + 1) % 100;
            if (d == 0) c++;
        }
    }
    else
    {
        for (int i = 0; i < val; i++)
        {
            d = (100 + d - 1) % 100;
            if (d == 0) c++;
        }
    }
}

WriteLine(c);