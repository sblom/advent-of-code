<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var lines = await AoC.GetLinesWeb();
	//var lines = @"";
	
	var rx = new Regex(@"");
	
	var norms = from L in lines
	            //let rxr = rx.Match(L)
				//select (rxr.Groups[""], rxr.Groups[""]);
				select int.Parse(L);
				
	norms.SelectMany(x => Fuelrequirements(x)).Sum().Dump();
}

IEnumerable<int> Fuelrequirements(int n)
{
	while (n > 0)
	{
		n = n / 3 - 2;
		if (n > 0) yield return n / 3 - 2;
		else yield break;
	}
}

// Define other methods, classes and namespaces here
