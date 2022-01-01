<Query Kind="Program">
  <Connection>
    <ID>b7440089-72f5-46f7-8c5f-8419bdb81277</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>System.Data.SQLite</Provider>
    <CustomCxString>Data Source=C:\Users\sblom\AppData\Local\Packages\Fitbit.Fitbit_6mqt6hf9g46tw\LocalState\fitbit.232TPT.db;FailIfMissing=True</CustomCxString>
    <AttachFileName>&lt;LocalApplicationData&gt;\Packages\Fitbit.Fitbit_6mqt6hf9g46tw\LocalState\fitbit.232TPT.db</AttachFileName>
    <DisplayName>fitbit</DisplayName>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
    </DriverData>
  </Connection>
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

(int Δx, int Δy) ΔxΔy(char c) => c switch
{
	'U' => (0, -1),
	'D' => (0, 1),
	'R' => (1, 0),
	'L' => (-1, 0)
};

async Task Main()
{
	var lines = await AoC.GetLinesWeb();
	//lines = @"R75,D30,R83,U83,L12,D49,R71,U7,L72
	//U62,R66,U55,R34,D71,R55,D58,R83".Split('\n');
	
	var (x, y) = (0, 0);
	
	var mindist = int.MaxValue;
	
	var instrs = lines.First().Split(',');
	
	var points = new Dictionary<(int, int), int>();
	
	var time = 0;
	
	foreach (var instr in instrs)
	{
		var (Δx, Δy) = ΔxΔy(instr[0]);
		
		var dist = int.Parse(instr.AsSpan()[1..]);
		
		for (int i = 0; i < dist; ++i)
		{
			++time;
			(x, y) = (x + Δx, y + Δy);
			if (!points.ContainsKey((x,y))) points.Add((x,y), time);
		}
	}
	
	(x,y) = (0,0);
	time = 0;
	instrs = lines.Skip(1).First().Split(',');
	
	foreach (var instr in instrs)
	{
		var (dy, dx) = instr[0] switch
		{
			'U' => (-1, 0),
			'D' => (1, 0),
			'R' => (0, 1),
			'L' => (0, -1)
		};
	
		var dist = int.Parse(instr[1..]);
	
		for (int i = 0; i < dist; ++i)
		{
			++time;
			(x, y) = (x + dx, y + dy);
			if (points.ContainsKey((x, y)))
			{
				if (points[(x,y)] + time < mindist) mindist = points[(x,y)] + time;
			}
		}
	}
	
	mindist.Dump("Part 2");
}

// Define other methods, classes and namespaces here