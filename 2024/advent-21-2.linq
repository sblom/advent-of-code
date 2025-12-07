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
var lines = $@"029A
980A
179A
456A
379A".Replace("\\", "").GetLines();
    //   2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0

#endif

//"v<<A>>^AvA^Av<<A>>^AAv<A<A>>^AAvAA<^A>Av<A>^AA<A>Av<A<A>>^AAAvA<^A>A".GroupBy(x => x).Dump();
//"<v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A".GroupBy(x => x).Dump();

var numgrid = new Dictionary<char,(int r,int c)> {
    ['7'] = (0,0),
    ['8'] = (0,1),
    ['9'] = (0,2),
    ['4'] = (1,0),
    ['5'] = (1, 1),
    ['6'] = (1, 2),
    ['1'] = (2,0),
    ['2'] = (2,1),
    ['3'] = (2,2),
    ['0'] = (3,1),
    ['A'] = (3,2),
};

var arrowgrid = new Dictionary<char, (int r, int c)>
{
    ['^'] = (0, 1),
    ['A'] = (0, 2),
    ['<'] = (1, 0),
    ['v'] = (1, 1),
    ['>'] = (1, 2),
};

var memo = new Dictionary<(string, int),long>();

long c = 0;

foreach (var line in lines)
{
    line.Dump();
    
    long m = long.MaxValue;
    
    for (int i = 0; i < 1000; i++)
    {        
        //int r = GetPressesForArrows(GetPressesForArrows(GetPressesForKeypad(line).Dump()).Dump()).Dump().Length.Dump() * int.Parse(line[..^1]);
        
        var res = GetPressesForKeypad(line);
        
        memo.Clear();
        long num = GetNumPressesForArrows(res,25);
        
        long r = num * long.Parse(line[..^1]);        
        if (r < m) m = r;
    }   
    c += m;
}

c.Dump();

DumpKeyPad((1,1));

string GetPressesForKeypad(string input)
{
    var start = "A" + input;

    var res = "";
    foreach (var (a, b) in start.Zip(start.Skip(1)))
    {
        var (r0, c0) = numgrid[a];
        var (r1, c1) = numgrid[b];

        var dr = r1 - r0; var sr = Math.Sign(dr); dr = dr * sr;
        var dc = c1 - c0; var sc = Math.Sign(dc); dc = dc * sc;

        var path = string.Create(dr + dc + 1, (r0, c0, r1, c1), (span, state) =>
                {
                    int index = 0;
                    if (r0 == 3 || r1 == 3 || Random.Shared.NextDouble() < 0.5)
                    {
                        for (int i = 0; i < dr; i++)
                        {
                            span[index++] = sr switch { -1 => '^', 1 => 'v' };
                        }
                        for (int i = 0; i < dc; i++)
                        {
                            span[index++] = sc switch { -1 => '<', 1 => '>' };
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dc; i++)
                        {
                            span[index++] = sc switch { -1 => '<', 1 => '>' };
                        }
                        for (int i = 0; i < dr; i++)
                        {
                            span[index++] = sr switch { -1 => '^', 1 => 'v' };
                        }
                    }
                    span[index] = 'A';
                });
        res += path;
    }

    return res;
}

long GetNumPressesForArrows(string input, int remaining)
{    
    if (memo.ContainsKey((input, remaining))) return memo[(input,remaining)];
    if (remaining == 0)
        return input.Length;
    
    var start = "A" + input;

    long tot = 0;

    foreach (var (a, b) in start.Zip(start.Skip(1)))
    {
        var res = "";
        var (r0, c0) = arrowgrid[a];
        var (r1, c1) = arrowgrid[b];
        
        long m1 = 0, m2 = 0;

        var dr = r1 - r0; var sr = Math.Sign(dr); dr = dr * sr;
        var dc = c1 - c0; var sc = Math.Sign(dc); dc = dc * sc;

        if ((r1,c1) == (1,0))
        {
            for (int i = 0; i < dr; i++)
            {
                res += sr switch { -1 => "^", 1 => "v" };
            }
            for (int i = 0; i < dc; i++)
            {
                res += sc switch { -1 => "<", 1 => ">" };
            }
            res += "A";
            m1 = m2 = GetNumPressesForArrows(res, remaining - 1);
        }
        else if ((r0,c0) == (1,0))
        {
            for (int i = 0; i < dc; i++)
            {
                res += sc switch { -1 => "<", 1 => ">" };
            }
            for (int i = 0; i < dr; i++)
            {
                res += sr switch { -1 => "^", 1 => "v" };
            }
            res += "A";
            m1 = m2 = GetNumPressesForArrows(res, remaining - 1);
        }
        else
        {
            for (int i = 0; i < dr; i++)
            {
                res += sr switch { -1 => "^", 1 => "v" };
            }
            for (int i = 0; i < dc; i++)
            {
                res += sc switch { -1 => "<", 1 => ">" };
            }
            res += "A";

            m1 = GetNumPressesForArrows(res, remaining - 1);
            
            res = "";

            for (int i = 0; i < dc; i++)
            {
                res += sc switch { -1 => "<", 1 => ">" };
            }
            for (int i = 0; i < dr; i++)
            {
                res += sr switch { -1 => "^", 1 => "v" };
            }

            res += "A";

            m2 = GetNumPressesForArrows(res, remaining - 1);
        }
        tot += Math.Min(m1,m2);
    }
    
    memo[(input,remaining)] = tot;
    
    return tot;
}

void DumpKeyPad((int r, int c) loc)
{
    for (int i = 0; i < 2; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            char ch = ' ';
            try
            {
                ch = arrowgrid.Where(kv => kv.Value == (i,j)).Single().Key;
            }
            catch {}

            if ((i,j) == loc)
            {
                Console.Write(Util.WithStyle(ch,"color:red;font-family:consolas;"));
            }
            else
            {
                Console.Write(Util.WithStyle(ch,"font-family:consolas;"));
            };
        }
        Console.WriteLine();
    }
}

// 10737418235  Too low ?!
// 3558157621908 too low
// 368480693584714 wrong

//298351708202188
//295324782191370
//295004978795032
//230049027535970