<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"".GetLines();
#endif

var tiles = lines.GroupLines().Where(group => group.Count() > 0).Select(group => new Tile(group)).ToArray();

var neighbors = (from t1 in tiles select (tile: t1, neighbors: from t2 in tiles where t1 != t2 && t1.AllBorders.Any(b => t2.AllBorders.Contains(b)) select t2));

neighbors.Where(x => x.neighbors.Count() == 2).Select(x => x.tile.Id).Aggregate(1L, (x,y) => x * y).Dump("Part 1");

Tile[][] assembledGrid = Enumerable.Range(0,12).Select(_ => new Tile[12]).ToArray();

var remainingTiles = tiles.ToHashSet();

for (int i = 0; i < 12; i++)
{
	for (int j = 0; j < 12; j++)
	{
		if (i == 0 && j == 0)
		{
			var first = neighbors.Where(x => x.neighbors.Count() == 2).First();
			first.tile.Transform = 0;
			while (!first.neighbors.Any(tile => tile.AllBorders.Contains(first.tile.OrientedBorders[1])) || !tiles.Any(tile => tile.AllBorders.Contains(first.tile.OrientedBorders[2])))
			{
				first.tile.Transform += 1;
			}
			
			assembledGrid[i][j] = first.tile;
			remainingTiles.Remove(first.tile);
		}
		else if (j == 0)
		{
			var tile = remainingTiles.First(tile => tile.AllBorders.Contains(assembledGrid[i-1][j].OrientedBorders[2]));
			while (tile.OrientedBorders[0] != assembledGrid[i-1][j].OrientedBorders[2])
			{
				tile.Transform += 1;
			}

			assembledGrid[i][j] = tile;
			remainingTiles.Remove(tile);
		}
		else
		{
			var tile = remainingTiles.First(tile => tile.AllBorders.Contains(assembledGrid[i][j - 1].OrientedBorders[1]));
			while (tile.OrientedBorders[3] != assembledGrid[i][j - 1].OrientedBorders[1])
			{
				tile.Transform += 1;
			}

			assembledGrid[i][j] = tile;
			remainingTiles.Remove(tile);
		}
	}
}

var monster = new[] {
"                  # ".Reverse().ToArray(),
"#    ##    ##    ###".Reverse().ToArray(),
" #  #  #  #  #  #   ".Reverse().ToArray()};


char[,] grid = new char[96,96];

for (int i = 0; i < 12 * 8; i++)
{
	for (int j = 0; j < 12 * 8; j++)
	{
		grid[i,j] = assembledGrid[i / 8][j / 8][i % 8 + 1,j % 8 + 1];
	}
}

for (int i = 0; i < 12 * 8 - monster.Length; i++)
{
	for (int j = 0; j < 12 * 8 - monster[0].Length; j++)
	{
		for (int n = 0; n < monster.Length; n++)
		{
			for (int m = 0; m < monster[0].Length; m++)
			{
				if (monster[n][m] == '#' && grid[i+n,j+m] != '#') goto nextcell;
			}
		}


		"Monster!".DumpTrace();
		for (int n = 0; n < monster.Length; n++)
		{
			for (int m = 0; m < monster[0].Length; m++)
			{
				if (monster[n][m] == '#') grid[i + n, j + m] = '.';
			}
		}

	nextcell:;
	}
}

int c = 0;
foreach (var ch in grid)
{
	if (ch == '#') c++;
}

c.Dump("Part 2");

//assembledGrid.Select(row => row.Select(cell => cell.Transform)).Dump();
//
//var sb = new StringBuilder();
//
//for (int i = 0; i < 12 * 8; i++)
//{
//	for (int j = 0; j < 12 * 8; j++)
//	{
//		sb.Append(assembledGrid[i / 8][j / 8][i % 8 + 1,j % 8 + 1]);
//	}
//	sb.AppendLine();
//}
//
//sb.ToString().DumpFixed();

class Tile
{
	public long Id { get; set; }
	public int[] AllBorders { get; set; }
	public char[][] Grid { get; set; }

	public char this[int y, int x] {
		get {
			var (x1, y1) = Transforms[_transform]((x,y));			
			return Grid[y1][x1];
		}
	}	
	
	public int[] OrientedBorders {get;set;}
	
	int _transform;
	public int Transform {
		get
		{
			return _transform;	
		}
		set
		{
			_transform = value;
			var ob = Enumerable.Range(0, Grid.Length).Aggregate((a: 0, b: 0, c: 0, d: 0), (seed, n) => (seed.a * 2 + N(this[0,n]), seed.b * 2 + N(this[n,end]), seed.c * 2 + N(this[end,n]), seed.d * 2 + N(this[n,0])));
			OrientedBorders = new[] {ob.a, ob.b, ob.c, ob.d};
		}
	}
	
	private int end;
	private Func<(int,int),(int,int)>[] Transforms;
	
	private int N(char ch) => ch switch {'#' => 1, '.' => 0};
	
	public Tile(IEnumerable<string> lines)
	{		
		Id = lines.First().Extract<long>(@"Tile (\d+):");
		Grid = lines.Skip(1).Select(line => line.ToCharArray()).ToArray();
		
		end = Grid.Length - 1;

		Transforms= new Func<(int, int), (int, int)>[]{
			((int x,int y) i) => (      i.x,       i.y),
			((int x,int y) i) => (end - i.y,       i.x),
			((int x,int y) i) => (end - i.x, end - i.y),
			((int x,int y) i) => (      i.y, end - i.x),
			((int x,int y) i) => (end - i.x,       i.y),
			((int x,int y) i) => (end - i.y, end - i.x),
			((int x,int y) i) => (      i.x, end - i.y),
			((int x,int y) i) => (      i.y,       i.x)
		};
		
		var borderValues = Enumerable.Range(0, Grid.Length).Aggregate((a: 0, b: 0, c: 0, d: 0, e: 0, f: 0, g: 0, h: 0), (seed, n) => (seed.a * 2 + N(Grid[0][n]), seed.b * 2 + N(Grid[0][end - n]), seed.c * 2 + N(Grid[n][end]), seed.d * 2 + N(Grid[end - n][end]), seed.e * 2 + N(Grid[end][n]), seed.f * 2 + N(Grid[end][end - n]), seed.g * 2 + N(Grid[n][0]), seed.h * 2 + N(Grid[end - n][0])));
		AllBorders = new int[] {borderValues.a, borderValues.b, borderValues.c, borderValues.d, borderValues.e, borderValues.f, borderValues.g, borderValues.h };
		OrientedBorders = new int[] {AllBorders[0], AllBorders[2], AllBorders[4], AllBorders[6]};
	}
}