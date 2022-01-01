<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

var rx = new Regex(@"Sue (\d+): (\w+): (\d+), (\w+): (\d+), (\w+): (\d+)");

var sues = lines.Select(x => rx.Match(x)).Select(x => new (string, int)[] { (x.Groups[2].Value, int.Parse(x.Groups[3].Value)), (x.Groups[4].Value, int.Parse(x.Groups[5].Value)), (x.Groups[6].Value, int.Parse(x.Groups[7].Value))});

//var facts = new (string, int)[]
//{
//	("children", 3),
//	("cats", 7),
//	("samoyeds", 2),
//	("pomeranians", 3),
//	("akitas", 0),
//	("vizslas", 0),
//	("goldfish", 5),
//	("trees", 3),
//	("cars", 2),
//	("perfumes", 1),
//};

int part = 1;

var facts = new Dictionary<string,Func<int,bool>> {
	["children"] = n => n == 3,
	["cats"] = n => part == 2 ? n > 7 : n == 7,
	["samoyeds"] = n => n == 2,
	["pomeranians"] = n => part == 2 ? n < 3 : n == 3,
	["akitas"] = n => n == 0,
	["vizslas"] = n => n == 0,
	["goldfish"] = n => part == 2 ? n < 5 : n == 5,
	["trees"] = n => part == 2 ? n > 3 : n == 3,
	["cars"] = n => n == 2,
	["perfumes"] = n => n == 1,
};

for (part = 1; part <= 2; part++)
{
	sues.Select((x, c) => (x, c + 1)).Where(x => x.x.All(x => facts[x.Item1](x.Item2))).Dump($"Part {part}");
}