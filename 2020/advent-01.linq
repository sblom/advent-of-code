<Query Kind="Statements" />

#load "..\Lib\Utils"

var nums = AoC.GetLines().Select(int.Parse).OrderBy(x => x).ToList();

foreach (var num in nums)
{
	if (nums.Contains(2020 - num))
	{
		$"{num * (2020-num)}".Dump("Part 1");
		break;
	}
}

var triples = (from x in nums from y in nums from z in nums where x + y + z == 2020 select new { x, y, z, product = x * y * z }).ToList();
triples.First().product.Dump("Part 2");