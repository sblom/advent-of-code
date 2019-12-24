<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();
//lines = @"....#
//#..#.
//#..##
//..#..
//#....".Split('\n');

var chars = lines.Select(x => x.ToArray()).ToArray();

//lines.DumpFixed();

var set = new HashSet<long>();

bool[][] field = new bool[7][];
for (int i = 0; i < field.Length; i++)
{
	field[i] = new bool[7];
}

for (int x = 1; x < 6; x++)
{
	for (int y = 1; y < 6; y++)
	{
		field[y][x] = chars[y-1][x-1] == '#';
	}
}

long Score(bool[][]field)
{
	long v = 1, result = 0;

	for (int y = 1; y < 6; y++)
	{
		for (int x = 1; x < 6; x++)
		{
			if(field[y][x])
				result += v;
			
			v <<= 1;
		}
	}
	return result;
}

var dirs = new(int dy, int dx)[] { (-1,0), (1,0), (0,-1), (0,1) };

while (true)
{
	var newfield = field.Select(x => x.ToArray()).ToArray();
	
	for (int x = 1; x < 6; x++)
	{
		for (int y = 1; y < 6; y++)
		{
			int neighbors = (from dir in dirs select field[y - dir.dy][x - dir.dx] ? 1 : 0).Sum();
			if (field[y][x])
			{
				newfield[y][x] = neighbors == 1;
			}
			else
			{
				newfield[y][x] = neighbors == 1 || neighbors == 2;
			}
		}
	}
	
	field = newfield;
	
	//field.Select(x => x.Select(y => y ? '#' : '.')).Select(x => string.Join("",x)).DumpFixed();
	
	var score = Score(field);

	// 18400817 is too low.
	if (set.Contains(score))
	{
		score.Dump("Part 1");
		break;
	}
	
	set.Add(Score(field));
}

Func<int[][]> newgrid = () => new int[5][] { new int[5], new int[5], new int[5], new int[5], new int[5] };

var fieeeeld = new Dictionary<int, int[][]> { { 0, newgrid() } };

for (int x = 0; x < 5; x++)
{
	for (int y = 0; y < 5; y++)
	{
		fieeeeld[0][y][x] = chars[y][x] == '#' ? 1 : 0;
	}
}

for (int c = 0; c < 200; c++)
{
	var min = fieeeeld.Keys.Min();
	var max = fieeeeld.Keys.Max();

	var five = Enumerable.Range(0, 5);
	if (five.Select(x => fieeeeld[min][0][x]).Sum() == 1 ||
		five.Select(y => fieeeeld[min][y][0]).Sum() == 1 ||
		five.Select(x => fieeeeld[min][4][x]).Sum() == 1 ||
		five.Select(y => fieeeeld[min][y][4]).Sum() == 1)
	{
		min--;
		fieeeeld[min] = newgrid();
	}
	if (fieeeeld[max][1][2] == 1 || fieeeeld[max][3][2] == 1 || fieeeeld[max][2][1] == 1 || fieeeeld[max][2][3] == 1)
	{
		max++;
		fieeeeld[max] = newgrid();
	}

	var newfieeeeld = fieeeeld.Select(x => new KeyValuePair<int,int[][]>(x.Key, x.Value.Select(x => x.ToArray()).ToArray())).ToDictionary(x => x.Key, x => x.Value);
	
	fieeeeld[min - 1] = newgrid();
	fieeeeld[max + 1] = newgrid();

	for (var k = min; k <= max; k++)
	{
		for (var i = 0; i < 5; i++)
		{
			for (var j = 0; j < 5; j++)
			{
				var neighbors = (from dir in dirs select (i, j, dir.dy, dir.dx) switch
				{
					(2,2,_,_)  => 0,
					(3,2,-1,0) => five.Select(f => fieeeeld[k + 1][4][f]).Sum(),
					(1,2,1,0)  => five.Select(f => fieeeeld[k + 1][0][f]).Sum(),
					(2,3,0,-1) => five.Select(f => fieeeeld[k + 1][f][4]).Sum(),
					(2,1,0,1)  => five.Select(f => fieeeeld[k + 1][f][0]).Sum(),
					(0,_,-1,0) => fieeeeld[k - 1][1][2],
					(4,_,1,0)  => fieeeeld[k - 1][3][2],
					(_,0,0,-1) => fieeeeld[k - 1][2][1],
					(_,4,0,1)  => fieeeeld[k - 1][2][3],
					(int y, int x, int dy, int dx) => fieeeeld[k][y + dy][x + dx]
				}).Sum();
				
				if (fieeeeld[k][i][j] == 1)
				{
					newfieeeeld[k][i][j] = neighbors == 1 ? 1 : 0;
				}
				else
				{
					newfieeeeld[k][i][j] = (neighbors == 1 || neighbors == 2) ? 1 : 0;
				}
			}
		}
	}
	
	fieeeeld.Remove(min - 1);
	fieeeeld.Remove(max + 1);
	
	fieeeeld = newfieeeeld;
}

fieeeeld.Select(kv => kv.Value.Select(row => row.Sum()).Sum()).Sum().Dump("Part 2");

//var min0 = fieeeeld.Keys.Min();
//var max0 = fieeeeld.Keys.Max();
//
//for (var k = min0; k <= max0; k++)
//{
//	k.Dump();
//	fieeeeld[k].Select(x => string.Join("", x)).DumpFixed();
//}