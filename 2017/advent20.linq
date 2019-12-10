<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Numerics.dll</Reference>
  <Namespace>System.Numerics</Namespace>
</Query>

var lines = AoC.GetLines();

var rx = new Regex(@"p=<(-?\d+,-?\d+,-?\d+)>, v=<(-?\d+,-?\d+,-?\d+)>, a=<(-?\d+,-?\d+,-?\d+)>");

var T = 100000000;

var P_list = new List<long[]>(1000);
var V_list = new List<long[]>(1000);
var A_list = new List<long[]>(1000);

foreach (var line in lines)
{
	var match = rx.Match(line);	
	P_list.Add(match.Groups[1].Value.Split(',').Select(x => long.Parse(x)).ToArray());
	V_list.Add(match.Groups[2].Value.Split(',').Select(x => long.Parse(x)).ToArray());
	A_list.Add(match.Groups[3].Value.Split(',').Select(x => long.Parse(x)).ToArray());
}

Enumerable.Range(0,1000).Select(x => (x, Math.Abs(A_list[x][0])+Math.Abs(A_list[x][1])+Math.Abs(A_list[x][2]), Math.Abs(V_list[x][0])+Math.Abs(V_list[x][1])+Math.Abs(V_list[x][2]), Math.Abs(P_list[x][0])+Math.Abs(P_list[x][1])+Math.Abs(P_list[x][2]))).OrderBy(x => x.Item2).ThenBy(x => x.Item3).ThenBy(x => x.Item4).First().Item1.Dump("Part 1");


for (long t = 0; ; t++)
{
	long min = long.MaxValue;
	int p_min = -1;
	
	int num_min = 1000;
	
	for (int n = 0; n < P_list.Count; n++)
	{
		V_list[n][0] += A_list[n][0];
		V_list[n][1] += A_list[n][1];
		V_list[n][2] += A_list[n][2];

		P_list[n][0] += V_list[n][0];
		P_list[n][1] += V_list[n][1];
		P_list[n][2] += V_list[n][2];
		
		var dist = Math.Abs(P_list[n][0]) + Math.Abs(P_list[n][1]) + Math.Abs(P_list[n][2]);
		if (n == 0) min = dist;
		if (dist < min)
		{
			min = dist;
			p_min = n;
		}
	}

	var collisions = Enumerable.Range(0, P_list.Count).Select(x => (x, (P_list[x][0], P_list[x][1], P_list[x][2]))).GroupBy(x => x.Item2).OrderByDescending(x => x.Count()).Where(x => x.Count() > 1);

	if (collisions.Any())
	{
		foreach (var collision in collisions.SelectMany(x => x.Select(y => y.x)).OrderByDescending(x => x).ToList())
		{
			P_list.RemoveAt(collision);
			V_list.RemoveAt(collision);
			A_list.RemoveAt(collision);
		}
		P_list.Count.Dump();
	}

	//	p_min.Dump();
//	
//	if (p_min != 223)
//		p_min.Dump();
}