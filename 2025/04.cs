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

c = 0;
var r = 1;

while (r > 0)
{
    r = 0;
    var newGrid = grid.Select(x => x.ToArray()).ToArray();
    
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
                catch { }
            }
            if (rolls < 4 && grid[i][j] == '@')
            {
                c++;
                r++;
                newGrid[i][j] = '.';
            }
        }
    }
    grid = newGrid;
}

// for (int i = 0; i < grid.Length; i++)
// {
//     Console.WriteLine(string.Join("", grid[i]));
// }

WriteLine(c);
