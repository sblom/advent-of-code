<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST
//#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = """
...#......
.......#..
\#.........
..........
......#...
.#........
.........#
..........
.......#..
\#...#.....
""".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

var galaxy = lines.Select(x => x.ToList()).ToList();

for (int i = 0; i < galaxy.Count; i++)
{
    if (!galaxy[i].Any(ch => ch != '.'))
    {
        galaxy.Insert(i, galaxy[i].ToList());
        i++;
    }
}

for (int i = 0; i < galaxy[0].Count; i++)
{
    for (int j = 0; j < galaxy.Count; j++)
    {
        if (galaxy[j][i] != '.') goto next_col;
    }
    
    for (int j = 0; j < galaxy.Count; j++)
    {
        galaxy[j].Insert(i, '.');
    }
    i++;
next_col:;
}

HashSet<(int x, int y)> galaxies = new();

for (int i = 0; i < galaxy.Count; i++)
{
    for (int j = 0; j < galaxy[0].Count; j++)
    {
        if (galaxy[i][j] == '#') galaxies.Add((j,i));
    }
}

((from g1 in galaxies
from g2 in galaxies
select (Math.Abs(g1.x - g2.x) + Math.Abs(g1.y - g2.y))).Sum()/2).Dump("Part 1");

galaxy = lines.Select(x => x.ToList()).ToList();
galaxies.Clear();
for (int i = 0; i < galaxy.Count; i++)
{
    for (int j = 0; j < galaxy[0].Count; j++)
    {
        if (galaxy[i][j] == '#') galaxies.Add((j, i));
    }
}

int loc = galaxies.Max(g => g.x);
while (loc > 0)
{
    loc--;
    if (!galaxies.Any(g => g.x == loc))
    {
        galaxies = galaxies.Select(g => g.x > loc ? (g.x + 999_999, g.y): (g.x, g.y)).ToHashSet();
    }
}

loc = galaxies.Max(g => g.y);
while (loc > 0)
{
    loc--;
    if (!galaxies.Any(g => g.y == loc))
    {
        galaxies = galaxies.Select(g => g.y > loc ? (g.x, g.y + 999_999) : (g.x, g.y)).ToHashSet();
    }
}

((from g1 in galaxies
  from g2 in galaxies
  select (Math.Abs((long)g1.x - g2.x) + Math.Abs((long)g1.y - g2.y))).Sum() / 2).Dump("Part 2");

#if CHECKED
}
#endif
