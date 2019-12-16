<Query Kind="Statements">
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();

var molecule = lines.Last();
var replacements = lines.Take(lines.Count() - 2).Select(x => x.Split(" => ")).Select(x => (from: x[0], to: x[1])).ToList();

var set = new HashSet<string>();

foreach (var replacement in replacements)
{	
	int index = -1;
	while ((index = molecule.IndexOf(replacement.from,index + 1)) > -1)
	{
		set.Add(string.Join("",molecule.Take(index).Concat(replacement.to).Concat(molecule.Skip(index + replacement.from.Length))));
	}
}

set.Count().Dump("Part 1");

set.Clear();

var steps = ImmutableStack<(string mol, int index, int replacement)>.Empty.Push((molecule, -1, -1));

int c = 0;

while (steps.Peek().mol != "e")
{
	var cur = steps.Peek();
	steps = steps.Pop();

	int i = 0;
	for (i = cur.replacement + 1; i < replacements.Count(); i++)
	{
		int idx = -1;
		idx = cur.mol.IndexOf(replacements[i].to, idx + 1);
		if (idx > -1)
		{
			var result = string.Join("", cur.mol.Take(idx).Concat(replacements[i].from).Concat(cur.mol.Skip(idx + replacements[i].to.Length)));
			if (!set.Contains(result))
			{
				set.Add(result);
				steps = steps.Push((cur.mol, i, idx));
				steps = steps.Push((result, -1, -1));
				c++;
				goto next_step;
			}
		}
	}
	if (i == replacements.Count())
	{
		steps = steps.Pop();
	}
next_step:;
}

(steps.Count() - 1).Dump("Part 2");