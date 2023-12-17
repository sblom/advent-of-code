<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20".GetLines();
#endif

var (hx, hy) = (0, 0);
var (tx, ty) = (0, 0);

HashSet<(int,int)> tailVisits = new();

foreach (var move in lines)
{
    var instr = move.Split(" ");
    var dir = instr[0][0];
    var dist = int.Parse(instr[1]);
    
    var (dx, dy) = dir switch {
        'U' => (0, -1),
        'D' => (0, 1),
        'L' => (-1, 0),
        'R' => (1, 0)
    };
    
    for (int i = 0; i < dist; i++)
    {
        (hx, hy) = (hx + dx, hy + dy);

        switch ((tx - hx, ty - hy))
        {
            case (2, _): tx = hx + 1; ty = hy;
                break;
            case (-2, _): tx = hx - 1; ty = hy;
                break;
            case (_, 2): tx = hx; ty = hy + 1;
                break;
            case (_, -2): tx = hx; ty = hy -1;
                break;
            
        }
        
        tailVisits.Add((tx,ty));
    }
}

tailVisits.Count().Dump("Part 1");

tailVisits.Clear();
int[] kxs = new int[10];
int[] kys = new int[10];

foreach (var move in lines)
{
    var instr = move.Split(" ");
    var dir = instr[0][0];
    var dist = int.Parse(instr[1]);

    var (dx, dy) = dir switch
    {
        'U' => (0, -1),
        'D' => (0, 1),
        'L' => (-1, 0),
        'R' => (1, 0)
    };

    for (int i = 0; i < dist; i++)
    {
        kxs[0] = kxs[0] + dx;
        kys[0] = kys[0] + dy;
        
        //(kxs[0], kys[0]).ToString().Dump();
        
        for (int k = 1; k < 10; k++)
        {
            switch ((kxs[k] - kxs[k - 1], kys[k] - kys[k - 1]))
            {
                case (2, int d):
                    kxs[k] = kxs[k] - 1; kys[k] = kys[k] - Math.Sign(d);
                    break;
                case (-2, int d):
                    kxs[k] = kxs[k] + 1; kys[k] = kys[k] - Math.Sign(d);
                    break;
                case (int d, 2):
                    kys[k] = kys[k] - 1; kxs[k] = kxs[k] - Math.Sign(d);
                    break;
                case (int d, -2):
                    kys[k] = kys[k] + 1; kxs[k] = kxs[k] - Math.Sign(d);
                    break;

            }
            
            //(kxs[k], kys[k]).ToString().Dump();
        }
        
        tailVisits.Add((kxs[9], kys[9]));
        //Console.WriteLine();
    }
}

tailVisits.Count().Dump();