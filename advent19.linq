<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "Lib\IntCodeVM.linq"

async Task Main()
{
	var lines = await AoC.GetLinesWeb();
	
	var mem = lines.First().Split(',').Select(x => long.Parse(x)).Concat(Enumerable.Repeat(0L,1_000_000)).ToArray();
	
	var prog = new TractorBeamPart2();
	var vm = new IntCodeVM(mem, prog);
	
	while (true)
	{
		while (vm.ExecuteNext())
		{}
		vm.Reset();
	}
}

// Define other methods, classes and namespaces here
class TractorBeamPart1 : IntCodeVM.IInOut
{
	IEnumerable<long> EnumeratePoints()
	{
		foreach (var (x, y) in from x in Enumerable.Range(0, 50) from y in Enumerable.Range(0, 50) select (x, y))
		{			
			yield return x;
			yield return y;
			cur = (x,y);			
		}
	}

	public long Read()
	{
		var result = range.Current;
		if (!range.MoveNext())
		{
			done = true;
		}
		
		return result;
	}

	public void Write(long val)
	{
		map[cur] = val == 1;
		cur.ToString().Dump();
		
		if (done)
		{
			map.Where(kv => kv.Value).Count().Dump("Part 1");
			
			var viz = from y in Enumerable.Range(0,50) select from x in Enumerable.Range(0,50) select map[(x,y)] ? '#' : ' ';
			
			viz.Select(x => string.Join("", x)).DumpFixed();
			
			Environment.Exit(0);
		}
	}
	
	IEnumerator<long> range;
	(int x, int y) cur = (-1,-1);
	bool done = false;
	Dictionary<(int x, int y), bool> map = new Dictionary<(int x, int y), bool>();

	public TractorBeamPart1()
	{
		range = EnumeratePoints().GetEnumerator();
		range.MoveNext();
	}
}

class TractorBeamPart2 : IntCodeVM.IInOut
{
	enum State {
		Tracking = 0,
		Probing = 1
	};
	
	Queue<long> buffer = new Queue<long>();
	char lastch = (char)0;
	(int x, int y) pos = (500,500);
	bool isTractor = false;
	bool checkingSize = false;
	State currentState = State.Tracking;
	
	public long Read()
	{
		if (buffer.Count <= 0)
		{
			switch (currentState)
			{
			case State.Tracking:
				buffer.Enqueue(pos.x);
				buffer.Enqueue(pos.y);
				break;
			case State.Probing:
				buffer.Enqueue(pos.x - 99);
				buffer.Enqueue(pos.y + 99);
				break;
			}
		}
		return buffer.Dequeue();
	}
	
	public void SetNextPoint()
	{
		// Because the slope is steeper than -1, this will track the right edge of the beam.
		pos = isTractor ? (pos.x + 1, pos.y + 1) : (pos.x - 1, pos.y);
	}

	public void Write(long val)
	{
		pos.ToString().Dump();
		switch (currentState)
		{
			case State.Tracking:
				isTractor = val == 1;
				if (isTractor)
				{
					currentState = State.Probing;
					"Probing".Dump();
				}
				else
				{
					SetNextPoint();
				}
				break;
			case State.Probing:
			if (val == 1)
			{
				// 8481095 is too high.
				(pos.x - 99, pos.y).ToString().Dump("Part 2");
				Environment.Exit(0);
			}
			else{
				currentState = State.Tracking;
				SetNextPoint();
			}
			break;
		}
	}

	public TractorBeamPart2()
	{
	}
}