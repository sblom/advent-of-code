<Query Kind="Program" />

async void Main()
{
	var lines = await AoC.GetLinesWeb();

	var rx = new Regex(@"^(?<units>\d+) units each with (?<hits>\d+) hit points( \((immune to (?<immunities>[\w, ]+)?(; )?)?(weak to (?<weaknesses>[\w, ]+)?)?\))? with an attack that does (?<damage>\d+) (?<type>\w+) damage at initiative (?<initiative>\d+)$");
	var rx2 = new Regex(@"^(?<units>\d+) units each with (?<hits>\d+) hit points( \((weak to (?<weaknesses>[\w, ]+)?(; )?)?(immune to (?<immunities>[\w, ]+)?)?\))? with an attack that does (?<damage>\d+) (?<type>\w+) damage at initiative (?<initiative>\d+)$");


	for (int boost = 35; ; boost += 10)
	{
		var immune = new List<AttackGroup>();
		var infection = new List<AttackGroup>();

		List<AttackGroup> working = null;

		foreach (var line in lines)
		{
			if (line.StartsWith("Immune System:"))
			{
				working = immune;
				continue;
			}
			else if (line.StartsWith("Infection:"))
			{
				working = infection;
				continue;
			}
			else if (string.IsNullOrWhiteSpace(line)) continue;

			var match = rx.Match(line);
			if (!match.Success)
				match = rx2.Match(line);

			var newgroup = new AttackGroup
			{
				num = int.Parse(match.Groups["units"].Value),
				hits = int.Parse(match.Groups["hits"].Value),
				immunities = match.Groups["immunities"].Success ? match.Groups["immunities"].Value.Split(',').Select(t => t.Trim()).ToList() : new List<string>(),
				weaknesses = match.Groups["weaknesses"].Success ? match.Groups["weaknesses"].Value.Split(',').Select(t => t.Trim()).ToList() : new List<string>(),
				dam = int.Parse(match.Groups["damage"].Value) + (working == immune ? boost : 0),
				type = match.Groups["type"].Value,
				init = int.Parse(match.Groups["initiative"].Value),
			};
			working.Add(newgroup);
		}

//			immune.Dump();
//			infection.Dump();

		while (immune.Count > 0 && infection.Count > 0)
		{
			var targets = new Dictionary<AttackGroup, AttackGroup>();
			foreach (var group in immune.Union(infection).OrderByDescending(u => u.num * u.dam).ThenByDescending(u => u.init).ToList())
			{
				if (immune.Contains(group))
				{
					var target = infection.Where(u => !targets.ContainsValue(u))
									.OrderByDescending(u => group.dam * group.num * (u.weaknesses.Contains(group.type) ? 2 : 1) * (u.immunities.Contains(group.type) ? 0 : 1))
									.ThenByDescending(u => u.num * u.dam)
									.ThenByDescending(u => u.init)
									.FirstOrDefault();
					if (target != null && !target.immunities.Contains(group.type))
					{
						targets[group] = target;
					}
				}
				else
				{
					var target = immune.Where(u => !targets.ContainsValue(u))
									.OrderByDescending(u => group.dam * group.num * (u.weaknesses.Contains(group.type) ? 2 : 1) * (u.immunities.Contains(group.type) ? 0 : 1))
									.ThenByDescending(u => u.num * u.dam)
									.ThenByDescending(u => u.init)
									.FirstOrDefault();
					if (target != null && !target.immunities.Contains(group.type))
					{
						targets[group] = target;
					}
				}
			}
			foreach (var kvp in targets.OrderByDescending(kvp => kvp.Key.init))
			{
				if (!immune.Contains(kvp.Key) && !infection.Contains(kvp.Key)) continue;

				var damage = kvp.Key.dam * kvp.Key.num * (kvp.Value.weaknesses.Contains(kvp.Key.type) ? 2 : 1);
				var units = damage / kvp.Value.hits;
				kvp.Value.num -= units;
				if (kvp.Value.num <= 0)
				{
					immune.Remove(kvp.Value);
					infection.Remove(kvp.Value);
				}
			}
		}
		if (immune.Count > 0)
		{
			immune.Sum(g => g.num).Dump("Part 1 (Immune system wins)");
			boost.Dump();
			return;
		}
		else // infection.Count > 0
		{
			//infection.Sum(g => g.num).Dump("Part 1 (Infection wins)");
			// Try next boost!
			continue;
		}
	}
}

public class AttackGroup
{
	public int num;
	public int hits;
	public List<string> immunities;
	public List<string> weaknesses;
	public int dam;
	public string type;
	public int init;
};