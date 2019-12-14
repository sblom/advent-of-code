<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

//lines = @"10 ORE => 10 A
//1 ORE => 1 B
//7 A, 1 B => 1 C
//7 A, 1 C => 1 D
//7 A, 1 D => 1 E
//7 A, 1 E => 1 FUEL".Split("\n").Select(x => x.Trim());
//
//lines = @"9 ORE => 2 A
//8 ORE => 3 B
//7 ORE => 5 C
//3 A, 4 B => 1 AB
//5 B, 7 C => 1 BC
//4 C, 1 A => 1 CA
//2 AB, 3 BC, 4 CA => 1 FUEL".Split("\n").Select(x => x.Trim());
//
//lines = @"157 ORE => 5 NZVS
//165 ORE => 6 DCFZ
//44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL
//12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ
//179 ORE => 7 PSHF
//177 ORE => 5 HKGWZ
//7 DCFZ, 7 PSHF => 2 XJWVT
//165 ORE => 2 GPVTF
//3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT".Split("\n").Select(x => x.Trim());
//
//lines = @"2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG
//17 NVRVD, 3 JNWZP => 8 VPVL
//53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL
//22 VJHF, 37 MNCFX => 5 FWMGM
//139 ORE => 4 NVRVD
//144 ORE => 7 JNWZP
//5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC
//5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV
//145 ORE => 6 MNCFX
//1 NVRVD => 8 CXFTF
//1 VJHF, 6 MNCFX => 4 RFSQX
//176 ORE => 6 VJHF".Split("\n").Select(x => x.Trim());

var rx = new Regex(@"((?<reagents>(?<num>\d+) (?<name>\w+)),? ?)+ => (?<resultnum>\d+) (?<resultname>\w+)");

var reactions = lines.Select(x => rx.Match(x)).ToDictionary(x => x.Groups["resultname"].Value, x => (num: long.Parse(x.Groups["resultnum"].Value), x.Groups["reagents"].Captures.Select(x => x.Value).Select(x => x.Split(' ')).Select(x => (num: long.Parse(x[0]), name: x[1])).ToList()));

var goals = new List<(long num, string reagent)> { (2_595_245, "FUEL") };
var ore = 0L;

var inventory = reactions.ToDictionary(x => x.Key, x => 0L);
inventory["ORE"] = 0;

while (goals.Count > 0)
{
	var goal = goals[0]; goals.RemoveAt(0);

	//Console.WriteLine($"\nUsing {goal.num} of {goal.reagent} with {inventory[goal.reagent]} on hand");
	
	if (goal.reagent == "ORE") { ore += goal.num; continue; }
	
	var reaction = reactions[goal.reagent];
	var reactionCount = (long)Math.Ceiling(1.0 * (goal.num - inventory[goal.reagent]) / reaction.num);
	
	foreach (var reagent in reaction.Item2)
	{
		//Console.WriteLine($"{reactionCount} reactions will make {reactionCount * reaction.num}");
		//Console.WriteLine($"Which requires {reactionCount} * {reagent.num} of {reagent.name}");		
		goals.Add((reactionCount * reagent.num, reagent.name));
	}

	inventory[goal.reagent] += reactionCount * reaction.num - goal.num;
	Debug.Assert(inventory[goal.reagent] >= 0);

	//inventory.Dump();	
}

// 756027 is too high.
// 475983 is wrong.
ore.ToString("N0").Dump("Part 2");