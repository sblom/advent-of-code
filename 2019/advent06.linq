<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

class Orbit {
	public string Name { get; set; }
	public Orbit Orbiting { get; set; }
	public List<Orbit> Orbits = new List<Orbit>();
}

Dictionary<string, Orbit> Orbits = new Dictionary<string, Orbit>();

int OrbitalChecksum(Orbit orbit, int depth = 0)
{
	int result = orbit.Orbits.Select(o => OrbitalChecksum(o, depth + 1)).Sum() + depth;
	return result;
}

int OrbitalDepth(Orbit orbit, string name)
{
	if (orbit.Orbits.Count == 0) return -1;
	else if (orbit.Orbits.Any(o => o.Name == name)) return 0;
	else
	{
		var depth = orbit.Orbits.Select(o => OrbitalDepth(o, name)).Max();
		if (depth == -1) return depth;
		else return depth + 1;
	}
}

async Task Main()
{
	var lines = await AoC.GetLinesWeb();
	//	lines = @"COM)B
	//B)C
	//C)D
	//D)E
	//E)F
	//B)G
	//G)H
	//D)I
	//E)J
	//J)K
	//K)L".Split("\n").Select(L => L.Trim());
//	lines = @"COM)B
//B)C
//C)D
//D)E
//E)F
//B)G
//G)H
//D)I
//E)J
//J)K
//K)L
//K)YOU
//I)SAN".Split("\n").Select(L => L.Trim());
	
	var orbs = lines.Select(L => L.Split(')')).ToList();
	
	foreach (var orb in orbs)
	{
		var major = Orbits.ContainsKey(orb[0]) ? Orbits[orb[0]] : new Orbit { Name = orb[0] };
		var minor = Orbits.ContainsKey(orb[1]) ? Orbits[orb[1]] : new Orbit { Name = orb[1] };
		
		major.Orbits.Add(minor);
		minor.Orbiting = major;
		Orbits[major.Name] = major;
		Orbits[minor.Name] = minor;
	}
	
	OrbitalChecksum(Orbits["COM"]).Dump("Part 1");
	
	Orbit up = Orbits["YOU"].Orbiting;
	int transfers = 0;

	while (up.Orbiting.Name != "COM")
	{
		var depth = OrbitalDepth(up, "SAN");
		if (depth != -1) {
			(transfers + depth).Dump("Part 2");
			break;
		}
		up = up.Orbiting;
		++transfers;
	}
}

// Define other methods, classes and namespaces here
