<Query Kind="Program">
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

List<string> floornames = new List<string> { "first", "second", "third", "fourth" };
List<string> elements = new List<string>();
HashSet<ulong> visited = new HashSet<ulong>();
List<(int,ulong)> frontier = new List<(int,ulong)>();

void Main()
{
	ulong state = 0; // every pair of bits represents the floor of one item; chip+gen is a quad of 0bCCGG; elevator is bits 63&62.

	var rx = new Regex(@"The (?<floor>[^ ]+) floor contains (nothing relevant\.|(and )?an? (?<element>[^ -]+)(-compatible)? (?<type>microchip|generator)( |, |\.))+");

	var lines = File.ReadLines(@"C:\Users\sblom\Documents\LINQPad Queries\Advent\advent11.txt");
	foreach (var line in lines)
	{
		var match = rx.Match(line);
		if (!match.Groups["element"].Success)
		{
			continue;
		}
		foreach (int n in Enumerable.Range(0, match.Groups["element"].Captures.Count))
		{
			var element = match.Groups["element"].Captures[n].Value;
			var type = match.Groups["type"].Captures[n].Value;
			var floor = floornames.IndexOf(match.Groups["floor"].Value);
			if (!elements.Contains(element)) elements.Add(element);

			state = MoveItem(state, element, type, floor);
			PrintState(state);
		}
	}
	
	// Seed the frontier.
	frontier.Add((0, state));

	while (true)
	{
		var (n, cur) = frontier[0];
		frontier.RemoveAt(0);

		foreach (var next in PossibleMoves(cur))
		{
			if (!visited.Contains(next))
			{
				visited.Add(next);
				frontier.Add((n + 1, next));
			}
		}
	}
}

ulong MoveItem(ulong state, string element, string type, int floor)
{
	int offset = elements.IndexOf(element) * 4 + (type == "microchip" ? 0 : 2);
	state = state & ~((ulong)0x3 << offset);
	state = state | ((ulong)floor << offset);
	return state;
}

IEnumerable<ulong> PossibleMoves(ulong state)
{
	int floor = (int)((state >> 62) & 0x3);
	state = state % (1 << 62);
	List<int> generators = new List<int>();
	
	var items = new List<int>();
	for (int i = 0; state > 0; i += 2, state = state >> 2)
	{
		
	}
	if (((state >> 62) & 0x3) > 0)
	{

	}
	if (((state >> 62) & 0x3) > 3)
	{
		
	}
}

int GetFloorForItem(ulong state, string element, string type)
{
	int offset = elements.IndexOf(element) * 4 + (type == "microchip" ? 0 : 2);
	return (int)((state & (ulong)(0x3 << offset)) >> offset);
}

ulong MoveElevator(ulong state, int floor)
{
	state = state & ~((ulong)0x3 << 62);
	state = state | ((ulong)floor << 62);
	return state;
}

void PrintState(ulong state)
{
	$"elevator = {((state & ((ulong)0x3 << 62)) >> 62) + 1}".Dump();

	for (int i = 0; i < 4; ++i)
	{
		string inventory = "";
		foreach (int n in Enumerable.Range(0, elements.Count()))
		{
			inventory += GetFloorForItem(state, elements[n], "microchip") == i ? "m" + elements[n].Substring(0,2) + " " : "    ";
			inventory += GetFloorForItem(state, elements[n], "generator") == i ? "g" + elements[n].Substring(0,2) + " " : "    ";
		}

		$"{i + 1}: {inventory}".Dump();
	}
}

class Node
{
	public long NodeId { get; private set; }
	public List<long> Adjacencies { get; private set;}
	public Node(long id)
	{
		NodeId = id;
		Adjacencies = new List<long>();
	}
}