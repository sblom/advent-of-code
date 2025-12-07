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

var arrays = lines.Select(line => Regex.Split(line, @"\s+").ToArray());

var nums = arrays.Take(4).Select(line => line.Select(long.Parse).ToArray()).ToArray();
var ops = arrays.Skip(4).First().ToArray();

nums.Dump();

long t = 0;

for (int i = 0; i < nums[0].Length; i++)
{
    var n = ops[i] switch
    {
        "+" => nums[0][i] + nums[1][i] + nums[2][i] + nums[3][i],
        "*" => nums[0][i] * nums[1][i] * nums[2][i] * nums[3][i],
    };
    
    t += n;
}

t.Dump();

var grid = lines.Select(line => line.ToCharArray()).ToArray();

var list = new List<long>();

t = 0;

for (int i = grid[0].Length - 1; i >= 0; i--)
{
    long n = 0;
    for (int j = 0; j < 5; j++)
    {
        if (char.IsDigit(grid[j][i]))
        {
            n = n * 10 + (grid[j][i] - '0');
        }
        if (j == 4)
        {
            if (n != 0) list.Add(n);
            
            if (grid[j][i] is '+' or '*')
            {
                list.Dump();
                t += list.Aggregate((long a, long b) => grid[j][i] == '+' ? a + b : a * b);
                list.Clear();
            }
        }
    }
}

t.Dump();
