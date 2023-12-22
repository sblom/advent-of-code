<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
//#load "..\Lib\BFS"

//#define TEST
//#define CHECKED
//#define TRACE

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"1,0,1~1,2,1   <- A
0,0,2~2,0,2   <- B
0,2,3~2,2,3   <- C
0,0,4~0,2,4   <- D
2,0,5~2,2,5   <- E
0,1,6~2,1,6   <- F
1,1,8~1,1,9   <- G".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] {new(), new()};
Util.HorizontalRun("Part 1,Part 2",AoC._outputs).Dump();

#endregion

var bricks = lines.Extract<((int x1,int y1,int z1) e1, (int x2,int y2,int z2) e2)>(@"((\d+),(\d+),(\d+))~((\d+),(\d+),(\d+))").ToArray();

var bricks1 = bricks.ToArray();

var xl = bricks.Min(b => Min(b.e1.x1, b.e2.x2));
var xh = bricks.Max(b => Max(b.e1.x1, b.e2.x2));
var yl = bricks.Min(b => Min(b.e1.y1, b.e2.y2));
var yh = bricks.Max(b => Max(b.e1.y1, b.e2.y2));
var zl = bricks.Min(b => Min(b.e1.z1, b.e2.z2));
var zh = bricks.Max(b => Max(b.e1.z1, b.e2.z2));

(xl, xh, yl, yh, zl, zh).Dump();

var cubes = Enumerable.Range(0, zh + 1).Select(x => Enumerable.Range(0, 10).Select(x => Enumerable.Range(0, 10).Select(x => -1).ToArray()).ToArray()).ToArray();

for (var i = 0; i < bricks.Length; i++)
{
    for (int x = bricks[i].e1.x1, y = bricks[i].e1.y1, z = bricks[i].e1.z1; ; x += Sign(bricks[i].e2.x2 - bricks[i].e1.x1), y += Sign(bricks[i].e2.y2 - bricks[i].e1.y1), z += Sign(bricks[i].e2.z2 - bricks[i].e1.z1))
    {
        cubes[z][x][y] = i;
        if (!(x != bricks[i].e2.x2 || y != bricks[i].e2.y2 || z != bricks[i].e2.z2)) break;
    }
}

bool lowered = false;
do
{
    lowered = false;
    HashSet<int> done = new();
    for (int h = 0; h < cubes.Length; h++)
    {
        var next = cubes[h].SelectMany(x => x.Where(y => y != -1)).ToHashSet().Except(done);
        
        foreach (var n in next)
        {
            var f = LowerBlock(n, true, bricks1, cubes);
            if (f > 0)
            {
                //f.Dump();
                lowered = true;
            }
        }
        
        done.UnionWith(next);
    }
} while (lowered);

#if VISUALIZE
for (int i = 9; i >= 0; i--)
{
    StringBuilder sb = new();
    for (int x = 0; x < 3; x++)
    {
        for (int y = 0; y < 3; y++)
        {
            sb.Append(cubes[i][x][y] == -1 ? '.' : (char)('A' + cubes[i][x][y]));
        }
        sb.AppendLine();
    }
    sb.AppendLine();
    sb.ToString().DumpFixed();
}
#endif

int p1 = 0;

for (var i = 0; i < bricks1.Length; i++)
{
    for (int x = bricks1[i].e1.x1, y = bricks1[i].e1.y1, z = bricks1[i].e1.z1; ; x += Sign(bricks1[i].e2.x2 - bricks1[i].e1.x1), y += Sign(bricks1[i].e2.y2 - bricks1[i].e1.y1), z += Sign(bricks1[i].e2.z2 - bricks1[i].e1.z1))
    {
        cubes[z][x][y] = -1;
        if(!(x != bricks1[i].e2.x2 || y != bricks1[i].e2.y2 || z != bricks1[i].e2.z2)) break;
    }

    bool safe = true;

    for (int x = bricks1[i].e1.x1, y = bricks1[i].e1.y1, z = bricks1[i].e1.z1; ; x += Sign(bricks1[i].e2.x2 - bricks1[i].e1.x1), y += Sign(bricks1[i].e2.y2 - bricks1[i].e1.y1), z += Sign(bricks1[i].e2.z2 - bricks1[i].e1.z1))
    {
        if (cubes[z + 1][x][y] != -1)
        {
            if (LowerBlock(cubes[z + 1][x][y], false, bricks1, cubes) > 0)
            {
                safe = false;
                break;
            }
        }
        if (!(x != bricks1[i].e2.x2 || y != bricks1[i].e2.y2 || z != bricks1[i].e2.z2)) break;
    }
    
    if (safe) p1++;

    for (int x = bricks1[i].e1.x1, y = bricks1[i].e1.y1, z = bricks1[i].e1.z1; ; x += Sign(bricks1[i].e2.x2 - bricks1[i].e1.x1), y += Sign(bricks1[i].e2.y2 - bricks1[i].e1.y1), z += Sign(bricks1[i].e2.z2 - bricks1[i].e1.z1))
    {
        cubes[z][x][y] = i;
        if (!(x != bricks1[i].e2.x2 || y != bricks1[i].e2.y2 || z != bricks1[i].e2.z2)) break;
    }
}

p1.Dump1();

#region Part 2

var cubesclone = cubes.Select(x => x.Select(y => y.ToArray()).ToArray()).ToArray();

int p2 = 0;

for (var i = 0; i < bricks1.Length; i++)
{
    var cubes2 = cubesclone.Select(x => x.Select(y => y.ToArray()).ToArray()).ToArray();
    var bricks2 = bricks1.ToArray();
    
    for (int x = bricks2[i].e1.x1, y = bricks2[i].e1.y1, z = bricks2[i].e1.z1; ; x += Sign(bricks2[i].e2.x2 - bricks2[i].e1.x1), y += Sign(bricks2[i].e2.y2 - bricks2[i].e1.y1), z += Sign(bricks2[i].e2.z2 - bricks2[i].e1.z1))
    {
        cubes2[z][x][y] = -1;
        if (!(x != bricks2[i].e2.x2 || y != bricks2[i].e2.y2 || z != bricks2[i].e2.z2)) break;
    }

    HashSet<int> supported = GetSupported(i, bricks2, cubes2).ToHashSet();
    HashSet<int> fallen = new();
    
    do
    {
        lowered = false;

        HashSet<int> next = new();

        foreach (var sup in supported)
        {
            next.UnionWith(GetSupported(sup, bricks2, cubes2));
            var f = LowerBlock(sup, true, bricks2, cubes2);
            if (f > 0)
            {
                fallen.Add(sup);
                lowered = true;
            }
        }
        
        supported.UnionWith(next);
    } while (lowered);
    
    p2 += fallen.Count();
}

p2.Dump2();

#endregion

int LowerBlock(int i, bool actually, ((int x1,int y1, int z1) e1,(int x2,int y2,int z2) e2)[] bricks1, int[][][] cubes)
{
    int hl = 100000;
    
    for (int x = bricks1[i].e1.x1, y = bricks1[i].e1.y1, z = bricks1[i].e1.z1; ; x += Sign(bricks1[i].e2.x2 - bricks1[i].e1.x1), y += Sign(bricks1[i].e2.y2 - bricks1[i].e1.y1), z += Sign(bricks1[i].e2.z2 - bricks1[i].e1.z1))
    {
        for (int h = z;; h--)
        {
            if (h == -1 || (cubes[h][x][y] != -1 && cubes[h][x][y] != i))
            {
                hl = Min(z - h - 1,hl);
                break;
            }
        }
        if (!(x != bricks1[i].e2.x2 || y != bricks1[i].e2.y2 || z != bricks1[i].e2.z2)) break;
    }

    if (actually)
    {
        for (int x = bricks1[i].e1.x1, y = bricks1[i].e1.y1, z = bricks1[i].e1.z1; ; x += Sign(bricks1[i].e2.x2 - bricks1[i].e1.x1), y += Sign(bricks1[i].e2.y2 - bricks1[i].e1.y1), z += Sign(bricks1[i].e2.z2 - bricks1[i].e1.z1))
        {
            cubes[z][x][y] = -1;
            if (!(x != bricks1[i].e2.x2 || y != bricks1[i].e2.y2 || z != bricks1[i].e2.z2)) break;
        }

        bricks1[i] = (bricks1[i].e1 with { z1 = bricks1[i].e1.z1 - hl }, bricks1[i].e2 with { z2 = bricks1[i].e2.z2 - hl });

        for (int x = bricks1[i].e1.x1, y = bricks1[i].e1.y1, z = bricks1[i].e1.z1; ; x += Sign(bricks1[i].e2.x2 - bricks1[i].e1.x1), y += Sign(bricks1[i].e2.y2 - bricks1[i].e1.y1), z += Sign(bricks1[i].e2.z2 - bricks1[i].e1.z1))
        {
            cubes[z][x][y] = i;
            if (!(x != bricks1[i].e2.x2 || y != bricks1[i].e2.y2 || z != bricks1[i].e2.z2)) break;
        }
    }

    return hl;
}

IEnumerable<int> GetSupported(int i, ((int x1,int y1, int z1) e1,(int x2,int y2,int z2) e2)[] bricks, int[][][] cubes)
{
    for (int x = bricks[i].e1.x1, y = bricks[i].e1.y1, z = bricks[i].e1.z1; ; x += Sign(bricks[i].e2.x2 - bricks[i].e1.x1), y += Sign(bricks[i].e2.y2 - bricks[i].e1.y1), z += Sign(bricks[i].e2.z2 - bricks[i].e1.z1))
    {
        if (cubes[z + 1][x][y] != -1)
        {
                yield return cubes[z+1][x][y];
        }
        if (!(x != bricks[i].e2.x2 || y != bricks[i].e2.y2 || z != bricks[i].e2.z2)) break;
    }

}

#if CHECKED
}
#endif
