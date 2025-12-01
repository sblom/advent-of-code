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

int c2 = 0;

foreach (var line in lines)
{
    var (cmd, val) = line.Extract<(string, int)>(@"^(.)(\d+)$");

    c2 += val / 100;
    val = val % 100;

    if (cmd == "R")
    {
        if (d + val >= 100)
        {
            c2++;
        }
        d = (d + val) % 100;
    }
    else
    {
        if (d > 0 && d - val <= 0)
        {
            c2++;
        }
        d = (100 + d - val) % 100;
    }
}

WriteLine(c2);