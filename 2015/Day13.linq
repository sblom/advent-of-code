<Query Kind="Program">
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

async Task Main()
{
	var lines = await AoC.GetLinesWeb();

	var rx = new Regex(@"(?<person>\w+) would (?<dir>gain|lose) (?<num>\d+) happiness units by sitting next to (?<neighbor>\w+).");

	var data = lines.Select(x => rx.Match(x)).Select(m => (person: m.Groups["person"].Value, happiness: (m.Groups["dir"].Value == "lose" ? -1 : 1) * int.Parse(m.Groups["num"].Value), neighbor: m.Groups["neighbor"].Value));
	var dict = data.GroupBy(x => x.person).ToDictionary(x => x.Key, x => x.ToDictionary(x => x.neighbor, x => x.happiness));

	var people = data.GroupBy(x => x.person).Select(x => x.Key);

	int maxhappiness1 = 0, maxhappiness2 = 0;

	foreach (var perm in Permute(people))
	{
		var list = perm.Concat(perm.Take(1)).ToList();
		int happiness1 = 0, happiness2 = 0;
		for (int i = 0; i < perm.Count(); ++i)
		{
			happiness1 += dict[list[i]][list[i + 1]];
			happiness1 += dict[list[i + 1]][list[i]];

			if (i < perm.Count() - 1)
			{
				happiness2 += dict[list[i]][list[i + 1]];
				happiness2 += dict[list[i + 1]][list[i]];
			}
		}

		if (happiness1 > maxhappiness1) maxhappiness1 = happiness1;
		if (happiness2 > maxhappiness2) maxhappiness2 = happiness2;
	}
	maxhappiness1.Dump("Part 1");
	maxhappiness2.Dump("Part 2");
}

public IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> @in)
{
	var items = ImmutableList.CreateRange(@in);
	var stack = ImmutableStack<(ImmutableList<T> cur, int pos, ImmutableList<T> acc)>.Empty;

	var (curitems, pos, acc) = (items, 0, ImmutableList<T>.Empty);

	while (true)
	{
		if (pos >= curitems.Count())
		{
			if (stack.Count() == 0) yield break;
			else if (curitems.Count() == 0) yield return acc;

			(curitems, pos, acc) = stack.Peek();
			pos = pos + 1;
			stack = stack.Pop();
		}
		else
		{
			stack = stack.Push((curitems, pos, acc));
			(curitems, pos, acc) = (curitems.RemoveAt(pos), 0, acc.Add(curitems[pos]));
		}
	}
}