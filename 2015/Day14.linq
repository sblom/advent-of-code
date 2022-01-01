<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();
//lines = @"Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
//Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.".Split("\n").Select(x => x.Trim());

var rx = new Regex(@"(?<name>\w+) can fly (?<speed>\d+) km/s for (?<sprint>\d+) seconds, but then must rest for (?<rest>\d+) seconds.");

var reindeerData = lines.Select(x => rx.Match(x)).Select(x => (name: x.Groups["name"].Value, speed: int.Parse(x.Groups["speed"].Value), sprint: int.Parse(x.Groups["sprint"].Value), rest: int.Parse(x.Groups["rest"].Value)));

var time = 2503;

var scores = reindeerData.ToDictionary(x => x.name, x => 0);

for (int i = 1; i <= 2503; i++)
{
	var distances = from r in reindeerData
					let cycles = i / (r.sprint + r.rest)
					let remainder = i - cycles * (r.sprint + r.rest)
					let @base = cycles * r.sprint * r.speed
					let surplus = (remainder < r.sprint) ? (remainder * r.speed) : (r.speed * r.sprint)
					select (r.name, distance: @base + surplus);

	var max = distances.Max(x => x.distance);
	
	foreach (var r in distances.Where(x => x.distance == max))
	{
		scores[r.name]++;
	}
}

scores.Dump(); scores.Values.Sum().Dump();

// 647 is too low. 648 is too low.
scores.OrderByDescending(x => x.Value).First().Value.Dump("Part 2");

// 896 is too low.
var distances1 = from r in reindeerData
				let cycles = time / (r.sprint + r.rest)
				let remainder = time - cycles * (r.sprint + r.rest)
				let @base = cycles * r.sprint * r.speed
				let surplus = (remainder < r.sprint) ? (remainder * r.speed) : (r.speed * r.sprint)
				select (r.name, distance: @base + surplus);

distances1.Max(x => x.distance).Dump("Part 1");