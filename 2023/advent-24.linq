<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <NuGetReference>Z3.Linq</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
  <Namespace>Z3.Linq</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
//#load "..\Lib\BFS"

//#define TEST
//#define CHECKED
//#define TRACE

#if !TEST
var lines = await AoC.GetLinesWeb();
long L = 200000000000000, H = 400000000000000;
#else
var lines = @"19, 13, 30 @ -2,  1, -2
18, 19, 22 @ -1, -1, -2
20, 25, 34 @ -2, -2, -4
12, 31, 28 @ -1, -2, -1
20, 19, 15 @  1, -5, -3".GetLines();

long L = 7, H = 27;
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] { new(), new() };
Util.HorizontalRun("Part 1,Part 2", AoC._outputs).Dump();

#endregion

(double xi, double yi, double ab, double xy)? FindIntersection((double x0, double y0, double x1, double y1, double a0, double b0, double a1, double b1) segs)
{
    var (x0, y0, x1, y1, a0, b0, a1, b1) = segs;
    // four endpoints are x0, y0 & x1,y1 & a0,b0 & a1,b1
    // returned values xy and ab are the fractional distance along xy and ab
    // and are only defined when the result is true

    double denominator = (x0 - x1) * (b0 - b1) - (y0 - y1) * (a0 - a1);

    if (denominator == 0)
    {
        // Lines are parallel, or coincident
        return null;
    }

    double intersectionX = ((x0 * y1 - y0 * x1) * (a0 - a1) - (x0 - x1) * (a0 * b1 - b0 * a1)) / denominator;
    double intersectionY = ((x0 * y1 - y0 * x1) * (b0 - b1) - (y0 - y1) * (a0 * b1 - b0 * a1)) / denominator;

    // Check if the intersection point is within the bounds of both line segments
    if (L <= intersectionX && intersectionX <= H)
    if (L <= intersectionY && intersectionY <= H)    
    if (Min(x0, x1) <= intersectionX && intersectionX <= Max(x0, x1))
    if (Min(y0,y1) <= intersectionY && intersectionY <= Max(y0,y1))
    if (Min(a0,a1) <= intersectionX && intersectionX <= Max(a0,a1))
    if (Min(b0,b1) <= intersectionY && intersectionY <= Max(b0,b1))
    {
        return (intersectionX, intersectionY, 0, 0);
    }

    return null;
}

var stones = lines.Extract<(long x, long y, long z, long vx, long vy, long vz)>(@"(\d+), +(\d+), +(\d+) +@ +(-?\d+), +(-?\d+), +(-?\d+)").ToList();

StringBuilder mathematica = new();
mathematica.Append("Solve[");
int count = 3;
foreach (var (stone,i) in stones.Select((x,i) => (x,i)))
{
    mathematica.Append($"{{{stone.x}, {stone.y}, {stone.z}}} + t{i} * {{{stone.vx}, {stone.vy}, {stone.vz}}} == {{x,y,z}} + t{i} * {{vx,vy,vz}}");
    if (--count == 0) break;
    else mathematica.Append("&&");
}
mathematica.Append(",{x,y,z,vx,vy,vz},Integers];x + y + z /. results");
mathematica.ToString().Dump();

using (var ctx = new Z3Context())
{
    var theorem = from t in ctx.NewTheorem<(long x, long y, long z, long vx, long vy, long vz, long t1, long t2, long t3)>()
                  where 312839808368697L + -34 * t.t1 == t.x + t.t1 * t.vx
                  where 484657929662796L + -84 * t.t1 == t.y + t.t1 * t.vy
                  where 311370704669898L + 26 * t.t1 == t.z + t.t1 * t.vz
                  where 404183206681040L + -166 * t.t2 == t.x + t.t2 * t.vx
                  where 437508574234990L + -130 * t.t2 == t.y + t.t2 * t.vy
                  where 324857613500219L + -10 * t.t2 == t.z + t.t2 * t.vz
                  where 301278417732710L + -21 * t.t3 == t.x + t.t3 * t.vx
                  where 383208525587490L + 39 * t.t3 == t.y + t.t3 * t.vy
                  where 324567902247773L + 15 * t.t3 == t.z + t.t3 * t.vz
                  select t;

    var solution = theorem.Solve();
    (solution.x + solution.y + solution.z).Dump2();
}

var segments = stones.Select(stone => (stone.x, stone.y, segment: SegmentInBox(stone))).Where(seg => seg.Item3 != null);

var intersections = ((from s1 in segments
from s2 in segments
where s1.segment != null && s2.segment != null // && s1 != s2
select FindIntersection((s1.segment?.x0 ?? double.NaN, s1.segment?.y0 ?? double.NaN, s1.segment?.x1 ?? double.NaN, s1.segment?.y1 ?? double.NaN, s2.segment?.x0 ?? double.NaN, s2.segment?.y0 ?? double.NaN, s2.segment?.x1 ?? double.NaN, s2.segment?.y1 ?? double.NaN))).Where(x => x?.Item1 != null).Count() / 2).Dump1();

(double x0, double y0, double x1, double y1)? SegmentInBox((long x, long y, long z, long vx, long vy, long vz) line)
{
    var (x, y, z, vx, vy, vz) = line;

    var tleft = 1.0 * (L - x) / vx;
    var yleft = y + tleft * vy;
    
    var tright = 1.0 * (H - x) / vx;
    var yright = y + tright * vy;
    
    var tbottom = 1.0 * (L - y) / vy;
    var xbottom = x + tbottom * vx;
    
    var ttop = 1.0 * (H - y) / vy;
    var xtop = x + ttop * vx;
    
    var endpoints = new List<(double, double)>();
    
    if (L <= x && x <= H && L <= y && y <= H)
        endpoints.Add((x,y));
    
    if (tleft >= 0 && yleft >= L && yleft <= H)
        endpoints.Add((L, yleft));
    if (tright >= 0 && yright >= L && yright <= H)
        endpoints.Add((H, yright));
    if (tbottom >= 0 && xbottom >= L && xbottom <= H)
        endpoints.Add((xbottom, L));
    if (ttop >= 0 && xtop >= L && xtop <= H)
        endpoints.Add((xtop, H));
        
    if (endpoints.Count != 2)
    {
        if (endpoints.Count != 0) endpoints.Count.Dump();
        return null;
    }
    else
    {
        return (endpoints[0].Item1, endpoints[0].Item2, endpoints[1].Item1, endpoints[1].Item2);
    }
}

#if CHECKED
}
#endif