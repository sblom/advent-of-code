#LINQPad checked+

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = $@"
.......S.......
...............
.......^.......
...............
......^.^......
...............
.....^.^.^.....
...............
....^.^...^....
...............
...^.^...^.^...
...............
..^...^.....^..
...............
.^.^.^.^.^...^.
...............".Replace("\\", "").GetLines();

#endif

var groups = lines.GroupLines();

var loc = lines.First().Select((c,i) => (c,i)).Where(x => x.Item1 == 'S').First().i;

var L = lines.ToList();

var beams = new HashSet<int>();
beams.Add(loc);

int c = 0;

for (int i = 2; i < L.Count; i += 2)
{
    var newset = new HashSet<int>();
    foreach (var beam in beams)
    {
        if (L[i][beam] == '^')
        {
            c++;
            newset.Add(beam - 1);
            newset.Add(beam + 1);
        }
        else
        {
            newset.Add(beam);
        }
    }
    beams = newset;
}

c.Dump();

var qbeams = new Dictionary<int,long>();
qbeams[loc] = 1;

for (int i = 2; i < L.Count; i += 2)
{
    var qnewset = new Dictionary<int,long>();
    foreach (var (pos, ways) in qbeams)
    {
        if (L[i][pos] == '^')
        {
            if (!qnewset.ContainsKey(pos - 1)) qnewset[pos - 1] = 0;
            if (!qnewset.ContainsKey(pos + 1)) qnewset[pos + 1] = 0;
            qnewset[pos - 1] += ways;
            qnewset[pos + 1] += ways;
        }
        else
        {
            if (!qnewset.ContainsKey(pos)) qnewset[pos] = 0;
            qnewset[pos] += ways;
        }
    }
    qbeams = qnewset;
}

qbeams.Sum(x => x.Value).Dump();