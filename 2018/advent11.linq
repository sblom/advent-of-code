<Query Kind="Statements" />

// See bottom for results of 41:06.060 part 2 run.

var lines = await AoC.GetLinesWeb();

var SN = 5235;

int f(int x, int y)
{
	return (((x + 10) * y + SN) * (x + 10) / 100) % 10 - 5;
}

int[,] grid = new int[301,301];
int[,] conv = new int[301,301];

for (int i = 1; i <= 300; i++)
{
	for (int j = 1; j <= 300; j++)
	{
		grid[j,i] = f(j,i);
	}
}

conv = grid.Clone() as int[,];

int[,] dump = new int[11,11];
for (int i = 0; i < 11; i++)
{
	for (int j = 0; j < 11; j++)
	{
		dump[i,j] = conv[i,j];
	}
}
dump.Dump();

var maxtot = 0;
var (mx, my, mk) = (0,0,0);

for (int i = 1; i <= 298; i++)
{
	for (int j = 1; j <= 298; j++)
	{
		for (int z = 1; z <= 3; z++)
		{
			int tot = 0;			
			var offsets = from dx in Enumerable.Range(0,z) from dy in Enumerable.Range(0,z) select (dx, dy);
			foreach (var (dx, dy) in offsets)
			{
				tot += grid[i + dy, j + dx];
			}

			if (tot > maxtot)
			{
				maxtot = tot;
				(mx, my) = (i,j);
			}
		}
	}
}

(mx,my).ToString().Dump("Part 1");
maxtot.Dump();

maxtot = 0;

for (int k = 1; k <= 300; k++)
{
	for (int i = k; i <= 300; i++)
	{
		for (int j = k; j <= 300; j++)
		{
			if (conv[i,j] > maxtot)
			{
				maxtot = conv[i,j];
				(mx, my, mk) = (i + 1 - k, j + 1 - k, k - 1);
				(mx, my, mk, conv[i,j]).ToString().Dump();
			}

			conv[i,j] += grid[i - k, j - k];
			for (int ii = 0; ii < k; ii++)
			{
				conv[i,j] += grid[i + ii - k, j - k];
				conv[i,j] += grid[i - k, j + ii - k];
			}
		}
	}
}

(mx, my, mk).ToString().Dump("Part 2");


//(1, 1, 1, 2)
//(1, 1, 2, 5)
//(1, 7, 2, 6)
//(1, 22, 7, 12)
//(1, 23, 6, 14)
//(1, 26, 3, 16)
//(1, 26, 4, 23)
//(7, 169, 7, 27)
//(15, 219, 9, 29)
//(16, 8, 7, 34)
//(16, 220, 8, 37)
//(17, 8, 7, 39)
//(18, 8, 6, 41)
//(42, 233, 9, 48)
//(44, 152, 9, 49)
//(91, 198, 14, 56)
//(94, 201, 11, 60)
//(166, 231, 12, 64)
//(228, 285, 12, 65)
//(230, 287, 10, 68)
//(231, 287, 10, 73)
//(232, 288, 9, 78)
//(232, 289, 8, 79)
//79