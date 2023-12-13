<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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
var lines = @"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

var records = lines.Extract<(string, List<int>)>(@"([.?#]+) (?:(\d+),?)+").Select(x => (x.Item1.ToCharArray(), x.Item2.ToArray()));

//int total = 0;
//
//foreach (var record in records)
//{
//    int matches = 0;
//    var c = record.Item1.Where(ch => ch == '?').Count();
//    var ii = record.Item1.Select((ch,i) => (ch,i)).Where(ch => ch.ch == '?').ToArray();
//    
//    for (int i = 0; i < 1 << c; i++)
//    {
//        var t = i;
//        
//        for (int j = 0; j < c; j++)
//        {
//            record.Item1[ii[j].i] = (t % 2 == 1) ? '#' : '.';
//            t /= 2;
//        }
//        
//        //record.Item1.Dump();
//
//        int runlength = 0, idx = 0;
//        bool isRun = false;
//        for (int k = 0; k < record.Item1.Length; k++)
//        {
//            switch (record.Item1[k])
//            {
//                case '#':
//                    if (idx >= record.Item2.Length) goto next_chaff;
//                    runlength++;
//                    isRun = true;
//                    break;
//                case '.':
//                    if (isRun)
//                    {
//                        if (runlength != record.Item2[idx++]) goto next_chaff;
//                        isRun = false;
//                        runlength = 0;
//                    }
//                    break;
//            }
//        }
//        
//        if (isRun && runlength != record.Item2[idx++])
//        {
//            goto next_chaff;
//        }
//        if (idx != record.Item2.Length)
//        {
//            goto next_chaff;
//        }
//        
//        ++matches;
//        
//        next_chaff:;
//    }    
//    matches.Dump();
//    total += matches;
//}
//
//total.Dump("Part 1");

records = lines.Extract<(string, List<int>)>(@"([.?#]+) (?:(\d+),?)+").Select(x => (x.Item1.ToCharArray(), x.Item2.ToArray()));

long total = 0;

Dictionary<(int,int),long> cache = new(5000);

foreach (var record in records)
{
    cache.Clear();
    //record.Dump();
    total += WaysToFill(record.Item1, record.Item2);
}

total.Dump("Part 1");

records = lines.Extract<(string, List<int>)>(@"([.?#]+) (?:(\d+),?)+").Select(x => ((x.Item1 + "?" + x.Item1 + "?" + x.Item1 + "?" + x.Item1 + "?" + x.Item1).ToCharArray(), x.Item2.Concat(x.Item2).Concat(x.Item2).Concat(x.Item2).Concat(x.Item2).ToArray()));

total = 0;

foreach (var record in records)
{
    cache.Clear();
    //record.Dump();
    total += WaysToFill(record.Item1, record.Item2);
}

total.Dump("Part 2");

long WaysToFill(ReadOnlySpan<char> map, ReadOnlySpan<int> chunks)
{        
    var key = (map.Length,chunks.Length);
    if (cache.ContainsKey(key))
    {
        return cache[key];
    }
    
    if (chunks.Length == 0) return !map.Contains('#') ? 1 : 0;
    
    int minlen = 0;
    for (int i = 0; i < chunks.Length; i++)
    {
        minlen += chunks[i];
    }
    minlen += chunks.Length - 1;
    
    long total = 0;
    
    bool final = false;
    
    for (int i = 0; i <= map.Length - minlen; i++)
    {
        if (map[i] == '#') final = true;
        
        for (int j = 0; j < chunks[0]; j++)
        {
            if (map[i + j] is '.') goto next_i;
        }
        
        if (i + chunks[0] < map.Length && map[i + chunks[0]] == '#') goto next_i;
        
        if (chunks.Length > 1 && map[i + chunks[0]] == '#') goto next_i;
        
        if (chunks.Length == 1 && i + chunks[0] == map.Length) total += 1;
        else total += WaysToFill(map[(i + chunks[0] + 1)..], chunks[1..]);
    next_i:;
        if (final) break;
    }
    
    cache[key] = total;
    
    return total;
}

#if CHECKED
}
#endif
