<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>static UserQuery</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
var round = new[] { (5, 0, 4, 0, 1L) };
#else
var round = new[] { (8, 0, 4, 0, 1L) };
var lines = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

^#..#.
^#....
^##..#
..#..
..###".Replace("^","").GetLines();
#endif

int rolls = 0;
int p1 = 4;
int p2 = 5;
int s1 = 0;
int s2 = 0;

int dd = 1;

int roll()
{
	rolls++;
	int ddt = dd;
	dd = (dd % 100) + 1;
	return ddt;
}

while (true)
{
	int r = roll() + roll() + roll();
	p1 = ((p1 + r) - 1) % 10 + 1;
	s1 += p1;
	
	if (s1 >= 1000)
	{
		(rolls * s2).Dump();
		goto round2;
	}
	
	r = roll() + roll() + roll();
	p2 = ((p2 + r) - 1) % 10 + 1;
	s2 += p2;
	
	if (s2 >= 1000)
	{
		(rolls * s1).Dump();
		goto round2;
	}
}

round2:
;

(int tot, int ways)[] dice = new[]{
		(3, 1),
		(4, 3),
		(5, 6),
		(6, 7),
		(7, 6),
		(8, 3),
		(9, 1)
	};

(long w1, long w2) w = (0L,0L);

do
{
	round = NextRound(round);
	
	w.ToString().Dump();
} while (round.Any());

(int p1, int s1, int p2, int s2, long ways)[] NextRound((int p1, int s1, int p2, int s2, long ways)[] round)
{
	var nextRound = round.SelectMany(
		r => dice.Select(d =>
		{
			var p = ((r.p2 + d.tot) - 1) % 10 + 1;
			return (p1: p, s1: r.s2 + p, p2: r.p1, s2: r.s1, ways: r.ways * d.ways);
		}
	)).GroupBy(x => (x.p1, x.s1, x.p2, x.s2)).Select(g => (g.Key.p1, g.Key.s1, g.Key.p2, g.Key.s2, ways: g.Sum(x => x.ways))).ToArray();
	
	var wins = nextRound.Where(x => x.s1 >= 21).Sum(x => x.ways);
	
	w = (w.w2 + wins, w.w1);
	
	var result = nextRound.Where(x => x.s1 < 21).ToArray();
	return result;
}

// 575025114466224 too low