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
var lines = @"zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw".GetLines();
#endif

Stack<Dir> loc = new();
Dir root;
loc.Push(root = new Dir("/", new(), new()));

bool dirMode = false;

foreach (var line in lines.Skip(1))
{
    if (dirMode)
    {        
        if (line.StartsWith("$")) {
            dirMode = false;
        }
        else
        {
            if (line.StartsWith("dir"))
            {
                loc.Peek().dirs[line[4..]] = new Dir(line[4..],new(),new());
            }
            else
            {                
                loc.Peek().files.Add(line.Extract<(int,string)>(@"(\d+) (\w+)"));
            }
        }
    }
    
    if (line == "$ ls")
    {
        dirMode = true;
        continue;
    }
    else if (line.StartsWith("$ cd "))
    {
        if (line[5..] == "..") loc.Pop();
        else
        {
            loc.Push(loc.Peek().dirs[line[5..]]);
        }
    }
}

List<(int size, Dir dir)> smalldirs = new();
List<(int size, Dir dir)> alldirs = new();

int GoDirs(Dir dir)
{
    int size = dir.files.Sum(x => x.size) + dir.dirs.Values.Select(x => GoDirs(x)).Sum();
    
    if (size <= 100000) smalldirs.Add((size, dir));
    alldirs.Add((size,dir));
    
    return size;
}

int rootsize = GoDirs(root);
smalldirs.Sum(x => x.size).Dump("Part 1");

int available = 70000000 - rootsize;
int need = 30000000 - available;

//need.Dump();

alldirs.OrderBy(x => x.size).First(x => x.size > need).Item1.Dump("Part 2");

//root.Dump();

record Dir(string name, List<(int size,string name)> files, Dictionary<string,Dir> dirs);