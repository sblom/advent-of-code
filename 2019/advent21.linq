<Query Kind="Program" />

#load "Lib\IntCodeVM.linq"

async void Main()
{
	var lines = await AoC.GetLinesWeb();
	var mem = lines.First().Split(',').Select(x => long.Parse(x)).Concat(Enumerable.Repeat(0L, 1000000)).ToArray();
	
	IntCodeVM.IInOut io = new Part1();
	
	var vm = new IntCodeVM(mem, io);

	while (vm.ExecuteNext())
	{}

	(io as Part1).result.Dump("Part 1");


	io = new Part2();

	vm = new IntCodeVM(mem, io);

	while (vm.ExecuteNext())
	{ }

	(io as Part2).result.Dump("Part 2");
}

class Part2 : IntCodeVM.IInOut
{
	// Jump if at least one of A,B,C is false.
	// So long as D is true.
	// And either E or H is true.
	
	char[] program = ("NOT C J\nNOT B T\nOR T J\nNOT A T\nOR T J\n" +
					  "NOT A T\nAND A T\n" + // reset T
					  "OR E T\nOR H T\nAND T J\n" +
					  "AND D J\n" +
					  "RUN\n").ToCharArray();
	int c = 0;
	public long result = 0;
	
	public long Read()
	{
		return program[c++];
	}

	public void Write(long output)
	{
		result = output;
		Console.Write((char)output);
	}
}

// Define other methods, classes and namespaces here

class Part1 : IntCodeVM.IInOut
{
	char[] program = ("NOT C J\nNOT B T\nOR T J\nNOT A T\nOR T J\n" +
					  "AND D J\n" +
					  "WALK\n").ToCharArray();
	int c = 0;
	public long result = 0;
	
	public long Read()
	{
		return program[c++];
	}

	public void Write(long output)
	{
		result = output;
		Console.Write((char)output);
	}
}