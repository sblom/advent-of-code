<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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
var lines = @"2333133121414131402".GetLines();
#endif

var line = lines.Single();

Queue<int> freelist = new();
Dictionary<int,int> files = new();

int loc = 0;
int fnum = 0;

for (int i = 0; i < line.Length; i++)
{
    var flen = line[i] - '0';
    for (int j = 0; j < flen; j++)
    {
        files[loc++] = fnum;
    }
    fnum++;
    
    i++;
    if (i == line.Length) break;
    
    flen = line[i] - '0';
    for (int j = 0; j < flen; j++)
    {
        freelist.Enqueue(loc++);
    }
}

freelist.Dump();

while (freelist.Peek() < files.Max(x => x.Key))
{
    var end = files.Max(x => x.Key);
    files[freelist.First()] = files[end];
    files.Remove(end);
    freelist.Dequeue();
}

files.Sum(x => (long)x.Key * (long)x.Value).Dump();

loc = 0;
fnum = 0;

Dictionary<int, List<(int start, int len)>> freeextents = new();
Dictionary<int,(int start, int len)> fileextents = new();

for (int i = 1; i <= 9; i++)
{
    freeextents[i] = new();
}

for (int i = 0; i < line.Length; i++)
{
    var flen = line[i] - '0';
    fileextents[fnum++] = (loc, flen);
    loc += flen;
    
    i++;
    if (i == line.Length) break;

    flen = line[i] - '0';
    if (flen > 0) freeextents[flen].Add((loc, flen));
    loc += flen;
}

//freeextents.Dump();

for (fnum--; fnum >= 0; fnum--)
{
    var targetSize = freeextents.Where(x => x.Key >= fileextents[fnum].len && x.Value.Any())
    .OrderBy(x => x.Value.First().start)
    .FirstOrDefault().Key;
    if (targetSize == 0) continue;
    var targetExtent = freeextents[targetSize][0];
    var len = fileextents[fnum].len;
    if (targetExtent.start > fileextents[fnum].start) continue;
    fileextents[fnum] = (targetExtent.start,len);
    var newextent = (start: targetExtent.start + len, len: targetExtent.len - len);

    if (newextent.len > 0)
    {
        int i = 0;
        for (i = 0; i < freeextents[newextent.len].Count && freeextents[newextent.len][i].start < newextent.start; i++)
        {
        }
        //(i,freeextents[newextent.len].Take(10),newextent.start).Dump();
        freeextents[newextent.Item2].Insert(i, newextent);
        //freeextents.Dump();
    }
    freeextents[targetSize].RemoveAt(0);
}

fileextents.Sum(x => (long)x.Key * (long)Enumerable.Range(x.Value.start, x.Value.len).Sum()).Dump();
// Wrong: 8409764614019