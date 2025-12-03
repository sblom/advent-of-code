#!/bin/env dotnet
#:package RegExtract@*

using static System.Console;

var lines = File.ReadAllLines(@"04.txt");

var grid = lines.Select(line => line.ToCharArray()).ToArray();

var dirs = new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

var c = 0;

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[0].Length; j++)
    {
        var rolls = 0;
        foreach (var (dx, dy) in dirs)
        {
            try
            {
                if (grid[i + dy][j + dx] == '@') rolls++;
            }
            catch {}
        }
        if (rolls < 4 && grid[i][j] == '@')
        {
            c++;
        }
    }
}

WriteLine(c);
