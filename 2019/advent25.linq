<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "Lib\IntCodeVM.linq"
#load "..\Lib\Utils"

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
    AoC._outputs = new DumpContainer[] { new(), new() };
    Util.HorizontalRun("Part 1,Part 2", AoC._outputs).Dump();

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
    
    int seen = 0;
    string prefix = "typing ";
    int code = 0;

	public long Read()
	{
		var result = (long)Buffer[0];
		Buffer.RemoveAt(0);
        Console.Write(Util.WithStyle((char)result,"font-weight:bold"));
		return result;
	}

	public void Write(long val)
	{
        if (seen == prefix.Length)
        {
            if ((char)val >= '0' && (char)val <= '9')
            {
                code *= 10;
                code += (char)val - '0';
            }
            else
            {
                code.Dump1();
                seen = 0;
            }
        }
        else if (prefix[seen] == (char)val)
        {
            seen++;
        }
        else
        {
            seen = 0;
        }
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