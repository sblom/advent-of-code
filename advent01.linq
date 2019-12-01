<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

#LINQPad optimize+

public class AoC2019Day1
{
	IEnumerable<string> lines;
	IEnumerable<int> parsed;
	
	public AoC2019Day1()
	{
		lines = AoC.GetLinesWeb().Result;
		parsed = lines.Select(x => int.Parse(x)).ToList();
	}

	[Benchmark]
	public int Part1Linq()
	{
		return parsed.Select(x => x / 3 - 2).Sum();
	}
	
	[Benchmark]
	public int Part2Linq()
	{
		return parsed.SelectMany(x => Fuelrequirements(x)).Sum();
	}
	
	[Benchmark]
	public int Part1Foreach()
	{
		int total = 0;
		foreach (int n in parsed)
		{
			total += n / 3 - 2;
		}
		return total;
	}
	
	[Benchmark]
	public int Part2Foreach()
	{
		int total = 0;
		foreach (int n in parsed)
		{
			int ni = n;
			while (true)
			{
				ni = ni / 3 - 2;
				if (ni > 0) total += ni;
				else break;
			}
		}
		return total;
	}

	IEnumerable<int> Fuelrequirements(int n)
	{
		while (n > 0)
		{
			n = n / 3 - 2;
			if (n > 0) yield return n;
			else yield break;
		}
	}
}

async Task Main()
{
	BenchmarkRunner.Run<AoC2019Day1>();	
}

//| Method | Mean | Error | StdDev | Median |
//|------------- | ----------:| ----------:| ----------:| ----------:|
//| Part1Linq | 1.353 us | 0.0288 us | 0.0845 us | 1.324 us |
//| Part2Linq | 30.275 us | 0.6576 us | 1.8439 us | 30.296 us |
//| Part1Foreach | 1.081 us | 0.0228 us | 0.0575 us | 1.059 us |
//| Part2Foreach | 3.138 us | 0.0625 us | 0.1318 us | 3.084 us |

// Define other methods, classes and namespaces here