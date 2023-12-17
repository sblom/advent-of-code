<Query Kind="Statements">
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>RegExtract</Namespace>
</Query>

var input = "R3, R1, R4, L4, R3, R1, R1, L3, L5, L5, L3, R1, R4, L2, L1, R3, L3, R2, R1, R1, L5, L2, L1, R2, L4, R1, L2, L4, R2, R2, L2, L4, L3, R1, R4, R3, L1, R1, L5, R4, L2, R185, L2, R4, R49, L3, L4, R5, R1, R1, L1, L1, R2, L1, L4, R4, R5, R4, L3, L5, R1, R71, L1, R1, R186, L5, L2, R5, R4, R1, L5, L2, R3, R2, R5, R5, R4, R1, R4, R2, L1, R4, L1, L4, L5, L4, R4, R5, R1, L2, L4, L1, L5, L3, L5, R2, L5, R4, L4, R3, R3, R1, R4, L1, L2, R2, L1, R4, R2, R2, R5, R2, R5, L1, R1, L4, R5, R4, R2, R4, L5, R3, R2, R5, R3, L3, L5, L4, L3, L2, L2, R3, R2, L1, L1, L5, R1, L3, R3, R4, R5, L3, L5, R1, L3, L5, L5, L2, R1, L3, L1, L3, R4, L1, R3, L2, L2, R3, R3, R4, R4, R1, L4, R1, L5";
var list = input.Split(',').Select(x => x.Trim());

Func<ValueTuple<int,int>,ValueTuple<int,int>> R = (t) => (t.Item2, -t.Item1);
Func<ValueTuple<int,int>,ValueTuple<int,int>> L = (t) => (-t.Item2, t.Item1);

var loc = (x: 0, y: 0);
var dir = (dx: -1, dy: 0);

var locs = new List<ValueTuple<int,int>>();

foreach (string item in list)
{
    int.TryParse(item.Substring(1), out var dist);
    
    if (item[0] == 'L') dir = L(dir);
    if (item[0] == 'R') dir = R(dir);

    for (int i = dist; i > 0; --i)
    {
        loc = (loc.x + dir.dx, loc.y + dir.dy);

        if (locs.Contains(loc))
        {
            goto done;
        }
        
        locs.Add(loc);
    }
}

done:

(Math.Abs(loc.Item1) + Math.Abs(loc.Item2)).Dump();