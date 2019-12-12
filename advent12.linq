<Query Kind="Statements">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <RemoveNamespace>System</RemoveNamespace>
</Query>

var lines = await AoC.GetLinesWeb();
//lines = @"<x=-1, y=0, z=2>
//<x=2, y=-10, z=-7>
//<x=4, y=-8, z=8>
//<x=3, y=5, z=-1>".Split('\n').Select(x => x.Trim());
//lines = @"<x=-8, y=-10, z=0>
//<x=5, y=5, z=10>
//<x=2, y=-7, z=3>
//<x=9, y=-8, z=-3>".Split('\n').Select(x => x.Trim());

var rx = new Regex(@"\<x=(?<x>-?\d+), y=(?<y>-?\d+), z=(?<z>-?\d+)\>");

var positions = lines.Select(L => rx.Match(L)).Select(x => (x: int.Parse(x.Groups["x"].Value), y: int.Parse(x.Groups["y"].Value), z: int.Parse(x.Groups["z"].Value))).ToArray();
(int x, int y, int z)[] velocities = new[] { (0, 0, 0), (0, 0, 0), (0, 0, 0), (0, 0, 0) };
var states = new[] { new HashSet<(int, int, int, int, int, int, int, int)>(), new HashSet<(int, int, int, int, int, int, int, int)>(), new HashSet<(int, int, int, int, int, int, int, int)>() };

int? xc = null, yc = null, zc = null;

for (int c = 0; ; c++)
{
	//if (c % 10 == 0)
	//{
	//	var energy = (from i in Enumerable.Range(0, 4)
	//				  let pos = positions[i]
	//				  let vel = velocities[i]
	//				  let ke = Abs(vel.x) + Abs(vel.y) + Abs(vel.z)
	//				  let pe = Abs(pos.x) + Abs(pos.y) + Abs(pos.z)
	//				  select ke * pe).Sum().Dump($"Iter {c}");
	//}


	(int x, int y, int z)[] deltas = new[] { (0, 0, 0), (0, 0, 0), (0, 0, 0), (0, 0, 0) };
	for (int i = 0; i < positions.Count(); i++)
	{
		for (int j = i + 1; j < positions.Count(); j++)
		{
			var dx = positions[i].x > positions[j].x ? 1 : positions[i].x == positions[j].x ? 0 : -1;
			var dy = positions[i].y > positions[j].y ? 1 : positions[i].y == positions[j].y ? 0 : -1;
			var dz = positions[i].z > positions[j].z ? 1 : positions[i].z == positions[j].z ? 0 : -1;

			deltas[i] = (deltas[i].x - dx, deltas[i].y - dy, deltas[i].z - dz);
			deltas[j] = (deltas[j].x + dx, deltas[j].y + dy, deltas[j].z + dz);
		}
	}

	for (int i = 0; i < positions.Count(); i++)
	{
		velocities[i] = (velocities[i].x + deltas[i].x, velocities[i].y + deltas[i].y, velocities[i].z + deltas[i].z);
		positions[i] = (positions[i].x + velocities[i].x, positions[i].y + velocities[i].y, positions[i].z + velocities[i].z);
	}

	if (states[0].Contains((velocities[0].x, velocities[1].x, velocities[2].x, velocities[3].x, positions[0].x, positions[1].x, positions[2].x, positions[3].x)))
	{
		xc ??= c;
	}
	else
	{
		states[0].Add((velocities[0].x, velocities[1].x, velocities[2].x, velocities[3].x, positions[0].x, positions[1].x, positions[2].x, positions[3].x));
	}

	if (states[0].Contains((velocities[0].y, velocities[1].y, velocities[2].y, velocities[3].y, positions[0].y, positions[1].y, positions[2].y, positions[3].y)))
	{
		yc ??= c;
	}
	else
	{
		states[0].Add((velocities[0].y, velocities[1].y, velocities[2].y, velocities[3].y, positions[0].y, positions[1].y, positions[2].y, positions[3].y));
	}

	if (states[0].Contains((velocities[0].z, velocities[1].z, velocities[2].z, velocities[3].z, positions[0].z, positions[1].z, positions[2].z, positions[3].z)))
	{
		zc ??= c;
	}
	else
	{
		states[0].Add((velocities[0].z, velocities[1].z, velocities[2].z, velocities[3].z, positions[0].z, positions[1].z, positions[2].z, positions[3].z));
	}
	
	if (xc != null && yc != null && zc != null) (xc,yc,zc).ToString().Dump();
}