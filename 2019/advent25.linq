<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "Lib\IntCodeVM.linq"

// Solved this by hand. Wandered the environment and discovered the
// obtainable items:
// - dark matter
// - space heater
// - semiconductor
// - planetoid
// - hypercube
// - spool of cat6
// - sand
// - festive hat

// Correct combination:
// everything except sand, planetoid, dark matter, and spool of cat6
// so: space heater, semiconductor, hypercube, festive hat
//
// dark matter and spool of cat6 are independently too heavy
// the remaining 6 minus semiconductor or space heater are too light, meaning that dark matter and space heater are required
// the other four take some fiddling, but there are only 16 combinations, and I considered trying to put together a gray code
// south, east, take space heater, west, north, east, north, north, take hypercube, south, south, west,
// north, take festive hat, west, north, east, take semiconductor, east, north, west

async Task Main()
{
	var lines = await AoC.GetLinesWeb();
	
	var mem = lines.First().Split(',').Select(long.Parse).Concat(Enumerable.Repeat(0L,100_000_000)).ToArray();
	
	//var io = new Interactive();
	var io = new Autosolve();
	
	var vm = new IntCodeVM(mem,io);
	
	while (vm.ExecuteNext())
	{		
	}
}

public class Autosolve : IntCodeVM.IInOut
{
	List<char> Buffer = @"south
east
take space heater
west
north
east
north
north
take hypercube
south
south
west
north
take festive hat
west
north
east
take semiconductor
east
north
west
".Replace("\r", "").ToCharArray().ToList();

	public long Read()
	{
		var result = (long)Buffer[0];
		Buffer.RemoveAt(0);
		return result;
	}

	public void Write(long val)
	{
		Console.Write((char)val);
	}
}

public class Interactive: IntCodeVM.IInOut
{
	List<char> Buffer = new List<char>();
	
	public long Read()
	{
		if (Buffer.Count <= 0)
		{
			Buffer.AddRange(Console.ReadLine());
			Buffer.Add('\n');
		}
		var result = (long)Buffer[0];
		Buffer.RemoveAt(0);
		return result;
	}
	
	public void Write(long val)
	{
		Console.Write((char)val);
	}
}