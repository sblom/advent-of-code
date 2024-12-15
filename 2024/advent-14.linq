<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#LINQPad checked+

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3".GetLines();
#endif

const int w = 101, h = 103;

char[,] grid = new char[h,w];

var robots = new Queue<((int x,int y)p,(int x,int y)v)>(lines.Extract<((int x,int y) p,(int x, int y) v)>(@"p=((\d+),(\d+)) v=((-?\d+),(-?\d+))"));

var newqueue = new Queue<((int,int),(int,int))>();


var minfact = int.MaxValue;

for (int r = 0; ; r++)
{
    while (robots.Any())
    {
        var robot = robots.Dequeue();
        newqueue.Enqueue((((robot.p.x + robot.v.x + w) % w,(robot.p.y + robot.v.y + h) % h),robot.v));
    }
    
    (robots, newqueue) = (newqueue, robots);
    
    //for (int y = 0; y < h; y++)
    //{
    //    for (int x = 0; x < w; x++)
    //    {
    //        if (robots.Any(r => r.p.x == x && r.p.y == y)) grid[y,x] = '*';
    //        else grid[y,x] = '.';
    //    }
    //}
    //var fact = robots.Where(r => r.p.x < w / 2 && r.p.y < h / 2).Count() * robots.Where(r => r.p.x > w / 2 && r.p.y < h / 2).Count();
    
    var robash = robots.Select(x => x.p).ToHashSet();
    
    int fact = 0;

//    for (int y = 0; y < h / 2; y++)
//    {
//        for (int x = 0; x < (w - y) / 2; x++)
//        {
//            if (robash.Contains((x,y))) fact++;
//        }
//
//        for (int x = 0; x < (w + y) / 2; x++)
//        {
//            if (robash.Contains((x,y))) fact++;
//        }
//    }

    if (robash.Count() == robots.Count())
    {
        var sb = new StringBuilder();
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                if (robash.Contains((x,y))) sb.Append('*');
                else sb.Append('.');
                sb.Append(' ');
            }
            sb.Append('\n');
        }
        sb.ToString().DumpFixed();
        r.Dump();
        minfact = fact;
    }
}

(robots.Where(r => r.p.x < w / 2 && r.p.y < h / 2).Count().Dump() * robots.Where(r => r.p.x > w / 2 && r.p.y < h / 2).Count().Dump() * robots.Where(r => r.p.x < w / 2 && r.p.y > h / 2).Count().Dump() * robots.Where(r => r.p.x > w / 2 && r.p.y > h / 2).Count().Dump()).Dump();