<Query Kind="Statements" />

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"125 17".GetLines();
#endif

Dictionary<long,(int,LinkedList<long>)> cache = new();

var nums = new LinkedList<long>(lines.Single().Split(" ").Select(x => long.Parse(x)));

var outernode = nums.First;
while (outernode != null)
{
    if (cache.ContainsKey(outernode.Value)) goto next;
    nums = new LinkedList<long>();
    nums.AddFirst(outernode.Value);
    for (int r = 1; r <= 25; r++)
    {
        var node = nums.First;
        while (node != null)
        {
            if (node.Value == 0) node.Value = 1;
            else if (node.Value.ToString().Length % 2 == 0)
            {
                var tmp = node.Value.ToString();
                node.Value = long.Parse(tmp[0..(tmp.Length / 2)]);
                nums.AddAfter(node, long.Parse(tmp[(tmp.Length / 2)..]));
                node = node.Next;
            }
            else
            {
                node.Value = (node.Value * 2024);
            }
            node = node.Next;
        }
        //(r, nums.Count).Dump();
    }
    cache[outernode.Value] = (nums.Count, nums);

next: outernode = outernode.Next;
}

foreach (var v in cache.Values.ToList())
{
    outernode = v.Item2.First;
    while (outernode != null)
    {
        if (cache.ContainsKey(outernode.Value)) goto next;
        nums = new LinkedList<long>();
        nums.AddFirst(outernode.Value);
        for (int r = 1; r <= 25; r++)
        {
            var node = nums.First;
            while (node != null)
            {
                if (node.Value == 0) node.Value = 1;
                else if (node.Value.ToString().Length % 2 == 0)
                {
                    var tmp = node.Value.ToString();
                    node.Value = long.Parse(tmp[0..(tmp.Length / 2)]);
                    nums.AddAfter(node, long.Parse(tmp[(tmp.Length / 2)..]));
                    node = node.Next;
                }
                else
                {
                    node.Value = (node.Value * 2024);
                }
                node = node.Next;
            }
            //(r, nums.Count).Dump();
        }
        cache[outernode.Value] = (nums.Count, nums);

    next: outernode = outernode.Next;
    }
}

foreach (var v in cache.Values.ToList())
{
    outernode = v.Item2.First;
    while (outernode != null)
    {
        if (cache.ContainsKey(outernode.Value)) goto next;
        nums = new LinkedList<long>();
        nums.AddFirst(outernode.Value);
        for (int r = 1; r <= 25; r++)
        {
            var node = nums.First;
            while (node != null)
            {
                if (node.Value == 0) node.Value = 1;
                else if (node.Value.ToString().Length % 2 == 0)
                {
                    var tmp = node.Value.ToString();
                    node.Value = long.Parse(tmp[0..(tmp.Length / 2)]);
                    nums.AddAfter(node, long.Parse(tmp[(tmp.Length / 2)..]));
                    node = node.Next;
                }
                else
                {
                    node.Value = (node.Value * 2024);
                }
                node = node.Next;
            }
            //(r, nums.Count).Dump();
        }
        cache[outernode.Value] = (nums.Count, nums);

    next: outernode = outernode.Next;
    }
}

var sizes = new Dictionary<(long, int), long>();

long GetSize(long num, int layers)
{
    if (sizes.ContainsKey((num, layers))) return sizes[(num, layers)];
    if (layers == 0) return 1;
    return sizes[(num, layers)] = cache[num].Item2.Sum(x => GetSize(x, layers - 1));
}

nums = new LinkedList<long>(lines.Single().Split(" ").Select(x => long.Parse(x)));

nums.Sum(x => GetSize(x, 3)).Dump();