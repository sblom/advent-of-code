#LINQPad checked+

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = $@"1
2
3
2024".Replace("\\", "").GetLines();
    //   2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0

#endif

var groups = lines.GroupLines();

var ranges = groups.First().Extract<(long,long)>(@"(\d+)-(\d+)").ToList();

var ingredients = groups.Skip(1).First().Extract<long>(@"\d+");

int c = 0;

foreach (var ing in ingredients)
{
    foreach (var range in ranges)
    {
        if (ing >= range.Item1 && ing <= range.Item2)
        {
            c++;
            break;
        }
    }
}

c.Dump();

long t = 0;

for (int i = 0; i < ranges.Count(); i++)
{
    List<(long, long)> distinct = new();
    distinct.Add(ranges[i]);
    for (int j = 0; j < i; j++)
    {
        var r2 = ranges[j];

        var newList = new List<(long, long)>();
        foreach (var d in distinct)
        {
            if (r2.Item2 < d.Item1 || r2.Item1 > d.Item2) // no overlap
            {
                newList.Add(d);
            }
            else // overlap
            {
                if (r2.Item1 <= d.Item1 && r2.Item2 >= d.Item2) // r2 contains d
                {
                    // d is removed
                }
                else if (d.Item1 <= r2.Item1 && d.Item2 >= r2.Item2) // d contains r2
                {
                    if (r2.Item1 == d.Item1) // r2 starts same as d
                    {
                        newList.Add((r2.Item2 + 1, d.Item2));
                    }
                    else if (r2.Item2 == d.Item2) // r2 ends same as d
                    {
                        newList.Add((d.Item1, r2.Item1 - 1));
                    }
                    else // r2 is in middle of d
                    {
                        newList.Add((d.Item1, r2.Item1 - 1));
                        newList.Add((r2.Item2 + 1, d.Item2));
                    }
                }
                else if (r2.Item1 <= d.Item1 && r2.Item2 < d.Item2) // r2 overlaps d start
                {
                    newList.Add((r2.Item2 + 1, d.Item2));
                }
                else if (r2.Item1 > d.Item1 && r2.Item2 >= d.Item2) // r2 overlaps d end
                {
                    newList.Add((d.Item1, r2.Item1 - 1));
                }
            }
        }
        distinct = newList;
    }
    foreach (var d in distinct)
    {
        t += d.Item2 - d.Item1 + 1;
    }
}

t.Dump();