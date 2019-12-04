<Query Kind="Statements">
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

var lines = await AoC.GetLinesWeb();

var (x, y) = (0, 0);

var mindist = int.MaxValue;

var instrs = lines.First().Split(',');

var points = new HashSet<(int, int)>();

foreach (var instr in instrs)
{
	var (dy, dx) = instr[0] switch {
		'U' => (-1, 0),
		'D' => (1,0),
		'R' => (0, 1),
		'L' => (0, -1)
	};
	
	var dist = int.Parse(instr[1..]);
	
	for (int i = 0; i < dist; ++i)
	{
		(x, y) = (x + dx, y + dy);
		points.Add((x,y));
	}
}

(x,y) = (0,0);
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
		(x, y) = (x + dx, y + dy);
		if (points.Contains((x, y)))
		{
			if (Math.Abs(x) + Math.Abs(y) < mindist) mindist = Math.Abs(x) + Math.Abs(y);
		}
	}
}

mindist.Dump("Part 1");